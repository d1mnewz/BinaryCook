﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;

namespace BinaryCook.Infrastructure.Caching
{
	public class MemoryCacheManager : ICacheManager
	{
		private readonly TimeSpan _defaultCacheTime;
		private readonly IOptions<MemoryCacheOptions> _options;

		public MemoryCacheManager(TimeSpan? defaultCacheTime = null)
		{
			_defaultCacheTime = defaultCacheTime ?? TimeSpan.FromDays(1.0);
			_options = Options.Create(new MemoryCacheOptions());
			Cache = new MemoryCache(_options);
		}

		private IMemoryCache Cache { get; set; }

		public T Get<T>(string key) where T : class
		{
			return Cache.Get<T>(key);
		}

		public GetListResult<T> Get<T>(IEnumerable<string> keys) where T : class
		{
			var missingKeys = new List<string>();
			var data = new List<T>();
			foreach (var key in keys)
			{
				var obj = Get<T>(key);
				if (obj == null)
					missingKeys.Add(key);
				else
					data.Add(obj);
			}

			return new GetListResult<T>(data, missingKeys);
		}

		public GetDictionaryResult<T> GetAsDictionary<T>(IEnumerable<string> keys) where T : class
		{
			var missingKeys = new List<string>();
			var data = new Dictionary<string, T>();
			foreach (var key in keys)
			{
				var obj = Get<T>(key);
				if (obj == null)
					missingKeys.Add(key);
				else
					data.Add(key, obj);
			}

			return new GetDictionaryResult<T>(data, missingKeys);
		}

		public void Set<T>(CacheItem<T> item) where T : class
		{
			if (item == null)
				return;
			var expiresIn = item.ExpiresIn;
			TimeSpan timeSpan;
			if (!expiresIn.HasValue)
			{
				var expiresAt = item.ExpiresAt;
				// ISSUE: explicit reference operation
				// ISSUE: variable of a reference type
				var local = expiresAt;
				// ISSUE: explicit reference operation
				// ISSUE: explicit reference operation
				timeSpan = local?.Subtract(DateTimeOffset.Now) ?? _defaultCacheTime;
			}
			else
				timeSpan = expiresIn.GetValueOrDefault();

			var absoluteExpirationRelativeToNow = timeSpan;
			// ISSUE: variable of a boxed type
			Cache.Set(item.Key, item.Data, absoluteExpirationRelativeToNow);
		}

		public void Set<T>(CacheItemList<T> items) where T : class
		{
			if (items == null || !items.Data.Any())
				return;
			var expiresIn = items.ExpiresIn;
			TimeSpan timeSpan;
			if (!expiresIn.HasValue)
			{
				var expiresAt = items.ExpiresAt;
				// ISSUE: explicit reference operation
				// ISSUE: variable of a reference type
				var local = expiresAt;
				// ISSUE: explicit reference operation
				// ISSUE: explicit reference operation
				timeSpan = local?.Subtract(DateTimeOffset.Now) ?? _defaultCacheTime;
			}
			else
				timeSpan = expiresIn.GetValueOrDefault();

			var absoluteExpirationRelativeToNow = timeSpan;
			foreach (var tuple in items.Data)
			{
				// ISSUE: variable of a boxed type
				Cache.Set(tuple.Item1, tuple.Item2, absoluteExpirationRelativeToNow);
			}
		}

		public bool Exists(string key)
		{
			object obj;
			return Cache.TryGetValue(key, out obj);
		}

		public void Remove(string key)
		{
			Cache.Remove(key);
		}

		public void RemoveAll(IList<string> keys)
		{
			foreach (var key in keys)
				Remove(key);
		}

		public int RemoveByPattern(string pattern)
		{
			throw new NotImplementedException();
		}

		public string[] GetHashSet(string key)
		{
			throw new NotImplementedException();
		}

		public void PushToHashSet(string key, string id)
		{
			throw new NotImplementedException();
		}

		public void PushToHashSet(string key, IEnumerable<string> ids)
		{
			throw new NotImplementedException();
		}

		public void RemoveFromHashSet(string key, string id)
		{
			throw new NotImplementedException();
		}

		public void RemoveFromHashSet(string key, IEnumerable<string> ids)
		{
			throw new NotImplementedException();
		}

		public void Inc(string key, double incBy)
		{
			throw new NotImplementedException();
		}

		public void Clear()
		{
			Cache.Dispose();
			Cache = new MemoryCache(_options);
		}

		public Task<object> GetAsync(string key)
		{
			return Task.FromResult(Cache.Get(key));
		}

		public Task<T> GetAsync<T>(string key) where T : class
		{
			return Task.FromResult(Get<T>(key));
		}

		public Task<GetListResult<T>> GetAsync<T>(IEnumerable<string> keys) where T : class
		{
			return Task.FromResult(Get<T>(keys));
		}

		public Task SetAsync<T>(CacheItem<T> item) where T : class
		{
			Set(item);
			return Task.FromResult(true);
		}

		public Task SetAsync<T>(CacheItemList<T> items) where T : class
		{
			Set(items);
			return Task.FromResult(true);
		}

		public Task<bool> ExistsAsync(string key)
		{
			return Task.FromResult(Exists(key));
		}

		public Task RemoveAsync(string key)
		{
			Remove(key);
			return Task.FromResult(true);
		}

		public Task RemoveAllAsync(IList<string> keys)
		{
			RemoveAll(keys);
			return Task.FromResult(true);
		}

		public Task<string[]> GetHashSetAsync(string key)
		{
			return Task.FromResult(GetHashSet(key));
		}

		public Task PushToHashSetAsync(string key, string id)
		{
			PushToHashSet(key, id);
			return Task.FromResult(true);
		}

		public Task PushToHashSetAsync(string key, IEnumerable<string> ids)
		{
			PushToHashSet(key, ids);
			return Task.FromResult(true);
		}

		public Task RemoveFromHashSetAsync(string key, string id)
		{
			RemoveFromHashSet(key, id);
			return Task.FromResult(true);
		}

		public Task RemoveFromHashSetAsync(string key, IEnumerable<string> ids)
		{
			RemoveFromHashSet(key, ids);
			return Task.FromResult(true);
		}

		public Task IncAsync(string key, double incBy)
		{
			Inc(key, incBy);
			return Task.FromResult(true);
		}

		public Task<int> RemoveByPatternAsync(string pattern)
		{
			return Task.FromResult(RemoveByPattern(pattern));
		}

		public Task ClearAsync()
		{
			Clear();
			return Task.FromResult(true);
		}
	}
}