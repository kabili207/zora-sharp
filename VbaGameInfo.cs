using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;

namespace Zyrenth.OracleHack
{
	public class VbaGameInfo : GameInfo
	{

		public VbaGameInfo()
		{


		}

		public static VbaGameInfo Load(Stream fsSource)
		{
			VbaGameInfo info = new VbaGameInfo();

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

			info.Game = (Game)(versionBytes[0] & 1);


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

			info.GameID = BitConverter.ToInt16(gameIdBytes, 0);

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

			info.Hero = System.Text.Encoding.ASCII.GetString(heroBytes);


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

			info.Child = System.Text.Encoding.ASCII.GetString(kidBytes);


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

			info.Animal = (Animal)(animalBytes[0] & 7);


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
			ulong rings = BitConverter.ToUInt64(ringBytes, 0);
			info.Rings = (Rings)rings;

			return info;
		}
	}
}

