﻿using System;
using COVIDMonitoringSystem.ConsoleApp.Utilities;

namespace COVIDMonitoringSystem.ConsoleApp.Display
{
    public class LegacyScreen : Screen
    {
        public Action Runner { get; set; }

        public LegacyScreen(ConsoleManager manager) : base(manager)
        {
        }

        public LegacyScreen(ConsoleManager manager, string name, string header, Action runner) : base(manager)
        {
            Name = name;
            //TODO: header
            // Header = header;
            Runner = runner;
        }

        public override void OnView()
        {
            ColourSelector.Element();
            Console.SetCursorPosition(0, 4);
            Runner?.Invoke();
            CHelper.WriteEmpty();
            CHelper.Pause("Back to menu...");
            Manager.PopScreen();
        }
    }
}