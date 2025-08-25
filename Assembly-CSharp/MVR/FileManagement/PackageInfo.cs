using System;
using UnityEngine;
using UnityEngine.UI;

namespace MVR.FileManagement
{
	// Token: 0x02000BEB RID: 3051
	public class PackageInfo : JSONStorable
	{
		// Token: 0x060057B4 RID: 22452 RVA: 0x00203AE5 File Offset: 0x00201EE5
		public PackageInfo()
		{
		}

		// Token: 0x060057B5 RID: 22453 RVA: 0x00203AF0 File Offset: 0x00201EF0
		protected void SyncDescriptionContainer()
		{
			string text = null;
			if (this.descriptionJSON != null)
			{
				text = this.descriptionJSON.val;
			}
			if (this.descriptionContainer != null)
			{
				if (text != null && text != string.Empty)
				{
					this.descriptionContainer.gameObject.SetActive(true);
				}
				else
				{
					this.descriptionContainer.gameObject.SetActive(false);
				}
			}
		}

		// Token: 0x060057B6 RID: 22454 RVA: 0x00203B64 File Offset: 0x00201F64
		protected void SyncDescription(string s)
		{
			this.SyncDescriptionContainer();
		}

		// Token: 0x060057B7 RID: 22455 RVA: 0x00203B6C File Offset: 0x00201F6C
		protected void SyncInstructionsContainer()
		{
			string text = null;
			if (this.instructionsJSON != null)
			{
				text = this.instructionsJSON.val;
			}
			if (this.instructionsContainer != null)
			{
				if (text != null && text != string.Empty)
				{
					this.instructionsContainer.gameObject.SetActive(true);
				}
				else
				{
					this.instructionsContainer.gameObject.SetActive(false);
				}
			}
		}

		// Token: 0x060057B8 RID: 22456 RVA: 0x00203BE0 File Offset: 0x00201FE0
		protected void SyncInstructions(string s)
		{
			this.SyncInstructionsContainer();
		}

		// Token: 0x060057B9 RID: 22457 RVA: 0x00203BE8 File Offset: 0x00201FE8
		private void OpenPackageInManager()
		{
			string val = this.packageUidJSON.val;
			if (val != null && val != string.Empty)
			{
				SuperController.singleton.OpenPackageInManager(val);
			}
		}

		// Token: 0x060057BA RID: 22458 RVA: 0x00203C24 File Offset: 0x00202024
		protected void SyncOpenOnHubButton()
		{
			bool active = this._package != null && this._package.IsOnHub;
			if (this.openOnHubAction.button != null)
			{
				this.openOnHubAction.button.gameObject.SetActive(active);
			}
			if (this.openOnHubAction.buttonAlt != null)
			{
				this.openOnHubAction.buttonAlt.gameObject.SetActive(active);
			}
		}

		// Token: 0x060057BB RID: 22459 RVA: 0x00203CA3 File Offset: 0x002020A3
		private void OpenOnHub()
		{
			if (this._package != null)
			{
				this._package.OpenOnHub();
			}
		}

		// Token: 0x060057BC RID: 22460 RVA: 0x00203CBC File Offset: 0x002020BC
		protected void SyncPackagePanel()
		{
			if (this.packageInfoPanel != null)
			{
				if (this._package != null)
				{
					this.packageInfoPanel.gameObject.SetActive(true);
				}
				else
				{
					this.packageInfoPanel.gameObject.SetActive(false);
				}
			}
		}

		// Token: 0x060057BD RID: 22461 RVA: 0x00203D0C File Offset: 0x0020210C
		protected void SyncPromotionalButton()
		{
			if (this.promotionalButton != null)
			{
				if (!SuperController.singleton.promotionalDisabled && this._promotionalLink != null && this._promotionalLink != string.Empty)
				{
					this.promotionalButton.gameObject.SetActive(true);
				}
				else
				{
					this.promotionalButton.gameObject.SetActive(false);
				}
			}
			if (this.promotionalButtonText != null)
			{
				this.promotionalButtonText.text = this._promotionalLink;
			}
			if (this.copyPromotionalLinkAction != null && this.copyPromotionalLinkAction.button != null)
			{
				if (!SuperController.singleton.promotionalDisabled && this._promotionalLink != null && this._promotionalLink != string.Empty)
				{
					this.copyPromotionalLinkAction.button.gameObject.SetActive(true);
				}
				else
				{
					this.copyPromotionalLinkAction.button.gameObject.SetActive(false);
				}
			}
		}

		// Token: 0x060057BE RID: 22462 RVA: 0x00203E23 File Offset: 0x00202223
		protected void GoToPromotionalLink()
		{
			if (this.promotionalButtonText != null)
			{
				SuperController.singleton.OpenLinkInBrowser(this.promotionalButtonText.text);
			}
		}

		// Token: 0x060057BF RID: 22463 RVA: 0x00203E4B File Offset: 0x0020224B
		protected void CopyPromotionalLink()
		{
			if (this.promotionalButtonText != null)
			{
				GUIUtility.systemCopyBuffer = this.promotionalButtonText.text;
			}
		}

		// Token: 0x060057C0 RID: 22464 RVA: 0x00203E6E File Offset: 0x0020226E
		protected void SyncPromotionalLink(string s)
		{
			this._promotionalLink = s;
			this.SyncPromotionalButton();
		}

