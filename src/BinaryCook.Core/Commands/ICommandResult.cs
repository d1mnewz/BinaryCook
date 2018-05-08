using BinaryCook.Core.Messaging;

namespace BinaryCook.Core.Commands
{
    public interface ICommandResult
    {
        bool Succeeded { get; }
        IValidationResult ValidationResult { get; }
        ICorrelation Correlation { get; }
        object ReferenceId { get; }
    }

    public class CommandResult : ICommandResult
    {
        public bool Succeeded => ValidationResult == null || ValidationResult.IsValid;
        public IValidationResult ValidationResult { get; }
        public ICorrelation Correlation { get; }
        public object ReferenceId { get; }

        protected CommandResult(ICorrelation correlation, object referenceId, IValidationResult validationResult)
        {
            Correlation = correlation;
            ReferenceId = referenceId;
            ValidationResult = validationResult;
        }
    }

    public class FailedCommandResult : CommandResult
    {
        public FailedCommandResult(ICorrelation correlation, IValidationResult result) : base(correlation, null, result)
        {
        }

        public FailedCommandResult(ICorrelation correlation, string message) : base(correlation, null, Commands.ValidationResult.Error(message, null))
        {
        }

        public FailedCommandResult(ICorrelation correlation, string key, string error) : base(correlation, null, Commands.ValidationResult.Error(key, error))
        {
        }
    }

    public class SucceededCommandResult : CommandResult
    {
        public SucceededCommandResult(ICorrelation correlation, object referenceId) : base(correlation, referenceId, Commands.ValidationResult.Succeeded)
        {
        }
    }

    public static class CommandResultExtensions
    {
        public static ICommandResult Succeeded<TCommand>(this CommandContext<TCommand> ctx, object referenceId) where TCommand : ICommand =>
            new SucceededCommandResult(ctx.Session.Correlation, referenceId);

        public static ICommandResult FailWith<TCommand>(this CommandContext<TCommand> ctx, IValidationResult result) where TCommand : ICommand =>
            new FailedCommandResult(ctx.Session.Correlation, result);

        public static ICommandResult FailWith<TCommand>(this CommandContext<TCommand> ctx, string message) where TCommand : ICommand =>
            new FailedCommandResult(ctx.Session.Correlation, message);

        public static ICommandResult FailWith<TCommand>(this CommandContext<TCommand> ctx, string key, string message) where TCommand : ICommand =>
            new FailedCommandResult(ctx.Session.Correlation, key, message);
    }
}