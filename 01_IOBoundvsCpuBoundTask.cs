using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using AsyncLab.Support;

namespace AsyncLab
{
    //[DoNotRun]
    public class IOBoundVsCpuBoundTask
    {
        readonly IOutput output;

        public IOBoundVsCpuBoundTask(IOutput output)
        {
            this.output = output;
        }
        
        /**        
        [Run]
        public async Task IOBound()
        {
            // On IO task you can only await

            var weatherUrl = "http://wttr.in/~everest?format=3";
        }
        
        /**
        [Run]
        public void CpuBound()
        {
            // This is blocking the calling thread. Is it a problem?
            // It depends on the caller...
            var result = FindPrimeNumbers(100, 10000000);

            output.Log("I'm blocked, I've to wait for FindPrimeNumbers() to finish...");

            output.Log($"Found {result.Length} prime numbers");
        }
        
        /**
        [Run]
        public void IOBoundDoneWrong()
        {
            // Stop stealing threads from the ThreadPool!
            // You're even paying a context switch    
        }
        /**/

        int[] FindPrimeNumbers(int begin, int end)
        {
            output.Log("FindPrimeNumber is running");
            
            IList<int> primeNumbers = new List<int>();
            for (int i = begin; i <= end; i++)
            {
                if (IsPrime(i)) primeNumbers.Add(i);
            }

            return primeNumbers.ToArray();
        }

        static bool IsPrime(int number)
        {
            if (number <= 1) return false;
            if (number == 2) return true;
            if (number % 2 == 0) return false;

            var boundary = (int) Math.Floor(Math.Sqrt(number));

            for (int i = 3; i <= boundary; i += 2)
                if (number % i == 0)
                    return false;

            return true;
        }
        
    }
}