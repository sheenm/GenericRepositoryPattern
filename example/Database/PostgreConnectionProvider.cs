using System;
using System.Data;
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

        public IDbConnection GetDatabaseConnection()
        {
            return new NpgsqlConnection(_connectionString);
        }
    }
}