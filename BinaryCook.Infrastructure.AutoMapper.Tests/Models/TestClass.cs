using System;
using BinaryCook.Infrastructure.AutoMapper.Mapping;

namespace BinaryCook.Infrastructure.AutoMapper.Tests.Models
{
    public class TestClass
    {
        public TestClass(string name)
        {
            Name = name;
            Id = Guid.NewGuid();
        }

        public Guid Id { get; private set; }
        public string Name { get; private set; }
    }

    public class TestClassBiMappedModel : BiMapping<TestClassBiMappedModel, TestClass>
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
    }

    public class TestClassMapping : Mapping<TestClassBiMappedModel, TestClass>
    {
    }

    public class TestClassModelMapping : Mapping<TestClass, TestClassBiMappedModel>
    {
    }
}