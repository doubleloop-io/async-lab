using System.Threading.Tasks;
using AsyncLab.Support;
using Nito.AsyncEx;

namespace AsyncLab.Answers
{
    [DoNotRun]
    public class WtfIsSynchronizationContext
    {
        readonly IOutput output;

        public WtfIsSynchronizationContext(IOutput output)
        {
            this.output = output;
        }
        
        Task<TInput> EchoAsync<TInput>(TInput input) => T.Create(output, 200, input)();
        
        [Run]
        public void LikeOnTheDesktop()
        {
            AsyncContext.Run(async () =>
            {
                output.Log("Consider this like the UI Thread or ASP.NET http request");

                //var result = WeatherAsync().Result; // don't block async calls...
                var result = await WeatherAsync();    // ...await them!
                
                // This should be done exactly on the same Thread
                output.Log("Is this the same Thread as above? It should be!");
                output.Log(result);
            });
        }

        async Task<string> WeatherAsync()
        {
            var content = await EchoAsync("+28°");
            return "Oh my " + content;
        }
    }
}