using System;

namespace UnityEngine.UI.Extensions.Tweens
{
	// Token: 0x020004A9 RID: 1193
	internal interface ITweenValue
	{
		// Token: 0x06001E15 RID: 7701
		void TweenValue(float floatPercentage);

		// Token: 0x1700034D RID: 845
		// (get) Token: 0x06001E16 RID: 7702
		bool ignoreTimeScale { get; }

		// Token: 0x1700034E RID: 846
		// (get) Token: 0x06001E17 RID: 7703
		float duration { get; }

		// Token: 0x06001E18 RID: 7704
		bool ValidTarget();

		// Token: 0x06001E19 RID: 7705
		void Finished();
	}
}
