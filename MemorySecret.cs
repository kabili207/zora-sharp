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
	/// Represents a secret used to transfer memories between the games in the Zelda Oracle series.
	/// </summary>
	public class MemorySecret : Secret
	{
		byte _memory = 0;
		bool _isReturnSecret = true;
		byte _agesSeasons = 0;

		/// <summary>
		/// Gets the required length of the secret
		/// </summary>
		public override int Length
		{
			get { return 5; }
		}

		/// <summary>
		/// Gets or sets the memory to use for this secret
		/// </summary>
		public Memory Memory 
		{
			get { return (Memory)_memory; }
			set
			{
				_memory = (byte)value;
				OnPropertyChanged("Memory");
			}
		}

		/// <summary>
		/// Gets or sets the target game used for this secret
		/// </summary>
		public Game TargetGame
		{
			get { return (Game)_agesSeasons; }
			set
			{
				_agesSeasons = (byte)value;
				OnPropertyChanged("Game");
			}
		}

		/// <summary>
		/// Gets or sets a value indicating whether this instance is return secret.
		/// </summary>
		/// <value>
		/// <c>true</c> if this instance is return secret; otherwise, <c>false</c>.
		/// </value>
		public bool IsReturnSecret
		{
			get { return _isReturnSecret; }
			set
			{
				_isReturnSecret = value;
				OnPropertyChanged("IsReturnSecret");
			}
		}

		/// <summary>
		/// Loads in data from the specified game info
		/// </summary>
		/// <param name="info">The game info</param>
		public override void Load(GameInfo info)
		{
			GameID = info.GameID;
		}

		/// <summary>
		/// Loads in data from the raw secret data provided
		/// </summary>
		/// <param name="secret">The raw secret data</param>
		public override void Load(byte[] secret)
		{
			// Loading the Game and IsReturnSecret properties require
			// parsing the cipher and checksum, which we don't do yet.
			throw new NotImplementedException();

			if (secret == null || secret.Length != Length)
				throw new InvalidSecretException("Secret must contatin exactly 5 bytes");

			byte[] decodedBytes = DecodeBytes(secret);
			string decodedSecret = ByteArrayToBinaryString(decodedBytes);
			
			if (decodedSecret[3] != '1' && decodedSecret[4] != '1')
				throw new ArgumentException("The specified data is not a memory code", "secret");

			GameID = Convert.ToInt16(decodedSecret.ReversedSubstring(5, 15), 2);
			Memory = (Memory)Convert.ToByte(decodedSecret.ReversedSubstring(20, 4), 2);

			// TODO: Verify checksum
			byte checksum = secret[4];
		}

		/// <summary>
		/// Gets the raw secret data as a byte array
		/// </summary>
		/// <returns>A byte array containing the secret</returns>
		public override byte[] GetSecretBytes()
		{
			int cipher = 0;
			if (TargetGame == Game.Ages)
				cipher = _isReturnSecret ? 3 : 0;
			else
				cipher = _isReturnSecret ? 1 : 2;

			cipher |= (((byte)_memory & 1) << 2);
			cipher = ((GameID >> 8) + (GameID & 255) + cipher) & 7;
			cipher = Convert.ToInt32(Convert.ToString(cipher, 2).PadLeft(3, '0').Reverse(), 2);

			string unencodedSecret = Convert.ToString(cipher, 2).PadLeft(3, '0');

			unencodedSecret += "11"; // memory secret

			unencodedSecret += Convert.ToString(GameID, 2).PadLeft(15, '0').Reverse();
			unencodedSecret += Convert.ToString((byte)_memory, 2).PadLeft(4, '0').Reverse();

			int mask = 0;

			if (TargetGame == Game.Ages)
				mask = _isReturnSecret ? 3 : 0;
			else
				mask = _isReturnSecret ? 2 : 1;
			byte[] unencodedBytes = BinaryStringToByteArray(unencodedSecret);
			unencodedBytes[4] = (byte)(CalculateChecksum(unencodedBytes) | (mask << 4));
			byte[] secret = EncodeBytes(unencodedBytes);

			return secret;
		}
	}
}
