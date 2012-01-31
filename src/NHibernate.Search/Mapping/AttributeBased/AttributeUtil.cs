using System;
using System.Collections.Generic;
using System.Reflection;

using NHibernate.Search.Attributes;

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

        public static FieldAttribute[] GetFields(MemberInfo member)
        {
            FieldAttribute[] attribs = GetAttributes<FieldAttribute>(member);
            if (attribs != null)
            {
                foreach (FieldAttribute attribute in attribs)
                {
                    attribute.Name = attribute.Name ?? member.Name;
                }
            }

            return attribs;
        }

        public static FieldBridgeAttribute GetFieldBridge(ICustomAttributeProvider member)
        {
            FieldBridgeAttribute fieldBridge = GetAttribute<FieldBridgeAttribute>(member);
            if (fieldBridge == null)
            {
                return null;
            }

            bool classBridges = GetAttributes<ClassBridgeAttribute>(member) != null;

            // Ok, get all the parameters
            IList<ParameterAttribute> parameters = GetParameters(member);
            if (parameters != null)
            {
                foreach (ParameterAttribute parameter in parameters)
                {
                    // Ok, it's ours if there are no class bridges or no owner for the parameter
                    if (!classBridges || string.IsNullOrEmpty(parameter.Owner))
                    {
                        fieldBridge.Parameters.Add(parameter.Name, parameter.Value);
                    }
                }
            }

            return fieldBridge;
        }

        public static IList<ParameterAttribute> GetParameters(ICustomAttributeProvider member)
        {
            return GetAttributes<ParameterAttribute>(member);
        }
    }
}
