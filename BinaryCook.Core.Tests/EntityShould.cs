using System;
using BinaryCook.Core.Data.Entities;
using FluentAssertions;
using NSubstitute;
using Xunit;

namespace BinaryCook.Core.Tests
{
    public class EntityShould
    {
        private readonly ISession _session;

        public class TestEntity : Entity
        {
            public TestEntity()
            {
            }

            public TestEntity(Guid id) : base(id)
            {
            }
        }

        public EntityShould()
        {
            var now = DateTime.UtcNow;
            _session = Substitute.For<ISession>();
            _session.Clock.UtcNow.Returns(now);
            _session.User.Returns(SystemUser.Create("test_user"));
        }

        [Fact]
        public void GenerateUniqueId()
        {
            var entity = new TestEntity();
            entity.Should().NotBe(new TestEntity());
        }

        [Fact]
        public void BeEqual()
        {
            var entity = new TestEntity();
            var entity2 = new TestEntity(entity.Id);
            entity.Should().Be(entity2);
        }
    }
}