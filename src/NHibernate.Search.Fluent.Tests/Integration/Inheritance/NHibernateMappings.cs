using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;

namespace NHibernate.Search.Fluent.Tests.Integration.Inheritance
{
	public class DocumenNHMap : ClassMapping<Document>
	{
		public DocumenNHMap()
		{
			Id(x => x.Id, mapper => mapper.Generator(Generators.Identity));
			Property(x => x.Name);
		}
	}

	public class OrderDocumentMap : JoinedSubclassMapping<OrderDocument>
	{
		public OrderDocumentMap()
		{
			Property(x => x.OrderId);
		}
	}

	public class InvoiceDocumentMap : JoinedSubclassMapping<InvoiceDocument>
	{
		public InvoiceDocumentMap()
		{
			Property(x => x.InvoiceId);
		}
	}
}