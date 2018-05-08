using System.Collections.Generic;
using BinaryCook.Core.Commands;

namespace BinaryCook.Application.Core.Quering
{
    public class FindByIdsQuery<TId> : IQuery
    {
        public IList<TId> Ids { get; }

        public FindByIdsQuery(IList<TId> ids)
        {
            Ids = ids;
        }
    }
}