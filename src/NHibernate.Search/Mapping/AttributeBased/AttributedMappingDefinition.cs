using System.Collections.Generic;
using NHibernate.Search.Attributes;
using NHibernate.Search.Mapping.Model;

namespace NHibernate.Search.Mapping.AttributeBased
{
	using Type = System.Type;

	public class AttributedMappingDefinition : ISearchMappingDefinition
	{
		public IIndexedDefinition IndexedDefinition(Type type)
		{
			return AttributeUtil.GetAttribute<IndexedAttribute>(type);
		}

		public IEnumerable<IFullTextFilterDefinition> FullTextFilters(Type type)
		{
			return AttributeUtil.GetAttributes<FullTextFilterDefAttribute>(type, false);
		}
	}
}