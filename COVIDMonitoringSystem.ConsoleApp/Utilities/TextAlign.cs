using System;

namespace COVIDMonitoringSystem.ConsoleApp.Utilities
{
    public class TextAlign
    {
        private static readonly string LeftPadText = new string(' ', CHelper.LeftPadding);
        private static readonly string RightPadText = new string(' ', CHelper.RightPadding);

        public static readonly TextAlign None = new TextAlign((text) => "    " + text);
        
        public static readonly TextAlign Left = new TextAlign((text) =>
        {
            var spaceNeeded = CHelper.WritableWidth - text.Length;
            return spaceNeeded < 0
                ? text
                : FormatWithPadding($"{text}{new string(' ', spaceNeeded)}");
        });

        public static readonly TextAlign Right = new TextAlign((text) =>
        {
            var spaceNeeded = CHelper.WritableWidth - text.Length;
            return spaceNeeded < 0
                ? text
                : FormatWithPadding($"{new string(' ', spaceNeeded)}{text}");
        });

        public static readonly TextAlign Center = new TextAlign((text) =>
        {
            var spaceNeeded = CHelper.WritableWidth - text.Length;
            if (spaceNeeded < 0)
            {
                return text;
            }

            var left = spaceNeeded / 2;
            
            return FormatWithPadding($"{new string(' ', left)}{text}{new string(' ', spaceNeeded - left)}");
        });

        private static string FormatWithPadding(string text)
        {
            return $"{LeftPadText}{text}{RightPadText}";
        }
        
        public Func<string, string> DoAlignment { get; }

        private TextAlign(Func<string, string> doAlignment)
        {
            DoAlignment = doAlignment;
        }
    }
}