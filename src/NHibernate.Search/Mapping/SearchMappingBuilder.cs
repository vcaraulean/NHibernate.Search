using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
using Iesi.Collections.Generic;
using Lucene.Net.Analysis;
using NHibernate.Properties;
using NHibernate.Search.Bridge;
using NHibernate.Search.Impl;
using NHibernate.Search.Mapping.Definition;

namespace NHibernate.Search.Mapping
{
	public abstract class SearchMappingBuilder
	{
		private static readonly IInternalLogger logger = LoggerProvider.LoggerFor(typeof(SearchMappingBuilder));

		private readonly ISearchMappingDefinition mappingDefinition;

		protected SearchMappingBuilder(ISearchMappingDefinition mappingDefinition)
		{
			this.mappingDefinition = mappingDefinition;
		}

		private int level;
		private int maxLevel = int.MaxValue;

		#region BuildContext class

		public class BuildContext
		{
			public BuildContext()
			{
				Processed = new HashedSet<System.Type>();
			}

			public DocumentMapping Root { get; set; }
			public Iesi.Collections.Generic.ISet<System.Type> Processed { get; private set; }
		}

		#endregion

		public DocumentMapping Build(System.Type type)
		{
			var documentMapping = new DocumentMapping(type)
			{
				Boost = GetBoost(type),
				IndexName = mappingDefinition.Indexed(type).Index
			};

			var context = new BuildContext
			{
				Root = documentMapping,
				Processed = { type }
			};

			BuildClass(documentMapping, true, string.Empty, context);
			BuildFilterDefinitions(documentMapping);

			return documentMapping;
		}

		private void BuildFilterDefinitions(DocumentMapping classMapping)
		{
			foreach (var filterDef in mappingDefinition.FullTextFilters(classMapping.MappedClass))
			{
				classMapping.FullTextFilterDefinitions.Add(filterDef);
			}
		}

		private void BuildClass(DocumentMapping documentMapping, bool isRoot, string path, BuildContext context)
		{
			IList<System.Type> hierarchy = new List<System.Type>();
			var currClass = documentMapping.MappedClass;

			do
			{
				hierarchy.Add(currClass);
				currClass = currClass.BaseType;
				// NB Java stops at null we stop at object otherwise we process the class twice
				// We also need a null test for things like ISet which have no base class/interface
			} while (currClass != null && currClass != typeof(object));

			for (int index = hierarchy.Count - 1; index >= 0; index--)
			{
				currClass = hierarchy[index];
				/**
				 * Override the default analyzer for the properties if the class hold one
				 * That's the reason we go down the hierarchy
				 */

				// NB Must cast here as we want to look at the type's metadata
				var localAnalyzer = GetAnalyzer(currClass);
				var analyzer = documentMapping.Analyzer ?? localAnalyzer;

				// Check for any ClassBridges
				var classBridges = mappingDefinition.ClassBridges(currClass);
				GetClassBridgeParameters(currClass, classBridges);

				// Now we can process the class bridges
				foreach (var classBridge in classBridges)
				{
					var bridge = BuildClassBridge(classBridge, analyzer);
					documentMapping.ClassBridges.Add(bridge);
				}

				// NB As we are walking the hierarchy only retrieve items at this level
				var propertyInfos = currClass.GetProperties(
					BindingFlags.DeclaredOnly | BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance
					);
				foreach (var propertyInfo in propertyInfos)
				{
					BuildProperty(documentMapping, propertyInfo, analyzer, isRoot, path, context);
				}

				var fields = currClass.GetFields(
					BindingFlags.DeclaredOnly | BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance
					);
				foreach (var fieldInfo in fields)
				{
					BuildProperty(documentMapping, fieldInfo, analyzer, isRoot, path, context);
				}
			}
		}

