using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
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
			21, 35, 46,  4, 13, 63, 26, 16,
			58, 47, 30, 32, 15, 62, 54, 55,
			 9, 41, 59, 49,  2, 22, 61, 56, 
			40, 19, 52, 50,  1, 11, 10, 53,
			14, 27, 18, 44, 33, 45, 37, 48,
			25, 42,  6, 57, 60, 23, 51, 24
		};

		#region Fields


		string _hero = "     ";
		string _child = "     ";
		short _gameId = 0;
		byte _behavior = 0;
		byte _animal = 0;
		byte _linkedHeros = 0;
		byte _agesSeasons = 0;

		// JSON.Net has problems serializing the rings if it's an enum,
		// so we have to put the attribute here instead
		[JsonProperty("Rings")]
		long _rings = 0L;

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
		[JsonProperty]
		[JsonConverter(typeof(StringEnumConverter))]
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
		[JsonProperty("QuestType")]
		[JsonConverter(typeof(StringEnumConverter))]
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
		[JsonProperty]
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
		[JsonProperty]
		[JsonConverter(typeof(StringEnumConverter))]
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
		[JsonProperty]
		[JsonConverter(typeof(StringEnumConverter))]
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
				_rings = (long)value;
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

		private byte CalculateChecksum(byte[] secret)
		{
			byte sum = (byte)secret.Sum(x => x);
			int checksum = sum & 0x0F;
			return (byte)checksum;
		}

		#region Secret parsing logic

		private string BytesToString(byte[] secret)
		{
			string data = "";
			foreach (byte b in secret)
			{
				data += Convert.ToString(b, 2).PadLeft(6, '0');
			}
			return data;
		}

		private byte[] DecodeBytes(byte[] secret)
		{
			int cipherKey = (secret[0] >> 3);
			CurrXor = (byte)(cipherKey * 4);

			byte[] decodedBytes = new byte[secret.Length];

			for (int i = 0; i < secret.Length; ++i)
			{
				decodedBytes[i] = (byte)(secret[i] ^ Cipher[CurrXor++]);
			}

			decodedBytes[0] = (byte)(decodedBytes[0] & 7 | (cipherKey << 3));

			return decodedBytes;
		}

		/// <summary>
		/// Loads the specified <paramref name="secret"/> data into this GameInfo
		/// </summary>
		/// <param name="secret">The raw secret data</param>
		public void LoadGameData(byte[] secret)
		{
			if (secret == null || secret.Length != 20)
				throw new InvalidSecretException("Secret must contatin exactly 20 bytes");

			byte[] decodedBytes = DecodeBytes(secret);
			string decodedSecret = BytesToString(decodedBytes);

			byte[] clonedBytes = (byte[])decodedBytes.Clone();
			clonedBytes[19] = 0;
			var checksum = CalculateChecksum(clonedBytes);
			if ((decodedBytes[19] & 7) != (checksum & 7))
				throw new InvalidSecretException("Checksum does not match expected value");

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
			bool unknown6 = decodedSecret[105] == '1';


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
		public void LoadRings(byte[] secret, bool appendRings)
		{
			if (secret == null || secret.Length != 15)
				throw new InvalidSecretException("Secret must contatin exactly 15 bytes");

			byte[] decodedBytes = DecodeBytes(secret);
			string decodedSecret = BytesToString(decodedBytes);

			byte[] clonedBytes = (byte[])decodedBytes.Clone();
			clonedBytes[14] = 0;
			var checksum = CalculateChecksum(clonedBytes);
			if ((decodedBytes[14] & 7) != (checksum & 7))
				throw new InvalidSecretException("Checksum does not match expected value");

			bool isRingCode = decodedSecret[4] == '1';

			// TODO: This value alone doesn't seem to specify the secret type.
			//if(!isGameCode)
			//	throw new ArgumentException("The specified data is not a ring code", "secret");

			short gameId = Convert.ToInt16(decodedSecret.ReversedSubstring(5, 15), 2);
			if (_gameId != gameId)
				throw new InvalidSecretException("The specified secret is not for this Game ID");

			long rings = unchecked((long)Convert.ToUInt64(
				decodedSecret.ReversedSubstring(36, 8) +
				decodedSecret.ReversedSubstring(76, 8) +
				decodedSecret.ReversedSubstring(28, 8) +
				decodedSecret.ReversedSubstring(60, 8) +
				decodedSecret.ReversedSubstring(44, 8) +
				decodedSecret.ReversedSubstring(68, 8) +
				decodedSecret.ReversedSubstring(20, 8) +
				decodedSecret.ReversedSubstring(52, 8)
				, 2));
			if (appendRings)
				rings |= _rings;
			_rings = rings;

			// TODO: Figure out what all the unknown values are for.
			bool unknown1 = decodedSecret[3] == '1';

			OnPropertyChanged("Rings");

		}

		public Memory ReadMemorySecret(byte[] secret)
		{
			if (secret == null || secret.Length != 5)
				throw new InvalidSecretException("Secret must contatin exactly 5 bytes");

			byte[] decodedBytes = DecodeBytes(secret);
			string decodedSecret = BytesToString(decodedBytes);

			short gameId = Convert.ToInt16(decodedSecret.ReversedSubstring(5, 15), 2);


			byte memoryCode = Convert.ToByte(decodedSecret.ReversedSubstring(20, 4), 2);

			// Signifies a memory secret
			bool unknown1 = decodedSecret[3] == '1';
			bool unknown2 = decodedSecret[4] == '1';

			// TODO: Checksum
			byte checksum = secret[4];

			return (Memory)memoryCode;

		}

		#endregion // Secret parsing logic

		#region Secret generation logic

		private byte[] StringToBytes(string data)
		{
			byte[] secret = new byte[data.Length / 6 + 1];
			for (int i = 0; i < secret.Length - 1; ++i)
			{
				secret[i] = (byte)(Convert.ToByte(data.Substring(i * 6, 6), 2));
			}
			return secret;
		}

		private byte[] EncodeBytes(byte[] data)
		{
			int cipherKey = (data[0] >> 3);
			CurrXor = (byte)(cipherKey * 4);

			byte[] secret = new byte[data.Length];
			for (int i = 0; i < data.Length; ++i)
			{
				secret[i] = (byte)(data[i] ^ Cipher[CurrXor++]);
			}

			secret[0] = (byte)(secret[0] & 7 | (cipherKey << 3));
			return secret;
		}

		public byte[] CreateGameSecret()
		{
			int cipherKey = ((_gameId >> 8) + (_gameId & 255)) & 7;
			string unencodedSecret = Convert.ToString(cipherKey, 2).PadLeft(3, '0').Reverse();

			unencodedSecret += "00"; // game = 0

			unencodedSecret += Convert.ToString(_gameId, 2).PadLeft(15, '0').Reverse();
			unencodedSecret += Quest == OracleHack.Quest.LinkedGame ? "0" : "1";
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
			unencodedSecret += Quest == OracleHack.Quest.LinkedGame ? "1" : "0";
			unencodedSecret += Convert.ToString((byte)_child[4], 2).PadLeft(8, '0').Reverse();

			byte[] unencodedBytes = StringToBytes(unencodedSecret);
			unencodedBytes[19] = CalculateChecksum(unencodedBytes);
			byte[] secret = EncodeBytes(unencodedBytes);

			return secret;
		}

		public byte[] CreateRingSecret()
		{
			byte ring1 = (byte)_rings;
			byte ring2 = (byte)(_rings >> 8);
			byte ring3 = (byte)(_rings >> 16);
			byte ring4 = (byte)(_rings >> 24);
			byte ring5 = (byte)(_rings >> 32);
			byte ring6 = (byte)(_rings >> 40);
			byte ring7 = (byte)(_rings >> 48);
			byte ring8 = (byte)(_rings >> 56);

			int cipherKey = ((_gameId >> 8) + (_gameId & 255)) & 7;
			string unencodedSecret = Convert.ToString(cipherKey, 2).PadLeft(3, '0').Reverse();

			unencodedSecret += "01"; // ring secret

			unencodedSecret += Convert.ToString(_gameId, 2).PadLeft(15, '0').Reverse();
			unencodedSecret += Convert.ToString(ring2, 2).PadLeft(8, '0').Reverse();
			unencodedSecret += Convert.ToString(ring6, 2).PadLeft(8, '0').Reverse();
			unencodedSecret += Convert.ToString(ring8, 2).PadLeft(8, '0').Reverse();
			unencodedSecret += Convert.ToString(ring4, 2).PadLeft(8, '0').Reverse();
			unencodedSecret += Convert.ToString(ring1, 2).PadLeft(8, '0').Reverse();
			unencodedSecret += Convert.ToString(ring5, 2).PadLeft(8, '0').Reverse();
			unencodedSecret += Convert.ToString(ring3, 2).PadLeft(8, '0').Reverse();
			unencodedSecret += Convert.ToString(ring7, 2).PadLeft(8, '0').Reverse();

			byte[] unencodedBytes = StringToBytes(unencodedSecret);
			unencodedBytes[14] = CalculateChecksum(unencodedBytes);
			byte[] secret = EncodeBytes(unencodedBytes);

			return secret;
		}

		public byte[] CreateMemorySecret(Memory memory, bool isReturnSecret)
		{
			int cipher = 0;
			if (Game == Game.Ages)
				cipher = isReturnSecret ? 3 : 0;
			else
				cipher = isReturnSecret ? 1 : 2;

			cipher |= (((byte)memory & 1) << 2);
			cipher = ((_gameId >> 8) + (_gameId & 255) + cipher) & 7;
			cipher = Convert.ToInt32(Convert.ToString(cipher, 2).PadLeft(3, '0').Reverse(), 2);

			string unencodedSecret = Convert.ToString(cipher, 2).PadLeft(3, '0');

			unencodedSecret += "11"; // memory secret

			unencodedSecret += Convert.ToString(_gameId, 2).PadLeft(15, '0').Reverse();
			unencodedSecret += Convert.ToString((byte)memory, 2).PadLeft(4, '0').Reverse();

			int mask = 0;

			if (Game == Game.Ages)
				mask = isReturnSecret ? 3 : 0;
			else
				mask = isReturnSecret ? 2 : 1;
			byte[] unencodedBytes = StringToBytes(unencodedSecret);
			unencodedBytes[4] = (byte)(CalculateChecksum(unencodedBytes) | (mask << 4));
			byte[] secret = EncodeBytes(unencodedBytes);

			return secret;
		}

		#endregion // Secret generation logic

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
