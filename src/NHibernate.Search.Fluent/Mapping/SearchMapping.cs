using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using NHibernate.Cfg;
using NHibernate.Search.Mapping;

namespace NHibernate.Search.Fluent.Mapping
{
	using Type = System.Type;

	public abstract class SearchMapping : ISearchMapping
	{
		private readonly IList<Assembly> assemblies;
		private readonly IList<Type> explicitProviders;
		private bool assert;

		protected SearchMapping()
		{
			assemblies = new List<Assembly>();
			explicitProviders = new List<Type>();
		}

		public ICollection<DocumentMapping> Build(Configuration cfg)
		{
			Configure();

			var mappings = GetMappingProviders().Select(provider =>
			{
				if (assert)
					provider.AssertIsValid();
				return provider.GetMapping();
			});

			return mappings.ToList();
		}

		protected abstract void Configure();

		private IEnumerable<IDocumentMappingProvider> GetMappingProviders()
		{
			var mappingProviderType = typeof (IDocumentMappingProvider);

			var types = assemblies
				.SelectMany(a => a.GetTypes())
				.Where(t =>
				       !t.IsGenericType &&
				       !t.IsAbstract &&
				       mappingProviderType.IsAssignableFrom(t));

			foreach (var type in types)
				yield return Activator.CreateInstance(type) as IDocumentMappingProvider;

			foreach (var type in explicitProviders)
				yield return Activator.CreateInstance(type) as IDocumentMappingProvider;
		}

		protected void AddAssemblyContaining<T>()
		{
			var type = typeof (T);
			var asm = Assembly.GetAssembly(type);

			if (asm == null)
			{
				throw new InvalidOperationException(
					String.Format("The assembly with the specified type '{0}' doesn't exists", type.Name));
			}

			AddAssembly(asm);
		}

		protected void Add<TDocumentMappingProvider>() where TDocumentMappingProvider : IDocumentMappingProvider
		{
			explicitProviders.Add(typeof (TDocumentMappingProvider));
		}

		protected void AddAssembly(Assembly asm)
		{
			if (!assemblies.Contains(asm))
			{
				assemblies.Add(asm);
			}
		}

		protected void AssertIsValid()
		{
			assert = true;
		}
	}
}