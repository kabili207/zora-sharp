using NUnit.Framework;
using System;

namespace Zyrenth.OracleHack.Tests
{
	[TestFixture]
	public class SecretParserTest
	{
		const string DesiredSecretString = "H~2:@ ←2♦yq GB3●( 6♥?↑6";
		static readonly byte[] DesiredSecretBytes = new byte[] {
			4, 37, 51, 36, 63,
			61, 51, 10, 44, 39,
			3,  0, 52, 21, 48,
			55,  9, 45, 59, 55
		};
		
		[Test]
		public void CreateString()
		{
			string testString = SecretParser.CreateString(DesiredSecretBytes);
			Assert.AreEqual(DesiredSecretString, testString);
		}
		
		[Test]
		public void ParseString()
		{
			string s1 = "H~2:@ ←2♦yq GB3●( 6♥?↑6";
			string s2 = "H~2:@ {left}2{diamond}yq GB3{circle}( 6{heart}?{up}6";
			string s3 = "H~2:@ left 2 diamond yq GB3 circle ( 6 heart ? up 6";
			string s4 = "H~2 :@LEFT2{dIAmoNd}yq G B3cirCle (6 heaRT}?    UP   6";
			
			Assert.AreEqual(DesiredSecretBytes, SecretParser.ParseSecret(s1));
			Assert.AreEqual(DesiredSecretBytes, SecretParser.ParseSecret(s2));
			Assert.AreEqual(DesiredSecretBytes, SecretParser.ParseSecret(s3));
			Assert.AreEqual(DesiredSecretBytes, SecretParser.ParseSecret(s4));
		}

		[Test]
		[ExpectedException(typeof(InvalidSecretException))]
		public void ParseInvalidString()
		{
			SecretParser.ParseSecret("INVALID");
		}
	}
}

