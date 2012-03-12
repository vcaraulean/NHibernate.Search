using Lucene.Net.Documents;
using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;
using NHibernate.Search.Bridge;
using NHibernate.Search.Fluent.Mapping;

namespace NHibernate.Search.Fluent.Tests.Integration.Bridge
{
	public class Customer
	{
		public virtual int Id { get; set; }
		public virtual string Phone { get; set; } 
	}

	public class CustomerSearchMap : DocumentMap<Customer>
	{
		public CustomerSearchMap()
		{
			Id(x => x.Id);

			Map(c => c.Phone)
				.Store().Yes()
				.Index().UnTokenized();

			FieldBridge(x => x.Phone).Custom<RemoveSpacesInStringBridge>();
		}
	}

	public class SearchMappings :  FluentSearchMapping
	{
		protected override void Configure()
		{
			Add<CustomerSearchMap>();
		}
	}

	public class RemoveSpacesInStringBridge : IFieldBridge
	{
		public void Set(string name, object value, Document document, Field.Store store, Field.Index index, float? boost)
		{
			var str = (string) value;
			str = str.Replace(" ", "");
			document.Add(new Field(name, str, store, index));
		}
	}

	public class CustomerNHMap : ClassMapping<Customer>
	{
		public CustomerNHMap()
		{
			Id(x => x.Id, mapper => mapper.Generator(Generators.Identity));
			Property(x => x.Phone);
		}
	}
}