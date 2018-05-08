using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using StackExchange.Redis;
using StackExchange.Redis.Extensions.Core;

namespace BinaryCook.Infrastructure.Caching
{
    public class RedisCacheManager : ICacheManager
    {
        protected readonly StackExchangeRedisCacheClient Client;

        public RedisCacheManager(IConnectionMultiplexer redis, ISerializer serializer)
        {
            Client = new StackExchangeRedisCacheClient(redis, serializer);
        }

        public T Get<T>(string key) where T : class => Client.Get<T>(key);

        public GetListResult<T> Get<T>(IEnumerable<string> keys) where T : class => Client.GetAllPropper<T>(keys);

        public GetDictionaryResult<T> GetAsDictionary<T>(IEnumerable<string> keys) where T : class => Client.GetAllPropperAsDictionary<T>(keys);

        public void Set<T>(CacheItem<T> item) where T : class => Client.Add(item.Key, item.Data, item.ExpiresAt, item.ExpiresIn, item.FireAndForget);

        public void Set<T>(CacheItemList<T> items) where T : class => Client.AddAll(items.Data, items.ExpiresAt, items.ExpiresIn, items.FireAndForget);

        public bool Exists(string key) => Client.Exists(key);

        public void Remove(string key) => Client.Remove(key);

        public void RemoveAll(IList<string> keys) => Client.RemoveAllKeys(keys, true);

        public int RemoveByPattern(string pattern)
        {
            var result = Client.Database.ScriptEvaluate("return redis.call('del', unpack(redis.call('keys', ARGV[1])))", null,
                new RedisValue[] {pattern});
            try
            {
                return Convert.ToInt32(result.ToString().Split(" ".ToCharArray(), StringSplitOptions.RemoveEmptyEntries)[0]);
            }
            catch (Exception)
            {
                return 0;
            }
        }

        public string[] GetHashSet(string key) => Client.SetMember(key);

        public void PushToHashSet(string key, string id) => Client.SetAdd(key, id);

        public void PushToHashSet(string key, IEnumerable<string> ids) => Client.SetAdd(key, ids);

        public void RemoveFromHashSet(string key, string id) => Client.SetRemove(key, id);

        public void RemoveFromHashSet(string key, IEnumerable<string> ids) => Client.SetRemove(key, ids);

        public void Inc(string key, double incBy) => Client.Database.StringIncrement(key, incBy, CommandFlags.FireAndForget);

        public void Clear() => Client.FlushDb();

        public Task<object> GetAsync(string key) => Client.GetAsync<object>(key);

        public Task<T> GetAsync<T>(string key) where T : class => Client.GetAsync<T>(key);

        public Task<GetListResult<T>> GetAsync<T>(IEnumerable<string> keys) where T : class => Client.GetAllPropperAsync<T>(keys);

        public Task SetAsync<T>(CacheItem<T> item) where T : class => Client.AddAsync(item.Key, item.Data, item.ExpiresAt, item.ExpiresIn, item.FireAndForget);

        public Task SetAsync<T>(CacheItemList<T> items) where T : class =>
            Client.AddAllAsync(items.Data, items.ExpiresAt, items.ExpiresIn, items.FireAndForget);

        public Task<bool> ExistsAsync(string key) => Client.ExistsAsync(key);

        public Task RemoveAsync(string key) => Client.RemoveAsync(key);

        public Task RemoveAllAsync(IList<string> keys) => Client.RemoveAllKeysAsync(keys, true);

        public Task<int> RemoveByPatternAsync(string pattern)
        {
            var result = Client.Database.ScriptEvaluate("return redis.call('del', unpack(redis.call('keys', ARGV[1])))", null,
                new RedisValue[] {pattern});
            try
            {
                return Task.FromResult(Convert.ToInt32(result.ToString().Split(" ".ToCharArray(), StringSplitOptions.RemoveEmptyEntries)[0]));
            }
            catch (Exception)
            {
                return Task.FromResult(0);
            }
        }

        public Task<string[]> GetHashSetAsync(string key) => Client.SetMemberAsync(key);

        public Task PushToHashSetAsync(string key, string id) => Client.SetAddAsync(key, id);

        public Task PushToHashSetAsync(string key, IEnumerable<string> ids) => Client.SetAddAsync(key, ids);

        public Task RemoveFromHashSetAsync(string key, string id) => Client.SetRemoveAsync(key, id);

        public Task RemoveFromHashSetAsync(string key, IEnumerable<string> ids) => Client.SetRemoveAsync(key, ids);

        public Task IncAsync(string key, double incBy) => Client.Database.StringIncrementAsync(key, incBy, CommandFlags.FireAndForget);

        public Task ClearAsync() => Client.FlushDbAsync();
    }
}