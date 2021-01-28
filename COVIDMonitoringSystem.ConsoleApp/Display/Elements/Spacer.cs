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

        public override void Render()
        {
            ColourSelector.Element();
            Console.SetCursorPosition(0, BoundingBox.Top);
            CHelper.WriteEmpty();
        }

        protected override void WriteToScreen()
        {
            throw new NotImplementedException();
        }
    }
}