using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;

namespace Zyrenth.OracleHack
{
	[Serializable]
	[JsonObject(MemberSerialization.OptIn)]
	public class GameInfo : INotifyPropertyChanged
	{

		private static readonly byte[] Cipher =
		{ 7, 35, 46, 4, 13, 63, 26, 16, 58, 47, 30,
			32, 15, 62, 54, 55, 9, 41, 59, 49, 2,
			22, 61, 56, 40, 19, 52, 50, 1, 11, 10,
			53, 14, 27, 18, 44, 33, 45, 37, 48, 25,
			42, 6, 57, 60, 23, 51 };

		[JsonProperty("GameID")]
		short _gameId = 0;
		[JsonProperty("Hero")]
		string _hero = "     ";
		[JsonProperty("Child")]
		string _child = "     ";
		[JsonProperty("Behavior")]
		byte _behavior = 0;
		[JsonProperty("Animal")]
		byte _animal = 0;
		[JsonProperty("QuestType")]
		byte _linkedHeros = 0;
		[JsonProperty("GameVersion")]
		byte _agesSeasons = 0;
		[JsonProperty("Rings")]
		ulong _rings = 0L;

		[NonSerialized]
		byte _currXor = 0;

		/// <summary>
		/// Occurs when a property has changed
		/// </summary>
		[field: NonSerialized]
		public event PropertyChangedEventHandler PropertyChanged;

		#region Properties

		public Game Game
		{
			get { return (Game)_agesSeasons; }
			set
			{
				_agesSeasons = (byte)value;
				OnPropertyChanged("Game");
			}
		}

		public Quest Quest
		{
			get { return (Quest)_linkedHeros; }
			set
			{
				_linkedHeros = (byte)Quest;
				OnPropertyChanged("Quest");
			}
		}

		public short GameID
		{
			get { return _gameId; }
			set
			{
				_gameId = value;
				OnPropertyChanged("GameID");
			}
		}

		public string Hero
		{
			get { return _hero.Trim(' ', '\0'); }
			set
			{
				if (string.IsNullOrWhiteSpace(value))
					_hero = "    ";
				else
					_hero = value.TrimEnd().PadRight(5, '\0');
				OnPropertyChanged("Hero");
			}
		}

		public string Child
		{
			get { return _child.Trim(' ', '\0'); }
			set
			{
				if (string.IsNullOrWhiteSpace(value))
					_child = "    ";
				else
					_child = value.TrimEnd().PadRight(5, '\0');
				OnPropertyChanged("Child");
			}
		}

		public Animal Animal
		{
			get { return (Animal)_animal; }
			set
			{
				_animal = (byte)value;
				OnPropertyChanged("Animal");
			}
		}

		public ChildBehavior Behavior
		{
			get { return (ChildBehavior)_behavior; }
			set
			{
				_behavior = (byte)value;
				OnPropertyChanged("Behavior");
			}
		}

		public Rings Rings
		{
			get { return (Rings)_rings; }
			set
			{
				_rings = (ulong)value;
				OnPropertyChanged("Rings");
			}
		}

		private byte CurrXor
		{
			get { return _currXor; }
			set
			{
				if (value > Cipher.Length - 1)
					_currXor = 0;
				else
					_currXor = value;
			}
		}

		#endregion // Properties

		public static GameInfo FromGameSecret(byte[] secret)
		{
			GameInfo parser = new GameInfo();
			parser.ReadGameSecret(secret);
			return parser;
		}

		#region Secret parsing logic

