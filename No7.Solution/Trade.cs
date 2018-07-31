using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Net.Http;
using System.Xml.Schema;

namespace No7.Solution.Console
{
    public class Trade
    {
        internal int NumberOfTreads { get; }
        internal List<TradeRecord> trades = new List<TradeRecord>();
        internal List<string> errorList = new List<string>();

        public Trade(IEnumerable<string> lines)
        {
            NumberOfTreads = 1;

            foreach (var line in lines)
            {
                var fields = line.Split(new char[] { ',' });
               
                try
                {
                    ValidFields(fields);
                }
                catch (FieldLengthException)
                {
                    errorList.Add($"WARN: Trade length on line {NumberOfTreads} is: '{fields.Length}'");
                    continue;
                }
                catch (CurrencyException)
                {
                    errorList.Add($"WARN: Trade currencies on line {NumberOfTreads} malformed: '{fields[0]}'");
                    continue;
                }
                catch (TradeAmountException)
                {
                    errorList.Add($"WARN: Trade amount on line {NumberOfTreads} not a valid integer: '{fields[1]}'");
                    continue;
                }

                catch (TradePriceException)
                {
                    errorList.Add($"WARN: Trade price on line {NumberOfTreads} not a valid decimal: '{fields[2]}'");
                    continue;
                }

                var tradeAmount = int.Parse(fields[1]);
                //changed to try parse with culture info
                var tradePrice = decimal.Parse(fields[2], NumberStyles.Number, new CultureInfo("en-US"));

                var sourceCurrencyCode = fields[0].Substring(0, 3);
                var destinationCurrencyCode = fields[0].Substring(3, 3);

                var trade = new TradeRecord
                {
                    SourceCurrency = sourceCurrencyCode,
                    DestinationCurrency = destinationCurrencyCode,
                    Lots = tradeAmount / TradeHandler.LotSize,
                    Price = tradePrice
                };

                trades.Add(trade);

                NumberOfTreads++;
            }
        }

        private void ValidFields(string[] fields)
        {
            if (fields.Length != 3)
            {
                throw new FieldLengthException();               
            }

            if (fields[0].Length != 6)
            {
                throw new CurrencyException();
            }

            if (!int.TryParse(fields[1], out var t))
            {
                throw new TradeAmountException();
            }

            if (!decimal.TryParse(fields[2], NumberStyles.Number, new CultureInfo("en-US"), out var tradePrice))
            {
                throw new TradePriceException();
            }

        }

    }

}