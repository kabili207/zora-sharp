using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace Zyrenth.Zora.Tests
{
	[TestFixture]
	public class MemorySecretTest
	{
        private const string DesiredSecretString = "6●sW↑";
        private const string DesiredSecretString_JP = "ぼつき3し";
        private static readonly MemorySecret DesiredSecret = new MemorySecret()
		{
			Region = GameRegion.US,
			TargetGame = Game.Ages,
			GameID = 14129,
			Memory = Memory.ClockShopKingZora,
			IsReturnSecret = true
		};
        private static readonly MemorySecret DesiredSecret_JP = new MemorySecret()
		{
			Region = GameRegion.JP,
			TargetGame = Game.Seasons,
			GameID = 15963,
			Memory = Memory.DiverPlen,
			IsReturnSecret = false
		};
        private static readonly byte[] DesiredSecretBytes = new byte[] {
			55, 21, 41, 18, 59
		};
        private static readonly byte[] DesiredSecretBytes_JP = new byte[] {
			61,  5, 28, 24, 7
		};
        private static readonly byte[] DesiredSecretBytes_JP_Weird = new byte[] {
			31, 12, 34, 9, 15
		};

		[Test]
		public void LoadSecretFromBytes()
		{
			MemorySecret secret = new MemorySecret();
			secret.Load(DesiredSecretBytes, GameRegion.US);
			Assert.AreEqual(DesiredSecret, secret);
		}

		[Test]
		public void LoadSecretFromBytes_JP()
		{
			MemorySecret secret = new MemorySecret();
			secret.Load(DesiredSecretBytes_JP, GameRegion.JP);
			Assert.AreEqual(DesiredSecret_JP, secret);
		}

		[Test]
		public void LoadSecretFromString()
		{
			MemorySecret secret = new MemorySecret();
			secret.Load(DesiredSecretString, GameRegion.US);
			Assert.AreEqual(DesiredSecret, secret);
		}

		[Test]
		public void LoadFromGameInfo()
		{
			MemorySecret secret = new MemorySecret()
			{
				Region = GameRegion.US,
				Memory = Memory.ClockShopKingZora,
				IsReturnSecret = true
			};
			secret.Load(GameInfoTest.DesiredInfo);
			Assert.AreEqual(DesiredSecret, secret);
		}

		[Test]
		public void LoadFromGameInfoConstuct()
		{
			MemorySecret secret = new MemorySecret(GameInfoTest.DesiredInfo, Memory.ClockShopKingZora, true);
			Assert.AreEqual(DesiredSecret, secret);
		}

		[Test]
		public void TestToString()
		{
			string secret = DesiredSecret.ToString();
			Assert.AreEqual(DesiredSecretString, secret);
		}

		[Test]
		public void TestWeirdBytes()
		{
			MemorySecret secret = new MemorySecret();
			Assert.Throws<UnknownMemoryException>(() =>
			{
				secret.Load(DesiredSecretBytes_JP_Weird, GameRegion.JP);
			});
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
			MemorySecret s2 = new MemorySecret()
			{
				Region = GameRegion.US,
				TargetGame = Game.Ages,
				GameID = 14129,
				Memory = Memory.ClockShopKingZora,
				IsReturnSecret = true
			};

			Assert.AreEqual(DesiredSecret, s2);
		}

		[Test]
		public void TestNotEquals()
		{
			MemorySecret s2 = new MemorySecret()
			{
				Region = GameRegion.US,
				TargetGame = Game.Ages,
				GameID = 14129,
				Memory = Memory.GraveyardFairy,
				IsReturnSecret = true
			};

			Assert.AreNotEqual(DesiredSecret, s2);
			Assert.AreNotEqual(DesiredSecret, null);
			Assert.AreNotEqual(DesiredSecret, "");
		}
		
		[Test]
		public void TestInvalidByteLoad()
		{
			MemorySecret secret = new MemorySecret();
			Assert.Throws<SecretException>(() => secret.Load((byte[])null, GameRegion.US));
			Assert.Throws<SecretException>(() => secret.Load(new byte[] { 0 }, GameRegion.US));
			Assert.Throws<InvalidChecksumException>(() => secret.Load("6●sWh", GameRegion.US));
			Assert.Throws<ArgumentException>(() =>
			{
				secret.Load("H~2:♥", GameRegion.US);
			});
		}

		[Test]
		public void TestHashCode()
		{
			MemorySecret r1 = new MemorySecret(Game.Ages, GameRegion.US, 1234, Memory.DiverPlen, true);
			MemorySecret r2 = new MemorySecret(Game.Ages, GameRegion.JP, 1596, Memory.DiverPlen, true);

			MemorySecret r3 = new MemorySecret(Game.Ages, GameRegion.US, 1234, Memory.DiverPlen, false);
			MemorySecret r4 = new MemorySecret(Game.Ages, GameRegion.US, 1234, Memory.DiverPlen, true);

			// Because using mutable objects as a key is an awesome idea...
			Dictionary<MemorySecret, bool> dict = new Dictionary<MemorySecret, bool>();
			dict.Add(r1, true);
			dict.Add(r2, true);

			Assert.That(dict, !Contains.Key(r3));
			Assert.That(dict, Contains.Key(r4));
		}
		
		[Test]
		public void TestNotifyPropChanged()
		{
			bool hit = false;
			MemorySecret g = new MemorySecret();
			g.PropertyChanged += (s, e) => { hit = true; };
			g.GameID = 42;
			Assert.IsTrue(hit);
		}
	}
}

