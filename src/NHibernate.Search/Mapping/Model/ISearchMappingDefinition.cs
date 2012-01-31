using System.Collections.Generic;
using NHibernate.Search.Engine;

namespace NHibernate.Search.Mapping.Model
{
	using Type = System.Type;

	public interface ISearchMappingDefinition
	{
		IIndexedDefinition IndexedDefinition(Type type);
		IList<FilterDef> FullTextFilters(Type type);
	}
}