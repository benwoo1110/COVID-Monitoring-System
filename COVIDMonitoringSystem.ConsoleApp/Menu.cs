using System;

namespace COVIDMonitoringSystem.ConsoleApp
{
    public class Menu
    {
        private const int ExitOption = 0;
        
        public string Header { get; }
        public MenuOption[] Contents { get; }

        public Menu(string header, MenuOption[] contents)
        {
            Header = header;
            Contents = contents;
        }

        public void RunMenuOptionLooped()
        {
            while (true)
            {
                RunMenuOption();
            }
        }

        public void RunMenuOption()
        {
            ShowMenu();
            DoOptionAction();
        }

        private void ShowMenu()
        {
            ConsoleHelper.EmptyLine();
            Console.WriteLine(Header);
            for (var index = 0; index < Contents.Length; index++)
            {
                Console.WriteLine($"[{index + 1}] {Contents[index].Name}");
            }
            Console.WriteLine($"[{ExitOption}] Exit");
        }

        private void DoOptionAction()
        {
            while (true)
            {
                int input = ConsoleHelper.GetInput("Enter option: ", Convert.ToInt32);
                if (!IsInRange(input))
                {
                    Console.WriteLine($"Please enter a number between 0 and {Contents.Length}!");
                    continue;
                }
                if (IsExitOption(input))
                {
                    ConsoleHelper.ExitProgram();
                }

                Contents[input - 1].RunMethod();
                return;
            }
        }

        private static bool IsExitOption(int input)
        {
            return input == ExitOption;
        }

        private bool IsInRange(int input)
        {
            return input >= 0 && input <= Contents.Length;
        }
    }
}