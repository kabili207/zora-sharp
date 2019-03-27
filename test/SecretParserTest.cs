using NUnit.Framework;

namespace Zyrenth.Zora.Tests
{
	[TestFixture]
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

		private static readonly byte[] highByte = new byte[] { 64 };
		private static readonly byte[] lowByte = new byte[] { 0 };

		[Test]
		public void CreateString()
		{
			string testString = SecretParser.CreateString(desiredSecretBytes, GameRegion.US);
			Assert.AreEqual(desiredSecretString, testString);
		}

		[Test]
		public void TestDivideThrows()
		{
			Assert.That(() => SecretParser.CreateString(new byte[] { 64 }, GameRegion.US), Throws.TypeOf<SecretException>());
		}

		[Test]
		public void CreateString_JP()
		{
			string testString = SecretParser.CreateString(desiredSecretBytes_JP, GameRegion.JP);
			Assert.AreEqual(desiredSecretString_JP, testString);
		}

		[Test]
		public void ParseString()
		{
			string s1 = "H~2:@ ←2♦yq GB3●) 6♥?↑4";
			string s2 = "H~2:@ {left}2{diamond}yq GB3{circle}) 6{heart}?{up}4";
			string s3 = "H~2:@ left 2 diamond yq GB3 circle ) 6 heart ? up 4";
			string s4 = "H~2 :@LEFT2{dIAmoNd}yq G B3cirCle )6 heaRT}?    UP   4";

			byte[][] allSecrets = new[] {
				SecretParser.ParseSecret(s1, GameRegion.US),
				SecretParser.ParseSecret(s2, GameRegion.US),
				SecretParser.ParseSecret(s3, GameRegion.US),
				SecretParser.ParseSecret(s4, GameRegion.US)
			};
			Assert.That(allSecrets, Is.All.EquivalentTo(desiredSecretBytes));
		}

		[Test]
		public void ParseString_JP()
		{
			string s1 = "かね6ごわ 4さをれか さ7ちわも るこぴりふ";
			string s2 = "kane 6 go wa 4 sa wore kasa 7 tiwa moru ko piriHu ";
			string s3 = "KaNe6GoWa 4SaWoReKa Sa7TiWaMo RuKoPiRiHu";
			string s4 = "KaNe6GoWa 4SaWoReKa Sa7ChiWaMo RuKoPiRiFu";

			byte[][] allSecrets = new[] {
				SecretParser.ParseSecret(s1, GameRegion.JP),
				SecretParser.ParseSecret(s2, GameRegion.JP),
				SecretParser.ParseSecret(s3, GameRegion.JP),
				SecretParser.ParseSecret(s4, GameRegion.JP)
			};
			Assert.That(allSecrets, Is.All.EquivalentTo(desiredSecretBytes_JP));
		}

		[Test]
		public void ParseInvalidString()
		{
			Assert.Throws<SecretException>(() => SecretParser.ParseSecret("INVALID", GameRegion.US));
		}

		[Test]
		public void ParseInvalidBytes()
		{
			Assert.Throws<SecretException>(() => SecretParser.CreateString(new byte[] { 2, 15, 53, 21, 64 }, GameRegion.US));
		}
	}
}

