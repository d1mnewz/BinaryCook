using System;

namespace BinaryCook.Core.Data.Entities
{
    public interface IEntity
    {
        object GetId();
    }

    public interface IEntity<out TId> : IEntity
    {
        TId Id { get; }
    }

    [Serializable]
    public abstract class Entity<TId> : IEntity<TId>, IEquatable<Entity<TId>>, IUnremovable
    {
        protected Entity()
        {
        }

        protected Entity(TId id) : this()
        {
            Id = id;
        }

        public object GetId() => Id;
        public TId Id { get; protected set; }

        public bool Equals(Entity<TId> entity)
        {
            if (ReferenceEquals(this, entity)) return true;
            if (ReferenceEquals(null, entity)) return false;
            return Id.Equals(entity.Id);
        }

        public override bool Equals(object anotherObject) => Equals(anotherObject as Entity<TId>);

        public override int GetHashCode() => GetType().GetHashCode() * 907 + (Id?.GetHashCode() ?? 0);

        public override string ToString() => $"{GetType().Name} [Id={Id}]";

        public bool IsDeleted { get; protected set; }

        public void Delete()
        {
            IsDeleted = true;
        }
    }

    [Serializable]
    public abstract class Entity : Entity<Guid>
    {
        protected Entity() : base(Guid.NewGuid())
        {
        }

        protected Entity(Guid id) : base(id)
        {
        }
    }
}