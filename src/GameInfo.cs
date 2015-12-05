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
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web.Script.Serialization;

namespace Zyrenth.Zora
{
	/// <summary>
	/// Represents the user data for an individual game
	/// </summary>
	[Serializable]
	public class GameInfo : INotifyPropertyChanged
	{

		#region Fields

		string _hero = "\0\0\0\0\0";
		string _child = "\0\0\0\0\0";
		short _gameId = 0;
		byte _behavior = 0;
		byte _animal = 0;
		byte _agesSeasons = 0;
		bool _isHeroQuest = false;
		bool _isLinkedGame = false;
		long _rings = 0L;
		bool _wasGivenFreeRing = false;

		bool _unknown58 = false;
		bool _unknown59 = false;
		bool _unknown88 = false; // default true

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
				NotifyPropertyChanged("Game");
			}
		}

		/// <summary>
		/// Gets or sets the Quest type used for this user data
		/// </summary>
		public bool IsHeroQuest
		{
			get { return _isHeroQuest; }
			set
			{
				_isHeroQuest = value;
				NotifyPropertyChanged("IsHeroQuest");
			}
		}

		/// <summary>
		/// Gets or sets the Quest type used for this user data
		/// </summary>
		public bool IsLinkedGame
		{
			get { return _isLinkedGame; }
			set
			{
				_isLinkedGame = value;
				NotifyPropertyChanged("IsLinkedGame");
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
				NotifyPropertyChanged("GameID");
			}
		}

		/// <summary>
		/// Gets or sets the hero's name
		/// </summary>
		public string Hero
		{
			get { return _hero.Trim(' ', '\0'); }
			set
			{
				if (string.IsNullOrWhiteSpace(value))
					_hero = "\0\0\0\0\0";
				else
					_hero = value.TrimEnd().PadRight(5, '\0');
				NotifyPropertyChanged("Hero");
			}
		}

		/// <summary>
		/// Gets or sets the child's name
		/// </summary>
		public string Child
		{
			get { return _child.Trim(' ', '\0'); }
			set
			{
				if (string.IsNullOrWhiteSpace(value))
					_child = "\0\0\0\0\0";
				else
					_child = value.TrimEnd().PadRight(5, '\0');
				NotifyPropertyChanged("Child");
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
				NotifyPropertyChanged("Animal");
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
				NotifyPropertyChanged("Behavior");
			}
		}

		/// <summary>
		/// Gets or sets the value indicating if Vasu has given the player a free ring
		/// </summary>
		public bool WasGivenFreeRing
		{
			get { return _wasGivenFreeRing; }
			set
			{
				_wasGivenFreeRing = value;
				NotifyPropertyChanged("WasGivenFreeRing");
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
				NotifyPropertyChanged("Rings");
			}
		}

		/// <summary>
		/// Gets or sets the unknown flag at offset 58
		/// </summary>
		public bool Unknown58
		{
			get { return _unknown58; }
			set
			{
				_unknown58 = value;
				NotifyPropertyChanged("Unknown58");
			}
		}

		/// <summary>
		/// Gets or sets the unknown flag at offset 59
		/// </summary>
		public bool Unknown59
		{
			get { return _unknown59; }
			set
			{
				_unknown59 = value;
				NotifyPropertyChanged("Unknown59");
			}
		}

		/// <summary>
		/// Gets or sets the unknown flag at offset 88
		/// </summary>
		public bool Unknown88
		{
			get { return _unknown88; }
			set
			{
				_unknown88 = value;
				NotifyPropertyChanged("Unknown88");
			}
		}

		#endregion // Properties

		/// <summary>
		/// Sends a notification that a property has changed.
		/// </summary>
		/// <param name="propertyName">Name of the property.</param>
		/// <example>
		/// <code language="C#">
		/// private short _gameID = 0;
		/// public short GameID
		/// {
		///     get { return _gameId; }
		///     set
		///     {
		///         _gameId = value;
		///         NotifyPropertyChanged("GameID");
		///     }
		/// }
		/// </code>
		/// </example>
		protected void NotifyPropertyChanged(string propertyName)
		{
			if (PropertyChanged != null)
			{
				PropertyChanged(this,
					new PropertyChangedEventArgs(propertyName));
			}
		}

		#region File Saving/Loading methods

		/// <summary>
		/// Writes this game info out to the specified file
		/// </summary>
		/// <param name="filename">The file name</param>
		/// <example>
		/// <code language="C#">
		/// string file = @"C:\Users\Link\Documents\my_game.zora";
		/// GameInfo info = new GameInfo();
		/// info.Write(file);
		/// </code>
		/// </example>
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
		/// <example>
		/// This example demonstrates creating a .zora save file from a<see cref="GameInfo"/>
		/// object. If you have access to the file name you will be saving to, as in this
		/// example, you should consider using the <see cref="Write(string)"/> method instead.
		/// <code language="C#">
		/// GameInfo info = new GameInfo();
		/// string file = @"C:\Users\Link\Documents\my_game.zora";
		/// using (FileStream outFile = File.Create(file))
		/// {
		///     info.Write(fileStream);
		/// }
		/// </code>
		/// </example>
		public void Write(Stream stream)
		{
			using (var swriter = new StreamWriter(stream))
			{
				var serializer = new JavaScriptSerializer();
				serializer.RegisterConverters(new[] { new GameInfoJsonConverter() });
				string json = serializer.Serialize(this);
				swriter.WriteLine(json);
			}
		}

		/// <summary>
		/// Loads the game info from the specified file
		/// </summary>
		/// <param name="filename">The file name of the saved GameInfo</param>
		/// <returns>A GameInfo</returns>
		/// <example>
		/// <code language="C#">
		/// string file = @"C:\Users\Link\Documents\my_game.zora";
		/// GameInfo info = GameInfo.Load(file);
		/// </code>
		/// </example>
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
		/// <example>
		/// This example demonstrates creating a <see cref="GameInfo"/> object from a .zora
		/// save file. If you have access to the file name you will be loading from, as in
		/// this example, you should consider using the <see cref="Load(string)"/> method instead.
		/// <code language="C#">
		/// string file = @"C:\Users\Link\Documents\my_game.zora";
		/// using (FileStream fileStream = File.OpenRead(file))
		/// {
		///     GameInfo info = GameInfo.Load(fileStream);
		/// }
		/// </code>
		/// </example>
		public static GameInfo Load(Stream stream)
		{
			using (var sreader = new StreamReader(stream))
			{
				string json = sreader.ReadToEnd();
				return Parse(json);
			}
		}

		/// <summary>
		/// Parses the specified json.
		/// </summary>
		/// <param name="json">The json.</param>
		/// <returns>A game info object</returns>
		/// <example>
		/// <code language="C#">
		/// string json = @"
		/// {
		///    ""Game"": ""Ages"",
		///    ""GameID"": 14129,
		///    ""Hero"": ""Link"",
		///    ""Child"": ""Pip"",
		///    ""Animal"": ""Dimitri"",
		///    ""Behavior"": ""BouncyD"",
		///    ""IsLinkedGame"": true,
		///    ""IsHeroQuest"": false,
		///    ""Rings"": -9222246136947933182
		/// }";
		/// GameInfo info = GameInfo.Parse(json);
		/// </code>
		/// </example>
		public static GameInfo Parse(string json)
		{
			var serializer = new JavaScriptSerializer();
			serializer.RegisterConverters(new[] { new GameInfoJsonConverter() });
			return serializer.Deserialize<GameInfo>(json);
		}

		#endregion // File Saving/Loading methods
	}
}
