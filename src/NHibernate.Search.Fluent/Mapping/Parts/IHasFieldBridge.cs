using NHibernate.Search.Mapping.Definition;

namespace NHibernate.Search.Fluent.Mapping.Parts
{
	public interface IHasFieldBridge
	{
		IFieldBridgeDefinition BridgeDef { get; }
	}
}