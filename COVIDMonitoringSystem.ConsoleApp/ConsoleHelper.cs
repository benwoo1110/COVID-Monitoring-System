using System;

namespace Cryptography.ConsoleApp
{
    public class ConsoleHelper
    {
        public static string GetInput(string prompt)
        {
            return GetInput(prompt, Convert.ToString);
        }
        
        public static T GetInput<T>(string prompt, Func<String, T> converter)
        {
            while (true)
            {
                Console.Write(prompt);
                try
                {
                    return converter(Console.ReadLine());
                }
                catch (Exception)
                {
                    Console.WriteLine($"Input is not a {typeof(T).Name}, please try again!");
                }
            }
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

        public static void EmptyLine()
        {
            Console.WriteLine();
        }

        public static void ExitProgram()
        {
            Console.WriteLine("Exiting...");
            Environment.Exit(0);
        }
    }
}