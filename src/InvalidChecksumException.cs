/*
 *  Copyright © 2013-2018, Amy Nagle.
 *  All rights reserved.
 *
 *  This file is part of ZoraSharp.
 *
 *  ZoraSharp is free software: you can redistribute it and/or modify
 *  it under the terms of the GNU Lesser General Public License as
 *  published by the Free Software Foundation, either version 3 of the
 *  License, or (at your option) any later version.
 *
 *  ZoraSharp is distributed in the hope that it will be useful,
 *  but WITHOUT ANY WARRANTY; without even the implied warranty of
 *  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 *  GNU Lesser General Public License for more details.
 *
 *  You should have received a copy of the GNU Lesser General Public
 *  License along with ZoraSharp. If not, see <http://www.gnu.org/licenses/>.
 */

using System;
using System.Runtime.Serialization;

namespace Zyrenth.Zora
{
	/// <summary>
	/// Represents the exception that is thrown when a secret is invalid.
	/// </summary>
	[Serializable]
	public class InvalidChecksumException : SecretException
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="InvalidChecksumException"/> class.
		/// </summary>
		public InvalidChecksumException()
		{
			// Add implementation.
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="InvalidChecksumException"/> class
		/// with a specified error message.
		/// </summary>
		/// <param name="message">The message that describes the error.</param>
		public InvalidChecksumException(string message)
			: base(message)
		{
			// Add implementation.
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="InvalidChecksumException"/> class
		/// with a specified error message and a reference to the inner exception that is
		/// the cause of this exception.
		/// </summary>
		/// <param name="message">The error message that explains the reason for the exception.</param>
		/// <param name="innerException">
		/// The exception that is the cause of the current exception. If the <paramref name="innerException"/>
		/// parameter is not a <b>null</b> reference (<b>Nothing</b> in Visual Basic), the current exception is raised in
		/// a catch block that handles the inner exception.
		/// </param>
		public InvalidChecksumException(string message, Exception innerException)
			: base(message, innerException)
		{
			// Add implementation.
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="InvalidChecksumException"/> class
		/// with serialized data.
		/// </summary>
		/// <param name="info">
		/// The <see cref="T:System.Runtime.Serialization.SerializationInfo" /> that holds the
		/// serialized object data about the exception being thrown.
		/// </param>
		/// <param name="context">
		/// The <see cref="T:System.Runtime.Serialization.StreamingContext" /> that contains
		/// contextual information about the source or destination.
		/// </param>
		protected InvalidChecksumException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
			// Add implementation.
		}
	}

}
