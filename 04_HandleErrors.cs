using System;
using System.Linq;
using System.Threading.Tasks;
using AsyncLab.Support;

namespace AsyncLab
{
    //[DoNotRun]
    public class HandleErrors
    {
        readonly IOutput output;
        
        Task<TInput> FaultyAsync<TInput>(TInput input) => T.CreateFaultyTask(output, 200, input)();
        Task<TInput> FaultyAsync<TInput>(TInput input, int durationInMs) => T.CreateFaultyTask(output, durationInMs, input)();

        public HandleErrors(IOutput output)
        {
            this.output = output;
        }

        /**
        [Run]
        public void OneError()
        {
            // How do you handle an async function which throws an error?
            
            FaultyAsync("F1");
        }

        /**
        [Run]
        public async Task ManyErrors()
        {
            // How do you handle many async function where each one throws an error?
            // I want to log all the exceptions being thrown
        
            var f1 = FaultyAsync("F1");
            var f2 = FaultyAsync("F2");
        }        
        /**/
    }
}