using System;
using NHibernate.Search.Mapping.Definition;

namespace NHibernate.Search.Attributes
{
	/// <summary>
    /// Mark a property as indexable
    /// </summary>
    /// <remarks>We allow multiple instances of this attribute rather than having a Fields as per Java</remarks>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = true)]
    public class FieldAttribute : Attribute, IFieldDefinition
	{
    	public FieldAttribute()
        {
        	Index = Attributes.Index.Tokenized;
        	Store = Store.No;
        }

    	public FieldAttribute(Index index)
    	{
    		Store = Store.No;
    		this.Index = index;
    	}

    	/// <summary>
    	/// Field name, default to the property name
    	/// </summary>
    	public string Name { get; set; }

    	/// <summary>
    	/// Should the value be stored in the document
    	/// defaults to no.
    	/// </summary>
    	public Store Store { get; set; }

    	/// <summary>
    	/// Defines how the Field should be indexed
    	/// defaults to tokenized
    	/// </summary>
    	public Index Index { get; set; }

    	/// <summary>
    	/// Define an analyzer for the field, default to
    	/// the inherited analyzer
    	/// </summary>
    	public System.Type Analyzer { get; set; }

    	/// <summary>
    	/// Field bridge used. Default is autowired.
    	/// </summary>
    	/// TODO: Not sure if this is correct
    	public FieldBridgeAttribute FieldBridge { get; set; }
    }
}