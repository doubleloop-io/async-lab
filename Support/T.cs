using System;
using System.Threading.Tasks;

namespace AsyncLab.Support
{
    public static class T
    {
        public const int GCDelay = 500;

        public static Func<Task<TInput>> Create<TInput>(IOutput output, int durationInMs, TInput input)
        {
            var name = input.ToString();
            if (name.StartsWith("faulty", StringComparison.OrdinalIgnoreCase))
                return CreateFaultyTask(output, durationInMs, input);
            else
                return CreateTask(output, durationInMs, input);
        }

        public static Func<Task<TInput>> CreateTask<TInput>(IOutput output, int durationInMs, TInput input)
        {
            return async () =>
            {
                output.Log($"{input} started");
                await Task.Delay(durationInMs).ConfigureAwait(false);
                output.Log($"{input} finished");

                return input;
            };
        }
        
        public static Func<Task<TOut>> CreateTask<TIn, TOut>(IOutput output, int durationInMs, TIn input, Func<TOut> fn)
        {
            return async () =>
            {
                output.Log($"{input} started");
                await Task.Delay(durationInMs).ConfigureAwait(false);
                output.Log($"{input} finished");

                return fn();
            };
        }
        
        public static Func<TIn, Task<TOut>> CreateTask<TIn, TOut>(IOutput output, int durationInMs, Func<TIn, TOut> fn)
        {
            return async (input) =>
            {
                output.Log($"{input} started");
                await Task.Delay(durationInMs).ConfigureAwait(false);
                output.Log($"{input} finished");

                return fn(input);
            };
        }
        
        public static Func<Task<TInput>> CreateFaultyTask<TInput>(IOutput output, int durationInMs, TInput input)
        {
            return async () =>
            {
                output.Log($"{input} started");
                await Task.Delay(durationInMs).ConfigureAwait(false);
                throw new InvalidOperationException(input.ToString());
            };
        }
        
        public static void RegisterUnobserved(IOutput output)
        {
            TaskScheduler.UnobservedTaskException += (sender, args) =>
            {
                output.Log("");
                output.Log("----- UNOBSERVED ------");
                output.Log(args.Exception.ToString());
                args.SetObserved();
                //Environment.Exit(1);
            };
        }

        public static void RegisterUnhandled(IOutput output)
        {
            AppDomain.CurrentDomain.UnhandledException += (sender, args) =>
            {
                output.Log("");
                output.Log("====== UNHANDLED ======");
                output.Log(args.ExceptionObject.ToString());
            };
        }


        public static async Task InvokeGC()
        {
            await Task.Delay(GCDelay);
            
            GC.Collect();
            GC.WaitForPendingFinalizers();
        }
    }
}