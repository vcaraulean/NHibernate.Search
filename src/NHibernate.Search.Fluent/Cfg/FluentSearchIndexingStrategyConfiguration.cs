using NHibernate.Search.Event;

namespace NHibernate.Search.Fluent.Cfg
{
	public class FluentSearchIndexingStrategyConfiguration
	{
		private readonly FluentSearchConfiguration cfg;

		internal FluentSearchIndexingStrategyConfiguration(FluentSearchConfiguration cfg)
		{
			this.cfg = cfg;
		}

		private FluentSearchConfiguration setStrategy(string name)
		{
			(cfg as IFluentSearchConfiguration).Configuration.Properties.Add(Environment.IndexingStrategy, name);
			return cfg;
		}

		/// <summary>
		/// Sets the IndexingStrategy property to "manual"
		/// </summary>
		/// <returns></returns>
		public FluentSearchConfiguration Manual()
		{
			return setStrategy("manual");
		}

		/// <summary>
		/// Sets the IndexingStrategy property to "event"
		/// </summary>
		/// <param name="registerListeners">
		/// Register NHibernate listeners. 
		/// Listeners are added to existent listeners collection.
		/// Default value - true.
		/// </param>
		/// <returns></returns>
		public FluentSearchConfiguration Event(bool registerListeners = true)
		{
			if (registerListeners)
			{
				var configuration = (cfg as IFluentSearchConfiguration).Configuration;

				configuration.AddListener(el => el.PostInsertEventListeners, new FullTextIndexEventListener());
				configuration.AddListener(el => el.PostUpdateEventListeners, new FullTextIndexEventListener());
				configuration.AddListener(el => el.PostDeleteEventListeners, new FullTextIndexEventListener());

				configuration.AddListener(el => el.PostCollectionRecreateEventListeners, new FullTextIndexCollectionEventListener());
				configuration.AddListener(el => el.PostCollectionRemoveEventListeners, new FullTextIndexCollectionEventListener());
				configuration.AddListener(el => el.PostCollectionUpdateEventListeners, new FullTextIndexCollectionEventListener());
			}

			return setStrategy("event");
		}
	}
}