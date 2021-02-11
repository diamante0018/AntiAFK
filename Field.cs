using InfinityScript;
using System.Collections.Generic;

namespace AntiAFK
{
    public static class Field
    {
        private static Dictionary<string, Dictionary<string, Parameter>> fields = new Dictionary<string, Dictionary<string, Parameter>>();

        public static bool MyHasField(this Entity player, string field)
        {
            if (!player.IsPlayer)
                return false;
            if (fields.ContainsKey(player.HWID))
                return fields[player.HWID].ContainsKey(field);
            return false;
        }

        public static void MySetField(this Entity player, string field, Parameter value)
        {
            if (!player.IsPlayer)
                return;
            if (!fields.ContainsKey(player.HWID))
                fields.Add(player.HWID, new Dictionary<string, Parameter>());

            if (!MyHasField(player, field))
                fields[player.HWID].Add(field, value);
            else
                fields[player.HWID][field] = value;
        }

        public static Parameter MyGetField(this Entity player, string field)
        {
            if (!player.IsPlayer)
                return new Parameter(int.MinValue);
            if (!MyHasField(player, field))
                return new Parameter(int.MinValue);
            return fields[player.HWID][field];
        }

        public static void MyRemoveField(this Entity player) => fields.Remove(player.HWID);
    }
}