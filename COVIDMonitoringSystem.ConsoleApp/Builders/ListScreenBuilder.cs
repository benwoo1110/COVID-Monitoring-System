//============================================================
// Student Number : S10203296, S10205301
// Student Name   : Benedict Woo, Melvin Kee
// Module Group   : T06
//============================================================

using System;
using System.Collections.Generic;
using COVIDMonitoringSystem.ConsoleApp.Display;
using COVIDMonitoringSystem.ConsoleApp.Display.Elements;

namespace COVIDMonitoringSystem.ConsoleApp.Builders
{
    public class ListScreenBuilder<T> : AbstractScreenBuilder<ListScreenBuilder<T>, BuilderScreen> where T : class
    {
        private ObjectList<T> ListObject { get; }
        
        public ListScreenBuilder(ConsoleDisplayManager displayManager) : base(displayManager)
        {
            ListObject = new ObjectList<T>(typeof(T).Name)
            {
                BoundingBox = {Top = 3}
            };
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