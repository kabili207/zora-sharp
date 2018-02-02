using NUnit.Framework;
using System;
using System.IO;
using System.Web.Script.Serialization;

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
	}
}
