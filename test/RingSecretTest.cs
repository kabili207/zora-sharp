using System;
using System.Collections.Generic;
using Xunit;
using Xunit.Sdk;

namespace Zyrenth.Zora.Tests
{
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

		
		[Fact]
		public void LoadSecretFromBytes()
		{
			var secret = new RingSecret();
			secret.Load(desiredSecretBytes, GameRegion.US);
			Assert.Equal(DesiredSecret, secret);
		}

		
		[Fact]
		public void LoadSecretFromString()
		{
			var secret = new RingSecret();
			secret.Load(desiredSecretString, GameRegion.US);
			Assert.Equal(DesiredSecret, secret);
		}

		
		[Fact]
		public void LoadFromGameInfo()
		{
			var secret = new RingSecret(GameInfoTest.DesiredInfo);
			Assert.Equal(DesiredSecret, secret);
		}

		
		[Fact]
		public void TestToString()
		{
			string secret = DesiredSecret.ToString();
			Assert.Equal(desiredSecretString, secret);
		}

		
		[Fact]
		public void TestToBytes()
		{
			byte[] bytes = DesiredSecret.ToBytes();
			Assert.Equal(desiredSecretBytes, bytes);
		}

		
		[Fact]
		public void TestEquals()
		{
			var s2 = new RingSecret()
			{
				Region = GameRegion.US,
				GameID = 14129,
				Rings = Rings.PowerRingL1 | Rings.DoubleEdgeRing | Rings.ProtectionRing
			};

			Assert.Equal(DesiredSecret, s2);
		}

		
		[Fact]
		public void TestNotEquals()
		{
			var s2 = new RingSecret()
			{
				Region = GameRegion.US,
				GameID = 14129,
				Rings = Rings.BlueJoyRing | Rings.BombproofRing | Rings.HundredthRing
			};

			Assert.NotEqual(s2, DesiredSecret);
			Assert.NotNull(DesiredSecret);
		}

		
		[Fact]
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

		
		[Fact]
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
			var ex = Record.Exception(() => s3.UpdateGameInfo(info, true));
			Assert.Null(ex);
			Assert.Equal(info, GameInfoTest.DesiredInfo);
		}

		
		[Fact]
		public void TestRingCount()
		{
			Assert.Equal(3, DesiredSecret.RingCount());
			Assert.Equal(0, new RingSecret().RingCount());
		}

		[Fact]
		public void TestHashCode()
		{
			var r1 = new RingSecret(1234, GameRegion.US, Rings.All);
			var r2 = new RingSecret(5632, GameRegion.JP, Rings.BlueRing);

			var r3 = new RingSecret(9876, GameRegion.US, Rings.All);
			var r4 = new RingSecret(1234, GameRegion.US, Rings.All);

			// Because using mutable objects as a key is an awesome idea...
			IDictionary<RingSecret, bool> dict = new Dictionary<RingSecret, bool>
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
			var g = new RingSecret();
			g.PropertyChanged += (s, e) => { hit = true; };
			g.GameID = 42;
			Assert.True(hit);
		}
	}
}

