//============================================================
// Student Number : S10203296, S10205301
// Student Name   : Benedict Woo, Melvin Kee
// Module Group   : T06
//============================================================

using System;
using System.Collections.Generic;
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
                UpdateSuggestion();
            }
        }

        public override bool ClearOnExit { get; set; } = true;
        public ActionMethod MethodRunner { get; set; }
        public Action OnEnterRunner { get; set; }
        public string SuggestionType { get; set; }
        public List<string> CachedSuggestions { get; set; }
        public int SuggestionIndex { get; set; }

        public Input(string name = null) : base(name)
        {
        }

        public Input(string name, string prompt) : base(name)
        {
            Prompt = prompt;
        }

        protected override int WriteToScreen()
        {
            if (!Selected || !HasSuggestions())
            {
                return CHelper.WriteLine($"{Prompt}: {Text}", Align);
            }
            
            return CHelper.WriteLine($"{Prompt}: {Text} , {CachedSuggestions[SuggestionIndex]}", Align);
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

        public void ApplySuggestion()
        {
            if (!HasSuggestions())
            {
                return;
            }

            Text = CachedSuggestions[SuggestionIndex];
        }
        
        public void NextSuggestion()
        {
            if (!HasSuggestions())
            {
                return;
            }
            
            SuggestionIndex = CoreHelper.Mod(++SuggestionIndex, CachedSuggestions.Count);
            Render();
        }
        
        protected void UpdateSuggestion()
        {
            if (SuggestionType == null)
            {
                return;
            }
            
            CachedSuggestions = TargetScreen.DisplayManager.ValuesManager.GetSuggestion(SuggestionType, TargetScreen, Text);
            SuggestionIndex = 0;
        }

        private bool HasSuggestions()
        {
            return CachedSuggestions != null && CachedSuggestions.Count > 0;
        }
    }
}