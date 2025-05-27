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
	/// Represents a secret used to transfer memories between the games in the Zelda Oracle series.
	/// </summary>
	public class MemorySecret : Secret
	{
		private byte memory = 0;
		private bool isReturnSecret = true;
		private byte targetGame = 0;

		/// <summary>
		/// Gets the required length of the secret
		/// </summary>
		public override int Length => 5;

		/// <summary>
		/// Gets or sets the memory to use for this secret
		/// </summary>
		public Memory Memory
		{
			get => (Memory)memory;
			set => SetProperty(ref memory, (byte)value, nameof(Memory));
		}

		/// <summary>
		/// Gets or sets the target game used for this secret
		/// </summary>
		public Game TargetGame
		{
			get => (Game)targetGame;
			set => SetProperty(ref targetGame, (byte)value, nameof(TargetGame));
		}

		/// <summary>
		/// Gets or sets a value indicating whether this instance is return secret.
		/// </summary>
		/// <value>
		/// <c>true</c> if this instance is return secret; otherwise, <c>false</c>.
		/// </value>
		public bool IsReturnSecret
		{
			get => isReturnSecret;
			set => SetProperty(ref isReturnSecret, value, nameof(IsReturnSecret));
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="MemorySecret"/> class.
		/// </summary>
		public MemorySecret() { }

		/// <summary>
		/// Initializes a new instance of the <see cref="MemorySecret"/> class.
		/// </summary>
		/// <param name="info">The game information.</param>
		/// <param name="memory">The memory.</param>
		/// <param name="isReturnSecret">if set to <c>true</c> is return secret.</param>
		/// <example>
		/// <code language="C#">
		/// GameInfo info = new GameInfo()
		/// {
		///     Region = GameRegion.US,
		///     Game = Game.Ages,
		///     GameID = 14129
		/// };
		/// MemorySecret secret = new MemorySecret(info, Memory.ClockShopKingZora, true);
		/// </code>
		/// </example>
		public MemorySecret(GameInfo info, Memory memory, bool isReturnSecret)
			: this(info.Game, info.Region, info.GameID, memory, isReturnSecret) { }

		/// <summary>
		/// Initializes a new instance of the <see cref="MemorySecret"/> class with
		/// the specified values.
		/// </summary>
		/// <param name="game">The game.</param>
		/// <param name="region">The region of the game</param>
		/// <param name="gameId">The game id.</param>
		/// <param name="memory">The memory.</param>
		/// <param name="isReturnSecret">if set to <c>true</c> is return secret.</param>
		public MemorySecret(Game game, GameRegion region, short gameId, Memory memory, bool isReturnSecret)
		{
			TargetGame = game;
			Region = region;
			GameID = gameId;
			Memory = memory;
			IsReturnSecret = isReturnSecret;
		}

		/// <summary>
		/// Loads in data from the raw secret data provided
		/// </summary>
		/// <param name="secret">The raw secret data</param>
		/// <param name="region">The region of the game</param>
		/// <example>
		/// This example demonstrates loading a <see cref="MemorySecret"/> from a
		/// a byte array containing an encoded secret.
		/// <code language="C#">
		/// // 6●sW↑
		/// byte[] rawSecret = new byte[]
		/// {
		///     55, 21, 41, 18, 59
		/// };
		/// Secret secret = new MemorySecret();
		/// secret.Load(rawSecret, GameRegion.US);
		/// </code>
		/// </example>
		public override void Load(byte[] secret, GameRegion region)
		{
			if (secret is null)
			{
				throw new ArgumentNullException(nameof(secret));
			}

			if (secret.Length != Length)
			{
				throw new SecretException("Secret must contatin exactly 5 bytes");
			}

			Region = region;

			byte[] decodedBytes = DecodeBytes(secret);
			byte[] clonedBytes = (byte[])decodedBytes.Clone();
			clonedBytes[4] = 0;
			byte checksum = CalculateChecksum(clonedBytes);


			if (( decodedBytes[4] & 0xF ) != ( checksum & 0xF ))
			{
				throw new InvalidChecksumException("Checksum does not match expected value");
			}

			if (ExtractBits(decodedBytes, 3, 1) != 1 || ExtractBits(decodedBytes, 4, 1) != 1)
			{
				throw new ArgumentException("The specified data is not a memory code", nameof(secret));
			}

			GameID = (short)ExtractBits(decodedBytes, 5, 15);
			Memory = (Memory)ExtractBits(decodedBytes, 20, 4);

			TargetGame = ExtractBits(decodedBytes, 24, 1) == ExtractBits(decodedBytes, 25, 1) ? Game.Ages : Game.Seasons;
			IsReturnSecret = ExtractBits(decodedBytes, 24, 1) == 1;
		}

		/// <summary>
		/// Gets the raw secret data as a byte array
		/// </summary>
		/// <returns>A byte array containing the secret</returns>
		/// <example>
		/// <code language="C#">
		/// MemorySecret secret = new MemorySecret()
		/// {
		///     GameID = 14129,
		///     TargetGame = Game.Ages,
		///     Memory = Memory.ClockShopKingZora,
		///     IsReturnSecret = true
		/// };
		/// byte[] data = secret.ToBytes();
		/// </code>
		/// </example>
		public override byte[] ToBytes()
		{
			int cipher = 0;
			if (TargetGame == Game.Ages)
			{
				cipher = isReturnSecret ? 3 : 0;
			}
			else
			{
				cipher = isReturnSecret ? 1 : 2;
			}

			cipher |= ( ( memory & 1 ) << 2 );
			cipher = ( ( GameID >> 8 ) + ( GameID & 255 ) + cipher ) & 7;

			byte[] unencoded = new byte[5];

			InsertBits(unencoded, cipher, 0, 3);
			InsertBits(unencoded, 0b11, 3, 2); // memory = 11 
			InsertBits(unencoded, GameID, 5, 15);
			InsertBits(unencoded, memory, 20, 4);

			int mask;

			if (TargetGame == Game.Ages)
			{
				mask = isReturnSecret ? 3 : 0;
			}
			else
			{
				mask = isReturnSecret ? 2 : 1;
			}

			unencoded[4] = (byte)( CalculateChecksum(unencoded) | ( mask << 4 ) );
			byte[] secret = EncodeBytes(unencoded);

			return secret;
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

			var g = (MemorySecret)obj;

			return
				( base.Equals(g) ) &&
				( targetGame == g.targetGame ) &&
				( memory == g.memory ) &&
				( isReturnSecret == g.isReturnSecret );

		}

		/// <summary>
		/// Returns a hash code for this instance.
		/// </summary>
		/// <returns>
		/// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table.
		/// </returns>
		public override int GetHashCode()
		{
			return base.GetHashCode() ^
				targetGame.GetHashCode() ^
				memory.GetHashCode() ^
				isReturnSecret.GetHashCode();
		}
	}
}
