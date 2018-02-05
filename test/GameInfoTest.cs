using NUnit.Framework;
using System;
using System.IO;
using System.Web.Script.Serialization;
using System.Collections.Generic;

namespace Zyrenth.Zora.Tests
{
	[TestFixture]
	public class GameInfoTest
	{
		public static readonly GameInfo DesiredInfo = new GameInfo()
		{
			Region = GameRegion.US,
			Game = Game.Ages,
			GameID = 14129,
			Hero = "Link",
			Child = "Pip",
			Animal = Animal.Dimitri,
			Behavior = 4,
			IsLinkedGame = true,
			IsHeroQuest = false,
			WasGivenFreeRing = true,
			Rings = Rings.PowerRingL1 | Rings.DoubleEdgeRing | Rings.ProtectionRing
		};

		[Test]
		public void TestParseJson()
		{
			string json = @"
			 {
				""Region"": ""US"",
				""Game"": ""Ages"",
				""GameID"": 14129,
				""Hero"": ""Link"",
				""Child"": ""Pip"",
				""Animal"": ""Dimitri"",
				""Behavior"": 4,
				""IsLinkedGame"": true,
				""IsHeroQuest"": false,
				""WasGivenFreeRing"": true,
				""Rings"": -9222246136947933182
			 }";
			
			GameInfo parsed = GameInfo.Parse(json);

			Assert.AreEqual(DesiredInfo, parsed);
		}

		[Test]
		public void TestParsePartialJson()
		{
			string json = @"
			 {
				""Region"": ""US"",
				""Game"": ""Ages"",
				""GameID"": 14129,
				""Hero"": ""Link"",
				""IsLinkedGame"": true,
				""IsHeroQuest"": false,
				""WasGivenFreeRing"": true
			 }";

			GameInfo partialInfo = new GameInfo()
			{
				Region = GameRegion.US,
				Game = Game.Ages,
				GameID = 14129,
				Hero = "Link",
				IsLinkedGame = true,
				IsHeroQuest = false,
				WasGivenFreeRing = true
			};

			GameInfo parsed = GameInfo.Parse(json);

			Assert.AreEqual(partialInfo, parsed);
		}

		[Test]
		public void TestReadAndWriteFile()
		{
			string outFile = Path.GetTempFileName();
			try
			{
				DesiredInfo.Write(outFile);
				GameInfo read = GameInfo.Load(outFile);
				Assert.AreEqual(DesiredInfo, read);
			}
			finally
			{
				File.Delete(outFile);
			}
		}

		[Test]
		public void UpdateGameInfo()
		{
			GameInfo info = new GameInfo();
			GameSecretTest.DesiredSecret.UpdateGameInfo(info);
			RingSecretTest.DesiredSecret.UpdateGameInfo(info, false);
			Assert.AreEqual(GameInfoTest.DesiredInfo, info);
		}
		
		[Test]
		public void NullJsonConvert()
		{
			var converter = new GameInfoJsonConverter();
			Assert.Throws<ArgumentNullException>(() => converter.Serialize(null));
			Assert.Throws<ArgumentNullException>(() => converter.Deserialize(null));
		}
		
		[Test]
		public void TestEquals()
		{
			GameInfo s2 = new GameInfo()
			{
				Region = GameRegion.US,
				Game = Game.Ages,
				GameID = 14129,
				Hero = "Link",
				Child = "Pip",
				Animal = Animal.Dimitri,
				Behavior = 4,
				IsLinkedGame = true,
				IsHeroQuest = false,
				WasGivenFreeRing = true,
				Rings = Rings.PowerRingL1 | Rings.DoubleEdgeRing | Rings.ProtectionRing
			};

			Assert.AreEqual(DesiredInfo, s2);
		}

		[Test]
		public void TestNotEquals()
		{
			GameInfo s2 = new GameInfo()
			{
				Region = GameRegion.US,
				Game = Game.Seasons,
				GameID = 14129,
				Hero = "",
				Child = "Pip",
				Animal = Animal.Dimitri,
				Behavior = 4,
				IsLinkedGame = true,
				IsHeroQuest = false,
				WasGivenFreeRing = true
			};

			Assert.AreNotEqual(DesiredInfo, s2);
			Assert.AreNotEqual(DesiredInfo, null);
			Assert.AreNotEqual(DesiredInfo, "");
		}

		[Test]
		public void TestHashCode()
		{
			GameInfo s1 = new GameInfo() { Hero = "Link", Child = "Pip", Animal = Animal.Ricky };
			GameInfo s2 = new GameInfo() { Hero = "Link", Child = "Pip~", Animal = Animal.Ricky };
			GameInfo s3 = new GameInfo() { Hero = "Link", Child = "Pip", Animal = Animal.None };
			GameInfo s4 = new GameInfo() { Hero = "Link", Child = "Pip", Animal = Animal.Ricky };

			// Because using mutable objects as a key is an awesome idea...
			Dictionary<GameInfo, bool> dict = new Dictionary<GameInfo, bool>();
			dict.Add(s1, true);
			dict.Add(s2, true);

			Assert.That(dict, !Contains.Key(s3));
			Assert.That(dict, Contains.Key(s4));
		}
		
		[Test]
		public void TestNotifyPropChanged()
		{
			bool hit = false;
			GameInfo g = new GameInfo();
			g.PropertyChanged += (s, e) => { hit = true; };
			g.GameID = 42;
			Assert.IsTrue(hit);
		}
	}
}
