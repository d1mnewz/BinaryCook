using System;
using System.Collections.Generic;
using System.Linq;
using BinaryCook.Core.Code;

namespace BinaryCook.Core.Extensions
{
    public static class RequiresExtensions
    {
        private static IEnumerable<Required<T>> Require<T>(this IEnumerable<Required<T>> list, Action<T> that) => list.Select(x =>
            {
                that(x.Item);
                return x;
            }
        );

        public static IEnumerable<Required<T>> NotNull<T>(this IEnumerable<Required<T>> list, string argumentName) =>
            list.Require(x => Requires.NotNull(x, argumentName));

        public static IEnumerable<Required<string>> NotEmpty(this IEnumerable<Required<string>> list, string argumentName) =>
            list.Require(x => Requires.NotEmpty(x, argumentName));

        public static IEnumerable<Required<T>> That<T>(this IEnumerable<Required<T>> list, Func<T, bool> predicate, string message) =>
            list.Require(x => Requires.That(predicate(x), message));

        public static void Do<T>(this IEnumerable<Required<T>> list, Action<T> action)
        {
            foreach (var item in list)
            {
                action(item.Item);
            }
        }
    }

    public class Required<T>
    {
        public T Item { get; }

        public Required(T item)
        {
            Item = item;
        }
    }
}