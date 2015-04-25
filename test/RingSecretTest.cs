using NUnit.Framework;
using System;

namespace Zyrenth.OracleHack.Tests
{
	[TestFixture]
	public class RingSecretTest
	{
		const string DesiredSecretString = "L~2:N @bB↑& hmRh=";
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

			Assert.AreEqual(14129, secret.GameID);
			Assert.AreEqual(Rings.PowerRingL1 | Rings.DoubleEdgeRing | Rings.ProtectionRing, secret.Rings);
		}

		[Test]
		public void LoadSecretFromString()
		{
			RingSecret secret = new RingSecret();
			secret.Load(DesiredSecretString);

			Assert.AreEqual(14129, secret.GameID);
			Assert.AreEqual(Rings.PowerRingL1 | Rings.DoubleEdgeRing | Rings.ProtectionRing, secret.Rings);
		}

		[Test]
		public void TestToString()
		{
			RingSecret secret = new RingSecret() {
				GameID = 14129,
				Rings = Rings.PowerRingL1 | Rings.DoubleEdgeRing | Rings.ProtectionRing
			};

			Assert.AreEqual(DesiredSecretString, secret.ToString());
		}

		[Test]
		public void TestToBytes()
		{
			RingSecret secret = new RingSecret() {
				GameID = 14129,
				Rings = Rings.PowerRingL1 | Rings.DoubleEdgeRing | Rings.ProtectionRing
			};

			Assert.AreEqual(DesiredSecretBytes, secret.ToBytes());
		}
	}
}

