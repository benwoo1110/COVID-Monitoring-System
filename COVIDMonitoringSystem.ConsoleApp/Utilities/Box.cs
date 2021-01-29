using System;
using System.Collections.Generic;
using COVIDMonitoringSystem.ConsoleApp.Display.Elements;

namespace COVIDMonitoringSystem.ConsoleApp.Utilities
{
    public class Box
    {
        private int height = 1;
        
        public Element TargetElement { get; }
        public int Top { get; set; }
        public int CursorLeft { get; set; }
        public int Height
        {
            get => height;
            set
            {
                if (height != value)
                {
                    height = value;
                    RerenderDependents();
                }
            }
        }

        public Box RelativeBox { get; private set; }
        public bool AutoHeight { get; set; } = true;
        public List<Element> Dependents { get; }

        public Box(Element targetElement)
        {
            TargetElement = targetElement;
            Dependents = new List<Element>();
        }

        public void SetRelativeBox(Element element)
        {
            if (element.BoundingBox == this)
            {
                throw new ArgumentException("You cant set relative bounding box itself.");
            }

            RelativeBox = element.BoundingBox;
            element.BoundingBox.Dependents.Add(TargetElement);
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

        public void UpdateHeight(int to)
        {
            if (!AutoHeight)
            {
                return;
            }

            Height = to;
        }

        private void RerenderDependents()
        {
            //TODO: Shouldnt re-render everything, but works for now
            if (Dependents.Count != 0)
            {
                TargetElement.TargetScreen.Render();
            }
        }
    }
}