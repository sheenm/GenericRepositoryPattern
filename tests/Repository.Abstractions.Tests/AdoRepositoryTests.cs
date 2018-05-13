using System;
using Xunit;
using Repository.Abstractions.Tests.TestHelpers;
using System.Data;

namespace Repository.Abstractions.Tests
{
    public class AdoRepositoryTests
    {
        private readonly AdoRepository<TestEntity> _repository;
        public AdoRepositoryTests()
        {
            _repository = new TestEntityRepository(null, "TestEntity");
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

            var actual = _repository.GetId(item);

            Assert.Equal(item.Id, actual);
        }

        [Fact]
        public void GetId_EntityWithoutID_ShouldThrowArgumentException()
        {
            var item = new TestEntityWithoutId();
            var repository = new TestEntityWithoutIdRepository(null, "TestEntityWithoutId");
            Assert.Throws<ArgumentException>(() => repository.GetId(item));
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
            Assert.Equal(DbType.Int32, _repository.GetDbType(type));
        }

        [Fact]
        public void GetDbType_String()
        {
            var type = typeof(string);
            Assert.Equal(DbType.String, _repository.GetDbType(type));
        }

        [Fact]
        public void GetDbType_DateTime()
        {
            var type = typeof(DateTime);
            Assert.Equal(DbType.DateTime, _repository.GetDbType(type));
        }

        [Fact]
        public void GetDbType_Binary()
        {
            var type = typeof(byte[]);
            Assert.Equal(DbType.Binary, _repository.GetDbType(type));
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
            var actual = _repository.GetCommandTextForSave(item);
            Assert.Equal("UPDATE TestEntity SET Name=@Name, Date=@Date, BinaryData=@BinaryData WHERE Id=1", actual);
        }
    }
}
