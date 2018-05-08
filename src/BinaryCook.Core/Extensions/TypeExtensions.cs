using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace BinaryCook.Core.Extensions
{
	public static class TypeExtensions
	{
		public static bool ImplementsInterfaceTemplate(this Type pluggedType, Type templateType)
		{
			if (pluggedType.IsConcrete())
				return ((IEnumerable<Type>) pluggedType.GetTypeInfo().GetInterfaces()).Any<Type>((Func<Type, bool>) (itfType =>
				{
					if (itfType.IsGeneric())
						return itfType.GetGenericTypeDefinition() == templateType;
					return false;
				}));
			return false;
		}

		public static bool ImplementsInterface<TInterface>(this Type type)
		{
			var tInterface = typeof(TInterface);
			return ((IEnumerable<Type>) type.GetTypeInfo().GetInterfaces()).Any<Type>((Func<Type, bool>) (itfType => itfType == tInterface));
		}

		public static bool IsConcrete(this Type type)
		{
			var typeInfo = type.GetTypeInfo();
			if (!typeInfo.IsAbstract)
				return !typeInfo.IsInterface;
			return false;
		}

		public static Type BaseType(this Type type)
		{
			if ((object) type == null)
				return (Type) null;
			return type.GetTypeInfo().BaseType;
		}

		public static bool IsGeneric(this Type type, Type openType)
		{
			if (!type.IsGeneric())
				return false;
			if (type.GetGenericTypeDefinition() == openType)
				return true;
			if (type.BaseType() != (Type) null)
				return type.BaseType().IsGeneric(openType);
			return false;
		}

		public static bool IsGeneric(this Type type)
		{
			return type.GetTypeInfo().IsGenericType;
		}
	}
}