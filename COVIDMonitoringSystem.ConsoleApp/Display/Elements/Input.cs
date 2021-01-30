//============================================================
// Student Number : S10203296, S10205301
// Student Name   : Benedict Woo, Melvin Kee
// Module Group   : T06
//============================================================

using System;
using COVIDMonitoringSystem.ConsoleApp.Utilities;
using COVIDMonitoringSystem.Core.Utilities;

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

        public override bool ClearOnExit { get; set; } = true;

        public ActionMethod MethodRunner { get; set; }

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
            
            BoundingBox.CursorLeft = CoreHelper.GetStringLength(Prompt) + CoreHelper.GetStringLength(Text) + 6;
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