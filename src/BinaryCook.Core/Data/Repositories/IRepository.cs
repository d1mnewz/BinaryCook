using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BinaryCook.Core.Data.AggregateRoots;
using BinaryCook.Core.Data.Entities;
using BinaryCook.Core.Data.Specifications;

namespace BinaryCook.Core.Data.Repositories
{
    public interface IRead<TAggregateRoot, in TId> : IDisposable where TAggregateRoot : class, IAggregateRoot<TId>
    {
        Task<bool> AnyAsync(TId id);
        Task<TAggregateRoot> GetAsync(TId id);

        Task<bool> AnyAsync(ISpecification<TAggregateRoot> specification);
        Task<IPagedList<TAggregateRoot>> GetAsync(ISpecification<TAggregateRoot> specification, ISorter<TAggregateRoot> sorter = null,
            IPagedInfo pagedInfo = null);
    }

    public interface ISave<in TAggregateRoot, TId> : IDisposable where TAggregateRoot : class, IEntity<TId>
    {
        Task InsertAsync(TAggregateRoot aggregate);
        Task InsertAsync(IEnumerable<TAggregateRoot> aggregates);
        Task UpdateAsync(TAggregateRoot aggregate);
    }

    public interface IDelete<in TAggregateRoot, in TId> : IDisposable where TAggregateRoot : class, IEntity<TId>
    {
        Task<bool> DeleteAsync(TAggregateRoot aggregate);
    }
}