﻿using COVIDMonitoringSystem.ConsoleApp.Utilities;

namespace COVIDMonitoringSystem.ConsoleApp.Display.Elements
{
    public abstract class TextElement : Element
    {
        protected string text = "";
        private TextAlign _align = TextAlign.Left;

        public virtual string Text
        {
            get => text;
            set
            {
                text = value;
                QueueToRerender();
            }
        }

        public virtual TextAlign Align
        {
            get => _align;
            set
            {
                _align = value;
                QueueToRerender();
            }
        }

        protected TextElement(string name) : base(name)
        {
        }

        protected TextElement(string name, string text) : base(name)
        {
            Text = text;
        }

        protected override int WriteToScreen()
        {
           return CHelper.WriteLine(Text, Align);
        }

        public bool HasText(bool allowWhiteSpace = false)
        {
            return allowWhiteSpace 
                ? !string.IsNullOrEmpty(Text) 
                : !string.IsNullOrWhiteSpace(Text);
        }

        public void ClearText()
        {
            Text = "";
        }
    }
}