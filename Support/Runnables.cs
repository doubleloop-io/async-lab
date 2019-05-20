using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AsyncLab.Support
{
    public class Runnables
    {
        readonly IOutput output;
        readonly ICollection<RunnableMethod> runnableMethods;

        public Runnables(IOutput output, ICollection<RunnableMethod> runnableMethods)
        {
            this.runnableMethods = runnableMethods;
            this.output = output;
        }

        public async Task Run()
        {
            foreach (var runnable in runnableMethods)
            {
                Header(runnable);

                try
                {
                    await runnable.Invoke();
                }
                catch (Exception e)
                {
                    output.Log("");
                    output.Log($"******** Main Try-Catch ********\n{e}");
                }
                
                //Give a chance to the finalizer thread to dispose all tasks
                await T.InvokeGC();
                
                Footer();
            }
        }

        static void Header(RunnableMethod runnable)
        {
            Console.WriteLine($"================= {runnable.Name}() ==================");
            Console.WriteLine();
            S.StartTheWatch();
        }

        static void Footer()
        {
            Console.WriteLine();
            S.StopTheWatch();
            Console.Write(Environment.NewLine + Environment.NewLine);
        }
    }
}