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
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Dynamic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web.Script.Serialization;

namespace Zyrenth.OracleHack
{
	/// <summary>
	/// A javascript converter for the <see cref="GameInfo"/> class
	/// </summary>
	public class GameInfoJsonConverter : JavaScriptConverter
	{
		/// <summary>
		/// Gets a collection of the supported types.
		/// </summary>
		public override IEnumerable<Type> SupportedTypes
		{
			get
			{
				return new[] { typeof(GameInfo) };
			}
		}

		/// <summary>
		/// Converts the provided dictionary into an object of the specified type.
		/// </summary>
		/// <param name="dictionary">An <see cref="T:System.Collections.Generic.IDictionary`2" /> instance of property data stored as name/value pairs.</param>
		/// <param name="type">The type of the resulting object.</param>
		/// <param name="serializer">The <see cref="T:System.Web.Script.Serialization.JavaScriptSerializer" /> instance.</param>
		/// <returns>
		/// The deserialized object.
		/// </returns>
		/// <exception cref="ArgumentNullException">dictionary</exception>
		public override object Deserialize(IDictionary<string, object> dictionary, Type type, JavaScriptSerializer serializer)
		{
			if (dictionary == null)
				throw new ArgumentNullException("dictionary");
			if (type != typeof(GameInfo))
				return null;

			GameInfo info = new GameInfo();

			info.Hero = ReadValue<string>(dictionary, "Hero");
			info.Child = ReadValue<string>(dictionary, "Child");
			info.IsHeroQuest = ReadValue<bool>(dictionary, "IsHeroQuest");
			info.IsLinkedGame = ReadValue<bool>(dictionary, "IsLinkedGame");
			info.GameID = ReadValue<short>(dictionary, "GameID");
			info.Rings = (Rings)ReadValue<long>(dictionary, "Rings");
			info.Game = ReadValue<Game>(dictionary, "Game");
			info.Animal = ReadValue<Animal>(dictionary, "Animal");
			info.Behavior = ReadValue<ChildBehavior>(dictionary, "Behavior");

			return info;

		}

		/// <summary>
		/// Builds a dictionary of name/value pairs.
		/// </summary>
		/// <param name="obj">The object to serialize.</param>
		/// <param name="serializer">The object that is responsible for the serialization.</param>
		/// <returns>
		/// An object that contains key/value pairs that represent the object’s data.
		/// </returns>
		/// <exception cref="ArgumentException">Invalid type;obj</exception>
		public override IDictionary<string, object> Serialize(object obj, JavaScriptSerializer serializer)
		{
			GameInfo info = obj as GameInfo;
			if (info == null)
				throw new ArgumentException("Invalid type", "obj");

			var dict = new Dictionary<string, object>();

			dict["Hero"] = info.Hero;
			dict["Child"] = info.Child;
			dict["GameID"] = info.GameID;
			dict["Game"] = info.Game.ToString();
			dict["Animal"] = info.Animal.ToString();
			dict["Behavior"] = info.Behavior.ToString();
			dict["IsLinkedGame"] = info.IsLinkedGame;
			dict["IsHeroQuest"] = info.IsHeroQuest;
			dict["Rings"] = (long)info.Rings;

			return dict;
		}

		private static T ReadValue<T>(IDictionary<string, object> dict, string key) where T : IConvertible
		{

			object obj = dict[key];
			Type type = typeof(T);
			T val = default(T);

			if (dict.ContainsKey(key))
			{
				if (type.IsEnum)
				{
					// The T constraints on our method conflict with those on Enum.TryParse<T>
					// so we have to use some black magic instead.
					MethodInfo method = typeof(Enum).GetMethod("TryParse",
						new Type[] { typeof(string), type.MakeByRefType() });

					var args = new object[] { ReadValue<string>(dict, key), default(T) };
					method.Invoke(null, args);

					val = (T)args[1];
				}
				else
				{
					val = (T)Convert.ChangeType(obj, type);
				}
			}

			return val;
		}

	}
}

