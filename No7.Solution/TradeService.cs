using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using No7.Solution.Exceptions;
using No7.Solution.Interfaces;

namespace No7.Solution
{
    public class TradeService
    {
        internal static float LotSize = 100000f;

        public IEnumerable<string> ReadFromStream(Stream stream)
        {
            IDataProvider dataProvider = new DataProvider();
            return dataProvider.ProvideData(stream);
        }

        public void WriteToDb(IEnumerable<string> tradeRecords)
        {
            IStore store = new Storage();
            store.Save(Parse(tradeRecords));
        }

        private IEnumerable<TradeRecord> Parse(IEnumerable<string> tradeRecords)
        {
            int NumberOfTreads = 0;
            List<TradeRecord> buf = new List<TradeRecord>();
            IParser parser = new Parser();
            ILogger logger = new Logger();

            foreach (var line in tradeRecords)
            {
                string[] fields = line.Split(new char[] { ',' });
                TradeRecord tradeRecord = null;
                try
                {
                    tradeRecord = parser.Parse(fields);
                }
                catch (FieldLengthException)
                {
                    logger.Warn($"WARN: Trade length on line {NumberOfTreads} is: '{fields.Length}'");
                    continue;
                }
                catch (CurrencyException)
                {
                    logger.Warn($"WARN: Trade currencies on line {NumberOfTreads} malformed: '{fields[0]}'");
                    continue;
                }
                catch (TradeAmountException)
                {
                    logger.Warn($"WARN: Trade amount on line {NumberOfTreads} not a valid integer: '{fields[1]}'");
                    continue;
                }
                catch (TradePriceException)
                {
                    logger.Warn($"WARN: Trade price on line {NumberOfTreads} not a valid decimal: '{fields[2]}'");
                    continue;
                }

                buf.Add(tradeRecord);
                NumberOfTreads++;
            }

            logger.Info($"INFO: {NumberOfTreads} trades processed");
            return buf;
        }
    }
}
