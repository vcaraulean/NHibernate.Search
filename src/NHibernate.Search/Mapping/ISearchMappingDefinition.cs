using System.Collections.Generic;
using System.Reflection;
using NHibernate.Search.Engine;
using NHibernate.Search.Mapping.Definition;

namespace NHibernate.Search.Mapping
{
	public interface ISearchMappingDefinition
	{
		IIndexedDefinition IndexedDefinition(System.Type type);
		IList<FilterDef> FullTextFilters(System.Type type);
		
		// Parameter should be a Type, not ICustomAttribute provider
		IList<IClassBridgeDefinition> ClassBridges(ICustomAttributeProvider type);
		IFieldBridgeDefinition FieldBridge(MemberInfo member);
		IEnumerable<IParameterDefinition> BridgeParameters(ICustomAttributeProvider member);
		IDocumentIdDefinition DocumentId(MemberInfo member);
		IList<IFieldDefinition> FieldDefinitions(MemberInfo member);
		IIndexedEmbeddedDefinition IndexedEmbedded(MemberInfo member);
		bool HasContainedInDefinition(MemberInfo member);
		IDateBridgeDefinition DateBridge(MemberInfo member);
		IAnalyzerDefinition Analyzer(ICustomAttributeProvider member);
		IBoostDefinition Boost(ICustomAttributeProvider member);

		// TODO: review
		// FieldBridgeAttribute is applicable only to properties & fields
		// This method is used to look for this attribute on a type.
		// Review usages...
		bool HasFieldBridge(System.Type type);
	}
}