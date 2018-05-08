using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace BinaryCook.Core.Data.Entities
{
    public interface IUnremovable
    {
        void Delete();
    }

    public static class UnremovableExtensions
    {
        public static IEnumerable<T> NotDeleted<T>(this IEnumerable<T> list) where T : IUnremovable => list.Where(DefaultNotDeletedFunc<T>());

        public static Expression<Func<T, bool>> DefaultNotDeletedExpression<T>()
        {
            var arg = Expression.Parameter(typeof(T), "i");
            return Expression.Lambda<Func<T, bool>>(Expression.Not(Expression.Property(arg, "IsDeleted")), arg);
        }

        public static Func<T, bool> DefaultNotDeletedFunc<T>() where T : IUnremovable
        {
            //TODO: Cache compiled expression
            var result = DefaultNotDeletedExpression<T>().Compile();
            return result;
        }
    }
}