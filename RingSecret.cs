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

namespace Zyrenth.OracleHack
{
	/// <summary>
	/// Represents a secret used to transfer rings between games in the Zelda Oracle series.
	/// </summary>
	public class RingSecret : Secret
	{
		long _rings = 0L;

		/// <summary>
		/// Gets the required length of the secret
		/// </summary>
		public override int Length
		{
			get { return 15; }
		}

		/// <summary>
		/// Gets or sets the user's ring collection
		/// </summary>
		public Rings Rings
		{
			get { return (Rings)_rings; }
			set
			{
				_rings = (long)value;
				OnPropertyChanged("Rings");
			}
		}

		/// <summary>
		/// Loads in data from the specified game info
		/// </summary>
		/// <param name="info">The game info</param>
		public override void Load(GameInfo info)
		{
			GameID = info.GameID;
			Rings = info.Rings;
		}

		/// <summary>
		/// Loads in data from the raw secret data provided
		/// </summary>
		/// <param name="secret">The raw secret data</param>
		public override void Load(byte[] secret)
		{
			if (secret == null || secret.Length != Length)
				throw new InvalidSecretException("Secret must contatin exactly 15 bytes");

			byte[] decodedBytes = DecodeBytes(secret);
			string decodedSecret = ByteArrayToBinaryString(decodedBytes);

			byte[] clonedBytes = (byte[])decodedBytes.Clone();
			clonedBytes[14] = 0;
			var checksum = CalculateChecksum(clonedBytes);

			if ((decodedBytes[14] & 7) != (checksum & 7))
				throw new InvalidSecretException("Checksum does not match expected value");

			if (decodedSecret[3] != '0' && decodedSecret[4] != '1')
				throw new ArgumentException("The specified data is not a ring code", "secret");

			GameID = Convert.ToInt16(decodedSecret.ReversedSubstring(5, 15), 2);

			long rings = unchecked((long)Convert.ToUInt64(
				decodedSecret.ReversedSubstring(36, 8) +
				decodedSecret.ReversedSubstring(76, 8) +
				decodedSecret.ReversedSubstring(28, 8) +
				decodedSecret.ReversedSubstring(60, 8) +
				decodedSecret.ReversedSubstring(44, 8) +
				decodedSecret.ReversedSubstring(68, 8) +
				decodedSecret.ReversedSubstring(20, 8) +
				decodedSecret.ReversedSubstring(52, 8)
				, 2));
			_rings = rings;
		}

		/// <summary>
		/// Gets the raw secret data as a byte array
		/// </summary>
		/// <returns>A byte array containing the secret</returns>
		public override byte[] GetSecretBytes()
		{
			byte ring1 = (byte)_rings;
			byte ring2 = (byte)(_rings >> 8);
			byte ring3 = (byte)(_rings >> 16);
			byte ring4 = (byte)(_rings >> 24);
			byte ring5 = (byte)(_rings >> 32);
			byte ring6 = (byte)(_rings >> 40);
			byte ring7 = (byte)(_rings >> 48);
			byte ring8 = (byte)(_rings >> 56);

			int cipherKey = ((GameID >> 8) + (GameID & 255)) & 7;
			string unencodedSecret = Convert.ToString(cipherKey, 2).PadLeft(3, '0').Reverse();

			unencodedSecret += "01"; // ring secret

			unencodedSecret += Convert.ToString(GameID, 2).PadLeft(15, '0').Reverse();
			unencodedSecret += Convert.ToString(ring2, 2).PadLeft(8, '0').Reverse();
			unencodedSecret += Convert.ToString(ring6, 2).PadLeft(8, '0').Reverse();
			unencodedSecret += Convert.ToString(ring8, 2).PadLeft(8, '0').Reverse();
			unencodedSecret += Convert.ToString(ring4, 2).PadLeft(8, '0').Reverse();
			unencodedSecret += Convert.ToString(ring1, 2).PadLeft(8, '0').Reverse();
			unencodedSecret += Convert.ToString(ring5, 2).PadLeft(8, '0').Reverse();
			unencodedSecret += Convert.ToString(ring3, 2).PadLeft(8, '0').Reverse();
			unencodedSecret += Convert.ToString(ring7, 2).PadLeft(8, '0').Reverse();

			byte[] unencodedBytes = BinaryStringToByteArray(unencodedSecret);
			unencodedBytes[14] = CalculateChecksum(unencodedBytes);
			byte[] secret = EncodeBytes(unencodedBytes);

			return secret;
		}
	}
}
