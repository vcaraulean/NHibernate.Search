using NHibernate.Search.Mapping;

namespace NHibernate.Search.Fluent.Mapping
{
	public interface IDocumentMappingProvider
	{
		DocumentMapping GetMapping();
		void AssertIsValid();
	}
}