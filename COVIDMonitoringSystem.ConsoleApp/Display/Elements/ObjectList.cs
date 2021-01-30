//============================================================
// Student Number : S10203296, S10205301
// Student Name   : Benedict Woo, Melvin Kee
// Module Group   : T06
//============================================================

using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using COVIDMonitoringSystem.ConsoleApp.Utilities;
using COVIDMonitoringSystem.Core.Utilities;

namespace COVIDMonitoringSystem.ConsoleApp.Display.Elements
{
    public class ObjectList<T> : Element where T : class
    {
        private const int ColumnGap = 4;

        public string[] PropertyToInclude { get; set; }
        public Func<List<T>> ListGetter { get; set; }

        public ObjectList(string name = null) : base(name)
        {
        }

        protected override int WriteToScreen()
        {
            var objList = ListGetter?.Invoke();

            if (objList == null)
            {
                CHelper.WriteLine("NULL");
                return 1;
            }

            var columnWidths = new int[PropertyToInclude.Length];

            var headerItems = new string[PropertyToInclude.Length];
            for (var i = 0; i < PropertyToInclude.Length; i++)
            {
                headerItems[i] = SplitPascalCase(PropertyToInclude[i]);
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
                for (var index = 0; index < PropertyToInclude.Length; index++)
                {
                    var textLength = propertyValues[PropertyToInclude[index]].Length;
                    if (textLength > columnWidths[index])
                    {
                        columnWidths[index] = textLength;
                    }
                }

                valuesArray[x++] = propertyValues;
            }

            var headerBuilder = new StringBuilder();
            for (var index = 0; index < columnWidths.Length; index++)
            {
                var format = $"{{0,-{columnWidths[index] + ColumnGap}}}";
                headerBuilder.Append(string.Format(format, headerItems[index]));
            }

            ColourSelector.Selected();
            CHelper.WriteEmpty();
            CHelper.WriteLine(headerBuilder.ToString());
            ColourSelector.Default();
            CHelper.WriteEmpty();

            foreach (var propertyValues in valuesArray)
            {
                var contentBuilder = new StringBuilder();
                for (var index = 0; index < columnWidths.Length; index++)
                {
                    contentBuilder.Append(string.Format($"{{0,-{columnWidths[index] + ColumnGap}}}",
                        propertyValues[PropertyToInclude[index]]));
                }

                CHelper.WriteLine(contentBuilder.ToString());
                CHelper.WriteEmpty();
            }

            //TODO: Use builder and return correct lines written
            return 1;
        }

        private static string SplitPascalCase(string text)
        {
            return Regex.Replace(text, @"(?<=[A-Za-z])(?=[A-Z][a-z])|(?<=[a-z0-9])(?=[0-9]?[A-Z])", " ");
        }
    }
}