using System;
using System.Collections.Generic;
using COVIDMonitoringSystem.ConsoleApp.Display;
using COVIDMonitoringSystem.ConsoleApp.Display.Elements;
using COVIDMonitoringSystem.ConsoleApp.Screens;

namespace COVIDMonitoringSystem.ConsoleApp.Builders
{
    public class ListScreenBuilder<T> : AbstractScreenBuilder<ListScreenBuilder<T>, BuilderScreen> where T : class
    {
        public ObjectList<T> ListObject { get; set; }
        
        public ListScreenBuilder(ConsoleDisplayManager displayManager) : base(displayManager)
        {
            ListObject = new ObjectList<T>(typeof(T).Name);
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
        
        public override AbstractScreen Build()
        {
            TargetScreen.AddElement(ListObject);
            return TargetScreen;
        }
    }
}