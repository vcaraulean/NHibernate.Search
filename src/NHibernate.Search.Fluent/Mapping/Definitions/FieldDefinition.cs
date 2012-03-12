using NHibernate.Search.Attributes;
using NHibernate.Search.Mapping.Definition;

namespace NHibernate.Search.Fluent.Mapping.Definitions
{
	public class FieldDefinition : IFieldDefinition
	{
		public FieldDefinition()
		{
			Index = Index.Tokenized;
			Store = Attributes.Store.No;
		}

		public string Name { get; set; }
		public Attributes.Store Store { get; set; }
		public Index Index { get; set; }
		public System.Type Analyzer { get; set; }
	}
}