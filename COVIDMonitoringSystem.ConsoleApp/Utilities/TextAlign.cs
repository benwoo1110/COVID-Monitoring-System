using System;

namespace COVIDMonitoringSystem.ConsoleApp.Utilities
{
    public class TextAlign
    {
        public static readonly TextAlign None = new TextAlign((text) => "    " + text);
        
        public static readonly TextAlign Left = new TextAlign((text) =>
        {
            var spaceNeeded = CHelper.WindowWidth - text.Length - 4;
            return spaceNeeded <= 0
                ? text
                : "    " + text + new string(' ', spaceNeeded);
        });

        public static readonly TextAlign Right = new TextAlign((text) =>
        {
            var spaceNeeded = CHelper.WindowWidth - text.Length - 4;
            return spaceNeeded <= 0
                ? text
                : new string(' ', spaceNeeded) + text + "    ";
        });

        public static readonly TextAlign Center = new TextAlign((text) =>
        {
            var spaceNeeded = CHelper.WindowWidth - text.Length;
            if (spaceNeeded <= 0)
            {
                return text;
            }

            var left = spaceNeeded / 2;
            return $"{new string(' ', left)}{text}{new string(' ', spaceNeeded - left)}";
        });
        
        public Func<string, string> DoAlignment { get; }

        private TextAlign(Func<string, string> doAlignment)
        {
            DoAlignment = doAlignment;
        }
    }
}