using NHibernate.Search.Mapping.AttributeBased;
using NHibernate.Search.Mapping.Model;
using NUnit.Framework;

namespace NHibernate.Search.Tests.Mapping.AttributeBased
{
	[TestFixture]
	public class AttributedMappingDefinitionTestBase
	{
		protected ISearchMappingDefinition mappingDefinition;

		[SetUp]
		public void SetUp()
		{
			mappingDefinition = new AttributedMappingDefinition();
			When();
		}

		protected virtual void When() { }
	}
}