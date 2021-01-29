//============================================================
// Student Number : S10203296, S10205301
// Student Name   : Benedict Woo, Melvin Kee
// Module Group   : T06
//============================================================

using System;
using System.Collections.Generic;
using System.Reflection;
using COVIDMonitoringSystem.ConsoleApp.Display;
using COVIDMonitoringSystem.ConsoleApp.Display.Attributes;
using COVIDMonitoringSystem.ConsoleApp.Display.Elements;
using COVIDMonitoringSystem.Core.Utilities;

namespace COVIDMonitoringSystem.ConsoleApp.Utilities
{
    public class ActionMethod
    {
        public MethodInfo Method { get; }
        public List<KeyValuePair<ParameterInfo, Parser>> Parameters { get; }

        public ActionMethod(MethodInfo method)
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

                var input = screen.FindElementOfType<Input>(parser.InputName);

                var parseResult = (parser.TargetLabel == null)
                    ? SilentParsing(screen, input, parameter.ParameterType)
                    : ResponsiveParsing(screen, input, parameter.ParameterType, screen.FindElementOfType<TextElement>(parser.TargetLabel));

                if (parseResult == null)
                {
                    return;
                }
                
                arguments.Add(parseResult);
            }
            
            Method?.Invoke(screen, arguments.ToArray());
        }

        private object SilentParsing(AbstractScreen screen, Input input, Type type)
        {
            try
            {
                return screen.DisplayManager.ResolveManager.Parse(screen, input.Text, type);
            }
            catch
            {
                // ignored
            }

            return null;
        }

        private object ResponsiveParsing(AbstractScreen screen, Input input, Type type, TextElement errorText)
        {
            object parseResult;
            try
            {
                return screen.DisplayManager.ResolveManager.Parse(screen, input.Text, type);
            }
            catch (ArgumentNullException e)
            {
                errorText.Text = $"Incomplete details. Please enter an input for {input.Name}.";
            }
            catch (InputParseFailedException e)
            {
                errorText.Text = e.Message;
            }
            catch (Exception)
            {
                errorText.Text = $"There was an error getting input for {input.Name}. Is it in the correct format?";
            }

            return null;
        }
    }
}