using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace Zyrenth.Zora.Tests
{
	[TestFixture]
	public class RingSecretTest
	{
        private const string DesiredSecretString = "L~2:N @bB↑& hmRh=";

		public static readonly RingSecret DesiredSecret = new RingSecret()
		{
			Region = GameRegion.US,
			GameID = 14129,
			Rings = Rings.PowerRingL1 | Rings.DoubleEdgeRing | Rings.ProtectionRing
		};
        private static readonly byte[] DesiredSecretBytes = new byte[] {
			6, 37, 51, 36, 13,
			63, 26,  0, 59, 47,
			30, 32, 15, 30, 49
		};

		[Test]
		public void LoadSecretFromBytes()
		{
			RingSecret secret = new RingSecret();
			secret.Load(DesiredSecretBytes, GameRegion.US);
			Assert.AreEqual(DesiredSecret, secret);
		}

		[Test]
		public void LoadSecretFromString()
		{
			RingSecret secret = new RingSecret();
			secret.Load(DesiredSecretString, GameRegion.US);
			Assert.AreEqual(DesiredSecret, secret);
		}

		[Test]
		public void LoadFromGameInfo()
		{
			RingSecret secret = new RingSecret(GameInfoTest.DesiredInfo);
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
			RingSecret s2 = new RingSecret()
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
			RingSecret s2 = new RingSecret()
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
			RingSecret secret = new RingSecret();
			Assert.Throws<SecretException>(() => secret.Load((byte[])null, GameRegion.US));
			Assert.Throws<SecretException>(() => secret.Load(new byte[] { 0 }, GameRegion.US));
			Assert.Throws<InvalidChecksumException>(() => secret.Load("L~2:N @bB↑& hmRhh", GameRegion.US));
			Assert.Throws<ArgumentException>(() => {
				secret.Load("H~2:@ ←2♦yq GB3●9", GameRegion.US);
			});
		}

		[Test]
		public void UpdateGameInfo()
		{
			GameInfo info = new GameInfo()
			{
				Region = GameRegion.US,
				GameID = 14129,
				Rings = Rings.PowerRingL1
			};

			// Mismatched region
			RingSecret s1 = new RingSecret()
			{
				Region = GameRegion.JP,
				GameID = 14129,
				Rings = Rings.DoubleEdgeRing | Rings.ProtectionRing
			};

			// Mismatched game ID
			RingSecret s2 = new RingSecret()
			{
				Region = GameRegion.US,
				GameID = 1,
				Rings = Rings.DoubleEdgeRing | Rings.ProtectionRing
			};

			RingSecret s3 = new RingSecret()
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
			RingSecret r1 = new RingSecret(1234, GameRegion.US, Rings.All);
			RingSecret r2 = new RingSecret(5632, GameRegion.JP, Rings.BlueRing);
			
			RingSecret r3 = new RingSecret(9876, GameRegion.US, Rings.All);
			RingSecret r4 = new RingSecret(1234, GameRegion.US, Rings.All);
			
			// Because using mutable objects as a key is an awesome idea...
			Dictionary<RingSecret, bool> dict = new Dictionary<RingSecret, bool>();
			dict.Add(r1, true);
			dict.Add(r2, true);
			
			Assert.That(dict, !Contains.Key(r3));
			Assert.That(dict, Contains.Key(r4));
		}

		[Test]
		public void TestNotifyPropChanged()
		{
			bool hit = false;
			RingSecret g = new RingSecret();
			g.PropertyChanged += (s, e) => { hit = true; };
			g.GameID = 42;
			Assert.IsTrue(hit);
		}
	}
}

