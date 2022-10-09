using Xunit;

namespace Zyrenth.Zora.Tests
{
	public class ChildBehaviorTest
	{

		
		[Theory]
		[InlineData(1, GameRegion.US, "Pip")]
		[InlineData(2, GameRegion.JP, "Pazu")]
		[InlineData(2, GameRegion.JP, "うま")]
		[InlineData(3, GameRegion.US, "Pipin")]
		[InlineData(3, GameRegion.US, "Derp")]
		public void GetInitial(int expected, GameRegion region, string name)
		{
			Assert.Equal(expected, ChildBehaviorHelper.GetValue(region, name));
		}

		
		[Theory]
		[InlineData(13, GameRegion.US, "Pip", RupeesGiven.Ten, SleepMethod.Play)]
		[InlineData(3, GameRegion.US, "Pip", RupeesGiven.Ten, SleepMethod.Sing)]
		[InlineData(19, GameRegion.US, "Pip", RupeesGiven.OneHundredFifty, SleepMethod.Play)]
		[InlineData(6, GameRegion.US, "Pip", RupeesGiven.Fifty, SleepMethod.Sing)]
		public void GetValueForLinked(int expected, GameRegion region, string name, RupeesGiven rupees, SleepMethod method)
		{
			Assert.Equal(expected, ChildBehaviorHelper.GetValue(region, name, rupees, method));
		}


		[Theory]
		[InlineData(13, GameRegion.US, "Pip", RupeesGiven.One, SleepMethod.Sing, ChildQuestion.YesOrChicken, KindOfChild.Hyperactive)]
		[InlineData(6, GameRegion.US, "Pip", RupeesGiven.One, SleepMethod.Sing, ChildQuestion.NoOrEgg, KindOfChild.Quiet)]
		[InlineData(5, GameRegion.US, "Pip", RupeesGiven.One, SleepMethod.Sing, ChildQuestion.YesOrChicken, KindOfChild.None)]
		[InlineData(6, GameRegion.US, "Pip", RupeesGiven.One, SleepMethod.Sing, ChildQuestion.YesOrChicken, KindOfChild.Weird)]
		public void GetValueForHero(int expected, GameRegion region, string name, RupeesGiven rupees, SleepMethod method, ChildQuestion question, KindOfChild kind)
		{
			Assert.Equal(expected, ChildBehaviorHelper.GetValue(region, name, rupees, method, question, kind));
		}

		
		[Theory]
		[InlineData(ChildBehavior.None, 0)]
		[InlineData(ChildBehavior.Curious, 2)]
		[InlineData(ChildBehavior.Shy, 7)]
		[InlineData(ChildBehavior.Hyperactive, 16)]
		public void GetBehavior(ChildBehavior behavior, byte value)
		{
			Assert.Equal(behavior, ChildBehaviorHelper.GetBehavior(value));
		}
	}
}
