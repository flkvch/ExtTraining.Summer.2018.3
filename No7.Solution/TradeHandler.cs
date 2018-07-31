using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;

namespace No7.Solution.Console
{
    public class TradeHandler
    {
        internal static float LotSize = 100000f;

        public void HandleTrades(Stream stream)
        {
            var readLines = ReadLines(stream);
            Trade trade = new Trade(readLines);           
            ISaver saver = new SaverToDb();
            saver.Save(trade);
            new SaverToDb().Save(trade);
            GetInfo(trade);
        }

        public void GetInfo(Trade trade)
        {
            foreach (var i in trade.errorList)
            {
                System.Console.WriteLine(i);
            }

            System.Console.WriteLine("INFO: {0} trades processed", trade.NumberOfTreads);
        }

        private IEnumerable<string> ReadLines(Stream stream)
        {
            var lines = new List<string>();
            using (var reader = new StreamReader(stream))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    lines.Add(line);
                }
            }

            return lines;
        }
    }
}
