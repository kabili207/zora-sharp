using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;

namespace Zyrenth.OracleHack
{
	/// <summary>
	/// Represents the user data for an idividual game
	/// </summary>
	[Serializable]
	[JsonObject(MemberSerialization.OptIn)]
	public class GameInfo : INotifyPropertyChanged
	{

		private static readonly byte[] Cipher =
		{ 
			 7, 35, 46,  4, 13, 63, 26, 16,
			58, 47, 30, 32, 15, 62, 54, 55,
			 9, 41, 59, 49,  2, 22, 61, 56, 
			40, 19, 52, 50,  1, 11, 10, 53,
			14, 27, 18, 44, 33, 45, 37, 48,
			25, 42,  6, 57, 60, 23, 51
		};

		#region Fields


		string _hero = "     ";
		string _child = "     ";

		// We want to serialize the underlying fields instead of the
		// friendier properties. This will make interopability with
		// other programs easier.
		[JsonProperty("GameID")]
		short _gameId = 0;
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

		#endregion // Fields

		/// <summary>
		/// Occurs when a property has changed
		/// </summary>
		[field: NonSerialized]
		public event PropertyChangedEventHandler PropertyChanged;

		#region Properties

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
		public Quest Quest
		{
			get { return (Quest)_linkedHeros; }
			set
			{
				_linkedHeros = (byte)value;
				OnPropertyChanged("Quest");
			}
		}

		/// <summary>
		/// Gets or sets the unique game ID 
		/// </summary>
		public short GameID
		{
			get { return _gameId; }
			set
			{
				_gameId = value;
				OnPropertyChanged("GameID");
			}
		}

		/// <summary>
		/// Gets or sets the hero's name
		/// </summary>
		[JsonProperty]
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

		/// <summary>
		/// Gets or sets the child's name
		/// </summary>
		[JsonProperty]
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
		/// Gets or sets the user's ring collection
		/// </summary>
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

		protected void OnPropertyChanged(string propertyName)
		{
			if (PropertyChanged != null)
			{
				PropertyChanged(this,
					new PropertyChangedEventArgs(propertyName));
			}
		}

		/// <summary>
		/// Creates a GameInfo instance from a game secret data
		/// </summary>
		/// <param name="secret">The raw secret data</param>
		/// <returns>A GameInfo object containing data from the secret</returns>
		public static GameInfo FromGameSecret(byte[] secret)
		{
			GameInfo parser = new GameInfo();
			parser.LoadGameData(secret);
			return parser;
		}

		#region Secret parsing logic

		private string DecodeBytes(byte[] secret)
		{
			CurrXor = (byte)((secret[0] >> 3) * 4);

			byte currentByte = 0;
			string data = "";
			foreach (byte b in secret)
			{
				currentByte = (byte)(b ^ Cipher[CurrXor++]);
				data += Convert.ToString(currentByte, 2).PadLeft(6, '0');
			}

			return data;
		}

