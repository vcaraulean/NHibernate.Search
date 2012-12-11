using System;
using System.Reflection;

namespace NHibernate.Search.Mapping.AttributeBased
{
    public class AttributeUtil
    {
    	public static T GetAttribute<T>(ICustomAttributeProvider member) where T : Attribute
        {
            object[] objects = member.GetCustomAttributes(typeof(T), true);
            if (objects.Length == 0)
            {
                return null;
            }

            return (T) objects[0];
        }

        public static bool HasAttribute<T>(ICustomAttributeProvider member) where T : Attribute
        {
            return member.IsDefined(typeof(T), true);
        }

        public static T[] GetAttributes<T>(ICustomAttributeProvider member)
            where T : class
        {
            return GetAttributes<T>(member, true);
        }

        public static T[] GetAttributes<T>(ICustomAttributeProvider member, bool inherit)
            where T : class
        {
            return (T[])member.GetCustomAttributes(typeof(T), inherit);
        }
    }
}
