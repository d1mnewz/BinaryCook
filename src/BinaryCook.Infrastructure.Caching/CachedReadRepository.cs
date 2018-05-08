using System.Threading.Tasks;
using BinaryCook.Core.Data;
using BinaryCook.Core.Data.AggregateRoots;
using BinaryCook.Core.Data.Events;
using BinaryCook.Core.Data.Repositories;
using BinaryCook.Core.Data.Specifications;
using BinaryCook.Core.Events;
using BinaryCook.Core.Messaging;

namespace BinaryCook.Infrastructure.Caching
{
    public class CachedReadRepository<T, TId> :
        IRead<T, TId>,
        IEventHandler<DomainUpdated<T>>,
        IEventHandler<DomainDeleted<T>>
        where T : class, IAggregateRoot<TId>
    {
        private readonly IRead<T, TId> _read;
        private readonly ICacheManager _cacheManager;

        public CachedReadRepository(IRead<T, TId> read, ICacheManager cacheManager)
        {
            _read = read;
            _cacheManager = cacheManager;
        }

        public void Dispose()
        {
            _read.Dispose();
        }

        public Task<bool> AnyAsync(TId id) => _read.AnyAsync(id);

        private static string CacheKey(TId id) => $"{typeof(T).FullName}-{id}";

        public Task<T> GetAsync(TId id) => _cacheManager.GetOrSetAsync(CacheKey(id), () => _read.GetAsync(id));

        public Task<bool> AnyAsync(ISpecification<T> specification) => _read.AnyAsync(specification);

        public Task<IPagedList<T>> GetAsync(ISpecification<T> specification, ISorter<T> sorter = null, IPagedInfo pagedInfo = null) =>
            _read.GetAsync(specification, sorter, pagedInfo);

        public Task HandleAsync(EventContext<DomainUpdated<T>> ctx) => _cacheManager.RemoveAsync(CacheKey(ctx.Event.Entity.Id));

        public Task HandleAsync(EventContext<DomainDeleted<T>> ctx) => _cacheManager.RemoveAsync(CacheKey(ctx.Event.Entity.Id));
    }
}