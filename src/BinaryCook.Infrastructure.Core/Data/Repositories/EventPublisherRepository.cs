using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BinaryCook.Core.Data.Entities;
using BinaryCook.Core.Data.Events;
using BinaryCook.Core.Data.Repositories;
using BinaryCook.Core.Messaging;

namespace BinaryCook.Infrastructure.Core.Data.Repositories
{
    public class SaveEventPublisher<TRoot, TId> : ISave<TRoot, TId> where TRoot : class, IEntity<TId>
    {
        private readonly ISave<TRoot, TId> _save;
        private readonly IBus _bus;

        public SaveEventPublisher(ISave<TRoot, TId> save, IBus bus)
        {
            _save = save;
            _bus = bus;
        }

        public void Dispose()
        {
            _save.Dispose();
        }

        public async Task InsertAsync(TRoot aggregate)
        {
            await _save.InsertAsync(aggregate);
            await _bus.Emit(new DomainInserted<TRoot>(aggregate));
        }

        public async Task InsertAsync(IEnumerable<TRoot> aggregates)
        {
            var items = aggregates.ToList();
            await _save.InsertAsync(items);
            await Task.WhenAll(items.Select(aggregate => _bus.Emit(new DomainInserted<TRoot>(aggregate))));
        }

        public async Task UpdateAsync(TRoot aggregate)
        {
            await _save.UpdateAsync(aggregate);
            await _bus.Emit(new DomainUpdated<TRoot>(aggregate));
        }
    }

    public class DeleteEventPublisher<TRoot, TId> : IDelete<TRoot, TId> where TRoot : class, IEntity<TId>
    {
        private readonly IDelete<TRoot, TId> _delete;
        private readonly IBus _bus;

        public DeleteEventPublisher(IDelete<TRoot, TId> delete, IBus bus)
        {
            _delete = delete;
            _bus = bus;
        }

        public void Dispose()
        {
            _delete.Dispose();
        }

        public async Task<bool> DeleteAsync(TRoot aggregate)
        {
            var result = await _delete.DeleteAsync(aggregate);
            if (result)
                await _bus.Emit(new DomainDeleted<TRoot>(aggregate));
            return result;
        }
    }
}