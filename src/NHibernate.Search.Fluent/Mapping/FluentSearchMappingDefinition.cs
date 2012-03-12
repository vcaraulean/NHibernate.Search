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
		private readonly IDictionary<MemberInfo, IFieldBridgeDefinition> fieldBridges;

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
			fieldBridges = new Dictionary<MemberInfo, IFieldBridgeDefinition>();
			
			foreach (var map in documentMaps)
			{
				indexedDefinitions.Add(map.DocumentType,  map.Name);
				documentIDs[map.IdProperty] = map.IdProperty;
				classBridges.Add(map.DocumentType, map.ClassBridges);
				
				boostValues = boostValues.Concat(map.BoostValues).ToDictionary(x => x.Key, x => x.Value);
				analyzers = analyzers.Concat(map.Analyzers).ToDictionary(x => x.Key, x => x.Value);
				Merge(map.EmbeddedDefs);
				Merge(map.FieldMappings);

				fieldBridges = fieldBridges.Concat(map.FieldBridges).ToDictionary(x => x.Key, x => x.Value);
			}
		}

		private void Merge(IDictionary<MemberInfo, IIndexedEmbeddedDefinition> embeddedDefs)
		{
			foreach (var def in embeddedDefs)
			{
				if (embeddings.ContainsKey(def.Key) == false)
					embeddings[def.Key] = def.Value;
			}
		}

		private void Merge(IDictionary<MemberInfo, IList<IFieldDefinition>> fieldMappings)
		{
			foreach (var fieldMapping in fieldMappings)
			{
				IList<IFieldDefinition> existentDefs;
				if (fields.TryGetValue(fieldMapping.Key, out existentDefs))
				{
					// Merging only IFieldDefinition that have Name != from existent definition
					var missingDefinitions = fieldMapping.Value
						.Where(d => existentDefs.Any(exd => exd.Name == d.Name) == false);
					fields[fieldMapping.Key] = existentDefs.Concat(missingDefinitions).ToList();
				}
				else
					fields[fieldMapping.Key] = fieldMapping.Value;
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
			IFieldBridgeDefinition def;
			if (fieldBridges.TryGetValue(member, out def))
				return def;
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