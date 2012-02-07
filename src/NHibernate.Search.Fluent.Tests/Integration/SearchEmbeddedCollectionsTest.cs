using NHibernate.Cfg;
using NHibernate.Event;
using NHibernate.Search.Event;
using NHibernate.Search.Fluent.Cfg;
using NUnit.Framework;

namespace NHibernate.Search.Fluent.Tests.Integration
{
	[TestFixture]
	public class SearchEmbeddedCollectionsTest : FluentSearchTestBase
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
			Session.Save(new Contact
			{
				Name = "Bill Gates",
				Addresses = new[]
				{
					new Address {AddressLine = "Redmond"},
					new Address {AddressLine = "Geneva"}
				}
			});
			Session.Save(new Contact
			{
				Name = "Valeriu Caraulean",
				Addresses = new[]
				{
					new Address {AddressLine = "Geneva"}
				}
			});

			Session.Flush();
			Session.Clear();
		}

		[Test]
		public void Should_find_entities_by_their_address_1()
		{
			var result = SearchSession
				.CreateFullTextQuery<Contact>("Addresses.AddressLine:Gene*")
				.List<Contact>();

			Assert.AreEqual(2, result.Count);
		}

		[Test]
		public void Should_find_entities_by_their_address_2()
		{
			var result = SearchSession
				.CreateFullTextQuery<Contact>("Addresses.AddressLine:Redmond")
				.List<Contact>();

			Assert.AreEqual(1, result.Count);
		}
	}
}