using System.Collections.Generic;
using System.Reflection;
using NHibernate.Search.Engine;
using NHibernate.Search.Mapping.Definition;

namespace NHibernate.Search.Mapping
{
	using Type = System.Type;

	public interface ISearchMappingDefinition
	{
		IIndexedDefinition Indexed(Type type);
		IList<FilterDef> FullTextFilters(Type type);
		IList<IClassBridgeDefinition> ClassBridges(Type type);

		IFieldBridgeDefinition FieldBridge(MemberInfo member);
		IDocumentIdDefinition DocumentId(MemberInfo member);
		IList<IFieldDefinition> FieldDefinitions(MemberInfo member);
		IIndexedEmbeddedDefinition IndexedEmbedded(MemberInfo member);
		bool HasContainedInDefinition(MemberInfo member);
		IDateBridgeDefinition DateBridge(MemberInfo member);

		IEnumerable<IParameterDefinition> BridgeParameters(ICustomAttributeProvider member);
		IAnalyzerDefinition Analyzer(ICustomAttributeProvider member);
		IBoostDefinition Boost(ICustomAttributeProvider member);
	}
}