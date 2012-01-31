using System.Linq;
using NHibernate.Search.Attributes;
using NHibernate.Search.Tests.Bridge;
using NUnit.Framework;

namespace NHibernate.Search.Tests.Mapping.AttributeBased.MappingDefinition
{
	[TestFixture]
	public class ClassBridgesTest : AttributedMappingDefinitionTestBase
	{
		[Test]
		public void Should_get_all_class_bridges()
		{
			var bridges = mappingDefinition.ClassBridges(typeof(Departments));
			Assert.AreEqual(2, bridges.Count);

			var first = bridges.Single(b => b.Name == "branchnetwork");
			Assert.IsNotNull(first);
			Assert.AreEqual(typeof(CatDeptsFieldsClassBridge), first.Impl);
			Assert.AreEqual(Index.Tokenized, first.Index);
			Assert.AreEqual(Attributes.Store.Yes, first.Store);

			var second = bridges.Single(b => b.Name == "equiptype");
			Assert.IsNotNull(second);
			Assert.AreEqual(typeof(EquipmentType), second.Impl);
			Assert.AreEqual(Index.Tokenized, second.Index);
			Assert.AreEqual(Attributes.Store.Yes, second.Store);
		}
	}
}