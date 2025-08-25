using System;

namespace UnityEngine.UI.Extensions
{
	// Token: 0x02000556 RID: 1366
	public static class RectTransformExtension
	{
		// Token: 0x060022C4 RID: 8900 RVA: 0x000C63C0 File Offset: 0x000C47C0
		public static Vector2 switchToRectTransform(this RectTransform from, RectTransform to)
		{
			Vector2 b = new Vector2(from.rect.width * from.pivot.x + from.rect.xMin, from.rect.height * from.pivot.y + from.rect.yMin);
			Vector2 vector = RectTransformUtility.WorldToScreenPoint(null, from.position);
			vector += b;
			Vector2 b2;
			RectTransformUtility.ScreenPointToLocalPointInRectangle(to, vector, null, out b2);
			Vector2 b3 = new Vector2(to.rect.width * to.pivot.x + to.rect.xMin, to.rect.height * to.pivot.y + to.rect.yMin);
			return to.anchoredPosition + b2 - b3;
		}
	}
}
