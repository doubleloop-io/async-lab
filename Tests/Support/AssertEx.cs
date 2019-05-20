using System;
using System.Linq;
using Xunit.Sdk;

namespace AsyncLab.Tests.Support
{
    public static class AssertEx
    {
        public static void ContainsInOrder(string[] values, string text)
        {
            var foundValues = values
                .Select(v => new {Value = v, Index = text.IndexOf(v)})
                .OrderBy(x => x.Index)
                .Select(x => x.Value).ToArray();
            
            if (!values.SequenceEqual(foundValues))
            {
                throw new AssertActualExpectedException(values, foundValues, BuildMessage(values, text));
            }
            //Assert.True(values.SequenceEqual(foundValues), BuildMessage(values, text));
        }

        private static string BuildMessage(string[] values, string text)
        {
            var msgValues = String.Join(", ", values);
            return $"[{msgValues}] should appear in order inside:" + Environment.NewLine + text;
        }
    }
}