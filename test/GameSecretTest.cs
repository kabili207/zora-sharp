using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace Zyrenth.Zora.Tests
{
	[TestFixture]
	public class GameSecretTest
	{
        private const string desiredSecretString = "H~2:@ ←2♦yq GB3●( 6♥?↑6";
        private const string desiredSecretString_JP = "かね6ごわ 4さをれか さ7ちわも るこぴりふ";

		public static readonly GameSecret DesiredSecret = new GameSecret()
		{
			Region = GameRegion.US,
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

		public static readonly GameSecret DesiredSecret_JP = new GameSecret()
		{
			Region = GameRegion.JP,
			TargetGame = Game.Seasons,
			GameID = 0,
			Hero = "あしうま",
			Child = "",
			Animal = Animal.None,
			Behavior = 0,
			IsLinkedGame = false,
			IsHeroQuest = true,
			WasGivenFreeRing = false
		};
        private static readonly byte[] desiredSecretBytes = new byte[] {
			 4, 37, 51, 36, 63,
			61, 51, 10, 44, 39,
			 3,  0, 52, 21, 48,
			55,  9, 45, 59, 55
		};

		[Test]
		public void LoadSecretFromBytes()
		{
			var secret = new GameSecret();
			secret.Load(desiredSecretBytes, GameRegion.US);
			Assert.AreEqual(DesiredSecret, secret);
		}

		[Test]
		public void LoadSecretFromString()
		{
			var secret = new GameSecret();
			secret.Load(desiredSecretString, GameRegion.US);

			Assert.AreEqual(DesiredSecret, secret);
		}

		[Test]
		public void LoadSecretFromString_JP()
		{
			var secret = new GameSecret();
			secret.Load(desiredSecretString_JP, GameRegion.JP);

			Assert.AreEqual(DesiredSecret_JP, secret);
		}

		[Test]
		public void LoadFromGameInfo()
		{
			var secret = new GameSecret(GameInfoTest.DesiredInfo);
			Assert.AreEqual(DesiredSecret, secret);
		}

		[Test]
		public void TestToString()
		{
			string secret = DesiredSecret.ToString();
			Assert.AreEqual(desiredSecretString, secret);
		}

		[Test]
		public void TestToString_JP()
		{
			string secret = DesiredSecret_JP.ToString();
			Assert.AreEqual(desiredSecretString_JP, secret);
		}

		[Test]
		public void TestToBytes()
		{
			byte[] bytes = DesiredSecret.ToBytes();
			Assert.AreEqual(desiredSecretBytes, bytes);
		}

		[Test]
		public void TestEquals()
		{
			var s2 = new GameSecret()
			{
				Region = GameRegion.US,
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
			var s2 = new GameSecret()
			{
				Region = GameRegion.US,
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
		
		[Test]
		public void TestInvalidByteLoad()
		{
			var secret = new GameSecret();
			Assert.Throws<SecretException>(() => secret.Load((byte[])null, GameRegion.US));
			Assert.Throws<SecretException>(() => secret.Load(new byte[] { 0 }, GameRegion.US));
			Assert.Throws<InvalidChecksumException>(() => secret.Load("H~2:@ ←2♦yq GB3●( 6♥?↑b", GameRegion.US));
			Assert.Throws<ArgumentException>(() => {
				secret.Load("L~2:N @bB↑& hmRh= HHHH↑", GameRegion.US);
			});
		}
		
		[Test]
		public void TestPalValidity()
		{
			var g1 = new GameSecret() { Hero = "Link~", Child = "    ", Animal = Animal.Ricky };
			var g2 = new GameSecret() { Hero = "     ", Child = "Pip~", Animal = Animal.Ricky };
			var g3 = new GameSecret() { Hero = "Link", Child = "Pip", Animal = Animal.None };
			var g4 = new GameSecret() { Hero = "Link", Child = "Pip", Animal = Animal.Ricky };
			
			Assert.IsFalse(g1.IsValidForPAL(), "Hero check failed");
			Assert.IsFalse(g2.IsValidForPAL(), "Child check failed");
			Assert.IsFalse(g3.IsValidForPAL(), "Animal check failed");
			Assert.IsTrue(g4.IsValidForPAL(), "Both failed");
		}
		
		[Test]
		public void TestHashCode()
		{
			var s1 = new GameSecret() { Hero = "Link", Child = "Pip", Animal = Animal.Ricky };
			var s2 = new GameSecret() { Hero = "Link", Child = "Pip~", Animal = Animal.Ricky };
			var s3 = new GameSecret() { Hero = "Link", Child = "Pip", Animal = Animal.None };
			var s4 = new GameSecret() { Hero = "Link", Child = "Pip", Animal = Animal.Ricky };

            // Because using mutable objects as a key is an awesome idea...
            var dict = new Dictionary<GameSecret, bool>
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
			var g = new GameSecret();
			g.PropertyChanged += (s, e) => { hit = true; };
			g.GameID = 42;
			Assert.IsTrue(hit);
		}
	}
}

