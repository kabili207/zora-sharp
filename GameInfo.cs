using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Zyrenth.OracleHack
{
	public class GameInfo
	{

		private static readonly byte[] Cipher =
		{ 7, 35, 46, 4, 13, 63, 26, 16, 58, 47, 30,
			32, 15, 62, 54, 55, 9, 41, 59, 49, 2,
			22, 61, 56, 40, 19, 52, 50, 1, 11, 10,
			53, 14, 27, 18, 44, 33, 45, 37, 48, 25,
			42, 6, 57, 60, 23, 51 };

		short gameId = 0;
		byte cipherStart = 0;
		string hero = "     ";
		string child = "     ";
		byte behavior = 0;
		byte animal = 0;
		byte linkedHeros = 0;
		byte agesSeasons = 0;

		byte _currXor = 0;

		public Game Game
		{
			get { return (Game)agesSeasons; }
			set { agesSeasons = (byte)value; }
		}

		public Quest Quest
		{
			get { return (Quest)linkedHeros; }
			set { linkedHeros = (byte)Quest; }
		}

		public short GameID
		{
			get { return gameId; }
			set { gameId = value; }
		}

		public string Hero
		{
			get { return hero.Trim(' ', '\0'); }
			set
			{
				if (string.IsNullOrWhiteSpace(value))
					hero = "    ";
				else
					hero = value.TrimEnd().PadRight(5, '\0');
			}
		}

		public string Child
		{
			get { return child.Trim(' ', '\0'); }
			set
			{
				if (string.IsNullOrWhiteSpace(value))
					child = "    ";
				else
					child = value.TrimEnd().PadRight(5, '\0');
			}
		}

		public Animal Animal
		{
			get { return (Animal)animal; }
			set { animal = (byte)value; }
		}

		public ChildBehavior Behavior
		{
			get { return (ChildBehavior)behavior; }
			set { behavior = (byte)value; }
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

		public static GameInfo FromGameSecret(byte[] secret)
		{
			GameInfo parser = new GameInfo();
			parser.ReadGameSecret(secret);
			return parser;
		}

		// Logic taken from http://www.gamefaqs.com/boards/472313-the-legend-of-zelda-oracle-of-ages/66934363
		public void ReadGameSecret(byte[] secret)
		{
			if (secret == null || secret.Length != 20)
				throw new ArgumentException("Secret must contatin exactly 20 bytes", "secret");


			byte ringCode = 0;

			// unknowns
			byte unknown1 = 0;

			SetBit(ref cipherStart, 2, GetBit(secret[0], 5));
			SetBit(ref cipherStart, 1, GetBit(secret[0], 4));
			SetBit(ref cipherStart, 0, GetBit(secret[0], 3));

			CurrXor = (byte)(cipherStart * 4);

			SetBit(ref unknown1, 0, GetBit((byte)(secret[0] ^ Cipher[CurrXor]), 2));
			SetBit(ref ringCode, 0, GetBit((byte)(secret[0] ^ Cipher[CurrXor]), 1));
			SetBit(ref gameId, 0, GetBit((byte)(secret[0] ^ Cipher[CurrXor++]), 0));

			SetBit(ref gameId, 1, GetBit((byte)(secret[1] ^ Cipher[CurrXor]), 5));
			SetBit(ref gameId, 2, GetBit((byte)(secret[1] ^ Cipher[CurrXor]), 4));
			SetBit(ref gameId, 3, GetBit((byte)(secret[1] ^ Cipher[CurrXor]), 3));
			SetBit(ref gameId, 4, GetBit((byte)(secret[1] ^ Cipher[CurrXor]), 2));
			SetBit(ref gameId, 5, GetBit((byte)(secret[1] ^ Cipher[CurrXor]), 1));
			SetBit(ref gameId, 6, GetBit((byte)(secret[1] ^ Cipher[CurrXor++]), 0));

			SetBit(ref gameId, 7, GetBit((byte)(secret[2] ^ Cipher[CurrXor]), 5));
			SetBit(ref gameId, 8, GetBit((byte)(secret[2] ^ Cipher[CurrXor]), 4));
			SetBit(ref gameId, 9, GetBit((byte)(secret[2] ^ Cipher[CurrXor]), 3));
			SetBit(ref gameId, 10, GetBit((byte)(secret[2] ^ Cipher[CurrXor]), 2));
			SetBit(ref gameId, 11, GetBit((byte)(secret[2] ^ Cipher[CurrXor]), 1));
			SetBit(ref gameId, 12, GetBit((byte)(secret[2] ^ Cipher[CurrXor++]), 0));


			SetBit(ref gameId, 13, GetBit((byte)(secret[3] ^ Cipher[CurrXor]), 5));
			SetBit(ref gameId, 14, GetBit((byte)(secret[3] ^ Cipher[CurrXor]), 4));
			SetBit(ref linkedHeros, 0, GetBit((byte)(secret[3] ^ Cipher[CurrXor]), 3));
			SetBit(ref agesSeasons, 0, GetBit((byte)(secret[3] ^ Cipher[CurrXor]), 2));
			SetBit(ref hero, 0, 0, GetBit((byte)(secret[3] ^ Cipher[CurrXor]), 1));
			SetBit(ref hero, 0, 1, GetBit((byte)(secret[3] ^ Cipher[CurrXor++]), 0));

			SetBit(ref hero, 0, 2, GetBit((byte)(secret[4] ^ Cipher[CurrXor]), 5));
			SetBit(ref hero, 0, 3, GetBit((byte)(secret[4] ^ Cipher[CurrXor]), 4));
			SetBit(ref hero, 0, 4, GetBit((byte)(secret[4] ^ Cipher[CurrXor]), 3));
			SetBit(ref hero, 0, 5, GetBit((byte)(secret[4] ^ Cipher[CurrXor]), 2));
			SetBit(ref hero, 0, 6, GetBit((byte)(secret[4] ^ Cipher[CurrXor]), 1));
			SetBit(ref hero, 0, 7, GetBit((byte)(secret[4] ^ Cipher[CurrXor++]), 0));

			SetBit(ref child, 0, 0, GetBit((byte)(secret[5] ^ Cipher[CurrXor]), 5));
			SetBit(ref child, 0, 1, GetBit((byte)(secret[5] ^ Cipher[CurrXor]), 4));
			SetBit(ref child, 0, 2, GetBit((byte)(secret[5] ^ Cipher[CurrXor]), 3));
			SetBit(ref child, 0, 3, GetBit((byte)(secret[5] ^ Cipher[CurrXor]), 2));
			SetBit(ref child, 0, 4, GetBit((byte)(secret[5] ^ Cipher[CurrXor]), 1));
			SetBit(ref child, 0, 5, GetBit((byte)(secret[5] ^ Cipher[CurrXor++]), 0));

			SetBit(ref child, 0, 6, GetBit((byte)(secret[6] ^ Cipher[CurrXor]), 5));
			SetBit(ref child, 0, 7, GetBit((byte)(secret[6] ^ Cipher[CurrXor]), 4));
			SetBit(ref hero, 1, 0, GetBit((byte)(secret[6] ^ Cipher[CurrXor]), 3));
			SetBit(ref hero, 1, 1, GetBit((byte)(secret[6] ^ Cipher[CurrXor]), 2));
			SetBit(ref hero, 1, 2, GetBit((byte)(secret[6] ^ Cipher[CurrXor]), 1));
			SetBit(ref hero, 1, 3, GetBit((byte)(secret[6] ^ Cipher[CurrXor++]), 0));

			SetBit(ref hero, 1, 4, GetBit((byte)(secret[7] ^ Cipher[CurrXor]), 5));
			SetBit(ref hero, 1, 5, GetBit((byte)(secret[7] ^ Cipher[CurrXor]), 4));
			SetBit(ref hero, 1, 6, GetBit((byte)(secret[7] ^ Cipher[CurrXor]), 3));
			SetBit(ref hero, 1, 7, GetBit((byte)(secret[7] ^ Cipher[CurrXor]), 2));
			SetBit(ref child, 1, 0, GetBit((byte)(secret[7] ^ Cipher[CurrXor]), 1));
			SetBit(ref child, 1, 1, GetBit((byte)(secret[7] ^ Cipher[CurrXor++]), 0));

			SetBit(ref child, 1, 2, GetBit((byte)(secret[8] ^ Cipher[CurrXor]), 5));
			SetBit(ref child, 1, 3, GetBit((byte)(secret[8] ^ Cipher[CurrXor]), 4));
			SetBit(ref child, 1, 4, GetBit((byte)(secret[8] ^ Cipher[CurrXor]), 3));
			SetBit(ref child, 1, 5, GetBit((byte)(secret[8] ^ Cipher[CurrXor]), 2));
			SetBit(ref child, 1, 6, GetBit((byte)(secret[8] ^ Cipher[CurrXor]), 1));
			SetBit(ref child, 1, 7, GetBit((byte)(secret[8] ^ Cipher[CurrXor++]), 0));

			byte unknown2 = 0;
			byte unknown3 = 0;

			SetBit(ref behavior, 0, GetBit((byte)(secret[9] ^ Cipher[CurrXor]), 5));
			SetBit(ref behavior, 1, GetBit((byte)(secret[9] ^ Cipher[CurrXor]), 4));
			SetBit(ref behavior, 2, GetBit((byte)(secret[9] ^ Cipher[CurrXor]), 3));
			SetBit(ref behavior, 3, GetBit((byte)(secret[9] ^ Cipher[CurrXor]), 2));
			SetBit(ref unknown2, 0, GetBit((byte)(secret[9] ^ Cipher[CurrXor]), 1));
			SetBit(ref unknown3, 0, GetBit((byte)(secret[9] ^ Cipher[CurrXor++]), 0));

			SetBit(ref hero, 2, 0, GetBit((byte)(secret[10] ^ Cipher[CurrXor]), 5));
			SetBit(ref hero, 2, 1, GetBit((byte)(secret[10] ^ Cipher[CurrXor]), 4));
			SetBit(ref hero, 2, 2, GetBit((byte)(secret[10] ^ Cipher[CurrXor]), 3));
			SetBit(ref hero, 2, 3, GetBit((byte)(secret[10] ^ Cipher[CurrXor]), 2));
			SetBit(ref hero, 2, 4, GetBit((byte)(secret[10] ^ Cipher[CurrXor]), 1));
			SetBit(ref hero, 2, 5, GetBit((byte)(secret[10] ^ Cipher[CurrXor++]), 0));

			SetBit(ref hero, 2, 6, GetBit((byte)(secret[11] ^ Cipher[CurrXor]), 5));
			SetBit(ref hero, 2, 7, GetBit((byte)(secret[11] ^ Cipher[CurrXor]), 4));
			SetBit(ref child, 2, 0, GetBit((byte)(secret[11] ^ Cipher[CurrXor]), 3));
			SetBit(ref child, 2, 1, GetBit((byte)(secret[11] ^ Cipher[CurrXor]), 2));
			SetBit(ref child, 2, 2, GetBit((byte)(secret[11] ^ Cipher[CurrXor]), 1));
			SetBit(ref child, 2, 3, GetBit((byte)(secret[11] ^ Cipher[CurrXor++]), 0));

			byte unknown4 = 0;

			SetBit(ref child, 2, 4, GetBit((byte)(secret[12] ^ Cipher[CurrXor]), 5));
			SetBit(ref child, 2, 5, GetBit((byte)(secret[12] ^ Cipher[CurrXor]), 4));
			SetBit(ref child, 2, 6, GetBit((byte)(secret[12] ^ Cipher[CurrXor]), 3));
			SetBit(ref child, 2, 7, GetBit((byte)(secret[12] ^ Cipher[CurrXor]), 2));
			SetBit(ref unknown4, 0, GetBit((byte)(secret[12] ^ Cipher[CurrXor]), 1));
			SetBit(ref hero, 3, 0, GetBit((byte)(secret[12] ^ Cipher[CurrXor++]), 0));

			SetBit(ref hero, 3, 1, GetBit((byte)(secret[13] ^ Cipher[CurrXor]), 5));
			SetBit(ref hero, 3, 2, GetBit((byte)(secret[13] ^ Cipher[CurrXor]), 4));
			SetBit(ref hero, 3, 3, GetBit((byte)(secret[13] ^ Cipher[CurrXor]), 3));
			SetBit(ref hero, 3, 4, GetBit((byte)(secret[13] ^ Cipher[CurrXor]), 2));
			SetBit(ref hero, 3, 5, GetBit((byte)(secret[13] ^ Cipher[CurrXor]), 1));
			SetBit(ref hero, 3, 6, GetBit((byte)(secret[13] ^ Cipher[CurrXor++]), 0));

			byte unknown5 = 0;

			SetBit(ref hero, 3, 7, GetBit((byte)(secret[14] ^ Cipher[CurrXor]), 5));
			SetBit(ref animal, 0, GetBit((byte)(secret[14] ^ Cipher[CurrXor]), 4));
			SetBit(ref animal, 1, GetBit((byte)(secret[14] ^ Cipher[CurrXor]), 3));
			SetBit(ref animal, 2, GetBit((byte)(secret[14] ^ Cipher[CurrXor]), 2));
			SetBit(ref unknown5, 0, GetBit((byte)(secret[14] ^ Cipher[CurrXor]), 1));
			SetBit(ref hero, 4, 1, GetBit((byte)(secret[14] ^ Cipher[CurrXor++]), 0));

			SetBit(ref hero, 4, 1, GetBit((byte)(secret[15] ^ Cipher[CurrXor]), 5));
			SetBit(ref hero, 4, 2, GetBit((byte)(secret[15] ^ Cipher[CurrXor]), 4));
			SetBit(ref hero, 4, 3, GetBit((byte)(secret[15] ^ Cipher[CurrXor]), 3));
			SetBit(ref hero, 4, 4, GetBit((byte)(secret[15] ^ Cipher[CurrXor]), 2));
			SetBit(ref hero, 4, 5, GetBit((byte)(secret[15] ^ Cipher[CurrXor]), 1));
			SetBit(ref hero, 4, 6, GetBit((byte)(secret[15] ^ Cipher[CurrXor++]), 0));

			SetBit(ref hero, 4, 7, GetBit((byte)(secret[16] ^ Cipher[CurrXor]), 5));
			SetBit(ref child, 3, 0, GetBit((byte)(secret[16] ^ Cipher[CurrXor]), 4));
			SetBit(ref child, 3, 1, GetBit((byte)(secret[16] ^ Cipher[CurrXor]), 3));
			SetBit(ref child, 3, 2, GetBit((byte)(secret[16] ^ Cipher[CurrXor]), 2));
			SetBit(ref child, 3, 3, GetBit((byte)(secret[16] ^ Cipher[CurrXor]), 1));
			SetBit(ref child, 3, 4, GetBit((byte)(secret[16] ^ Cipher[CurrXor++]), 0));

			byte unknown6 = 0;

			SetBit(ref child, 3, 5, GetBit((byte)(secret[17] ^ Cipher[CurrXor]), 5));
			SetBit(ref child, 3, 6, GetBit((byte)(secret[17] ^ Cipher[CurrXor]), 4));
			SetBit(ref child, 3, 7, GetBit((byte)(secret[17] ^ Cipher[CurrXor]), 3));
			SetBit(ref unknown6, 0, GetBit((byte)(secret[17] ^ Cipher[CurrXor]), 2));
			SetBit(ref child, 4, 0, GetBit((byte)(secret[17] ^ Cipher[CurrXor]), 1));
			SetBit(ref child, 4, 1, GetBit((byte)(secret[17] ^ Cipher[CurrXor++]), 0));

			SetBit(ref child, 4, 2, GetBit((byte)(secret[18] ^ Cipher[CurrXor]), 5));
			SetBit(ref child, 4, 3, GetBit((byte)(secret[18] ^ Cipher[CurrXor]), 4));
			SetBit(ref child, 4, 4, GetBit((byte)(secret[18] ^ Cipher[CurrXor]), 3));
			SetBit(ref child, 4, 5, GetBit((byte)(secret[18] ^ Cipher[CurrXor]), 2));
			SetBit(ref child, 4, 6, GetBit((byte)(secret[18] ^ Cipher[CurrXor]), 1));
			SetBit(ref child, 4, 7, GetBit((byte)(secret[18] ^ Cipher[CurrXor++]), 0));

			// TODO: Validate checksum

		}


		public static bool GetBit(byte b, int bitNumber)
		{
			return (b & (1 << bitNumber)) != 0;
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


	}
}
