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
            IRepository trade = new Trade();
            foreach (var line in ReadLines(stream))
            {
                var fields = line.Split(new char[] { ',' });
                trade.Create(fields);
            }

            trade.GetInfo();
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