		/// <summary>
		/// Loads the specified <paramref name="secret"/> data into this GameInfo
		/// </summary>
		/// <param name="secret">The raw secret data</param>
		public void LoadGameData(byte[] secret)
		{
			if (secret == null || secret.Length != 20)
				throw new ArgumentException("Secret must contatin exactly 20 bytes", "secret");

			string decodedSecret = DecodeBytes(secret);

			_gameId = Convert.ToInt16(decodedSecret.ReversedSubstring(5, 15), 2);

			bool isGameCode = decodedSecret[4] == '0';

			// TODO: This value alone doesn't seem to specify the secret type.
			//if(!isGameCode)
			//	throw new ArgumentException("The specified data is not a game code", "secret");

			_linkedHeros = (byte)(decodedSecret[20] == '1' ? 1 : 0);
			_agesSeasons = (byte)(decodedSecret[21] == '1' ? 1 : 0);


			_hero = System.Text.Encoding.ASCII.GetString(new byte[] {
				Convert.ToByte(decodedSecret.ReversedSubstring(22, 8), 2),
				Convert.ToByte(decodedSecret.ReversedSubstring(38, 8), 2),
				Convert.ToByte(decodedSecret.ReversedSubstring(60, 8), 2),
				Convert.ToByte(decodedSecret.ReversedSubstring(77, 8), 2),
				Convert.ToByte(decodedSecret.ReversedSubstring(89, 8), 2)
			});

			_child = System.Text.Encoding.ASCII.GetString(new byte[] {
				Convert.ToByte(decodedSecret.ReversedSubstring(30, 8), 2),
				Convert.ToByte(decodedSecret.ReversedSubstring(46, 8), 2),
				Convert.ToByte(decodedSecret.ReversedSubstring(68, 8), 2),
				Convert.ToByte(decodedSecret.ReversedSubstring(97, 8), 2),
				Convert.ToByte(decodedSecret.ReversedSubstring(106, 8), 2)
			});

			_animal = Convert.ToByte(decodedSecret.ReversedSubstring(85, 3), 2);
			_behavior = Convert.ToByte(decodedSecret.ReversedSubstring(54, 4), 2);


			// TODO: Figure out what all the unknown values are for.
			bool unknown1 = decodedSecret[3] == '1';
			bool unknown2 = decodedSecret[58] == '1';
			bool unknown3 = decodedSecret[59] == '1';
			bool unknown4 = decodedSecret[76] == '1';
			bool unknown5 = decodedSecret[88] == '1';
			

			// TODO: Validate checksum
			byte checksum = secret[19];

			OnPropertyChanged("Hero");
			OnPropertyChanged("Child");
			OnPropertyChanged("GameID");
			OnPropertyChanged("Animal");
			OnPropertyChanged("Behavior");
			OnPropertyChanged("Game");
			OnPropertyChanged("Quest");
		}

		/// <summary>
		/// Sets the <see cref="Rings"/> property using the specified secret
		/// </summary>
		/// <param name="secret">The raw secret data</param>
		public void LoadRings(byte[] secret)
		{
			if (secret == null || secret.Length != 15)
				throw new ArgumentException("Secret must contatin exactly 15 bytes", "secret");

			string decodedSecret = DecodeBytes(secret);

			bool isRingCode = decodedSecret[4] == '1';

			// TODO: This value alone doesn't seem to specify the secret type.
			//if(!isGameCode)
			//	throw new ArgumentException("The specified data is not a ring code", "secret");

			short gameId = Convert.ToInt16(decodedSecret.ReversedSubstring(5, 15), 2);

			_rings = Convert.ToUInt64(
				decodedSecret.ReversedSubstring(36, 8) +
				decodedSecret.ReversedSubstring(76, 8) +
				decodedSecret.ReversedSubstring(28, 8) +
				decodedSecret.ReversedSubstring(60, 8) +
				decodedSecret.ReversedSubstring(44, 8) +
				decodedSecret.ReversedSubstring(68, 8) +
				decodedSecret.ReversedSubstring(20, 8) +
				decodedSecret.ReversedSubstring(52, 8)
				, 2);

			// TODO: Figure out what all the unknown values are for.
			bool unknown1 = decodedSecret[3] == '1';

			OnPropertyChanged("Rings");

			// TODO: Validate checksum
			byte checksum = secret[14];
		}
		
		public Memory ReadMemorySecret(byte[] secret)
		{
			if (secret == null || secret.Length != 5)
				throw new ArgumentException("Secret must contatin exactly 5 bytes", "secret");

			string decodedSecret = DecodeBytes(secret);

			short gameId = Convert.ToInt16(decodedSecret.ReversedSubstring(5, 15), 2);


			byte memoryCode = Convert.ToByte(decodedSecret.ReversedSubstring(20, 4), 2);

			// TODO: Figure out what all the unknown values are for.
			bool unknown1 = decodedSecret[3] == '1';
			bool unknown2 = decodedSecret[4] == '0';

			// TODO: Checksum
			byte checksum = secret[4];

			return (Memory)memoryCode;

		}

		#endregion // Secret parsing logic

		#region Secret generation logic

