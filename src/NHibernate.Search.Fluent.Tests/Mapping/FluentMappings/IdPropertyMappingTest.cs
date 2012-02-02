using NHibernate.Search.Fluent.Exceptions;
using NHibernate.Search.Fluent.Mapping;
using NUnit.Framework;

namespace NHibernate.Search.Fluent.Tests.Mapping.FluentMappings
{
	[TestFixture]
	public class IdPropertyMappingTest : PropertyMappingTest
	{
		[Test]
		public void Should_extract_document_id_mapping()
		{
			Assert.AreEqual("Id", CreateDocumentMapping<DocMap>().DocumentId.PropertyName);
		}

		[Test]
		public void Should_throw_if_id_is_declared_twice()
		{
			Assert.That(() => CreateDocumentMapping<BadDocMap>(),
			            Throws.TargetInvocationException.With.InnerException.TypeOf<FluentMappingException>());
		}

		private class Doc
		{
			public int Id { get; set; }
			public string Prop { get; set; }
		}

		private class DocMap : DocumentMap<Doc>
		{
			public DocMap()
			{
				Id(d => d.Id);
			}
		}

		private class BadDocMap : DocumentMap<Doc>
		{
			public BadDocMap()
			{
				Id(d => d.Id);
				Id(d => d.Prop);
			}
		}
	}
}