using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Security.Principal;
using System.Threading.Tasks;
using AsyncLab.Support;

namespace AsyncLab
{
    /**
     * Use EchoAsync(value) to invoke an async function that return the same value back 
     * Use output.Log(object|string) to pretty print results onto the console
     */
    
    //[DoNotRun]
    public class TaskComposition
    {
        readonly IOutput output;
        
        Task<TInput> EchoAsync<TInput>(TInput input) => T.Create(output, 200, input)();
        Task<TInput> EchoAsync<TInput>(TInput input, int durationInMs) => T.Create(output, durationInMs, input)();
        
        public TaskComposition(IOutput output)
        {
            this.output = output;
        }
        
        /**        
        [Run]
        public void Sequential()
        {
            // Run EchoAsync("1") then EchoAsync("2") (sequentially) and print the results
        }
        
        /**
        [Run]
        public void Parallel()
        {
            // Run EchoAsync("1"), EchoAsync("2") in parallel and print the results
        }

        /**
        [Run]
        public void Mixed()
        {
            // Run Echo("1"), Echo("2") then Echo("3") then Echo("4"), Echo("5") then print the results
        }
        
        /**/
    }
}