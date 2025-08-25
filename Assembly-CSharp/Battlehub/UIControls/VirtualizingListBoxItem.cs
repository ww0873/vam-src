using System;
using UnityEngine.UI;

namespace Battlehub.UIControls
{
	// Token: 0x02000293 RID: 659
	public class VirtualizingListBoxItem : VirtualizingItemContainer
	{
		// Token: 0x06000F58 RID: 3928 RVA: 0x000582F5 File Offset: 0x000566F5
		public VirtualizingListBoxItem()
		{
		}

		// Token: 0x1700018D RID: 397
		// (get) Token: 0x06000F59 RID: 3929 RVA: 0x000582FD File Offset: 0x000566FD
		// (set) Token: 0x06000F5A RID: 3930 RVA: 0x00058305 File Offset: 0x00056705
		public override bool IsSelected
		{
			get
			{
				return base.IsSelected;
			}
			set
			{
				if (base.IsSelected != value)
				{
					this.m_toggle.isOn = value;
					base.IsSelected = value;
				}
			}
		}

		// Token: 0x06000F5B RID: 3931 RVA: 0x00058326 File Offset: 0x00056726
		protected override void AwakeOverride()
		{
			this.m_toggle = base.GetComponent<Toggle>();
			this.m_toggle.interactable = false;
			this.m_toggle.isOn = this.IsSelected;
		}

		// Token: 0x04000E2C RID: 3628
		private Toggle m_toggle;
	}
}
