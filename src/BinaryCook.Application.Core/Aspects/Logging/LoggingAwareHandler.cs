using System;
using System.Diagnostics;
using System.Threading.Tasks;
using BinaryCook.Core;
using BinaryCook.Core.Commands;
using BinaryCook.Core.Messaging;
using Microsoft.Extensions.Logging;

namespace BinaryCook.Application.Core.Aspects.Logging
{
    public class LoggingAwareCommandHandler<TCommand> : ICommandHandler<TCommand> where TCommand : ICommand
    {
        private readonly ICommandHandler<TCommand> _handler;
        private readonly ILogger<TCommand> _logger;
        private readonly ISession _session;

        public LoggingAwareCommandHandler(ICommandHandler<TCommand> handler, ILogger<TCommand> logger, ISession session)
        {
            _handler = handler;
            _logger = logger;
            _session = session;
        }

        public async Task<ICommandResult> HandleAsync(CommandContext<TCommand> ctx)
        {
            _logger.LogTrace($"Command handling started {typeof(TCommand).Name} at [{_session.Clock.UtcNow}] by [{_session.User}]");
            ICommandResult result;
            var sw = Stopwatch.StartNew();
            try
            {
                result = await _handler.HandleAsync(ctx);
            }
            catch (Exception e)
            {
                sw.Stop();
                var message =
                    $"Command handling completed with error {typeof(TCommand).Name} at [{_session.Clock.UtcNow}] by [{_session.User}] in {sw.ElapsedMilliseconds}ms ";
                _logger.LogTrace(message);
                _logger.LogError(e, message);
                throw;
            }

            sw.Stop();
            _logger.LogTrace(
                $"Command handling completed {typeof(TCommand).Name} at [{_session.Clock.UtcNow}] by [{_session.User}] in {sw.ElapsedMilliseconds}ms");
            return result;
        }
    }

    public class LoggingAwareQueryHandler<TDto, TQuery> : IQueryHandler<TDto, TQuery>
        where TQuery : IQuery
    {
        private readonly IQueryHandler<TDto, TQuery> _handler;
        private readonly ILogger<TQuery> _logger;
        private readonly ISession _session;

        public LoggingAwareQueryHandler(IQueryHandler<TDto, TQuery> handler, ILogger<TQuery> logger, ISession session)
        {
            _handler = handler;
            _logger = logger;
            _session = session;
        }

        public async Task<IQueryResult<TDto>> FetchAsync(TQuery query)
        {
            _logger.LogTrace($"Query handling started {typeof(TQuery).Name} at [{_session.Clock.UtcNow}] by [{_session.User}]");
            IQueryResult<TDto> result;
            var sw = Stopwatch.StartNew();
            try
            {
                result = await _handler.FetchAsync(query);
            }
            catch (Exception e)
            {
                sw.Stop();
                var message =
                    $"Query handling completed with error {typeof(TQuery).Name} at [{_session.Clock.UtcNow}] by [{_session.User}] in {sw.ElapsedMilliseconds}ms";
                _logger.LogTrace(message);
                _logger.LogError(e, message);
                throw;
            }

            sw.Stop();
            _logger.LogTrace($"Query handling completed {typeof(TQuery).Name} at [{_session.Clock.UtcNow}] by [{_session.User}] in {sw.ElapsedMilliseconds}ms");
            return result;
        }
    }
}