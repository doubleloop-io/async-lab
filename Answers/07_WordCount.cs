using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AsyncLab.Support;

namespace AsyncLab.Answers
{
    [DoNotRun]
    public class WordCount
    {
        static readonly object LockObj = new object();
        readonly IOutput output;
        static readonly char[] Separators = new[] {' ', ',', '.', '-', ';', ':', '"', '*'};

        public WordCount(IOutput output)
        {
            this.output = output;
        }

        [Run]
        public async Task Count(string[] args)
        {
            if (args.Length == 0)
            {
                output.Log("You must pass 1,2 or 3 to choose between Sync, Async, Parallel execution");
                return;
            }

            var cmd = args[0];
            if (cmd == "1")
                await Sync();
            else if (cmd == "2")
                await Async();
            else if (cmd == "3")
                await ParallelForEach();
            else
                output.Log($"You passed the wrong choice '{cmd}'");
        }

        Task Sync()
        {
            var content = LoadDraculaBook();

            var wordUsages = new Dictionary<string, int>();
            var threadUsages = new Dictionary<int, int>();
            CountWords(wordUsages, threadUsages, AllWordsFrom(content));

            output.Log($"Stats: {wordUsages.Count} unique words");
            LogUsages("Top Words", TopWords(wordUsages, 10));
            LogUsages("Thread Usages", threadUsages);
            
            return Task.CompletedTask;
        }

        async Task Async()
        {
            var content = LoadDraculaBook();

            var wordUsages = new Dictionary<string, int>();
            var threadUsages = new Dictionary<int, int>();
            await AsyncCountWords(wordUsages, threadUsages, AllWordsFrom(content));

            output.Log($"Stats: {wordUsages.Count} unique words");
            LogUsages("Top Words", TopWords(wordUsages, 10));
            LogUsages("Thread Usages", threadUsages);
        }

        Task ParallelForEach()
        {
            var content = LoadDraculaBook();

            var wordUsages = new Dictionary<string, int>();
            var threadUsages = new Dictionary<int, int>();
            ParallelCountWords(wordUsages, threadUsages, AllWordsFrom(content));

            output.Log($"Stats: {wordUsages.Count} unique words");
            LogUsages("Top Words", TopWords(wordUsages, 10));
            LogUsages("Thread Usages", threadUsages);

            return Task.CompletedTask;
        }

        static void CountWords(Dictionary<string, int> wordUsages, Dictionary<int, int> threadUsages, List<string> allWords)
        {
            allWords.ForEach(word =>
            {
                var lower = word.ToLowerInvariant();
                IncKeyUsage(wordUsages, lower);
                IncKeyUsage(threadUsages, Thread.CurrentThread.ManagedThreadId);
            });
        }

        static async Task AsyncCountWords(Dictionary<string, int> wordUsages,
            Dictionary<int, int> threadUsages, List<string> allWords)
        {
            var allTasks = allWords.Select(word =>
            {
                return Task.Run(() =>
                {
                    var lower = word.ToLowerInvariant();
                    lock (LockObj)
                    {
                        IncKeyUsage(wordUsages, lower);
                        IncKeyUsage(threadUsages, Thread.CurrentThread.ManagedThreadId);
                    }
                });
            });
            
            await Task.WhenAll(allTasks);
        }

        static void ParallelCountWords(Dictionary<string, int> wordUsages,
            Dictionary<int, int> threadUsages, List<string> allWords)
        {
            Parallel.ForEach(allWords, word =>
            {
                var lower = word.ToLowerInvariant();
                lock (LockObj)
                {
                    IncKeyUsage(wordUsages, lower);
                    IncKeyUsage(threadUsages, Thread.CurrentThread.ManagedThreadId);
                }
            });
        }

        static List<string> AllWordsFrom(string content)
        {
            var lines = content.Split("\r\n", StringSplitOptions.RemoveEmptyEntries);
            return lines.SelectMany(line => line.Split(Separators, StringSplitOptions.RemoveEmptyEntries)).ToList();
        }

        static void IncKeyUsage<TKey>(IDictionary<TKey, int> usageRegistry, TKey key)
        {
            if (!usageRegistry.ContainsKey(key))
                usageRegistry[key] = 0;
            usageRegistry[key]++;
        }

        void LogUsages<TKey>(string title, ICollection<KeyValuePair<TKey, int>> usages)
        {
            output.Log("");
            output.Log($"--------------------- {title} ({usages.Count})--------------------");
            foreach (var entry in usages)
            {
                output.Log($"{entry.Key} {entry.Value}");
            }
        }

        static List<KeyValuePair<string, int>> TopWords(Dictionary<string, int> wordUsages, int count)
        {
            return wordUsages.OrderByDescending(x => x.Value).Take(count).ToList();
        }

        static string LoadDraculaBook()
        {
            var content = File.ReadAllText("./dracula.txt");
            return content;
        }
    }
}