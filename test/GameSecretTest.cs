using NUnit.Framework;
using System;

namespace Zyrenth.OracleHack.Tests
{
	[TestFixture]
	public class GameSecretTest
	{
		
		const string DesiredSecretString = "H~2:@ ←2♦yq GB3●( 6♥?↑6";
		static readonly byte[] DesiredSecretBytes = new byte[] {
			4, 37, 51, 36, 63,
			61, 51, 10, 44, 39,
			3,  0, 52, 21, 48,
			55,  9, 45, 59, 55
		};

		[Test]
		public void LoadSecretFromBytes()
		{
			GameSecret secret = new GameSecret();
			secret.Load(DesiredSecretBytes);

			Assert.AreEqual("Link", secret.Hero);
			Assert.AreEqual("Pip", secret.Child);
			Assert.AreEqual(Game.Ages, secret.TargetGame);
			Assert.AreEqual(14129, secret.GameID);
			Assert.AreEqual(Animal.Dimitri, secret.Animal);
			Assert.AreEqual(ChildBehavior.BouncyD, secret.Behavior);
			Assert.AreEqual(true, secret.IsLinkedGame);
			Assert.AreEqual(false, secret.IsHeroQuest);
			Assert.AreEqual(true, secret.WasGivenFreeRing);
		}

		[Test]
		public void LoadSecretFromString()
		{
			GameSecret secret = new GameSecret();
			secret.Load(DesiredSecretString);

			Assert.AreEqual("Link", secret.Hero);
			Assert.AreEqual("Pip", secret.Child);
			Assert.AreEqual(Game.Ages, secret.TargetGame);
			Assert.AreEqual(14129, secret.GameID);
			Assert.AreEqual(Animal.Dimitri, secret.Animal);
			Assert.AreEqual(ChildBehavior.BouncyD, secret.Behavior);
			Assert.AreEqual(true, secret.IsLinkedGame);
			Assert.AreEqual(false, secret.IsHeroQuest);
			Assert.AreEqual(true, secret.WasGivenFreeRing);
		}

		[Test]
		public void TestToString()
		{
			GameSecret secret = new GameSecret() {
				TargetGame = Game.Ages,
				GameID = 14129,
				Hero = "Link",
				Child = "Pip",
				Animal = Animal.Dimitri,
				Behavior = ChildBehavior.BouncyD,
				IsLinkedGame = true,
				IsHeroQuest = false,
				WasGivenFreeRing = true
			};

			Assert.AreEqual(DesiredSecretString, secret.ToString());
		}

		[Test]
		public void TestToBytes()
		{
			GameSecret secret = new GameSecret() {
				TargetGame = Game.Ages,
				GameID = 14129,
				Hero = "Link",
				Child = "Pip",
				Animal = Animal.Dimitri,
				Behavior = ChildBehavior.BouncyD,
				IsLinkedGame = true,
				IsHeroQuest = false,
				WasGivenFreeRing = true
			};

			Assert.AreEqual(DesiredSecretBytes, secret.ToBytes());
		}
		
	}
}

