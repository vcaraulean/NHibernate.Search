using NHibernate.Cfg;
using NHibernate.Search.Fluent.Mapping;
using NUnit.Framework;

namespace NHibernate.Search.Fluent.Tests.Mapping
{
	[TestFixture]
	public class MappingHierachiesAndIDsTest
	{

		[Test]
		public void Should_not_throw_when_using_same_id_property_in_multiple_mappings()
		{
			new SearchMap().Build(new Configuration());
		}

		class SearchMap : FluentSearchMapping
		{
			protected override void Configure()
			{
				AssertIsValid();
				Add<Doc1Map>();
				Add<Doc2Map>();
			}
		}

		abstract class DocBase
		{
			public int Id { get; set; }
		}
		class Doc1 : DocBase{}
		class Doc2 : DocBase{}

		class Doc1Map : DocumentMap<Doc1>
		{
			public Doc1Map()
			{
				Id(x => x.Id);
			}
		}
		class Doc2Map : DocumentMap<Doc2>
		{
			public Doc2Map()
			{
				Id(x => x.Id);
			}
		}
	}
}