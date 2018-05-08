using System;
using System.Collections.Generic;
using System.Diagnostics;
using BinaryCook.Core.Extensions;

namespace BinaryCook.Core.Code
{
    public class Requires
    {
        public static void That<TException>(bool predicate, string message)
            where TException : Exception, new()
        {
            if (predicate) return;

            Debug.WriteLine(message);
            throw new TException();
        }

        public static void That(bool predicate, string message)
        {
            That<ArgumentException>(predicate, message);
        }

        public static void NotNull<T>(T obj, string argumentName, string messagePattern = null)
        {
            That<ArgumentNullException>(obj != null,
                messagePattern != null ? string.Format(messagePattern, argumentName) : $"{argumentName} should not be null");
        }

        public static void NotEmpty(Guid guid, string argumentName, string messagePattern = null)
        {
            That<ArgumentException>(guid != Guid.Empty,
                messagePattern != null ? string.Format(messagePattern, argumentName) : $"{argumentName} cannot be empty");
        }

        public static void NotEmpty(string str, string argumentName, string messagePattern = null)
        {
            That<ArgumentNullException>(str != null,
                messagePattern != null ? string.Format(messagePattern, argumentName) : $"{argumentName} cannot be null");
            That<ArgumentException>(!str.IsEmptyString(),
                messagePattern != null ? string.Format(messagePattern, argumentName) : $"{argumentName} cannot be empty");
        }

        public static void NotEmpty<T>(IList<T> list, string argumentName, string messagePattern = null)
        {
            That<ArgumentNullException>(list != null,
                messagePattern != null ? string.Format(messagePattern, argumentName) : $"{argumentName} cannot be null");
            That<ArgumentException>(!list.IsEmpty(),
                messagePattern != null ? string.Format(messagePattern, argumentName) : $"{argumentName} cannot be empty");
        }
    }
}