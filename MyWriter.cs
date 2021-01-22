using InfinityScript;
using System;
using System.IO;
using static InfinityScript.Function;

namespace AntiAFK
{
    public class MyWriter
    {
        private readonly string currentPath;
        private readonly string dirPath;

        public MyWriter()
        {
            CreateDirectory();
            dirPath = GetDirPath();
            currentPath = Directory.GetCurrentDirectory() + $"\\{dirPath}\\Permanent_GUID.ban";
        }

        public void Info(string format, params object[] args)
        {
            if (!File.Exists(currentPath))
            {
                // Create a file to write to.
                using (StreamWriter sw = File.CreateText(currentPath))
                {
                    sw.WriteLine("[GUIDBans]");
                }
            }

            using (StreamWriter sw = File.AppendText(currentPath))
            {
                sw.WriteLine(string.Format(format, args));
                sw.Flush();
                sw.Close();
            }
        }

        public string GetDirPath()
        {
            string result = Call<string>("GetDvar", "sv_path_for_ban_dbs");
            if (string.IsNullOrWhiteSpace(result))
            {
                Log.Write(LogLevel.Warning, "\"sv_path_for_ban_dbs\" dvar is null or contains white spaces. What did you do? Modify the server.cfg to fix this!");
                result = "BanDB";
            }

            return result;
        }

        private void CreateDirectory()
        {
            try
            {
                DirectoryInfo dir = Directory.CreateDirectory(Directory.GetCurrentDirectory() + $"\\{dirPath}");
            }

            catch (Exception e)
            {
                Console.WriteLine("The process failed: {0}", e.ToString());
            }
        }
    }
}
