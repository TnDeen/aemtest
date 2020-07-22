using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace aemtest.Services
{
    public class DataConnector : IDataConnector
    {
        private readonly IConfiguration _configuration;

        public DataConnector(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public IEnumerable<T> Query<T>(CommandType cmdType, string cmdText) where T : class
        {
            var result = default(IEnumerable<T>);
            var connectionString = _configuration.GetConnectionString("DefaultConnection");
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        var command = CreateCommandDefinition(cmdType, cmdText, transaction, 100);
                        result = connection.Query<T>(command);
                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw ex;
                    }
                }
                connection.Close();
            }

            return result;
        }

        private CommandDefinition CreateCommandDefinition(CommandType cmdType, string cmdText, IDbTransaction transaction, int timeout)
        {
            return new CommandDefinition(commandText: cmdText, commandType: cmdType, transaction: transaction, commandTimeout: timeout);
        }
    }
}
