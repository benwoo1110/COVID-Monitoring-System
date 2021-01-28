using COVIDMonitoringSystem.ConsoleApp.Display;
using COVIDMonitoringSystem.ConsoleApp.Display.Elements;

namespace COVIDMonitoringSystem.ConsoleApp.Builders
{
    public class MenuBuilder : AbstractScreenBuilder<MenuBuilder, Screen>
    {
        protected int OptionCount { get; set; }

        public MenuBuilder(ConsoleDisplayManager displayManager) : base(displayManager)
        {
        }

        public MenuBuilder AddOption(string optionName, string targetScreen)
        {
            TargetScreen.AddElement(new Button(optionName)
            {
                Text = $"[{++OptionCount}] {optionName}",
                Runner = () => DisplayManager.PushScreen(targetScreen),
                BoundingBox = {Top = OptionCount + 3}
            });
            return this;
        }

        public override Screen Build()
        {
            TargetScreen.AddElement(new Label("separator")
            {
                Text = "----",
                BoundingBox = {Top = OptionCount + 4}
            });
            AddSpecialOption();

            return TargetScreen;
        }

        protected virtual void AddSpecialOption()
        {
            TargetScreen.AddElement(new Button("back")
            {
                Text = "[0] Back",
                Runner = () => DisplayManager.PopScreen(),
                BoundingBox = {Top = OptionCount + 5}
            });
        }
    }
}