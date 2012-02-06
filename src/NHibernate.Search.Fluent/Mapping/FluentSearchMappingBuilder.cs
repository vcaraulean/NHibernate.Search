using NHibernate.Search.Mapping;

namespace NHibernate.Search.Fluent.Mapping
{
	public class FluentSearchMappingBuilder : SearchMappingBuilder
	{
		public FluentSearchMappingBuilder(FluentSearchMappingDefinition mappingDefinition) 
			: base(mappingDefinition)
		{
		}
	}
}