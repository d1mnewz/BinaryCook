using System;
using BinaryCook.Core.Data.Extensions;
using BinaryCook.Core.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BinaryCook.Core.Data.Entities
{
    public interface IEntityConfiguration
    {
        Type EntityType { get; }
        void Configure(ModelBuilder builder);
    }

    public abstract class EntityConfiguration : IEntityConfiguration
    {
        public abstract Type EntityType { get; }
        public abstract void Configure(ModelBuilder builder);
    }

    public abstract class EntityConfiguration<T> : EntityConfiguration where T : class
    {
        public override Type EntityType => typeof(T);
        protected virtual string TableName => EntityType.Name;

        public override void Configure(ModelBuilder builder)
        {
            var cfg = builder.Entity<T>();
            cfg.ToTable(TableName);

            if (typeof(T).ImplementsInterface<IEntity>())
            {
                cfg.Property<byte[]>("RowVersion").IsRowVersion();
                cfg.Property<DateTime>("Metadata_CreatedDateAtUtc").Required();
                cfg.Property<string>("Metadata_CreatedBy").Required();
                cfg.Property<DateTime?>("Metadata_UpdatedDateAtUtc").Optional();
                cfg.Property<string>("Metadata_UpdatedBy").Optional();
                cfg.Property<DateTime?>("Metadata_DeletedDateAtUtc").Optional();
                cfg.Property<DateTime?>("Metadata_DeletedBy").Optional();
                
                if (typeof(T).ImplementsInterface<IUnremovable>())
                {
                    cfg.Property<bool>("IsDeleted").Required();
                    cfg.WithDefaultUnremovableFilter();
                }
            }

            Initialize(builder, cfg);
        }


        protected virtual void Initialize(ModelBuilder builder, EntityTypeBuilder<T> cfg)
        {
        }
    }

    public abstract class EntityDiscriminatorValueConfiguration<T> : EntityConfiguration where T : class
    {
        public override Type EntityType => typeof(T);

        public override void Configure(ModelBuilder builder)
        {
            var cfg = builder.Entity<T>();

            Initialize(builder, cfg);
        }

        protected virtual void Initialize(ModelBuilder builder, EntityTypeBuilder<T> cfg)
        {
        }
    }
}