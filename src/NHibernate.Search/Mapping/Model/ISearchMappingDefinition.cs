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
		IList<IClassBridgeDefinition> ClassBridges(Type type);
		IFieldBridgeDefinition FieldBridge(Type type);
		IList<IParameterDefinition> BridgeParameters(Type type);
		IDocumentIdDefinition DocumentId(MemberInfo member);
		IList<IFieldDefinition> FieldDefinitions(MemberInfo member);
	}
}