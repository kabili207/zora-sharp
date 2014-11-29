using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Zyrenth.OracleHack
{
	[Serializable]
	public class InvalidSecretException : Exception, ISerializable
	{
		public InvalidSecretException()
		{
			// Add implementation.
		}
		public InvalidSecretException(string message)
			: base(message)
		{
			// Add implementation.
		}
		public InvalidSecretException(string message, Exception inner)
			: base(message,inner)
		{
			// Add implementation.
		}

		// This constructor is needed for serialization.
		protected InvalidSecretException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
			// Add implementation.
		}
	}

}
