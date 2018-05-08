using BinaryCook.Core.Commands;
using BinaryCook.Core.Events;

namespace BinaryCook.Core.Messaging
{
    public abstract class MessageContext
    {
        public ISession Session { get; }
        public IBus Bus { get; }

        protected MessageContext(ISession session, IBus bus)
        {
            Session = session;
            Bus = bus;
        }
    }

    public abstract class EventContext<TEvent> : MessageContext where TEvent : IEvent
    {
        public TEvent Event { get; }

        public EventContext(TEvent @event, ISession session, IBus bus) : base(session, bus)
        {
            Event = @event;
        }
    }

    public abstract class CommandContext<TCommand> : MessageContext where TCommand : ICommand
    {
        public TCommand Command { get; }

        public CommandContext(TCommand command, ISession session, IBus bus) : base(session, bus)
        {
            Command = command;
        }
    }
}