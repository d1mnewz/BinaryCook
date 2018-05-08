using System;
using BinaryCook.Core.Domain.Model.Common;
using FluentAssertions;
using Xunit;

namespace BinaryCook.Core.Domain.Tests.Model.Common
{
	public class MeasureShould
	{
		[Fact]
		public void HaveCorrectCurrency()
		{
			Measure.Cup(decimal.Zero).Unit.Should().BeEquivalentTo("CUP");
			Measure.Oz(decimal.Zero).Unit.Should().BeEquivalentTo("OZ");
			Measure.Tbsp(decimal.Zero).Unit.Should().BeEquivalentTo("TBSP");
		}

		[Theory]
		[InlineData(1)]
		[InlineData(5)]
		[InlineData(10)]
		[InlineData(2)]
		public void HaveCorrectAmount(decimal value)
		{
			Measure.Cup(value).Amount.Should().Be(value);
			Measure.Oz(value).Amount.Should().Be(value);
			Measure.Tbsp(value).Amount.Should().Be(value);
		}

		[Fact]
		public void ThrowArgumentException()
		{
			Assert.Throws<ArgumentException>(() => Measure.Cup(-1));
			Assert.Throws<ArgumentException>(() => Measure.Oz(-1));
			Assert.Throws<ArgumentException>(() => Measure.Tbsp(-1));
		}
	}
}