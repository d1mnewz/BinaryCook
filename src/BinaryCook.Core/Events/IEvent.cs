using System;

namespace BinaryCook.Core.Events
{
    public interface IEvent
    {
        Guid EventId { get; }
    }

    public abstract class Event : IEvent
    {
        public Guid EventId { get; } = Guid.NewGuid();
    }
}