using NHibernate.Search.Fluent.Mapping;
using NHibernate.Search.Mapping;

namespace NHibernate.Search.Fluent.Tests.Mapping.FluentMappings
{
	public abstract class PropertyMappingTest
	{
		protected static DocumentMapping CreateDocumentMapping<TMap>() 
			where TMap : IDocumentMap, new()
		{
			var fluentMap = new TMap();
			var builder = new FluentSearchMappingBuilder(fluentMap);
			return builder.Build(fluentMap.DocumentType);
		}
	}
}