using System;
using Xunit;
using Repository.Abstractions.Tests.TestHelpers;
using System.Data;
using System.Threading.Tasks;


namespace Repository.Abstractions.Tests
{
    public class SqliteRepositoryTests
    {
        private readonly SqliteRepository<TestEntity> _repository;

        public SqliteRepositoryTests()
        {
            _repository = new TestEntitySqliteRepository(null, "TestEntity");
        }

        [Fact]
        public void TestGetCommandForCreate()
        {
            var item = new TestEntity
            {
                BinaryData = { },
                Id = 1,
                Date = DateTime.Now,
                Name = "Name of item"
            };
            var actual = _repository.GetCommandTextForCreate();
            Assert.Equal("INSERT INTO TestEntity(Name,Date,BinaryData) VALUES(@Name,@Date,@BinaryData);SELECT last_insert_rowid();", actual);
        }
    }
}