using System;
using System.Collections.Generic;
using System.Reflection;

// ReSharper disable NonReadonlyMemberInGetHashCode

namespace BinaryCook.Core.Data.Entities
{
    public interface IValueObject<T>
    {
    }

    [Serializable]
    public abstract class ValueObject<T> : IValueObject<T>, IEquatable<T> where T : class, IValueObject<T>
    {
        private int? _cachedHash;

        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;

            var other = obj as T;
            return Equals(other);
        }

        public override int GetHashCode()
        {
            if (_cachedHash.HasValue) return _cachedHash.Value;

            const int startValue = 17;
            const int multiplier = 59;
            var hashCode = startValue;

            foreach (var field in GetFields(GetType()))
            {
                var value = field.GetValue(this);

                if (value != null)
                    hashCode = hashCode * multiplier + value.GetHashCode();
            }
            _cachedHash = hashCode;

            return _cachedHash.Value;
        }

        public virtual bool Equals(T other)
        {
            if (other == null)
                return false;

            var t = GetType();
            var otherType = other.GetType();

            if (t != otherType)
                return false;

            foreach (var field in GetFields(t))
            {
                var value1 = field.GetValue(other);
                var value2 = field.GetValue(this);

                if (value1 == null)
                {
                    if (value2 != null)
                        return false;
                }

                else if (!value1.Equals(value2))
                    return false;
            }

            return true;
        }


        private static IEnumerable<FieldInfo> GetFields(Type t)
        {
            var fields = new List<FieldInfo>();

            while (t != typeof(object))
            {
                fields.AddRange(t.GetFields(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public));
                t = t.BaseType;
            }

            return fields;
        }

        public static bool operator ==(ValueObject<T> x, ValueObject<T> y) => x.Equals(y);
        public static bool operator !=(ValueObject<T> x, ValueObject<T> y) => !(x == y);
    }
}