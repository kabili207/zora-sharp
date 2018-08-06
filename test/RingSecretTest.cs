using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace Zyrenth.Zora.Tests
{
	[TestFixture]
	public class RingSecretTest
	{
		private const string desiredSecretString = "L~2:N @bB↑& hmRh=";

		public static readonly RingSecret DesiredSecret = new RingSecret()
		{
			Region = GameRegion.US,
			GameID = 14129,
			Rings = Rings.PowerRingL1 | Rings.DoubleEdgeRing | Rings.ProtectionRing
		};
		private static readonly byte[] desiredSecretBytes = new byte[] {
			6, 37, 51, 36, 13,
			63, 26,  0, 59, 47,
			30, 32, 15, 30, 49
		};

		[Test]
		public void LoadSecretFromBytes()
		{
			var secret = new RingSecret();
			secret.Load(desiredSecretBytes, GameRegion.US);
			Assert.AreEqual(DesiredSecret, secret);
		}

		[Test]
		public void LoadSecretFromString()
		{
			var secret = new RingSecret();
			secret.Load(desiredSecretString, GameRegion.US);
			Assert.AreEqual(DesiredSecret, secret);
		}

		[Test]
		public void LoadFromGameInfo()
		{
			var secret = new RingSecret(GameInfoTest.DesiredInfo);
			Assert.AreEqual(DesiredSecret, secret);
		}

		[Test]
		public void TestToString()
		{
			string secret = DesiredSecret.ToString();
			Assert.AreEqual(desiredSecretString, secret);
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
			var s2 = new RingSecret()
			{
				Region = GameRegion.US,
				GameID = 14129,
				Rings = Rings.PowerRingL1 | Rings.DoubleEdgeRing | Rings.ProtectionRing
			};

			Assert.AreEqual(DesiredSecret, s2);
		}

		[Test]
		public void TestNotEquals()
		{
			var s2 = new RingSecret()
			{
				Region = GameRegion.US,
				GameID = 14129,
				Rings = Rings.BlueJoyRing | Rings.BombproofRing | Rings.HundredthRing
			};

			Assert.AreNotEqual(DesiredSecret, s2);
			Assert.AreNotEqual(DesiredSecret, null);
			Assert.AreNotEqual(DesiredSecret, "");
		}

		[Test]
		public void TestInvalidByteLoad()
		{
			var secret = new RingSecret();
			Assert.Throws<SecretException>(() => secret.Load((byte[])null, GameRegion.US));
			Assert.Throws<SecretException>(() => secret.Load(new byte[] { 0 }, GameRegion.US));
			Assert.Throws<InvalidChecksumException>(() => secret.Load("L~2:N @bB↑& hmRhh", GameRegion.US));
			Assert.Throws<ArgumentException>(() =>
			{
				secret.Load("H~2:@ ←2♦yq GB3●9", GameRegion.US);
			});
		}

		[Test]
		public void UpdateGameInfo()
		{
			var info = new GameInfo()
			{
				Region = GameRegion.US,
				GameID = 14129,
				Rings = Rings.PowerRingL1
			};

			// Mismatched region
			var s1 = new RingSecret()
			{
				Region = GameRegion.JP,
				GameID = 14129,
				Rings = Rings.DoubleEdgeRing | Rings.ProtectionRing
			};

			// Mismatched game ID
			var s2 = new RingSecret()
			{
				Region = GameRegion.US,
				GameID = 1,
				Rings = Rings.DoubleEdgeRing | Rings.ProtectionRing
			};

			var s3 = new RingSecret()
			{
				Region = GameRegion.US,
				GameID = 14129,
				Rings = Rings.DoubleEdgeRing | Rings.ProtectionRing
			};

			GameSecretTest.DesiredSecret.UpdateGameInfo(info);

			Assert.Throws<SecretException>(() => s1.UpdateGameInfo(info, true));
			Assert.Throws<SecretException>(() => s2.UpdateGameInfo(info, true));
			Assert.DoesNotThrow(() => s3.UpdateGameInfo(info, true));
			Assert.AreEqual(GameInfoTest.DesiredInfo, info);
		}

		[Test]
		public void TestHashCode()
		{
			var r1 = new RingSecret(1234, GameRegion.US, Rings.All);
			var r2 = new RingSecret(5632, GameRegion.JP, Rings.BlueRing);

			var r3 = new RingSecret(9876, GameRegion.US, Rings.All);
			var r4 = new RingSecret(1234, GameRegion.US, Rings.All);

			// Because using mutable objects as a key is an awesome idea...
			var dict = new Dictionary<RingSecret, bool>
			{
				{ r1, true },
				{ r2, true }
			};

			Assert.That(dict, !Contains.Key(r3));
			Assert.That(dict, Contains.Key(r4));
		}

		[Test]
		public void TestNotifyPropChanged()
		{
			bool hit = false;
			var g = new RingSecret();
			g.PropertyChanged += (s, e) => { hit = true; };
			g.GameID = 42;
			Assert.IsTrue(hit);
		}
	}
}

