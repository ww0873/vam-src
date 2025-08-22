using System;
using UnityEngine;

namespace MeshVR
{
	// Token: 0x02000AC5 RID: 2757
	public class DAZDynamicDeleter : JSONStorable
	{
		// Token: 0x06004943 RID: 18755 RVA: 0x00179CB9 File Offset: 0x001780B9
		public DAZDynamicDeleter()
		{
		}

		// Token: 0x06004944 RID: 18756 RVA: 0x00179CC4 File Offset: 0x001780C4
		protected void DeleteConfirm()
		{
			if (this.cancelAction != null && this.cancelAction.button != null)
			{
				this.cancelAction.button.gameObject.SetActive(false);
			}
			if (this.deleteConfirmAction != null && this.deleteConfirmAction.button != null)
			{
				this.deleteConfirmAction.button.gameObject.SetActive(false);
			}
			if (this.dd != null)
			{
				this.dd.Delete();
				DAZClothingItemControl component = base.GetComponent<DAZClothingItemControl>();
				if (component != null)
				{
					component.Delete();
					component.RefreshClothingItems();
				}
				else
				{
					DAZHairGroupControl component2 = base.GetComponent<DAZHairGroupControl>();
					if (component2 != null)
					{
						component2.Delete();
						component2.RefreshHairItems();
					}
				}
				if (SuperController.singleton != null)
				{
					SuperController.singleton.SetActiveUI("SelectedOptions");
				}
			}
		}

		// Token: 0x06004945 RID: 18757 RVA: 0x00179DC0 File Offset: 0x001781C0
		protected void Cancel()
		{
			if (this.cancelAction != null && this.cancelAction.button != null)
			{
				this.cancelAction.button.gameObject.SetActive(false);
			}
			if (this.deleteConfirmAction != null && this.deleteConfirmAction.button != null)
			{
				this.deleteConfirmAction.button.gameObject.SetActive(false);
			}
		}

		// Token: 0x06004946 RID: 18758 RVA: 0x00179E3C File Offset: 0x0017823C
		protected void Delete()
		{
			if (this.cancelAction != null && this.cancelAction.button != null)
			{
				this.cancelAction.button.gameObject.SetActive(true);
			}
			if (this.deleteConfirmAction != null && this.deleteConfirmAction.button != null)
			{
				this.deleteConfirmAction.button.gameObject.SetActive(true);
			}
		}

		// Token: 0x06004947 RID: 18759 RVA: 0x00179EB8 File Offset: 0x001782B8
		protected override void InitUI(Transform t, bool isAlt)
		{
			base.InitUI(t, isAlt);
			if (t != null)
			{
				DAZDynamicDeleterUI componentInChildren = t.GetComponentInChildren<DAZDynamicDeleterUI>();
				if (this.dd != null && componentInChildren != null)
				{
					if (this.dd.IsInPackage())
					{
						if (componentInChildren.deleteButton != null)
						{
							componentInChildren.deleteButton.interactable = false;
						}
					}
					else
					{
						if (componentInChildren.deleteButton != null)
						{
							componentInChildren.deleteButton.interactable = true;
						}
						this.deleteAction.RegisterButton(componentInChildren.deleteButton, isAlt);
						this.cancelAction.RegisterButton(componentInChildren.cancelButton, isAlt);
						this.deleteConfirmAction.RegisterButton(componentInChildren.confirmButton, isAlt);
					}
				}
			}
		}

		// Token: 0x06004948 RID: 18760 RVA: 0x00179F84 File Offset: 0x00178384
		protected virtual void Init()
		{
			this.dd = base.GetComponent<DAZDynamic>();
			if (this.dd != null && !this.dd.IsInPackage())
			{
				this.deleteAction = new JSONStorableAction("Delete", new JSONStorableAction.ActionCallback(this.Delete));
				base.RegisterAction(this.deleteAction);
				this.cancelAction = new JSONStorableAction("Cancel", new JSONStorableAction.ActionCallback(this.Cancel));
				base.RegisterAction(this.cancelAction);
				this.deleteConfirmAction = new JSONStorableAction("DeleteConfirm", new JSONStorableAction.ActionCallback(this.DeleteConfirm));
				base.RegisterAction(this.deleteConfirmAction);
			}
		}

		// Token: 0x06004949 RID: 18761 RVA: 0x0017A036 File Offset: 0x00178436
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

		// Token: 0x040037B3 RID: 14259
		protected JSONStorableAction deleteConfirmAction;

		// Token: 0x040037B4 RID: 14260
		protected JSONStorableAction cancelAction;

		// Token: 0x040037B5 RID: 14261
		protected JSONStorableAction deleteAction;

		// Token: 0x040037B6 RID: 14262
		protected DAZDynamic dd;
	}
}
