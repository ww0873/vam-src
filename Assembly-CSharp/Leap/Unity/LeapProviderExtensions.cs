using System;

namespace Leap.Unity
{
	// Token: 0x020006FB RID: 1787
	public static class LeapProviderExtensions
	{
		// Token: 0x06002B51 RID: 11089 RVA: 0x000E97F9 File Offset: 0x000E7BF9
		public static Hand MakeTestHand(this LeapProvider provider, bool isLeft)
		{
			return TestHandFactory.MakeTestHand(isLeft, Hands.Provider.editTimePose, 0, 0, TestHandFactory.UnitType.LeapUnits).Transform(Hands.Provider.transform.GetLeapMatrix());
		}
	}
}
