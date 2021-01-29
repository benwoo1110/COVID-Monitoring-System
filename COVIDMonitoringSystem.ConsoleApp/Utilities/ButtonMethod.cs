using System;
using System.Collections.Generic;
using System.Reflection;
using COVIDMonitoringSystem.ConsoleApp.Display;
using COVIDMonitoringSystem.ConsoleApp.Display.Attributes;
using COVIDMonitoringSystem.Core.Utilities;

namespace COVIDMonitoringSystem.ConsoleApp.Utilities
{
    public class ButtonMethod
    {
        public MethodInfo Method { get; }
        public List<KeyValuePair<ParameterInfo, Parser>> Parameters { get; }

        public ButtonMethod(MethodInfo method)
        {
            Method = method;
            Parameters = ReflectHelper.GetParametersAttributeMap<Parser>(method);
        }

        public void Run(AbstractScreen screen)
        {
            var arguments = new List<object>();
            
            foreach (var (parameter, parser) in Parameters)
            {
                if (parser == null)
                {
                    arguments.Add(null);
                    continue;
                }
                arguments.Add(parser.InputName);
            }
            
            Method?.Invoke(screen, arguments.ToArray());
        }
    }
}