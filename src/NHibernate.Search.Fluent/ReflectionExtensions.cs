using System;
using System.Linq.Expressions;
using System.Reflection;

namespace NHibernate.Search.Fluent
{
	public static class ReflectionExtensions
	{
		public static PropertyInfo ToPropertyInfo<T, TResult>(this Expression<Func<T, TResult>> expression)
		{
			MemberExpression memberExpression;
			var unary = expression.Body as UnaryExpression;
			if (unary != null)
			{
				memberExpression = unary.Operand as MemberExpression;
			}
			else
			{
				memberExpression = expression.Body as MemberExpression;
			}
			if (memberExpression == null || !(memberExpression.Member is PropertyInfo))
			{
				throw new ArgumentException("Expected property expression");
			}
			return (PropertyInfo)memberExpression.Member;
		}

		public static System.Type GetMemberTypeOrGenericArguments(this MemberInfo member)
		{
			var type = GetMemberType(member);
			if (type.IsGenericType)
			{
				var arguments = type.GetGenericArguments();
				return arguments[arguments.Length - 1];
			}

			return type;
		}

		public static System.Type GetMemberTypeOrGenericCollectionType(this MemberInfo member)
		{
			var type = GetMemberType(member);
			return type.IsGenericType ? type.GetGenericTypeDefinition() : type;
		}

		public static System.Type GetMemberType(this MemberInfo member)
		{
			var info = member as PropertyInfo;
			return info != null ? info.PropertyType : ((FieldInfo)member).FieldType;
		}

		public static PropertyInfo GetProperty<MODEL, T>(Expression<Func<MODEL, T>> expression)
		{
			MemberExpression memberExpression = getMemberExpression(expression);
			return (PropertyInfo)memberExpression.Member;
		}

		private static MemberExpression getMemberExpression<MODEL, T>(Expression<Func<MODEL, T>> expression)
		{
			MemberExpression memberExpression = null;
			if (expression.Body.NodeType == ExpressionType.Convert)
			{
				var body = (UnaryExpression)expression.Body;
				memberExpression = body.Operand as MemberExpression;
			}
			else if (expression.Body.NodeType == ExpressionType.MemberAccess)
			{
				memberExpression = expression.Body as MemberExpression;
			}

			if (memberExpression == null) throw new ArgumentException("Not a member access", "expression");
			return memberExpression;
		}
	}
}