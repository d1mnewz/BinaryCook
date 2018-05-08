using System;
using System.ComponentModel;
using System.Globalization;

namespace BinaryCook.Core.Code
{
    public static class DataTypeConverter
    {
        public static T Convert<T>(string value, T fallback = default(T), CultureInfo culture = null)
        {
            var foo = TypeDescriptor.GetConverter(typeof(T));
            try
            {
                return (T) foo.ConvertFromString(null, culture ?? CultureInfo.InvariantCulture, value);
            }
            catch
            {
                return fallback;
            }
        }

        public static string ConvertAsString<T>(T value, CultureInfo culture = null)
        {
            var converter = TypeDescriptor.GetConverter(typeof(T));
            return converter.CanConvertTo(typeof(string))
                ? converter.ConvertToString(null, culture ?? CultureInfo.InvariantCulture, value)
                : value.ToString();
        }

        public static bool TryConvert(Type type, string value, CultureInfo culture = null)
        {
            var foo = TypeDescriptor.GetConverter(type);
            try
            {
                foo.ConvertFromString(null, culture ?? CultureInfo.InvariantCulture, value);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}