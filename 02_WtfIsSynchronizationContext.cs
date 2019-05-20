using System.Threading.Tasks;
using AsyncLab.Support;
using Nito.AsyncEx;

namespace AsyncLab
{
    //[DoNotRun]
    public class WtfIsSynchronizationContext
    {
        readonly IOutput output;

        public WtfIsSynchronizationContext(IOutput output)
        {
            this.output = output;
        }
        
        Task<TInput> EchoAsync<TInput>(TInput input) => T.Create(output, 200, input)();
        
        /**
        [Run]
        public void LikeOnTheDesktop()
        {
            AsyncContext.Run(() =>
            {
                output.Log("Consider this like the UI Thread or ASP.NET http request");

                //Get everest weather and print the results
                
                // This must be done on the same thread as above
                // otherwise an exception is thrown
                output.Log("Is this the same Thread as above? It should be!");
            });
        }
        /**/

        async Task<string> WeatherAsync()
        {
            var content = await EchoAsync("+28°");
            return "Oh my " + content;
        }
    }
}