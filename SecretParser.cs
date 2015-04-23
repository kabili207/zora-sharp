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
using System.Text;
using System.Text.RegularExpressions;

namespace Zyrenth.OracleHack
{
	/// <summary>
	/// A convenience class used to convert raw secret data into the display format
	/// used by the games.
	/// </summary>
	public static class SecretParser
	{
		private static readonly char[] Symbols = 
		{
			'B', 'D', 'F', 'G', 'H', 'J', 'L', 'M', '♠', '♥', '♦', '♣', '#',
			'N', 'Q', 'R', 'S', 'T', 'W', 'Y', '!', '●', '▲', '■', '+', '-',
			'b', 'd', 'f', 'g', 'h', 'j',      'm', '$', '*', '/', ':', '~',
			'n', 'q', 'r', 's', 't', 'w', 'y', '?', '%', '&', '(', '=', ')',
			'2', '3', '4', '5', '6', '7', '8', '9', '↑', '↓', '←', '→', '@'
		};

		private static readonly Dictionary<string, string> SymbolRegexes =
			new Dictionary<string, string> {
			{ @"\{?spade\}?", "♠"}, { @"\{?heart\}?", "♥" }, { @"\{?diamond\}?", "♦" },
			{ @"\{?club\}?", "♣" }, { @"\{?circle\}?", "●"}, { @"\{?triangle\}?", "▲" },
			{ @"\{?square\}?", "■" }, { @"\{?up\}?", "↑" }, { @"\{?down\}?", "↓" },
			{ @"\{?left\}?", "←" }, { @"\{?right\}?", "→" }, { "<", "(" }, { ">", ")" },
			{ @"\s+", ""},
		};

		/// <summary>
		/// Converts a secret string into a byte array.
		/// </summary>
		/// <returns>The secret</returns>
		/// <param name="secret">Secret.</param>
		/// <remarks>
		/// This method is fairly flexible in what it will accept.
		/// <list type="bullet">
		/// <item><description><c>→N♥Nh</c></description></item>
		/// <item><description><c>right N heart N h</c></description></item>
		/// <item><description><c>{right}N{heart}Nh</c></description></item>
		/// <item><description><c>RigHtN ♥N  h</c></description></item>
		/// </list>
		/// </remarks>
		public static byte[] ParseSecret(string secret)
		{
			foreach (var kvp in SymbolRegexes)
			{
				secret = Regex.Replace(secret, kvp.Key, kvp.Value, RegexOptions.IgnoreCase);
			}
			byte[] data = new byte[secret.Length];

			int symbol = 0;
			for (int i = 0; i < secret.Length; ++i)
			{
				symbol = Array.IndexOf(Symbols, secret[i]);
				if (symbol < 0 || symbol > 63)
					throw new InvalidSecretException("Secret contains invalid symbols");

				data[i] = (byte)symbol;
			}

			return data;
		}

		/// <summary>
		/// Creates a string representation of a secret byte array.
		/// </summary>
		/// <param name="data">The secret data</param>
		/// <returns>A representation of the secret data</returns>
		/// <remarks>This method always returns the secret formatted as <c>→N♥Nh</c></remarks>
		public static string CreateString(byte[] data)
		{
			StringBuilder sBuilder = new StringBuilder();
			for (int i = 0; i < data.Length; ++i)
			{
				sBuilder.Append(Symbols[data[i]]);
				if (i % 5 == 4)
					sBuilder.Append(" ");
			}
			return sBuilder.ToString().Trim();
		}
	}
}
