using NHibernate.Cfg;
using NHibernate.Search.Fluent.Cfg;
using NUnit.Framework;

namespace NHibernate.Search.Fluent.Tests.Integration.Bridge
{
	[TestFixture]
	public class CustomFieldBridgeTests : FluentSearchTestBase
	{
		protected override void ConfigureSearch(Configuration cfg)
		{
			FluentSearch
				.Configure(cfg)
				.MappingClass<SearchMappings>()
				.IndexingStrategy().Event()
				.IndexBase("LuceneIndex")
				.DirectoryProvider().FSDirectory();
		}

		protected override void AfterSetup()
		{
			Session.Save(new Customer {Phone = "022 457 54 78"});
			Session.Flush();
			Session.Clear();
		}

		[Test]
		public void Should_find_match_for_string_without_spaces()
		{
			var result = SearchSession
				.CreateFullTextQuery<Customer>("Phone:022457*")
				.List<Customer>();
			CollectionAssert.IsNotEmpty(result);
		}
	}
}