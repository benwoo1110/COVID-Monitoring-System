using System;
using COVIDMonitoringSystem.ConsoleApp.Display;
using COVIDMonitoringSystem.ConsoleApp.Display.Elements;

namespace COVIDMonitoringSystem.ConsoleApp.Utilities
{
    public class Box
    {
        public int Top { get; set; }
        public int CursorLeft { get; set; }
        public int Height { get; set; } = 1;
        public Box RelativeBox { get; private set; }
        public bool AutoHeight { get; set; } = true;

        public void SetRelativeBox(Element element)
        {
            SetRelativeBox(element.BoundingBox);
        }

        public void SetRelativeBox(Box box)
        {
            if (box == this)
            {
                throw new ArgumentException("You cant set relative bounding box itself.");
            }
            
            RelativeBox = box;
        }

        public int GetBottom()
        {
            var totalBottom = Top + Height;
            if (RelativeBox == null)
            {
                return totalBottom;
            }

            return totalBottom + RelativeBox.GetBottom();
        }

        public int GetTop()
        {
            var totalTop = Top;
            if (RelativeBox == null)
            {
                return Top;
            }

            return totalTop + RelativeBox.GetBottom();
        }

        public void SetDrawPosition()
        {
            Console.SetCursorPosition(0, GetTop());
        }

        public void SetCursorPosition()
        {
            Console.SetCursorPosition(CursorLeft, GetTop());
        }
    }
}