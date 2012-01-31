using System;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;

namespace NHibernate.Search.Engine
{
	public class FilterDef
    {
		public FilterDef()
        {
            Setters = new Dictionary<string, PropertyInfo>();
			
			// Default value - true
			Cache = true;
        }

        #region Property methods

		public MethodInfo KeyMethod { get; set; }

		public MethodInfo FactoryMethod { get; set; }

		public System.Type Impl { get; set; }

		public bool Cache { get; set; }

		public string Name { get; set; }

		public IDictionary<string, PropertyInfo> Setters { get; private set; }

        #endregion

        #region Public methods

        public void Invoke(string parameterName, object filter, object parameterValue)
        {
            if (!Setters.ContainsKey(parameterName))
            {
                throw new NotSupportedException(
                    string.Format(CultureInfo.InvariantCulture, "No property {0} found in {1}", parameterName,
                                  Impl != null ? Impl.Name : "<impl>"));
            }

            Setters[parameterName].SetValue(filter, parameterValue, null);
        }

        #endregion
    }
}