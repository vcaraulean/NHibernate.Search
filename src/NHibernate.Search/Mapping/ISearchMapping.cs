using System.Collections.Generic;
using NHibernate.Cfg;

namespace NHibernate.Search.Mapping
{
	public interface ISearchMapping
    {
        ICollection<DocumentMapping> Build(Configuration cfg);
    }
}
