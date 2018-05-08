using System.Threading;
using System.Threading.Tasks;
using BinaryCook.Core.Commands;
using FluentValidation;

namespace BinaryCook.Application.Core.Validations.Fluent
{
    public abstract class FluentValidator<T> : AbstractValidator<T>, IValidator<T>
    {
        public new virtual async Task<IValidationResult> ValidateAsync(T instance, CancellationToken ct = default(CancellationToken))
        {
            var validationResult = await base.ValidateAsync(instance, ct);
            if (validationResult.IsValid)
                return ValidationResult.Succeeded;

            var result = new ValidationResultAggregate();
            foreach (var error in validationResult.Errors)
            {
                result.Add(ValidationResult.Error(error.PropertyName, error.ErrorMessage));
            }

            return result;
        }
    }
}