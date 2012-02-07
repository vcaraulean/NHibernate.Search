using System;
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
				OrderId = "xxx"
			});

			Session.Save(new InvoiceDocument
			{
				Name = "doc 2",
				InvoiceId = "yyy"
			});
			Session.Flush();
			Session.Clear();
		}

		protected override void Cleanup()
		{
			SearchSession.PurgeAll(typeof (Document));
			SearchSession.PurgeAll(typeof (OrderDocument));
			SearchSession.PurgeAll(typeof (InvoiceDocument));
		}

		[Test]
		public void Should_be_able_to_get_results_from_whole_hierarchy()
		{
			var nhresult = Session.QueryOver<Document>().List<Document>();
			Console.WriteLine(nhresult.Count);
			
			var parser = new QueryParser(Version.LUCENE_29, "Name", new StandardAnalyzer(Version.LUCENE_29));
			var query = parser.Parse("doc*");
			var result = SearchSession
				.CreateFullTextQuery(query, typeof(Document))
				.List<Document>();

			Assert.AreEqual(2, result.Count);
		}

	}
}