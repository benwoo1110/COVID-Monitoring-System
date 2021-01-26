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

        public ObjectList(string name) : base(name)
        {
        }

        public ObjectList(string name, string[] propertyToInclude, Func<List<T>> listGetter) : base(name)
        {
            PropertyToInclude = propertyToInclude;
            ListGetter = listGetter;
        }

        protected override void WriteToScreen()
        {
            var objList = ListGetter?.Invoke();

            if (objList == null)
            {
                Console.WriteLine("NULL");
                return;
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

            CHelper.WriteLine(headerBuilder.ToString());
            CHelper.FillLine('-');

            foreach (var propertyValues in valuesArray)
            {
                var contentBuilder = new StringBuilder();
                for (var index = 0; index < columnWidths.Length; index++)
                {
                    contentBuilder.Append(string.Format($"{{0,-{columnWidths[index] + ColumnGap}}}", propertyValues[PropertyToInclude[index]]));
                }
                CHelper.WriteLine(contentBuilder.ToString());
                CHelper.WriteEmpty();
            }
        }

        private static string SplitPascalCase(string text)
        {
            return Regex.Replace(text, @"(?<=[A-Za-z])(?=[A-Z][a-z])|(?<=[a-z0-9])(?=[0-9]?[A-Z])", " ");
        }
    }
}