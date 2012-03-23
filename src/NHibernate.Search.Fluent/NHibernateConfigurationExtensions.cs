using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using NHibernate.Cfg;
using NHibernate.Event;

namespace NHibernate.Search.Fluent
{
	public static class NHibernateConfigurationExtensions
	{
		public static void AddListener<TListener>(
			this Configuration configuration,
			Expression<Func<EventListeners, TListener[]>> expression,
			TListener listenerImpl)
		{
			var propertyInfo = ReflectionExtensions.GetProperty(expression);

			var existentListeners = (TListener[])propertyInfo.GetValue(configuration.EventListeners, null);

			var newListeners = new List<TListener>(existentListeners) { listenerImpl }.ToArray();

			propertyInfo.SetValue(configuration.EventListeners, newListeners, null);
		}

		public static void AddListeners<TListener>(
			this Configuration configuration,
			Expression<Func<EventListeners, TListener[]>> expression,
			IEnumerable<TListener> listeners)
		{
			foreach (var listener in listeners)
			{
				configuration.AddListener(expression, listener);
			}
		}
	}
}