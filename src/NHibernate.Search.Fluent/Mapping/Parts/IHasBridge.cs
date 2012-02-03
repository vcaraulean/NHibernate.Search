using NHibernate.Search.Bridge;

namespace NHibernate.Search.Fluent.Mapping.Parts
{
	public interface IHasBridge
	{
		IFieldBridge FieldBridge { get; set; }
	}
}