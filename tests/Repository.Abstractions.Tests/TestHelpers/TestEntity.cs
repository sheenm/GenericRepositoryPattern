using System;

namespace Repository.Abstractions.Tests.TestHelpers
{
    public class TestEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime Date { get; set; }
        public byte[] BinaryData { get; set; }
    }
}