using BinaryCook.Core.Commands;
using BinaryCook.Core.Data.AggregateRoots;

namespace BinaryCook.Application.Core.Quering
{
  public class FindByFilterQuery<T> : IQuery where T : class, IAggregateRoot
    {
        public IFilter<T> Filter { get; }

        public FindByFilterQuery(IFilter<T> filter)
        {
            Filter = filter;
        }
    }
}