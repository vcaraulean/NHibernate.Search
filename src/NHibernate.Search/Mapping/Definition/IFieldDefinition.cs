using NHibernate.Search.Attributes;

namespace NHibernate.Search.Mapping.Definition
{
	public interface IFieldDefinition
	{
		/// <summary>
		/// Field name, default to the property name
		/// </summary>
		string Name { get; set; }

		/// <summary>
		/// Should the value be stored in the document
		/// defaults to no.
		/// </summary>
		Attributes.Store Store { get; set; }

		/// <summary>
		/// Defines how the Field should be indexed
		/// defaults to tokenized
		/// </summary>
		Index Index { get; set; }

		/// <summary>
		/// Define an analyzer for the field, default to
		/// the inherited analyzer
		/// </summary>
		System.Type Analyzer { get; set; }

		/// <summary>
		/// Field bridge used. Default is autowired.
		/// </summary>
		/// TODO: Not sure if this is correct
		FieldBridgeAttribute FieldBridge { get; set; }
	}
}