using BinaryCook.Core.Data.Entities;

namespace BinaryCook.Core.Data.Events
{
    public class DomainInserted<T> : DomainEvent where T : class, IEntity
    {
        public T Entity { get; }

        public DomainInserted(T entity)
        {
            Entity = entity;
        }
    }
}