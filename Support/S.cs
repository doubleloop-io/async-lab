using System;
using System.Diagnostics;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace AsyncLab.Support
{
    public static class S
    {
        public static void Log(object obj, Formatting formatting = Formatting.Indented)
        {
            Log($"{obj.ToJson(formatting)}");
        }
        public static void Log(string message)
        {
            var enrichedMessage = Format(message);
            Console.WriteLine(enrichedMessage);
        }

        public static string Format(string message)
        {
            var enrichedMessage = string.IsNullOrEmpty(message) 
                ? "" 
                : $"[{Thread.CurrentThread.ManagedThreadId}:{Type()}-{Source()}] {message}";

            string Source()
            {
                return Thread.CurrentThread.IsThreadPoolThread ? "Pool" : "Dedicated";
            }

            string Type()
            {
                return Thread.CurrentThread.IsBackground ? "Bg" : "Fg";
            }

            return enrichedMessage;
        }

        private static Stopwatch theWatch;

        public static void StartTheWatch([CallerMemberName] string callerName = null)
        {
            //Log($"{callerName}() running");
            theWatch = Stopwatch.StartNew();
        }

        public static void StopTheWatch([CallerMemberName] string callerName = null)
        {
            //Log($"{callerName}() total duration: {theWatch.Elapsed.TotalMilliseconds}ms");
            Log($"Total duration: {theWatch.Elapsed.TotalMilliseconds - T.GCDelay}ms");
        }

        public static async Task<string> HttpGetAsync(string url)
        {
            string content = "";
            using (var httpClient = new HttpClient())
            {
                var response = await httpClient.GetAsync(url).ConfigureAwait(false);
                content = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            }
            
            return content;
        }
    }

    public static class JsonExtensions
    {
        public static string ToJson(this object obj, Formatting formatting = Formatting.Indented)
        {
            return JsonConvert.SerializeObject(obj, formatting, new StringEnumConverter());
        }
    }
}