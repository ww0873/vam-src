using System;
using UnityEngine;
using UnityEngine.UI;

namespace VikingCrewTools.UI
{
	// Token: 0x0200056C RID: 1388
	[RequireComponent(typeof(ContentSizeFitter))]
	public class RatioLayoutFitter : HorizontalLayoutGroup
	{
		// Token: 0x06002330 RID: 9008 RVA: 0x000C8795 File Offset: 0x000C6B95
		public RatioLayoutFitter()
		{
		}

		// Token: 0x06002331 RID: 9009 RVA: 0x000C87B4 File Offset: 0x000C6BB4
		public override void CalculateLayoutInputHorizontal()
		{
			float preferredWidth = LayoutUtility.GetPreferredWidth(this.childToFit);
			float preferredHeight = LayoutUtility.GetPreferredHeight(this.childToFit);
			base.padding.left = (int)(preferredWidth * this.startPad.x / (1f - this.startPad.x - this.stopPad.x));
			base.padding.right = (int)(preferredWidth * this.stopPad.x / (1f - this.startPad.x - this.stopPad.x));
			base.padding.top = (int)(preferredHeight * this.startPad.y / (1f - this.startPad.y - this.stopPad.y));
			base.padding.bottom = (int)(preferredHeight * this.stopPad.y / (1f - this.startPad.y - this.stopPad.y));
			base.CalculateLayoutInputHorizontal();
		}

		// Token: 0x04001D26 RID: 7462
		[Header("Use these to set the ratio [0..1] of child size needed as padding on each side")]
		public Vector2 startPad = Vector2.zero;

		// Token: 0x04001D27 RID: 7463
		public Vector2 stopPad = Vector2.zero;

		// Token: 0x04001D28 RID: 7464
		public RectTransform childToFit;
	}
}
