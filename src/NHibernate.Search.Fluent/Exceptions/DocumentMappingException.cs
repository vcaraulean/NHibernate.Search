using System;
using System.Runtime.Serialization;

namespace NHibernate.Search.Fluent.Exceptions
{
	public class DocumentMappingException : Exception
	{
		public DocumentMappingException()
		{
		}

		public DocumentMappingException(string message) : base(message)
		{
		}

		public DocumentMappingException(string message, Exception innerException) : base(message, innerException)
		{
		}

		protected DocumentMappingException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}
	}
}