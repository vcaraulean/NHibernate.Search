using System;
using NHibernate.Search.Mapping.Definition;

namespace NHibernate.Search.Attributes
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class IndexedAttribute : Attribute, IIndexedDefinition
    {
        private string index = string.Empty;

        public string Index
        {
            get { return index; }
            set { index = value; }
        }
    }
}