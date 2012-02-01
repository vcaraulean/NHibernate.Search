using NHibernate.Search.Mapping;
using NHibernate.Search.Mapping.AttributeBased;
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
		}
	}
}