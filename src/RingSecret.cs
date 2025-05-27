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

namespace Zyrenth.Zora
{
	/// <summary>
	/// Represents a secret used to transfer rings between games in the Zelda Oracle series.
	/// </summary>
	public class RingSecret : Secret
	{
		private long rings = 0L;

		/// <summary>
		/// Gets the required length of the secret
		/// </summary>
		public override int Length => 15;

		/// <summary>
		/// Gets or sets the user's ring collection
		/// </summary>
		public Rings Rings
		{
			get => (Rings)rings;
			set => SetProperty(ref rings, (long)value, nameof(Rings));
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="RingSecret"/> class.
		/// </summary>
		public RingSecret() { }

		/// <summary>
		/// Initializes a new instance of the <see cref="RingSecret"/> class with the
		/// specified <paramref name="gameId"/> and <paramref name="rings"/>.
		/// </summary>
		/// <param name="gameId">The game identifier.</param>
		/// <param name="region">The region of the game</param>
		/// <param name="rings">The rings.</param>
		public RingSecret(short gameId, GameRegion region, Rings rings)
		{
			GameID = gameId;
			Region = region;
			Rings = rings;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="RingSecret"/> class from the
		/// specified game <paramref name="info"/>.
		/// </summary>
		/// <param name="info">The game information.</param>
		/// <example>
		/// <code language="C#">
		/// GameInfo info = new GameInfo()
		/// {
		///     GameID = 14129,
		///     Rings = Rings.PowerRingL1 | Rings.DoubleEdgeRing | Rings.ProtectionRing
		/// };
		/// RingSecret secret = new RingSecret();
		/// secret.Load(info);
		/// </code>
		/// </example>
		public RingSecret(GameInfo info) :
			this(info.GameID, info.Region, info.Rings)
		{
		}


		/// <summary>
		/// Loads in data from the raw secret data provided
		/// </summary>
		/// <param name="secret">The raw secret data</param>
		/// <param name="region">The region of the game</param>
		/// <example>
		/// This example demonstrates loading a <see cref="RingSecret"/> from a
		/// a byte array containing an encoded secret.
		/// <code language="C#">
		/// // L~2:N @bB↑&amp; hmRh=
		/// byte[] rawSecret = new byte[]
		/// {
		///      6, 37, 51, 36, 13,
		///     63, 26,  0, 59, 47,
		///     30, 32, 15, 30, 49
		/// };
		/// Secret secret = new RingSecret();
		/// secret.Load(rawSecret, GameRegion.US);
		/// </code>
		/// </example>
		public override void Load(byte[] secret, GameRegion region)
		{
			if (secret is null || secret.Length != Length)
			{
				throw new SecretException("Secret must contain exactly 15 bytes");
			}

			Region = region;

			byte[] decodedBytes = DecodeBytes(secret);
			byte[] clonedBytes = (byte[])decodedBytes.Clone();
			clonedBytes[14] = 0;
			byte checksum = CalculateChecksum(clonedBytes);

			if (( decodedBytes[14] & 0xF ) != ( checksum & 0xF ))
			{
				throw new InvalidChecksumException("Checksum does not match expected value");
			}

			if (ExtractBits(decodedBytes, 3, 1) != 0 || ExtractBits(decodedBytes, 4, 1) != 1)
			{
				throw new ArgumentException("The specified data is not a ring code", nameof(secret));
			}

			GameID = (short)ExtractBits(decodedBytes, 5, 15);

			long rings =
				( (long)ExtractBits(decodedBytes, 36, 8) ) << 56 |
				( (long)ExtractBits(decodedBytes, 76, 8) ) << 48 |
				( (long)ExtractBits(decodedBytes, 28, 8) ) << 40 |
				( (long)ExtractBits(decodedBytes, 60, 8) ) << 32 |
				( (long)ExtractBits(decodedBytes, 44, 8) ) << 24 |
				( (long)ExtractBits(decodedBytes, 68, 8) ) << 16 |
				( (long)ExtractBits(decodedBytes, 20, 8) ) << 8 |
				( (long)ExtractBits(decodedBytes, 52, 8) ) << 0;

			this.rings = rings;
		}

		/// <summary>
		/// Gets the raw secret data as a byte array
		/// </summary>
		/// <returns>A byte array containing the secret</returns>
		/// <example>
		/// <code language="C#">
		/// RingSecret secret = new RingSecret()
		/// {
		///     GameID = 14129,
		///     Rings = Rings.PowerRingL1 | Rings.DoubleEdgeRing | Rings.ProtectionRing
		/// };
		/// byte[] data = secret.ToBytes();
		/// </code>
		/// </example>
		public override byte[] ToBytes()
		{
			byte ring1 = (byte)rings;
			byte ring2 = (byte)( rings >> 8 );
			byte ring3 = (byte)( rings >> 16 );
			byte ring4 = (byte)( rings >> 24 );
			byte ring5 = (byte)( rings >> 32 );
			byte ring6 = (byte)( rings >> 40 );
			byte ring7 = (byte)( rings >> 48 );
			byte ring8 = (byte)( rings >> 56 );

			int cipherKey = ( ( GameID >> 8 ) + ( GameID & 0xFF ) ) & 7;

			byte[] unencodedBytes = new byte[15];

			InsertBits(unencodedBytes, cipherKey, 0, 3);
			InsertBits(unencodedBytes, 0b10, 3, 2); // secret type = 01 (ring)

			InsertBits(unencodedBytes, GameID, 5, 15);

			InsertBits(unencodedBytes, ring2, 20, 8);
			InsertBits(unencodedBytes, ring6, 28, 8);
			InsertBits(unencodedBytes, ring8, 36, 8);
			InsertBits(unencodedBytes, ring4, 44, 8);
			InsertBits(unencodedBytes, ring1, 52, 8);
			InsertBits(unencodedBytes, ring5, 60, 8);
			InsertBits(unencodedBytes, ring3, 68, 8);
			InsertBits(unencodedBytes, ring7, 76, 8);

			unencodedBytes[14] = CalculateChecksum(unencodedBytes);
			return EncodeBytes(unencodedBytes);
		}

		/// <summary>
		/// Updates the <see cref="GameInfo.Rings"/> property with the rings in this secret
		/// </summary>
		/// <param name="info">The information.</param>
		/// <param name="appendRings">
		/// If true, this will add the rings contained in the secret to the
		/// existings Rings. If false, it will overwrite them.
		/// </param>
		/// <exception cref="SecretException">
		/// The Game IDs or regions of the secret and game info do not match.
		/// </exception>
		/// <example>
		/// <code language="C#">
		/// RingSecret secret = new RingSecret()
		/// {
		///     GameID = 14129,
		///     Rings = Rings.PowerRingL1 | Rings.DoubleEdgeRing | Rings.ProtectionRing
		/// };
		/// GameInfo info = new GameInfo() { GameID = 14129 };
		/// bool appendRings = true;
		/// secret.UpdateGameInfo(info, appendRings);
		/// </code>
		/// </example>
		public void UpdateGameInfo(GameInfo info, bool appendRings)
		{
			if (info.Region != Region)
			{
				throw new SecretException("The regions of the secret and game info do not match.");
			}

			if (info.GameID != GameID)
			{
				throw new SecretException("The Game IDs of the secret and game info do not match. (Secret's Game ID is " + GameID + ".)");
			}

			info.Rings = Rings | ( appendRings ? info.Rings : Rings.None );
		}

		/// <summary>
		/// Counts the number of rings
		/// </summary>
		/// <returns>The number of rings from 0 to 64</returns>
		public byte RingCount()
		{
			return Extensions.CountSetBits(rings);
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
			{
				return false;
			}

			var g = (RingSecret)obj;

			return base.Equals(g) && ( rings == g.rings );

		}

		/// <summary>
		/// Returns a hash code for this instance.
		/// </summary>
		/// <returns>
		/// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table.
		/// </returns>
		public override int GetHashCode()
		{
			return base.GetHashCode() ^ rings.GetHashCode();
		}
	}
}
