using System;
using System.Collections.Generic;
using COVIDMonitoringSystem.ConsoleApp.Display.Elements;

namespace COVIDMonitoringSystem.ConsoleApp.Display.Builders
{
    public class ListScreenBuilder<T> : AbstractScreenBuilder<ListScreenBuilder<T>, Screen> where T : class
    {
        public ObjectList<T> ListObject { get; set; }
        
        public ListScreenBuilder(ConsoleManager manager) : base(manager)
        {
            ListObject = new ObjectList<T>();
        }

        public ListScreenBuilder<T> WithProperties(string[] properties)
        {
            ListObject.PropertyToInclude = properties;
            return this;
        }

        public ListScreenBuilder<T> WithGetter(Func<List<T>> getter)
        {
            ListObject.ListGetter = getter;
            return this;
        }
        
        public override Screen Build()
        {
            TargetScreen.AddElement(ListObject);
            return TargetScreen;
        }
    }
}