using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Weelco.VRKeyboard
{
	// Token: 0x02000598 RID: 1432
	public class VRKeyboardButton : VRKeyboardBase
	{
		// Token: 0x06002402 RID: 9218 RVA: 0x000D079E File Offset: 0x000CEB9E
		public VRKeyboardButton()
		{
		}

		// Token: 0x06002403 RID: 9219 RVA: 0x000D07A6 File Offset: 0x000CEBA6
		private void OnDestroy()
		{
			if (this.Initialized)
			{
				this.button.onClick.RemoveListener(new UnityAction(this.HandleClick));
			}
		}

		// Token: 0x06002404 RID: 9220 RVA: 0x000D07D0 File Offset: 0x000CEBD0
		public void Init()
		{
			if (!this.Initialized)
			{
				Transform transform = base.transform.Find("Image");
				if (transform != null)
				{
					this.icon = transform.GetComponent<Image>();
				}
				this.label = base.transform.Find("Text").GetComponent<Text>();
				this.label.enabled = (this.icon == null);
				this.button = base.transform.GetComponent<Button>();
				this.button.onClick.AddListener(new UnityAction(this.HandleClick));
				this.Initialized = true;
			}
		}

		// Token: 0x06002405 RID: 9221 RVA: 0x000D0877 File Offset: 0x000CEC77
		public void SetKeyText(string value, bool ignoreIcon = false)
		{
			this.label.text = value;
			if (this.icon != null)
			{
				this.label.enabled = ignoreIcon;
				this.icon.enabled = !ignoreIcon;
			}
		}

		// Token: 0x06002406 RID: 9222 RVA: 0x000D08B1 File Offset: 0x000CECB1
		private void HandleClick()
		{
			if (this.OnVRKeyboardBtnClick != null)
			{
				this.OnVRKeyboardBtnClick(this.label.text);
			}
		}

		// Token: 0x04001E4F RID: 7759
		private Button button;

		// Token: 0x04001E50 RID: 7760
		private Text label;

		// Token: 0x04001E51 RID: 7761
		private Image icon;
	}
}