		// Token: 0x060057C1 RID: 22465 RVA: 0x00203E80 File Offset: 0x00202280
		public void SetPackage(VarPackage vp)
		{
			this._package = vp;
			if (vp != null)
			{
				this.packageUidJSON.val = vp.Uid;
				this.descriptionJSON.val = vp.Description;
				this.instructionsJSON.val = vp.Instructions;
				this.promotionalLinkJSON.val = vp.PromotionalLink;
			}
			else
			{
				this.packageUidJSON.val = string.Empty;
				this.descriptionJSON.val = string.Empty;
				this.instructionsJSON.val = string.Empty;
				this.promotionalLinkJSON.val = string.Empty;
			}
			this.SyncOpenOnHubButton();
			this.SyncPackagePanel();
		}

		// Token: 0x060057C2 RID: 22466 RVA: 0x00203F30 File Offset: 0x00202330
		protected void Init()
		{
			this.packageUidJSON = new JSONStorableString("packageUid", string.Empty);
			this.packageUidJSON.interactable = false;
			this.descriptionJSON = new JSONStorableString("description", string.Empty, new JSONStorableString.SetStringCallback(this.SyncDescription));
			this.descriptionJSON.interactable = false;
			this.instructionsJSON = new JSONStorableString("instructions", string.Empty, new JSONStorableString.SetStringCallback(this.SyncInstructions));
			this.instructionsJSON.interactable = false;
			this.promotionalLinkJSON = new JSONStorableString("promotionalLink", string.Empty, new JSONStorableString.SetStringCallback(this.SyncPromotionalLink));
			this.goToPromotionalLinkAction = new JSONStorableAction("GoToPromotionalLink", new JSONStorableAction.ActionCallback(this.GoToPromotionalLink));
			base.RegisterAction(this.goToPromotionalLinkAction);
			this.copyPromotionalLinkAction = new JSONStorableAction("CopyPromotionalLink", new JSONStorableAction.ActionCallback(this.CopyPromotionalLink));
			base.RegisterAction(this.copyPromotionalLinkAction);
			this.openPackageInManagerAction = new JSONStorableAction("OpenPackageInManager", new JSONStorableAction.ActionCallback(this.OpenPackageInManager));
			base.RegisterAction(this.openPackageInManagerAction);
			this.openOnHubAction = new JSONStorableAction("OpenOnHub", new JSONStorableAction.ActionCallback(this.OpenOnHub));
			base.RegisterAction(this.openOnHubAction);
		}

		// Token: 0x060057C3 RID: 22467 RVA: 0x0020407C File Offset: 0x0020247C
		protected override void InitUI(Transform t, bool isAlt)
		{
			if (t != null)
			{
				PackageInfoUI componentInChildren = t.GetComponentInChildren<PackageInfoUI>(true);
				if (componentInChildren != null)
				{
					this.copyPromotionalLinkAction.RegisterButton(componentInChildren.copyPromotionalLinkButton, isAlt);
					if (!isAlt)
					{
						this.packageInfoPanel = componentInChildren.packageInfoPanel;
						this.SyncPackagePanel();
						this.descriptionContainer = componentInChildren.descriptionContainer;
						this.SyncDescriptionContainer();
						this.instructionsContainer = componentInChildren.instructionsContainer;
						this.SyncInstructionsContainer();
						this.promotionalButton = componentInChildren.promotionalButton;
						this.promotionalButtonText = componentInChildren.promotionalButtonText;
						this.SyncPromotionalButton();
					}
					this.packageUidJSON.RegisterText(componentInChildren.packageUidText, isAlt);
					this.descriptionJSON.RegisterInputField(componentInChildren.descriptionField, isAlt);
					this.instructionsJSON.RegisterInputField(componentInChildren.instructionsField, isAlt);
					this.goToPromotionalLinkAction.RegisterButton(componentInChildren.promotionalButton, isAlt);
					this.openPackageInManagerAction.RegisterButton(componentInChildren.openPackageInManagerButton, isAlt);
					this.openOnHubAction.RegisterButton(componentInChildren.openOnHubButton, isAlt);
					this.SyncOpenOnHubButton();
				}
			}
		}

		// Token: 0x060057C4 RID: 22468 RVA: 0x00204187 File Offset: 0x00202587
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

		// Token: 0x04004876 RID: 18550
		protected JSONStorableString packageUidJSON;

		// Token: 0x04004877 RID: 18551
		protected RectTransform descriptionContainer;

		// Token: 0x04004878 RID: 18552
		protected JSONStorableString descriptionJSON;

		// Token: 0x04004879 RID: 18553
		protected RectTransform instructionsContainer;

		// Token: 0x0400487A RID: 18554
		protected JSONStorableString instructionsJSON;

		// Token: 0x0400487B RID: 18555
		protected JSONStorableAction openPackageInManagerAction;

		// Token: 0x0400487C RID: 18556
		protected JSONStorableAction openOnHubAction;

		// Token: 0x0400487D RID: 18557
		protected RectTransform packageInfoPanel;

		// Token: 0x0400487E RID: 18558
		protected Button promotionalButton;

		// Token: 0x0400487F RID: 18559
		protected Text promotionalButtonText;

		// Token: 0x04004880 RID: 18560
		protected JSONStorableAction goToPromotionalLinkAction;

		// Token: 0x04004881 RID: 18561
		protected JSONStorableAction copyPromotionalLinkAction;

		// Token: 0x04004882 RID: 18562
		protected string _promotionalLink;

		// Token: 0x04004883 RID: 18563
		protected JSONStorableString promotionalLinkJSON;

		// Token: 0x04004884 RID: 18564
		protected VarPackage _package;
	}
}
