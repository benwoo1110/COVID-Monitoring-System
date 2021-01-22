using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using COVIDMonitoringSystem.ConsoleApp.Utilities;
using COVIDMonitoringSystem.Core.Utilities;

namespace COVIDMonitoringSystem.ConsoleApp.Display
{
    public static class FancyObjectDisplay
    {
        private const int ColumnGap = 4;

        public static void PrintList<T>(ICollection<T> objList, string[] propertyToInclude) where T : class
        {
            var columnWidths = new int[propertyToInclude.Length];

            var headerItems = new string[propertyToInclude.Length];
            for (var i = 0; i < propertyToInclude.Length; i++)
            {
                headerItems[i] = SplitPascalCase(propertyToInclude[i]);
            }

            for (var i = 0; i < headerItems.Length; i++)
            {
                columnWidths[i] = headerItems[i].Length;
            }

            var valuesArray = new Dictionary<string, string>[objList.Count];
            var x = 0;
            foreach (var item in objList)
            {
                var propertyValues = ReflectHelper.GetAllPropertyValues(item);
                for (var index = 0; index < propertyToInclude.Length; index++)
                {
                    var textLength = propertyValues[propertyToInclude[index]].Length;
                    if (textLength > columnWidths[index])
                    {
                        columnWidths[index] = textLength;
                    }
                }

                valuesArray[x++] = propertyValues;
            }

            for (var index = 0; index < columnWidths.Length; index++)
            {
                var format = $"{{0,-{columnWidths[index] + ColumnGap}}}";
                Console.Write(format, headerItems[index]);
            }

            ConsoleHelper.EmptyLine();

            Console.WriteLine(new string('-', columnWidths.Sum() + (columnWidths.Length - 1) * ColumnGap));

            foreach (var propertyValues in valuesArray)
            {
                for (var index = 0; index < columnWidths.Length; index++)
                {
                    var format = $"{{0,-{columnWidths[index] + ColumnGap}}}";
                    Console.Write(format, propertyValues[propertyToInclude[index]]);
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