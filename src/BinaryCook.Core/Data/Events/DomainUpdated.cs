using BinaryCook.Core.Data.Entities;

namespace BinaryCook.Core.Data.Events
{
    public class DomainUpdated<T> : DomainEvent where T : class, IEntity
    {
        public T Entity { get; }

        public DomainUpdated(T entity)
        {
            Entity = entity;
        }
    }
}