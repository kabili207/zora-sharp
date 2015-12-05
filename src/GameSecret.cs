/*
 *  Copyright © 2013-2015, Andrew Nagle.
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
	/// Represents a secret used to start a new game in the Zelda Oracle series
	/// </summary>
	public class GameSecret : Secret
	{
		string _hero = "\0\0\0\0\0";
		string _child = "\0\0\0\0\0";
		byte _behavior = 0;
		byte _animal = 0;
		Game _targetGame = 0;
		bool _isHeroQuest = false;
		bool _isLinkedGame = false;
		bool _wasGivenFreeRing = false;

		bool _unknown58 = false;
		bool _unknown59 = false;
		bool _unknown88 = true;

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
		public Game TargetGame
		{
			get { return _targetGame; }
			set
			{
				_targetGame = value;
				NotifyPropertyChanged("Game");
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
				NotifyPropertyChanged("IsHeroQuest");
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
				NotifyPropertyChanged("IsLinkedGame");
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
				NotifyPropertyChanged("Hero");
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
				NotifyPropertyChanged("Child");
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
				NotifyPropertyChanged("Animal");
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
				NotifyPropertyChanged("Behavior");
			}
		}

		/// <summary>
		/// Gets or sets the value indicating if Vasu has given the player a free ring
		/// </summary>
		public bool WasGivenFreeRing
		{
			get { return _wasGivenFreeRing; }
			set
			{
				_wasGivenFreeRing = value;
				NotifyPropertyChanged("WasGivenFreeRing");
			}
		}

		/// <summary>
		/// Gets or sets the unknown flag at offset 58
		/// </summary>
		public bool Unknown58
		{
			get { return _unknown58; }
			set
			{
				_unknown58 = value;
				NotifyPropertyChanged("Unknown58");
			}
		}

		/// <summary>
		/// Gets or sets the unknown flag at offset 59
		/// </summary>
		public bool Unknown59
		{
			get { return _unknown59; }
			set
			{
				_unknown59 = value;
				NotifyPropertyChanged("Unknown59");
			}
		}

		/// <summary>
		/// Gets or sets the unknown flag at offset 88
		/// </summary>
		public bool Unknown88
		{
			get { return _unknown88; }
			set
			{
				_unknown88 = value;
				NotifyPropertyChanged("Unknown88");
			}
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="GameSecret"/> class.
		/// </summary>
		public GameSecret() { }

		/// <summary>
		/// Initializes a new instance of the <see cref="GameSecret"/> class from the
		/// specified game <paramref name="info"/>.
		/// </summary>
		/// <param name="info">The game information.</param>
		public GameSecret(GameInfo info) : base()
		{
			Load(info);
		}

		/// <summary>
		/// Loads in data from the specified game info
		/// </summary>
		/// <param name="info">The game info</param>
		/// <example>
		/// <code language="C#">
		/// GameInfo info = new GameInfo()
		/// {
		///     Game = Game.Ages,
		///     GameID = 14129,
		///     Hero = "Link",
		///     Child = "Pip",
		///     Animal = Animal.Dimitri,
		///     Behavior = ChildBehavior.BouncyD,
		///     IsLinkedGame = true,
		///     IsHeroQuest = false,
		///     WasGivenFreeRing = true
		/// };
		/// GameSecret secret = new GameSecret();
		/// secret.Load(info);
		/// </code>
		/// </example>
		public override void Load(GameInfo info)
		{
			GameID = info.GameID;
			TargetGame = info.Game;
			Hero = info.Hero;
			Child = info.Child;
			Animal = info.Animal;
			Behavior = info.Behavior;
			IsLinkedGame = info.IsLinkedGame;
			IsHeroQuest = info.IsHeroQuest;
			WasGivenFreeRing = info.WasGivenFreeRing;
			Unknown58 = info.Unknown58;
			Unknown59 = info.Unknown59;
			Unknown88 = info.Unknown88;
		}

		/// <summary>
		/// Loads in data from the raw secret data provided
		/// </summary>
		/// <param name="secret">The raw secret data</param>
		/// <example>
		/// This example demonstrates loading a <see cref="GameSecret"/> from a
		/// a byte array containing an encoded secret.
		/// <code language="C#">
		/// // H~2:@ ←2♦yq GB3●( 6♥?↑6
		/// byte[] rawSecret = new byte[]
		/// {
		///      4, 37, 51, 36, 63,
		///     61, 51, 10, 44, 39,
		///      3,  0, 52, 21, 48,
		///     55,  9, 45, 59, 55
		/// };
		/// Secret secret = new GameSecret();
		/// secret.Load(rawSecret);
		/// </code>
		/// </example>
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
			
			TargetGame = (Game)(byte)(decodedSecret[21] == '1' ? 1 : 0);
			IsHeroQuest = decodedSecret[20] == '1';
			IsLinkedGame = decodedSecret[105] == '1';


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
			WasGivenFreeRing = decodedSecret[76] == '1';


			// TODO: Figure out what all the unknown values are for.
			Unknown58 = decodedSecret[58] == '1';
			Unknown59 = decodedSecret[59] == '1';
			Unknown88 = decodedSecret[88] == '1';
		}

		/// <summary>
		/// Gets the raw secret data as a byte array
		/// </summary>
		/// <returns>A byte array containing the secret</returns>
		/// <example>
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
		///     IsHeroQuest = false,
		///     WasGivenFreeRing = true
		/// };
		/// byte[] data = secret.ToBytes();
		/// </code>
		/// </example>
		public override byte[] ToBytes()
		{
			int cipherKey = ((GameID >> 8) + (GameID & 255)) & 7;
			string unencodedSecret = Convert.ToString(cipherKey, 2).PadLeft(3, '0').Reverse();

			unencodedSecret += "00"; // game = 0

			unencodedSecret += Convert.ToString(GameID, 2).PadLeft(15, '0').Reverse();
			unencodedSecret += _isHeroQuest ? "1" : "0";
			unencodedSecret += TargetGame == Game.Ages ? "0" : "1";
			unencodedSecret += Convert.ToString((byte)_hero[0], 2).PadLeft(8, '0').Reverse();
			unencodedSecret += Convert.ToString((byte)_child[0], 2).PadLeft(8, '0').Reverse();
			unencodedSecret += Convert.ToString((byte)_hero[1], 2).PadLeft(8, '0').Reverse();
			unencodedSecret += Convert.ToString((byte)_child[1], 2).PadLeft(8, '0').Reverse();
			unencodedSecret += Convert.ToString(_behavior, 2).PadLeft(8, '0').Reverse().Substring(0, 4);
			unencodedSecret += _unknown58 ? "1" : "0"; // TODO: This
			unencodedSecret += _unknown59 ? "1" : "0"; // TODO: This
			unencodedSecret += Convert.ToString((byte)_hero[2], 2).PadLeft(8, '0').Reverse();
			unencodedSecret += Convert.ToString((byte)_child[2], 2).PadLeft(8, '0').Reverse();
			unencodedSecret += _wasGivenFreeRing ? "1" : "0";
			unencodedSecret += Convert.ToString((byte)_hero[3], 2).PadLeft(8, '0').Reverse();
			unencodedSecret += Convert.ToString(_animal, 2).PadLeft(8, '0').Reverse().Substring(0, 3);
			unencodedSecret += _unknown88 ? "1" : "0"; // TODO: This
			unencodedSecret += Convert.ToString((byte)_hero[4], 2).PadLeft(8, '0').Reverse();
			unencodedSecret += Convert.ToString((byte)_child[3], 2).PadLeft(8, '0').Reverse();
			unencodedSecret += _isLinkedGame ? "1" : "0";
			unencodedSecret += Convert.ToString((byte)_child[4], 2).PadLeft(8, '0').Reverse();

			byte[] unencodedBytes = BinaryStringToByteArray(unencodedSecret);
			unencodedBytes[19] = CalculateChecksum(unencodedBytes);
			byte[] secret = EncodeBytes(unencodedBytes);

			return secret;
		}

		/// <summary>
		/// Updates the game information.
		/// </summary>
		/// <param name="info">The information.</param>
		/// <example>
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
		///     IsHeroQuest = false,
		///     WasGivenFreeRing = true
		/// };
		/// GameInfo info = new GameInfo();
		/// secret.UpdateGameInfo(info);
		/// </code>
		/// </example>
		public void UpdateGameInfo(GameInfo info)
		{
			info.GameID = GameID;
			info.Game = TargetGame;
			info.Hero = Hero;
			info.Child = Child;
			info.Animal = Animal;
			info.Behavior = Behavior;
			info.IsLinkedGame = IsLinkedGame;
			info.IsHeroQuest = IsHeroQuest;
			info.WasGivenFreeRing = WasGivenFreeRing;
			info.Unknown58 = Unknown58;
			info.Unknown59 = Unknown59;
			info.Unknown88 = Unknown88;
		}
	}
}
