using BinaryCook.Core;
using BinaryCook.Core.Commands;
using BinaryCook.Core.Messaging;
using BinaryCook.Core.Testing.Core;
using NSubstitute;

namespace BinaryCook.Application.Tests.Services
{
    public abstract class CommandHandlerShouldBase : TestBase
    {
        protected readonly IBus Bus;

        protected CommandHandlerShouldBase()
        {
            Bus = Substitute.For<IBus>();
        }

        protected class CommandContext<TCommand> : BinaryCook.Core.Messaging.CommandContext<TCommand> where TCommand : ICommand
        {
            public CommandContext(TCommand command, ISession session, IBus bus) : base(command, session, bus)
            {
            }
        }

        protected CommandContext<TCommand> Create<TCommand>(TCommand command) where TCommand : ICommand => new CommandContext<TCommand>(command, Session, Bus);
    }
}