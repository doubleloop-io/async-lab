using System;
using System.Threading.Tasks;
using AsyncLab.Support;

namespace AsyncLab
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("Welcome to AsyncLab");
            
            var output = new Output();
            T.RegisterUnhandled(output);
            T.RegisterUnobserved(output);

            var all = RunnableMethod.FindAll();
            await new Runnables(output, all).Run(args);
        }
    }
}