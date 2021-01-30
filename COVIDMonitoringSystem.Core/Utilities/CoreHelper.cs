//============================================================
// Student Number : S10203296, S10205301
// Student Name   : Benedict Woo, Melvin Kee
// Module Group   : T06
//============================================================

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using JetBrains.Annotations;
using Newtonsoft.Json;

namespace COVIDMonitoringSystem.Core.Utilities
{
    public static class CoreHelper
    {
        private static readonly JsonSerializerSettings Config = new JsonSerializerSettings
        {
            ReferenceLoopHandling = ReferenceLoopHandling.Serialize,
            PreserveReferencesHandling = PreserveReferencesHandling.Objects
        };

        public static TE ParseEnum<TE>(string value) where TE : struct
        {
            return Enum.Parse<TE>(value, true);
        }

        public static bool OpenFile(string path)
        {
            try
            {
                Process.Start("cmd.exe", $"/C {path}");
            }
            catch
            {
                return false;
            }

            return true;
        }
        
        [NotNull] public static Dictionary<string, string>[] ReadCsv([NotNull] string filePath)
        {
            var fileData = File.ReadAllLines(filePath);
            var headers = fileData[0].Split(',');

            var csvEntries = new Dictionary<string, string>[fileData.Length - 1];
            for (var index = 1; index < fileData.Length; index++)
            {
                string[] lineData = fileData[index].Split(',');
                csvEntries[index-1] = ParseSingleCsvLine(headers, lineData);
            }

            return csvEntries;
        }

        [NotNull] private static Dictionary<string, string> ParseSingleCsvLine(
            [NotNull] IReadOnlyList<string> headers, 
            [NotNull] IReadOnlyList<string> lineData)
        {
            if (headers.Count != lineData.Count)
            {
                throw new ArgumentException("Headers does not match amount of data provided.");
            }

            var csvEntry = new Dictionary<string, string>();
            for (var index = 0; index < headers.Count; index++)
            {
                csvEntry.Add(headers[index], lineData[index]);
            }

            return csvEntry;
        }

        public static FileCreateResult WriteCsv(
            [NotNull] string filePath, 
            [NotNull] string[] headers, 
            [NotNull] IEnumerable<Dictionary<string, string>> data)
        {
            var result = new FileCreateResult();
            var contents = BuildCsvContents(headers, data);
            
            try
            {
                using var fs = File.Create(filePath);
                var byteContents = new UTF8Encoding(true).GetBytes(contents);
                fs.Write(byteContents, 0, contents.Length);
                result.FilePath = fs.Name;
            }
            catch (IOException e)
            {
                result.Errors = e;
                result.Status = CreateStatus.Failed;
            }

            result.Status = CreateStatus.Success;
            return result;
        }

        [NotNull] private static string BuildCsvContents(
            [NotNull] string[] headers, 
            [NotNull] IEnumerable<Dictionary<string, string>> data)
        {
            var builder = new StringBuilder();

            builder.Append(string.Join(',', headers)).Append("\n");
            foreach (var entry in data)
            {
                BuildEntry(headers, entry, builder);
            }

            return builder.ToString();
        }

        private static void BuildEntry(
            [NotNull] string[] headers, 
            [NotNull] IReadOnlyDictionary<string, string> entry, 
            [NotNull] StringBuilder builder)
        {
            foreach (var colName in headers)
            {
                builder.Append(entry[colName]);
                if (colName != headers.Last())
                {
                    builder.Append(',');
                }
            }
            builder.Append("\n");
        }

        [CanBeNull] public static object FetchFromWeb<T>(
            [NotNull] string uri, 
            [NotNull] string request)
        {
            using var client = new HttpClient { BaseAddress = new Uri(uri)};
            var responseTask = client.GetAsync(request);
            var response = responseTask.Result;
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine($"Response from server was not successful. Response dump as follows:\n{GetObjectData(response)}");
                return null;
            }
            var rawData = response.Content.ReadAsStringAsync().Result;
            return JsonConvert.DeserializeObject<T>(rawData);
        }

        [NotNull] public static string GetObjectData([CanBeNull] object o)
        {
            return JsonConvert.SerializeObject(o, Formatting.Indented, Config);
        }

        public static int GetStringLength([CanBeNull] string text)
        {
            return text?.Length ?? 0;
        }

        public static int Mod(int value, int modulus)
        {
            return value == 0
                ? 0
                : ((value % modulus) + modulus) % modulus;
        }
    }
}