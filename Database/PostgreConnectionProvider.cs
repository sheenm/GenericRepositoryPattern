using System.Data;
using Npgsql;

namespace GenericRepositoryPattern.Database
{
    public class PostgreConnectionProvider : IDbConnectionProvider
    {
        private string _connectionString;

        public PostgreConnectionProvider(string connectionString)
        {
            _connectionString = connectionString;
        }

        public IDbConnection GetDatabaseConnection()
        {
            return new NpgsqlConnection(_connectionString);
        }
    }
}