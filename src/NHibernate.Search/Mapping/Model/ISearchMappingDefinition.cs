using System.Collections.Generic;
using System.Reflection;
using NHibernate.Search.Engine;
using NHibernate.Search.Mapping.Definition;

namespace NHibernate.Search.Mapping.Model
{
	using Type = System.Type;

	public interface ISearchMappingDefinition
	{
		IIndexedDefinition IndexedDefinition(Type type);
		IList<FilterDef> FullTextFilters(Type type);
		
		// Parameter should be a Type, not ICustomAttribute provider
		IList<IClassBridgeDefinition> ClassBridges(ICustomAttributeProvider type);
		IFieldBridgeDefinition FieldBridge(MemberInfo member);
		IList<IParameterDefinition> BridgeParameters(ICustomAttributeProvider member);
		IDocumentIdDefinition DocumentId(MemberInfo member);
		IList<IFieldDefinition> FieldDefinitions(MemberInfo member);
		IIndexedEmbeddedDefinition IndexedEmbedded(MemberInfo member);
		bool HasContainedInDefinition(MemberInfo member);

		// TODO: review
		// FieldBridgeAttribute is applicable only to properties & fields
		// This method is used to look for this attribute on a type.
		// Review usages...
		bool HasFieldBridge(Type type);
	}
}