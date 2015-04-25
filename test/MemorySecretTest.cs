using NUnit.Framework;
using System;

namespace Zyrenth.OracleHack.Tests
{
	[TestFixture]
	public class MemorySecretTest
	{
		const string DesiredSecretString = "6●sW↑";
		static readonly byte[] DesiredSecretBytes = new byte[] {
			55, 21, 41, 18, 59
		};

		[Test]
		public void LoadSecretFromBytes()
		{
			MemorySecret secret = new MemorySecret();
			secret.Load(DesiredSecretBytes);
			
			Assert.AreEqual(Game.Ages, secret.TargetGame);
			Assert.AreEqual(14129, secret.GameID);
			Assert.AreEqual(Memory.ClockShopKingZora, secret.Memory);
			Assert.AreEqual(true, secret.IsReturnSecret);
		}

		[Test]
		public void LoadSecretFromString()
		{
			MemorySecret secret = new MemorySecret();
			secret.Load(DesiredSecretString);

			Assert.AreEqual(Game.Ages, secret.TargetGame);
			Assert.AreEqual(14129, secret.GameID);
			Assert.AreEqual(Memory.ClockShopKingZora, secret.Memory);
			Assert.AreEqual(true, secret.IsReturnSecret);
		}

		[Test]
		public void TestToString()
		{
			MemorySecret secret = new MemorySecret() {
				TargetGame = Game.Ages,
				GameID = 14129,
				Memory = Memory.ClockShopKingZora,
				IsReturnSecret = true
			};

			Assert.AreEqual(DesiredSecretString, secret.ToString());
		}

		[Test]
		public void TestToBytes()
		{
			MemorySecret secret = new MemorySecret() {
				TargetGame = Game.Ages,
				GameID = 14129,
				Memory = Memory.ClockShopKingZora,
				IsReturnSecret = true
			};

			Assert.AreEqual(DesiredSecretBytes, secret.ToBytes());
		}
	}
}

