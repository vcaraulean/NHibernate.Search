namespace NHibernate.Search.Fluent.Mapping.Parts
{
	public interface IFluentMappingPart
	{
		string Name { get; set; }
		float? Boost { get; set; }
	}

	public abstract class FluentMappingPart : IFluentMappingPart
	{
		string IFluentMappingPart.Name { get; set; }
		float? IFluentMappingPart.Boost { get; set; }
	}
}