		public byte[] CreateGameSecret()
		{
			byte[] secret = new byte[20];

			byte cipherStart = 0;

			byte ringOrGame = 0;

			// unknowns
			byte unknown1 = 0; // Always clear

			// TODO: Odd game IDs seem to use a different mechanism
			SetBit(ref cipherStart, 0, GetBit(_gameId, 2) ^ GetBit(_gameId, 10));
			SetBit(ref cipherStart, 1, GetBit(_gameId, 1) ^ GetBit(_gameId, 9));
			SetBit(ref cipherStart, 2, GetBit(_gameId, 0) ^ GetBit(_gameId, 8));

			CurrXor = (byte)(cipherStart * 4);

			byte currentByte = 0;

			SetBit(ref currentByte, 2, GetBit(unknown1, 0));
			SetBit(ref currentByte, 1, GetBit(ringOrGame, 0)); // Might need another value for this
			SetBit(ref currentByte, 0, GetBit(_gameId, 0));
			currentByte = (byte)(currentByte ^ Cipher[CurrXor++]);
			SetBit(ref currentByte, 5, GetBit(cipherStart, 2));
			SetBit(ref currentByte, 4, GetBit(cipherStart, 1));
			SetBit(ref currentByte, 3, GetBit(cipherStart, 0));
			secret[0] = currentByte;

			SetBit(ref currentByte, 5, GetBit(_gameId, 1));
			SetBit(ref currentByte, 4, GetBit(_gameId, 2));
			SetBit(ref currentByte, 3, GetBit(_gameId, 3));
			SetBit(ref currentByte, 2, GetBit(_gameId, 4));
			SetBit(ref currentByte, 1, GetBit(_gameId, 5));
			SetBit(ref currentByte, 0, GetBit(_gameId, 6));
			secret[1] = (byte)(currentByte ^ Cipher[CurrXor++]);

			SetBit(ref currentByte, 5, GetBit(_gameId, 7));
			SetBit(ref currentByte, 4, GetBit(_gameId, 8));
			SetBit(ref currentByte, 3, GetBit(_gameId, 9));
			SetBit(ref currentByte, 2, GetBit(_gameId, 10));
			SetBit(ref currentByte, 1, GetBit(_gameId, 11));
			SetBit(ref currentByte, 0, GetBit(_gameId, 12));
			secret[2] = (byte)(currentByte ^ Cipher[CurrXor++]);

			SetBit(ref currentByte, 5, GetBit(_gameId, 13));
			SetBit(ref currentByte, 4, GetBit(_gameId, 14));
			SetBit(ref currentByte, 3, GetBit(_linkedHeros, 0));
			SetBit(ref currentByte, 2, GetBit(_agesSeasons, 0));
			SetBit(ref currentByte, 1, GetBit(_hero, 0, 0));
			SetBit(ref currentByte, 0, GetBit(_hero, 0, 1));
			secret[3] = (byte)(currentByte ^ Cipher[CurrXor++]);

			SetBit(ref currentByte, 5, GetBit(_hero, 0, 2));
			SetBit(ref currentByte, 4, GetBit(_hero, 0, 3));
			SetBit(ref currentByte, 3, GetBit(_hero, 0, 4));
			SetBit(ref currentByte, 2, GetBit(_hero, 0, 5));
			SetBit(ref currentByte, 1, GetBit(_hero, 0, 6));
			SetBit(ref currentByte, 0, GetBit(_hero, 0, 7));
			secret[4] = (byte)(currentByte ^ Cipher[CurrXor++]);

			SetBit(ref currentByte, 5, GetBit(_child, 0, 0));
			SetBit(ref currentByte, 4, GetBit(_child, 0, 1));
			SetBit(ref currentByte, 3, GetBit(_child, 0, 2));
			SetBit(ref currentByte, 2, GetBit(_child, 0, 3));
			SetBit(ref currentByte, 1, GetBit(_child, 0, 4));
			SetBit(ref currentByte, 0, GetBit(_child, 0, 5));
			secret[5] = (byte)(currentByte ^ Cipher[CurrXor++]);

			SetBit(ref currentByte, 5, GetBit(_child, 0, 6));
			SetBit(ref currentByte, 4, GetBit(_child, 0, 7));
			SetBit(ref currentByte, 3, GetBit(_hero, 1, 0));
			SetBit(ref currentByte, 2, GetBit(_hero, 1, 1));
			SetBit(ref currentByte, 1, GetBit(_hero, 1, 2));
			SetBit(ref currentByte, 0, GetBit(_hero, 1, 3));
			secret[6] = (byte)(currentByte ^ Cipher[CurrXor++]);

			SetBit(ref currentByte, 5, GetBit(_hero, 1, 4));
			SetBit(ref currentByte, 4, GetBit(_hero, 1, 5));
			SetBit(ref currentByte, 3, GetBit(_hero, 1, 6));
			SetBit(ref currentByte, 2, GetBit(_hero, 1, 7));
			SetBit(ref currentByte, 1, GetBit(_child, 1, 0));
			SetBit(ref currentByte, 0, GetBit(_child, 1, 1));
			secret[7] = (byte)(currentByte ^ Cipher[CurrXor++]);

			SetBit(ref currentByte, 5, GetBit(_child, 1, 2));
			SetBit(ref currentByte, 4, GetBit(_child, 1, 3));
			SetBit(ref currentByte, 3, GetBit(_child, 1, 4));
			SetBit(ref currentByte, 2, GetBit(_child, 1, 5));
			SetBit(ref currentByte, 1, GetBit(_child, 1, 6));
			SetBit(ref currentByte, 0, GetBit(_child, 1, 7));
			secret[8] = (byte)(currentByte ^ Cipher[CurrXor++]);

			byte unknown2 = 0; // Always clear
			byte unknown3 = 0; // Always clear

			SetBit(ref currentByte, 5, GetBit(_behavior, 0));
			SetBit(ref currentByte, 4, GetBit(_behavior, 1));
			SetBit(ref currentByte, 3, GetBit(_behavior, 2));
			SetBit(ref currentByte, 2, GetBit(_behavior, 3));
			SetBit(ref currentByte, 1, GetBit(unknown2, 0));
			SetBit(ref currentByte, 0, GetBit(unknown3, 0));
			secret[9] = (byte)(currentByte ^ Cipher[CurrXor++]);

			SetBit(ref currentByte, 5, GetBit(_hero, 2, 0));
			SetBit(ref currentByte, 4, GetBit(_hero, 2, 1));
			SetBit(ref currentByte, 3, GetBit(_hero, 2, 2));
			SetBit(ref currentByte, 2, GetBit(_hero, 2, 3));
			SetBit(ref currentByte, 1, GetBit(_hero, 2, 4));
			SetBit(ref currentByte, 0, GetBit(_hero, 2, 5));
			secret[10] = (byte)(currentByte ^ Cipher[CurrXor++]);

			SetBit(ref currentByte, 5, GetBit(_hero, 2, 6));
			SetBit(ref currentByte, 4, GetBit(_hero, 2, 7));
			SetBit(ref currentByte, 3, GetBit(_child, 2, 0));
			SetBit(ref currentByte, 2, GetBit(_child, 2, 1));
			SetBit(ref currentByte, 1, GetBit(_child, 2, 2));
			SetBit(ref currentByte, 0, GetBit(_child, 2, 3));
			secret[11] = (byte)(currentByte ^ Cipher[CurrXor++]);

			byte unknown4 = 1; // Always set

			SetBit(ref currentByte, 5, GetBit(_child, 2, 4));
			SetBit(ref currentByte, 4, GetBit(_child, 2, 5));
			SetBit(ref currentByte, 3, GetBit(_child, 2, 6));
			SetBit(ref currentByte, 2, GetBit(_child, 2, 7));
			SetBit(ref currentByte, 1, GetBit(unknown4, 0));
			SetBit(ref currentByte, 0, GetBit(_hero, 3, 0));
			secret[12] = (byte)(currentByte ^ Cipher[CurrXor++]);

			SetBit(ref currentByte, 5, GetBit(_hero, 3, 1));
			SetBit(ref currentByte, 4, GetBit(_hero, 3, 2));
			SetBit(ref currentByte, 3, GetBit(_hero, 3, 3));
			SetBit(ref currentByte, 2, GetBit(_hero, 3, 4));
			SetBit(ref currentByte, 1, GetBit(_hero, 3, 5));
			SetBit(ref currentByte, 0, GetBit(_hero, 3, 6));
			secret[13] = (byte)(currentByte ^ Cipher[CurrXor++]);

			// NOTE: This bit may not be unknown. Since it is always
			// set and if we include it in the animal bytes, we end
			// up with values that match those in the VBA save file.
			byte unknown5 = 1; // Always set

			SetBit(ref currentByte, 5, GetBit(_hero, 3, 7));
			SetBit(ref currentByte, 4, GetBit(_animal, 0));
			SetBit(ref currentByte, 3, GetBit(_animal, 1));
			SetBit(ref currentByte, 2, GetBit(_animal, 2));
			SetBit(ref currentByte, 1, GetBit(unknown5, 0));
			SetBit(ref currentByte, 0, GetBit(_hero, 4, 1));
			secret[14] = (byte)(currentByte ^ Cipher[CurrXor++]);

			SetBit(ref currentByte, 5, GetBit(_hero, 4, 1));
			SetBit(ref currentByte, 4, GetBit(_hero, 4, 2));
			SetBit(ref currentByte, 3, GetBit(_hero, 4, 3));
			SetBit(ref currentByte, 2, GetBit(_hero, 4, 4));
			SetBit(ref currentByte, 1, GetBit(_hero, 4, 5));
			SetBit(ref currentByte, 0, GetBit(_hero, 4, 6));
			secret[15] = (byte)(currentByte ^ Cipher[CurrXor++]);

			SetBit(ref currentByte, 5, GetBit(_hero, 4, 7));
			SetBit(ref currentByte, 4, GetBit(_child, 3, 0));
			SetBit(ref currentByte, 3, GetBit(_child, 3, 1));
			SetBit(ref currentByte, 2, GetBit(_child, 3, 2));
			SetBit(ref currentByte, 1, GetBit(_child, 3, 3));
			SetBit(ref currentByte, 0, GetBit(_child, 3, 4));
			secret[16] = (byte)(currentByte ^ Cipher[CurrXor++]);

			byte unknown6 = 1; // Always set

			SetBit(ref currentByte, 5, GetBit(_child, 3, 5));
			SetBit(ref currentByte, 4, GetBit(_child, 3, 6));
			SetBit(ref currentByte, 3, GetBit(_child, 3, 7));
			SetBit(ref currentByte, 2, GetBit(unknown6, 0));
			SetBit(ref currentByte, 1, GetBit(_child, 4, 0));
			SetBit(ref currentByte, 0, GetBit(_child, 4, 1));
			secret[17] = (byte)(currentByte ^ Cipher[CurrXor++]);

			SetBit(ref currentByte, 5, GetBit(_child, 4, 2));
			SetBit(ref currentByte, 4, GetBit(_child, 4, 3));
			SetBit(ref currentByte, 3, GetBit(_child, 4, 4));
			SetBit(ref currentByte, 2, GetBit(_child, 4, 5));
			SetBit(ref currentByte, 1, GetBit(_child, 4, 6));
			SetBit(ref currentByte, 0, GetBit(_child, 4, 7));
			secret[18] = (byte)(currentByte ^ Cipher[CurrXor++]);

			// TODO: Figure out what all the unknown values are for.

			// TODO: Calculate the checksum
			secret[19] = 255;
			return secret;
		}

