using System;
using System.Collections.Generic;
using System.Linq;
using NHibernate.Search.Attributes;
using NHibernate.Search.Engine;
using NHibernate.Search.Impl;
using NHibernate.Search.Mapping.Definition;
using NHibernate.Search.Mapping.Model;

namespace NHibernate.Search.Mapping.AttributeBased
{
	using Type = System.Type;

	public class AttributedMappingDefinition : ISearchMappingDefinition
	{
		public IIndexedDefinition IndexedDefinition(Type type)
		{
			return AttributeUtil.GetAttribute<IndexedAttribute>(type);
		}

		public IList<FilterDef> FullTextFilters(Type type)
		{
			return AttributeUtil
				.GetAttributes<FullTextFilterDefAttribute>(type, false)
				.Select(CreateFilterDefinition)
				.ToList();
		}

		public IList<IClassBridgeDefinition> ClassBridges(Type type)
		{
			return AttributeUtil.GetAttributes<ClassBridgeAttribute>(type);
		}

		public IFieldBridgeDefinition FieldBridge(Type type)
		{
			return AttributeUtil.GetAttribute<FieldBridgeAttribute>(type);
		}

		public IList<IParameterDefinition> BridgeParameters(Type type)
		{
			return AttributeUtil.GetAttributes<ParameterAttribute>(type);
		}

		private FilterDef CreateFilterDefinition(FullTextFilterDefAttribute att)
		{
			try
			{
			    Activator.CreateInstance(att.Impl);
			}
			catch (Exception ex)
			{
				throw new SearchException("Unable to create Filter class: " + att.Impl.FullName, ex);
			}

			var filterDefinition = new FilterDef
			{
				Name = att.Name, 
				Cache = att.Cache, 
				Impl = att.Impl
			};

			foreach (var method in filterDefinition.Impl.GetMethods())
			{
				if (AttributeUtil.HasAttribute<FactoryAttribute>(method))
				{
					if (filterDefinition.FactoryMethod != null)
						throw new SearchException("Multiple Factory methods found " + filterDefinition.Name + ":" +
												  filterDefinition.Impl.FullName + "." + method.Name);
					filterDefinition.FactoryMethod = method;
				}

				if (AttributeUtil.HasAttribute<KeyAttribute>(method))
				{
					if (filterDefinition.KeyMethod != null)
						throw new SearchException("Multiple Key methods found " + filterDefinition.Name + ":" +
												  filterDefinition.Impl.FullName + "." + method.Name);
					filterDefinition.KeyMethod = method;
				}
			}

			foreach (var prop in filterDefinition.Impl.GetProperties())
			{
				if (AttributeUtil.HasAttribute<FilterParameterAttribute>(prop))
				{
					filterDefinition.Setters[prop.Name] = prop;
				}
			}

			return filterDefinition;
		}
	}
}