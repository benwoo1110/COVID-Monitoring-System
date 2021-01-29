using System;
using System.Collections.Generic;
using System.Linq;
using COVIDMonitoringSystem.Core.Utilities;

namespace COVIDMonitoringSystem.ConsoleApp.Utilities
{
    public static class CHelper
    {
        public const int LeftPadding = 4;
        public const int RightPadding = 4;

        private static int cachedWidth;
        private static int cachedHeight;
        
        public static int WritableWidth => Console.WindowWidth - LeftPadding - RightPadding;
        public static int WindowWidth => Console.WindowWidth;
        public static int WindowHeight => Console.WindowHeight - 1;
        
        public static bool DidChangeWindowSize()
        {
            if (cachedWidth == Console.WindowWidth && cachedHeight == Console.WindowHeight)
            {
                return false;
            }
            
            cachedWidth = Console.WindowWidth;
            cachedHeight = Console.WindowHeight;

            Clear();
            Console.SetBufferSize(Console.WindowWidth, Console.WindowHeight);
            
            return true;
        }

        public static void WriteLine(string text)
        {
            WriteLine(text, TextAlign.Left);
        }
        
        public static int WriteLine(string text, TextAlign textAlign)
        {
            var linesWritten = 0;
            var lines = text.Split("\n");
            
            foreach (var line in lines)
            {
                foreach (var chunkLine in SplitByLength(line))
                {
                    Console.Write(textAlign.DoAlignment(chunkLine));
                    linesWritten++;
                }
            }

            return linesWritten;
        }

        private static IEnumerable<string> SplitByLength(string line)
        {
            if (line.Length == 0)
            {
                yield return line;
            }
            
            for (var i = 0; i < line.Length; i += WritableWidth)
            {
                yield return line.Substring(i, Math.Min(WritableWidth, line.Length - i));
            }
        }

        public static void FillLine(char c)
        {
            Console.Write(new string(c, WindowWidth));
        }

        public static void WriteEmpty()
        {
            FillLine(' ');
        }

        public static void Clear()
        {
            Console.Clear();
            Console.SetCursorPosition(0, 0);
        }

        public static TE EnumParser<TE>(string value) where TE : struct
        {
            try
            {
                return CoreHelper.ParseEnum<TE>(value);
            }
            catch (ArgumentException)
            {
                throw new InputParseFailedException($"No such {typeof(TE).Name}. Available values are: {string.Join(", ", Enum.GetNames(typeof(TE)))}");
            }
        }

        public static string GetInput(string prompt)
        {
            return GetInput(prompt, Convert.ToString);
        }

        public static T GetInput<T>(string prompt, Func<string, T> parser)
        {
            while (true)
            {
                WriteLine(prompt, TextAlign.None);
                try
                {
                    return parser(Console.ReadLine());
                }
                catch (InputParseFailedException e)
                {
                    WriteLine(e.Message);
                }
                catch (Exception)
                {
                    WriteLine("Input is in invalid format. Please try again!");
                }
            }
        }

        public static void Pause(string message)
        {
            GetInput(message);
        }

        public static bool Confirm(string prompt)
        {
            return GetInput($"{prompt} [y/n]: ", input =>
            {
                if (input == null)
                {
                    throw new ArgumentException();
                }

                return input.ToLower().Equals("y");
            });
        }
    }
}