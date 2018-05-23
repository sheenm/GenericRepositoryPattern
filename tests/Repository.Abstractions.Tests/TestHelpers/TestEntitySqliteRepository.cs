using System.Data;
using Repository.Abstractions;

namespace Repository.Abstractions.Tests.TestHelpers
{
    public class TestEntitySqliteRepository : SqliteRepository<TestEntity>
    {
        public TestEntitySqliteRepository(IDbConnectionProvider dbProvider, string tableName) : base(dbProvider, tableName)
        {
        }

        protected override TestEntity PopulateRecord(IDataReader reader)
        {
            throw new System.NotImplementedException();
        }
    }
}