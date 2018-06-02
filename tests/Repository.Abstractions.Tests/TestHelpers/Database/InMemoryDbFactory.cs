using System.Data.Common;
using Microsoft.Data.Sqlite;

namespace Repository.Abstractions.Tests.TestHelpers.Database
{
    public class InMemoryDbFactory : IDbConnectionFactory
    {
        public DbConnection CreateConnection()
        {

            var connection = new SqliteConnection("Data Source=:memory:");

            var command = connection.CreateCommand();
            command.CommandText = "CREATE TABLE TestEntity (Id int, Name varchar(250), Date datetime, BinaryData binary NULL)";
            connection.Open();
            command.ExecuteNonQuery();
            connection.Close();

            return connection;
        }
    }
}