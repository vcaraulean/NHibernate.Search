using System.Linq;
using NHibernate.Search.Fluent.Mapping;
using NUnit.Framework;

namespace NHibernate.Search.Fluent.Tests.Mapping.FluentMappings
{
	[TestFixture]
	public class BoostPropertyMappingTest : PropertyMappingTest
	{
		[Test]
		public void Should_map_boost_if_defined_on_a_class()
		{
			Assert.AreEqual(1.3f, CreateDocumentMapping<DocMap>().Boost);
		}

		[Test]
		public void Should_not_map_boost_if_not_defined_on_a_class()
		{
			Assert.AreEqual(null, CreateDocumentMapping<DocMapNoBoost>().Boost);
		}

		[Test]
		public void Should_map_boost_if_defined_on_a_property()
		{
			var fieldMapping = CreateDocumentMapping<DocMap>().Fields.Single(f => f.Name == "Title");
			Assert.AreEqual(1.5f, fieldMapping.Boost);
		}

		[Test]
		public void Should_not_map_boo_if_not_defined_on_a_property()
		{
			var fieldMapping = CreateDocumentMapping<DocMap>().DocumentId;
			Assert.AreEqual(null, fieldMapping.Boost);
		}

		class Doc
		{
			public int Id { get; set; }
			public string Title { get; set; }
		}
		class DocMap : DocumentMap<Doc>
		{
			public DocMap()
			{
				Id(x => x.Id);
				Boost(1.3f);

				Map(x => x.Title)
					.Boost(1.5f);
			}
		}
		class DocMapNoBoost : DocumentMap<Doc>
		{
			public DocMapNoBoost()
			{
				Id(x => x.Id);
			}
		}

	}
}