using System;
using System.Collections.Generic;
using Xunit;
using Xunit.Sdk;

namespace Zyrenth.Zora.Tests
{
	public class MemorySecretTest
	{
		private const string desiredSecretString = "6●sW↑";
		private const string desiredSecretString_JP = "ぼつき3し";
		private static readonly MemorySecret desiredSecret = new MemorySecret()
		{
			Region = GameRegion.US,
			TargetGame = Game.Ages,
			GameID = 14129,
			Memory = Memory.ClockShopKingZora,
			IsReturnSecret = true
		};
		private static readonly MemorySecret desiredSecret_JP = new MemorySecret()
		{
			Region = GameRegion.JP,
			TargetGame = Game.Seasons,
			GameID = 15963,
			Memory = Memory.DiverPlen,
			IsReturnSecret = false
		};
		private static readonly byte[] desiredSecretBytes = new byte[] {
			55, 21, 41, 18, 59
		};
		private static readonly byte[] desiredSecretBytes_JP = new byte[] {
			61,  5, 28, 24, 7
		};
		private static readonly byte[] desiredSecretBytes_JP_Weird = new byte[] {
			31, 12, 34, 9, 15
		};

		
		[Fact]
		public void LoadSecretFromBytes()
		{
			var secret = new MemorySecret();
			secret.Load(desiredSecretBytes, GameRegion.US);
			Assert.Equal(desiredSecret, secret);
		}

		
		[Fact]
		public void LoadSecretFromBytes_JP()
		{
			var secret = new MemorySecret();
			secret.Load(desiredSecretBytes_JP, GameRegion.JP);
			Assert.Equal(desiredSecret_JP, secret);
		}

		
		[Fact]
		public void LoadSecretFromString()
		{
			var secret = new MemorySecret();
			secret.Load(desiredSecretString, GameRegion.US);
			Assert.Equal(desiredSecret, secret);
		}

		
		[Fact]
		public void LoadSecretFromString_JP()
		{
			var secret = new MemorySecret();
			secret.Load(desiredSecretString_JP, GameRegion.JP);
			Assert.Equivalent(desiredSecret_JP, secret);
		}

		
		[Fact]
		public void LoadFromGameInfoConstuct()
		{
			var secret = new MemorySecret(GameInfoTest.DesiredInfo, Memory.ClockShopKingZora, true);
			Assert.Equal(desiredSecret, secret);
		}

		
		[Fact]
		public void TestToString()
		{
			string secret = desiredSecret.ToString();
			Assert.Equal(desiredSecretString, secret);
		}

		
		[Fact]
		public void TestToString_JP()
		{
			string secret = desiredSecret_JP.ToString();
			Assert.Equivalent(desiredSecretString_JP, secret);
		}

		
		[Fact]
		public void TestWeirdBytes()
		{
			var secret = new MemorySecret();
			Assert.Throws<InvalidChecksumException>(() =>
			{
				secret.Load(desiredSecretBytes_JP_Weird, GameRegion.JP);
			});
		}

		
		[Fact]
		public void TestToBytes()
		{
			byte[] bytes = desiredSecret.ToBytes();
			Assert.Equivalent(desiredSecretBytes, bytes);
		}

		
		[Fact]
		public void TestEquals()
		{
			var s2 = new MemorySecret()
			{
				Region = GameRegion.US,
				TargetGame = Game.Ages,
				GameID = 14129,
				Memory = Memory.ClockShopKingZora,
				IsReturnSecret = true
			};

			Assert.Equal(desiredSecret, s2);
		}

		
		[Fact]
		public void TestNotEquals()
		{
			Assert.NotEqual(desiredSecret, new MemorySecret());
		}

		
		[Fact]
		public void TestInvalidByteLoad()
		{
			var secret = new MemorySecret();
			Assert.Throws<ArgumentNullException>(() => secret.Load((byte[])null, GameRegion.US));
			Assert.Throws<SecretException>(() => secret.Load(new byte[] { 0 }, GameRegion.US));
			Assert.Throws<InvalidChecksumException>(() => secret.Load("6●sWh", GameRegion.US));
			Assert.Throws<ArgumentException>(() =>
			{
				secret.Load("H~2:♥", GameRegion.US);
			});
		}

		
		[Fact]
		public void TestHashCode()
		{
			var r1 = new MemorySecret(Game.Ages, GameRegion.US, 1234, Memory.DiverPlen, true);
			var r2 = new MemorySecret(Game.Ages, GameRegion.JP, 1596, Memory.DiverPlen, true);

			var r3 = new MemorySecret(Game.Ages, GameRegion.US, 1234, Memory.DiverPlen, false);
			var r4 = new MemorySecret(Game.Ages, GameRegion.US, 1234, Memory.DiverPlen, true);

			// Because using mutable objects as a key is an awesome idea...
			IDictionary<MemorySecret,bool> dict = new Dictionary<MemorySecret, bool>
			{
				{ r1, true },
				{ r2, true }
			};

			Assert.True(Assert.Contains(r4, dict));
			var actual = Record.Exception(() => Assert.Contains(r3, dict));
			var ex = Assert.IsType<ContainsException>(actual);
		}

		[Fact]
		public void TestNotifyPropChanged()
		{
			bool hit = false;
			var g = new MemorySecret();
			g.PropertyChanged += (s, e) => { hit = true; };
			g.GameID = 42;
			Assert.True(hit);
		}
	}
}

