using System.Collections.Generic;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;

namespace No7.Solution.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            var tradeStream = Assembly.GetExecutingAssembly().GetManifestResourceStream("No7.Solution.Console.trades.txt");
            var tradeProcessor = new TradeService();
            tradeProcessor.WriteToDb(tradeProcessor.ReadFromStream(tradeStream));
        }
    }
}