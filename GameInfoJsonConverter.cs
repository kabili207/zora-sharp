using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Web.Script.Serialization;

namespace Zyrenth.OracleHack
{
	public class GameInfoJsonConverter : JavaScriptConverter
	{
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

			var sGame = ReadValue<string>(dictionary, "Game");
			var sAnimal = ReadValue<string>(dictionary, "Animal");
			var sBehavior = ReadValue<string>(dictionary, "Behavior");

			Game game;
			if (Enum.TryParse<Game>(sGame, out game))
				info.Game = game;
			Animal animal;
			if (Enum.TryParse<Animal>(sAnimal, out animal))
				info.Animal = animal;
			ChildBehavior behavior;
			if (Enum.TryParse<ChildBehavior>(sBehavior, out behavior))
				info.Behavior = behavior;


			return info;

		}

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

		public override IEnumerable<Type> SupportedTypes
		{
			get
			{
				return new ReadOnlyCollection<Type>(new [] { typeof(GameInfo) });
			}
		}

		private static T ReadValue<T>(IDictionary<string, object>  dict, string key) where T: IConvertible
		{
			object obj = dict[key];
			return (T)Convert.ChangeType(obj, typeof(T));
		}

	}
}

