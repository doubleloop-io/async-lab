using System;
using AsyncLab.Support;
using Newtonsoft.Json;

namespace AsyncLab.Tests.Support
{
    public class OutputSpy : IOutput
    {
        private string text = Environment.NewLine;

        public string Text => text;

        public void Log(string message)
        {
            text += S.Format(message) + Environment.NewLine;
        }

        public void Log(object obj)
        {
            Log(obj, Formatting.Indented);
        }

        public void Log(object obj, Formatting formatting)
        {
            Log(obj.ToJson(formatting));
        }
    }
}