using System;

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
    }
}
