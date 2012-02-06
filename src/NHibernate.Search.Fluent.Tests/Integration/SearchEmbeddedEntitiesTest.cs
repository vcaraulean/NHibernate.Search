using NHibernate.Cfg;
using NHibernate.Event;
using NHibernate.Search.Event;
using NHibernate.Search.Fluent.Cfg;
using NUnit.Framework;

namespace NHibernate.Search.Fluent.Tests.Integration
{
	[TestFixture]
	public class SearchEmbeddedEntitiesTest : FluentSearchTestBase
	{
		protected override void ConfigureSearch(Configuration cfg)
		{
			FluentSearch
				.Configure(cfg)
				.MappingClass<DomainSearchMapping>()
				.IndexingStrategy().Event()
				.IndexBase("LuceneIndex")
				.DirectoryProvider().FSDirectory();

			cfg.SetListener(ListenerType.PostUpdate, new FullTextIndexEventListener());
			cfg.SetListener(ListenerType.PostInsert, new FullTextIndexEventListener());
			cfg.SetListener(ListenerType.PostDelete, new FullTextIndexEventListener());
			cfg.SetListener(ListenerType.PostCollectionRecreate, new FullTextIndexCollectionEventListener());
			cfg.SetListener(ListenerType.PostCollectionRemove, new FullTextIndexCollectionEventListener());
			cfg.SetListener(ListenerType.PostCollectionUpdate, new FullTextIndexCollectionEventListener());
		}

		protected override void AfterSetup()
		{
			var switzerland = new Country {Name = "Switzerland"};
			Session.Save(new Address
			{
				AddressLine = "Geneve",
				Country = switzerland
			});
			Session.Save(new Address
			{
				AddressLine = "Lausanne",
				Country = switzerland
			});

			Session.Flush();
			Session.Clear();
		}

		[Test]
		public void Should_get_results_when_searching_by_embedded_entity()
		{
			var result = SearchSession
				.CreateFullTextQuery<Address>("Country.Name:Switzerland")
				.List<Address>();

			Assert.AreEqual(2, result.Count);
		}

		[Test]
		public void Should_search_in_address_line()
		{
			var result = SearchSession
				.CreateFullTextQuery<Address>("AddressLine:lausanne")
				.List<Address>();
			Assert.AreEqual(1, result.Count);
		}

		protected override void Cleanup()
		{
			SearchSession.PurgeAll(typeof(Address));
			SearchSession.PurgeAll(typeof(Country));
		}
	}
}