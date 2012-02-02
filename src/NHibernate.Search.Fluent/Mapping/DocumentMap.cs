namespace NHibernate.Search.Fluent.Mapping
{
	public interface IDocumentMap
	{
		System.Type DocumentType { get; }
		string Name {get;}
	}

	public abstract class DocumentMap<T> : IDocumentMap
	{
		private string name;
		
		protected DocumentMap()
		{
			name = typeof (T).Name;
		}

		protected void Name(string indexName)
		{
			name = indexName;
		}

		System.Type IDocumentMap.DocumentType
		{
			get { return typeof (T); }
		}

		string IDocumentMap.Name
		{
			get { return name; }
		}

		
	}
}