using System;
using BinaryCook.Core.Data.Entities;
using FluentAssertions;
using Xunit;

namespace BinaryCook.Core.Tests
{
    public class ValueObjectShould
    {
        public class V : ValueObject<V>
        {
            public string Name { get; set; }
            public string Type { get; set; }
            public TimeSpan TimeSpan { get; set; }
        }

        [Fact]
        public void BeEqual()
        {
            var time = DateTime.UtcNow.TimeOfDay; 
            var vo = new V
            {
                Name = "abc",
                Type = "book",
                TimeSpan = time
            };
            var vo2 = new V
            {
                Name = "abc",
                Type = "book",
                TimeSpan = time
            };
            vo.Should().Be(vo2);
        }

        [Fact]
        public void NotBeEqual()
        {
            var vo = new V
            {
                Name = "abc",
                Type = "book",
                TimeSpan = DateTime.UtcNow.TimeOfDay
            };
            var vo2 = new V
            {
                Name = "abc",
                Type = "book2",
                TimeSpan = DateTime.UtcNow.TimeOfDay
            };
            vo.Should().NotBe(null);
            vo.Should().NotBe(vo2);
        }
    }
}