using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Newtonsoft.Json;

namespace COVIDMonitoringSystem.Core
{
    public static class Utilities
    {
        public static Dictionary<string, string>[] ReadCsv([NotNull] string filePath)
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

        private static Dictionary<string, string> ParseSingleCsvLine(
            [NotNull] string[] headers, 
            [NotNull] string[] lineData)
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

        public static bool WriteCsv(
            [NotNull] string filePath, 
            [NotNull] string[] headers, 
            [NotNull] IEnumerable<Dictionary<string, string>> data) 
        {
            var contents = BuildCsvContents(headers, data);
            try
            {
                using var fs = File.Create(filePath);
                var byteContents = new UTF8Encoding(true).GetBytes(contents);
                fs.Write(byteContents, 0, contents.Length);
            }
            catch (IOException)
            {
                return false;
            }

            return true;
        }

        private static string BuildCsvContents(
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
            [NotNull] Dictionary<string, string> entry, 
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

        public static object FetchFromWeb<T>(
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

        public static string GetObjectData(object o)
        {
            return JsonConvert.SerializeObject(o, Formatting.Indented);
        }

        public static void DebugObject(object o)
        {
            Console.WriteLine(GetObjectData(o));
        }
    }
}