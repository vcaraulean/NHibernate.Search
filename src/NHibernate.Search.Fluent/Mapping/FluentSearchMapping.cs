using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using NHibernate.Cfg;
using NHibernate.Search.Mapping;

namespace NHibernate.Search.Fluent.Mapping
{
	using Type = System.Type;

	public abstract class FluentSearchMapping : ISearchMapping
	{
		private readonly IList<Assembly> assemblies;
		private readonly IList<Type> explicitProviders;
		private bool assert;
		
		protected FluentSearchMapping()
		{
			assemblies = new List<Assembly>();
			explicitProviders = new List<Type>();
		}

		public ICollection<DocumentMapping> Build(Configuration cfg)
		{
			Configure();

			var mappings = GetMappingDocuments().Select(provider =>
			{
				//if (assert)
				//    provider.AssertIsValid();
				var builder = new FluentSearchMappingBuilder(provider);
				return builder.Build(provider.DocumentType);
			});

			return mappings.ToList();
		}

		protected abstract void Configure();

		public IEnumerable<IDocumentMap> GetMappingDocuments()
		{
			var mappingProviderType = typeof (IDocumentMap);

			var types = assemblies
				.SelectMany(a => a.GetTypes())
				.Where(t =>
				       !t.IsGenericType &&
				       !t.IsAbstract &&
				       mappingProviderType.IsAssignableFrom(t));

			foreach (var type in types)
				yield return Activator.CreateInstance(type) as IDocumentMap;

			foreach (var type in explicitProviders)
				yield return Activator.CreateInstance(type) as IDocumentMap;
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

		protected void Add<TDocumentMap>() where TDocumentMap : IDocumentMap
		{
			explicitProviders.Add(typeof(TDocumentMap));
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