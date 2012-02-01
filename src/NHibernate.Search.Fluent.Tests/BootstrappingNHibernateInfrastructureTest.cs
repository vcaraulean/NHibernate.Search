using NHibernate.Mapping.ByCode.Conformist;
using NUnit.Framework;

namespace NHibernate.Search.Fluent.Tests
{
	public class Entity
	{
		public virtual int Id { get; set; }
		public virtual string Name { get; set; }
	}

	public class EntityMap : ClassMapping<Entity>
	{
		public EntityMap()
		{
			Id(x => x.Id);
			Property(x => x.Name);
		}
	}

	public class BootstrappingNHibernateInfrastructureTest : FluentSearchTestBase
	{
		[Test]
		public void Entity_should_be_saved_then_queried()
		{
			var entity = new Entity {Name = "first"};
			Session.Save(entity);
			Session.Flush();
			Session.Clear();

			var retrievedEntity = Session.QueryOver<Entity>().SingleOrDefault();
			Assert.AreEqual(1, retrievedEntity.Id);
			Assert.AreEqual("first", retrievedEntity.Name);
		}

	}
}