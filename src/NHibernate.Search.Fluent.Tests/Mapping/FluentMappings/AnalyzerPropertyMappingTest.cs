using Lucene.Net.Analysis;
using NHibernate.Search.Fluent.Mapping;
using NUnit.Framework;

namespace NHibernate.Search.Fluent.Tests.Mapping.FluentMappings
{
	[TestFixture]
	public class AnalyzerPropertyMappingTest : PropertyMappingTest
	{
		[Test]
		[Ignore("Looks like a valid case for the root DocumentMapper do not have it's analyzer set")]
		public void Should_set_analyzer()
		{
			Assert.AreEqual(typeof(StopAnalyzer), CreateDocumentMapping<DocMap>().Analyzer);
		}


		class Doc { }
		class DocMap : DocumentMap<Doc>
		{
			public DocMap()
			{
				Analyzer().Stop();
			}
		}
	}
}