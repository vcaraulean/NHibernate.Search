using System.Reflection;
using NHibernate.Search.Bridge;
using NHibernate.Search.Fluent.Mapping.Definitions;
using NHibernate.Search.Mapping.Definition;

namespace NHibernate.Search.Fluent.Mapping.Parts
{
	using Attributes;
	using Type = System.Type;
	public class FieldMappingPart : IHasBridge, IHasAnalyzer, IHasIndex, IHasStore
	{
		private readonly IDocumentMap documentMap;
		private readonly PropertyInfo property;
		Index? IHasIndex.Index { get; set; }
		Store? IHasStore.Store { get; set; }
		Type IHasAnalyzer.AnalyzerType { get; set; }
		IFieldBridge IHasBridge.FieldBridge { get; set; }
		string name { get; set; }
		float? boost { get; set; }

		public FieldMappingPart(IDocumentMap documentMap, PropertyInfo property)
		{
			this.documentMap = documentMap;
			this.property = property;
		}

		public FieldMappingPart Name(string name)
		{
			this.name = name;
			return this;
		}

		public FieldMappingPart Boost(float boostValue)
		{
			boost = boostValue;
			documentMap.BoostValues.Add(property, boostValue);
			return this;
		}

		/// <summary>
		/// Defines the Field Index.
		/// </summary>
		/// <returns></returns>
		public IndexPart<FieldMappingPart> Index()
		{
			return new IndexPart<FieldMappingPart>(this);
		}

		/// <summary>
		/// Defines the Field Store.
		/// </summary>
		/// <returns></returns>
		public StorePart<FieldMappingPart> Store()
		{
			return new StorePart<FieldMappingPart>(this);
		}

		/// <summary>
		/// Defines the Field Bridge.
		/// </summary>
		/// <returns></returns>
		public BridgePart<FieldMappingPart> Bridge()
		{
			return new BridgePart<FieldMappingPart>(this);
		}

		/// <summary>
		/// Defines the Field Analyzer.
		/// </summary>
		/// <returns></returns>
		public AnalyzerPart<FieldMappingPart> Analyzer()
		{
			return new AnalyzerPart<FieldMappingPart>(this);
		}

		public IFieldDefinition FieldDefinition
		{
			get
			{ 
				var definition = new FieldDefinition
				{
					Name = name,
				};

				var hasIndex = this as IHasIndex;
				if (hasIndex.Index.HasValue)
					definition.Index = hasIndex.Index.Value;

				var hasStore = this as IHasStore;
				if (hasStore.Store.HasValue)
					definition.Store = hasStore.Store.Value;
				
				var hasAnalyzer = this as IHasAnalyzer;
				if (hasAnalyzer.AnalyzerType != null)
					definition.Analyzer = hasAnalyzer.AnalyzerType;

				return definition;
			}
		}
	}
}