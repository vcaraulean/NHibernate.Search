using NHibernate.Search.Mapping.Definition;

namespace NHibernate.Search.Fluent.Mapping.Definitions
{
	public class BoostDefinition : IBoostDefinition
	{
		public float Value { get; set; }
	}
}