		private void BuildProperty(
			DocumentMapping documentMapping, MemberInfo member, Analyzer parentAnalyzer,
			bool isRoot, string path, BuildContext context
			)
		{
			IFieldBridge bridge = null;

			var analyzer = GetAnalyzer(member) ?? parentAnalyzer;
			var boost = GetBoost(member);

			var getter = GetGetterFast(documentMapping.MappedClass, member);

			var documentId = mappingDefinition.DocumentId(member);
			if (documentId != null)
			{
				string documentIdName = documentId.Name ?? member.Name;
				bridge = GetFieldBridge(member);

				if (isRoot)
				{
					if (!(bridge is ITwoWayFieldBridge))
					{
						throw new SearchException("Bridge for document id does not implement TwoWayFieldBridge: " + member.Name);
					}

					documentMapping.DocumentId = new DocumentIdMapping(
						documentIdName, member.Name, (ITwoWayFieldBridge)bridge, getter
						) { Boost = boost };
				}
				else
				{
					// Components should index their document id
					documentMapping.Fields.Add(new FieldMapping(
					                           	GetFieldName(member, documentIdName),
					                           	bridge, getter)
					{
						Store = Attributes.Store.Yes,
						Index = Attributes.Index.UnTokenized,
						Boost = boost
					});
				}
			}

			var fieldDefinitions = mappingDefinition.FieldDefinitions(member);
			if (fieldDefinitions.Count > 0)
			{
				if (bridge == null)
					bridge = GetFieldBridge(member);

				foreach (var fieldDefinition in fieldDefinitions)
				{
					fieldDefinition.Name = fieldDefinition.Name ?? member.Name;
					var fieldAnalyzer = GetAnalyzerByType(fieldDefinition.Analyzer) ?? analyzer;
					var field = new FieldMapping(
						GetFieldName(member, fieldDefinition.Name),
						bridge, getter
						)
					{
						Store = fieldDefinition.Store,
						Index = fieldDefinition.Index,
						Analyzer = fieldAnalyzer
					};

					documentMapping.Fields.Add(field);
				}
			}

			var embeddedDefinition = mappingDefinition.IndexedEmbedded(member);
			if (embeddedDefinition != null)
			{
				int oldMaxLevel = maxLevel;
				int potentialLevel = embeddedDefinition.Depth + level;
				if (potentialLevel < 0)
				{
					potentialLevel = int.MaxValue;
				}

				maxLevel = potentialLevel > maxLevel ? maxLevel : potentialLevel;
				level++;

				System.Type elementType = embeddedDefinition.TargetElement ?? GetMemberTypeOrGenericArguments(member);

				var localPrefix = embeddedDefinition.Prefix == "." ? member.Name + "." : embeddedDefinition.Prefix;

				if (maxLevel == int.MaxValue && context.Processed.Contains(elementType))
				{
					throw new SearchException(
						string.Format(
							"Circular reference, Duplicate use of {0} in root entity {1}#{2}",
							elementType.FullName,
							context.Root.MappedClass.FullName,
							path + localPrefix));
				}


				if (level <= maxLevel)
				{
					context.Processed.Add(elementType); // push
					var embedded = new EmbeddedMapping(new DocumentMapping(elementType)
					{
						Boost = GetBoost(member),
						Analyzer = GetAnalyzer(member) ?? parentAnalyzer
					}, getter)
					{
						Prefix = localPrefix
					};

					BuildClass(embedded.Class, false, path + localPrefix, context);

					/**
					 * We will only index the "expected" type but that's OK, HQL cannot do downcasting either
					 */
					// ayende: because we have to deal with generic collections here, we aren't 
					// actually using the element type to determine what the value is, since that 
					// was resolved to the element type of the possible collection
					System.Type actualFieldType = GetMemberTypeOrGenericCollectionType(member);
					embedded.IsCollection = typeof(IEnumerable).IsAssignableFrom(actualFieldType);

					documentMapping.Embedded.Add(embedded);
					context.Processed.Remove(actualFieldType); // pop
				}
				else if (logger.IsDebugEnabled)
				{
					logger.Debug("Depth reached, ignoring " + path + localPrefix);
				}

				level--;
				maxLevel = oldMaxLevel; // set back the old max level
			}

			if (mappingDefinition.HasContainedInDefinition(member))
			{
				documentMapping.ContainedIn.Add(new ContainedInMapping(getter));
			}
		}

		/// <summary>
		/// Get the field name out of the member unless overridden by name
		/// </summary>
		/// <param name="member"></param>
		/// <param name="name"></param>
		/// <returns></returns>
		private string GetFieldName(MemberInfo member, string name)
		{
			return !string.IsNullOrEmpty(name) ? name : member.Name;
		}

		// ashmind: this method is a bit too hardcoded, on the other hand it does not make
		// sense to ask IPropertyAccessor to find accessor by name when we already have MemberInfo        
		private IGetter GetGetterFast(System.Type type, MemberInfo member)
		{
			if (member is PropertyInfo)
				return new BasicPropertyAccessor.BasicGetter(type, (PropertyInfo)member, member.Name);

			if (member is FieldInfo)
				return new FieldAccessor.FieldGetter((FieldInfo)member, type, member.Name);

			throw new ArgumentException("Can not get getter for " + member.GetType() + ".", "member");
		}

		private IFieldBridge GetFieldBridge(MemberInfo member)
		{
			var memberType = GetMemberType(member);

			return BridgeFactory.GuessType(
				member.Name, memberType,
				GetFieldBridgeDefinition(member),
				mappingDefinition.DateBridge(member)
				);
		}

		private IFieldBridgeDefinition GetFieldBridgeDefinition(MemberInfo member)
		{
			var fieldBridge = mappingDefinition.FieldBridge(member);
			if (fieldBridge == null)
			{
				return null;
			}

			// Ok, get all the parameters
			var parameters = mappingDefinition.BridgeParameters(member);
			foreach (var parameter in parameters)
			{
				// Ok, it's ours if there are no class bridges or no owner for the parameter
				if (string.IsNullOrEmpty(parameter.Owner))
				{
					fieldBridge.Parameters.Add(parameter.Name, parameter.Value);
				}
			}

			return fieldBridge;
		}

