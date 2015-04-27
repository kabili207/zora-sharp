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
using System.IO;
using System.Linq;
using System.Collections.Generic;

namespace Zyrenth.OracleHack
{
	/// <summary>
	/// Vba save file loader.
	/// </summary>
	public class VbaSaveFileLoader
	{
		/// <summary>
		/// Loads all the game data from the specified stream
		/// </summary>
		/// <returns>The all.</returns>
		/// <param name="fsSource">Fs source.</param>
		public static IEnumerable<GameInfo> LoadAll(Stream stream)
		{
			// These offset seem to be static for both versions, but I can't be certain.
			return new [] {
				Load(stream, 19), // Slot 1
				Load(stream, 1379), // Slot 2
				Load(stream, 2739) // Slot 3
			};
		}

		/// <summary>
		/// Loads a game info from the stream at the specified offset
		/// </summary>
		/// <param name="stream">Stream.</param>
		/// <param name="offset">Offset.</param>
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
			byte[] beatenBytes = new byte[1];
			byte[] ringBytes = new byte[8];


			stream.Seek(offset, SeekOrigin.Begin);

			stream.Read(versionBytes, 0, 1);

			stream.Seek(96, SeekOrigin.Begin);
			stream.Read(gameIdBytes, 0, 2);
			stream.Read(heroBytes, 0, 5);

			stream.Seek(2, SeekOrigin.Current);
			stream.Read(kidBytes, 0, 5);

			stream.Seek(1, SeekOrigin.Current);
			stream.Read(behaviorBytes, 0, 1);

			stream.Seek(2, SeekOrigin.Current);
			stream.Read(animalBytes, 0, 1);

			// These are mostly just a guess
			stream.Read(linkedBytes, 0, 1);
			stream.Read(heroQuestBytes, 0, 1);
			stream.Read(beatenBytes, 0, 1);

			stream.Seek(5, SeekOrigin.Current);
			stream.Read(ringBytes, 0, 8);

			info.Game = (Game)(versionBytes[0] & 1);
			info.GameID = BitConverter.ToInt16(gameIdBytes, 0);
			info.Hero = System.Text.Encoding.ASCII.GetString(heroBytes);
			info.Child = System.Text.Encoding.ASCII.GetString(kidBytes);
			info.Animal = (Animal)(animalBytes[0] & 7);
			info.IsLinkedGame = linkedBytes[0] == 0;
			info.IsHeroQuest = heroQuestBytes[0] == 0;
			info.Rings = (Rings)BitConverter.ToUInt64(ringBytes, 0);

			return info;
		}
	}
}

