using System;
using System.Runtime.Serialization;

namespace NHibernate.Search.Fluent.Exceptions
{
	public class FluentMappingException : MappingException
	{
		public FluentMappingException(string message) : base(message)
		{
		}

		public FluentMappingException(Exception innerException) : base(innerException)
		{
		}

		public FluentMappingException(string message, Exception innerException) : base(message, innerException)
		{
		}

		protected FluentMappingException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}
	}
}