using COVIDMonitoringSystem.ConsoleApp.Display.Elements;

namespace COVIDMonitoringSystem.ConsoleApp.Display.Builders
{
    public class MainMenuBuilder : MenuBuilder
    {
        public MainMenuBuilder(ConsoleManager manager) : base(manager)
        {
        }

        protected override void AddSpecialOption()
        {
            TargetScreen.AddElement(new Button
            {
                Text = "[0] Exit",
                Run = () => Manager.Stop()
            });
        }
    }
}