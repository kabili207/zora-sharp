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
			Game = Game.Ages,
			GameID = 14129,
			Hero = "Link",
			Child = "Pip",
			Animal = Animal.Dimitri,
			Behavior = ChildBehavior.BouncyD,
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
				""Game"": ""Ages"",
				""GameID"": 14129,
				""Hero"": ""Link"",
				""Child"": ""Pip"",
				""Animal"": ""Dimitri"",
				""Behavior"": ""BouncyD"",
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
	}
}
