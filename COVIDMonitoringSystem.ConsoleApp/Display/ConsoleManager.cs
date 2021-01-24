﻿using System;
using System.Collections.Generic;
using COVIDMonitoringSystem.ConsoleApp.Display.Elements;

namespace COVIDMonitoringSystem.ConsoleApp.Display
{
    public class ConsoleManager
    {
        private Dictionary<string, Screen> ScreenMap { get; }
        public Stack<Screen> ScreenStack { get; set; }
        public Screen CurrentScreen { get; set; }
        public Dictionary<ConsoleKey, Action<ConsoleKeyInfo>> KeyActionMap { get; set; }
        private bool Running { get; set; }

        public ConsoleManager()
        {
            ScreenMap = new Dictionary<string, Screen>();
            KeyActionMap = new Dictionary<ConsoleKey, Action<ConsoleKeyInfo>>();
            SetDefaultKeyMap();
        }

        private void SetDefaultKeyMap()
        {
            KeyActionMap.Add(ConsoleKey.DownArrow, NextSelection);
            KeyActionMap.Add(ConsoleKey.UpArrow, PreviousSelection);
            KeyActionMap.Add(ConsoleKey.Enter, DoSelection);
            KeyActionMap.Add(ConsoleKey.Escape, EscapeBack);
        }

        private void NextSelection(ConsoleKeyInfo key)
        {
            CurrentScreen.ChangeSelection(1);
        }

        private void PreviousSelection(ConsoleKeyInfo key)
        {
            CurrentScreen.ChangeSelection(-1);
        }

        private void DoSelection(ConsoleKeyInfo key)
        {
            if (CurrentScreen.SelectedElement is Button buttonElement)
            {
                buttonElement.Run();
            }
        }

        private void TypeInput(ConsoleKeyInfo key)
        {
            if (CurrentScreen.SelectedElement is Input inputElement)
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

                CurrentScreen.DisplaySelected(inputElement);
                inputElement.UpdateWidth();
                Console.SetCursorPosition(inputElement.BoundingBox.Left, inputElement.BoundingBox.Top);
            }
        }

        private void EscapeBack(ConsoleKeyInfo key)
        {
            PopScreen();
        }
        
        public ConsoleManager RegisterScreen(Screen screen)
        {
            screen.Manager = this;
            ScreenMap.Add(screen.Name, screen);
            return this;
        }
        
        public void Run(string startingScreenName)
        {
            if (Running)
            {
                throw new InvalidOperationException("Nested console running is not supported.");
            }

            ScreenStack = new Stack<Screen>(ScreenMap.Count);
            PushScreen(startingScreenName);
            Running = true;


            while (Running)
            {
                CurrentScreen.OnView();
                if (CHelper.DidChangeWindowSize())
                {
                    CurrentScreen.Display();
                }

                var keyPressed = Console.ReadKey(true);
                RunKeyAction(keyPressed);
            }
        }

        private void RunKeyAction(ConsoleKeyInfo keyInfo)
        {
            var keyAction = KeyActionMap.GetValueOrDefault(keyInfo.Key, TypeInput);
            keyAction?.Invoke(keyInfo);
        }

        public void PushScreen(string screenName)
        {
            var targetScreen = ScreenMap[screenName];
            ScreenStack.Push(targetScreen);
            targetScreen.Load();
            CurrentScreen = targetScreen;
        }

        public void PopScreen(bool doQuit = false)
        {
            if (ScreenStack.Count == 1 && !doQuit)
            {
                return;
            }
            
            ScreenStack.Pop().Unload();
            if (ScreenStack.Count == 0)
            {
                Running = false;
                return;
            }

            CurrentScreen = ScreenStack.Peek();
            CurrentScreen.Display();
        }

        public void Stop()
        {
            Running = false;
        }
    }
}