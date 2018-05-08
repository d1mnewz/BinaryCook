using System;
using System.Linq;
using System.Linq.Expressions;
using BinaryCook.Core.Data.Specifications;
using FluentAssertions;
using Xunit;

namespace BinaryCook.Core.Tests.Data
{
	public class SpecificationShould
	{
		private class PositiveSpecification : Specification<int>
		{
			public override Expression<Func<int, bool>> Expression => x => x >= 0;
		}

		private class LessThanSpecification : Specification<int>
		{
			private readonly int _lessThan;

			public LessThanSpecification(int lessThan)
			{
				_lessThan = lessThan;
			}

			public override Expression<Func<int, bool>> Expression => x => x < _lessThan;
		}

		private class GreaterThanSpecification : Specification<int>
		{
			private readonly int _greaterThan;

			public GreaterThanSpecification(int greaterThan)
			{
				_greaterThan = greaterThan;
			}

			public override Expression<Func<int, bool>> Expression => x => x > _greaterThan;
		}

		[Fact]
		public void FilterValues()
		{
			var positiveSpec = new PositiveSpecification();
			var array = Enumerable.Range(-10, 20).ToList();
			var filteredArray = array.Where(x => positiveSpec.IsSatisfiedBy(x)).ToList();

			array.Count.Should().BeGreaterThan(filteredArray.Count);
			filteredArray.Should().HaveCount(10);
			filteredArray.All(x => x >= 0).Should().BeTrue();
		}

		[Theory]
		[InlineData(1)]
		[InlineData(2)]
		[InlineData(3)]
		[InlineData(4)]
		[InlineData(0)]
		[InlineData(int.MaxValue)]
		public void Satisfied(int value)
		{
			var positiveSpec = new PositiveSpecification();
			positiveSpec.IsSatisfiedBy(value).Should().BeTrue();
		}

		[Theory]
		[InlineData(-100)]
		[InlineData(-2)]
		[InlineData(-1)]
		[InlineData(-int.MaxValue)]
		public void NotSatisfied(int value)
		{
			var positiveSpec = new PositiveSpecification();
			positiveSpec.IsSatisfiedBy(value).Should().BeFalse();
		}

		[Fact]
		public void CombineAndExpression()
		{
			var lessSpec = new LessThanSpecification(5);
			var greaterSpec = new GreaterThanSpecification(2);
			var array = Enumerable.Range(0, 10).ToList();
			var combinedSpec = lessSpec.And(greaterSpec);
			var filteredArray = array.Where(x => combinedSpec.IsSatisfiedBy(x)).ToList();

			array.Count.Should().BeGreaterThan(filteredArray.Count);
			filteredArray.Should().HaveCount(2);
			filteredArray.All(x => x > 2 && x < 5).Should().BeTrue();
		}

		[Fact]
		public void CombineOrExpression()
		{
			var lessSpec = new LessThanSpecification(-5);
			var positiveSpec = new PositiveSpecification();
			var array = Enumerable.Range(-10, 20).ToList();
			var combinedSpec = positiveSpec.Or(lessSpec);
			var filteredArray = array.Where(x => combinedSpec.IsSatisfiedBy(x)).ToList();

			array.Count.Should().BeGreaterThan(filteredArray.Count);
			filteredArray.Should().HaveCount(15);
			filteredArray.All(x => x < -5 || x >= 0).Should().BeTrue();
		}

		[Fact]
		public void CombineNotSpecification()
		{
			var positiveSpec = new PositiveSpecification();
			var array = Enumerable.Range(-10, 20).ToList();
			var combinedSpec = positiveSpec.Not();
			var filteredArray = array.Where(x => combinedSpec.IsSatisfiedBy(x)).ToList();

			array.Count.Should().BeGreaterThan(filteredArray.Count);
			filteredArray.Should().HaveCount(10);
			filteredArray.All(x => x < 0).Should().BeTrue();
		}
	}
}