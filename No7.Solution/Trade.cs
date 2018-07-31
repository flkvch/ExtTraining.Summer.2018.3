using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Globalization;
using NLog;

namespace No7.Solution.Console
{
    internal class Trade : IRepository
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();
        private readonly string connectionString;

        public Trade()
        {
            connectionString = ConfigurationManager.ConnectionStrings["TradeData"].ConnectionString;
        }

        public int NumberOfTreads { get; private set; }

        public void Create(string[] fields)
        {           
            try
            {
                ValidFields(fields);
            }
            catch (FieldLengthException)
            {
                logger.Warn("WARN: Trade length on line {0} is: '{1}'", NumberOfTreads, fields.Length);
                return;
            }
            catch (CurrencyException)
            {
                logger.Warn("WARN: Trade currencies on line {0} malformed: '{1}'", NumberOfTreads, fields[0]);
                return;
            }
            catch (TradeAmountException)
            {
                logger.Warn("WARN: Trade amount on line {0} not a valid integer: '{1}'", NumberOfTreads, fields[1]);
                return;
            }

            catch (TradePriceException)
            {
                logger.Warn("WARN: Trade price on line {0} not a valid decimal: '{1}'", NumberOfTreads, fields[2]);
                return;
            }

            var tradeAmount = int.Parse(fields[1]);
            //changed to try parse with culture info
            var tradePrice = decimal.Parse(fields[2], NumberStyles.Number, new CultureInfo("en-US"));

            var trade = new TradeRecord
            {
                SourceCurrency = fields[0].Substring(0, 3),
                DestinationCurrency = fields[0].Substring(3, 3),
                Lots = tradeAmount / TradeHandler.LotSize,
                Price = tradePrice
            };

            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
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
                    transaction.Commit();
                }
                connection.Close();
            }

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
    }

}