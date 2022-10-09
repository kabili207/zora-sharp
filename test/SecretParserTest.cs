using Xunit;

namespace Zyrenth.Zora.Tests
{
	public class SecretParserTest
	{
		private const string desiredSecretString = "H~2:@ ←2♦yq GB3●) 6♥?↑4";
		private const string desiredSecretString_JP = "かね6ごわ 4さをれか さ7ちわも るこぴりふ";

		private static readonly byte[] desiredSecretBytes = new byte[] {
			4, 37, 51, 36, 63,
			61, 51, 10, 44, 39,
			3,  0, 52, 21, 50,
			55,  9, 45, 59, 53
		};

		private static readonly byte[] desiredSecretBytes_JP = new byte[] {
			1, 9, 41, 55, 30,
			60, 26, 43, 53, 1,
			26, 42, 48, 30, 45,
			37, 32, 58, 47, 25
		};

		[Fact]
		public void CreateString()
		{
			string testString = SecretParser.CreateString(desiredSecretBytes, GameRegion.US);
			Assert.Equal(desiredSecretString, testString);
		}

		[Fact]
		public void CreateString_JP()
		{
			string testString = SecretParser.CreateString(desiredSecretBytes_JP, GameRegion.JP);
			Assert.Equal(desiredSecretString_JP, testString);
		}

		[Theory]
		[InlineData(GameRegion.US, "H~2:@ ←2♦yq GB3●) 6♥?↑4")]
		[InlineData(GameRegion.US, "H~2:@ {left}2{diamond}yq GB3{circle}) 6{heart}?{up}4")]
		[InlineData(GameRegion.US, "H~2:@ left 2 diamond yq GB3 circle ) 6 heart ? up 4")]
		[InlineData(GameRegion.US, "H~2 :@LEFT2{dIAmoNd}yq G B3cirCle )6 heaRT}?    UP   4")]
		public void ParseString(GameRegion region, string secret)
		{
			byte[] bytes = SecretParser.ParseSecret(secret, region);
			Assert.Equivalent(desiredSecretBytes, bytes);
		}

		[Theory]
		[InlineData(GameRegion.JP, "かね6ごわ 4さをれか さ7ちわも るこぴりふ")]
		[InlineData(GameRegion.JP, "kane 6 go wa 4 sa wore kasa 7 tiwa moru ko piriHu ")]
		[InlineData(GameRegion.JP, "KaNe6GoWa 4SaWoReKa Sa7TiWaMo RuKoPiRiHu")]
		[InlineData(GameRegion.JP, "KaNe6GoWa 4SaWoReKa Sa7ChiWaMo RuKoPiRiFu")]
		public void ParseString_JP(GameRegion region, string secret)
		{
			byte[] bytes = SecretParser.ParseSecret(secret, region);
			Assert.Equivalent(desiredSecretBytes_JP, bytes);
		}

		[Fact]
		public void ParseInvalidString()
		{
			Assert.Throws<SecretException>(() => SecretParser.ParseSecret("INVALID", GameRegion.US));
		}

		[Fact]
		public void ParseInvalidBytes()
		{
			Assert.Throws<SecretException>(() => SecretParser.CreateString(new byte[] { 2, 15, 53, 21, 64 }, GameRegion.US));
		}
	}
}

