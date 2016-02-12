using NUnit.Framework;
using System;

namespace Zyrenth.Zora.Tests
{
	[TestFixture]
	public class RingSecretTest
	{
		const string DesiredSecretString = "L~2:N @bB↑& hmRh=";

		static readonly RingSecret DesiredSecret = new RingSecret()
		{
			GameID = 14129,
			Rings = Rings.PowerRingL1 | Rings.DoubleEdgeRing | Rings.ProtectionRing
		};

		static readonly byte[] DesiredSecretBytes = new byte[] {
			6, 37, 51, 36, 13,
			63, 26,  0, 59, 47,
			30, 32, 15, 30, 49
		};

		[Test]
		public void LoadSecretFromBytes()
		{
			RingSecret secret = new RingSecret();
			secret.Load(DesiredSecretBytes);
			Assert.AreEqual(DesiredSecret, secret);
		}

		[Test]
		public void LoadSecretFromString()
		{
			RingSecret secret = new RingSecret();
			secret.Load(DesiredSecretString);
			Assert.AreEqual(DesiredSecret, secret);
		}

		[Test]
		public void LoadFromGameInfo()
		{
			RingSecret secret = new RingSecret();
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
			RingSecret s2 = new RingSecret()
			{
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
				GameID = 14129,
				Rings = Rings.BlueJoyRing | Rings.BombproofRing | Rings.HundredthRing
			};

			Assert.AreNotEqual(DesiredSecret, s2);
		}
	}
}

