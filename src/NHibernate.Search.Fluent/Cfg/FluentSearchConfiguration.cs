using System;
using Lucene.Net.Analysis;
using NHibernate.Cfg;
using NHibernate.Search.Fluent.Exceptions;
using NHibernate.Search.Fluent.Mapping.Parts;
using NHibernate.Search.Mapping;

namespace NHibernate.Search.Fluent.Cfg
{
	using Type = System.Type;

	public interface IFluentSearchConfiguration
	{
		Configuration Configuration { get; }
	}

	public class FluentSearchConfiguration : IFluentSearchConfiguration, IHasAnalyzer
	{
		private readonly Configuration configuration;

		Configuration IFluentSearchConfiguration.Configuration
		{
			get { return configuration; }
		}

		public FluentSearchConfiguration(Configuration cfg)
		{
			configuration = cfg;
		}

		/// <summary>
		/// Specifies the IndexBase property
		/// </summary>
		/// <param name="path"></param>
		/// <returns></returns>
		public FluentSearchConfiguration IndexBase(string path)
		{
			if (string.IsNullOrEmpty(path))
				throw new ConfigurationException("IndexBase cannot be null or empty");

			configuration.Properties.Add("hibernate.search.default." + Environment.IndexBase, path);

			return this;
		}

		/// <summary>
		/// Specifies the MappingClass property
		/// </summary>
		/// <returns></returns>
		public FluentSearchConfiguration MappingClass<TSearchMapping>() where TSearchMapping : ISearchMapping
		{
			return MappingClass(typeof(TSearchMapping));
		}

		/// <summary>
		/// Specifies the MappingClass property
		/// </summary>
		/// <param name="mappingClassType"></param>
		/// <returns></returns>
		public FluentSearchConfiguration MappingClass(Type mappingClassType)
		{
			if (!typeof(ISearchMapping).IsAssignableFrom(mappingClassType))
				throw new ArgumentException("Must implement ISearchMapping", "mappingClassType");

			configuration.Properties.Add(Environment.MappingClass, mappingClassType.AssemblyQualifiedName);
			return this;
		}

		/// <summary>
		/// Fluently set the Default Analyzer
		/// </summary>
		/// <returns></returns>
		public AnalyzerPart<FluentSearchConfiguration> DefaultAnalyzer()
		{
			return new AnalyzerPart<FluentSearchConfiguration>(this);
		}

		/// <summary>
		/// Defines the Default Analyzer property
		/// Shortcut for DefaultAnalyzer().Custom&lt;TAnalyzer&gt;()
		/// </summary>
		/// <typeparam name="TAnalyzer"></typeparam>
		/// <returns></returns>
		public FluentSearchConfiguration DefaultAnalyzer<TAnalyzer>() where TAnalyzer : Analyzer, new()
		{
			return DefaultAnalyzer().Custom<TAnalyzer>();
		}

		/// <summary>
		/// Fluently set the DirectoryProvider
		/// </summary>
		/// <returns></returns>
		public FluentSearchDirectoryProviderConfiguration DirectoryProvider()
		{
			return new FluentSearchDirectoryProviderConfiguration(this);
		}

		/// <summary>
		/// Fluently set the IndexingStrategy property
		/// </summary>
		/// <returns></returns>
		public FluentSearchIndexingStrategyConfiguration IndexingStrategy()
		{
			return new FluentSearchIndexingStrategyConfiguration(this);
		}

		Type IHasAnalyzer.AnalyzerType
		{
			get
			{
				if (!configuration.Properties.ContainsKey(Environment.AnalyzerClass))
					return null;
				return Type.GetType(configuration.Properties[Environment.AnalyzerClass]);
			}
			set { configuration.Properties[Environment.AnalyzerClass] = value.AssemblyQualifiedName; }
		}
	}
}