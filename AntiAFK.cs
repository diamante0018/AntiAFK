using InfinityScript;
using System;
using System.Collections.Generic;

namespace AntiAFK
{
    public class AntiAFK : BaseScript
    {
        private int banHitCount;
        private HashSet<string> quitList = new HashSet<string>();
        private MyWriter writeBan = new MyWriter();

        public AntiAFK()
        {
            PlayerConnected += OnPlayerConnect;
            Call("SetDvarIfUninitialized", "sv_banTick", "24");
            Call("SetDvar", "sv_kickBanTime", 3600f);
            Call("SetDvarIfUninitialized", "sv_ban_by_ip", true);
            banHitCount = Call<int>("GetDvarInt", "sv_banTick");

            if (banHitCount < 24 || banHitCount > 50)
                banHitCount = 24;
        }

        public void OnPlayerConnect(Entity player)
        {
            Vector3 oldPos = player.Origin;
            int hitCount = 0;
            if (quitList.Remove(player.HWID))
                Log.Write(LogLevel.Info, $"{player.Name} Joined back after quitting within the allotted time frame. He will not be banned.");

            player.OnInterval(5000, delegate (Entity ent)
            {
                if (ent.GetField<string>("sessionteam") == "axis")
                {
                    Vector3 newPos = player.Origin;

                    if (oldPos.DistanceTo2D(player.Origin) < 450f)
                    {
                        hitCount++;
                        if (hitCount % 5 == 0)
                            PushFoward(ent);

                        if (hitCount >= banHitCount)
                            TempBan(ent);
                    }

                    else
                        hitCount = 0;

                    oldPos = player.Origin;
                }

                return true;
            });
        }

        public void TempBan(Entity player) => Utilities.ExecuteCommand($"tempbanclient {player.EntRef} You have ^1Rage ^0Quitted ^7Stay Banned for 1 Hour");

        public void TempBan(string playerName) => Utilities.ExecuteCommand($"tempban {playerName} You have ^1Rage ^0Quitted ^7Stay Banned for 1 Hour");

        public void PushFoward(Entity player)
        {
            player.Call("IPrintLnBold", "^2Run ^7or get ^1Banned!");
            PlayLeaderDialog(player, "pushforward");
            int oldHealth = player.Health;
            player.Health /= 3;
            player.Notify("damage", (oldHealth - player.Health), player, new Vector3(0, 0, 0), new Vector3(0, 0, 0), "MOD_EXPLOSIVE", "", "", "", 0, "frag_grenade_mp");
        }

        /// <summary>
        /// Play leader dialog for player
        /// </summary>
        /// <param name="player">Player</param>
        /// <param name="sound">Sound</param>
        public void PlayLeaderDialog(Entity player, string sound)
        {
            if (player.GetField<string>("sessionteam") == "allies")
                player.Call("PlayLocalSound", GetTeamVoicePrefix(Call<string>("GetMapCustom", "allieschar")) + "1mc_" + sound);
            else
                player.Call("PlayLocalSound", GetTeamVoicePrefix(Call<string>("GetMapCustom", "axischar")) + "1mc_" + sound);
        }

        public string GetTeamVoicePrefix(string teamRef) => Call<string>("TableLookup", "mp/factionTable.csv", 0, teamRef, 7);

        public long EpochTime()
        {
            TimeSpan t = DateTime.UtcNow - new DateTime(1970, 1, 1);
            long secondsSinceEpoch = Convert.ToInt64(t.TotalSeconds);
            return secondsSinceEpoch;
        }

        public override void OnPlayerDisconnect(Entity player)
        {
            if (player.GetField<string>("sessionteam") == "axis")
            {
                string entName = player.Name;
                string hwid = player.HWID;
                quitList.Add(hwid);

                Utilities.RawSayAll($"{entName} ^1Left the server^7. Report them to an Admin for rage quitting.");
                Log.Write(LogLevel.Info, $"{entName} ^1Left the server^7. Report them to an Admin for rage quitting.");

                AfterDelay(1000 * 180, () =>
                {
                    if (quitList.Contains(hwid))
                    {
                        Log.Write(LogLevel.Info, $"Attempting to ban {entName} for rage quitting.");
                        writeBan.Info("{0}=TEMP({1})", hwid, EpochTime().ToString("X").ToUpperInvariant());
                    }
                });
            }
        }
    }
}