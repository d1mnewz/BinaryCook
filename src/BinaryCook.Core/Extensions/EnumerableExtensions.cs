using System;
using System.Collections.Generic;
using System.Linq;

namespace BinaryCook.Core.Extensions
{
	public static class EnumerableExtensions
	{
		public static IEnumerable<T> Safe<T>(this IEnumerable<T> other) => other ?? new T[0];

		public static void ForEach<T>(this IEnumerable<T> other, Action<T> action)
		{
			foreach (var item in other)
				action(item);
		}

		public static IEnumerable<Required<T>> Required<T>(this IEnumerable<T> list) => list.Safe().Select(item => new Required<T>(item));

		public static void Do<T>(this IEnumerable<T> list, Action<T> action)
		{
			foreach (var item in list.Safe())
				action(item);
		}

		public static bool IsEmpty<T>(this IEnumerable<T> collection)
		{
			if (collection != null)
				return !collection.Any<T>();
			return true;
		}
	}
}