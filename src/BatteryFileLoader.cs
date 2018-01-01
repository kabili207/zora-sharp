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
using System.IO;
using System.Linq;
using System.Collections.Generic;

namespace Zyrenth.Zora
{
	/// <summary>
	/// Battery file loader.
	/// </summary>
	public class BatteryFileLoader
	{
		/// <summary>
		/// The offset for slot 1
		/// </summary>
		public const int Slot1Offset = 19;

		/// <summary>
		/// The offset for slot 2
		/// </summary>
		public const int Slot2Offset = 1379;

		/// <summary>
		/// The offset for slot 3
		/// </summary>
		public const int Slot3Offset = 2739;

		/// <summary>
		/// Loads all the game data from the specified file
		/// </summary>
		/// <returns>All of the game information in the save file</returns>
		/// <param name="filename">The input file path.</param>
		public static IEnumerable<GameInfo> LoadAll(string filename)
		{
			using (FileStream inFile = File.OpenRead(filename))
			{
				return LoadAll(inFile);
			}
		}

		/// <summary>
		/// Loads all the game data from the specified stream
		/// </summary>
		/// <returns>All of the game information in the save file</returns>
		/// <param name="stream">The input stream.</param>
		public static IEnumerable<GameInfo> LoadAll(Stream stream)
		{
			List<GameInfo> gameData = new List<GameInfo>();
			GameInfo tmp;

			// These offsets seem to be static for both versions, but I can't be certain.

			// Slot 1
			tmp = Load(stream, Slot1Offset);
			if (tmp != null)
				gameData.Add(tmp);

			// Slot 2
			tmp = Load(stream, Slot2Offset);
			if (tmp != null)
				gameData.Add(tmp);

			// Slot 3
			tmp = Load(stream, Slot3Offset);
			if (tmp != null)
				gameData.Add(tmp);


			return gameData;
		}

		/// <summary>
		/// Loads a game info from the file at the specified offset
		/// </summary>
		/// <param name="filename">File.</param>
		/// <param name="offset">Offset.</param>
		/// <returns>The game information at the specified offset</returns>
		/// <remarks>This method has only been tested with the US version of the games</remarks>
		public static GameInfo Load(string filename, int offset)
		{
			using (FileStream inFile = File.OpenRead(filename))
			{
				return Load(inFile, offset);
			}
		}

		/// <summary>
		/// Loads a game info from the stream at the specified offset
		/// </summary>
		/// <param name="stream">Stream.</param>
		/// <param name="offset">Offset.</param>
		/// <returns>The game information at the specified offset</returns>
		/// <remarks>This method has only been tested with the US version of the games</remarks>
		public static GameInfo Load(Stream stream, int offset)
		{
			GameInfo info = new GameInfo();

			byte[] versionBytes = new byte[1];
			byte[] gameIdBytes = new byte[2];
			byte[] heroBytes = new byte[5];
			byte[] kidBytes = new byte[5];
			byte[] behaviorBytes = new byte[1];
			byte[] animalBytes = new byte[1];
			byte[] linkedBytes = new byte[1];
			byte[] heroQuestBytes = new byte[1];
			byte[] freeRingBytes = new byte[1];
			byte[] ringBytes = new byte[8];

			stream.Seek(offset, SeekOrigin.Begin);

			stream.Read(versionBytes, 0, 1);

			// The version is represented by the char values '1' or '2'
			if (versionBytes[0] != 49 && versionBytes[0] != 50)
				return null;

			stream.Seek(76, SeekOrigin.Current);
			stream.Read(gameIdBytes, 0, 2);
			stream.Read(heroBytes, 0, 5);

			stream.Seek(2, SeekOrigin.Current);
			stream.Read(kidBytes, 0, 5);

			stream.Seek(1, SeekOrigin.Current);
			stream.Read(behaviorBytes, 0, 1);
			stream.Read(animalBytes, 0, 1);

			stream.Seek(1, SeekOrigin.Current);

			stream.Read(linkedBytes, 0, 1);
			stream.Read(heroQuestBytes, 0, 1);

			stream.Seek(1, SeekOrigin.Current);
			stream.Read(freeRingBytes, 0, 1);
			stream.Read(ringBytes, 0, 8);

			info.Game = versionBytes[0] == 49 ? Game.Seasons : Game.Ages;
			info.GameID = BitConverter.ToInt16(gameIdBytes, 0);
			info.Hero = System.Text.Encoding.ASCII.GetString(heroBytes);
			info.Child = System.Text.Encoding.ASCII.GetString(kidBytes);
			info.Behavior = (ChildBehavior)(behaviorBytes[0] & 15);
			info.Animal = (Animal)(animalBytes[0] & 15);
			info.IsLinkedGame = linkedBytes[0] == 1;
			info.IsHeroQuest = heroQuestBytes[0] == 1;
			info.Rings = (Rings)BitConverter.ToUInt64(ringBytes, 0);

			info.Unknown58 = (behaviorBytes[0] >> 4 & 1) == 1;
			info.Unknown59 = (behaviorBytes[0] >> 5 & 1) == 1;
			info.WasGivenFreeRing = freeRingBytes[0] == 1;

			return info;
		}
	}
}

