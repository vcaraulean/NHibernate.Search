using NHibernate.Search.Fluent.Mapping;

namespace NHibernate.Search.Fluent.Tests.Integration.Inheritance
{
	public class SearchMappings : FluentSearchMapping
	{
		protected override void Configure()
		{
			Add<OrderDocumentSearchMap>();
			Add<InvoiceDocumentSearchMap>();
			Add<ReferenceSearchMap>();
		}
	}

	public abstract class DocumentSearchMap<TDocument> : DocumentMap<TDocument>
		where TDocument : Document
	{
		public DocumentSearchMap()
		{
			Id(x => x.Id);
			Map(x => x.Name);
			Embedded(x => x.References);
		}
	}

	public class OrderDocumentSearchMap : DocumentSearchMap<OrderDocument>
	{
		public OrderDocumentSearchMap()
		{
			Map(x => x.OrderId);
		}
	}

	public class InvoiceDocumentSearchMap : DocumentSearchMap<InvoiceDocument>
	{
		public InvoiceDocumentSearchMap()
		{
			Map(x => x.InvoiceId);
		}
	}

	public class ReferenceSearchMap : DocumentMap<Reference>
	{
		public ReferenceSearchMap()
		{
			Id(x => x.Id);
			Map(x => x.Description);
		}
	}
}