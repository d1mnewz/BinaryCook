using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace BinaryCook.Core.Code
{
    /// <summary>
    /// Simple maybe implementation. Consider moving to advanced implementation when needed! (when needed is a key point here!)
    /// E.g.: https://github.com/AndreyTsvetkov/Functional.Maybe
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Maybe<T> :
        IEnumerable<T>,
        IEquatable<Maybe<T>>,
        IComparable<Maybe<T>>
    {
        public static Maybe<T> Some(T item) => new Maybe<T>(item);
        public static Maybe<T> Nothing => new Maybe<T>();

        private readonly T[] _data;
        private bool HasValue => _data.Length > 0;

        private Maybe()
        {
            _data = new T[] { };
        }

        private Maybe(T item)
        {
            _data = new[] { item };
        }

        public IEnumerator<T> GetEnumerator() => ((IEnumerable<T>)_data).GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => _data.GetEnumerator();

        public static explicit operator T(Maybe<T> maybe) => maybe.FirstOrDefault();

        public static implicit operator Maybe<T>(T value) => Some(value);

        public override bool Equals(object obj)
        {
            if (obj is Maybe<T> maybe)
                return Equals(maybe);
            return false;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var value = HasValue.GetHashCode();

                if (HasValue)
                    value = value ^ EqualityComparer<T>.Default.GetHashCode(_data.First()) * 397;

                return value;
            }
        }

        public bool Equals(Maybe<T> other)
        {
            if (other == null)
                return false;

            if (HasValue && other.HasValue)
                return EqualityComparer<T>.Default.Equals(_data.First(), other._data.First());
            return HasValue == other.HasValue;
        }

        public int CompareTo(Maybe<T> other)
        {
            if (HasValue && !other.HasValue) return 1;
            if (!HasValue && other.HasValue) return -1;

            return Comparer<T>.Default.Compare(_data.First(), other._data.First());
        }
    }
}