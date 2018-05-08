using System.Threading.Tasks;
using BinaryCook.Application.Core.Quering;
using BinaryCook.Core.Commands;
using BinaryCook.Core.Data.AggregateRoots;
using BinaryCook.Core.Data.Repositories;
using BinaryCook.Infrastructure.Core.Data.Extensions;

namespace BinaryCook.Application.Services
{
	public abstract class GenericQueryHandler<TAggregateRoot, TId> :
		IQueryHandler<TAggregateRoot, FindByIdQuery<TId>>,
		IQueryHandler<TAggregateRoot, FindByIdsQuery<TId>>,
		IQueryHandler<TAggregateRoot, FindByFilterQuery<TAggregateRoot>>
		where TAggregateRoot : class, IAggregateRoot<TId>
	{
		protected readonly IRead<TAggregateRoot, TId> Repo;

		protected GenericQueryHandler(IRead<TAggregateRoot, TId> repo)
		{
			Repo = repo;
		}


		public virtual async Task<IQueryResult<TAggregateRoot>> FetchAsync(FindByIdQuery<TId> query) =>
			new QuerySingleResult<TAggregateRoot>(await Repo.GetAsync(query.Id));

		public virtual async Task<IQueryResult<TAggregateRoot>> FetchAsync(FindByIdsQuery<TId> query) =>
			new QueryResult<TAggregateRoot>(await Repo.GetAsync(query.Ids));

		public virtual async Task<IQueryResult<TAggregateRoot>> FetchAsync(FindByFilterQuery<TAggregateRoot> query) =>
			new QueryResult<TAggregateRoot>(await Repo.GetAsync(query.Filter));
	}
}