using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace COVIDMonitoringSystem.Core
{
    public class Utilities
    {
        public static Dictionary<string, string>[] ReadCsv(string filePath)
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

        public static object FetchFromWeb<T>(String uri, String request)
        {
            using HttpClient client = new HttpClient {BaseAddress = new Uri(uri)};
            Task<HttpResponseMessage> responseTask = client.GetAsync(request);
            responseTask.Wait();
            HttpResponseMessage response = responseTask.Result;
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine($"Response from server was not successful. Response dump as follows:\n{GetObjectData(response)}");
                return null;
            }
            string rawData = response.Content.ReadAsStringAsync().Result;
            return JsonConvert.DeserializeObject<T>(rawData);
        }

        public static string GetObjectData(object o)
        {
            return JsonConvert.SerializeObject(o, Formatting.Indented);
        }

        public static void PrintObject(object o)
        {
            Console.WriteLine(GetObjectData(o));
        }
    }
}