using System.Reflection;
using NHibernate.Cfg;
using NHibernate.Search.Fluent.Mapping;
using NHibernate.Search.Mapping;
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

			CollectionAssert.IsNotEmpty(mapping.Build(new Configuration()));
		}

		[Test]
		public void Should_extract_document_mappings_from_given_assembly()
		{
			var mapping = new SearchMapping_Assembly();

			CollectionAssert.IsNotEmpty(mapping.Build(new Configuration()));
		}

		[Test]
		public void Should_extract_document_mappings_from_providers_added_excplicitly()
		{
			var mapping = new SearchMapping_AddedExplicitly();

			CollectionAssert.IsNotEmpty(mapping.Build(new Configuration()));
		}

		private class SearchMapping_AssemblyContaining : FluentSearchMapping
		{
			protected override void Configure()
			{
				AddAssemblyContaining<SearchMapping_AssemblyContaining>();
			}
		}

		private class SearchMapping_Assembly : FluentSearchMapping
		{
			protected override void Configure()
			{
				AddAssembly(Assembly.GetExecutingAssembly());
			}
		}

		private class SearchMapping_AddedExplicitly : FluentSearchMapping
		{
			protected override void Configure()
			{
				Add<DummyDocumentMappingProvider>();
			}
		}

		private class DummyDocumentMappingProvider : IDocumentMappingProvider
		{
			public DocumentMapping GetMapping()
			{
				return new DocumentMapping(typeof (object));
			}

			public void AssertIsValid()
			{
			}
		}
	}
}