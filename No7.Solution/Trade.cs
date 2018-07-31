using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Net.Http;
using System.Xml.Schema;

namespace No7.Solution.Console
{
    public class Trade : IRepository
    {
        internal int NumberOfTreads { get; private set; }
        internal List<TradeRecord> trades;
        internal List<string> errorList = new List<string>();

        public Trade()
        {
            trades = new List<TradeRecord>();
        }

        public void Create(string[] fields)
        {           
            try
            {
                ValidFields(fields);
            }
            catch (FieldLengthException)
            {
                errorList.Add($"WARN: Trade length on line {NumberOfTreads} is: '{fields.Length}'");
                return;
            }
            catch (CurrencyException)
            {
                errorList.Add($"WARN: Trade currencies on line {NumberOfTreads} malformed: '{fields[0]}'");
                return;
            }
            catch (TradeAmountException)
            {
                errorList.Add($"WARN: Trade amount on line {NumberOfTreads} not a valid integer: '{fields[1]}'");
                return;
            }

            catch (TradePriceException)
            {
                errorList.Add($"WARN: Trade price on line {NumberOfTreads} not a valid decimal: '{fields[2]}'");
                return;
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

        public void Save()
        {
            // save into database
            string connectionString = ConfigurationManager.ConnectionStrings["TradeData"].ConnectionString;
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    foreach (var trade in trades)
                    {
                        var command = connection.CreateCommand();
                        command.Transaction = transaction;
                        command.CommandType = System.Data.CommandType.StoredProcedure;
                        command.CommandText = "dbo.Insert_Trade";
                        command.Parameters.AddWithValue("@sourceCurrency", trade.SourceCurrency);
                        command.Parameters.AddWithValue("@destinationCurrency", trade.DestinationCurrency);
                        command.Parameters.AddWithValue("@lots", trade.Lots);
                        command.Parameters.AddWithValue("@price", trade.Price);

                        command.ExecuteNonQuery();
                    }

                    transaction.Commit();
                }
                connection.Close();
            }
        }

    }

}