using NUnit.Framework;
using System;

namespace Zyrenth.Zora.Tests
{
	[TestFixture]
	public class GameSecretTest
	{
		const string DesiredSecretString = "H~2:@ ←2♦yq GB3●( 6♥?↑6";

		static readonly GameSecret DesiredSecret = new GameSecret(GameRegion.US)
		{
			TargetGame = Game.Ages,
			GameID = 14129,
			Hero = "Link",
			Child = "Pip",
			Animal = Animal.Dimitri,
			Behavior = 4,
			IsLinkedGame = true,
			IsHeroQuest = false,
			WasGivenFreeRing = true
		};
		
		static readonly byte[] DesiredSecretBytes = new byte[] {
			4, 37, 51, 36, 63,
			61, 51, 10, 44, 39,
			3,  0, 52, 21, 48,
			55,  9, 45, 59, 55
		};

		[Test]
		public void LoadSecretFromBytes()
		{
			GameSecret secret = new GameSecret(GameRegion.US);
			secret.Load(DesiredSecretBytes);
			Assert.AreEqual(DesiredSecret, secret);
		}

		[Test]
		public void LoadSecretFromString()
		{
			GameSecret secret = new GameSecret(GameRegion.US);
			secret.Load(DesiredSecretString);

			Assert.AreEqual(DesiredSecret, secret);
		}

		[Test]
		public void LoadFromGameInfo()
		{
			GameSecret secret = new GameSecret(GameRegion.US);
			secret.Load(GameInfoTest.DesiredInfo);
			Assert.AreEqual(DesiredSecret, secret);
		}

		[Test]
		public void TestToString()
		{
			string secret = DesiredSecret.ToString();
			Assert.AreEqual(DesiredSecretString, secret);
		}

		[Test]
		public void TestToBytes()
		{
			byte[] bytes = DesiredSecret.ToBytes();
			Assert.AreEqual(DesiredSecretBytes, bytes);
		}

		[Test]
		public void TestEquals()
		{
			GameSecret s2 = new GameSecret(GameRegion.US)
			{
				TargetGame = Game.Ages,
				GameID = 14129,
				Hero = "Link",
				Child = "Pip",
				Animal = Animal.Dimitri,
				Behavior = 4,
				IsLinkedGame = true,
				IsHeroQuest = false,
				WasGivenFreeRing = true
			};

			Assert.AreEqual(DesiredSecret, s2);
		}

		[Test]
		public void TestNotEquals()
		{
			GameSecret s2 = new GameSecret(GameRegion.US)
			{
				TargetGame = Game.Seasons,
				GameID = 14129,
				Hero = "Link",
				Child = "Pip",
				Animal = Animal.Dimitri,
				Behavior = 4,
				IsLinkedGame = true,
				IsHeroQuest = false,
				WasGivenFreeRing = true
			};

			Assert.AreNotEqual(DesiredSecret, s2);
			Assert.AreNotEqual(DesiredSecret, null);
			Assert.AreNotEqual(DesiredSecret, "");
		}
	}
}

