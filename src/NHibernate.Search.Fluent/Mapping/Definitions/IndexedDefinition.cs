using NHibernate.Search.Mapping.Definition;

namespace NHibernate.Search.Fluent.Mapping.Definitions
{
	public class IndexedDefinition : IIndexedDefinition
	{
		public string Index { get; set; }
	}
}