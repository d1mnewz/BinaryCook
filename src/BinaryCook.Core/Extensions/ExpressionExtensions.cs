using System;
using System.Linq.Expressions;
using System.Reflection;

namespace BinaryCook.Core.Extensions
{
	public static class ExpressionExtensions
	{
		public static string GetFullPath<T, TVal>(this Expression<Func<T, TVal>> exp)
		{
			return ((MemberExpression) exp.Body).GetFullPath();
		}

		public static string GetFullPath(this MemberExpression memberExpression)
		{
			var str = "";
			var expression = memberExpression.Expression as MemberExpression;
			if (expression != null)
				str = expression.GetFullPath() + ".";
			var member = (PropertyInfo) memberExpression.Member;
			return str + member.Name;
		}

		public static PropertyInfo GetPropertyInfo<T, TVal>(this Expression<Func<T, TVal>> exp)
		{
			var body = exp.Body as MemberExpression;
			return (body == null ? (MemberExpression) ((UnaryExpression) exp.Body).Operand : body).GetPropertyInfo();
		}

		public static PropertyInfo GetPropertyInfo(this MemberExpression memberExpression)
		{
			var expression = memberExpression.Expression as MemberExpression;
			if (expression != null)
				return expression.GetPropertyInfo();
			return (PropertyInfo) memberExpression.Member;
		}
	}
}