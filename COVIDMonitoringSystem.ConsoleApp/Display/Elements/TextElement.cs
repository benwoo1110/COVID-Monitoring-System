﻿using System;
using COVIDMonitoringSystem.ConsoleApp.Utilities;

namespace COVIDMonitoringSystem.ConsoleApp.Display.Elements
{
    public abstract class TextElement : Element
    {
        public string Text { get; set; } = "";

        protected TextElement()
        {
        }

        public override void Display()
        {
            CHelper.WriteLine(Text);
        }

        public virtual void Clear()
        {
            Text = "";
        }
    }
}