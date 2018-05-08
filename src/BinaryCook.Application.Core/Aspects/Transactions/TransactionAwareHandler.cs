using System.Threading.Tasks;
using System.Transactions;
using BinaryCook.Core.Commands;
using BinaryCook.Core.Messaging;

namespace BinaryCook.Application.Core.Aspects.Transactions
{
    public class TransactionAwareHandler<TCommand> : ICommandHandler<TCommand> where TCommand : ICommand
    {
        private readonly ICommandHandler<TCommand> _handler;

        public TransactionAwareHandler(ICommandHandler<TCommand> handler)
        {
            _handler = handler;
        }

        public async Task<ICommandResult> HandleAsync(CommandContext<TCommand> ctx)
        {
            using (var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                var result = await _handler.HandleAsync(ctx);
                transaction.Complete();
                return result;
            }
        }
    }
}