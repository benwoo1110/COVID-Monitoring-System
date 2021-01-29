using System;
using COVIDMonitoringSystem.ConsoleApp.Display.Elements;
using COVIDMonitoringSystem.ConsoleApp.Screens;

namespace COVIDMonitoringSystem.ConsoleApp.Display
{
    public abstract class AbstractScreenBuilder<TB, TS>
        where TS : BuilderScreen
        where TB : AbstractScreenBuilder<TB, TS>
    {
        protected ConsoleDisplayManager DisplayManager { get; }
        protected TS TargetScreen { get; }

        protected AbstractScreenBuilder(ConsoleDisplayManager displayManager)
        {
            DisplayManager = displayManager;
            TargetScreen = (TS) Activator.CreateInstance(typeof(TS), displayManager);
        }

        public TB OfName(string name)
        {
            TargetScreen.ScreenName = name;
            return (TB) this;
        }

        public TB WithHeader(string header)
        {
            TargetScreen.AddElement(new Header("header", header));
            return (TB) this;
        }

        public TB AddElement(Element element)
        {
            TargetScreen.AddElement(element);
            return (TB) this;
        }

        public virtual AbstractScreen Build()
        {
            return TargetScreen;
        }
    }
}