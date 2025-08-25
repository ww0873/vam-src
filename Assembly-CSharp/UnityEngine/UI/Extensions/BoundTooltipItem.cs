using System;

namespace UnityEngine.UI.Extensions
{
	// Token: 0x02000538 RID: 1336
	[AddComponentMenu("UI/Extensions/Bound Tooltip/Tooltip Item")]
	public class BoundTooltipItem : MonoBehaviour
	{
		// Token: 0x060021FD RID: 8701 RVA: 0x000C2805 File Offset: 0x000C0C05
		public BoundTooltipItem()
		{
		}

		// Token: 0x17000410 RID: 1040
		// (get) Token: 0x060021FE RID: 8702 RVA: 0x000C280D File Offset: 0x000C0C0D
		public bool IsActive
		{
			get
			{
				return base.gameObject.activeSelf;
			}
		}

		// Token: 0x060021FF RID: 8703 RVA: 0x000C281A File Offset: 0x000C0C1A
		private void Awake()
		{
			BoundTooltipItem.instance = this;
			if (!this.TooltipText)
			{
				this.TooltipText = base.GetComponentInChildren<Text>();
			}
			this.HideTooltip();
		}

		// Token: 0x06002200 RID: 8704 RVA: 0x000C2844 File Offset: 0x000C0C44
		public void ShowTooltip(string text, Vector3 pos)
		{
			if (this.TooltipText.text != text)
			{
				this.TooltipText.text = text;
			}
			base.transform.position = pos + this.ToolTipOffset;
			base.gameObject.SetActive(true);
		}

		// Token: 0x06002201 RID: 8705 RVA: 0x000C2896 File Offset: 0x000C0C96
		public void HideTooltip()
		{
			base.gameObject.SetActive(false);
		}

		// Token: 0x17000411 RID: 1041
		// (get) Token: 0x06002202 RID: 8706 RVA: 0x000C28A4 File Offset: 0x000C0CA4
		public static BoundTooltipItem Instance
		{
			get
			{
				if (BoundTooltipItem.instance == null)
				{
					BoundTooltipItem.instance = Object.FindObjectOfType<BoundTooltipItem>();
				}
				return BoundTooltipItem.instance;
			}
		}

		// Token: 0x04001C44 RID: 7236
		public Text TooltipText;

		// Token: 0x04001C45 RID: 7237
		public Vector3 ToolTipOffset;

		// Token: 0x04001C46 RID: 7238
		private static BoundTooltipItem instance;
	}
}
