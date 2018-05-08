using System.Threading.Tasks;
using BinaryCook.Application.Core.Validations;
using BinaryCook.Core.Commands;
using BinaryCook.Core.Messaging;

namespace BinaryCook.Application.Core.Aspects.Validations
{
    public class ValidationAwareCommandHandler<TCommand> : ICommandHandler<TCommand> where TCommand : ICommand
    {
        private readonly ICommandHandler<TCommand> _handler;
        private readonly IValidator<TCommand> _validator;

        public ValidationAwareCommandHandler(ICommandHandler<TCommand> handler, IValidator<TCommand> validator)
        {
            _handler = handler;
            _validator = validator;
        }

        public async Task<ICommandResult> HandleAsync(CommandContext<TCommand> ctx)
        {
            var validationResult = await _validator.ValidateAsync(ctx.Command);
            if (!validationResult.IsValid)
                return ctx.FailWith(validationResult);

            return await _handler.HandleAsync(ctx);
        }
    }
}