		private ClassBridgeMapping BuildClassBridge(IClassBridgeDefinition ann, Analyzer parentAnalyzer)
		{
			var classAnalyzer = GetAnalyzerByType(ann.Analyzer) ?? parentAnalyzer;
			return new ClassBridgeMapping(ann.Name, BridgeFactory.ExtractType(ann))
			{
				Boost = ann.Boost,
				Analyzer = classAnalyzer,
				Index = ann.Index,
				Store = ann.Store
			};
		}

		private Analyzer GetAnalyzer(MemberInfo member)
		{
			var analyzer = mappingDefinition.Analyzer(member);
			if (analyzer == null)
				return null;

			if (!typeof(Analyzer).IsAssignableFrom(analyzer.Type))
			{
				throw new SearchException("Lucene analyzer not implemented by " + analyzer.Type.FullName);
			}

			return GetAnalyzerByType(analyzer.Type);
		}

		private Analyzer GetAnalyzerByType(System.Type analyzerType)
		{
			if (analyzerType == null)
				return null;

			try
			{
				return (Analyzer)Activator.CreateInstance(analyzerType);
			}
			catch
			{
				// TODO: See if we can get a tigher exception trap here
				throw new SearchException("Failed to instantiate lucene analyzer with type  " + analyzerType.FullName);
			}
		}

		private static System.Type GetMemberTypeOrGenericArguments(MemberInfo member)
		{
			System.Type type = GetMemberType(member);
			if (type.IsGenericType)
			{
				System.Type[] arguments = type.GetGenericArguments();

				// if we have more than one generic arg, we assume that this is a map and return its value
				return arguments[arguments.Length - 1];
			}

			return type;
		}

		private static System.Type GetMemberTypeOrGenericCollectionType(MemberInfo member)
		{
			System.Type type = GetMemberType(member);
			return type.IsGenericType ? type.GetGenericTypeDefinition() : type;
		}

		private static System.Type GetMemberType(MemberInfo member)
		{
			var info = member as PropertyInfo;
			return info != null ? info.PropertyType : ((FieldInfo)member).FieldType;
		}

		private float? GetBoost(ICustomAttributeProvider member)
		{
			if (member == null)
				return null;

			var boost = mappingDefinition.Boost(member);
			if (boost == null)
				return null;

			return boost.Value;
		}

		private void GetClassBridgeParameters(System.Type member, IList<IClassBridgeDefinition> classBridges)
		{
			// This is a bit inefficient, but the loops will be very small
			var parameters = mappingDefinition.BridgeParameters(member);

			// Do it this way around so we can ensure we process all parameters
			foreach (IParameterDefinition parameter in parameters)
			{
				// What we want to ensure is :
				// 1. If there's a field bridge, unnamed parameters belong to it
				// 2. If there's 1 class bridge and no field bridge, unnamed parameters belong to it
				// 3. If there's > 1 class bridge and no field bridge, that's an error - we don't know which class bridge should get them
				if (string.IsNullOrEmpty(parameter.Owner))
				{
					if (classBridges.Count == 1)
					{
						// Case 2
						classBridges[0].Parameters.Add(parameter.Name, parameter.Value);
					}
					else
					{
						// Case 3
						LogParameterError(
							"Parameter needs a name when multiple bridges defined: {0}={1}, parameter={2}",
							member,
							parameter);
					}
				}
				else
				{
					bool found = false;

					// Now see if we can find the owner
					foreach (IClassBridgeDefinition classBridge in classBridges)
					{
						if (classBridge.Name == parameter.Owner)
						{
							classBridge.Parameters.Add(parameter.Name, parameter.Value);
							found = true;
							break;
						}
					}

					// Ok, did we find the appropriate class bridge?
					if (found == false)
					{
						LogParameterError(
							"No matching owner for parameter: {0}={1}, parameter={2}, owner={3}", member, parameter);
					}
				}
			}
		}

		private static void LogParameterError(string message, ICustomAttributeProvider member, IParameterDefinition parameter)
		{
			string type = string.Empty;
			string name = string.Empty;

			if (typeof(System.Type).IsAssignableFrom(member.GetType()))
			{
				type = "class";
				name = ((System.Type)member).FullName;
			}
			else if (typeof(MemberInfo).IsAssignableFrom(member.GetType()))
			{
				type = "member";
				name = ((MemberInfo)member).DeclaringType.FullName + "." + ((MemberInfo)member).DeclaringType.FullName;
			}

			// Now log it
			logger.Error(string.Format(CultureInfo.InvariantCulture, message, type, name, parameter.Name, parameter.Owner));
		}
	}
}