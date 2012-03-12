using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Lucene.Net.Analysis;
using NHibernate.Search.Bridge;
using NHibernate.Search.Fluent.Exceptions;
using NHibernate.Search.Fluent.Mapping.Definitions;
using NHibernate.Search.Fluent.Mapping.Parts;
using NHibernate.Search.Mapping.Definition;

namespace NHibernate.Search.Fluent.Mapping
{
	using Type = System.Type;

	public interface IDocumentMap : IHasAnalyzer, IValidateMapping
	{
		Type DocumentType { get; }
		string Name { get; set; }
		MemberInfo IdProperty { get; set; }
		IDictionary<ICustomAttributeProvider, float> BoostValues { get; }
		IDictionary<ICustomAttributeProvider, Type> Analyzers { get; }
		IList<IClassBridgeDefinition> ClassBridges { get; }
		IDictionary<MemberInfo, IList<IFieldDefinition>> FieldMappings { get; }
		IDictionary<MemberInfo, IIndexedEmbeddedDefinition> EmbeddedDefs { get; }
		IDictionary<MemberInfo, IFieldBridgeDefinition> FieldBridges { get; }
	}

	public abstract class DocumentMap<T> : IDocumentMap
	{
		private readonly IDictionary<ICustomAttributeProvider, float> boostValues;
		private readonly IDictionary<ICustomAttributeProvider, Type> analyzers;
		private readonly IList<ClassBridgePart<T>> classBridges;
		private readonly IDictionary<MemberInfo, IList<FieldMappingPart>> fieldMappings;
		private readonly IDictionary<MemberInfo, EmbeddedMappingPart> embeddedMappings;
		private readonly IDictionary<MemberInfo, FieldBridgePart> fieldBridges;

		protected DocumentMap()
		{
			Name(typeof (T).Name);

			boostValues = new Dictionary<ICustomAttributeProvider, float>();
			analyzers = new Dictionary<ICustomAttributeProvider, Type>();
			classBridges = new List<ClassBridgePart<T>>();
			fieldMappings = new Dictionary<MemberInfo, IList<FieldMappingPart>>();
			embeddedMappings = new Dictionary<MemberInfo, EmbeddedMappingPart>();
			fieldBridges = new Dictionary<MemberInfo, FieldBridgePart>();
		}

		Type IDocumentMap.DocumentType
		{
			get { return typeof (T); }
		}

		string IDocumentMap.Name { get; set; }
		MemberInfo IDocumentMap.IdProperty { get; set; }

		IDictionary<ICustomAttributeProvider, float> IDocumentMap.BoostValues
		{
			get { return boostValues; }
		}

		IDictionary<ICustomAttributeProvider, Type> IDocumentMap.Analyzers
		{
			get { return analyzers; }
		}

		IList<IClassBridgeDefinition> IDocumentMap.ClassBridges
		{
			get { return classBridges.Select(b => b.BridgeDefinition).ToList(); }
		}

		IDictionary<MemberInfo, IList<IFieldDefinition>> IDocumentMap.FieldMappings
		{
			get
			{
				var result = new Dictionary<MemberInfo, IList<IFieldDefinition>>();
				foreach (var fieldMapping in fieldMappings)
				{
					result.Add(fieldMapping.Key, fieldMapping.Value.Select(m => m.FieldDefinition).ToList());
				}
				return result;
			}
		}

		IDictionary<MemberInfo, IIndexedEmbeddedDefinition> IDocumentMap.EmbeddedDefs
		{
			get
			{
				var result = new Dictionary<MemberInfo, IIndexedEmbeddedDefinition>();
				foreach(var embeddedMap in embeddedMappings)
				{
					result.Add(embeddedMap.Key, embeddedMap.Value.EmbeddedDefinition);
				}
				return result;
			}
		}

		IDictionary<MemberInfo, IFieldBridgeDefinition> IDocumentMap.FieldBridges
		{
			get 
			{ 
				var result = new Dictionary<MemberInfo, IFieldBridgeDefinition>();
				foreach (var fieldBridgePart in fieldBridges)
				{
					result.Add(fieldBridgePart.Key, fieldBridgePart.Value.BridgeDef);
				}
				return result;
			}
		}

