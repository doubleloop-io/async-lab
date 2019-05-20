using System.Threading.Tasks;
using AsyncLab.Support;

namespace AsyncLab
{
    public class CustomCombinators
    {
        readonly IOutput output;

        public CustomCombinators(IOutput output)
        {
            this.output = output;
        }

        Task<TInput> EchoAsync<TInput>(TInput input) => T.Create(output, 200, input)();
        Task<TInput> EchoAsync<TInput>(TInput input, int durationInMs) => T.Create(output, durationInMs, input)();

        /**
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
            
            await tasks.Interleaved(value =>
            {
                output.Log($"{value} consumed");
                return value;
            });
        }

        /**/
    }
    
    
}