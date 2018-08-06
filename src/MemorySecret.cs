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
using System.Linq;
using System.Text;

namespace Zyrenth.Zora
{
	/// <summary>
	/// Represents a secret used to transfer memories between the games in the Zelda Oracle series.
	/// </summary>
	public class MemorySecret : Secret
	{
		private byte _memory = 0;
		private bool _isReturnSecret = true;
		private byte _targetGame = 0;

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
				SetProperty(ref _memory, (byte)value, "Memory");
			}
		}

		/// <summary>
		/// Gets or sets the target game used for this secret
		/// </summary>
		public Game TargetGame
		{
			get { return (Game)_targetGame; }
			set
			{
				SetProperty(ref _targetGame, (byte)value, "Game");
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
				SetProperty(ref _isReturnSecret, value, "IsReturnSecret");
			}
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
		/// Loads in data from the specified game info
		/// </summary>
		/// <param name="info">The game info</param>
		/// <remarks>
		/// Because <see cref="GameInfo"/> does not contain information about
		/// memories, only the properties <see cref="TargetGame"/> and
		/// <see cref="Secret.GameID"/> will be populated by this method.
		/// </remarks>
		/// <example>
		/// <code language="C#">
		/// GameInfo info = new GameInfo()
		/// {
		///     Region = GameRegion.US,
		///     Game = Game.Ages,
		///     GameID = 14129
		/// };
		/// MemorySecret secret = new MemorySecret();
		/// secret.Load(info);
		/// </code>
		/// </example>
		public override void Load(GameInfo info)
		{
			Region = info.Region;
			TargetGame = info.Game;
			GameID = info.GameID;
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
				throw new ArgumentNullException(nameof(secret));
			if (secret.Length != Length)
				throw new SecretException("Secret must contatin exactly 5 bytes");

			Region = region;

			byte[] decodedBytes = DecodeBytes(secret);
			string decodedSecret = ByteArrayToBinaryString(decodedBytes);

			byte[] clonedBytes = (byte[])decodedBytes.Clone();
			clonedBytes[4] = 0;
			var checksum = CalculateChecksum(clonedBytes);

			if (( decodedBytes[4] & 7 ) != ( checksum & 7 ))
				throw new InvalidChecksumException("Checksum does not match expected value");

			if (decodedSecret[3] != '1' || decodedSecret[4] != '1')
				throw new ArgumentException("The specified data is not a memory code", nameof(secret));

			GameID = Convert.ToInt16(decodedSecret.ReversedSubstring(5, 15), 2);
			Memory = (Memory)Convert.ToByte(decodedSecret.ReversedSubstring(20, 4), 2);

			// Because the game and return type are stored in the cipher and checksum
			// we compare it against all four possible values. It's ugly, but it works.
			string desiredString = SecretParser.CreateString(secret, Region);

			var memories = new[]
			{
				new MemorySecret(Game.Ages, region, GameID, Memory, true),
				new MemorySecret(Game.Ages, region, GameID, Memory, false),
				new MemorySecret(Game.Seasons, region, GameID, Memory, true),
				new MemorySecret(Game.Seasons, region, GameID, Memory, false)
			};

			bool found = false;

			foreach (var memSecret in memories)
			{
				if (desiredString == memSecret.ToString())
				{
					TargetGame = memSecret.TargetGame;
					IsReturnSecret = memSecret.IsReturnSecret;
					found = true;
					break;
				}
			}

			if (!found)
				throw new UnknownMemoryException("Cound not determine the type of memory secret");
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
				cipher = _isReturnSecret ? 3 : 0;
			else
				cipher = _isReturnSecret ? 1 : 2;

			cipher |= ( ( (byte)_memory & 1 ) << 2 );
			cipher = ( ( GameID >> 8 ) + ( GameID & 255 ) + cipher ) & 7;
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
			unencodedBytes[4] = (byte)( CalculateChecksum(unencodedBytes) | ( mask << 4 ) );
			byte[] secret = EncodeBytes(unencodedBytes);

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
				return false;

			var g = (MemorySecret)obj;

			return
				( base.Equals(g) ) &&
				( _targetGame == g._targetGame ) &&
				( _memory == g._memory ) &&
				( _isReturnSecret == g._isReturnSecret );

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
				_targetGame.GetHashCode() ^
				_memory.GetHashCode() ^
				_isReturnSecret.GetHashCode();
		}
	}
}
