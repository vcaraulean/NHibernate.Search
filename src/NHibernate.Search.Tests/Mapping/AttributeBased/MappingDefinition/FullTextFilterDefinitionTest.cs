using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using NHibernate.Search.Attributes;
using NHibernate.Search.Engine;
using NHibernate.Search.Impl;
using NHibernate.Search.Mapping.Model;
using NUnit.Framework;

namespace NHibernate.Search.Tests.Mapping.AttributeBased.MappingDefinition
{
	[TestFixture]
	public class FullTextFilterDefinitionTest : AttributedMappingDefinitionTestBase
	{
		[Test]
		public void Should_extract_correct_number_of_filter_definitions()
		{
			var filters = mappingDefinition.FullTextFilters(typeof(Doc));
			Assert.AreEqual(2, filters.Count());
		}

		[Test]
		public void Should_extract_correct_name_and_filter_type_and_cache()
		{
			var filters = mappingDefinition.FullTextFilters(typeof(Doc));

			var first = filters.SingleOrDefault(f => f.Name == "f1");
			Assert.IsNotNull(first);
			Assert.AreEqual(typeof(Filter1), first.Impl);
			Assert.IsTrue(first.Cache);
			
			var second = filters.SingleOrDefault(f => f.Name == "f2"); ;
			Assert.IsNotNull(second);
			Assert.AreEqual(typeof(Filter2), second.Impl);
			Assert.IsFalse(second.Cache);
		}

		[Test]
		public void Should_throw_if_cannot_instantiate_filter_impl()
		{
			Assert.Throws<SearchException>(
				() => mappingDefinition.FullTextFilters(typeof(DocWithBadFilter)),
				"Unable to create Filter class: " + typeof(Filter3).FullName);
		}

		[Test]
		public void Should_throw_when_a_filter_has_multiple_methods_marked_as_factory()
		{
			Assert.Throws<SearchException>(() => mappingDefinition.FullTextFilters(typeof(DocWithMultipleFactoriesInFilter)));
		}

		[Test]
		public void Should_extract_factory_method_from_filter()
		{
			var def = mappingDefinition.FullTextFilters(typeof (DocWithFilterAndFactory)).First();
			Assert.AreEqual("MethodF", def.FactoryMethod.Name);
		}

		[Test]
		public void Should_throw_when_a_filter_has_multple_methods_marked_as_keys()
		{
			Assert.Throws<SearchException>(() => mappingDefinition.FullTextFilters(typeof(DocWithMultipleKeysInFilter)));
		}

		[Test]
		public void Should_extract_key_method_from_filter()
		{
			var def = mappingDefinition.FullTextFilters(typeof(DocWithFilterAndFactory)).First();
			Assert.AreEqual("MethodKey", def.KeyMethod.Name);
		}

		[Test]
		public void Should_extract_filter_parameters()
		{
			var filter = mappingDefinition.FullTextFilters(typeof (DocWithFilterWithParameter)).Single();

			Assert.IsTrue(filter.Setters.ContainsKey("Parameter1"));
			Assert.IsTrue(filter.Setters.ContainsKey("Parameter2"));

			Assert.AreEqual("Parameter1", filter.Setters["Parameter1"].Name);
		}

		[FullTextFilterDef("f1", typeof(Filter1))]
		[FullTextFilterDef("f2", typeof(Filter2), Cache = false)]
		public class Doc
		{
			public string SomeProperty { get; set; }
		}

		public class Filter1 {}
		public class Filter2 {}


		[FullTextFilterDef("bad", typeof(Filter3))]
		public class DocWithBadFilter{}
		public class Filter3
		{
			public Filter3(string val)
			{
			}
		}

		[FullTextFilterDef("bad", typeof(BadFilterWithMultipleFactories))]
		public class DocWithMultipleFactoriesInFilter
		{
		}

		public class BadFilterWithMultipleFactories
		{
			[FactoryAttribute]
			public void Method1(){}
			[FactoryAttribute]
			public void Method2(){}
		}

		[FullTextFilterDef("ok", typeof(FilterWithFactoryAndKey))]
		public class DocWithFilterAndFactory{}

		public class FilterWithFactoryAndKey
		{
			[FactoryAttribute]
			public void MethodF() { }

			[KeyAttribute]
			public void MethodKey(){}
		}

		[FullTextFilterDef("bad", typeof(BadFilterWithMultipleKeys))]
		class DocWithMultipleKeysInFilter
		{
		}
		public class BadFilterWithMultipleKeys
		{
			[KeyAttribute]
			public void Method1() { }
			[KeyAttribute]
			public void Method2() { }
		}

		[FullTextFilterDef("ok", typeof(FilterWithParameters))]
		class DocWithFilterWithParameter{}

		class FilterWithParameters
		{
			[FilterParameter]
			public int Parameter1 { get; set; }

			[FilterParameter]
			public string Parameter2 { get; set; }
		}
	}
}