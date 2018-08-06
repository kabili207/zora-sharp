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

			var parsed = GameInfo.Parse(json);

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

			var partialInfo = new GameInfo()
			{
				Region = GameRegion.US,
				Game = Game.Ages,
				GameID = 14129,
				Hero = "Link",
				IsLinkedGame = true,
				IsHeroQuest = false,
				WasGivenFreeRing = true
			};

			var parsed = GameInfo.Parse(json);

			Assert.AreEqual(partialInfo, parsed);
		}

		[Test]
		public void TestReadAndWriteFile()
		{
			string outFile = Path.GetTempFileName();
			try
			{
				DesiredInfo.Write(outFile);
				var read = GameInfo.Load(outFile);
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
			var info = new GameInfo();
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
			var s2 = new GameInfo()
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
			var s2 = new GameInfo()
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
			var s1 = new GameInfo() { Hero = "Link", Child = "Pip", Animal = Animal.Ricky };
			var s2 = new GameInfo() { Hero = "Link", Child = "Pip~", Animal = Animal.Ricky };
			var s3 = new GameInfo() { Hero = "Link", Child = "Pip", Animal = Animal.None };
			var s4 = new GameInfo() { Hero = "Link", Child = "Pip", Animal = Animal.Ricky };

			// Because using mutable objects as a key is an awesome idea...
			var dict = new Dictionary<GameInfo, bool>
			{
				{ s1, true },
				{ s2, true }
			};

			Assert.That(dict, !Contains.Key(s3));
			Assert.That(dict, Contains.Key(s4));
		}

		[Test]
		public void TestNotifyPropChanged()
		{
			bool hit = false;
			var g = new GameInfo();
			g.PropertyChanged += (s, e) => { hit = true; };
			g.GameID = 42;
			Assert.IsTrue(hit);
		}
	}
}
