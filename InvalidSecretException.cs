/*
 *  Copyright © 2013-2015, Andrew Nagle.
 *  All rights reserved.
 *
 *  This file is part of OracleHack.
 *
 *  OracleHack is free software: you can redistribute it and/or modify
 *  it under the terms of the GNU Lesser General Public License as
 *  published by the Free Software Foundation, either version 3 of the
 *  License, or (at your option) any later version.
 *
 *  OracleHack is distributed in the hope that it will be useful,
 *  but WITHOUT ANY WARRANTY; without even the implied warranty of
 *  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 *  GNU Lesser General Public License for more details.
 *
 *  You should have received a copy of the GNU Lesser General Public
 *  License along with OracleHack. If not, see <http://www.gnu.org/licenses/>.
 */

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
