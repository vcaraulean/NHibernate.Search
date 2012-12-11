namespace NHibernate.Search.Mapping.Definition
{
	/// <summary>
	/// Parameter (basically key/value pattern)
	/// </summary>
	public interface IParameterDefinition
	{
		/// <summary>
		/// 
		/// </summary>
		string Name { get; }

		/// <summary>
		/// 
		/// </summary>
		object Value { get; }

		/// <summary>
		/// The bridge that owns this parameter
		/// </summary>
		string Owner { get; set; }
	}
}