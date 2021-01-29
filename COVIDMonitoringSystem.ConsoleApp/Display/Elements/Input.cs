﻿using System;
using COVIDMonitoringSystem.ConsoleApp.Utilities;

namespace COVIDMonitoringSystem.ConsoleApp.Display.Elements
{
    public class Input : SelectableElement
    {
        private string prompt;

        public string Prompt
        {
            get => prompt;
            set
            {
                prompt = value;
                UpdateCursor();
                QueueToRerender();
            }
        }

        public override string Text
        {
            get => text;
            set
            {
                text = value;
                UpdateCursor();
                QueueToRerender();
            }
        }

        public ButtonMethod MethodRunner { get; set; }

        public Action OnEnterRunner { get; set; }

        public Input(string name) : base(name)
        {
        }

        public Input(string name, string prompt) : base(name)
        {
            Prompt = prompt;
        }

        protected override int WriteToScreen()
        {
            return CHelper.WriteLine($"{Prompt}: {Text}", Align);
        }

        public void UpdateCursor()
        {
            //TODO: Improve this
            BoundingBox.CursorLeft = Prompt.Length + Text.Length + 6;
        }

        public void Run()
        {
            if (MethodRunner != null)
            {
                MethodRunner.Run(TargetScreen);
                return;
            }

            OnEnterRunner?.Invoke();
        }

        protected override void OnDeselect()
        {
            if (TargetScreen.Active)
            {
                Run();
            }
        }
    }
}