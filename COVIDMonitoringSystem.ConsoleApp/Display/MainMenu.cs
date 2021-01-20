using COVIDMonitoringSystem.ConsoleApp.Utilities;

namespace COVIDMonitoringSystem.ConsoleApp.MenuDisplay
{
    public class MainMenu : Menu
    {
        public MainMenu(string header, MenuOption[] contents) : base(header, contents)
        {
            SpecialOptionName = "Exit";
        }

        protected override void DoSpecialOption()
        {
            if (!ConsoleHelper.Confirm("Are you sure you want to quit the program?"))
            {
                return;
            }
            
            Running = false;
            ConsoleHelper.ExitProgram();
        }
    }
}