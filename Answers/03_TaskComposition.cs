using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AsyncLab.Support;

namespace AsyncLab.Answers
{
    /**
     * Use output.Log(object) to pretty print results onto the console
     */
    [DoNotRun]
    public class TaskComposition
    {
        readonly IOutput output;
        
        Task<TInput> EchoAsync<TInput>(TInput input) => T.Create(output, 200, input)();
        Task<TInput> EchoAsync<TInput>(TInput input, int durationInMs) => T.Create(output, durationInMs, input)();
        
        public TaskComposition(IOutput output)
        {
            this.output = output;
        }
        
        [Run]
        public async Task Sequential()
        {
            // Run EchoAsync("1") then EchoAsync("2") (sequentially) and print the results
            
            var res1 = await EchoAsync("1");
            var res2 = await EchoAsync("2");
            
            output.Log(res1);
            output.Log(res2);
        }
        
        [Run]
        public async Task Parallel()
        {
            // Run EchoAsync("1"), EchoAsync("2") in parallel and print the results
            
            var echo1 = EchoAsync("1");
            var echo2 = EchoAsync("2");

            //don't do this
            //await echo1
            //await echo2
            var results = await Task.WhenAll(echo1, echo2);
            
            output.Log(results);
        }

        [Run]
        public async Task Mixed()
        {
            // Run Echo("1"), Echo("2") then Echo("3") then Echo("4"), Echo("5")
            
            var echo1 = EchoAsync("1");
            var echo2 = EchoAsync("2");
            var oneAndTwo = await Task.WhenAll(echo1, echo2);

            var three = await EchoAsync("3");
            
            var fourTask = EchoAsync("4");
            var fiveTask = EchoAsync("5");
            var fourAndFive = await Task.WhenAll(fourTask, fiveTask);
            
            output.Log(F.A(oneAndTwo, F.A(three), fourAndFive));
        }
    }
}