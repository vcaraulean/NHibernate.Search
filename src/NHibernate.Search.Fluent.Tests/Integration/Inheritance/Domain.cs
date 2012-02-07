namespace NHibernate.Search.Fluent.Tests.Integration.Inheritance
{
	public abstract class Document
	{
		public virtual int Id { get; set; }
		public virtual string Name { get; set; }
	}

	public class OrderDocument : Document
	{
		public virtual string OrderId { get; set; }
	}

	public class InvoiceDocument : Document
	{
		public virtual string InvoiceId { get; set; }
	}
}