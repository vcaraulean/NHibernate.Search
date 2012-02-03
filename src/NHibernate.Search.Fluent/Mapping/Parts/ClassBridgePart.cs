using NHibernate.Search.Attributes;
using NHibernate.Search.Fluent.Mapping.Definitions;
using NHibernate.Search.Mapping.Definition;

namespace NHibernate.Search.Fluent.Mapping.Parts
{
	// TODO: Set bridge parameters
	public class ClassBridgePart<T> : FluentMappingPart, IHasAnalyzer, IHasIndex, IHasStore
	{
		private readonly System.Type bridgeType;
		System.Type IHasAnalyzer.AnalyzerType { get; set; }
		Attributes.Store? IHasStore.Store { get; set; }
		Index? IHasIndex.Index { get; set; }

		public ClassBridgePart(System.Type bridgeType)
		{
			this.bridgeType = bridgeType;
		}

		public IClassBridgeDefinition BridgeDefinition
		{
			get
			{
				var definition = new ClassBridgeDefinition
				{
					Analyzer = (this as IHasAnalyzer).AnalyzerType,
					Impl = bridgeType,
				};

				var fluentMappingPart = this as IFluentMappingPart;

				definition.Name = fluentMappingPart.Name;

				if (fluentMappingPart.Boost.HasValue)
					definition.Boost = fluentMappingPart.Boost.Value;

				var hasIndex = this as IHasIndex;
				if (hasIndex.Index.HasValue)
					definition.Index = hasIndex.Index.Value;

				var hasStore = this as IHasStore;
				if (hasStore.Store.HasValue)
					definition.Store = hasStore.Store.Value;

				return definition;
			}
		}

		public AnalyzerPart<ClassBridgePart<T>> Analyzer()
		{
			return new AnalyzerPart<ClassBridgePart<T>>(this);
		}

		public IndexPart<ClassBridgePart<T>> Index()
		{
			return new IndexPart<ClassBridgePart<T>>(this);
		}

		public StorePart<ClassBridgePart<T>> Store()
		{
			return new StorePart<ClassBridgePart<T>>(this);
		}
	}
}