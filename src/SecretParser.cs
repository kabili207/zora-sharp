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
			{ @"\{?spade\}?", "♠" }, { @"\{?heart\}?", "♥" }, { @"\{?diamond\}?", "♦" },
			{ @"\{?club\}?", "♣" }, { @"\{?circle\}?", "●" }, { @"\{?triangle\}?", "▲" },
			{ @"\{?square\}?", "■" }, { @"\{?up\}?", "↑" }, { @"\{?down\}?", "↓" },
			{ @"\{?left\}?", "←" }, { @"\{?right\}?", "→" }, { "<", "(" }, { ">", ")" },
			{ @"\s+", "" },
		};

		/// <summary>
		/// Converts a secret string into a byte array.
		/// </summary>
		/// <returns>The secret</returns>
		/// <param name="secret">Secret.</param>
		/// <exception cref="InvalidSecretException">
		/// The <paramref name="secret"/> contains invalid symbols.
		/// </exception>
		/// <remarks>
		/// This method is fairly flexible in what it will accept.
		/// <list type="bullet">
		/// <item><description><c>6●sW↑</c></description></item>
		/// <item><description><c>6 circle s W up</c></description></item>
		/// <item><description><c>6{circle}sW{up}</c></description></item>
		/// <item><description><c>6cIrClesW↑</c></description></item>
		/// </list>
		/// The lack of vowels in the normal secret keyboard is what makes the last
		/// format possible. Whitespace in the secret is ignored so long as it doesn't separate
		/// any keywords. Surrounding the keywords in curly braces is not required; they
		/// are supported in order to maintain compatibility with how many users share
		/// secrets online.
		/// </remarks>
		/// <example>
		/// <code language="C#">
		/// string gameSecret = "H~2:@ left 2 diamond yq GB3 circle ( 6 heart ? up 6";
		/// byte[] rawGameSecret = SecretParser.ParseSecret(gameSecret);
		/// </code>
		/// </example>
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
		/// <exception cref="InvalidSecretException">
		/// The <paramref name="data"/> contains values that cannot be used in a secret.
		/// </exception>
		/// <example>
		/// <code language="C#">
		/// byte[] rawSecret = new byte[]
		/// {
		///      4, 37, 51, 36, 63,
		///     61, 51, 10, 44, 39,
		///      3,  0, 52, 21, 48,
		///     55,  9, 45, 59, 55
		/// };
		/// string secret = SecretParser.CreateString(rawSecret);
		/// // H~2:@ ←2♦yq GB3●( 6♥?↑6
		/// </code>
		/// </example>
		public static string CreateString(byte[] data)
		{
			StringBuilder sBuilder = new StringBuilder();
			for (int i = 0; i < data.Length; ++i)
			{
				if(data[i] < 0 || data[i] > 63)
					throw new InvalidSecretException("Secret contains invalid values");

				sBuilder.Append(Symbols[data[i]]);
				if (i % 5 == 4)
					sBuilder.Append(" ");
			}
			return sBuilder.ToString().Trim();
		}
	}
}
