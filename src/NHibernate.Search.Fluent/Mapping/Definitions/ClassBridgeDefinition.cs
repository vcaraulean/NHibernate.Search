using System.Collections.Generic;
using NHibernate.Search.Attributes;
using NHibernate.Search.Mapping.Definition;

namespace NHibernate.Search.Fluent.Mapping.Definitions
{
	public class ClassBridgeDefinition : IClassBridgeDefinition
	{
		public ClassBridgeDefinition()
		{
			Boost = 1.0F;
			Index = Index.Tokenized;
			Store = Attributes.Store.No; 
			Parameters = new Dictionary<string, object>();
		}

		public string Name { get; set; }
		public Attributes.Store Store { get; set; }
		public Index Index { get; set; }
		public System.Type Analyzer { get; set; }
		public float Boost { get; set; }
		public System.Type Impl { get; set; }
		public Dictionary<string, object> Parameters { get; set; }
	}
}