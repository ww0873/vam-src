using System;

namespace Leap.Unity
{
	// Token: 0x0200073F RID: 1855
	public interface IProgressView
	{
		// Token: 0x06002D4E RID: 11598
		void Clear();

		// Token: 0x06002D4F RID: 11599
		void DisplayProgress(string title, string info, float progress);
	}
}
