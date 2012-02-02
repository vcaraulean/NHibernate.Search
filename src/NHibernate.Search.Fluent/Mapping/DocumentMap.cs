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
		string Name { get; set; }
		MemberInfo IdProperty { get; set; }
		float? Boost { get; set; }
	}

	public abstract class DocumentMap<T> : IDocumentMap
	{
		protected DocumentMap()
		{
			Name(typeof (T).Name);
		}

		Type IDocumentMap.DocumentType
		{
			get { return typeof(T); }
		}

		string IDocumentMap.Name { get; set; }
		MemberInfo IDocumentMap.IdProperty { get; set; }
		float? IDocumentMap.Boost { get; set; }

		/// <summary>
		/// Defines the Lucene.NET Index Name.
		/// </summary>
		/// <param name="indexName"></param>
		protected void Name(string indexName)
		{
			(this as IDocumentMap).Name = indexName;
		}

		/// <summary>
		/// Defines the DocumentId Mapping.
		/// </summary>
		/// <param name="property"></param>
		/// <returns></returns>
		protected void Id(Expression<Func<T, object>> property)
		{
			if ((this as IDocumentMap).IdProperty != null)
				throw new FluentMappingException("Id can be set only once");
			(this as IDocumentMap).IdProperty = property.ToPropertyInfo();
		}

		/// <summary>
		/// Sets the boost value
		/// </summary>
		/// <param name="boost"></param>
		protected void Boost(float boost)
		{
			(this as IDocumentMap).Boost = boost;
		}
	}
}