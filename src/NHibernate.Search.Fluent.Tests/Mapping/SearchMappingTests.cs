using System.Linq;
using System.Reflection;
using NHibernate.Search.Fluent.Mapping;
using NUnit.Framework;

namespace NHibernate.Search.Fluent.Tests.Mapping
{
	[TestFixture]
	public class SearchMappingTests
	{
		[Test]
		public void Should_extract_document_mappings_from_assebly_containing_a_type()
		{
			var mapping = new SearchMapping_AssemblyContaining();

			CollectionAssert.IsNotEmpty(mapping.GetMappingDocuments().ToList());
		}

		[Test]
		public void Should_extract_document_mappings_from_given_assembly()
		{
			var mapping = new SearchMapping_Assembly();

			CollectionAssert.IsNotEmpty(mapping.GetMappingDocuments());
		}

		[Test]
		public void Should_extract_document_mappings_from_providers_added_excplicitly()
		{
			var mapping = new SearchMapping_AddedExplicitly();

			CollectionAssert.IsNotEmpty(mapping.GetMappingDocuments());
		}

		private class SearchMapping_AssemblyContaining : FluentSearchMapping
		{
			public SearchMapping_AssemblyContaining()
			{
				Configure();
			}

			protected override void Configure()
			{
				AddAssemblyContaining<SearchMapping_AssemblyContaining>();
			}
		}

		private class SearchMapping_Assembly : FluentSearchMapping
		{
			public SearchMapping_Assembly()
			{
				Configure();
			}

			protected override void Configure()
			{
				AddAssembly(Assembly.GetExecutingAssembly());
			}
		}

		private class SearchMapping_AddedExplicitly : FluentSearchMapping
		{
			public SearchMapping_AddedExplicitly()
			{
				Configure();
			}

			protected override void Configure()
			{
				Add<DummyDocumentMap>();
			}
		}

		public class DummyDocumentMap : DocumentMap<object> { }
	}
}