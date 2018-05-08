using System;
using System.Linq;
using System.Threading.Tasks;
using BinaryCook.Core.Code;
using BinaryCook.Core.Commands;
using BinaryCook.Core.Events;
using Microsoft.Extensions.DependencyInjection;

namespace BinaryCook.Core.Messaging
{
    public interface IBus
    {
        Task Emit<TEvent>(TEvent @event) where TEvent : class, IEvent;

        Task<ICommandResult> Publish<TCommand>(TCommand command) where TCommand : class, ICommand;

        Task<IQueryResult<TDto>> Fetch<TDto, TQuery>(TQuery query) where TQuery : class, IQuery;
    }

    public class Bus : IBus
    {
        private readonly IServiceProvider _sp;

        public Bus(IServiceProvider sp) //TODO: do not use ServiceProvider. Write method to register Command Handlers
        {
            _sp = sp;
        }

        public async Task Emit<TEvent>(TEvent @event) where TEvent : class, IEvent
        {
            var handlerType = typeof(IEventHandler<>).MakeGenericType(@event.GetType());
            var handlers = _sp.GetServices(handlerType);
            var eventContext = ActivatorUtilities.CreateInstance(_sp, typeof(CurrentEventContext<>).MakeGenericType(@event.GetType()), @event, this);
            // handle one by one. if we will use Task.WhenAll - there might be a problem with simultaneous use of context. fix that issue first
            foreach (var handler in handlers.ToList())
            {
                var method = handler.GetType().GetMethod(nameof(IEventHandler<TEvent>.HandleAsync));
                await (Task) method.Invoke(handler, new[] {eventContext});
            }
        }

        public Task<ICommandResult> Publish<TCommand>(TCommand command) where TCommand : class, ICommand
        {
            Requires.NotNull(command, nameof(command));

            var handlerType = typeof(ICommandHandler<>).MakeGenericType(command.GetType());
            var handlers = _sp.GetServices(handlerType).ToArray();

            Ensure.That(handlers.Any(), $"No handler registered for command type: {typeof(TCommand)}");
            Ensure.That(handlers.Length == 1, $"More than one handler registered for command type: {typeof(TCommand)}");
            
            var commandHandler = handlers[0];
            var method = commandHandler.GetType().GetMethod(nameof(ICommandHandler<TCommand>.HandleAsync));
            var commandContext = ActivatorUtilities.CreateInstance(_sp, typeof(CurrentCommandContext<>).MakeGenericType(command.GetType()), command, this);
            return (Task<ICommandResult>) method.Invoke(handlers[0], new[] {commandContext});
        }

        public Task<IQueryResult<TDto>> Fetch<TDto, TQuery>(TQuery query) where TQuery : class, IQuery
        {
            Requires.NotNull(query, nameof(query));

            var handlerType = typeof(IQueryHandler<,>).MakeGenericType(typeof(TDto), query.GetType());
            var handlers = _sp.GetServices(handlerType).ToArray();

            Ensure.That(handlers.Any(), $"No handler registered for query type: {typeof(TQuery)}");
            Ensure.That(handlers.Length == 1, $"More than one handler registered for query type: {typeof(TQuery)}");

            var queryHandler = handlers[0];
            var method = queryHandler.GetType().GetMethod(nameof(IQueryHandler<TDto, TQuery>.FetchAsync));
            return (Task<IQueryResult<TDto>>) method.Invoke(queryHandler, new object[] {query});
        }

        private class CurrentCommandContext<TCommand> : CommandContext<TCommand> where TCommand : ICommand
        {
            public CurrentCommandContext(TCommand command, ISession session, IBus bus) : base(command, session, bus)
            {
            }
        }

        private class CurrentEventContext<TEvent> : EventContext<TEvent> where TEvent : IEvent
        {
            public CurrentEventContext(TEvent command, ISession session, IBus bus) : base(command, session, bus)
            {
            }
        }
    }
}