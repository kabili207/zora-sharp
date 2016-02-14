using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using NUnit.Framework;

namespace Zyrenth.Zora.Tests
{
	[TestFixture]
	public class BatteryFileTest
	{
		[Test]
		public void TestLoadAll()
		{
			Assembly asm = Assembly.GetExecutingAssembly();
			Stream s = asm.GetManifestResourceStream("Zyrenth.Zora.Tests.AgesSave.sav");
			IEnumerable<GameInfo> infos = BatteryFileLoader.LoadAll(s);
			Assert.AreEqual(1, infos.Count());
			Assert.AreEqual(GameInfoTest.DesiredInfo, infos.First());
		}
	}
}

