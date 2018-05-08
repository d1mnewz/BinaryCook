using BinaryCook.Core.Commands;

namespace BinaryCook.Application.Web.Models
{
    public interface IEditResult<out TEditModel>
    {
        TEditModel Model { get; }
        IValidationResult ValidationResult { get; }
    }
}