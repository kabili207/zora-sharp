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
			info.Game = ReadValue<Game>(dictionary, "Game");
			info.Animal = ReadValue<Animal>(dictionary, "Animal");
			info.Behavior = ReadValue<ChildBehavior>(dictionary, "Behavior");

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
				return new[] { typeof(GameInfo) };
			}
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

