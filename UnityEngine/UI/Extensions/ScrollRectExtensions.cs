using System;

namespace UnityEngine.UI.Extensions
{
	// Token: 0x02000551 RID: 1361
	public static class ScrollRectExtensions
	{
		// Token: 0x060022A3 RID: 8867 RVA: 0x000C5A62 File Offset: 0x000C3E62
		public static void ScrollToTop(this ScrollRect scrollRect)
		{
			scrollRect.normalizedPosition = new Vector2(0f, 1f);
		}

		// Token: 0x060022A4 RID: 8868 RVA: 0x000C5A79 File Offset: 0x000C3E79
		public static void ScrollToBottom(this ScrollRect scrollRect)
		{
			scrollRect.normalizedPosition = new Vector2(0f, 0f);
		}
	}
}
