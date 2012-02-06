using NHibernate.Search.Fluent.Mapping;

namespace NHibernate.Search.Fluent.Tests.Integration
{
	public class DomainSearchMapping : FluentSearchMapping
	{
		protected override void Configure()
		{
			Add<AddressSearchMap>();
			Add<CountrySearchMap>();
		}
	}

	public class AddressSearchMap : DocumentMap<Address>
	{
		public AddressSearchMap()
		{
			Id(x => x.Id);
			Map(x => x.AddressLine);
			Embedded(x => x.Country).TargetElement(typeof(Country));
		}
	}

	public class CountrySearchMap : DocumentMap<Country>
	{
		public CountrySearchMap()
		{
			Id(x => x.Id);
			Map(x => x.Name)
				.Index().Tokenized()
				.Store().Yes();
		}
	}
}