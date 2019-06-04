using OpenTK;
using System;
using System.Collections.Generic;
using System.IO;

namespace VoxelRPG.Engine.Diagnosatics
{
    public static class Debug
    {
        private static readonly ConsoleColor defaultColor = ConsoleColor.White;

        public static void Init() { }

        public static void LogInfo(object message) => Log(message.ToString(), ConsoleColor.White);

        public static void LogWaring(object message) => Log(message.ToString(), ConsoleColor.Yellow);

        public static void LogError(string message) => Log(message, ConsoleColor.Red);

        public static void LogError(Exception ex) => Log(ex.ToString(), ConsoleColor.Red);

        private static void Log(string message, ConsoleColor color)
        {
            Console.ForegroundColor = color;
            Console.WriteLine("[{0}]: {1}", DateTime.Now.ToString("H:mm:ss:fff"), message);
            Console.ForegroundColor = defaultColor;
        }

        public static class CSV
        {
            static Dictionary<string, string> csvData = new Dictionary<string, string>();

            public static void Start(string name, string[] columns)
            {
                LogInfo(string.Format("Starting CSV log {0}", name));
                csvData.Add(name, string.Join(";", columns));
            }

            public static void Add(string name, string[] values)
            {
                if (csvData.ContainsKey(name))
                    csvData[name] = string.Format("{0}\n{1}", csvData[name], string.Join(";", values));
            }

            public static void End(string name)
            {
                LogInfo(string.Format("Finalizing CSV log {0}", name));

                Directory.CreateDirectory("Log");

                if (csvData.ContainsKey(name))
                    File.WriteAllText("Log/" + name + ".csv", csvData[name]);
            }
        }
    }
}
