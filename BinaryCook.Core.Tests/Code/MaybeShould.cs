using System.Linq;
using BinaryCook.Core.Code;
using FluentAssertions;
using Xunit;

namespace BinaryCook.Core.Tests.Code
{
   public class MaybeShould
    {
        [Fact]
        public void HaveValue()
        {
            var sut = Maybe<string>.Some("1");
            Assert.NotNull(sut);
            sut.Any().Should().BeTrue();
            
            sut = Maybe<string>.Some(null);
            Assert.NotNull(sut);
            sut.Any().Should().BeTrue();
        }

        [Fact]
        public void HaveNoValue()
        {
            var sut = Maybe<string>.Nothing;
            sut.Any().Should().BeFalse();
        }

        [Theory]
        [InlineData(1)]
        [InlineData(-1)]
        [InlineData(10)]
        public void ReturnCorrectValue(int value)
        {
            var sut = Maybe<int>.Some(value);
            sut.Single().Should().Be(value);
        }

        [Fact]
        public void BeGreater()
        {
            var sut1 = Maybe<int>.Some(10);
            var sut2 = Maybe<int>.Some(1);

            Assert.True(sut1.CompareTo(sut2) > 0);
            Assert.True(sut1.CompareTo(1) > 0);
        }

        [Fact]
        public void BeLess()
        {
            var sut1 = Maybe<int>.Some(10);
            var sut2 = Maybe<int>.Some(1);

            Assert.True(sut2.CompareTo(sut1) < 0);
            Assert.True(sut2.CompareTo(10) < 0);
        }

        [Fact]
        public void BeEqual()
        {
            var sut1 = Maybe<int>.Some(10);
            var sut2 = Maybe<int>.Some(10);

            Assert.True(sut1.CompareTo(sut2) == 0);
            Assert.True(sut1.CompareTo(10) == 0);
        }

        [Fact]
        public void ReturnSameHashCodeWhenHasNoValue()
        {
            var sut1 = Maybe<string>.Nothing;
            var sut2 = Maybe<string>.Nothing;

            sut1.GetHashCode().Should().Be(sut2.GetHashCode());
        }

        [Fact]
        public void ReturnSameHashCodeWhenHasSameValues()
        {
            var sut1 = Maybe<string>.Some("some");
            var sut2 = Maybe<string>.Some("some");

            sut1.GetHashCode().Should().Be(sut2.GetHashCode());
        }
    }
}