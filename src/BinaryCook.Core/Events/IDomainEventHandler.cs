using System.Threading.Tasks;

namespace BinaryCook.Core.Events
{
    public interface IDomainEventHandler<in T> where T : IDomainEvent
    {
        Task Handle(T @event);
    }
}
