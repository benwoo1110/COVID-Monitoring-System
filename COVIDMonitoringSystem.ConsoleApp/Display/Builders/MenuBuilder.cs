using COVIDMonitoringSystem.ConsoleApp.Display.Elements;

namespace COVIDMonitoringSystem.ConsoleApp.Display.Builders
{
    public class MenuBuilder : AbstractScreenBuilder<MenuBuilder>
    { 
        private int OptionCount { get; set; }

        public MenuBuilder(ConsoleManager manager) : base(manager)
        {
            OptionCount = 0;
        }

        public MenuBuilder AddOption(string optionName, string targetScreen)
        {
            TargetScreen.AddElement(new Button
            {
                Text = $"[{++OptionCount}] {optionName}",
                Run = () => Manager.PushScreen(targetScreen)
            });
            return this;
        }

        public override Screen Build()
        {
            TargetScreen.AddElement(new Label
            {
                Text = "----"
            });
            AddSpecialOption();

            return TargetScreen;
        }

        protected virtual void AddSpecialOption()
        {
            TargetScreen.AddElement(new Button
            {
                Text = "[0] Back",
                Run = () => Manager.PopScreen()
            });
        }
    }
}