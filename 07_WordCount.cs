using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AsyncLab.Support;

namespace AsyncLab
{
    //[DoNotRun]
    public class WordCount
    {
        readonly IOutput output;
        static readonly char[] Separators = new[] {' ', ',', '.', '-', ';', ':', '"', '*'};

        public WordCount(IOutput output)
        {
            this.output = output;
        }

        /**
        [Run]
        public Task Sync()
        {
            var content = LoadDraculaBook();

            var wordUsages = new Dictionary<string, int>();
            var threadUsages = new Dictionary<int, int>();
            
            //SyncCountWords(wordUsages, threadUsages, AllWordsFrom(content));

            output.Log($"Stats: {wordUsages.Count} unique words");
            LogUsages("Top Words", wordUsages.OrderByDescending(x => x.Value).Take(10).ToList());
            LogUsages("Thread Usages", threadUsages);
            return Task.CompletedTask;
        }
        
        /**
        [Run]
        public Task Async()
        {
            var content = LoadDraculaBook();

            var wordUsages = new Dictionary<string, int>();
            var threadUsages = new Dictionary<int, int>();
            
            //await AsyncCountWords(wordUsages, threadUsages, AllWordsFrom(content));

            output.Log($"Stats: {wordUsages.Count} unique words");
            LogUsages("Top Words", wordUsages.OrderByDescending(x => x.Value).Take(10).ToList());
            LogUsages("Thread Usages", threadUsages);
            return Task.CompletedTask;
        }
        
        /**
        [Run]
        public Task ParallelForEach()
        {
            var content = LoadDraculaBook();

            var wordUsages = new Dictionary<string, int>();
            var threadUsages = new Dictionary<int, int>();
            
            //ParallelCountWords(wordUsages, threadUsages, AllWordsFrom(content));

            output.Log($"Stats: {wordUsages.Count} unique words");
            LogUsages("Top Words", wordUsages.OrderByDescending(x => x.Value).Take(10).ToList());
            LogUsages("Thread Usages", threadUsages);
            return Task.CompletedTask;
        }
        /**/

        void LogUsages<TKey>(string title, ICollection<KeyValuePair<TKey, int>> usages)
        {
            output.Log("");
            output.Log($"--------------------- {title} ({usages.Count})--------------------");
            foreach (var entry in usages)
            {
                output.Log($"{entry.Key} {entry.Value}");
            }
        }

        static string LoadDraculaBook()
        {
            var content = File.ReadAllText("./dracula.txt");
            return content;
        }
    }
}