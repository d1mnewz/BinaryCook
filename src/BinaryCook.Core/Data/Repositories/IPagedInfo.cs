using System.Linq;

namespace BinaryCook.Core.Data.Repositories
{
    public interface IPagedInfo
    {
        int Page { get; }
        int? PageSize { get; }
    }

    public class PagedInfo : IPagedInfo
    {
        public PagedInfo(int page, int? pageSize)
        {
            Page = page;
            PageSize = pageSize;
        }

        public int Page { get; }
        public int? PageSize { get; }

        public static IPagedInfo Default => new PagedInfo(0, null);
    }

    public static class PagedInfoExtensions
    {
        public static IQueryable<T> SafeApply<T>(this IQueryable<T> query, IPagedInfo pagedInfo) =>
            pagedInfo?.PageSize == null ? query : query.Skip(pagedInfo.Page * pagedInfo.PageSize.Value).Take(pagedInfo.PageSize.Value);
    }
}