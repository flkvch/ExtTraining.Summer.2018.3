using System.Configuration;
using System.Data.SqlClient;

namespace No7.Solution.Console
{
    public class SaverToDb : ISaver
    {
        public void Save(Trade trades)
        {
            // save into database
            string connectionString = ConfigurationManager.ConnectionStrings["TradeData"].ConnectionString;
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    foreach (var trade in trades.trades)
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