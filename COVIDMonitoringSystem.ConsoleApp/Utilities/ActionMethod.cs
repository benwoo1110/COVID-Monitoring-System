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
        private MethodInfo Method { get; }
        private List<KeyValuePair<ParameterInfo, InputParam>> Parameters { get; }
        private Dictionary<ParameterInfo, Values> ParamValuesChecker { get; }

        public ActionMethod(MethodInfo method)
        {
            Method = method;
            Parameters = ReflectHelper.GetParametersAttributeList<InputParam>(method);
            
            //TODO: Not efficient but ok for now
            ParamValuesChecker = ReflectHelper.GetParametersAttributeDict<Values>(method);
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
                if (input == null)
                {
                    throw new InvalidOperationException($"No input of have {parser.InputName} in screen.");
                }

                if (input.Hidden)
                {
                    arguments.Add(null);
                    continue;
                }

                var parseResult = (parser.TargetLabel == null)
                    ? SilentParsing(screen, input, parameter.ParameterType, parameter)
                    : ResponsiveParsing(screen, input, parameter.ParameterType, screen.FindElementOfType<TextElement>(parser.TargetLabel), parameter);

                if (parseResult == null)
                {
                    return;
                }
                
                arguments.Add(parseResult);
            }
            
            Method.Invoke(screen, arguments.ToArray());
        }

        private object SilentParsing(AbstractScreen screen, Input input, Type type, ParameterInfo parameterInfo)
        {
            try
            {
                return ParseResult(screen, input, type, parameterInfo);
            }
            catch
            {
                // ignored
            }

            return null;
        }

        private object ResponsiveParsing(AbstractScreen screen, Input input, Type type, TextElement errorText, ParameterInfo parameterInfo)
        {
            try
            {
                return ParseResult(screen, input, type, parameterInfo);
            }
            catch (ArgumentNullException)
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

        private object ParseResult(AbstractScreen screen, Input input, Type type, ParameterInfo parameterInfo)
        {
            var result = screen.DisplayManager.ResolveManager.Parse(screen, input.Text, type);
            var value = ParamValuesChecker.GetValueOrDefault(parameterInfo);

            if (value != null)
            {
                screen.DisplayManager.ValuesManager.DoCheck(value.ValueName, screen, result);
            }

            return result;
        }
    }
}