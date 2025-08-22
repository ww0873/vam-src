using System;

namespace UnityEngine.UI.Extensions
{
	// Token: 0x020004EF RID: 1263
	[RequireComponent(typeof(RectTransform))]
	[RequireComponent(typeof(Text))]
	[AddComponentMenu("UI/Effects/Extensions/Curly UI Text")]
	public class CUIText : CUIGraphic
	{
		// Token: 0x06001FFA RID: 8186 RVA: 0x000B68E5 File Offset: 0x000B4CE5
		public CUIText()
		{
		}

		// Token: 0x06001FFB RID: 8187 RVA: 0x000B68ED File Offset: 0x000B4CED
		public override void ReportSet()
		{
			if (this.uiGraphic == null)
			{
				this.uiGraphic = base.GetComponent<Text>();
			}
			base.ReportSet();
		}
	}
}
