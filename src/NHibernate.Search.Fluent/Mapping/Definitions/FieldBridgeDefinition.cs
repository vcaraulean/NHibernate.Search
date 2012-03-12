using System.Collections.Generic;
using NHibernate.Search.Mapping.Definition;

namespace NHibernate.Search.Fluent.Mapping.Definitions
{
	public class FieldBridgeDefinition : IFieldBridgeDefinition
	{
		public FieldBridgeDefinition()
		{
			Parameters = new Dictionary<string, object>();
		}

		public System.Type Impl { get; set; }
		public Dictionary<string, object> Parameters { get; set; }
	}
}