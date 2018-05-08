using System.Threading.Tasks;

namespace BinaryCook.Core.Commands
{
    public interface IQueryHandler<TDto, in TQuery> where TQuery : IQuery
    {
        Task<IQueryResult<TDto>> FetchAsync(TQuery query);
    }
}