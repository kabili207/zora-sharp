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
	/// Represents a secret used to start a new game in the Zelda Oracle series
	/// </summary>
	public class GameSecret : Secret
	{
		string _hero = "\0\0\0\0\0";
		string _child = "\0\0\0\0\0";
		byte _behavior = 0;
		byte _animal = 0;
		byte _agesSeasons = 0;
		bool _isHeroQuest = false;
		bool _isLinkedGame = false;

		/// <summary>
		/// Gets the required length of the secret
		/// </summary>
		public override int Length
		{
			get { return 20; }
		}

		/// <summary>
		/// Gets or sets the Game used for this user data
		/// </summary>
		public Game Game
		{
			get { return (Game)_agesSeasons; }
			set
			{
				_agesSeasons = (byte)value;
				OnPropertyChanged("Game");
			}
		}

		/// <summary>
		/// Gets or sets the Quest type used for this user data
		/// </summary>
		public bool IsHeroQuest
		{
			get { return _isHeroQuest; }
			set
			{
				_isHeroQuest = value;
				OnPropertyChanged("IsHeroQuest");
			}
		}

		/// <summary>
		/// Gets or sets the Quest type used for this user data
		/// </summary>
		public bool IsLinkedGame
		{
			get { return _isLinkedGame; }
			set
			{
				_isLinkedGame = value;
				OnPropertyChanged("IsLinkedGame");
			}
		}

		/// <summary>
		/// Gets or sets the hero's name
		/// </summary>
		public string Hero
		{
			get { return _hero.Trim(' ', '\0'); }
			set
			{
				if (string.IsNullOrWhiteSpace(value))
					_hero = "\0\0\0\0\0";
				else
					_hero = value.TrimEnd().PadRight(5, '\0');
				OnPropertyChanged("Hero");
			}
		}

		/// <summary>
		/// Gets or sets the child's name
		/// </summary>
		public string Child
		{
			get { return _child.Trim(' ', '\0'); }
			set
			{
				if (string.IsNullOrWhiteSpace(value))
					_child = "\0\0\0\0\0";
				else
					_child = value.TrimEnd().PadRight(5, '\0');
				OnPropertyChanged("Child");
			}
		}

		/// <summary>
		/// Gets or sets the animal friend
		/// </summary>
		public Animal Animal
		{
			get { return (Animal)_animal; }
			set
			{
				_animal = (byte)value;
				OnPropertyChanged("Animal");
			}
		}

		/// <summary>
		/// Gets or set the behavior of the child
		/// </summary>
		public ChildBehavior Behavior
		{
			get { return (ChildBehavior)_behavior; }
			set
			{
				_behavior = (byte)value;
				OnPropertyChanged("Behavior");
			}
		}

		/// <summary>
		/// Loads in data from the specified game info
		/// </summary>
		/// <param name="info">The game info</param>
		public override void Load(GameInfo info)
		{
			this.GameID = info.GameID;
		}

		/// <summary>
		/// Loads in data from the raw secret data provided
		/// </summary>
		/// <param name="secret">The raw secret data</param>
		public override void Load(byte[] secret)
		{
			if (secret == null || secret.Length != Length)
				throw new InvalidSecretException("Secret must contatin exactly 20 bytes");

			byte[] decodedBytes = DecodeBytes(secret);
			string decodedSecret = ByteArrayToBinaryString(decodedBytes);

			byte[] clonedBytes = (byte[])decodedBytes.Clone();
			clonedBytes[19] = 0;
			var checksum = CalculateChecksum(clonedBytes);
			if ((decodedBytes[19] & 7) != (checksum & 7))
				throw new InvalidSecretException("Checksum does not match expected value");

			GameID = Convert.ToInt16(decodedSecret.ReversedSubstring(5, 15), 2);

			if (decodedSecret[3] != '0' && decodedSecret[4] != '0')
				throw new ArgumentException("The specified data is not a game code", "secret");

			Game = (Game)(byte)(decodedSecret[21] == '1' ? 1 : 0);
			IsHeroQuest = decodedSecret[20] == '1';
			IsLinkedGame = decodedSecret[106] == '1';


			Hero = System.Text.Encoding.ASCII.GetString(new byte[] {
				Convert.ToByte(decodedSecret.ReversedSubstring(22, 8), 2),
				Convert.ToByte(decodedSecret.ReversedSubstring(38, 8), 2),
				Convert.ToByte(decodedSecret.ReversedSubstring(60, 8), 2),
				Convert.ToByte(decodedSecret.ReversedSubstring(77, 8), 2),
				Convert.ToByte(decodedSecret.ReversedSubstring(89, 8), 2)
			});

			Child = System.Text.Encoding.ASCII.GetString(new byte[] {
				Convert.ToByte(decodedSecret.ReversedSubstring(30, 8), 2),
				Convert.ToByte(decodedSecret.ReversedSubstring(46, 8), 2),
				Convert.ToByte(decodedSecret.ReversedSubstring(68, 8), 2),
				Convert.ToByte(decodedSecret.ReversedSubstring(97, 8), 2),
				Convert.ToByte(decodedSecret.ReversedSubstring(106, 8), 2)
			});

			Animal = (Animal)Convert.ToByte(decodedSecret.ReversedSubstring(85, 3), 2);
			Behavior = (ChildBehavior)Convert.ToByte(decodedSecret.ReversedSubstring(54, 4), 2);


			// TODO: Figure out what all the unknown values are for.
			bool unknown2 = decodedSecret[58] == '1';
			bool unknown3 = decodedSecret[59] == '1';
			bool unknown4 = decodedSecret[76] == '1';
			bool unknown5 = decodedSecret[88] == '1';
			bool unknown6 = decodedSecret[105] == '1';
		}

		/// <summary>
		/// Gets the raw secret data as a byte array
		/// </summary>
		/// <returns>A byte array containing the secret</returns>
		public override byte[] GetSecretBytes()
		{
			int cipherKey = ((GameID >> 8) + (GameID & 255)) & 7;
			string unencodedSecret = Convert.ToString(cipherKey, 2).PadLeft(3, '0').Reverse();

			unencodedSecret += "00"; // game = 0

			unencodedSecret += Convert.ToString(GameID, 2).PadLeft(15, '0').Reverse();
			unencodedSecret += _isHeroQuest ? "1" : "0";
			unencodedSecret += Game == OracleHack.Game.Ages ? "0" : "1";
			unencodedSecret += Convert.ToString((byte)_hero[0], 2).PadLeft(8, '0').Reverse();
			unencodedSecret += Convert.ToString((byte)_child[0], 2).PadLeft(8, '0').Reverse();
			unencodedSecret += Convert.ToString((byte)_hero[1], 2).PadLeft(8, '0').Reverse();
			unencodedSecret += Convert.ToString((byte)_child[1], 2).PadLeft(8, '0').Reverse();
			unencodedSecret += Convert.ToString(_behavior, 2).PadLeft(8, '0').Reverse().Substring(0, 4);
			unencodedSecret += "0"; // unknown 2
			unencodedSecret += "0"; // unknown 3
			unencodedSecret += Convert.ToString((byte)_hero[2], 2).PadLeft(8, '0').Reverse();
			unencodedSecret += Convert.ToString((byte)_child[2], 2).PadLeft(8, '0').Reverse();
			unencodedSecret += "1"; // unknown 4
			unencodedSecret += Convert.ToString((byte)_hero[3], 2).PadLeft(8, '0').Reverse();
			unencodedSecret += Convert.ToString(_animal, 2).PadLeft(8, '0').Reverse().Substring(0, 3);
			unencodedSecret += "1"; // unknown 5
			unencodedSecret += Convert.ToString((byte)_hero[4], 2).PadLeft(8, '0').Reverse();
			unencodedSecret += Convert.ToString((byte)_child[3], 2).PadLeft(8, '0').Reverse();
			unencodedSecret += _isLinkedGame ? "1" : "0";
			unencodedSecret += Convert.ToString((byte)_child[4], 2).PadLeft(8, '0').Reverse();

			byte[] unencodedBytes = BinaryStringToByteArray(unencodedSecret);
			unencodedBytes[19] = CalculateChecksum(unencodedBytes);
			byte[] secret = EncodeBytes(unencodedBytes);

			return secret;
		}
	}
}
