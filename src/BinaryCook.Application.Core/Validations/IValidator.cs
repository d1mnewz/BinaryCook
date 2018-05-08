using System.Threading;
using System.Threading.Tasks;
using BinaryCook.Core.Commands;

namespace BinaryCook.Application.Core.Validations
{
    public interface IValidator<in T>
    {
        Task<IValidationResult> ValidateAsync(T instance, CancellationToken ct = default(CancellationToken));
    }
}