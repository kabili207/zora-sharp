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
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace Zyrenth.Zora
{
	/// <summary>
	/// Represents a secret used in the Zelda Oracle series games.
	/// </summary>
	public abstract class Secret : INotifyPropertyChanged
	{
		private static readonly byte[][] ciphers =
		{ 
			// JP
			new byte[]
			{
				0x31, 0x09, 0x29, 0x3b, 0x18, 0x3c, 0x17, 0x33,
				0x35, 0x01, 0x0b, 0x0a, 0x30, 0x21, 0x2d, 0x25,
				0x20, 0x3a, 0x2f, 0x1e, 0x39, 0x19, 0x2a, 0x06,
				0x04, 0x15, 0x23, 0x2e, 0x32, 0x28, 0x13, 0x34,
				0x10, 0x0d, 0x3f, 0x1a, 0x37, 0x0f, 0x3e, 0x36,
				0x38, 0x02, 0x16, 0x3d, 0x2c, 0x0e, 0x1b, 0x12
			},
			// US/PAL
			new byte[]
			{
				21, 35, 46,  4, 13, 63, 26, 16,
				58, 47, 30, 32, 15, 62, 54, 55,
				 9, 41, 59, 49,  2, 22, 61, 56,
				40, 19, 52, 50,  1, 11, 10, 53,
				14, 27, 18, 44, 33, 45, 37, 48,
				25, 42,  6, 57, 60, 23, 51, 24
			},
		};
        private short _gameId = 0;
        private GameRegion _region = GameRegion.US;

		/// <summary>
		/// Occurs when a property has changed
		/// </summary>
		[field: NonSerialized]
		public event PropertyChangedEventHandler PropertyChanged;


		/// <summary>
		/// Gets the required length of the secret
		/// </summary>
		public abstract int Length { get; }

		/// <summary>
		/// Gets or sets the unique game ID 
		/// </summary>
		public short GameID
		{
			get { return _gameId; }
			set
			{
				_gameId = value;
				NotifyPropertyChanged("GameID");
			}
		}

		/// <summary>
		/// Gets or sets the region
		/// </summary>
		public GameRegion Region
		{
			get { return _region; }
			set
			{
				_region = value;
				NotifyPropertyChanged("Region");
			}
		}

		/// <summary>
		/// Loads in data from the specified game info
		/// </summary>
		/// <param name="info">The game info</param>
		public abstract void Load(GameInfo info);

		/// <summary>
		/// Loads in data from the raw secret data provided
		/// </summary>
		/// <param name="secret">The raw secret data</param>
		/// <param name="region">The region of the game</param>
		public abstract void Load(byte[] secret, GameRegion region);

		/// <summary>
		/// Loads in data from the secret string provided
		/// </summary>
		/// <param name="secret">The secret</param>
		/// <param name="region">The region of the game</param>
		/// <example>
		/// This example demonstrates loading a <see cref="GameSecret"/> from a
		/// secret string.
		/// <code language="C#">
		/// string gameSecret = "H~2:@ left 2 diamond yq GB3 circle ( 6 heart ? up 6";
		/// Secret secret = new GameSecret();
		/// secret.Load(gameSecret, GameRegion.US);
		/// </code>
		/// </example>
		public virtual void Load(string secret, GameRegion region)
		{
			Region = region;
			Load(SecretParser.ParseSecret(secret, region), region);
		}

		/// <summary>
		/// Gets the raw secret data as a byte array
		/// </summary>
		/// <returns>A byte array containing the secret</returns>
		public abstract byte[] ToBytes();

		/// <summary>
		/// Returns a string that represents the current secret.
		/// </summary>
		/// <returns>A string that represents the current secret.</returns>
		/// <seealso cref="SecretParser.CreateString"/>
		/// <remarks>
		/// ToString will format the secret as it would be represented in the games.
		/// Internally, this method calls <see cref="ToBytes"/> and passes the result
		/// to <see cref="SecretParser.CreateString"/>.
		/// </remarks>
		/// <example>
		/// This example demonstrates how to get secret string from a <see cref="GameSecret"/>.
		/// <code language="C#">
		/// GameSecret secret = new GameSecret()
		/// {
		///     TargetGame = Game.Ages,
		///     GameID = 14129,
		///     Hero = "Link",
		///     Child = "Pip",
		///     Animal = Animal.Dimitri,
		///     Behavior = ChildBehavior.BouncyD,
		///     IsLinkedGame = true,
		///     IsHeroQuest = false
		/// };
		/// string secretString = secret.ToString(secret);
		/// // H~2:@ ←2♦yq GB3●( 6♥?↑6
		/// </code>
		/// </example>
		public override string ToString()
		{
			return SecretParser.CreateString(ToBytes(), _region);
		}

		/// <summary>
		/// Calculates the checksum for the specified <paramref name="secret"/>
		/// </summary>
		/// <param name="secret">The secret</param>
		/// <returns>The calculated checksum</returns>
		internal protected byte CalculateChecksum(byte[] secret)
		{
			// LINQ doesn't support .Sum() for anything smaller than an int, so we
			// have to use the lambda overload instead. Bytes get auto-promoted to
			// ints, which explains why we have to cast the return back to a byte.
			// Because reasons.
			byte sum = (byte)secret.Sum(x => x);
			int checksum = sum & 0x0F;
			return (byte)checksum;
		}

		/// <summary>
		/// Takes a raw, unencoded secret and encodes it.
		/// </summary>
		/// <param name="data">The secret data.</param>
		/// <returns>An encoded secret.</returns>
		internal protected byte[] EncodeBytes(byte[] data)
		{
			int cipherKey = (data[0] >> 3);
			int cipherPosition = cipherKey * 4;

			byte[] secret = new byte[data.Length];
			for (int i = 0; i < data.Length; ++i)
			{
				secret[i] = (byte)(data[i] ^ ciphers[(int)_region][cipherPosition++]);
			}

			secret[0] = (byte)(secret[0] & 7 | (cipherKey << 3));
			return secret;
		}

		/// <summary>
		/// Takes an encoded secret, as used by the games, and decodes it.
		/// </summary>
		/// <param name="secret">The encoded secret.</param>
		/// <returns>A decoded secret.</returns>
		internal protected byte[] DecodeBytes(byte[] secret)
		{
			int cipherKey = (secret[0] >> 3);
			int cipherPosition = cipherKey * 4;

			byte[] decodedBytes = new byte[secret.Length];

			for (int i = 0; i < secret.Length; ++i)
			{
				decodedBytes[i] = (byte)(secret[i] ^ ciphers[(int)_region][cipherPosition++]);
			}

			decodedBytes[0] = (byte)(decodedBytes[0] & 7 | (cipherKey << 3));

			return decodedBytes;
		}

		/// <summary>
		/// Converts a byte array to a string representation of ones and zeros
		/// </summary>
		/// <param name="secret">The secret.</param>
		/// <returns>A string of ones and zeros</returns>
		internal protected string ByteArrayToBinaryString(byte[] secret)
		{
			string data = "";
			foreach (byte b in secret)
			{
				data += Convert.ToString(b, 2).PadLeft(6, '0');
			}
			return data;
		}

		/// <summary>
		/// Converts a binary string (i.e. a string of ones and zeros) to a byte array
		/// </summary>
		/// <param name="data">The binary string</param>
		/// <returns>A byte array that the string represents</returns>
		internal protected byte[] BinaryStringToByteArray(string data)
		{
			byte[] secret = new byte[data.Length / 6 + 1];
			for (int i = 0; i < secret.Length - 1; ++i)
			{
				secret[i] = (byte)(Convert.ToByte(data.Substring(i * 6, 6), 2));
			}
			return secret;
		}

		/// <summary>
		/// Sends a notification that a property has changed.
		/// </summary>
		/// <param name="propertyName">Name of the property.</param>
		/// <example>
		/// <code language="C#">
		/// private short _gameID = 0;
		/// public short GameID
		/// {
		///     get { return _gameId; }
		///     set
		///     {
		///         _gameId = value;
		///         NotifyPropertyChanged("GameID");
		///     }
		/// }
		/// </code>
		/// </example>
		internal protected void NotifyPropertyChanged(string propertyName)
		{
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

		/// <summary>
		/// Determines whether the specified <see cref="System.Object" />, is equal to this instance.
		/// </summary>
		/// <param name="obj">The <see cref="System.Object" /> to compare with this instance.</param>
		/// <returns>
		///   <c>true</c> if the specified <see cref="System.Object" /> is equal to this instance; otherwise, <c>false</c>.
		/// </returns>
		public override bool Equals(object obj)
		{
			if (GetType() != obj?.GetType())
				return false;

			var g = (Secret)obj;

			return _gameId == g._gameId && _region == g._region;

		}

		/// <summary>
		/// Returns a hash code for this instance.
		/// </summary>
		/// <returns>
		/// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table. 
		/// </returns>
		public override int GetHashCode()
		{
			return _gameId.GetHashCode() ^ _region.GetHashCode();
		}

	}
}
