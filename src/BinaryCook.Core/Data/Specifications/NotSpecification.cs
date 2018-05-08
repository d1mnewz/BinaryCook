using System;
using System.Linq.Expressions;
using BinaryCook.Core.Code;

namespace BinaryCook.Core.Data.Specifications
{
    public class NotSpecification<T> : Specification<T>
    {
        private readonly ISpecification<T> _specification;

        public NotSpecification(ISpecification<T> specification)
        {
            Requires.NotNull(specification, nameof(specification));

            _specification = specification;
        }

        public override Expression<Func<T, bool>> Expression => _specification.Expression.Not();
    }
}