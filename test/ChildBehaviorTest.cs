using NUnit.Framework;

namespace Zyrenth.Zora.Tests
{
	[TestFixture]
	public class ChildBehaviorTest
	{

		[Test]
		public void GetInitial()
		{
			Assert.AreEqual(1, ChildBehaviorHelper.GetValue(GameRegion.US, "Pip"));
			Assert.AreEqual(2, ChildBehaviorHelper.GetValue(GameRegion.JP, "Pazu"));
			Assert.AreEqual(2, ChildBehaviorHelper.GetValue(GameRegion.JP, "うま"));
			Assert.AreEqual(3, ChildBehaviorHelper.GetValue(GameRegion.US, "Pipin"));
			Assert.AreEqual(3, ChildBehaviorHelper.GetValue(GameRegion.US, "Derp"));
		}

		[Test]
		public void GetValueForLinked()
		{
			Assert.AreEqual(13, ChildBehaviorHelper.GetValue(GameRegion.US, "Pip", RupeesGiven.Ten, SleepMethod.Play));
			Assert.AreEqual(3, ChildBehaviorHelper.GetValue(GameRegion.US, "Pip", RupeesGiven.Ten, SleepMethod.Sing));
			Assert.AreEqual(19, ChildBehaviorHelper.GetValue(GameRegion.US, "Pip", RupeesGiven.OneHundredFifty, SleepMethod.Play));
			Assert.AreEqual(6, ChildBehaviorHelper.GetValue(GameRegion.US, "Pip", RupeesGiven.Fifty, SleepMethod.Sing));
		}

		[Test]
		public void GetValueForHero()
		{
			Assert.AreEqual(13, ChildBehaviorHelper.GetValue(GameRegion.US, "Pip", RupeesGiven.One, SleepMethod.Sing,
                ChildQuestion.YesOrChicken, KindOfChild.Hyperactive));
			Assert.AreEqual(6, ChildBehaviorHelper.GetValue(GameRegion.US, "Pip", RupeesGiven.One, SleepMethod.Sing,
                ChildQuestion.NoOrEgg, KindOfChild.Quiet));
			Assert.AreEqual(5, ChildBehaviorHelper.GetValue(GameRegion.US, "Pip", RupeesGiven.One, SleepMethod.Sing,
                ChildQuestion.YesOrChicken, KindOfChild.None));
			Assert.AreEqual(6, ChildBehaviorHelper.GetValue(GameRegion.US, "Pip", RupeesGiven.One, SleepMethod.Sing,
                ChildQuestion.YesOrChicken, KindOfChild.Weird));
		}

		[Test]
		public void GetBehavior()
		{
			Assert.AreEqual(ChildBehavior.None, ChildBehaviorHelper.GetBehavior(0));
			Assert.AreEqual(ChildBehavior.Curious, ChildBehaviorHelper.GetBehavior(2));
			Assert.AreEqual(ChildBehavior.Shy, ChildBehaviorHelper.GetBehavior(7));
			Assert.AreEqual(ChildBehavior.Hyperactive, ChildBehaviorHelper.GetBehavior(16));
		}
	}
}
