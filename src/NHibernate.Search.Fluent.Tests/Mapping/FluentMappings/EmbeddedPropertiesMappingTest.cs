using System.Collections.Generic;
using System.Linq;
using NHibernate.Search.Fluent.Mapping;
using NUnit.Framework;

namespace NHibernate.Search.Fluent.Tests.Mapping.FluentMappings
{
	[TestFixture]
	public class EmbeddedPropertiesMappingTest : PropertyMappingTest
	{
		[Test]
		public void Shoudl_extract_embeded_mapping_properties()
		{
			var mappings = CreateDocumentMapping<DocMap>().Embedded;
			Assert.AreEqual(1, mappings.Count);
			var first = mappings.First();
			Assert.AreEqual(":", first.Prefix);
		}

		[Test]
		public void Should_extract_target_class_mappings()
		{
			var mapping = CreateDocumentMapping<DocMap>().Embedded.First();
			Assert.AreEqual(typeof(Author), mapping.Class.MappedClass);
		}

		[Test]
		public void Should_detect_properly_if_it_is_a_collection()
		{
			var mapping = CreateDocumentMapping<DocMap>().Embedded.First();
			Assert.IsTrue(mapping.IsCollection);
		}

		[Test]
		public void Should_detect_properly_if_it_is_not_a_collection()
		{
			var mapping = CreateDocumentMapping<Doc2Map>().Embedded.First();
			Assert.IsFalse(mapping.IsCollection);
		}

		class Doc
		{
			public IList<Author> Authors { get; set; }
			public string Name { get; set; }
		}

		class Author
		{
			public string Name { get; set; }
		}

		class DocMap : DocumentMap<Doc>
		{
			public DocMap()
			{
				Embedded(x => x.Authors)
					.Depth(3)
					.Prefix(":")
					.TargetElement(typeof(Author));
			}
		}

		class Doc2
		{
			public Author Author { get; set; }
		}

		class Doc2Map : DocumentMap<Doc2>
		{
			public Doc2Map()
			{
				Embedded(d => d.Author);
			}
		}
	}
}