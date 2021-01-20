using System;
using JetBrains.Annotations;

namespace COVIDMonitoringSystem.ConsoleApp
{
    public class Menu
    {
        private const int ExitOption = 0;

        private string Header { get; }
        private MenuOption[] Contents { get; }

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
                var optionNumber = ConsoleHelper.GetInput("Enter option: ", OptionParser);
                if (IsExitOption(optionNumber))
                {
                    ConsoleHelper.ExitProgram();
                }

                Contents[optionNumber - 1].RunMethod();
                return;
            }
        }

        private int OptionParser([CanBeNull] string input)
        {
            int optionNumber;
            try
            {
                optionNumber = Convert.ToInt32(input);
                if (!IsInRange(optionNumber))
                {
                    throw new Exception();
                }
            }
            catch (Exception)
            {
                throw new InputParseFailedException($"Please enter a number between 0 and {Contents.Length}!");
            }

            return optionNumber;
        }

        private bool IsInRange(int input)
        {
            return input >= 0 && input <= Contents.Length;
        }

        private static bool IsExitOption(int input)
        {
            return input == ExitOption;
        }
    }
}