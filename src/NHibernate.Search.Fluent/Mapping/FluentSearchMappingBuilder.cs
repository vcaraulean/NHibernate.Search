using NHibernate.Search.Mapping;

namespace NHibernate.Search.Fluent.Mapping
{
	public class FluentSearchMappingBuilder : SearchMappingBuilder
	{
		public FluentSearchMappingBuilder(IDocumentMap documentMap) 
			: base(new FluentSearchMappingDefinition(documentMap))
		{
		}
	}
}