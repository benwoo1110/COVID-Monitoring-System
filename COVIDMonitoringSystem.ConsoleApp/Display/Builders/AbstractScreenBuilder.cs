using COVIDMonitoringSystem.ConsoleApp.Display.Elements;

namespace COVIDMonitoringSystem.ConsoleApp.Display.Builders
{
    public abstract class AbstractScreenBuilder<TB> where TB : AbstractScreenBuilder<TB>
    {
        protected ConsoleManager Manager { get; }
        protected Screen TargetScreen { get; }

        protected AbstractScreenBuilder(ConsoleManager manager)
        {
            Manager = manager;
            TargetScreen = new Screen(manager);
        }

        public TB OfName(string name)
        {
            TargetScreen.Name = name;
            return (TB) this;
        }

        public TB WithHeader(string header)
        {
            TargetScreen.Header = header;
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