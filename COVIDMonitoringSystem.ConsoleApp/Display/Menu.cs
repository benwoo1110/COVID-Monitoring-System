using System;
using COVIDMonitoringSystem.ConsoleApp.Utilities;
using JetBrains.Annotations;

namespace COVIDMonitoringSystem.ConsoleApp.Display
{
    public class Menu
    {
        private const int SpecialOption = 0;
        private const int Length = 40;

        public string Header { get; }
        public MenuOption[] Contents { get; }
        public string SpecialOptionName { get; set; }
        public bool Running { get; set; }

        public Menu(string header, MenuOption[] contents)
        {
            Header = header;
            Contents = contents;
            SpecialOptionName = "Back";
        }

        public void RunMenuOption()
        {
            if (Running)
            {
                throw new InvalidOperationException("Nested menu call!");
            }
            
            Running = true;
            while (Running)
            {
                ShowMenu();
                DoOptionAction();
            }
        }

        private void ShowMenu()
        {
            ConsoleHelper.EmptyLine();
            FancyObjectDisplay.PrintHeader(Header, Length);
            for (var index = 0; index < Contents.Length; index++)
            {
                ConsoleHelper.WriteWithPipe($"{$"[{index + 1}] {Contents[index].Name}",-(Length-4)}");
            }

            ConsoleHelper.WriteSubSeparator(Length);
            ConsoleHelper.WriteWithPipe($"{$"[{SpecialOption}] {SpecialOptionName}",-(Length - 4)}");
            ConsoleHelper.WriteSeparator(Length);
        }

        private void DoOptionAction()
        {
            while (true)
            {
                var optionNumber = ConsoleHelper.GetInput("Enter option: ", OptionParser);
                if (IsSpecialOption(optionNumber))
                {
                    DoSpecialOption();
                    return;
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

        private static bool IsSpecialOption(int input)
        {
            return input == SpecialOption;
        }

        protected virtual void DoSpecialOption()
        {
            Running = false;
        }
    }
}