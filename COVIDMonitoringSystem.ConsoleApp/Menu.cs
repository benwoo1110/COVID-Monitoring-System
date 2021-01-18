using System;

namespace Cryptography.ConsoleApp
{
    public class Menu
    {
        private readonly string header;
        private readonly string[] contents;
        
        private const int ExitOption = 0;

        public Menu(string header, string[] contents)
        {
            this.header = header;
            this.contents = contents;
        }

        public string RunMenuOption()
        {
            ShowMenu();
            return GetInputOption();
        }

        private string GetInputOption()
        {
            while (true)
            {
                int input = ConsoleHelper.GetInput("Enter option: ", Convert.ToInt32);
                if (!IsInRange(input))
                {
                    Console.WriteLine($"Please enter a number between 0 and {contents.Length}!");
                    continue;
                }
                if (IsExitOption(input))
                {
                    ConsoleHelper.ExitProgram();
                }

                return contents[input - 1];
            }
        }

        private static bool IsExitOption(int input)
        {
            return input == ExitOption;
        }

        private bool IsInRange(int input)
        {
            return input >= 0 && input <= contents.Length;
        }

        private void ShowMenu()
        {
            Console.WriteLine(header);
            for (var index = 0; index < contents.Length; index++)
            {
                Console.WriteLine($"[{index + 1}] {contents[index]}");
            }
            Console.WriteLine($"[{ExitOption}] Exit");
        }
    }
}