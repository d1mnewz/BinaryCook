using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using BinaryCook.Core.Data;
using BinaryCook.Core.Data.AggregateRoots;
using BinaryCook.Core.Data.Repositories;
using BinaryCook.Core.Data.Specifications;

namespace BinaryCook.Infrastructure.Core.Data.Extensions
{
    public static class RepositoryExtensions
    {
        private class FindByIdsSpecification<T, TId> : Specification<T> where T : IAggregateRoot<TId>
        {
            private readonly IList<TId> _ids;

            public FindByIdsSpecification(IList<TId> ids)
            {
                _ids = ids;
            }

            public override Expression<Func<T, bool>> Expression => x => _ids.Contains(x.Id);
        }
        public static Task<IPagedList<T>> GetAsync<T, TId>(this IRead<T, TId> read, IList<TId> ids) where T : class, IAggregateRoot<TId> =>
            read.GetAsync(new FindByIdsSpecification<T, TId>(ids));

        public static Task<IPagedList<T>> GetAsync<T, TId>(this IRead<T, TId> read, IFilter<T> filter) where T : class, IAggregateRoot<TId> =>
            read.GetAsync(filter.ToSpecification(), filter.ToSorter(), filter.ToPagedInfo());
    }
}