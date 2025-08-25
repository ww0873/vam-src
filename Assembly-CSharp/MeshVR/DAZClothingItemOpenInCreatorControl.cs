using System;
using UnityEngine;
using UnityEngine.UI;

namespace MeshVR
{
	// Token: 0x02000AC1 RID: 2753
	public class DAZClothingItemOpenInCreatorControl : JSONStorable
	{
		// Token: 0x06004923 RID: 18723 RVA: 0x0017368E File Offset: 0x00171A8E
		public DAZClothingItemOpenInCreatorControl()
		{
		}

		// Token: 0x06004924 RID: 18724 RVA: 0x00173698 File Offset: 0x00171A98
		protected void OpenInCreator()
		{
			if (this.dd != null)
			{
				DAZClothingItem componentInParent = base.GetComponentInParent<DAZClothingItem>();
				if (componentInParent != null && componentInParent.characterSelector != null && componentInParent.type == DAZDynamicItem.Type.Custom)
				{
					if (componentInParent.gender == DAZDynamicItem.Gender.Female)
					{
						componentInParent.characterSelector.SetActiveClothingItem(componentInParent, false, false);
						componentInParent.characterSelector.LoadFemaleClothingCreatorItem(this.dd);
					}
					else if (componentInParent.gender == DAZDynamicItem.Gender.Male)
					{
						componentInParent.characterSelector.SetActiveClothingItem(componentInParent, false, false);
						componentInParent.characterSelector.LoadMaleClothingCreatorItem(this.dd);
					}
				}
			}
		}

		// Token: 0x06004925 RID: 18725 RVA: 0x00173744 File Offset: 0x00171B44
		protected override void InitUI(Transform t, bool isAlt)
		{
			base.InitUI(t, isAlt);
			if (t != null)
			{
				DAZClothingItemOpenInCreatorControlUI componentInChildren = t.GetComponentInChildren<DAZClothingItemOpenInCreatorControlUI>(true);
				if (this.dd != null && componentInChildren != null)
				{
					this.openInCreatorAction.RegisterButton(componentInChildren.openButton, isAlt);
				}
				this.currentTagsText = componentInChildren.currentTagsText;
			}
		}

		// Token: 0x06004926 RID: 18726 RVA: 0x001737A8 File Offset: 0x00171BA8
		protected virtual void Init()
		{
			this.dd = base.GetComponent<DAZDynamic>();
			if (this.dd != null)
			{
				this.openInCreatorAction = new JSONStorableAction("OpenInCreator", new JSONStorableAction.ActionCallback(this.OpenInCreator));
				base.RegisterAction(this.openInCreatorAction);
			}
		}

		// Token: 0x06004927 RID: 18727 RVA: 0x001737FA File Offset: 0x00171BFA
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

		// Token: 0x06004928 RID: 18728 RVA: 0x0017381F File Offset: 0x00171C1F
		private void Update()
		{
			if (this.dd != null && this.currentTagsText != null)
			{
				this.currentTagsText.text = this.dd.tags;
			}
		}

		// Token: 0x040037A3 RID: 14243
		protected JSONStorableAction openInCreatorAction;

		// Token: 0x040037A4 RID: 14244
		protected DAZDynamic dd;

		// Token: 0x040037A5 RID: 14245
		protected Text currentTagsText;
	}
}
