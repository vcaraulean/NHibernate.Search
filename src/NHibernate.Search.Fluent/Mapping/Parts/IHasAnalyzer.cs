namespace NHibernate.Search.Fluent.Mapping.Parts
{
	using Type = System.Type;

	public interface IHasAnalyzer
	{
		Type AnalyzerType { get; set; }
	}
}