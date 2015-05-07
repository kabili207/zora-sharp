/*
 *  Copyright © 2011-2015, Andrew Nagle.
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
using System.Text;
using System.IO;
using System.Runtime.InteropServices;
using System.Reflection;

namespace Zyrenth.OracleHack
{
	public static class Extensions
	{
		internal static string Reverse(this string value)
		{
			return new string(Enumerable.Reverse(value).ToArray());
		}

		internal static string ReversedSubstring(this string value, int start, int length)
		{
			return new string(Enumerable.Reverse(value.Substring(start, length)).ToArray());
		}

		public static T ReadValue<T>(this IDictionary<string, object> dict, string key) where T : IConvertible
		{
			Type type = typeof(T);
			T val = default(T);

			if (dict.ContainsKey(key))
			{
				object obj = dict[key];
				if (type.IsEnum)
				{
					// The T constraints on our method conflict with those on Enum.TryParse<T>
					// so we have to use some black magic instead.
					MethodInfo method = typeof(Enum).GetMethods().First(x => x.Name.StartsWith("TryParse") &&
						x.GetParameters().Length == 2);

					method = method.MakeGenericMethod(type);

					var args = new object[] { ReadValue<string>(dict, key), default(T) };
					method.Invoke(null, args);

					val = (T)args[1];
				}
				else
				{
					val = (T)Convert.ChangeType(obj, type);
				}
			}

			return val;
		}

	}
}
