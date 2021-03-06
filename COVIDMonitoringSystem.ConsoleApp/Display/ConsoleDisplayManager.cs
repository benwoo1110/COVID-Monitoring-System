//============================================================
// Student Number : S10203296, S10205301
// Student Name   : Benedict Woo, Melvin Kee
// Module Group   : T06
//============================================================

using System;
using System.Collections.Generic;
using COVIDMonitoringSystem.ConsoleApp.Display.Elements;
using COVIDMonitoringSystem.ConsoleApp.Utilities;

namespace COVIDMonitoringSystem.ConsoleApp.Display
{
    public class ConsoleDisplayManager
    {
        private AbstractScreen currentAbstractScreen;
        private Dictionary<string, AbstractScreen> ScreenMap { get; }
        public Stack<AbstractScreen> ScreenStack { get; set; }

        public AbstractScreen CurrentAbstractScreen
        {
            get => currentAbstractScreen;
            set
            {
                currentAbstractScreen = value;
                ScreenUpdated = true;
            }
        }

        public Dictionary<ConsoleKey, Action<ConsoleKeyInfo>> KeyActionMap { get; set; }
        private bool Running { get; set; }
        public bool ScreenUpdated { get; set; }
        public InputResolverManager ResolveManager { get; }
        public InputValuesManager ValuesManager { get; }

        public ConsoleDisplayManager()
        {
            ScreenMap = new Dictionary<string, AbstractScreen>();
            KeyActionMap = new Dictionary<ConsoleKey, Action<ConsoleKeyInfo>>();
            ResolveManager = new InputResolverManager();
            ValuesManager = new InputValuesManager();
            SetDefaultKeyMap();
        }

        private void SetDefaultKeyMap()
        {
            KeyActionMap.Add(ConsoleKey.Tab, OnTab);
            KeyActionMap.Add(ConsoleKey.DownArrow, NextSelection);
            KeyActionMap.Add(ConsoleKey.UpArrow, PreviousSelection);
            KeyActionMap.Add(ConsoleKey.Enter, DoSelection);
            KeyActionMap.Add(ConsoleKey.Escape, EscapeBack);
        }

        private void OnTab(ConsoleKeyInfo key)
        {
            if (CurrentAbstractScreen.SelectedElement is Input inputElement)
            {
                var result = inputElement.NextSuggestion();
                if (!result)
                {
                    return;
                }
            }

            CurrentAbstractScreen.SelectNext();
        }

        private void NextSelection(ConsoleKeyInfo key)
        {
            CurrentAbstractScreen.SelectNext();
        }

        private void PreviousSelection(ConsoleKeyInfo key)
        {
            CurrentAbstractScreen.SelectPrevious();
        }

        private void DoSelection(ConsoleKeyInfo key)
        {
            if (CurrentAbstractScreen.SelectedElement is Button buttonElement)
            {
                buttonElement.Run();
                return;
            }
            if (CurrentAbstractScreen.SelectedElement is Input inputElement)
            {
                inputElement.ApplySuggestion();
                CurrentAbstractScreen.SelectNext();
            }
        }

        private void TypeInput(ConsoleKeyInfo key)
        {
            if (CurrentAbstractScreen.SelectedElement is Input inputElement)
            {
                if (key.Key == ConsoleKey.Backspace)
                {
                    if (inputElement.Text.Length == 0)
                    {
                        return;
                    }
                    
                    inputElement.Text = inputElement.Text.Substring(0, inputElement.Text.Length - 1);
                }
                else if (key.KeyChar >= 32 && key.KeyChar <= 126)
                {
                    inputElement.Text += key.KeyChar;
                }
            }
        }

        private void EscapeBack(ConsoleKeyInfo key)
        {
            if (CurrentAbstractScreen.HasSelection())
            {
                CurrentAbstractScreen.ClearSelection(); 
                return;
            }
            
            PopScreen();
        }
        
        public void RegisterScreen(AbstractScreen abstractScreen)
        {
            if (abstractScreen.DisplayManager != this)
            {
                throw new InvalidOperationException("Screen does not belong to this manager.");
            }
            
            ScreenMap.Add(abstractScreen.Name, abstractScreen);
        }
        
        public void Run(string startingScreenName)
        {
            if (Running)
            {
                throw new InvalidOperationException("Nested console running is not supported.");
            }

            CHelper.DidChangeWindowSize();
            ScreenStack = new Stack<AbstractScreen>(ScreenMap.Count);
            PushScreen(startingScreenName);
            Running = true;
            
            while (Running)
            {
                if (ScreenUpdated)
                {
                    ScreenUpdated = false;
                    CurrentAbstractScreen.PreLoad();
                    CurrentAbstractScreen.Load();
                    CurrentAbstractScreen.OnView();
                }
                
                if (CHelper.DidChangeWindowSize())
                {
                    CurrentAbstractScreen.Render();
                }

                CurrentAbstractScreen.Update();

                var keyPressed = Console.ReadKey(true);
                RunKeyAction(keyPressed);
            }
        }

        private void RunKeyAction(ConsoleKeyInfo keyInfo)
        {
            var keyAction = KeyActionMap.GetValueOrDefault(keyInfo.Key, TypeInput);
            keyAction?.Invoke(keyInfo);
        }

        public void PushScreen(string screenName, object data = null)
        {
            if (ScreenStack.Count > 0)
            {
                var closingScreen = ScreenStack.Peek();
                closingScreen.OnClose();
                closingScreen.Unload();
                closingScreen.Closed();
            }

            var targetScreen = ScreenMap[screenName];
            ScreenStack.Push(targetScreen);
            CurrentAbstractScreen = targetScreen;
            CurrentAbstractScreen.PrePassData(data);
        }

        public void PopScreen(bool doQuit = false)
        {
            if (ScreenStack.Count == 1 && !doQuit)
            {
                return;
            }
            
            var closingScreen = ScreenStack.Pop();
            closingScreen.OnClose();
            closingScreen.Unload();
            
            if (ScreenStack.Count == 0)
            {
                Running = false;
                return;
            }

            CurrentAbstractScreen = ScreenStack.Peek();
        }

        public void Stop()
        {
            Running = false;
        }
    }
}