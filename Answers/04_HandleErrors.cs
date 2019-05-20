using System;
using System.Threading.Tasks;
using AsyncLab.Support;

namespace AsyncLab.Answers
{
    [DoNotRun]
    public class HandleErrors
    {
        readonly IOutput output;

        public HandleErrors(IOutput output)
        {
            this.output = output;
        }
        
        Task<TInput> FaultyAsync<TInput>(TInput input) => T.CreateFaultyTask(output, 200, input)();
        Task<TInput> FaultyAsync<TInput>(TInput input, int durationInMs) => T.CreateFaultyTask(output, durationInMs, input)();
        
        [Run]
        public async Task OneError()
        {
            // How do you handle an async function which throws an error?
            
            try
            {
                await FaultyAsync("F1");
            }
            catch (InvalidOperationException ex)
            {
                output.Log(ex.ToString());
            }
        }

        [Run]
        public async Task ManyErrors()
        {
            // How do you handle many async function where each one throws an error?
            // I want to log all the exceptions being thrown

            var f1 = FaultyAsync("F1");
            var f2 = FaultyAsync("F2");

            //await f1;
            //await f2; <-- this will be unobserved
            
            Task<string[]> all = null;
            try
            {
                //awaiting will marshal only the first exception
                //onto the calling thread. What happens to the f2's exception?
                all = Task.WhenAll(f1, f2);
                await all;
            }
            catch
            {
                
                output.Log(all.Exception.ToString());
            }
        }
    }
}