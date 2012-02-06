using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;

namespace NHibernate.Search.Fluent.Tests.Integration
{
	public class CountryMap : ClassMapping<Country>
	{
		public CountryMap()
		{
			Id(x => x.Id, mapper => mapper.Generator(Generators.Identity));
			Property(x => x.Name);
		}
	}

	public class AddressMap : ClassMapping<Address>
	{
		public AddressMap()
		{
			Id(x => x.Id, mapper => mapper.Generator(Generators.Identity));
			Property(x => x.AddressLine);

			ManyToOne(x => x.Country, mapper =>
			{
				mapper.Column("country_id");
				mapper.Cascade(Cascade.All);
			});
		}
	}
}