using NUnit.Framework;
using System;

namespace Zyrenth.Zora.Tests
{
	[TestFixture]
	public class MemorySecretTest
	{
		const string DesiredSecretString = "6●sW↑";

		static readonly MemorySecret DesiredSecret = new MemorySecret()
		{
			Region = GameRegion.US,
			TargetGame = Game.Ages,
			GameID = 14129,
			Memory = Memory.ClockShopKingZora,
			IsReturnSecret = true
		};

		static readonly byte[] DesiredSecretBytes = new byte[] {
			55, 21, 41, 18, 59
		};

		[Test]
		public void LoadSecretFromBytes()
		{
			MemorySecret secret = new MemorySecret();
			secret.Load(DesiredSecretBytes, GameRegion.US);
			Assert.AreEqual(DesiredSecret, secret);
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
			Assert.Throws<ArgumentException>(() => {
				secret.Load("H~2:♥", GameRegion.US);
			});
		}
	}
}