		// Logic taken from http://www.gamefaqs.com/boards/472313-the-legend-of-zelda-oracle-of-ages/66934363
		public void ReadGameSecret(byte[] secret)
		{
			if (secret == null || secret.Length != 20)
				throw new ArgumentException("Secret must contatin exactly 20 bytes", "secret");

			byte cipherStart = 0;

			byte ringOrGame = 0;

			// unknowns
			byte unknown1 = 0;

			SetBit(ref cipherStart, 2, GetBit(secret[0], 5));
			SetBit(ref cipherStart, 1, GetBit(secret[0], 4));
			SetBit(ref cipherStart, 0, GetBit(secret[0], 3));

			CurrXor = (byte)(cipherStart * 4);

			SetBit(ref unknown1, 0, GetBit((byte)(secret[0] ^ Cipher[CurrXor]), 2));
			SetBit(ref ringOrGame, 0, GetBit((byte)(secret[0] ^ Cipher[CurrXor]), 1));

			if (ringOrGame != 0)
				throw new ArgumentException("The specified data is not a game code", "secret");

			SetBit(ref _gameId, 0, GetBit((byte)(secret[0] ^ Cipher[CurrXor++]), 0));

			SetBit(ref _gameId, 1, GetBit((byte)(secret[1] ^ Cipher[CurrXor]), 5));
			SetBit(ref _gameId, 2, GetBit((byte)(secret[1] ^ Cipher[CurrXor]), 4));
			SetBit(ref _gameId, 3, GetBit((byte)(secret[1] ^ Cipher[CurrXor]), 3));
			SetBit(ref _gameId, 4, GetBit((byte)(secret[1] ^ Cipher[CurrXor]), 2));
			SetBit(ref _gameId, 5, GetBit((byte)(secret[1] ^ Cipher[CurrXor]), 1));
			SetBit(ref _gameId, 6, GetBit((byte)(secret[1] ^ Cipher[CurrXor++]), 0));

			SetBit(ref _gameId, 7, GetBit((byte)(secret[2] ^ Cipher[CurrXor]), 5));
			SetBit(ref _gameId, 8, GetBit((byte)(secret[2] ^ Cipher[CurrXor]), 4));
			SetBit(ref _gameId, 9, GetBit((byte)(secret[2] ^ Cipher[CurrXor]), 3));
			SetBit(ref _gameId, 10, GetBit((byte)(secret[2] ^ Cipher[CurrXor]), 2));
			SetBit(ref _gameId, 11, GetBit((byte)(secret[2] ^ Cipher[CurrXor]), 1));
			SetBit(ref _gameId, 12, GetBit((byte)(secret[2] ^ Cipher[CurrXor++]), 0));

			SetBit(ref _gameId, 13, GetBit((byte)(secret[3] ^ Cipher[CurrXor]), 5));
			SetBit(ref _gameId, 14, GetBit((byte)(secret[3] ^ Cipher[CurrXor]), 4));
			SetBit(ref _linkedHeros, 0, GetBit((byte)(secret[3] ^ Cipher[CurrXor]), 3));
			SetBit(ref _agesSeasons, 0, GetBit((byte)(secret[3] ^ Cipher[CurrXor]), 2));
			SetBit(ref _hero, 0, 0, GetBit((byte)(secret[3] ^ Cipher[CurrXor]), 1));
			SetBit(ref _hero, 0, 1, GetBit((byte)(secret[3] ^ Cipher[CurrXor++]), 0));

			SetBit(ref _hero, 0, 2, GetBit((byte)(secret[4] ^ Cipher[CurrXor]), 5));
			SetBit(ref _hero, 0, 3, GetBit((byte)(secret[4] ^ Cipher[CurrXor]), 4));
			SetBit(ref _hero, 0, 4, GetBit((byte)(secret[4] ^ Cipher[CurrXor]), 3));
			SetBit(ref _hero, 0, 5, GetBit((byte)(secret[4] ^ Cipher[CurrXor]), 2));
			SetBit(ref _hero, 0, 6, GetBit((byte)(secret[4] ^ Cipher[CurrXor]), 1));
			SetBit(ref _hero, 0, 7, GetBit((byte)(secret[4] ^ Cipher[CurrXor++]), 0));

			SetBit(ref _child, 0, 0, GetBit((byte)(secret[5] ^ Cipher[CurrXor]), 5));
			SetBit(ref _child, 0, 1, GetBit((byte)(secret[5] ^ Cipher[CurrXor]), 4));
			SetBit(ref _child, 0, 2, GetBit((byte)(secret[5] ^ Cipher[CurrXor]), 3));
			SetBit(ref _child, 0, 3, GetBit((byte)(secret[5] ^ Cipher[CurrXor]), 2));
			SetBit(ref _child, 0, 4, GetBit((byte)(secret[5] ^ Cipher[CurrXor]), 1));
			SetBit(ref _child, 0, 5, GetBit((byte)(secret[5] ^ Cipher[CurrXor++]), 0));

			SetBit(ref _child, 0, 6, GetBit((byte)(secret[6] ^ Cipher[CurrXor]), 5));
			SetBit(ref _child, 0, 7, GetBit((byte)(secret[6] ^ Cipher[CurrXor]), 4));
			SetBit(ref _hero, 1, 0, GetBit((byte)(secret[6] ^ Cipher[CurrXor]), 3));
			SetBit(ref _hero, 1, 1, GetBit((byte)(secret[6] ^ Cipher[CurrXor]), 2));
			SetBit(ref _hero, 1, 2, GetBit((byte)(secret[6] ^ Cipher[CurrXor]), 1));
			SetBit(ref _hero, 1, 3, GetBit((byte)(secret[6] ^ Cipher[CurrXor++]), 0));

			SetBit(ref _hero, 1, 4, GetBit((byte)(secret[7] ^ Cipher[CurrXor]), 5));
			SetBit(ref _hero, 1, 5, GetBit((byte)(secret[7] ^ Cipher[CurrXor]), 4));
			SetBit(ref _hero, 1, 6, GetBit((byte)(secret[7] ^ Cipher[CurrXor]), 3));
			SetBit(ref _hero, 1, 7, GetBit((byte)(secret[7] ^ Cipher[CurrXor]), 2));
			SetBit(ref _child, 1, 0, GetBit((byte)(secret[7] ^ Cipher[CurrXor]), 1));
			SetBit(ref _child, 1, 1, GetBit((byte)(secret[7] ^ Cipher[CurrXor++]), 0));

			SetBit(ref _child, 1, 2, GetBit((byte)(secret[8] ^ Cipher[CurrXor]), 5));
			SetBit(ref _child, 1, 3, GetBit((byte)(secret[8] ^ Cipher[CurrXor]), 4));
			SetBit(ref _child, 1, 4, GetBit((byte)(secret[8] ^ Cipher[CurrXor]), 3));
			SetBit(ref _child, 1, 5, GetBit((byte)(secret[8] ^ Cipher[CurrXor]), 2));
			SetBit(ref _child, 1, 6, GetBit((byte)(secret[8] ^ Cipher[CurrXor]), 1));
			SetBit(ref _child, 1, 7, GetBit((byte)(secret[8] ^ Cipher[CurrXor++]), 0));

			byte unknown2 = 0;
			byte unknown3 = 0;

			SetBit(ref _behavior, 0, GetBit((byte)(secret[9] ^ Cipher[CurrXor]), 5));
			SetBit(ref _behavior, 1, GetBit((byte)(secret[9] ^ Cipher[CurrXor]), 4));
			SetBit(ref _behavior, 2, GetBit((byte)(secret[9] ^ Cipher[CurrXor]), 3));
			SetBit(ref _behavior, 3, GetBit((byte)(secret[9] ^ Cipher[CurrXor]), 2));
			SetBit(ref unknown2, 0, GetBit((byte)(secret[9] ^ Cipher[CurrXor]), 1));
			SetBit(ref unknown3, 0, GetBit((byte)(secret[9] ^ Cipher[CurrXor++]), 0));

			SetBit(ref _hero, 2, 0, GetBit((byte)(secret[10] ^ Cipher[CurrXor]), 5));
			SetBit(ref _hero, 2, 1, GetBit((byte)(secret[10] ^ Cipher[CurrXor]), 4));
			SetBit(ref _hero, 2, 2, GetBit((byte)(secret[10] ^ Cipher[CurrXor]), 3));
			SetBit(ref _hero, 2, 3, GetBit((byte)(secret[10] ^ Cipher[CurrXor]), 2));
			SetBit(ref _hero, 2, 4, GetBit((byte)(secret[10] ^ Cipher[CurrXor]), 1));
			SetBit(ref _hero, 2, 5, GetBit((byte)(secret[10] ^ Cipher[CurrXor++]), 0));

			SetBit(ref _hero, 2, 6, GetBit((byte)(secret[11] ^ Cipher[CurrXor]), 5));
			SetBit(ref _hero, 2, 7, GetBit((byte)(secret[11] ^ Cipher[CurrXor]), 4));
			SetBit(ref _child, 2, 0, GetBit((byte)(secret[11] ^ Cipher[CurrXor]), 3));
			SetBit(ref _child, 2, 1, GetBit((byte)(secret[11] ^ Cipher[CurrXor]), 2));
			SetBit(ref _child, 2, 2, GetBit((byte)(secret[11] ^ Cipher[CurrXor]), 1));
			SetBit(ref _child, 2, 3, GetBit((byte)(secret[11] ^ Cipher[CurrXor++]), 0));

			byte unknown4 = 0;

			SetBit(ref _child, 2, 4, GetBit((byte)(secret[12] ^ Cipher[CurrXor]), 5));
			SetBit(ref _child, 2, 5, GetBit((byte)(secret[12] ^ Cipher[CurrXor]), 4));
			SetBit(ref _child, 2, 6, GetBit((byte)(secret[12] ^ Cipher[CurrXor]), 3));
			SetBit(ref _child, 2, 7, GetBit((byte)(secret[12] ^ Cipher[CurrXor]), 2));
			SetBit(ref unknown4, 0, GetBit((byte)(secret[12] ^ Cipher[CurrXor]), 1));
			SetBit(ref _hero, 3, 0, GetBit((byte)(secret[12] ^ Cipher[CurrXor++]), 0));

			SetBit(ref _hero, 3, 1, GetBit((byte)(secret[13] ^ Cipher[CurrXor]), 5));
			SetBit(ref _hero, 3, 2, GetBit((byte)(secret[13] ^ Cipher[CurrXor]), 4));
			SetBit(ref _hero, 3, 3, GetBit((byte)(secret[13] ^ Cipher[CurrXor]), 3));
			SetBit(ref _hero, 3, 4, GetBit((byte)(secret[13] ^ Cipher[CurrXor]), 2));
			SetBit(ref _hero, 3, 5, GetBit((byte)(secret[13] ^ Cipher[CurrXor]), 1));
			SetBit(ref _hero, 3, 6, GetBit((byte)(secret[13] ^ Cipher[CurrXor++]), 0));

			byte unknown5 = 0;

			SetBit(ref _hero, 3, 7, GetBit((byte)(secret[14] ^ Cipher[CurrXor]), 5));
			SetBit(ref _animal, 0, GetBit((byte)(secret[14] ^ Cipher[CurrXor]), 4));
			SetBit(ref _animal, 1, GetBit((byte)(secret[14] ^ Cipher[CurrXor]), 3));
			SetBit(ref _animal, 2, GetBit((byte)(secret[14] ^ Cipher[CurrXor]), 2));
			SetBit(ref unknown5, 0, GetBit((byte)(secret[14] ^ Cipher[CurrXor]), 1));
			SetBit(ref _hero, 4, 1, GetBit((byte)(secret[14] ^ Cipher[CurrXor++]), 0));

			SetBit(ref _hero, 4, 1, GetBit((byte)(secret[15] ^ Cipher[CurrXor]), 5));
			SetBit(ref _hero, 4, 2, GetBit((byte)(secret[15] ^ Cipher[CurrXor]), 4));
			SetBit(ref _hero, 4, 3, GetBit((byte)(secret[15] ^ Cipher[CurrXor]), 3));
			SetBit(ref _hero, 4, 4, GetBit((byte)(secret[15] ^ Cipher[CurrXor]), 2));
			SetBit(ref _hero, 4, 5, GetBit((byte)(secret[15] ^ Cipher[CurrXor]), 1));
			SetBit(ref _hero, 4, 6, GetBit((byte)(secret[15] ^ Cipher[CurrXor++]), 0));

			SetBit(ref _hero, 4, 7, GetBit((byte)(secret[16] ^ Cipher[CurrXor]), 5));
			SetBit(ref _child, 3, 0, GetBit((byte)(secret[16] ^ Cipher[CurrXor]), 4));
			SetBit(ref _child, 3, 1, GetBit((byte)(secret[16] ^ Cipher[CurrXor]), 3));
			SetBit(ref _child, 3, 2, GetBit((byte)(secret[16] ^ Cipher[CurrXor]), 2));
			SetBit(ref _child, 3, 3, GetBit((byte)(secret[16] ^ Cipher[CurrXor]), 1));
			SetBit(ref _child, 3, 4, GetBit((byte)(secret[16] ^ Cipher[CurrXor++]), 0));

			byte unknown6 = 0;

			SetBit(ref _child, 3, 5, GetBit((byte)(secret[17] ^ Cipher[CurrXor]), 5));
			SetBit(ref _child, 3, 6, GetBit((byte)(secret[17] ^ Cipher[CurrXor]), 4));
			SetBit(ref _child, 3, 7, GetBit((byte)(secret[17] ^ Cipher[CurrXor]), 3));
			SetBit(ref unknown6, 0, GetBit((byte)(secret[17] ^ Cipher[CurrXor]), 2));
			SetBit(ref _child, 4, 0, GetBit((byte)(secret[17] ^ Cipher[CurrXor]), 1));
			SetBit(ref _child, 4, 1, GetBit((byte)(secret[17] ^ Cipher[CurrXor++]), 0));

			SetBit(ref _child, 4, 2, GetBit((byte)(secret[18] ^ Cipher[CurrXor]), 5));
			SetBit(ref _child, 4, 3, GetBit((byte)(secret[18] ^ Cipher[CurrXor]), 4));
			SetBit(ref _child, 4, 4, GetBit((byte)(secret[18] ^ Cipher[CurrXor]), 3));
			SetBit(ref _child, 4, 5, GetBit((byte)(secret[18] ^ Cipher[CurrXor]), 2));
			SetBit(ref _child, 4, 6, GetBit((byte)(secret[18] ^ Cipher[CurrXor]), 1));
			SetBit(ref _child, 4, 7, GetBit((byte)(secret[18] ^ Cipher[CurrXor++]), 0));

			// TODO: Figure out what all the unknown values are for.

			// TODO: Validate checksum
			byte checksum = (byte)(secret[19] ^ Cipher[CurrXor]);
		}

