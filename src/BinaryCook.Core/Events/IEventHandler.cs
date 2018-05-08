using System.Threading.Tasks;
using BinaryCook.Core.Messaging;

namespace BinaryCook.Core.Events
{
    public interface IEventHandler<T> where T : IEvent
    {
        Task HandleAsync(EventContext<T> ctx);
    }
}