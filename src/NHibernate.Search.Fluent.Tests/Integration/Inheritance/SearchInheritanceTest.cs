using Lucene.Net.Analysis.Standard;
using Lucene.Net.QueryParsers;
using NHibernate.Cfg;
using NHibernate.Event;
using NHibernate.Search.Event;
using NHibernate.Search.Fluent.Cfg;
using NUnit.Framework;
using Version = Lucene.Net.Util.Version;

namespace NHibernate.Search.Fluent.Tests.Integration.Inheritance
{
	[TestFixture]
	public class SearchInheritanceTest : FluentSearchTestBase
	{
		protected override void ConfigureSearch(Configuration cfg)
		{
			FluentSearch
				.Configure(cfg)
				.MappingClass<SearchMappings>()
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
			Session.Save(new OrderDocument
			{
				Name = "doc 1",
				OrderId = "xxx",
				References = new []
				{
					new Reference{Description = "code red"},
					new Reference{Description = "code blue"},
				}
			});

			Session.Save(new InvoiceDocument
			{
				Name = "doc 2",
				InvoiceId = "yyy",
				References = new []
				{
					new Reference{Description = "code blue"},
				}
			});
			Session.Flush();
			Session.Clear();
		}

		protected override void Cleanup()
		{
			SearchSession.PurgeAll(typeof (Document));
			SearchSession.PurgeAll(typeof (OrderDocument));
			SearchSession.PurgeAll(typeof (InvoiceDocument));
			SearchSession.PurgeAll(typeof (Reference));
		}

		[Test]
		public void Should_be_able_to_get_results_from_by_querying_by_inheritors()
		{
			var parser = new QueryParser(Version.LUCENE_29, "Name", new StandardAnalyzer(Version.LUCENE_29));
			var query = parser.Parse("doc*");
			var result = SearchSession
				.CreateFullTextQuery(query, typeof(OrderDocument), typeof(InvoiceDocument))
				.List<Document>();

			Assert.AreEqual(2, result.Count);
		}

		[Test]
		public void Should_get_results_by_querying_a_collection_in_base_class()
		{
			var parser = new QueryParser(Version.LUCENE_29, "References.Description", new StandardAnalyzer(Version.LUCENE_29));
			var query = parser.Parse("red");
			var result = SearchSession
				.CreateFullTextQuery(query, typeof(OrderDocument), typeof(InvoiceDocument))
				.List<Document>();

			Assert.AreEqual(1, result.Count);
		}

		[Test]
		[Ignore("TODO, is failing but will be nice to have it running")]
		public void Should_be_able_to_get_results_from_whole_hierarchy_by_querying_with_abstarct_base_class()
		{
			var parser = new QueryParser(Version.LUCENE_29, "Name", new StandardAnalyzer(Version.LUCENE_29));
			var query = parser.Parse("doc*");
			var result = SearchSession
				.CreateFullTextQuery(query, typeof(Document))
				.List<Document>();

			Assert.AreEqual(2, result.Count);
		}
	}
}