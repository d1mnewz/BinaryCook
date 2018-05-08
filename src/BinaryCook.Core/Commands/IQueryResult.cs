using System.Collections.Generic;
using System.Threading.Tasks;

namespace BinaryCook.Core.Commands
{
    //TODO: Consider about IPagedList
    public interface IQueryResult<out TDto> : IEnumerable<TDto>
    {
    }

    public class QueryResult<TDto> : List<TDto>, IQueryResult<TDto>
    {
        public QueryResult(IEnumerable<TDto> items) : base(items)
        {
        }
    }

    public class QuerySingleResult<TDto> : QueryResult<TDto>
    {
        public QuerySingleResult(TDto item) : base(new List<TDto>())
        {
            if (item != null)
                Add(item);
        }
    }

    public static class QueryResultExtensions
    {
        public static QueryResult<TDto> AsQueryResult<TDto>(this IEnumerable<TDto> list) => new QueryResult<TDto>(list);

        public static QuerySingleResult<TDto> AsQueryResult<TDto>(this TDto item) => new QuerySingleResult<TDto>(item);

        public static async Task<QuerySingleResult<TDto>> AsQueryResult<TDto>(this Task<TDto> item) => new QuerySingleResult<TDto>(await item);
    }
}