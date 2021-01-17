﻿using System;
using System.Collections.Generic;
using System.IO;

namespace COVIDMonitoringSystem.Core
{
    public class Utilities
    {
        public static Dictionary<string, string>[] ReadCsv(string filePath)
        {
            var fileData = File.ReadAllLines(filePath);
            var headers = fileData[0].Split(',');

            var csvEntries = new Dictionary<string, string>[fileData.Length - 1];
            for (var index = 1; index < fileData.Length - 1; index++)
            {
                string[] lineData = fileData[index].Split(',');
                csvEntries[index] = ParseSingleCsvLine(headers, lineData);
            }

            return csvEntries;
        }

        private static Dictionary<string, string> ParseSingleCsvLine(string[] headers, string[] lineData)
        {
            if (headers.Length != lineData.Length)
            {
                throw new ArgumentException("Headers does not match amount of data provided.");
            }

            var csvEntry = new Dictionary<string, string>();
            for (var index = 0; index < headers.Length; index++)
            {
                csvEntry.Add(headers[index], lineData[index]);
            }

            return csvEntry;
        }
    }
}