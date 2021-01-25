using COVIDMonitoringSystem.ConsoleApp.Display.Elements;

namespace COVIDMonitoringSystem.ConsoleApp.Display.Builders
{
    public class MenuBuilder : AbstractScreenBuilder<MenuBuilder, Screen>
    { 
        private int OptionCount { get; set; }

        public MenuBuilder(ConsoleManager manager) : base(manager)
        {
        }

        public MenuBuilder AddOption(string optionName, string targetScreen)
        {
            TargetScreen.AddElement(new Button(optionName)
            {
                Text = $"[{++OptionCount}] {optionName}",
                Runner = (s) => Manager.PushScreen(targetScreen)
            });
            return this;
        }

        public override Screen Build()
        {
            TargetScreen.AddElement(new Label("separator")
            {
                Text = "----"
            });
            AddSpecialOption();

            return TargetScreen;
        }

        protected virtual void AddSpecialOption()
        {
            TargetScreen.AddElement(new Button("back")
            {
                Text = "[0] Back",
                Runner = s => Manager.PopScreen()
            });
        }
    }
}