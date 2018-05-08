using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using BinaryCook.Core.Code;

namespace BinaryCook.Infrastructure.Caching
{
    public class InMemoryAsyncEnumerable<T> : IAsyncEnumerable<T>
    {
        private readonly IEnumerator<T> _inner;

        /// <summary>
        /// Asynchronous version of the IEnumerable interface
        /// </summary>
        public InMemoryAsyncEnumerable(IEnumerator<T> inner)
        {
            Requires.NotNull(inner, nameof(inner));
            _inner = inner;
        }

        /// <inheritdoc />
        /// <summary>
        /// Gets an asynchronous enumerator over the sequence.
        /// </summary>
        public IAsyncEnumerator<T> GetEnumerator() => new InMemoryAsyncEnumerator<T>(_inner);
    }

    public class InMemoryAsyncEnumerator<T> : IAsyncEnumerator<T>
    {
        private readonly IEnumerator<T> _inner;

        /// <summary>
        /// Asynchronous version of the IEnumerator of T interface that allows elements to be retrieved asynchronously.
        /// </summary>
        /// <param name="inner">The inner IEnumerator</param>
        public InMemoryAsyncEnumerator(IEnumerator<T> inner)
        {
            Requires.NotNull(inner, nameof(inner));
            _inner = inner;
        }

        /// <inheritdoc />
        /// <summary>
        /// Gets the current element in the iteration.
        /// </summary>
        public T Current => _inner.Current;

        /// <inheritdoc />
        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            _inner.Dispose();
        }

        /// <inheritdoc />
        /// <summary>
        /// Advances the enumerator to the next element in the sequence, returning the result asynchronously.
        /// </summary>
        /// <param name="cancellationToken">A System.Threading.CancellationToken to observe while waiting for the task to complete.</param>
        /// <returns>A task that represents the asynchronous operation.  The task result contains true if the enumerator was successfully advanced to the next element; false if the enumerator has passed the end of the sequence.</returns>
        public Task<bool> MoveNext(CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            return Task.FromResult(_inner.MoveNext());
        }
    }
}