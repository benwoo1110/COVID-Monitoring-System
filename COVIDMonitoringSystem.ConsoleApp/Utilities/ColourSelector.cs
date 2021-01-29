﻿//============================================================
// Student Number : S10203296, S10205301
// Student Name   : Benedict Woo, Melvin Kee
// Module Group   : T06
//============================================================

using System;

namespace COVIDMonitoringSystem.ConsoleApp.Utilities
{
    public static class ColourSelector
    {
        public static void Header()
        {
            Console.BackgroundColor = ConsoleColor.Green;
            Console.ForegroundColor = ConsoleColor.Black;
        }
        
        public static void Element()
        {
            Console.BackgroundColor = ConsoleColor.Gray;
            Console.ForegroundColor = ConsoleColor.Black;
        }
        
        public static void Selected()
        {
            Console.BackgroundColor = ConsoleColor.DarkGray;
            Console.ForegroundColor = ConsoleColor.White;
        }

        public static void Default()
        {
            Console.ResetColor();
        }
    }
}