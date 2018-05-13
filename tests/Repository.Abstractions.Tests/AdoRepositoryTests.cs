using System;
using Xunit;
using Repository.Abstractions.Tests.TestHelpers;

namespace Repository.Abstractions.Tests
{
    public class AdoRepositoryTests
    {
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

            var repository = new TestEntityRepository(null, "TestEntity");
            var actual = repository.GetId(entity);

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
            var repository = new TestEntityRepository(null, "TestEntity");
            Assert.Equal("@Id", AdoRepository<TestEntity>.GetParameterName(propertyInfo));
        }
    }
}
