using BinaryCook.Core.Commands;

namespace BinaryCook.Application.Core.Quering
{
    public class FindByIdQuery<TId> : IQuery
    {
        public TId Id { get; }

        public FindByIdQuery(TId id)
        {
            Id = id;
        }
    }
}