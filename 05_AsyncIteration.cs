using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AsyncLab.Support;

namespace AsyncLab
{
    //[DoNotRun]
    public class AsyncIteration
    {
        readonly IOutput output;
        
        Task<TInput> EchoAsync<TInput>(TInput input) => T.Create(output, 200, input)();
        Func<TIn, Task<TOut>> AsAsync<TIn, TOut>(Func<TIn, TOut> fn) => T.CreateTask(output, 200, fn);

        
        public AsyncIteration(IOutput output)
        {
            this.output = output;
        }
        
        /**
        [Run]
        public void Sequential()
        {
            // Parse sequentially and print the results. Use AsAsync((string v) => int.Parse(v))
            // Can you do it with LINQ?
            
            var values = F.A("1", "2", "3", "4", "5");

            // Does this work?
            //var tasks = values.Select(async v => await EchoAsync(v));
            //var results = await Task.WhenAll(tasks);
        }

        /**
        [Run]
        public void Parallel()
        {
            // Parse in parallel and print the results
            
            var values = F.A("1", "2", "3", "4", "5");
        }

        /**
        [Run]
        public void Error()
        {
            // Echo in parallel and print the results.
            // You should not incur in UnobservedException
            // You should report all exception
            
            var values = F.A("1", "Faulty 2", "3", "4", "Faulty 5");
        }
        
        /**
        [Run]
        public void Mixed()
        {
            // Before doing this exercise you have to extract your Sequential and Parallel
            // logic as combinators. Create an AsyncExtensions class to host them
        
            // Parse in parallel and Increment by 1 sequentially using your new combinators
            
            var values = F.A("1", "2", "3", "4", "5");
        }
         
        /**/
    }
}