		public byte[] CreateRingSecret()
		{
			byte[] secret = new byte[15];

			byte cipherStart = 0;

			byte ringOrGame = 1;


			byte ring1 = (byte)_rings;
			byte ring2 = (byte)(_rings >> 8);
			byte ring3 = (byte)(_rings >> 16);
			byte ring4 = (byte)(_rings >> 24);
			byte ring5 = (byte)(_rings >> 32);
			byte ring6 = (byte)(_rings >> 40);
			byte ring7 = (byte)(_rings >> 48);
			byte ring8 = (byte)(_rings >> 56);

			// unknowns
			byte unknown1 = 0; // Always clear


			// TODO: Odd game IDs seem to use a different mechanism
			SetBit(ref cipherStart, 0, GetBit(_gameId, 2) ^ GetBit(_gameId, 10));
			SetBit(ref cipherStart, 1, GetBit(_gameId, 1) ^ GetBit(_gameId, 9));
			SetBit(ref cipherStart, 2, GetBit(_gameId, 0) ^ GetBit(_gameId, 8));

			CurrXor = (byte)(cipherStart * 4);

			byte currentByte = 0;

			SetBit(ref currentByte, 2, GetBit(unknown1, 0));
			SetBit(ref currentByte, 1, GetBit(ringOrGame, 0)); // Might need another value for this
			SetBit(ref currentByte, 0, GetBit(_gameId, 0));
			currentByte = (byte)(currentByte ^ Cipher[CurrXor++]);
			SetBit(ref currentByte, 5, GetBit(cipherStart, 2));
			SetBit(ref currentByte, 4, GetBit(cipherStart, 1));
			SetBit(ref currentByte, 3, GetBit(cipherStart, 0));
			secret[0] = currentByte;

			SetBit(ref currentByte, 5, GetBit(_gameId, 1));
			SetBit(ref currentByte, 4, GetBit(_gameId, 2));
			SetBit(ref currentByte, 3, GetBit(_gameId, 3));
			SetBit(ref currentByte, 2, GetBit(_gameId, 4));
			SetBit(ref currentByte, 1, GetBit(_gameId, 5));
			SetBit(ref currentByte, 0, GetBit(_gameId, 6));
			secret[1] = (byte)(currentByte ^ Cipher[CurrXor++]);

			SetBit(ref currentByte, 5, GetBit(_gameId, 7));
			SetBit(ref currentByte, 4, GetBit(_gameId, 8));
			SetBit(ref currentByte, 3, GetBit(_gameId, 9));
			SetBit(ref currentByte, 2, GetBit(_gameId, 10));
			SetBit(ref currentByte, 1, GetBit(_gameId, 11));
			SetBit(ref currentByte, 0, GetBit(_gameId, 12));
			secret[2] = (byte)(currentByte ^ Cipher[CurrXor++]);

			SetBit(ref currentByte, 5, GetBit(_gameId, 13));
			SetBit(ref currentByte, 4, GetBit(_gameId, 14));
			// TODO: Compare game IDs

			SetBit(ref currentByte, 3, GetBit(ring2, 0));
			SetBit(ref currentByte, 2, GetBit(ring2, 1));
			SetBit(ref currentByte, 1, GetBit(ring2, 2));
			SetBit(ref currentByte, 0, GetBit(ring2, 3));
			secret[3] = (byte)(currentByte ^ Cipher[CurrXor++]);

			SetBit(ref currentByte, 5, GetBit(ring2, 4));
			SetBit(ref currentByte, 4, GetBit(ring2, 5));
			SetBit(ref currentByte, 3, GetBit(ring2, 6));
			SetBit(ref currentByte, 2, GetBit(ring2, 7));
			SetBit(ref currentByte, 1, GetBit(ring6, 0));
			SetBit(ref currentByte, 0, GetBit(ring6, 1));
			secret[4] = (byte)(currentByte ^ Cipher[CurrXor++]);

			SetBit(ref currentByte, 5, GetBit(ring6, 2));
			SetBit(ref currentByte, 4, GetBit(ring6, 3));
			SetBit(ref currentByte, 3, GetBit(ring6, 4));
			SetBit(ref currentByte, 2, GetBit(ring6, 5));
			SetBit(ref currentByte, 1, GetBit(ring6, 6));
			SetBit(ref currentByte, 0, GetBit(ring6, 7));
			secret[5] = (byte)(currentByte ^ Cipher[CurrXor++]);

			SetBit(ref currentByte, 5, GetBit(ring8, 0));
			SetBit(ref currentByte, 4, GetBit(ring8, 1));
			SetBit(ref currentByte, 3, GetBit(ring8, 2));
			SetBit(ref currentByte, 2, GetBit(ring8, 3));
			SetBit(ref currentByte, 1, GetBit(ring8, 4));
			SetBit(ref currentByte, 0, GetBit(ring8, 5));
			secret[6] = (byte)(currentByte ^ Cipher[CurrXor++]);

			SetBit(ref currentByte, 5, GetBit(ring8, 6));
			SetBit(ref currentByte, 4, GetBit(ring8, 7));
			SetBit(ref currentByte, 3, GetBit(ring4, 0));
			SetBit(ref currentByte, 2, GetBit(ring4, 1));
			SetBit(ref currentByte, 1, GetBit(ring4, 2));
			SetBit(ref currentByte, 0, GetBit(ring4, 3));
			secret[7] = (byte)(currentByte ^ Cipher[CurrXor++]);

			SetBit(ref currentByte, 5, GetBit(ring4, 4));
			SetBit(ref currentByte, 4, GetBit(ring4, 5));
			SetBit(ref currentByte, 3, GetBit(ring4, 6));
			SetBit(ref currentByte, 2, GetBit(ring4, 7));
			SetBit(ref currentByte, 1, GetBit(ring1, 0));
			SetBit(ref currentByte, 0, GetBit(ring1, 1));
			secret[8] = (byte)(currentByte ^ Cipher[CurrXor++]);

			SetBit(ref currentByte, 5, GetBit(ring1, 2));
			SetBit(ref currentByte, 4, GetBit(ring1, 3));
			SetBit(ref currentByte, 3, GetBit(ring1, 4));
			SetBit(ref currentByte, 2, GetBit(ring1, 5));
			SetBit(ref currentByte, 1, GetBit(ring1, 6));
			SetBit(ref currentByte, 0, GetBit(ring1, 7));
			secret[9] = (byte)(currentByte ^ Cipher[CurrXor++]);

			SetBit(ref currentByte, 5, GetBit(ring5, 0));
			SetBit(ref currentByte, 4, GetBit(ring5, 1));
			SetBit(ref currentByte, 3, GetBit(ring5, 2));
			SetBit(ref currentByte, 2, GetBit(ring5, 3));
			SetBit(ref currentByte, 1, GetBit(ring5, 4));
			SetBit(ref currentByte, 0, GetBit(ring5, 5));
			secret[10] = (byte)(currentByte ^ Cipher[CurrXor++]);

			SetBit(ref currentByte, 5, GetBit(ring5, 6));
			SetBit(ref currentByte, 4, GetBit(ring5, 7));
			SetBit(ref currentByte, 3, GetBit(ring3, 0));
			SetBit(ref currentByte, 2, GetBit(ring3, 1));
			SetBit(ref currentByte, 1, GetBit(ring3, 2));
			SetBit(ref currentByte, 0, GetBit(ring3, 3));
			secret[11] = (byte)(currentByte ^ Cipher[CurrXor++]);

			SetBit(ref currentByte, 5, GetBit(ring3, 4));
			SetBit(ref currentByte, 4, GetBit(ring3, 5));
			SetBit(ref currentByte, 3, GetBit(ring3, 6));
			SetBit(ref currentByte, 2, GetBit(ring3, 7));
			SetBit(ref currentByte, 1, GetBit(ring7, 0));
			SetBit(ref currentByte, 0, GetBit(ring7, 1));
			secret[12] = (byte)(currentByte ^ Cipher[CurrXor++]);
			
			SetBit(ref currentByte, 5, GetBit(ring7, 2));
			SetBit(ref currentByte, 4, GetBit(ring7, 3));
			SetBit(ref currentByte, 3, GetBit(ring7, 4));
			SetBit(ref currentByte, 2, GetBit(ring7, 5));
			SetBit(ref currentByte, 1, GetBit(ring7, 6));
			SetBit(ref currentByte, 0, GetBit(ring7, 7));
			secret[13] = (byte)(currentByte ^ Cipher[CurrXor++]);


			// TODO: Figure out what all the unknown values are for.

			// TODO: Calculate the checksum
			secret[14] = 255;
			return secret;
		}

