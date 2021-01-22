using System;
using COVIDMonitoringSystem.Core.Utilities;

namespace COVIDMonitoringSystem.ConsoleApp.Utilities
{
    public static class ConsoleHelper
    {
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

        public static void PadHeight()
        {
            PadHeight(Console.WindowHeight);
        }

        public static void PadHeight(int height)
        {
            for (var i = 0; i < height; i++)
            {
                EmptyLine();
            }
        }
        
        public static void WriteSeparator(int length)
        {
            Console.WriteLine($"+{new string('-', length - 2)}+");
        }

        public static void WriteSubSeparator(int length)
        {
            Console.WriteLine($"|{new string('-', length - 2)}|");
        }

        public static void WriteWithPipe(string text)
        {
            Console.WriteLine($"| {text} |");
        }

        public static void EmptyLine()
        {
            Console.WriteLine();
        }

        public static void ExitProgram()
        {
            Console.WriteLine("Exiting...");
            Environment.Exit(0);
        }

        public static string CenterText(string text, int width)
        {
            var spaceNeeded = width - text.Length;
            if (spaceNeeded <= 0)
            {
                return text;
            }
            
            var left = spaceNeeded / 2;
            return $"{new string(' ', left)}{text}{new string(' ', spaceNeeded - left)}";
        }

        public static string LeftText(string text, int width)
        {
            var spaceNeeded = width - text.Length;
            return spaceNeeded <= 0 
                ? text 
                : text + new string(' ', spaceNeeded);
        }
    }
}