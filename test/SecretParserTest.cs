using NUnit.Framework;
using System;

namespace Zyrenth.Zora.Tests
{
	[TestFixture]
	public class SecretParserTest
	{
		private const string desiredSecretString = "H~2:@ ←2♦yq GB3●) 6♥?↑4";
		private static readonly byte[] desiredSecretBytes = new byte[] {
			4, 37, 51, 36, 63,
			61, 51, 10, 44, 39,
			3,  0, 52, 21, 50,
			55,  9, 45, 59, 53
		};

		[Test]
		public void CreateString()
		{
			string testString = SecretParser.CreateString(desiredSecretBytes, GameRegion.US);
			Assert.AreEqual(desiredSecretString, testString);
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

