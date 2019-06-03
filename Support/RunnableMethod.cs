using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace AsyncLab.Support
{
    public class RunnableMethod
    {
        readonly Type type;
        readonly MethodInfo method;

        public RunnableMethod(Type type, MethodInfo method)
        {
            this.type = type;
            this.method = method;
        }

        public string Name
        {
            get { return $"{type.Name}.{method.Name}"; }
        }

        public Task Invoke(string[] args)
        {
            var argsToPass = method.GetParameters().Length > 0
                ? new object[] {args}
                : null;
            
            var obj = Activator.CreateInstance(type, new Output());
            var result = method.Invoke(obj, argsToPass);
            
            if (method.ReturnType == typeof(Task))
                return (Task) result;
            else
                return Task.CompletedTask;
        }

        public static ICollection<RunnableMethod> FindAll()
        {
            return Assembly.GetExecutingAssembly()
                .GetTypes()
                .SelectMany(GetRunnableMethods)
                .Select(x => new RunnableMethod(x.Item1, x.Item2)).ToList();
        }

        static IList<(Type, MethodInfo)> GetRunnableMethods(Type type)
        {
            if (type.GetCustomAttributes<DoNotRunAttribute>().Any()) return new List<(Type, MethodInfo)>();
            
            return type.GetMethods()
                .Where(IsRunnable)
                .Select(m => (Type: type, Method: m))
                .ToList();
        }

        static bool IsRunnable(MethodInfo arg)
        {
            return arg.GetCustomAttributes<RunAttribute>().Any();
        }
    }
}