using COVIDMonitoringSystem.ConsoleApp.Display.Elements;

namespace COVIDMonitoringSystem.ConsoleApp.Display.Builders
{
    public class MenuBuilder : AbstractScreenBuilder<MenuBuilder, Screen>
    {
        protected int OptionCount { get; set; }

        public MenuBuilder(ConsoleManager manager) : base(manager)
        {
        }

        public MenuBuilder AddOption(string optionName, string targetScreen)
        {
            TargetScreen.AddElement(new Button(optionName)
            {
                Text = $"[{++OptionCount}] {optionName}",
                Runner = () => Manager.PushScreen(targetScreen),
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
                Runner = () => Manager.PopScreen(),
                BoundingBox = {Top = OptionCount + 5}
            });
        }
    }
}