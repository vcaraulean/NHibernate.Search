using NHibernate.Search.Mapping;

namespace NHibernate.Search.Fluent.Mapping
{
	public class FluentSearchMappingBuilder : SearchMappingBuilder
	{
		public FluentSearchMappingBuilder() : base(new FluentSearchMappingDefinition())
		{
		}
	}
}