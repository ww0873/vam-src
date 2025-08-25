using System;
using UnityEngine;
using UnityEngine.UI;

namespace MeshVR
{
	// Token: 0x02000ACE RID: 2766
	public class DAZDynamicItemOpenInCreatorControl : JSONStorable
	{
		// Token: 0x06004981 RID: 18817 RVA: 0x0017A0FF File Offset: 0x001784FF
		public DAZDynamicItemOpenInCreatorControl()
		{
		}

		// Token: 0x06004982 RID: 18818 RVA: 0x0017A108 File Offset: 0x00178508
		protected void OpenInCreator()
		{
			if (this.dd != null)
			{
				DAZDynamicItem componentInParent = base.GetComponentInParent<DAZDynamicItem>();
				if (componentInParent != null && componentInParent.characterSelector != null)
				{
					componentInParent.characterSelector.LoadDynamicCreatorItem(componentInParent, this.dd);
				}
			}
		}

		// Token: 0x06004983 RID: 18819 RVA: 0x0017A15C File Offset: 0x0017855C
		protected override void InitUI(Transform t, bool isAlt)
		{
			base.InitUI(t, isAlt);
			if (t != null)
			{
				DAZDynamicItemOpenInCreatorControlUI componentInChildren = t.GetComponentInChildren<DAZDynamicItemOpenInCreatorControlUI>(true);
				if (this.dd != null && componentInChildren != null)
				{
					if (componentInChildren.openButton != null)
					{
						componentInChildren.openButton.interactable = true;
					}
					this.openInCreatorAction.RegisterButton(componentInChildren.openButton, isAlt);
				}
				this.currentTagsText = componentInChildren.currentTagsText;
			}
		}

		// Token: 0x06004984 RID: 18820 RVA: 0x0017A1E0 File Offset: 0x001785E0
		protected virtual void Init()
		{
			this.dd = base.GetComponent<DAZDynamic>();
			if (this.dd != null)
			{
				this.openInCreatorAction = new JSONStorableAction("OpenInCreator", new JSONStorableAction.ActionCallback(this.OpenInCreator));
				base.RegisterAction(this.openInCreatorAction);
			}
		}

		// Token: 0x06004985 RID: 18821 RVA: 0x0017A232 File Offset: 0x00178632
		protected override void Awake()
		{
			if (!this.awakecalled)
			{
				base.Awake();
				this.Init();
				this.InitUI();
				this.InitUIAlt();
			}
		}

		// Token: 0x06004986 RID: 18822 RVA: 0x0017A257 File Offset: 0x00178657
		private void Update()
		{
			if (this.dd != null && this.currentTagsText != null)
			{
				this.currentTagsText.text = this.dd.tags;
			}
		}

		// Token: 0x04003804 RID: 14340
		protected JSONStorableAction openInCreatorAction;

		// Token: 0x04003805 RID: 14341
		protected DAZDynamic dd;

		// Token: 0x04003806 RID: 14342
		protected Text currentTagsText;
	}
}
