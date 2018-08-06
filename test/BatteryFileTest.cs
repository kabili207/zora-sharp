using System;
using System.CodeDom.Compiler;
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
			var asm = Assembly.GetExecutingAssembly();
			Stream s = asm.GetManifestResourceStream("Zyrenth.Zora.Tests.TestSaves.Ages_US.srm");
			IEnumerable<GameInfo> infos = BatteryFileLoader.LoadAll(s, GameRegion.US);
			Assert.AreEqual(1, infos.Count());
			Assert.AreEqual(GameInfoTest.DesiredInfo, infos.First());
		}
		
		[Test]
		public void TestLoadAllFile()
		{
			string tempFile = Path.GetTempFileName();
			var asm = Assembly.GetExecutingAssembly();

			using(Stream s = asm.GetManifestResourceStream("Zyrenth.Zora.Tests.TestSaves.Seasons_US.srm"))
			using(FileStream fs = File.OpenWrite(tempFile))
			{
				s.CopyTo(fs);
			}
			
			IEnumerable<GameInfo> infos = BatteryFileLoader.LoadAll(tempFile, GameRegion.US);
			Assert.AreEqual(2, infos.Count());
			//Assert.AreEqual(GameInfoTest.DesiredInfo, infos.First());
		}
		
		[Test]
		public void TestLoadSlot3()
		{
			string tempFile = Path.GetTempFileName();
			var asm = Assembly.GetExecutingAssembly();
			
			using(Stream s = asm.GetManifestResourceStream("Zyrenth.Zora.Tests.TestSaves.Ages_JP.srm"))
			using(FileStream fs = File.OpenWrite(tempFile))
			{
				s.CopyTo(fs);
			}
			GameInfo info = BatteryFileLoader.Load(tempFile, GameRegion.JP, BatteryFileLoader.Slot3Offset);
			Assert.IsNotNull(info);
			
			var gs = new GameSecret();
			var test = new GameInfo();
			gs.Load("かね69わ 4さをれか さ7ちわも るこぴりお", GameRegion.JP);
			gs.UpdateGameInfo(test);
			//Assert.AreEqual(1, infos.Count());
			Assert.AreEqual(test, info);
		}
	}
}

