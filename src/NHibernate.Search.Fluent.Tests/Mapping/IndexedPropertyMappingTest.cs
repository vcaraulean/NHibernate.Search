using NHibernate.Search.Fluent.Mapping;
using NUnit.Framework;

namespace NHibernate.Search.Fluent.Tests.Mapping
{
	[TestFixture]
	public class IndexedPropertyMappingTest
	{
		[Test]
		public void Should_set_index()
		{
			var builder = new FluentSearchMappingBuilder(new DocMapping());
			var mapping = builder.Build(typeof (DocMapping));
			Assert.AreEqual("docIndex", mapping.IndexName);
		}

		class Doc{ }

		class DocMapping : DocumentMap<Doc>
		{
			public DocMapping()
			{
				Name("docIndex");
			}
		}
	}
}