		Type IHasAnalyzer.AnalyzerType
		{
			get
			{
				Type type;
				analyzers.TryGetValue(typeof (T), out type);
				return type;
			}
			set { analyzers[typeof (T)] = value; }
		}

		/// <summary>
		/// Defines the Lucene.NET Index Name.
		/// </summary>
		/// <param name="indexName"></param>
		protected void Name(string indexName)
		{
			(this as IDocumentMap).Name = indexName;
		}

		/// <summary>
		/// Defines the DocumentId Mapping.
		/// </summary>
		/// <param name="property"></param>
		/// <returns></returns>
		protected void Id(Expression<Func<T, object>> property)
		{
			if ((this as IDocumentMap).IdProperty != null)
				throw new FluentMappingException("Id can be set only once");
			(this as IDocumentMap).IdProperty = property.ToPropertyInfo();
		}

		/// <summary>
		/// Sets the boost value
		/// </summary>
		/// <param name="boost"></param>
		protected void Boost(float boost)
		{
			boostValues[typeof (T)] = boost;
		}

		/// <summary>
		/// Defines the Default Analyzer for this mapped class.
		/// </summary>
		/// <returns></returns>
		protected AnalyzerPart<DocumentMap<T>> Analyzer()
		{
			return new AnalyzerPart<DocumentMap<T>>(this);
		}

		/// <summary>
		/// Sets the Analyzer to the given Analyzer type.
		/// Shortcut for .Analyzer().Custom&lt;TAnalyzer&gt;
		/// </summary>
		/// <typeparam name="TAnalyzer"></typeparam>
		/// <returns></returns>
		protected DocumentMap<T> Analyzer<TAnalyzer>() where TAnalyzer : Analyzer, new()
		{
			return Analyzer().Custom<TAnalyzer>();
		}

		/// <summary>
		/// Defining a class bridge
		/// </summary>
		/// <typeparam name="TBridge">Bridge type</typeparam>
		/// <returns></returns>
		protected ClassBridgePart<T> ClassBridge<TBridge>() where TBridge : IFieldBridge
		{
			var newPart = new ClassBridgePart<T>(typeof(TBridge));
			classBridges.Add(newPart);
			return newPart;
		}

		/// <summary>
		/// Map a Property as a Lucene.NET Field.
		/// </summary>
		/// <param name="property"></param>
		/// <returns></returns>
		protected FieldMappingPart Map(Expression<Func<T, object>> property)
		{
			var propertyInfo = property.ToPropertyInfo();
			var field = new FieldMappingPart(this, propertyInfo);
			IList<FieldMappingPart> mappings;
			if (fieldMappings.TryGetValue(propertyInfo, out mappings))
				mappings.Add(field);
			else
				fieldMappings.Add(propertyInfo, new List<FieldMappingPart>(new []{field}));
			return field;
		}

		protected EmbeddedMappingPart Embedded(Expression<Func<T, object>> property)
		{
			var part = new EmbeddedMappingPart();
			embeddedMappings.Add(property.ToPropertyInfo(), part);
			return part;
		}
		
		/// <summary>
		/// Defining a bridge for a field
		/// </summary>
		/// <param name="property">field to attach the bridge</param>
		/// <returns></returns>
		protected FieldBridgePart FieldBridge(Expression<Func<T, object>> property)
		{
			var part = new FieldBridgePart();
			fieldBridges.Add(property.ToPropertyInfo(), part);
			return part;
		}

		public void AssertIsValid()
		{
			var documentMap = this as IDocumentMap;
			if (string.IsNullOrEmpty(documentMap.Name))
				throw new DocumentMappingException("Index name cannot be null or empty");

			if (documentMap.IdProperty == null)
				throw new DocumentMappingException("Document Id cannot be null. Document type " + documentMap.DocumentType);
		}
	}
}