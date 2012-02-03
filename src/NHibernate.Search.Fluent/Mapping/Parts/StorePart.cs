using System;

namespace NHibernate.Search.Fluent.Mapping.Parts
{
	using Attributes;

	public interface IHasStore
	{
		Store? Store { get; set; }
	}

	public class StorePart<T> where T : IHasStore
	{
		private T hasStore;

		public StorePart(T hasStore)
		{
			this.hasStore = hasStore;
		}

		private T store(Store store)
		{
			hasStore.Store = store;
			return hasStore;
		}

		/// <summary>
		/// Sets the Store mode to Yes
		/// </summary>
		/// <returns></returns>
		public T Yes()
		{
			return store(Store.Yes);
		}

		/// <summary>
		/// Sets the Store mode to No
		/// </summary>
		/// <returns></returns>
		public T No()
		{
			return store(Store.No);
		}

		/// <summary>
		/// Sets the Store mode to Compress
		/// Should not be used, not implemented in NHibernate.Search.
		/// </summary>
		/// <returns></returns>
		[Obsolete("Should not be used, not implemented in NHibernate.Search")]
		public T Compress()
		{
			return store(Store.Compress);
		}
	}
}