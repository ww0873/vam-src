using System;
using UnityEngine;

namespace Leap.Unity
{
	// Token: 0x02000730 RID: 1840
	public static class GuiRectUtil
	{
		// Token: 0x06002CB9 RID: 11449 RVA: 0x000EFDF7 File Offset: 0x000EE1F7
		public static Vector3 Corner00(this Rect rect)
		{
			return new Vector3(rect.x, rect.y);
		}

		// Token: 0x06002CBA RID: 11450 RVA: 0x000EFE0C File Offset: 0x000EE20C
		public static Vector3 Corner10(this Rect rect)
		{
			return new Vector3(rect.x + rect.width, rect.y);
		}

		// Token: 0x06002CBB RID: 11451 RVA: 0x000EFE29 File Offset: 0x000EE229
		public static Vector3 Corner01(this Rect rect)
		{
			return new Vector3(rect.x, rect.y + rect.height);
		}

		// Token: 0x06002CBC RID: 11452 RVA: 0x000EFE46 File Offset: 0x000EE246
		public static Vector3 Corner11(this Rect rect)
		{
			return new Vector3(rect.x + rect.width, rect.y + rect.height);
		}

		// Token: 0x06002CBD RID: 11453 RVA: 0x000EFE6C File Offset: 0x000EE26C
		public static Rect Encapsulate(this Rect rect, Vector2 point)
		{
			if (point.x < rect.x)
			{
				rect.width += rect.x - point.x;
				rect.x = point.x;
			}
			else if (point.x > rect.x + rect.width)
			{
				rect.width = point.x - rect.x;
			}
			if (point.y < rect.y)
			{
				rect.height += rect.y - point.y;
				rect.y = point.y;
			}
			else if (point.y > rect.y + rect.height)
			{
				rect.height = point.y - rect.y;
			}
			return rect;
		}

		// Token: 0x06002CBE RID: 11454 RVA: 0x000EFF62 File Offset: 0x000EE362
		public static void SplitHorizontally(this Rect rect, out Rect left, out Rect right)
		{
			left = rect;
			left.width /= 2f;
			right = left;
			right.x += right.width;
		}

		// Token: 0x06002CBF RID: 11455 RVA: 0x000EFF9C File Offset: 0x000EE39C
		public static void SplitHorizontallyWithRight(this Rect rect, out Rect left, out Rect right, float rightWidth)
		{
			left = rect;
			left.width -= rightWidth;
			right = left;
			right.x += right.width;
			right.width = rightWidth;
		}

		// Token: 0x06002CC0 RID: 11456 RVA: 0x000EFFD9 File Offset: 0x000EE3D9
		public static Rect NextLine(this Rect rect)
		{
			rect.y += rect.height;
			return rect;
		}

		// Token: 0x06002CC1 RID: 11457 RVA: 0x000EFFF1 File Offset: 0x000EE3F1
		public static Rect FromRight(this Rect rect, float width)
		{
			rect.x = rect.width - width;
			rect.width = width;
			return rect;
		}
	}
}
