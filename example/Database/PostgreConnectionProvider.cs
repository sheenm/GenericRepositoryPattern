using System.Data.Common;
using Npgsql;
using Repository.Abstractions;

namespace GenericRepositoryPattern.Database
{
    public class PostgreConnectionProvider : IDbConnectionProvider
    {
        private string _connectionString;

        public PostgreConnectionProvider(string connectionString)
        {
            _connectionString = connectionString;
        }

        public DbConnection GetDatabaseConnection()
        {
            return new NpgsqlConnection(_connectionString);
        }
    }
}