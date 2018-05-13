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
            var entity = new TestEntity()
            {
                Id = id,
            };

            var actual = _repository.GetId(entity);

            Assert.Equal(entity.Id, actual);
        }

        [Fact]
        public void GetId_EntityWithoutID_ShouldThrowArgumentException()
        {
            var entity = new TestEntityWithoutId();
            var repository = new TestEntityWithoutIdRepository(null, "TestEntityWithoutId");
            Assert.Throws<ArgumentException>(() => repository.GetId(entity));
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
    }
}
