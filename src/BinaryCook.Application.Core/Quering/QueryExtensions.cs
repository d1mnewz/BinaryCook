using System.Threading.Tasks;
using BinaryCook.Core.Commands;
using BinaryCook.Core.Data.AggregateRoots;
using BinaryCook.Core.Messaging;

namespace BinaryCook.Application.Core.Quering
{
    public static class QueryExtensions
    {
        public static QueryFilterFetcher<T> Filter<T>(this IBus bus, IFilter<T> filter) where T : class, IAggregateRoot =>
            new QueryFilterFetcher<T>(bus, filter);

        public static IdFetcher<TId> Find<TId>(this IBus bus, TId id) => new IdFetcher<TId>(bus, id);

        public static IdsFetcher<TId> Find<TId>(this IBus bus, TId[] ids) => new IdsFetcher<TId>(bus, ids);

        #region Nested Classes

        public class QueryFilterFetcher<T> where T : class, IAggregateRoot
        {
            private readonly IFilter<T> _filter;
            private readonly IBus _bus;

            public QueryFilterFetcher(IBus bus, IFilter<T> filter)
            {
                _bus = bus;
                _filter = filter;
            }

            public Task<IQueryResult<TDto>> Fetch<TDto>() => _bus.Fetch<TDto, FindByFilterQuery<T>>(new FindByFilterQuery<T>(_filter));
        }

        public class IdFetcher<TId>
        {
            private readonly IBus _bus;
            private readonly TId _id;

            public IdFetcher(IBus bus, TId id)
            {
                _bus = bus;
                _id = id;
            }

            public Task<IQueryResult<TDto>> Fetch<TDto>() => _bus.Fetch<TDto, FindByIdQuery<TId>>(new FindByIdQuery<TId>(_id));
        }

        public class IdsFetcher<TId>
        {
            private readonly IBus _bus;
            private readonly TId[] _ids;

            public IdsFetcher(IBus bus, TId[] ids)
            {
                _bus = bus;
                _ids = ids;
            }

            public Task<IQueryResult<TDto>> Fetch<TDto>() => _bus.Fetch<TDto, FindByIdsQuery<TId>>(new FindByIdsQuery<TId>(_ids));
        }

        #endregion
    }
}