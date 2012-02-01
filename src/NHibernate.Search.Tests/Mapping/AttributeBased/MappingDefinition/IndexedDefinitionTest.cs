using NHibernate.Search.Attributes;
using NUnit.Framework;

namespace NHibernate.Search.Tests.Mapping.AttributeBased.MappingDefinition
{
	[TestFixture]
	public class IndexedDefinitionTest : AttributedMappingDefinitionTestBase
	{
		[Test]
		public void Should_extract_index_name()
		{
			Assert.AreEqual("foo", mappingDefinition.Indexed(typeof(Doc)).Index);
		}
		
		[Indexed(Index = "foo")]
		public class Doc { }
	}
}