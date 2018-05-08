using System.Collections.Generic;
using System.Linq;
using System.Text;
using BinaryCook.Core.Extensions;

namespace BinaryCook.Core.Commands
{
    public interface IValidationResult
    {
        bool IsValid { get; }
        string ErrorMessage { get; }

        IReadOnlyDictionary<string, IReadOnlyList<string>> Result { get; }
    }

    public class ValidationResult : IValidationResult
    {
        public bool IsValid => ErrorMessage.IsEmptyString() && Result.IsEmpty();
        public string ErrorMessage { get; }
        public IReadOnlyDictionary<string, IReadOnlyList<string>> Result { get; }

        public ValidationResult(string errorMessage, IReadOnlyDictionary<string, IReadOnlyList<string>> result)
        {
            ErrorMessage = errorMessage;
            Result = result;
        }

        public ValidationResult(string errorMessage, Dictionary<string, List<string>> result) : this(errorMessage,
            result?.ToDictionary(x => x.Key, x => (IReadOnlyList<string>) x.Value))
        {
        }

        public static readonly IValidationResult Empty = new ValidationResult(null, (IReadOnlyDictionary<string, IReadOnlyList<string>>) null);
        public static readonly IValidationResult Succeeded = Empty;
        public static IValidationResult Error(string message) => new ValidationResult(message, (IReadOnlyDictionary<string, IReadOnlyList<string>>) null);
        public static IValidationResult Error(string key, string error) => new ValidationResult(null, new Dictionary<string, List<string>>(1) {{key, new List<string>(1) {error}}});
    }

    public class ValidationResultAggregate : IValidationResult
    {
        private readonly StringBuilder _sb = new StringBuilder();
        private readonly Dictionary<string, List<string>> _result = new Dictionary<string, List<string>>();
        public bool IsValid => ErrorMessage.IsEmptyString() && Result.IsEmpty();
        public string ErrorMessage => _sb.ToString();
        public IReadOnlyDictionary<string, IReadOnlyList<string>> Result => _result.ToDictionary(x => x.Key, x => (IReadOnlyList<string>) x.Value);

        public ValidationResultAggregate Add(IValidationResult other)
        {
            if (!other.ErrorMessage.IsEmptyString())
                _sb.AppendLine(other.ErrorMessage);

            if (!other.Result.IsEmpty())
            {
                foreach (var kv in other.Result.Where(x => !x.Value.IsEmpty()))
                {
                    if (!_result.ContainsKey(kv.Key))
                        _result.Add(kv.Key, new List<string>());

                    if (_result[kv.Key] == null)
                        _result[kv.Key] = new List<string>();

                    _result[kv.Key].AddRange(kv.Value);
                }
            }
            return this;
        }
    }
}