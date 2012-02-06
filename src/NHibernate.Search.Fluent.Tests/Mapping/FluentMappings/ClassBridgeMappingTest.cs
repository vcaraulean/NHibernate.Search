using System.Linq;
using Lucene.Net.Analysis;
using Lucene.Net.Documents;
using NHibernate.Search.Attributes;
using NHibernate.Search.Bridge;
using NHibernate.Search.Fluent.Mapping;
using NUnit.Framework;

namespace NHibernate.Search.Fluent.Tests.Mapping.FluentMappings
{
	[TestFixture]
	public class ClassBridgeMappingTest : PropertyMappingTest
	{
		[Test]
		public void Should_extract_correc_number_of_class_bridges()
		{
			var mapping = CreateDocumentMapping<DocMap>();

			Assert.AreEqual(1, mapping.ClassBridges.Count);
		}

		[Test]
		public void Should_extract_correct_class_bridge_definitions()
		{
			var bridgeMapping = CreateDocumentMapping<DocMap>().ClassBridges.Single();
			
			Assert.That(bridgeMapping.Bridge, Is.InstanceOf<DocBridge>());
			Assert.AreEqual(1.2f, bridgeMapping.Boost);
			Assert.That(bridgeMapping.Analyzer, Is.InstanceOf<KeywordAnalyzer>() );
			Assert.AreEqual(Index.UnTokenized, bridgeMapping.Index);
			Assert.AreEqual(Attributes.Store.Yes, bridgeMapping.Store);
		}

		[Test]
		public void Should_extract_multiple_class_bridges_if_they_are_defined()
		{
			var mapping = CreateDocumentMapping<DocMapWithTwoBridges>();

			Assert.AreEqual(2, mapping.ClassBridges.Count);
		}

		[Test]
		public void Should_set_properly_default_values()
		{
			var bridgeMapping = CreateDocumentMapping<DocMapWithTwoBridges>().ClassBridges.First();

			Assert.AreEqual(1.0f, bridgeMapping.Boost);
			Assert.AreEqual(Index.Tokenized, bridgeMapping.Index);
			Assert.AreEqual(Attributes.Store.No, bridgeMapping.Store);
		}

		[Test]
		public void Should_set_mapping_name()
		{
			var bridgeMapping = CreateDocumentMapping<DocMapWithTwoBridges>().ClassBridges.Last();
			Assert.AreEqual("b2", bridgeMapping.Name);
		}

		class Doc
		{
			public int Id { get; set; }
		}

		class DocMap : DocumentMap<Doc>
		{
			public DocMap()
			{
				Id(x => x.Id);
				Bridge<DocBridge>()
					.Boost(1.2f)
					.Analyzer().Keyword()
					.Index().UnTokenized()
					.Store().Yes();
			}
		}

		class DocMapWithTwoBridges : DocumentMap<Doc>
		{
			public DocMapWithTwoBridges()
			{
				Id(x => x.Id);
				Bridge<DocBridge>();
				
				Bridge<DocBridge2>()
					.Name("b2");
			}
		}

		class DocBridge : IFieldBridge
		{
			public void Set(string name, object value, Document document, Field.Store store, Field.Index index, float? boost)
			{
			}
		}
		class DocBridge2 : IFieldBridge
		{
			public void Set(string name, object value, Document document, Field.Store store, Field.Index index, float? boost)
			{
			}
		}
	}
}