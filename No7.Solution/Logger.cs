using No7.Solution.Interfaces;
using System;

namespace No7.Solution
{
    internal class Logger : ILogger 
    {       
        public void Info(string message)
        {
            Console.WriteLine("Info:" + message);
        }

        public void Warn(string message)
        {
            Console.WriteLine("Warn:" + message);
        }
    }
}
