using System;
using System.Collections.Generic;
using Lucene.Net.Analysis;
using NHibernate.Cfg;
using NHibernate.Search.Fluent.Cfg;
using NHibernate.Search.Mapping;
using NHibernate.Search.Store;
using NUnit.Framework;

namespace NHibernate.Search.Fluent.Tests.Cfg
{
	[TestFixture]
	public class FluentConfigurationTests
	{
		private Configuration nhConfig;

		[SetUp]
		public void SetUp()
		{
			nhConfig = new Configuration();
		}

		[Test]
		public void Should_configure_IndexBase()
		{
			FluentSearch
				.Configure(nhConfig)
				.IndexBase("localindex");

			Assert.AreEqual("localindex", nhConfig.Properties["hibernate.search.default." + Environment.IndexBase]);
		}

		[Test]
		public void Should_configure_Mapping_class()
		{
			FluentSearch
				.Configure(nhConfig)
				.MappingClass(typeof (SearchMapping));

			Assert.AreEqual(typeof (SearchMapping).AssemblyQualifiedName, nhConfig.Properties[Environment.MappingClass]);
		}

		[Test]
		public void Should_throw_when_mapping_class_not_implementing_ISearchMapping()
		{
			Assert.Throws<ArgumentException>(() => FluentSearch
			                                            	.Configure(nhConfig)
															.MappingClass(typeof(WrongSearchMapping)));
		}

		[Test]
		public void Should_set_default_analyzer()
		{
			FluentSearch
				.Configure(nhConfig)
				.DefaultAnalyzer().Keyword();

			Assert.AreEqual(typeof(KeywordAnalyzer).AssemblyQualifiedName, nhConfig.Properties[Environment.AnalyzerClass]);

			nhConfig = new Configuration();
			FluentSearch
				.Configure(nhConfig)
				.DefaultAnalyzer<SimpleAnalyzer>();

			Assert.AreEqual(typeof(SimpleAnalyzer).AssemblyQualifiedName, nhConfig.Properties[Environment.AnalyzerClass]);
		}

		[Test]
		public void Should_set_directory_provider()
		{
			FluentSearch
				.Configure(nhConfig)
				.DirectoryProvider().RAMDirectory();

			Assert.AreEqual(typeof(RAMDirectoryProvider).AssemblyQualifiedName, nhConfig.Properties["hibernate.search.default.directory_provider"]);
			
			nhConfig = new Configuration();
			FluentSearch
				.Configure(nhConfig)
				.DirectoryProvider().FSDirectory();

			Assert.AreEqual(typeof(FSDirectoryProvider).AssemblyQualifiedName, nhConfig.Properties["hibernate.search.default.directory_provider"]);
		}

		[Test]
		public void Should_set_indexing_strategy()
		{
			FluentSearch
				.Configure(nhConfig)
				.IndexingStrategy().Event();

			Assert.AreEqual("event", nhConfig.Properties[Environment.IndexingStrategy]);

			nhConfig = new Configuration();
			FluentSearch
				.Configure(nhConfig)
				.IndexingStrategy().Manual();

			Assert.AreEqual("manual", nhConfig.Properties[Environment.IndexingStrategy]);
		}

		private class SearchMapping : ISearchMapping
		{
			public ICollection<DocumentMapping> Build(Configuration cfg)
			{
				throw new NotImplementedException();
			}
		}

		private class WrongSearchMapping
		{
		}
	}
}