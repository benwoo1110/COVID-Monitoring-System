using System;
using COVIDMonitoringSystem.ConsoleApp.Display.Elements;

namespace COVIDMonitoringSystem.ConsoleApp.Display.Builders
{
    public abstract class AbstractScreenBuilder<TB, TS>
        where TS : Screen
        where TB : AbstractScreenBuilder<TB, TS>
    {
        protected ConsoleManager Manager { get; }
        protected TS TargetScreen { get; }

        protected AbstractScreenBuilder(ConsoleManager manager)
        {
            Manager = manager;
            TargetScreen = (TS) Activator.CreateInstance(typeof(TS), manager);
        }

        public TB OfName(string name)
        {
            TargetScreen.Name = name;
            return (TB) this;
        }

        public TB WithHeader(string header)
        {
            TargetScreen.AddElement(new Header("header", header));
            TargetScreen.AddElement(new Spacer());
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