		#endregion // Secret parsing logic

		public void OnPropertyChanged(string propertyName)
		{
			if (PropertyChanged != null)
			{
				PropertyChanged(this,
					new PropertyChangedEventArgs(propertyName));
			}
		}

		#region Bit manipulation helpers

		// TODO: Consider moving these to a separate class

		public static bool GetBit(byte b, int bitNumber)
		{
			return (b & (1 << bitNumber)) != 0;
		}

		public static bool GetBit(short b, int bitNumber)
		{
			return (b & (1 << bitNumber)) != 0;
		}

		public static bool GetBit(char b, int bitNumber)
		{
			return (b & (1 << bitNumber)) != 0;
		}

		public static bool GetBit(string b, int charNumber, int bitNumber)
		{
			byte[] chars = System.Text.Encoding.ASCII.GetBytes(b);
			return GetBit(chars[charNumber], bitNumber);
		}

		public static void SetBit(ref byte b, int bitNumber, bool value)
		{
			if (value)
				b = (byte)((1 << bitNumber) | b);
			else
				b = (byte)((byte.MaxValue ^ (1 << bitNumber)) & b);
		}

		public static void SetBit(ref short b, int bitNumber, bool value)
		{
			if (value)
				b = (short)((1 << bitNumber) | (ushort)b);
			else
				b = (short)((ushort.MaxValue ^ (1 << bitNumber)) & (ushort)b);
		}

		public static void SetBit(ref char b, int bitNumber, bool value)
		{
			if (value)
				b = (char)((1 << bitNumber) | b);
			else
				b = (char)((char.MaxValue ^ (1 << bitNumber)) & b);
		}

		public static void SetBit(ref string b, int charNumber, int bitNumber, bool value)
		{
			byte[] chars = System.Text.Encoding.ASCII.GetBytes(b);
			byte c = chars[charNumber];
			SetBit(ref c, bitNumber, value);
			chars[charNumber] = c;
			b = System.Text.Encoding.ASCII.GetString(chars);
		}

		#endregion // Bit manipulation helpers
	}
}
