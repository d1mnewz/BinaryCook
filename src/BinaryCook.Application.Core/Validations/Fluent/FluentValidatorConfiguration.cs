using BinaryCook.Core.Code;
using FluentValidation;

namespace BinaryCook.Application.Core.Validations.Fluent
{
    public class FluentValidatorConfiguration
    {
        public static FluentValidatorConfiguration Current
        {
            get => new FluentValidatorConfiguration(ValidatorOptions.CascadeMode == CascadeMode.StopOnFirstFailure);
            set
            {
                Requires.NotNull(value, nameof(value));
                ValidatorOptions.CascadeMode = value.BreakOnFirstError ? CascadeMode.StopOnFirstFailure : CascadeMode.Continue;
            }
        }

        public bool BreakOnFirstError { get; }

        public FluentValidatorConfiguration(bool breakOnFirstError)
        {
            BreakOnFirstError = breakOnFirstError;
        }
    }
}