﻿using System;

namespace COVIDMonitoringSystem.ConsoleApp.Utilities
{
    public static class ConsoleHelper
    {
        
        
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