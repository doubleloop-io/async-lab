using Newtonsoft.Json;

namespace AsyncLab.Support
{
    public interface IOutput
    {
        void Log(string message);
        void Log(object obj);
        void Log(object obj, Formatting formatting);
    }
    
    public class Output : IOutput
    {
        public void Log(string message)
        {
            S.Log(message);
        }

        public void Log(object obj)
        {
            Log(obj, Formatting.Indented);
        }

        public void Log(object obj, Formatting formatting)
        {
            S.Log(obj, formatting);
        }
    }
}