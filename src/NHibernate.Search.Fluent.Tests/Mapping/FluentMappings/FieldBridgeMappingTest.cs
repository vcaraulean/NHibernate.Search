using System.Linq;
using Lucene.Net.Documents;
using NHibernate.Search.Bridge;
using NHibernate.Search.Fluent.Mapping;
using NUnit.Framework;

namespace NHibernate.Search.Fluent.Tests.Mapping.FluentMappings
{
	[TestFixture]
	public class FieldBridgeMappingTest : PropertyMappingTest
	{

		[Test]
		public void Should_detect_bridge_mapping()
		{
			var mapping = CreateDocumentMapping<DocMap>().Fields.Last();
			Assert.AreEqual(typeof(NameFieldBridge), mapping.Bridge.GetType());
		}

		class Doc
		{
			public int Id { get; set; }
			public string Name { get; set; }
		}

		class DocMap : DocumentMap<Doc>
		{
			public DocMap()
			{
				Id(x => x.Id);
				Map(x => x.Name);

				FieldBridge(x => x.Name).Custom<NameFieldBridge>();
			}
		}
		
		class NameFieldBridge : IFieldBridge
		{
			public void Set(string name, object value, Document document, Field.Store store, Field.Index index, float? boost)
			{
				
			}
		}
	}
}