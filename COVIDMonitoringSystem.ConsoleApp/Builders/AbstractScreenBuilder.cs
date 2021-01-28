using System;
using COVIDMonitoringSystem.ConsoleApp.Display;
using COVIDMonitoringSystem.ConsoleApp.Display.Elements;

namespace COVIDMonitoringSystem.ConsoleApp.Builders
{
    public abstract class AbstractScreenBuilder<TB, TS>
        where TS : Screen
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
            TargetScreen.Name = name;
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

        public virtual Screen Build()
        {
            return TargetScreen;
        }
    }
}