		#endregion // Secret generation logic

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

		#region File Saving/Loading methods

		/// <summary>
		/// Writes this game info out to the specified file
		/// </summary>
		/// <param name="filename">The file name</param>
		public void Write(string filename)
		{
			using (FileStream outFile = File.Create(filename))
			{
				Write(outFile);
			}
		}

		/// <summary>
		/// Writes the game info to the specified stream
		/// </summary>
		/// <param name="stream">The stream to write to</param>
		public void Write(Stream stream)
		{
			string json = JsonConvert.SerializeObject(this);
			using (var swriter = new StreamWriter(stream))
			using (var jwriter = new JsonTextWriter(swriter))
			{
				var serializer = new JsonSerializer();
				serializer.Serialize(jwriter, this);
			}
		}

		/// <summary>
		/// Loads the game info from the specified file
		/// </summary>
		/// <param name="filename">The file name of the saved GameInfo</param>
		/// <returns>A GameInfo</returns>
		public static GameInfo Load(string filename)
		{
			using (FileStream inFile = File.OpenRead(filename))
			{
				return Load(inFile);
			}
		}

		/// <summary>
		/// Loads the game from the specified stream
		/// </summary>
		/// <param name="stream">The stream containing the saved GameInfo</param>
		/// <returns>A GameInfo</returns>
		public static GameInfo Load(Stream stream)
		{
			using (var sreader = new StreamReader(stream))
			using (var jreader = new JsonTextReader(sreader))
			{
				var serializer = new JsonSerializer();
				return serializer.Deserialize<GameInfo>(jreader);
			}
		}
		#endregion // File Saving/Loading methods
	}
}
