using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AsyncLab.Support;

namespace AsyncLab.Answers
{
    [DoNotRun]
    public class CustomCombinators
    {
        IOutput output;

        public CustomCombinators(IOutput output)
        {
            this.output = output;
        }

        Task<TInput> EchoAsync<TInput>(TInput input) => T.Create(output, 200, input)();
        Task<TInput> EchoAsync<TInput>(TInput input, int durationInMs) => T.Create(output, durationInMs, input)();

        [Run]
        public async Task InterleavedTasks()
        {
            // Build a combinator which enable to consume task in order of completion
            // In this case consume == output.Log() and you should see the results
            // printed in order [1, 2, 3, 4, 5]
            
            var tasks = new[] {  
                EchoAsync(3, 3000), 
                EchoAsync(1, 1000), 
                EchoAsync(2, 2000), 
                EchoAsync(5, 5000), 
                EchoAsync(4, 4000), 
            };
            
            await CombinatorAsyncExtensions.Interleaved(tasks, value =>
            {
                output.Log($"{value} consumed");
                return value;
            });
        }
    }
    
    static class CombinatorAsyncExtensions
    {
        public static async Task<TOut[]> Interleaved<TIn, TOut>(Task<TIn>[] tasks, Func<TIn, TOut> func)
        {
            var tasksCopy = tasks.ToImmutableList();
            var results = new List<TOut>();

            while (tasksCopy.Any())
            {
                var nextCompleted = await Task.WhenAny(tasksCopy);
                
                tasksCopy = tasksCopy.Remove(nextCompleted);
                var result = func(nextCompleted.Result);
                results.Add(result);
            }

            return results.ToArray();
        }
    }
}