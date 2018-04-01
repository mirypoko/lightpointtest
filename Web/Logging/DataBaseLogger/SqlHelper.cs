using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Domain.DataBaseModels.Logging;

namespace Web.Logging.DataBaseLogger
{
    class SqlHelper
    {
        private string ConnectionString { get; }

        public SqlHelper(string connectionStr)
        {
            ConnectionString = connectionStr;
        }

        private bool ExecuteNonQuery(string commandStr, List<SqlParameter> paramList)
        {
            bool result;
            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                if (conn.State != System.Data.ConnectionState.Open)
                {
                    try
                    {
                        conn.Open();
                    }
                    catch (Exception ex)
                    {
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.Write("Warning:");
                        Console.ResetColor();
                        Console.WriteLine($"Could not connect to the database for logging. {ex.Message}");
                        return false;
                    }
                }

                using (SqlCommand command = new SqlCommand(commandStr, conn))
                {
                    command.Parameters.AddRange(paramList.ToArray());
                    int count = command.ExecuteNonQuery();
                    result = count > 0;
                }
            }
            return result;
        }

        public bool InsertLog(ServerEvent log)
        {
            string command = @"INSERT INTO [dbo].[ServerLog] ([EventID],[LogLevel],[Message],[Time]) VALUES (@EventID, @LogLevel, @Message, @Time)";
            List<SqlParameter> paramList = new List<SqlParameter>
            {
                new SqlParameter("EventID", log.EventId),
                new SqlParameter("LogLevel", log.LogLevel),
                new SqlParameter("Message", log.Message),
                new SqlParameter("Time", log.Time)
            };
            return ExecuteNonQuery(command, paramList);
        }
    }
}
