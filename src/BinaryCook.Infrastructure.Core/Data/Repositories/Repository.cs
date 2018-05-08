using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BinaryCook.Core;
using BinaryCook.Core.Data;
using BinaryCook.Core.Data.AggregateRoots;
using BinaryCook.Core.Data.Entities;
using BinaryCook.Core.Data.Repositories;
using BinaryCook.Core.Data.Specifications;
using Microsoft.EntityFrameworkCore;

namespace BinaryCook.Infrastructure.Core.Data.Repositories
{
    public class Repository<TAggregateRoot, TId> :
        IRead<TAggregateRoot, TId>,
        ISave<TAggregateRoot, TId>,
        IDelete<TAggregateRoot, TId>
        where TAggregateRoot : class, IAggregateRoot<TId>
    {
        private class DefaultSorter : ISorter<TAggregateRoot>
        {
            public IOrderedQueryable<TAggregateRoot> Sort(IQueryable<TAggregateRoot> query) => query.OrderByDescending(x => x.Id);
        }

        protected readonly Context Context;
        protected readonly ISession Session;
        protected readonly DbSet<TAggregateRoot> Set;
        private readonly IQueryable<TAggregateRoot> _queryable;

        public Repository(Context context, ISession session) : this(context, session, null)
        {
        }

        protected Repository(Context context, ISession session, IQueryable<TAggregateRoot> queryable)
        {
            Context = context;
            Session = session;
            Set = Context.Set<TAggregateRoot>();
            _queryable = queryable ?? Set;
        }

        protected virtual IQueryable<TAggregateRoot> Queryable() => _queryable ?? Set;

        protected virtual Task<int> SaveAsync()
        {
            //TODO: rewrite this, extract to Audit Log
            foreach (var entry in Context.ChangeTracker.Entries())
            {
                switch (entry.State)
                {
                    case EntityState.Deleted:
                        if (entry.Properties.Any(x => x.Metadata.Name.StartsWith("Metadata_Deleted")))
                        {
                            entry.Property("Metadata_DeletedDateAtUtc").CurrentValue = Session.Clock.UtcNow;
                            entry.Property("Metadata_DeletedBy").CurrentValue = Session.User.Id;
                        }

                        break;
                    case EntityState.Modified:
                        if (entry.Properties.Any(x => x.Metadata.Name.StartsWith("Metadata_Updated")))
                        {
                            entry.Property("Metadata_UpdatedDateAtUtc").CurrentValue = Session.Clock.UtcNow;
                            entry.Property("Metadata_UpdatedBy").CurrentValue = Session.User.Id;
                        }

                        break;
                    case EntityState.Added:
                        if (entry.Properties.Any(x => x.Metadata.Name.StartsWith("Metadata_Created")))
                        {
                            entry.Property("Metadata_CreatedDateAtUtc").CurrentValue = Session.Clock.UtcNow;
                            entry.Property("Metadata_CreatedBy").CurrentValue = Session.User.Id;
                        }

                        break;
                }
            }

            return Context.SaveChangesAsync();
        }

        public virtual async Task<bool> DeleteAsync(TAggregateRoot aggregate)
        {
            switch (aggregate)
            {
                case IUnremovable metadata:
                    metadata.Delete();
                    break;
                default:
                    Set.Remove(aggregate);
                    break;
            }

            await SaveAsync();
            return true;
        }

        public void Dispose()
        {
            Context?.Dispose();
        }

        public virtual Task<TAggregateRoot> GetAsync(TId id) => Queryable().FirstOrDefaultAsync(x => id.Equals(x.Id));

        public virtual Task<bool> AnyAsync(TId id) => Queryable().AsNoTracking().AnyAsync(x => id.Equals(x.Id));

        public virtual Task<bool> AnyAsync(ISpecification<TAggregateRoot> specification) => Queryable().AsNoTracking().SafeApply(specification).AnyAsync();

        public virtual async Task<IPagedList<TAggregateRoot>> GetAsync(ISpecification<TAggregateRoot> specification, ISorter<TAggregateRoot> sorter = null,
            IPagedInfo pagedInfo = null)
        {
            var q = Queryable().AsNoTracking()
                .SafeApply(specification)
                .SafeSort(sorter ?? new DefaultSorter())
                .SafeApply(pagedInfo);

            var result = await q.ToListAsync();
            return new PagedList<TAggregateRoot>(result, pagedInfo?.Page ?? 1, pagedInfo?.PageSize ?? result.Count, result.Count);
        }

        public virtual async Task InsertAsync(TAggregateRoot aggregate)
        {
            await Set.AddAsync(aggregate);
            await SaveAsync();
        }

        public virtual async Task InsertAsync(IEnumerable<TAggregateRoot> aggregates)
        {
            var items = aggregates.ToList();
            await Set.AddRangeAsync(items);
            await SaveAsync();
        }

        public virtual async Task UpdateAsync(TAggregateRoot aggregate)
        {
            Set.Update(aggregate);
            await SaveAsync();
        }
    }
}