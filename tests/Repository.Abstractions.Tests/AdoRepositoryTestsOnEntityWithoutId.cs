using System;
using Xunit;
using Repository.Abstractions.Tests.TestHelpers;
using System.Data;
using Repository.Abstractions.Tests.TestHelpers.Database;
using System.Threading.Tasks;
using Microsoft.Data.Sqlite;

namespace Repository.Abstractions.Tests
{
    public class AdoRepositoryTestsOnEntityWithoutId : AdoRepository<TestEntityWithoutId>
    {
        public AdoRepositoryTestsOnEntityWithoutId() : base(null, "TestEntity")
        {
        }

        [Fact]
        public void GetId_EntityWithoutID_ShouldThrowArgumentException()
        {
            var item = new TestEntityWithoutId();
            Assert.Throws<ArgumentException>(() => GetId(item));
        }

        protected override TestEntityWithoutId PopulateRecord(IDataReader reader)
        {
            throw new NotImplementedException();
        }
    }
}