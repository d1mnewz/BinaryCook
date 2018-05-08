using System.Threading.Tasks;
using BinaryCook.Core.Messaging;

namespace BinaryCook.Core.Commands
{
    public interface ICommandHandler<TCommand> where TCommand : ICommand
    {
        Task<ICommandResult> HandleAsync(CommandContext<TCommand> ctx);
    }
}