using System.IO;
using System.Collections.Generic;
using Xunit;
using Xunit.Sdk;

namespace Zyrenth.Zora.Tests
{
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

		
		[Fact]
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

			Assert.Equal(DesiredInfo, parsed);
		}

		
		[Fact]
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

			Assert.Equal(partialInfo, parsed);
		}

		
		[Fact]
		public void TestReadAndWriteFile()
		{
			string outFile = Path.GetTempFileName();
			try
			{
				DesiredInfo.Write(outFile);
				var read = GameInfo.Load(outFile);
				Assert.Equal(DesiredInfo, read);
			}
			finally
			{
				File.Delete(outFile);
			}
		}

		
		[Fact]
		public void UpdateGameInfo()
		{
			var info = new GameInfo();
			GameSecretTest.DesiredSecret.UpdateGameInfo(info);
			RingSecretTest.DesiredSecret.UpdateGameInfo(info, false);
			Assert.Equal(DesiredInfo, info);
		}

		
		[Fact]
		public void TestRingCount()
		{
			Assert.Equal(3, DesiredInfo.RingCount());
			Assert.Equal(0, new GameInfo().RingCount());
		}

		
		[Fact]
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

			Assert.Equal(DesiredInfo, s2);
		}

		
		[Fact]
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

			Assert.NotEqual(DesiredInfo, s2);
		}

		
		[Fact]
		public void TestHashCode()
		{
			var s1 = new GameInfo() { Hero = "Link", Child = "Pip", Animal = Animal.Ricky };
			var s2 = new GameInfo() { Hero = "Link", Child = "Pip~", Animal = Animal.Ricky };
			var s3 = new GameInfo() { Hero = "Link", Child = "Pip", Animal = Animal.None };
			var s4 = new GameInfo() { Hero = "Link", Child = "Pip", Animal = Animal.Ricky };

			// Because using mutable objects as a key is an awesome idea...
			IDictionary<GameInfo, bool> dict = new Dictionary<GameInfo, bool>
			{
				{ s1, true },
				{ s2, true }
			};

			Assert.True(Assert.Contains(s4, dict));
			var actual = Record.Exception(() => Assert.Contains(s3, dict));
			var ex = Assert.IsType<ContainsException>(actual);
		}

		
		[Fact]
		public void TestNotifyPropChanged()
		{
			bool hit = false;
			var g = new GameInfo();
			g.PropertyChanged += (s, e) => { hit = true; };
			g.GameID = 42;
			Assert.True(hit);
		}
	}
}
