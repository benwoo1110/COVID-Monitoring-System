//============================================================
// Student Number : S10203296, S10205301
// Student Name   : Benedict Woo, Melvin Kee
// Module Group   : T06
//============================================================

using System;

namespace COVIDMonitoringSystem.Core.Utilities
{
    /// <summary>
    /// Store details of a created file by the program.
    /// </summary>
    public class FileCreateResult
    {
        public CreateStatus Status { get; internal set; } = CreateStatus.Unknown;
        public Exception Errors { get; internal set; }
        public string FilePath { get; internal set; }

        public bool IsSuccessful()
        {
            return Status == CreateStatus.Success && Errors == null;
        }

        public string GetErrorMessage()
        {
            if (IsSuccessful())
            {
                return string.Empty;
            }
            
            return Errors != null 
                ? Errors.Message 
                : "An unknown error occured when trying to create the file.";
        }
    }
}