using BinaryCook.Core.Data.Repositories;
using BinaryCook.Core.Data.Specifications;

namespace BinaryCook.Core.Data.AggregateRoots
{
    public interface IFilter
    {
        IPagedInfo ToPagedInfo();
    }

    public interface IFilter<T> : IFilter where T : class, IAggregateRoot
    {
        ISorter<T> ToSorter();
        ISpecification<T> ToSpecification();
    }
}