using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using NHibernate.Search.Engine;
using NHibernate.Search.Fluent.Mapping.Definitions;
using NHibernate.Search.Mapping;
using NHibernate.Search.Mapping.Definition;

namespace NHibernate.Search.Fluent.Mapping
{
	using Type = System.Type;

	public class FluentSearchMappingDefinition : ISearchMappingDefinition
	{
		private readonly IDictionary<Type, string> indexedDefinitions;
		private readonly IDictionary<ICustomAttributeProvider, float> boostValues;
		private readonly IDictionary<MemberInfo, MemberInfo> documentIDs;
		private readonly IDictionary<ICustomAttributeProvider, Type> analyzers;
		private readonly IDictionary<Type, IList<IClassBridgeDefinition>> classBridges;
		private readonly IDictionary<MemberInfo, IIndexedEmbeddedDefinition> embeddings;
		private readonly IDictionary<MemberInfo, IList<IFieldDefinition>> fields;

		public FluentSearchMappingDefinition(IDocumentMap documentMap)
			: this(new []{documentMap})
		{
		}

		public FluentSearchMappingDefinition(IEnumerable<IDocumentMap> documentMaps)
		{
			indexedDefinitions = new Dictionary<Type, string>();
			boostValues = new Dictionary<ICustomAttributeProvider, float>();
			documentIDs = new Dictionary<MemberInfo, MemberInfo>();
			analyzers = new Dictionary<ICustomAttributeProvider, Type>();
			classBridges = new Dictionary<Type, IList<IClassBridgeDefinition>>();
			embeddings = new Dictionary<MemberInfo, IIndexedEmbeddedDefinition>();
			fields = new Dictionary<MemberInfo, IList<IFieldDefinition>>();
			
			foreach (var map in documentMaps)
			{
				indexedDefinitions.Add(map.DocumentType,  map.Name);
				documentIDs[map.IdProperty] = map.IdProperty;
				classBridges.Add(map.DocumentType, map.ClassBridges);
				
				boostValues = boostValues.Concat(map.BoostValues).ToDictionary(x => x.Key, x => x.Value);
				analyzers = analyzers.Concat(map.Analyzers).ToDictionary(x => x.Key, x => x.Value);
				embeddings = embeddings.Concat(map.EmbeddedDefs).ToDictionary(x => x.Key, x => x.Value);
				fields = fields.Concat(map.FieldMappings).ToDictionary(x => x.Key, x => x.Value);
			}
		}

		public IIndexedDefinition Indexed(Type type)
		{
			return new IndexedDefinition {Index = indexedDefinitions[type]};
		}

		public IList<FilterDef> FullTextFilters(Type type)
		{
			return new List<FilterDef>();
		}

		public IList<IClassBridgeDefinition> ClassBridges(Type type)
		{
			IList<IClassBridgeDefinition> result;
			if (classBridges.TryGetValue(type, out result))
				return result;
			return new List<IClassBridgeDefinition>();
		}

		public IFieldBridgeDefinition FieldBridge(MemberInfo member)
		{
			return null;
		}

		public IDocumentIdDefinition DocumentId(MemberInfo member)
		{
			MemberInfo def;
			if (documentIDs.TryGetValue(member, out def))
				return new DocumentIdDefinition{Name = def.Name};
			return null;
		}

		public IList<IFieldDefinition> FieldDefinitions(MemberInfo member)
		{
			IList<IFieldDefinition> defs;
			if (fields.TryGetValue(member, out defs))
				return defs;
			return new List<IFieldDefinition>();
		}

		public IIndexedEmbeddedDefinition IndexedEmbedded(MemberInfo member)
		{
			IIndexedEmbeddedDefinition def;
			if (embeddings.TryGetValue(member, out def))
				return def;
			return null;
		}

		public bool HasContainedInDefinition(MemberInfo member)
		{
			return false;
		}

		public IDateBridgeDefinition DateBridge(MemberInfo member)
		{
			return null;
		}

		public IEnumerable<IParameterDefinition> BridgeParameters(ICustomAttributeProvider member)
		{
			return Enumerable.Empty<IParameterDefinition>();
		}

		public IAnalyzerDefinition Analyzer(ICustomAttributeProvider member)
		{
			Type type;
			if (analyzers.TryGetValue(member, out type))
				return new AnalyzerDefinition { Type = type};
			return null;
		}

		public IBoostDefinition Boost(ICustomAttributeProvider member)
		{
			float boost;
			if (boostValues.TryGetValue(member, out boost))
				return new BoostDefinition{Value = boost};
			return null;
		}
	}
}