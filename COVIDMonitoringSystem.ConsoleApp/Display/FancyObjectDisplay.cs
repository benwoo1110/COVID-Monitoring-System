using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using COVIDMonitoringSystem.ConsoleApp.Utilities;

namespace COVIDMonitoringSystem.ConsoleApp.Display
{
    public static class FancyObjectDisplay
    {
        public static void PrintList<T>(List<T> objList)
        {
            var properties = typeof(T).GetProperties();
            var headings = new string[properties.Length];

            var index = 0;
            foreach (var property in properties)
            {
                headings[index] = SplitPascalCase(property.Name);
                index++;
            }
            
            foreach (var heading in headings)
            {
                Console.Write($"{heading,-24}");
            }
            ConsoleHelper.EmptyLine();
            
            foreach (var item in objList)
            {
                foreach (var property in properties)
                {
                    Console.Write($"{property.GetValue(item),-24}");
                }

                ConsoleHelper.EmptyLine();
            }
        }

        private static string SplitPascalCase(string text)
        {
            return Regex.Replace(text, @"(?<=[A-Za-z])(?=[A-Z][a-z])|(?<=[a-z0-9])(?=[0-9]?[A-Z])", " ");
        }
    }
}