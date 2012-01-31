namespace NHibernate.Search.Mapping.Definition
{
	public interface IIndexedEmbeddedDefinition
	{
		/// <summary>
		/// Field name prefix
		/// Default to 'propertyname.'
		/// </summary>
		string Prefix { get; set; }

		/// <summary>
		/// Stop indexing embedded elements when depth is reached
		/// depth=1 means the associated element is index, but not its embedded elements
		/// Default: infinite (an exception will be raised in case of class circular reference when infinite is chosen)
		/// </summary>
		int Depth { get; set; }

		/// <summary>
		/// Overrides the type of an association. If a collection, overrides the type of the collection generics
		/// </summary>
		System.Type TargetElement { get; set; }
	}
}