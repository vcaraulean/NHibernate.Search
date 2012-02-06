using NHibernate.Search.Fluent.Mapping;
using NUnit.Framework;

namespace NHibernate.Search.Fluent.Tests.Mapping.FluentMappings
{
	[TestFixture]
	public class IndexedPropertyMappingTest : PropertyMappingTest
	{
		[Test]
		public void Should_set_index()
		{
			Assert.AreEqual("docIndex", CreateDocumentMapping<DocMapping>().IndexName);
		}

		class Doc
		{
			public int Id { get; set; }
		}

		class DocMapping : DocumentMap<Doc>
		{
			public DocMapping()
			{
				Id(x => x.Id);
				Name("docIndex");
			}
		}
	}
}