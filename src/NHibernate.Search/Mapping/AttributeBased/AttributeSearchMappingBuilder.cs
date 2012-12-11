namespace NHibernate.Search.Mapping.AttributeBased
{
	public class AttributeSearchMappingBuilder : SearchMappingBuilder 
	{
    	public AttributeSearchMappingBuilder() 
			: base(new AttributedMappingDefinition())
    	{
    	}
	}
}
