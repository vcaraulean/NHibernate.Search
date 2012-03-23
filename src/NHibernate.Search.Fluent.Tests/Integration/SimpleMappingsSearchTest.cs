using Lucene.Net.Analysis.Standard;
using Lucene.Net.QueryParsers;
using Lucene.Net.Util;
using NHibernate.Cfg;
using NHibernate.Search.Fluent.Cfg;
using NUnit.Framework;

namespace NHibernate.Search.Fluent.Tests.Integration
{
	[TestFixture]
	public class SimpleMappingsSearchTest : FluentSearchTestBase
	{
		protected override void ConfigureSearch(Configuration cfg)
		{
			FluentSearch
				.Configure(cfg)
				.MappingClass<DomainSearchMapping>()
				.IndexingStrategy().Event()
				.DirectoryProvider().RAMDirectory();
		}

		protected override void AfterSetup()
		{
			var switzerland = new Country {Name = "Switzerland"};
			var france = new Country {Name = "France"};
			var swaziland = new Country {Name = "Swaziland"};

			Session.Save(switzerland);
			Session.Save(france);
			Session.Save(swaziland);
			Session.Flush();
			Session.Clear();
		}

		[Test]
		public void Should_search_mapped_property_by_whole_word()
		{
			var result = SearchSession
				.CreateFullTextQuery<Country>("Name:Switzerland")
				.List<Country>();
			Assert.AreEqual(1, result.Count);
		}

		[Test]
		public void Should_search_by_partial_match()
		{
			var result = SearchSession
				.CreateFullTextQuery<Country>("Name:Sw*")
				.List<Country>();
			Assert.AreEqual(2, result.Count);
		}

		[Test]
		public void Should_parse_using_lucene_query()
		{
			var parser = new MultiFieldQueryParser(Version.LUCENE_29, new[] {"Name"}, new StandardAnalyzer(Version.LUCENE_29));
			var query = parser.Parse("Switzerland");
			var q = SearchSession.CreateFullTextQuery(query, typeof (Country))
				.List<Country>();
			Assert.AreEqual(1, q.Count);
		}
	}
}