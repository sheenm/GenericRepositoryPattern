using System.Data;

namespace Repository.Abstractions.Tests.TestHelpers
{
    public class TestEntityWithoutIdRepository : AdoRepository<TestEntityWithoutId>
    {
        public TestEntityWithoutIdRepository(IDbConnectionProvider dbProvider, string tableName) : base(dbProvider, tableName)
        {
        }

        protected override TestEntityWithoutId PopulateRecord(IDataReader reader)
        {
            throw new System.NotImplementedException();
        }
    }
}