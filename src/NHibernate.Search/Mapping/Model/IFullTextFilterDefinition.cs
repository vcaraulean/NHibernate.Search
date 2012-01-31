using NHibernate.Search.Attributes;

namespace NHibernate.Search.Mapping.Model
{
	public interface IFullTextFilterDefinition
	{
		/// <summary>
		/// Filter name. Must be unique accross all mappings for a given persistence unit
		/// </summary>
		string Name { get; }

		/// <summary>
		/// Either implements <see cref="Lucene.Net.Search.Filter" />
		/// or contains a <see cref="FactoryAttribute" /> method returning one.
		/// The Filter generated must be thread-safe
		///
		/// If the filter accept parameters, an <see cref="KeyAttribute" /> method must be present as well.
		/// </summary>
		System.Type Impl { get; }

		/// <summary>
		/// Enable caching for this filter (default true).
		/// </summary>
		bool Cache { get; set; }
	}
}