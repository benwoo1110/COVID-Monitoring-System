using System;
using COVIDMonitoringSystem.Core.Utilities;

namespace COVIDMonitoringSystem.ConsoleApp.Utilities
{
    public static class CHelper
    {
        public static int WindowWidth => Console.WindowWidth;
        public static int WindowHeight => Console.WindowHeight - 1;
        public static int LinesPrinted { get; set; }

        private static int cachedWidth;
        private static int cachedHeight;
        
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
        
        public static void WriteLine(string text, TextAlign textAlign)
        {
            Console.Write(textAlign.DoAlignment($"    {text}"));
            LinesPrinted++;
            //TODO: What if its multi-lined ;(
        }

        public static void FillLine(char c)
        {
            Console.Write(new string(c, WindowWidth));
            LinesPrinted++;
        }

        public static void WriteEmpty()
        {
            FillLine(' ');
        }

        public static void PadRemainingHeight()
        {
            int paddingLinesToWrite = WindowHeight - LinesPrinted;
            for (var i = 0; i < paddingLinesToWrite; i++)
            {
                WriteEmpty();
            }
        }

        public static void Clear()
        {
            Console.SetCursorPosition(0, 0);
            LinesPrinted = 0;
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
                Console.Write(prompt);
                try
                {
                    return parser(Console.ReadLine());
                }
                catch (InputParseFailedException e)
                {
                    Console.WriteLine(e.Message);
                }
                catch (Exception)
                {
                    Console.WriteLine("Input is in invalid format. Please try again!");
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