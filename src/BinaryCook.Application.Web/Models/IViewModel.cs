namespace BinaryCook.Application.Web.Models
{
    public interface IViewModel
    {
        object GetId();
    }

    public interface IViewModel<out TId> : IViewModel
    {
        TId Id { get; }
    }
}