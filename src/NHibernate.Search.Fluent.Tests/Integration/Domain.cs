namespace NHibernate.Search.Fluent.Tests.Integration
{
	public class Address
	{
		public virtual int Id { get; set; }

		public virtual string AddressLine { get; set; }
		public virtual Country Country { get; set; }
	}

	public class Country
	{
		public virtual int Id { get; set; }
		public virtual string Name { get; set; }
	}
}