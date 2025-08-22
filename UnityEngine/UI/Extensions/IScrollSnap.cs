using System;

namespace UnityEngine.UI.Extensions
{
	// Token: 0x02000511 RID: 1297
	internal interface IScrollSnap
	{
		// Token: 0x060020C3 RID: 8387
		void ChangePage(int page);

		// Token: 0x060020C4 RID: 8388
		void SetLerp(bool value);

		// Token: 0x060020C5 RID: 8389
		int CurrentPage();

		// Token: 0x060020C6 RID: 8390
		void StartScreenChange();
	}
}
