using System;
using UnityEngine.UI;

namespace Battlehub.UIControls
{
	// Token: 0x0200027C RID: 636
	public class ListBoxItem : ItemContainer
	{
		// Token: 0x06000E2B RID: 3627 RVA: 0x0005307C File Offset: 0x0005147C
		public ListBoxItem()
		{
		}

		// Token: 0x17000165 RID: 357
		// (get) Token: 0x06000E2C RID: 3628 RVA: 0x00053084 File Offset: 0x00051484
		// (set) Token: 0x06000E2D RID: 3629 RVA: 0x0005308C File Offset: 0x0005148C
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

		// Token: 0x06000E2E RID: 3630 RVA: 0x000530AD File Offset: 0x000514AD
		protected override void AwakeOverride()
		{
			this.m_toggle = base.GetComponent<Toggle>();
			this.m_toggle.interactable = false;
			this.m_toggle.isOn = this.IsSelected;
		}

		// Token: 0x04000DA6 RID: 3494
		private Toggle m_toggle;
	}
}
