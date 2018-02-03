/*
 *  Copyright © 2013-2018, Amy Nagle.
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
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Dynamic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Zyrenth.Zora
{
	/// <summary>
	/// A javascript converter for the <see cref="GameInfo"/> class
	/// </summary>
	public class GameInfoJsonConverter
	{

		/// <summary>
		/// Converts the provided dictionary into an object of the specified type.
		/// </summary>
		/// <param name="dictionary">An <see cref="T:System.Collections.Generic.IDictionary`2" /> instance of property data stored as name/value pairs.</param>
		/// <returns>
		/// The deserialized game data.
		/// </returns>
		/// <exception cref="ArgumentNullException">dictionary</exception>
		public GameInfo Deserialize(IDictionary<string, object> dictionary)
		{
			if (dictionary == null)
				throw new ArgumentNullException("dictionary");

			GameInfo info = new GameInfo();

			info.Region = dictionary.ReadValue<GameRegion>("Region");
			info.Game = dictionary.ReadValue<Game>("Game");
			info.GameID = dictionary.ReadValue<short>("GameID");
			info.Hero = dictionary.ReadValue<string>("Hero");
			info.Child = dictionary.ReadValue<string>("Child");
			info.Animal = dictionary.ReadValue<Animal>("Animal");
			info.Behavior = dictionary.ReadValue<byte>("Behavior");
			info.IsHeroQuest = dictionary.ReadValue<bool>("IsHeroQuest");
			info.IsLinkedGame = dictionary.ReadValue<bool>("IsLinkedGame");
			info.WasGivenFreeRing = dictionary.ReadValue<bool>("WasGivenFreeRing");
			info.Rings = (Rings)dictionary.ReadValue<long>("Rings");

			return info;
		}

		/// <summary>
		/// Builds a dictionary of name/value pairs.
		/// </summary>
		/// <param name="info">The game data to serialize.</param>
		/// <returns>
		/// An object that contains key/value pairs that represent the object’s data.
		/// </returns>
		/// <exception cref="ArgumentException">Invalid type;info</exception>
		public IDictionary<string, object> Serialize(GameInfo info)
		{
			if (info == null)
				throw new ArgumentNullException("info cannot be null");

			var dict = new Dictionary<string, object>();

			dict["Region"] = info.Region;
			dict["Game"] = info.Game.ToString();
			dict["GameID"] = info.GameID;
			dict["Hero"] = info.Hero;
			dict["Child"] = info.Child;
			dict["Animal"] = info.Animal.ToString();
			dict["Behavior"] = info.Behavior.ToString();
			dict["IsHeroQuest"] = info.IsHeroQuest;
			dict["IsLinkedGame"] = info.IsLinkedGame;
			dict["WasGivenFreeRing"] = info.WasGivenFreeRing;
			dict["Rings"] = (long)info.Rings;

			return dict;
		}

	}
}

