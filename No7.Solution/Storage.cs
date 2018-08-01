using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Globalization;
using NLog;
using No7.Solution.Interfaces;

namespace No7.Solution
{
    internal class Storage : IStore
    {
        private readonly string connectionString;

        public Storage()
        {
            connectionString = ConfigurationManager.ConnectionStrings["TradeData"].ConnectionString;
        }

        public int NumberOfTreads { get; private set; }

        public void Save(IEnumerable<TradeRecord> trades)
        {                    
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