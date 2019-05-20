using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AsyncLab.Support;

namespace AsyncLab.Answers
{
    [DoNotRun]
    public class AsyncIteration
    {
        readonly IOutput output;
        
        Task<TInput> EchoAsync<TInput>(TInput input) => T.Create(output, 200, input)();
        Func<TIn, Task<TOut>> AsAsync<TIn, TOut>(Func<TIn, TOut> fn) => T.CreateTask(output, 200, fn);

        
        public AsyncIteration(IOutput output)
        {
            this.output = output;
        }
        
        [Run]
        public async Task Sequential()
        {
            // Parse sequentially and print the results. Use AsAsync((string v) => int.Parse(v))
            // Can you do it with LINQ?
            
            var values = F.A("1", "2", "3", "4", "5");

            // Does this work?
            //var tasks = values.Select(async v => await EchoAsync(v));
            //var results = await Task.WhenAll(tasks);
            
            var results = await AsyncExtensions.Sequential(values, AsAsync((string v) => int.Parse(v)));

            output.Log(results);
        }

        [Run]
        public async Task Parallel()
        {
            // Parse in parallel and print the results
            
            var values = F.A("1", "2", "3", "4", "5");
            
            var results = await AsyncExtensions.Parallel(values, AsAsync((string v) => int.Parse(v)));

            output.Log(results);
        }

        [Run]
        public async Task Error()
        {
            // Run in parallel and print the results.
            // You should not incur in UnobservedException
            // You should report all exception
            
            var values = F.A("1", "2", "3", "4", "5");
            
            var tasks = values.Select(v => EchoAsync(v)).ToList();

            Task<string[]> allTask = null;
            try
            {
                allTask = Task.WhenAll(tasks);
                var results = await allTask;
                output.Log(results);
            }
            catch
            {
                output.Log(allTask.Exception.ToString());
            }
        }
        
        [Run]
        public async Task Mixed()
        {
            // Before doing this exercise you have to extract your Sequential and Parallel
            // logic as combinators. Create an AsyncExtensions class to host them
        
            // Parse in parallel and Increment by 1 sequentially using your new combinators
            
            var values = F.A("1", "2", "3", "4", "5");

            var parsed = await AsyncExtensions.Parallel(values, AsAsync((string v) => int.Parse(v)));
            var incremented = await AsyncExtensions.Sequential(parsed, AsAsync((int v) => v + 1));

            output.Log(incremented);
        }
        static class AsyncExtensions
        {
            public static async Task<TOut[]> Sequential<TIn, TOut>(TIn[] values, Func<TIn, Task<TOut>> func)
            {
                var results = new List<TOut>();
                foreach (var value in values)
                {
                    var newValue = await func(value);
                    results.Add(newValue);
                }

                return results.ToArray();
            }

            public static async Task<TOut[]> Parallel<TIn, TOut>(TIn[] values, Func<TIn, Task<TOut>> func)
            {
                var tasks = values.Select(v => func(v));
                return await Task.WhenAll(tasks);
            }
        }    
    }

    
}