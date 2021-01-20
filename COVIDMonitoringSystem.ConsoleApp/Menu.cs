using System;
using JetBrains.Annotations;

namespace COVIDMonitoringSystem.ConsoleApp
{
    public class Menu
    {
        private const int SpecialOption = 0;

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
            Console.WriteLine(Header);
            for (var index = 0; index < Contents.Length; index++)
            {
                Console.WriteLine($"[{index + 1}] {Contents[index].Name}");
            }
            Console.WriteLine($"[{SpecialOption}] {SpecialOptionName}");
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