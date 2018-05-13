using System.Data;
using Repository.Abstractions;

namespace Repository.Abstractions.Tests.TestHelpers
{
    public class TestEntityRepository : AdoRepository<TestEntity>
    {
        public TestEntityRepository(IDbConnectionProvider dbProvider, string tableName) : base(dbProvider, tableName)
        {
        }

        protected override TestEntity PopulateRecord(IDataReader reader)
        {
            throw new System.NotImplementedException();
        }
    }
}