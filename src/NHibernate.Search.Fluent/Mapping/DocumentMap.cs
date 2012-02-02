using System;
using System.Linq.Expressions;
using System.Reflection;
using NHibernate.Search.Fluent.Exceptions;

namespace NHibernate.Search.Fluent.Mapping
{
	using Type = System.Type;

	public interface IDocumentMap
	{
		Type DocumentType { get; }
		string Name {get;}
		MemberInfo IdProperty { get; }
	}

	public abstract class DocumentMap<T> : IDocumentMap
	{
		private string name;
		private PropertyInfo idProperty;

		protected DocumentMap()
		{
			name = typeof (T).Name;
		}

		/// <summary>
		/// Defines the Lucene.NET Index Name.
		/// </summary>
		/// <param name="indexName"></param>
		protected void Name(string indexName)
		{
			name = indexName;
		}

		protected void Id(Expression<Func<T, object>> property)
		{
			if (idProperty != null)
				throw new FluentMappingException("Id can be set only once");
			idProperty = property.ToPropertyInfo();
		}

		Type IDocumentMap.DocumentType
		{
			get { return typeof (T); }
		}

		string IDocumentMap.Name
		{
			get { return name; }
		}

		MemberInfo IDocumentMap.IdProperty
		{
			get { return idProperty; }
		}
	}
}