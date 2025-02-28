using System;
using System.Collections.Generic;
using System.Diagnostics.Meyer.Contracts;
using System.Reflection;

namespace ChessAnalysis
{
    public class ChessConsole
    {
        // Singleton instance
        private static readonly Lazy<ChessConsole> instance = new Lazy<ChessConsole>(() => new ChessConsole());
        public static ChessConsole Instance => instance.Value;

        // Thread safety lock
        private static readonly object _lock = new object();

        // Color toggle
        private static volatile bool _useColor = true;
        public static bool UseColor
        {
            get { lock (_lock) { return _useColor; } }
            set { lock (_lock) { _useColor = value; } }
        }

        // Log Levels and corresponding colors
        private static readonly Dictionary<string, ConsoleColor> _logColors = new()
        {
            { "INFO", ConsoleColor.White },
            { "STATUS", ConsoleColor.Green },
            { "MOVE", ConsoleColor.Yellow },
            { "DEBUG", ConsoleColor.Cyan },
            { "ERROR", ConsoleColor.Red },
            { "HEADER", ConsoleColor.Magenta }
        };

        // Constructor
        public ChessConsole() { }

        // Timestamp generator
        private string GetTimeStamp()
        {
            return DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss.fff");
        }

        // Badge generator
        private string Badge()
        {
            var appName = System.AppDomain.CurrentDomain.FriendlyName;
            var version = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;
            return $"{appName} v{version}";
        }

        // Badge Output
        public void OutputBadgeToConsole()
        {
            Info(Badge());
        }

        // Core Message Writer
        private void WriteMessage(string level, string message, bool useErrorStream)
        {
            lock (_lock)
            {
                // Set color if enabled and valid level
                if (UseColor && _logColors.ContainsKey(level))
                {
                    Console.ForegroundColor = _logColors[level];
                }

                // Format and output message
                string timestamp = GetTimeStamp();
                string output = $"[{level}] {timestamp} {message}";

                if (useErrorStream)
                {
                    Console.Error.WriteLine(output);
                    Console.Error.Flush();
                }
                else
                {
                    Console.WriteLine(output);
                    Console.Out.Flush();
                }

                // Reset color
                if (UseColor)
                {
                    Console.ResetColor();
                }
            }
        }

        // Public Methods for Different Categories
        public void Info(string message) => WriteMessage("INFO", message, useErrorStream: false);
        public void Status(string message) => WriteMessage("STATUS", message, useErrorStream: false);
        public void Move(string message) => WriteMessage("MOVE", message, useErrorStream: false);
        public void Debug(string message) => WriteMessage("DEBUG", message, useErrorStream: false);
        public void Error(string message) => WriteMessage("ERROR", message, useErrorStream: true);
        public void Header(string message) => WriteMessage("HEADER", message, useErrorStream: false);

        // No Prefix, No Color
        public void OutputToConsole(string message)
        {
            lock (_lock)
            {
                string timestamp = GetTimeStamp();
                string output = $"{timestamp} {message}";

                Console.WriteLine(output);
                Console.Out.Flush();
            }
        }

        // Raw Output (No Colors, No Prefix)
        public void LogRaw(string message)
        {
            lock (_lock)
            {
                Console.WriteLine(message);
                Console.Out.Flush();
            }
        }
    }
}
