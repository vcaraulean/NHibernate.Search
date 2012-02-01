using System.Collections.Generic;
using System.Reflection;
using NHibernate.Search.Engine;
using NHibernate.Search.Mapping;
using NHibernate.Search.Mapping.Definition;

namespace NHibernate.Search.Fluent.Mapping
{
	public class FluentSearchMappingDefinition : ISearchMappingDefinition
	{
		public IIndexedDefinition Indexed(System.Type type)
		{
			throw new System.NotImplementedException();
		}

		public IList<FilterDef> FullTextFilters(System.Type type)
		{
			throw new System.NotImplementedException();
		}

		public IList<IClassBridgeDefinition> ClassBridges(System.Type type)
		{
			throw new System.NotImplementedException();
		}

		public IFieldBridgeDefinition FieldBridge(MemberInfo member)
		{
			throw new System.NotImplementedException();
		}

		public IDocumentIdDefinition DocumentId(MemberInfo member)
		{
			throw new System.NotImplementedException();
		}

		public IList<IFieldDefinition> FieldDefinitions(MemberInfo member)
		{
			throw new System.NotImplementedException();
		}

		public IIndexedEmbeddedDefinition IndexedEmbedded(MemberInfo member)
		{
			throw new System.NotImplementedException();
		}

		public bool HasContainedInDefinition(MemberInfo member)
		{
			throw new System.NotImplementedException();
		}

		public IDateBridgeDefinition DateBridge(MemberInfo member)
		{
			throw new System.NotImplementedException();
		}

		public IEnumerable<IParameterDefinition> BridgeParameters(ICustomAttributeProvider member)
		{
			throw new System.NotImplementedException();
		}

		public IAnalyzerDefinition Analyzer(ICustomAttributeProvider member)
		{
			throw new System.NotImplementedException();
		}

		public IBoostDefinition Boost(ICustomAttributeProvider member)
		{
			throw new System.NotImplementedException();
		}
	}
}