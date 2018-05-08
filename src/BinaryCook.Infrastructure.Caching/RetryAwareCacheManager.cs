using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Polly;
using Polly.Retry;

namespace BinaryCook.Infrastructure.Caching
{
    public class RetryAwareCacheManager : ICacheManager
    {
        private readonly ICacheManager _cacheManager;
        private readonly RetryPolicy _syncPolicy;
        private readonly RetryPolicy _asyncPolicy;

        public RetryAwareCacheManager(ICacheManager cacheManager, int retryInterval, int retryCount)
        {
            _cacheManager = cacheManager;

            _syncPolicy = Policy.Handle<Exception>()
                .WaitAndRetry(retryCount, count => TimeSpan.FromSeconds(count * retryInterval));
            _asyncPolicy = Policy.Handle<Exception>()
                .WaitAndRetryAsync(retryCount, count => TimeSpan.FromSeconds(count * retryInterval));
        }


        public T Get<T>(string key) where T : class => _syncPolicy.Execute(() => _cacheManager.Get<T>(key));

        public GetListResult<T> Get<T>(IEnumerable<string> keys) where T : class => _syncPolicy.Execute(() => _cacheManager.Get<T>(keys));

        public GetDictionaryResult<T> GetAsDictionary<T>(IEnumerable<string> keys) where T : class =>
            _syncPolicy.Execute(() => _cacheManager.GetAsDictionary<T>(keys));

        public void Set<T>(CacheItem<T> item) where T : class => _syncPolicy.Execute(() => _cacheManager.Set(item));

        public void Set<T>(CacheItemList<T> items) where T : class => _syncPolicy.Execute(() => _cacheManager.Set(items));

        public bool Exists(string key) => _syncPolicy.Execute(() => _cacheManager.Exists(key));

        public void Remove(string key) => _syncPolicy.Execute(() => _cacheManager.Remove(key));

        public void RemoveAll(IList<string> keys) => _syncPolicy.Execute(() => _cacheManager.RemoveAll(keys));

        public int RemoveByPattern(string pattern) => _syncPolicy.Execute(() => _cacheManager.RemoveByPattern(pattern));

        public string[] GetHashSet(string key) => _syncPolicy.Execute(() => _cacheManager.GetHashSet(key));

        public void PushToHashSet(string key, string id) => _syncPolicy.Execute(() => _cacheManager.PushToHashSet(key, id));

        public void PushToHashSet(string key, IEnumerable<string> ids) => _syncPolicy.Execute(() => _cacheManager.PushToHashSet(key, ids));

        public void RemoveFromHashSet(string key, string id) => _syncPolicy.Execute(() => _cacheManager.RemoveFromHashSet(key, id));

        public void RemoveFromHashSet(string key, IEnumerable<string> ids) => _syncPolicy.Execute(() => _cacheManager.RemoveFromHashSet(key, ids));

        public void Inc(string key, double incBy) => _syncPolicy.Execute(() => _cacheManager.Inc(key, incBy));

        public void Clear() => _syncPolicy.Execute(() => _cacheManager.Clear());

        public Task<object> GetAsync(string key) => _asyncPolicy.ExecuteAsync(() => _cacheManager.GetAsync(key));

        public Task<T> GetAsync<T>(string key) where T : class => _asyncPolicy.ExecuteAsync(() => _cacheManager.GetAsync<T>(key));

        public Task<GetListResult<T>> GetAsync<T>(IEnumerable<string> keys) where T : class => _asyncPolicy.ExecuteAsync(() => _cacheManager.GetAsync<T>(keys));

        public Task SetAsync<T>(CacheItem<T> item) where T : class => _asyncPolicy.ExecuteAsync(() => _cacheManager.SetAsync(item));

        public Task SetAsync<T>(CacheItemList<T> items) where T : class => _asyncPolicy.ExecuteAsync(() => _cacheManager.SetAsync(items));

        public Task<bool> ExistsAsync(string key) => _asyncPolicy.ExecuteAsync(() => _cacheManager.ExistsAsync(key));

        public Task RemoveAsync(string key) => _asyncPolicy.ExecuteAsync(() => _cacheManager.RemoveAsync(key));

        public Task RemoveAllAsync(IList<string> keys) => _asyncPolicy.ExecuteAsync(() => _cacheManager.RemoveAllAsync(keys));

        public Task<int> RemoveByPatternAsync(string pattern) => _asyncPolicy.ExecuteAsync(() => _cacheManager.RemoveByPatternAsync(pattern));

        public Task<string[]> GetHashSetAsync(string key) => _asyncPolicy.ExecuteAsync(() => _cacheManager.GetHashSetAsync(key));

        public Task PushToHashSetAsync(string key, string id) => _asyncPolicy.ExecuteAsync(() => _cacheManager.PushToHashSetAsync(key, id));

        public Task PushToHashSetAsync(string key, IEnumerable<string> ids) => _asyncPolicy.ExecuteAsync(() => _cacheManager.PushToHashSetAsync(key, ids));

        public Task RemoveFromHashSetAsync(string key, string id) => _asyncPolicy.ExecuteAsync(() => _cacheManager.RemoveFromHashSetAsync(key, id));

        public Task RemoveFromHashSetAsync(string key, IEnumerable<string> ids) =>
            _asyncPolicy.ExecuteAsync(() => _cacheManager.RemoveFromHashSetAsync(key, ids));

        public Task IncAsync(string key, double incBy) => _asyncPolicy.ExecuteAsync(() => _cacheManager.IncAsync(key, incBy));

        public Task ClearAsync() => _asyncPolicy.ExecuteAsync(() => _cacheManager.ClearAsync());
    }
}