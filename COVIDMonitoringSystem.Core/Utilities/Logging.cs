using System;
using JetBrains.Annotations;

namespace COVIDMonitoringSystem.Core.Utilities
{
    public static class Logging
    {
        private static LogLevel CurrentLevel { get; set; } = LogLevel.Info;

        public static void SetLogLevel(LogLevel newLevel)
        {
            CurrentLevel = newLevel;
        }

        public static void Error([CanBeNull] string message)
        {
            Log(LogLevel.Error, message);
        }

        public static void Info([CanBeNull] string message)
        {
            Log(LogLevel.Info, message);
        }
        
        public static void Debug([CanBeNull] string message)
        {
            Log(LogLevel.Debug, message);
        }
        
        public static void DebugObject([CanBeNull] object o) 
        {
            if (!ShouldLogAtLevel(LogLevel.Debug))
            {
                return;
            }

            DoLog(LogLevel.Debug, CoreHelper.GetObjectData(o));
        }

        public static void Log(LogLevel level, [CanBeNull] string message)
        {
            if (!ShouldLogAtLevel(level))
            {
                return;
            }

            DoLog(level, message);
        }

        private static bool ShouldLogAtLevel(LogLevel level)
        {
            return CurrentLevel != LogLevel.None && level >= CurrentLevel;
        }

        private static void DoLog(LogLevel level, [CanBeNull] string message)
        {
            if (message == null)
            {
                LogSingleLine(level, null);
                return;
            }

            var messageLines = message.Split("\n");
            foreach (var line in messageLines)
            {
                LogSingleLine(level, line);
            }
        }

        private static void LogSingleLine(LogLevel level, [CanBeNull] string line)
        {            
            Console.WriteLine($"[{level}] {line ?? "NULL"}");
        }
    }
}