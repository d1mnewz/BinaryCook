using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BinaryCook.Infrastructure.Caching
{
	public class GetDictionaryResult<T>
	{
		public Dictionary<string, T> Data { get; set; }

		public List<string> MissingKeys { get; set; }

		public GetDictionaryResult()
		{
			this.Data = new Dictionary<string, T>();
			this.MissingKeys = new List<string>();
		}

		public GetDictionaryResult(Dictionary<string, T> data, List<string> missingKeys)
		{
			this.Data = data;
			this.MissingKeys = missingKeys;
		}

		public GetDictionaryResult<T> Add(string key, T otherData)
		{
			this.Data[key] = otherData;
			return this;
		}

		public GetDictionaryResult<T> AddRange(IDictionary<string, T> otherData)
		{
			if (otherData != null)
			{
				foreach (var keyValuePair in (IEnumerable<KeyValuePair<string, T>>) otherData)
					this.Data[keyValuePair.Key] = keyValuePair.Value;
			}

			return this;
		}

		public GetDictionaryResult<T> AddMissing(string otherMissing)
		{
			this.MissingKeys.Add(otherMissing);
			return this;
		}

		public GetDictionaryResult<T> AddMissingRange(IList<string> otherMissing)
		{
			if (otherMissing != null)
				this.MissingKeys.AddRange((IEnumerable<string>) otherMissing);
			return this;
		}

		public GetDictionaryResult<T> Add(GetDictionaryResult<T> other)
		{
			if (other == null)
				return this;
			return this.AddRange((IDictionary<string, T>) other.Data).AddMissingRange((IList<string>) other.MissingKeys);
		}

		public Dictionary<string, TOther> TryExtract<TOther>(Func<T, TOther> extract)
		{
			var dictionary = new Dictionary<string, TOther>();
			foreach (var keyValuePair in this.Data ?? new Dictionary<string, T>())
			{
				try
				{
					var other = extract(keyValuePair.Value);
					dictionary.Add(keyValuePair.Key, other);
				}
				catch
				{
				}
			}

			return dictionary;
		}
	}

	public static class CacheManagerExtensions
	{
		public static void Set<T>(this ICacheManager cacheManager, string key, T obj) where T : class
		{
			cacheManager.Set<T>(new CacheItem<T>(key, obj, false));
		}

		public static void Set<T>(this ICacheManager cacheManager, string key, T obj, TimeSpan expiresIn) where T : class
		{
			cacheManager.Set<T>(new CacheItem<T>(key, obj, expiresIn, false));
		}

		public static void Set<T>(this ICacheManager cacheManager, string key, T obj, DateTimeOffset expiresAt) where T : class
		{
			cacheManager.Set<T>(new CacheItem<T>(key, obj, expiresAt, false));
		}

		public static Task SetAsync<T>(this ICacheManager cacheManager, string key, T obj) where T : class
		{
			return cacheManager.SetAsync<T>(new CacheItem<T>(key, obj, false));
		}

		public static Task SetAsync<T>(this ICacheManager cacheManager, string key, T obj, TimeSpan expiresIn) where T : class
		{
			return cacheManager.SetAsync<T>(new CacheItem<T>(key, obj, expiresIn, false));
		}

		public static Task SetAsync<T>(this ICacheManager cacheManager, string key, T obj, DateTimeOffset expiresAt) where T : class
		{
			return cacheManager.SetAsync<T>(new CacheItem<T>(key, obj, expiresAt, false));
		}

		public static T GetOrSet<T>(this ICacheManager cacheManager, string key, Func<T> acuire) where T : class
		{
			try
			{
				T obj = cacheManager.Get<T>(key);
				if ((object) obj != null)
					return obj;
			}
			catch
			{
			}

			T obj1 = acuire();
			try
			{
				if ((object) obj1 != null)
					cacheManager.Set<T>(key, obj1);
			}
			catch
			{
			}

			return obj1;
		}

		public static T GetOrSet<T>(this ICacheManager cacheManager, string key, Func<T> acuire, TimeSpan cacheTime) where T : class
		{
			try
			{
				T obj = cacheManager.Get<T>(key);
				if ((object) obj != null)
					return obj;
			}
			catch
			{
			}

			T obj1 = acuire();
			try
			{
				if ((object) obj1 != null)
					cacheManager.Set<T>(key, obj1, cacheTime);
			}
			catch
			{
			}

			return obj1;
		}

		public static T GetOrSet<T>(this ICacheManager cacheManager, string key, Func<T> acuire, int cacheSeconds) where T : class
		{
			return cacheManager.GetOrSet<T>(key, acuire, TimeSpan.FromSeconds((double) cacheSeconds));
		}

		public static async Task<T> GetOrSetAsync<T>(this ICacheManager cacheManager, string key, Func<Task<T>> acuire) where T : class
		{
			try
			{
				T async = await cacheManager.GetAsync<T>(key);
				if ((object) async != null)
					return async;
			}
			catch
			{
			}

			T obj = await acuire();
			try
			{
				if ((object) obj != null)
					await cacheManager.SetAsync<T>(key, obj);
			}
			catch
			{
			}

			return obj;
		}

		public static async Task<T> GetOrSetAsync<T>(this ICacheManager cacheManager, string key, Func<Task<T>> acuire, TimeSpan cacheTime) where T : class
		{
			try
			{
				T async = await cacheManager.GetAsync<T>(key);
				if ((object) async != null)
					return async;
			}
			catch
			{
			}

			T obj = await acuire();
			try
			{
				if ((object) obj != null)
					await cacheManager.SetAsync<T>(key, obj, cacheTime);
			}
			catch
			{
			}

			return obj;
		}

		public static Task<T> GetOrSetAsync<T>(this ICacheManager cacheManager, string key, Func<Task<T>> acuire, int cacheSeconds) where T : class
		{
			return cacheManager.GetOrSetAsync<T>(key, acuire, TimeSpan.FromSeconds((double) cacheSeconds));
		}
	}

	public class GetListResult<T>
	{
		public List<T> Data { get; set; }

		public List<string> MissingKeys { get; set; }

		public GetListResult()
		{
			this.Data = new List<T>();
			this.MissingKeys = new List<string>();
		}

		public GetListResult(List<T> data, List<string> missingKeys)
		{
			this.Data = data;
			this.MissingKeys = missingKeys;
		}

		public GetListResult<T> Add(T otherData)
		{
			this.Data.Add(otherData);
			return this;
		}

		public GetListResult<T> AddRange(IList<T> otherData)
		{
			if (otherData != null)
				this.Data.AddRange((IEnumerable<T>) otherData);
			return this;
		}

		public GetListResult<T> AddMissing(string otherMissing)
		{
			this.MissingKeys.Add(otherMissing);
			return this;
		}

		public GetListResult<T> AddMissingRange(IList<string> otherMissing)
		{
			if (otherMissing != null)
				this.MissingKeys.AddRange((IEnumerable<string>) otherMissing);
			return this;
		}

		public GetListResult<T> Add(GetListResult<T> other)
		{
			if (other == null)
				return this;
			return this.AddRange((IList<T>) other.Data).AddMissingRange((IList<string>) other.MissingKeys);
		}
	}

	public class CacheItemList<T>
	{
		public List<Tuple<string, T>> Data { get; private set; }

		public TimeSpan? ExpiresIn { get; private set; }

		public DateTimeOffset? ExpiresAt { get; private set; }

		public bool FireAndForget { get; private set; }

		public CacheItemList()
		{
		}

		public CacheItemList(List<Tuple<string, T>> data, bool fireAndForget = false)
		{
			this.Data = data;
			this.FireAndForget = fireAndForget;
		}

		public CacheItemList(List<Tuple<string, T>> data, TimeSpan expiresIn, bool fireAndForget = false)
			: this(data, fireAndForget)
		{
			this.ExpiresIn = new TimeSpan?(expiresIn);
		}

		public CacheItemList(List<Tuple<string, T>> data, DateTimeOffset expiresAt, bool fireAndForget = false)
			: this(data, fireAndForget)
		{
			this.ExpiresAt = new DateTimeOffset?(expiresAt);
		}
	}

	public class CacheItem<T>
	{
		public string Key { get; private set; }

		public T Data { get; private set; }

		public TimeSpan? ExpiresIn { get; private set; }

		public DateTimeOffset? ExpiresAt { get; private set; }

		public bool FireAndForget { get; private set; }

		public CacheItem()
		{
		}

		public CacheItem(string key, T data, bool fireAndForget = false)
		{
			this.Key = key;
			this.Data = data;
			this.FireAndForget = fireAndForget;
		}

		public CacheItem(string key, T data, TimeSpan expiresIn, bool fireAndForget = false)
			: this(key, data, fireAndForget)
		{
			this.ExpiresIn = new TimeSpan?(expiresIn);
		}

		public CacheItem(string key, T data, DateTimeOffset expiresAt, bool fireAndForget = false)
			: this(key, data, fireAndForget)
		{
			this.ExpiresAt = new DateTimeOffset?(expiresAt);
		}
	}

	public interface ICacheManager
	{
		T Get<T>(string key) where T : class;

		GetListResult<T> Get<T>(IEnumerable<string> keys) where T : class;

		GetDictionaryResult<T> GetAsDictionary<T>(IEnumerable<string> keys) where T : class;

		void Set<T>(CacheItem<T> item) where T : class;

		void Set<T>(CacheItemList<T> items) where T : class;

		bool Exists(string key);

		void Remove(string key);

		void RemoveAll(IList<string> keys);

		int RemoveByPattern(string pattern);

		string[] GetHashSet(string key);

		void PushToHashSet(string key, string id);

		void PushToHashSet(string key, IEnumerable<string> ids);

		void RemoveFromHashSet(string key, string id);

		void RemoveFromHashSet(string key, IEnumerable<string> ids);

		void Inc(string key, double incBy);

		void Clear();

		Task<object> GetAsync(string key);

		Task<T> GetAsync<T>(string key) where T : class;

		Task<GetListResult<T>> GetAsync<T>(IEnumerable<string> keys) where T : class;

		Task SetAsync<T>(CacheItem<T> item) where T : class;

		Task SetAsync<T>(CacheItemList<T> items) where T : class;

		Task<bool> ExistsAsync(string key);

		Task RemoveAsync(string key);

		Task RemoveAllAsync(IList<string> keys);

		Task<int> RemoveByPatternAsync(string pattern);

		Task<string[]> GetHashSetAsync(string key);

		Task PushToHashSetAsync(string key, string id);

		Task PushToHashSetAsync(string key, IEnumerable<string> ids);

		Task RemoveFromHashSetAsync(string key, string id);

		Task RemoveFromHashSetAsync(string key, IEnumerable<string> ids);

		Task IncAsync(string key, double incBy);

		Task ClearAsync();
	}
}