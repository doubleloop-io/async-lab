using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AsyncLab.Support;

namespace AsyncLab.Answers
{
    [DoNotRun]
    public class TaskCombinators
    {
        IOutput output;

        public TaskCombinators(IOutput output)
        {
            this.output = output;
        }

        Task<TInput> EchoAsync<TInput>(TInput input) => T.Create(output, 200, input)();
        Task<TInput> EchoAsync<TInput>(TInput input, int durationInMs) => T.Create(output, durationInMs, input)();
        Task<string> EchoAsync(int durationInMs) => T.Create(output, durationInMs, $"Task {durationInMs}")();
        Task<TOut> DoAsync<TIn, TOut>(TIn input, Func<TOut> fn) => T.CreateTask(output, 200, input, fn)();
        Func<TIn, Task<TOut>> AsAsync<TIn, TOut>(Func<TIn, TOut> fn) => T.CreateTask(output, 200, fn);

        [Run]
        public async Task Sequential()
        {
            var inputs = F.A("1", "2", "3", "4", "5");

            var results = await inputs.Sequential(AsAsync((string v) => int.Parse(v)));
            //var results = await inputs.Sequential(input => DoAsync(input, () => int.Parse(input) + 1));

            output.Log(results);
        }
        
        /*
        [Run]
        public async Task Parallel()
        {
            var inputs = F.A("1", "2", "3", "4", "5");

            var results = await inputs.Parallel(input => EchoAsync(input));

            output.Log(results);
        }

        [Run]
        public async Task Mixed()
        {
            var inputs = F.A("1", "2", "3", "4", "5");

            var parsed = await inputs.Parallel(input => DoAsync(input, () => int.Parse(input)));
            var incremented = await parsed.Sequential(input => DoAsync(input, () => input + 1));

            output.Log(incremented);
        }
        
        /**/
    }

    public static class TplCombinatorsExtensions
    {
        public static async Task<TOut[]> Parallel<TIn, TOut>(this IEnumerable<TIn> actual, Func<TIn, Task<TOut>> fn)
        {
            var tasks = actual.Select(fn);

            return await Task.WhenAll(tasks);
        }

        public static async Task<IEnumerable<TOut>> Sequential<TIn, TOut>(this IEnumerable<TIn> inputs,
            Func<TIn, Task<TOut>> fn)
        {
            var results = new List<TOut>();
            foreach (var input in inputs)
            {
                results.Add(await fn(input));
            }

            return results;
        }
    }
}