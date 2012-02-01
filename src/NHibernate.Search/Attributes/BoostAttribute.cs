using System;
using NHibernate.Search.Mapping.Definition;

namespace NHibernate.Search.Attributes
{
	/// <summary>
    /// Applies a boost factor on a field or whole entity
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    public class BoostAttribute : Attribute, IBoostDefinition
	{
        private readonly float value;

        public BoostAttribute(float value)
        {
            this.value = value;
        }

        public float Value
        {
            get { return value; }
        }
    }
}