using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using NLog;

namespace No7.Solution.Console
{
    public class TradeHandler
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        internal static float LotSize = 100000f;

        public void HandleTrades(Stream stream)
        {
            IRepository trade = new Trade();
            foreach (var line in ReadLines(stream))
            {
                var fields = line.Split(new char[] { ',' });
                trade.Create(fields);
            }

            logger.Info("INFO: {0} trades processed", trade.NumberOfTreads);
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
