using NHibernate.Search.Fluent.Mapping;
using NUnit.Framework;

namespace NHibernate.Search.Fluent.Tests.Mapping.FluentMappings
{
	[TestFixture]
	public class BoostPropertyMappingTest : PropertyMappingTest
	{
		[Test]
		public void Should_map_boost_if_defined()
		{
			Assert.AreEqual(1.3f, CreateDocumentMapping<DocMap>().Boost);
		}

		[Test]
		public void Should_not_map_boost_if_not_defined()
		{
			Assert.AreEqual(null, CreateDocumentMapping<DocMapNoBoost>().Boost);
		}

		class Doc { }
		class DocMap : DocumentMap<Doc>
		{
			public DocMap()
			{
				Boost(1.3f);
			}
		}
		class DocMapNoBoost : DocumentMap<Doc> { }

	}
}