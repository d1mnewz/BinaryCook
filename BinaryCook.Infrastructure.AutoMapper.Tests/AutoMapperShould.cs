using System;
using BinaryCook.Infrastructure.AutoMapper.Tests.Models;
using FluentAssertions;
using Xunit;

namespace BinaryCook.Infrastructure.AutoMapper.Tests
{
    [Collection(AutoMapperFixture.CollectionDefinition)]
    public class AutoMapperShould
    {
        private readonly AutoMapperFixture _fixture;

        public AutoMapperShould(AutoMapperFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public void ForwardMap()
        {
            var mapper = _fixture.Mapper<TestClass, TestClassBiMappedModel>();
            var testClass = new TestClass(nameof(ForwardMap));
            var model = mapper.Map(testClass);

            model.Should().NotBe(testClass);
            model.Id.Should().Be(testClass.Id);
            model.Name.Should().Be(testClass.Name);
        }

        [Fact]
        public void BackwardMap()
        {
            var mapper = _fixture.Mapper<TestClassBiMappedModel, TestClass>();
            var model = new TestClassBiMappedModel
            {
                Id = Guid.NewGuid(),
                Name = nameof(BackwardMap)
            };
            var testClass = mapper.Map(model);
            testClass.Should().NotBe(model);
            testClass.Id.Should().Be(model.Id);
            testClass.Name.Should().Be(model.Name);
        }
    }
}