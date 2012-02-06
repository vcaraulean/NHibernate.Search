using NHibernate.Search.Mapping.Definition;

namespace NHibernate.Search.Fluent.Mapping.Definitions
{
	public class EmbeddedMappingPart
	{
		private readonly IndexedEmbeddedDefinition definition;

		public EmbeddedMappingPart()
		{
			definition = new IndexedEmbeddedDefinition();
		}

		public IIndexedEmbeddedDefinition EmbeddedDefinition
		{
			get
			{
				return definition;
			}
		}

		public EmbeddedMappingPart Prefix(string prefix)
		{
			definition.Prefix = prefix;
			return this;
		}

		public EmbeddedMappingPart Depth(int depth)
		{
			definition.Depth = depth;
			return this;
		}

		public EmbeddedMappingPart TargetElement(System.Type targetElement)
		{
			definition.TargetElement = targetElement;
			return this;
		}
	}
}