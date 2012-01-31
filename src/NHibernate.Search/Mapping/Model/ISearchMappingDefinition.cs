using System.Collections.Generic;

namespace NHibernate.Search.Mapping.Model
{
	using Type = System.Type;

	public interface ISearchMappingDefinition
	{
		IIndexedDefinition IndexedDefinition(Type type);
		IEnumerable<IFullTextFilterDefinition> FullTextFilters(Type type);
	}
}