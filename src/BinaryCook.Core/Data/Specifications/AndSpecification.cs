using System;
using System.Linq.Expressions;
using BinaryCook.Core.Code;

namespace BinaryCook.Core.Data.Specifications
{
    public class AndSpecification<T> : Specification<T>
    {
        private readonly ISpecification<T> _left;
        private readonly ISpecification<T> _right;

        public AndSpecification(ISpecification<T> left, ISpecification<T> right)
        {
            Requires.NotNull(left, nameof(left));
            Requires.NotNull(right, nameof(right));
            
            _left = left;
            _right = right;
        }

        public override Expression<Func<T, bool>> Expression => _left.Expression.And(_right.Expression);
    }
}