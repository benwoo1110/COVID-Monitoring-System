//============================================================
// Student Number : S10203296, S10205301
// Student Name   : Benedict Woo, Melvin Kee
// Module Group   : T06
//============================================================

using System;
using System.Runtime.Serialization;
using JetBrains.Annotations;

namespace COVIDMonitoringSystem.ConsoleApp.Utilities
{
    public class InputParseFailedException : Exception
    {
        public InputParseFailedException()
        {
        }

        protected InputParseFailedException([NotNull] SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        public InputParseFailedException([CanBeNull] string message) : base(message)
        {
        }

        public InputParseFailedException([CanBeNull] string message, [CanBeNull] Exception innerException) : base(message, innerException)
        {
        }
    }
}