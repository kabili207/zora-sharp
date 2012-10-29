using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;

namespace Zyrenth.OracleHack
{
	public enum GameType : byte
	{
		None = 0,
		Seasons = 49,
		Ages = 50
	}

	public enum AnimalType : byte
	{
		None = 0,
		Ricky = 11,
		Dimitri = 12,
		Moosh = 13
	}
	
	public class GameInfo
	{
		private short _GameId;
		private string _HeroName;
		private string _KidName;
		private GameType _Version;
		private AnimalType _Animal;
		private readonly bool[,] _RingBits = new bool[8, 8];

		public short GameId
		{
			get { return _GameId; }
			set { _GameId = value; }
		}

		public string HeroName
		{
			get { return _HeroName; }
			set { _HeroName = value; }
		}

		public string KidName
		{
			get { return _KidName; }
			set { _KidName = value; }
		}

		public GameType Version
		{
			get { return _Version; }
			set { _Version = value; }
		}

		public AnimalType Animal
		{
			get { return _Animal; }
			set { _Animal = value; }
		}
		
		public bool[,] RingBits
		{
			get { return _RingBits; }
		}
		
		public GameInfo()
		{
			
			
		}
		
		public static GameInfo Load(Stream fsSource)
		{
			GameInfo info = new GameInfo();
			
			fsSource.Seek(19, SeekOrigin.Begin);
			// Read the source file into a byte array.
			byte[] versionBytes = new byte[1];
			int numBytesToRead = 1;
			int numBytesRead = 0;
			while (numBytesToRead > 0)
			{
				// Read may return anything from 0 to numBytesToRead.
				int n = fsSource.Read(versionBytes, numBytesRead, numBytesToRead);

				// Break when the end of the file is reached.
				if (n == 0)
					break;

				numBytesRead += n;
				numBytesToRead -= n;
			}

			info.Version = (GameType)versionBytes[0];


			fsSource.Seek(96, SeekOrigin.Begin);
			// Read the source file into a byte array.
			byte[] gameIdBytes = new byte[2];
			numBytesToRead = 2;
			numBytesRead = 0;
			while (numBytesToRead > 0)
			{
				// Read may return anything from 0 to numBytesToRead.
				int n = fsSource.Read(gameIdBytes, numBytesRead, numBytesToRead);

				// Break when the end of the file is reached.
				if (n == 0)
					break;

				numBytesRead += n;
				numBytesToRead -= n;
			}
				
			info.GameId = BitConverter.ToInt16(gameIdBytes, 0);
				
			//fsSource.Seek(98, SeekOrigin.Begin);
			// Read the source file into a byte array.
			byte[] heroBytes = new byte[5];
			numBytesToRead = 5;
			numBytesRead = 0;
			while (numBytesToRead > 0)
			{
				// Read may return anything from 0 to numBytesToRead.
				int n = fsSource.Read(heroBytes, numBytesRead, numBytesToRead);

				// Break when the end of the file is reached.
				if (n == 0)
					break;

				numBytesRead += n;
				numBytesToRead -= n;
			}

			info.HeroName = System.Text.Encoding.ASCII.GetString(heroBytes);
				
			
			//fsSource.Seek(105, SeekOrigin.Begin);
			fsSource.Seek(2, SeekOrigin.Current);
			// Read the source file into a byte array.
			byte[] kidBytes = new byte[5];
			numBytesToRead = 5;
			numBytesRead = 0;
			while (numBytesToRead > 0)
			{
				// Read may return anything from 0 to numBytesToRead.
				int n = fsSource.Read(kidBytes, numBytesRead, numBytesToRead);

				// Break when the end of the file is reached.
				if (n == 0)
					break;

				numBytesRead += n;
				numBytesToRead -= n;
			}

			info.KidName = System.Text.Encoding.ASCII.GetString(kidBytes);


			fsSource.Seek(2, SeekOrigin.Current);
			// Read the source file into a byte array.
			byte[] animalBytes = new byte[1];
			numBytesToRead = 1;
			numBytesRead = 0;
			while (numBytesToRead > 0)
			{
				// Read may return anything from 0 to numBytesToRead.
				int n = fsSource.Read(animalBytes, numBytesRead, numBytesToRead);

				// Break when the end of the file is reached.
				if (n == 0)
					break;

				numBytesRead += n;
				numBytesToRead -= n;
			}

			info.Animal = (AnimalType)animalBytes[0];
			
				
			fsSource.Seek(5, SeekOrigin.Current);
			//fsSource.Seek(118, SeekOrigin.Begin);
			// Read the source file into a byte array.
			byte[] ringBytes = new byte[8];
			numBytesToRead = 8;
			numBytesRead = 0;
			while (numBytesToRead > 0)
			{
				// Read may return anything from 0 to numBytesToRead.
				int n = fsSource.Read(ringBytes, numBytesRead, numBytesToRead);

				// Break when the end of the file is reached.
				if (n == 0)
					break;

				numBytesRead += n;
				numBytesToRead -= n;
			}
			// Write the byte array to the other FileStream.
			bool[] ringBits = ringBytes.SelectMany(x => x.GetBits()).ToArray();
				
			bool[,] ringBitArray = ringBits.Split(8, 8);
			
			for (int i = 0; i < 8; i++)
			{
				for (int j = 0, k = 7; j < 8; j++, k--)
					info.RingBits[i, k] = ringBitArray[i, j];
			}
			return info;		
		}
	}
}

