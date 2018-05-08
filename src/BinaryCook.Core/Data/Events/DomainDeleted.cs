using BinaryCook.Core.Data.Entities;

namespace BinaryCook.Core.Data.Events
{
    public class DomainDeleted<T> : DomainEvent where T : class, IEntity
    {
        public T Entity { get; }

        public DomainDeleted(T entity)
        {
            Entity = entity;
        }
    }
}