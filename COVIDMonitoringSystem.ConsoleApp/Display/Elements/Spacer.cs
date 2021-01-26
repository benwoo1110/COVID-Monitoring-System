using System;
using COVIDMonitoringSystem.ConsoleApp.Utilities;

namespace COVIDMonitoringSystem.ConsoleApp.Display.Elements
{
    public class Spacer : Element
    {
        public Spacer()
        {
            Name = "spacer";
        }

        public override void Display()
        {
            ColourSelector.Element();
            Console.SetCursorPosition(0, BoundingBox.Top);
            CHelper.WriteEmpty();
        }
    }
}