using System;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;

namespace BinaryCook.Application.Core.Validations.Extensions
{
    public static class RuleBuilderExtensions
    {
        public static IRuleBuilderOptions<T, TProperty> MustNotAsync<T, TProperty>(this IRuleBuilder<T, TProperty> ruleBuilder,
            Func<T, TProperty, CancellationToken, Task<bool>> predicate)
        {
            return ruleBuilder.MustAsync(async (x, val, ctx, cancel) => !await predicate(x, val, cancel));
        }

        public static IRuleBuilderOptions<T, TProperty> MustNotAsync<T, TProperty>(this IRuleBuilder<T, TProperty> ruleBuilder,
            Func<TProperty, CancellationToken, Task<bool>> predicate)
        {
            return ruleBuilder.MustAsync(async (x, val, ctx, cancel) => !await predicate(val, cancel));
        }
    }
}