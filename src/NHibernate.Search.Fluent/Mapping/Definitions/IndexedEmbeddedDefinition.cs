using NHibernate.Search.Mapping.Definition;

namespace NHibernate.Search.Fluent.Mapping.Definitions
{
	public class IndexedEmbeddedDefinition : IIndexedEmbeddedDefinition
	{
		public IndexedEmbeddedDefinition()
		{
			Depth = int.MaxValue;
			Prefix = ".";
		}

		public string Prefix { get; set; }
		public int Depth { get; set; }
		public System.Type TargetElement { get; set; }
	}
}