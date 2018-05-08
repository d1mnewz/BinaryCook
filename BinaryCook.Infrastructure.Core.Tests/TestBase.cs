using System;
using NSubstitute;

namespace BinaryCook.Core.Testing.Core
{
	public abstract class TestBase
	{
		protected readonly ISession Session;
		private readonly IClock _constClock;

		protected TestBase()
		{
			_constClock = Substitute.For<IClock>();
			_constClock.UtcNow.Returns(DateTime.UtcNow);

			Session = Substitute.For<ISession>();
			Session.User.Returns(x => SystemUser.Create(GetType().FullName));
			Session.Clock.Returns(x => Clock());
		}

		protected virtual IClock Clock() => _constClock;
	}
}