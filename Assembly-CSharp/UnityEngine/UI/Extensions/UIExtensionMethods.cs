using System;

namespace UnityEngine.UI.Extensions
{
	// Token: 0x0200055B RID: 1371
	public static class UIExtensionMethods
	{
		// Token: 0x060022D7 RID: 8919 RVA: 0x000C7320 File Offset: 0x000C5720
		public static Canvas GetParentCanvas(this RectTransform rt)
		{
			RectTransform rectTransform = rt;
			Canvas canvas = rt.GetComponent<Canvas>();
			int num = 0;
			while (canvas == null || num > 50)
			{
				canvas = rt.GetComponentInParent<Canvas>();
				if (canvas == null)
				{
					rectTransform = rectTransform.parent.GetComponent<RectTransform>();
					num++;
				}
			}
			return canvas;
		}
	}
}
