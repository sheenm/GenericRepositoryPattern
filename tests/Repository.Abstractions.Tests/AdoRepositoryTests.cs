using System;
using Xunit;
using Repository.Abstractions.Tests.TestHelpers;
using System.Data;
using Repository.Abstractions.Tests.TestHelpers.Database;
using System.Threading.Tasks;
using Microsoft.Data.Sqlite;
using System.Data.SqlClient;

namespace Repository.Abstractions.Tests
{
    public class AdoRepositoryTests : AdoRepository<TestEntity>
    {
        public AdoRepositoryTests() : base(null, "TestEntity")
        {
        }

        [Theory]
        [InlineData(1)]
        [InlineData(20)]
        [InlineData(-5)]
        [InlineData(0)]
        public void GetId_ShouldFindValue(int id)
        {
            var item = new TestEntity()
            {
                Id = id,
            };

            var actual = GetId(item);

            Assert.Equal(item.Id, actual);
        }

        [Fact]
        public void GetParameterName_ShouldPrependSymbolAt()
        {
            var propertyInfo = typeof(TestEntity).GetProperty("Id");
            Assert.Equal("@Id", AdoRepository<TestEntity>.GetParameterName(propertyInfo));
        }

        [Fact]
        public void GetDbType_Int()
        {
            var type = typeof(int);
            Assert.Equal(DbType.Int32, AdoRepository<TestEntity>.GetDbType(type));
        }

        [Fact]
        public void GetDbType_String()
        {
            var type = typeof(string);
            Assert.Equal(DbType.String, AdoRepository<TestEntity>.GetDbType(type));
        }

        [Fact]
        public void GetDbType_DateTime()
        {
            var type = typeof(DateTime);
            Assert.Equal(DbType.DateTime, AdoRepository<TestEntity>.GetDbType(type));
        }

        [Fact]
        public void GetDbType_Binary()
        {
            var type = typeof(byte[]);
            Assert.Equal(DbType.Binary, AdoRepository<TestEntity>.GetDbType(type));
        }

        [Fact]
        public void TestGetCommandTextForSave()
        {
            var item = new TestEntity
            {
                BinaryData = { },
                Id = 1,
                Date = DateTime.Now,
                Name = "Name of item"
            };
            var actual = GetCommandTextForSave(item);
            Assert.Equal("UPDATE TestEntity SET Name=@Name, Date=@Date, BinaryData=@BinaryData WHERE Id=1", actual);
        }

        [Fact]
        public void TestGetCommandForCreate_MsSqlConnection()
        {
            var item = new TestEntity
            {
                BinaryData = { },
                Id = 1,
                Date = DateTime.Now,
                Name = "Name of item"
            };
            var actual = GetCommandTextForCreate(new SqlCommand());
            Assert.Equal("INSERT INTO TestEntity(Name,Date,BinaryData) output INSERTED.ID VALUES(@Name,@Date,@BinaryData)", actual);
        }

        [Fact]
        public void TestGetCommandForCreate_SqliteConnection()
        {
            var item = new TestEntity
            {
                BinaryData = { },
                Id = 1,
                Date = DateTime.Now,
                Name = "Name of item"
            };
            var actual = GetCommandTextForCreate(new SqliteCommand());
            Assert.Equal("INSERT INTO TestEntity(Name,Date,BinaryData) VALUES(@Name,@Date,@BinaryData);SELECT last_insert_rowid();", actual);
        }

        protected override TestEntity PopulateRecord(IDataReader reader)
        {
            throw new NotImplementedException();
        }
    }
}
