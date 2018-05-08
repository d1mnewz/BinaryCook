using System.Linq;

namespace BinaryCook.Core.Data.Repositories
{
    public interface ISorter<T>
    {
        IOrderedQueryable<T> Sort(IQueryable<T> query);
    }

    public static class SorterExtensions
    {
        public static IQueryable<T> SafeSort<T>(this IQueryable<T> query, ISorter<T> sorter) => sorter?.Sort(query) ?? query;
    }
}