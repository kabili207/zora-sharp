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

		/// <summary>
		/// Loads the specified <paramref name="secret"/> data into this GameInfo
		/// </summary>
		/// <param name="secret">The raw secret data</param>
		public void LoadGameData(byte[] secret)
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

			byte currentByte = (byte)(secret[0] ^ Cipher[CurrXor++]);
			SetBit(ref unknown1, 0, GetBit(currentByte, 2));
			SetBit(ref ringOrGame, 0, GetBit(currentByte, 1));
			
			// TODO: This value alone doesn't seem to specify the secret type.
			//if (ringOrGame != 0)
			//	throw new ArgumentException("The specified data is not a game code", "secret");

			SetBit(ref _gameId, 0, GetBit(currentByte, 0));

			currentByte = (byte)(secret[1] ^ Cipher[CurrXor++]);
			SetBit(ref _gameId, 1, GetBit(currentByte, 5));
			SetBit(ref _gameId, 2, GetBit(currentByte, 4));
			SetBit(ref _gameId, 3, GetBit(currentByte, 3));
			SetBit(ref _gameId, 4, GetBit(currentByte, 2));
			SetBit(ref _gameId, 5, GetBit(currentByte, 1));
			SetBit(ref _gameId, 6, GetBit(currentByte, 0));

			currentByte = (byte)(secret[2] ^ Cipher[CurrXor++]);
			SetBit(ref _gameId, 7, GetBit(currentByte, 5));
			SetBit(ref _gameId, 8, GetBit(currentByte, 4));
			SetBit(ref _gameId, 9, GetBit(currentByte, 3));
			SetBit(ref _gameId, 10, GetBit(currentByte, 2));
			SetBit(ref _gameId, 11, GetBit(currentByte, 1));
			SetBit(ref _gameId, 12, GetBit(currentByte, 0));

			currentByte = (byte)(secret[3] ^ Cipher[CurrXor++]);
			SetBit(ref _gameId, 13, GetBit(currentByte, 5));
			SetBit(ref _gameId, 14, GetBit(currentByte, 4));
			SetBit(ref _linkedHeros, 0, GetBit(currentByte, 3));
			SetBit(ref _agesSeasons, 0, GetBit(currentByte, 2));
			SetBit(ref _hero, 0, 0, GetBit(currentByte, 1));
			SetBit(ref _hero, 0, 1, GetBit(currentByte, 0));

			// TODO: Validate game ID with cipher bytes

			currentByte = (byte)(secret[4] ^ Cipher[CurrXor++]);
			SetBit(ref _hero, 0, 2, GetBit(currentByte, 5));
			SetBit(ref _hero, 0, 3, GetBit(currentByte, 4));
			SetBit(ref _hero, 0, 4, GetBit(currentByte, 3));
			SetBit(ref _hero, 0, 5, GetBit(currentByte, 2));
			SetBit(ref _hero, 0, 6, GetBit(currentByte, 1));
			SetBit(ref _hero, 0, 7, GetBit(currentByte, 0));

			currentByte = (byte)(secret[5] ^ Cipher[CurrXor++]);
			SetBit(ref _child, 0, 0, GetBit(currentByte, 5));
			SetBit(ref _child, 0, 1, GetBit(currentByte, 4));
			SetBit(ref _child, 0, 2, GetBit(currentByte, 3));
			SetBit(ref _child, 0, 3, GetBit(currentByte, 2));
			SetBit(ref _child, 0, 4, GetBit(currentByte, 1));
			SetBit(ref _child, 0, 5, GetBit(currentByte, 0));

			currentByte = (byte)(secret[6] ^ Cipher[CurrXor++]);
			SetBit(ref _child, 0, 6, GetBit(currentByte, 5));
			SetBit(ref _child, 0, 7, GetBit(currentByte, 4));
			SetBit(ref _hero, 1, 0, GetBit(currentByte, 3));
			SetBit(ref _hero, 1, 1, GetBit(currentByte, 2));
			SetBit(ref _hero, 1, 2, GetBit(currentByte, 1));
			SetBit(ref _hero, 1, 3, GetBit(currentByte, 0));

			currentByte = (byte)(secret[7] ^ Cipher[CurrXor++]);
			SetBit(ref _hero, 1, 4, GetBit(currentByte, 5));
			SetBit(ref _hero, 1, 5, GetBit(currentByte, 4));
			SetBit(ref _hero, 1, 6, GetBit(currentByte, 3));
			SetBit(ref _hero, 1, 7, GetBit(currentByte, 2));
			SetBit(ref _child, 1, 0, GetBit(currentByte, 1));
			SetBit(ref _child, 1, 1, GetBit(currentByte, 0));

			currentByte = (byte)(secret[8] ^ Cipher[CurrXor++]);
			SetBit(ref _child, 1, 2, GetBit(currentByte, 5));
			SetBit(ref _child, 1, 3, GetBit(currentByte, 4));
			SetBit(ref _child, 1, 4, GetBit(currentByte, 3));
			SetBit(ref _child, 1, 5, GetBit(currentByte, 2));
			SetBit(ref _child, 1, 6, GetBit(currentByte, 1));
			SetBit(ref _child, 1, 7, GetBit(currentByte, 0));

			byte unknown2 = 0;
			byte unknown3 = 0;

			currentByte = (byte)(secret[9] ^ Cipher[CurrXor++]);
			SetBit(ref _behavior, 0, GetBit(currentByte, 5));
			SetBit(ref _behavior, 1, GetBit(currentByte, 4));
			SetBit(ref _behavior, 2, GetBit(currentByte, 3));
			SetBit(ref _behavior, 3, GetBit(currentByte, 2));
			SetBit(ref unknown2, 0, GetBit(currentByte, 1));
			SetBit(ref unknown3, 0, GetBit(currentByte, 0));

			currentByte = (byte)(secret[10] ^ Cipher[CurrXor++]);
			SetBit(ref _hero, 2, 0, GetBit(currentByte, 5));
			SetBit(ref _hero, 2, 1, GetBit(currentByte, 4));
			SetBit(ref _hero, 2, 2, GetBit(currentByte, 3));
			SetBit(ref _hero, 2, 3, GetBit(currentByte, 2));
			SetBit(ref _hero, 2, 4, GetBit(currentByte, 1));
			SetBit(ref _hero, 2, 5, GetBit(currentByte, 0));

			currentByte = (byte)(secret[11] ^ Cipher[CurrXor++]);
			SetBit(ref _hero, 2, 6, GetBit(currentByte, 5));
			SetBit(ref _hero, 2, 7, GetBit(currentByte, 4));
			SetBit(ref _child, 2, 0, GetBit(currentByte, 3));
			SetBit(ref _child, 2, 1, GetBit(currentByte, 2));
			SetBit(ref _child, 2, 2, GetBit(currentByte, 1));
			SetBit(ref _child, 2, 3, GetBit(currentByte, 0));

			byte unknown4 = 0;

			currentByte = (byte)(secret[12] ^ Cipher[CurrXor++]);
			SetBit(ref _child, 2, 4, GetBit(currentByte, 5));
			SetBit(ref _child, 2, 5, GetBit(currentByte, 4));
			SetBit(ref _child, 2, 6, GetBit(currentByte, 3));
			SetBit(ref _child, 2, 7, GetBit(currentByte, 2));
			SetBit(ref unknown4, 0, GetBit(currentByte, 1));
			SetBit(ref _hero, 3, 0, GetBit(currentByte, 0));

			currentByte = (byte)(secret[13] ^ Cipher[CurrXor++]);
			SetBit(ref _hero, 3, 1, GetBit(currentByte, 5));
			SetBit(ref _hero, 3, 2, GetBit(currentByte, 4));
			SetBit(ref _hero, 3, 3, GetBit(currentByte, 3));
			SetBit(ref _hero, 3, 4, GetBit(currentByte, 2));
			SetBit(ref _hero, 3, 5, GetBit(currentByte, 1));
			SetBit(ref _hero, 3, 6, GetBit(currentByte, 0));

			byte unknown5 = 0;

			currentByte = (byte)(secret[14] ^ Cipher[CurrXor++]);
			SetBit(ref _hero, 3, 7, GetBit(currentByte, 5));
			SetBit(ref _animal, 0, GetBit(currentByte, 4));
			SetBit(ref _animal, 1, GetBit(currentByte, 3));
			SetBit(ref _animal, 2, GetBit(currentByte, 2));
			SetBit(ref unknown5, 0, GetBit(currentByte, 1));
			SetBit(ref _hero, 4, 1, GetBit(currentByte, 0));

			currentByte = (byte)(secret[15] ^ Cipher[CurrXor++]);
			SetBit(ref _hero, 4, 1, GetBit(currentByte, 5));
			SetBit(ref _hero, 4, 2, GetBit(currentByte, 4));
			SetBit(ref _hero, 4, 3, GetBit(currentByte, 3));
			SetBit(ref _hero, 4, 4, GetBit(currentByte, 2));
			SetBit(ref _hero, 4, 5, GetBit(currentByte, 1));
			SetBit(ref _hero, 4, 6, GetBit(currentByte, 0));

			currentByte = (byte)(secret[16] ^ Cipher[CurrXor++]);
			SetBit(ref _hero, 4, 7, GetBit(currentByte, 5));
			SetBit(ref _child, 3, 0, GetBit(currentByte, 4));
			SetBit(ref _child, 3, 1, GetBit(currentByte, 3));
			SetBit(ref _child, 3, 2, GetBit(currentByte, 2));
			SetBit(ref _child, 3, 3, GetBit(currentByte, 1));
			SetBit(ref _child, 3, 4, GetBit(currentByte, 0));

			byte unknown6 = 0;

			currentByte = (byte)(secret[17] ^ Cipher[CurrXor++]);
			SetBit(ref _child, 3, 5, GetBit(currentByte, 5));
			SetBit(ref _child, 3, 6, GetBit(currentByte, 4));
			SetBit(ref _child, 3, 7, GetBit(currentByte, 3));
			SetBit(ref unknown6, 0, GetBit(currentByte, 2));
			SetBit(ref _child, 4, 0, GetBit(currentByte, 1));
			SetBit(ref _child, 4, 1, GetBit(currentByte, 0));

			currentByte = (byte)(secret[18] ^ Cipher[CurrXor++]);
			SetBit(ref _child, 4, 2, GetBit(currentByte, 5));
			SetBit(ref _child, 4, 3, GetBit(currentByte, 4));
			SetBit(ref _child, 4, 4, GetBit(currentByte, 3));
			SetBit(ref _child, 4, 5, GetBit(currentByte, 2));
			SetBit(ref _child, 4, 6, GetBit(currentByte, 1));
			SetBit(ref _child, 4, 7, GetBit(currentByte, 0));

			// TODO: Figure out what all the unknown values are for.

			// TODO: Validate checksum
			byte checksum = (byte)(secret[19] ^ Cipher[CurrXor]);

			OnPropertyChanged("Hero");
			OnPropertyChanged("Child");
			OnPropertyChanged("GameID");
			OnPropertyChanged("Animal");
			OnPropertyChanged("Behavior");
			OnPropertyChanged("Game");
			OnPropertyChanged("Quest");
			string json = JsonConvert.SerializeObject(this);
		}

		/// <summary>
		/// Sets the <see cref="Rings"/> property using the specified secret
		/// </summary>
		/// <param name="secret">The raw secret data</param>
		public void LoadRings(byte[] secret)
		{
			if (secret == null || secret.Length != 15)
				throw new ArgumentException("Secret must contatin exactly 15 bytes", "secret");

			byte cipherStart = 0;

			byte ringOrGame = 0;

			short gameID = 0;

			byte ring1 = 0;
			byte ring2 = 0;
			byte ring3 = 0;
			byte ring4 = 0;
			byte ring5 = 0;
			byte ring6 = 0;
			byte ring7 = 0;
			byte ring8 = 0;

			// unknowns
			byte unknown1 = 0;

			SetBit(ref cipherStart, 2, GetBit(secret[0], 5));
			SetBit(ref cipherStart, 1, GetBit(secret[0], 4));
			SetBit(ref cipherStart, 0, GetBit(secret[0], 3));

			CurrXor = (byte)(cipherStart * 4);

			byte currentByte;

			currentByte = (byte)(secret[0] ^ Cipher[CurrXor++]);

			SetBit(ref unknown1, 0, GetBit(currentByte, 2));
			SetBit(ref ringOrGame, 0, GetBit(currentByte, 1));

			// TODO: This value alone doesn't seem to specify the secret type.
			//if (ringOrGame != 1)
			//	throw new ArgumentException("The specified data is not a ring code", "secret");

			SetBit(ref gameID, 0, GetBit(currentByte, 0));

			currentByte = (byte)(secret[1] ^ Cipher[CurrXor++]);
			SetBit(ref gameID, 1, GetBit(currentByte, 5));
			SetBit(ref gameID, 2, GetBit(currentByte, 4));
			SetBit(ref gameID, 3, GetBit(currentByte, 3));
			SetBit(ref gameID, 4, GetBit(currentByte, 2));
			SetBit(ref gameID, 5, GetBit(currentByte, 1));
			SetBit(ref gameID, 6, GetBit(currentByte, 0));

			currentByte = (byte)(secret[2] ^ Cipher[CurrXor++]);
			SetBit(ref gameID, 7, GetBit(currentByte, 5));
			SetBit(ref gameID, 8, GetBit(currentByte, 4));
			SetBit(ref gameID, 9, GetBit(currentByte, 3));
			SetBit(ref gameID, 10, GetBit(currentByte, 2));
			SetBit(ref gameID, 11, GetBit(currentByte, 1));
			SetBit(ref gameID, 12, GetBit(currentByte, 0));

			currentByte = (byte)(secret[3] ^ Cipher[CurrXor++]);
			SetBit(ref gameID, 13, GetBit(currentByte, 5));
			SetBit(ref gameID, 14, GetBit(currentByte, 4));

			// TODO: Compare game IDs

			SetBit(ref ring2, 0, GetBit(currentByte, 3));
			SetBit(ref ring2, 1, GetBit(currentByte, 2));
			SetBit(ref ring2, 2, GetBit(currentByte, 1));
			SetBit(ref ring2, 3, GetBit(currentByte, 0));

			currentByte = (byte)(secret[4] ^ Cipher[CurrXor++]);
			SetBit(ref ring2, 4, GetBit(currentByte, 5));
			SetBit(ref ring2, 5, GetBit(currentByte, 4));
			SetBit(ref ring2, 6, GetBit(currentByte, 3));
			SetBit(ref ring2, 7, GetBit(currentByte, 2));
			SetBit(ref ring6, 0, GetBit(currentByte, 1));
			SetBit(ref ring6, 1, GetBit(currentByte, 0));

			currentByte = (byte)(secret[5] ^ Cipher[CurrXor++]);
			SetBit(ref ring6, 2, GetBit(currentByte, 5));
			SetBit(ref ring6, 3, GetBit(currentByte, 4));
			SetBit(ref ring6, 4, GetBit(currentByte, 3));
			SetBit(ref ring6, 5, GetBit(currentByte, 2));
			SetBit(ref ring6, 6, GetBit(currentByte, 1));
			SetBit(ref ring6, 7, GetBit(currentByte, 0));

			currentByte = (byte)(secret[6] ^ Cipher[CurrXor++]);
			SetBit(ref ring8, 0, GetBit(currentByte, 5));
			SetBit(ref ring8, 1, GetBit(currentByte, 4));
			SetBit(ref ring8, 2, GetBit(currentByte, 3));
			SetBit(ref ring8, 3, GetBit(currentByte, 2));
			SetBit(ref ring8, 4, GetBit(currentByte, 1));
			SetBit(ref ring8, 5, GetBit(currentByte, 0));

			currentByte = (byte)(secret[7] ^ Cipher[CurrXor++]);
			SetBit(ref ring8, 6, GetBit(currentByte, 5));
			SetBit(ref ring8, 7, GetBit(currentByte, 4));
			SetBit(ref ring4, 0, GetBit(currentByte, 3));
			SetBit(ref ring4, 1, GetBit(currentByte, 2));
			SetBit(ref ring4, 2, GetBit(currentByte, 1));
			SetBit(ref ring4, 3, GetBit(currentByte, 0));

			currentByte = (byte)(secret[8] ^ Cipher[CurrXor++]);
			SetBit(ref ring4, 4, GetBit(currentByte, 5));
			SetBit(ref ring4, 5, GetBit(currentByte, 4));
			SetBit(ref ring4, 6, GetBit(currentByte, 3));
			SetBit(ref ring4, 7, GetBit(currentByte, 2));
			SetBit(ref ring1, 0, GetBit(currentByte, 1));
			SetBit(ref ring1, 1, GetBit(currentByte, 0));

			currentByte = (byte)(secret[9] ^ Cipher[CurrXor++]);
			SetBit(ref ring1, 2, GetBit(currentByte, 5));
			SetBit(ref ring1, 3, GetBit(currentByte, 4));
			SetBit(ref ring1, 4, GetBit(currentByte, 3));
			SetBit(ref ring1, 5, GetBit(currentByte, 2));
			SetBit(ref ring1, 6, GetBit(currentByte, 1));
			SetBit(ref ring1, 7, GetBit(currentByte, 0));

			currentByte = (byte)(secret[10] ^ Cipher[CurrXor++]);
			SetBit(ref ring5, 0, GetBit(currentByte, 5));
			SetBit(ref ring5, 1, GetBit(currentByte, 4));
			SetBit(ref ring5, 2, GetBit(currentByte, 3));
			SetBit(ref ring5, 3, GetBit(currentByte, 2));
			SetBit(ref ring5, 4, GetBit(currentByte, 1));
			SetBit(ref ring5, 5, GetBit(currentByte, 0));

			currentByte = (byte)(secret[11] ^ Cipher[CurrXor++]);
			SetBit(ref ring5, 6, GetBit(currentByte, 5));
			SetBit(ref ring5, 7, GetBit(currentByte, 4));
			SetBit(ref ring3, 0, GetBit(currentByte, 3));
			SetBit(ref ring3, 1, GetBit(currentByte, 2));
			SetBit(ref ring3, 2, GetBit(currentByte, 1));
			SetBit(ref ring3, 3, GetBit(currentByte, 0));

			currentByte = (byte)(secret[12] ^ Cipher[CurrXor++]);
			SetBit(ref ring3, 4, GetBit(currentByte, 5));
			SetBit(ref ring3, 5, GetBit(currentByte, 4));
			SetBit(ref ring3, 6, GetBit(currentByte, 3));
			SetBit(ref ring3, 7, GetBit(currentByte, 2));
			SetBit(ref ring7, 0, GetBit(currentByte, 1));
			SetBit(ref ring7, 1, GetBit(currentByte, 0));

			currentByte = (byte)(secret[13] ^ Cipher[CurrXor++]);
			SetBit(ref ring7, 2, GetBit(currentByte, 5));
			SetBit(ref ring7, 3, GetBit(currentByte, 4));
			SetBit(ref ring7, 4, GetBit(currentByte, 3));
			SetBit(ref ring7, 5, GetBit(currentByte, 2));
			SetBit(ref ring7, 6, GetBit(currentByte, 1));
			SetBit(ref ring7, 7, GetBit(currentByte, 0));

			_rings = ring8;
			_rings = (_rings << 8) | ring7;
			_rings = (_rings << 8) | ring6;
			_rings = (_rings << 8) | ring5;
			_rings = (_rings << 8) | ring4;
			_rings = (_rings << 8) | ring3;
			_rings = (_rings << 8) | ring2;
			_rings = (_rings << 8) | ring1;

			OnPropertyChanged("Rings");

			// TODO: Validate checksum
			byte checksum = (byte)(secret[14] ^ Cipher[CurrXor]);
		}

		public void GetDecodedBinary(byte[] secret)
		{
			byte cipherStart = 0;

			SetBit(ref cipherStart, 2, GetBit(secret[0], 5));
			SetBit(ref cipherStart, 1, GetBit(secret[0], 4));
			SetBit(ref cipherStart, 0, GetBit(secret[0], 3));

			CurrXor = (byte)(cipherStart * 4);

			byte currentByte = (byte)(secret[0] ^ Cipher[CurrXor++]);

			StringBuilder builder = new StringBuilder();
			builder.Append(GetBit(secret[0], 5) ? 1 : 0);
			builder.Append(GetBit(secret[0], 4) ? 1 : 0);
			builder.Append(GetBit(secret[0], 3) ? 1 : 0);
			builder.Append(GetBit(currentByte, 2) ? 1 : 0);
			builder.Append(GetBit(currentByte, 1) ? 1 : 0);
			builder.Append(GetBit(currentByte, 0) ? 1 : 0);

			for (int i = 1; i < secret.Length; i++)
			{
				currentByte = (byte)(secret[i] ^ Cipher[CurrXor++]);
				builder.Append(" ");
				for (int j = 5; j > -1; j--)
				{
					builder.Append(GetBit(currentByte, j) ? "1" : "0");
				}
			}

			string data = builder.ToString();
		}

		public Memory ReadMemorySecret(byte[] secret)
		{
			//if (secret == null || secret.Length != 20)
			//	throw new ArgumentException("Secret must contatin exactly 5 bytes", "secret");

			byte cipherStart = 0;
			short gameId = 0;
			byte memoryCode = 0;

			// unknowns
			byte unknown1 = 0;
			byte unknown2 = 0;

			SetBit(ref cipherStart, 2, GetBit(secret[0], 5));
			SetBit(ref cipherStart, 1, GetBit(secret[0], 4));
			SetBit(ref cipherStart, 0, GetBit(secret[0], 3));

			CurrXor = (byte)(cipherStart * 4);

			byte currentByte = (byte)(secret[0] ^ Cipher[CurrXor++]);
			SetBit(ref unknown1, 0, GetBit(currentByte, 2));
			SetBit(ref unknown2, 0, GetBit(currentByte, 1));

			//if (ringOrGame != 0)
			//	throw new ArgumentException("The specified data is not a game code", "secret");

			CurrXor = (byte)(cipherStart * 4);

			currentByte = (byte)(secret[0] ^ Cipher[CurrXor++]);

			SetBit(ref gameId, 0, GetBit(currentByte, 0));

			currentByte = (byte)(secret[1] ^ Cipher[CurrXor++]);
			SetBit(ref gameId, 1, GetBit(currentByte, 5));
			SetBit(ref gameId, 2, GetBit(currentByte, 4));
			SetBit(ref gameId, 3, GetBit(currentByte, 3));
			SetBit(ref gameId, 4, GetBit(currentByte, 2));
			SetBit(ref gameId, 5, GetBit(currentByte, 1));
			SetBit(ref gameId, 6, GetBit(currentByte, 0));

			currentByte = (byte)(secret[2] ^ Cipher[CurrXor++]);
			SetBit(ref gameId, 7, GetBit(currentByte, 5));
			SetBit(ref gameId, 8, GetBit(currentByte, 4));
			SetBit(ref gameId, 9, GetBit(currentByte, 3));
			SetBit(ref gameId, 10, GetBit(currentByte, 2));
			SetBit(ref gameId, 11, GetBit(currentByte, 1));
			SetBit(ref gameId, 12, GetBit(currentByte, 0));

			currentByte = (byte)(secret[3] ^ Cipher[CurrXor++]);
			SetBit(ref gameId, 13, GetBit(currentByte, 5));
			SetBit(ref gameId, 14, GetBit(currentByte, 4));
			SetBit(ref memoryCode, 0, GetBit(currentByte, 3));
			SetBit(ref memoryCode, 1, GetBit(currentByte, 2));
			SetBit(ref memoryCode, 2, GetBit(currentByte, 1));
			SetBit(ref memoryCode, 3, GetBit(currentByte, 0));


			// TODO: Checksum

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
