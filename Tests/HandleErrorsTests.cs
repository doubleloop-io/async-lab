using System;
using System.Threading.Tasks;
using AsyncLab.Support;
using AsyncLab.Tests.Support;
using Xunit;

namespace AsyncLab.Tests
{
    public class HandleErrorsTests
    {
        readonly OutputSpy output = new OutputSpy();

        public HandleErrorsTests()
        {
            T.RegisterUnobserved(output);
        }
        
        Task<TInput> FaultyAsync<TInput>(TInput input) => T.CreateFaultyTask(output, 200, input)();
        
        [Fact]
        public async Task ManyErrors()
        {
            // This pass...
            
            await Exec(async () =>
            {
                try
                {
                    var t1 = FaultyAsync("F1");
                    var t2 = FaultyAsync("F2");
                    await t1;
                    await t2;
                }
                catch (Exception e)
                {
                    output.Log(e.ToString());
                }    
            });
            
            await T.InvokeGC();

            Assert.Contains("UNOBSERVED", output.Text);
        }
        
        [Fact]
        public async Task ManyErrorsPuzzle()
        {
            // ...but this not! Why does it fail?
            try
            {
                var t1 = FaultyAsync("F1");
                var t2 = FaultyAsync("F2");
                await t1;
                await t2;
            }
            catch (Exception e)
            {
                output.Log(e.ToString());
            }

            await T.InvokeGC();
            
            Assert.Contains("UNOBSERVED", output.Text);
        }

        async Task Exec(Func<Task> action)
        {
            await action();
        }
    }
}