using System.Linq;
using Lucene.Net.Analysis;
using NHibernate.Search.Attributes;
using NHibernate.Search.Fluent.Mapping;
using NUnit.Framework;

namespace NHibernate.Search.Fluent.Tests.Mapping.FluentMappings
{
	[TestFixture]
	public class FieldPropertyMappingTest : PropertyMappingTest
	{
		[Test]
		public void Should_map_all_properties()
		{
			var fieldMapping = CreateDocumentMapping<DocMap>().Fields.Single();

			Assert.AreEqual("prop", fieldMapping.Name);
			Assert.That(fieldMapping.Analyzer, Is.InstanceOf<WhitespaceAnalyzer>());
			Assert.That(fieldMapping.Store, Is.EqualTo(Attributes.Store.Yes));
			Assert.That(fieldMapping.Index, Is.EqualTo(Index.NoNorms));
		}

		[Test]
		public void Should_map_Boost_property()
		{
			var fieldMapping = CreateDocumentMapping<DocMap>().Fields.Single();

			Assert.AreEqual(2.1f, fieldMapping.Boost);
		}

		class Doc
		{
			public int Id { get; set; }
			public string SomeProperty { get; set; }
		}

		class DocMap : DocumentMap<Doc>
		{
			public DocMap()
			{
				Id(x => x.Id);
				Map(doc => doc.SomeProperty)
					.Name("prop")
					.Boost(2.1f)
					.Index().NoNorms()
					.Store().Yes()
					.Analyzer().Whitespace();
			}
		}
	}
}