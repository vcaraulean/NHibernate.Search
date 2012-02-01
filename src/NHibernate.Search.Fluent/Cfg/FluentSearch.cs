using NHibernate.Cfg;

namespace NHibernate.Search.Fluent.Cfg
{
	public static class FluentSearch
	{
		/// <summary>
		/// Fluently configure NHibernate.Search Mappings using the underlying NHibernate Configuration.
		/// </summary>
		/// <param name="cfg"></param>
		/// <returns></returns>
		public static FluentSearchConfiguration Configure(Configuration cfg)
		{
			return new FluentSearchConfiguration(cfg);
		}
	}
}