using System;
using BinaryCook.Core.Code;
using Xunit;

namespace BinaryCook.Core.Tests.Code
{
	public class RequiresShould
	{
		[Fact]
		public void ThrowNullException()
		{
			Assert.Throws<ArgumentNullException>(() => Requires.NotNull((long?) null, "testvalue"));
			Assert.Throws<ArgumentNullException>(() => Requires.NotEmpty(null, "testvalue"));
		}

		[Fact]
		public void ThrowArgumentException()
		{
			Assert.Throws<ArgumentException>(() => Requires.NotEmpty("", "testvalue"));
			Assert.Throws<ArgumentException>(() => Requires.NotEmpty(" ", "testvalue"));
			Assert.Throws<ArgumentException>(() => Requires.NotEmpty(Guid.Empty, "testvalue"));
		}
	}
}