using System;
using System.Linq;
using System.Linq.Expressions;
using BinaryCook.Core.Code;

namespace BinaryCook.Core.Data.Specifications
{
    public interface ISpecification
    {
    }

    public interface ISpecification<T> : ISpecification
    {
        bool IsSatisfiedBy(T candidate);
        Expression<Func<T, bool>> Expression { get; }
    }

    public abstract class Specification<T> : ISpecification<T>
    {
        private Func<T, bool> _compiled;

        public bool IsSatisfiedBy(T candidate)
        {
            Requires.NotNull(Expression, nameof(Expression));

            _compiled = _compiled ?? Expression.Compile();
            return _compiled(candidate);
        }

        public abstract Expression<Func<T, bool>> Expression { get; }
    }

    public static class SpecificationExtensions
    {
        public static ISpecification<T> And<T>(this ISpecification<T> left, ISpecification<T> right) => new AndSpecification<T>(left, right);
        public static ISpecification<T> Or<T>(this ISpecification<T> left, ISpecification<T> right) => new OrSpecification<T>(left, right);
        public static ISpecification<T> Not<T>(this ISpecification<T> left) => new NotSpecification<T>(left);

        public static IQueryable<T> SafeApply<T>(this IQueryable<T> query, ISpecification<T> specification) =>
            specification?.Expression == null ? query : query.Where(specification.Expression);
    }
}