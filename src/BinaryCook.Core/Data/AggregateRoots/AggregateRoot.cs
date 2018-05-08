using System;
using System.Collections.Generic;
using BinaryCook.Core.Data.Entities;
using BinaryCook.Core.Events;

namespace BinaryCook.Core.Data.AggregateRoots
{
    public interface IAggregateRoot : IEntity
    {
        IEnumerable<IDomainEvent> Events { get; }
    }

    public interface IAggregateRoot<out TId> : IAggregateRoot, IEntity<TId>
    {
    }

    [Serializable]
    public abstract class AggregateRoot<TId> : Entity<TId>, IAggregateRoot<TId>
    {
        protected AggregateRoot()
        {
        }

        protected AggregateRoot(TId id) : base(id)
        {
        }

        private readonly IList<IDomainEvent> _events = new List<IDomainEvent>();
        public IEnumerable<IDomainEvent> Events => _events;

        protected void AddEvent<T>(T @event) where T : IDomainEvent
        {
            _events.Add(@event);
        }
    }

    [Serializable]
    public abstract class AggregateRoot : AggregateRoot<Guid>
    {
        protected AggregateRoot() : base(Guid.NewGuid())
        {
        }

        protected AggregateRoot(Guid id) : base(id)
        {
        }
    }
}