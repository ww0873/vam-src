using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace MVR.FileManagement
{
	// Token: 0x02000BF1 RID: 3057
	public class UserConfirmPanel : MonoBehaviour
	{
		// Token: 0x060057D4 RID: 22484 RVA: 0x00204544 File Offset: 0x00202944
		public UserConfirmPanel()
		{
		}

		// Token: 0x060057D5 RID: 22485 RVA: 0x0020454C File Offset: 0x0020294C
		public void SetPrompt(string prompt)
		{
			if (this.promptText != null)
			{
				this.promptText.text = prompt;
			}
		}

		// Token: 0x060057D6 RID: 22486 RVA: 0x0020456B File Offset: 0x0020296B
		public void Confirm()
		{
			if (this.confirmCallback != null)
			{
				this.confirmCallback();
			}
		}

		// Token: 0x060057D7 RID: 22487 RVA: 0x00204584 File Offset: 0x00202984
		public void SetConfirmCallback(UserConfirmPanel.ResponseCallback callback)
		{
			this.confirmCallback = callback;
			if (this.confirmButton != null)
			{
				this.confirmButton.onClick.RemoveAllListeners();
				if (callback != null)
				{
					this.confirmButton.onClick.AddListener(new UnityAction(this.Confirm));
				}
			}
		}

		// Token: 0x060057D8 RID: 22488 RVA: 0x002045DB File Offset: 0x002029DB
		public void AutoConfirm()
		{
			if (this.autoConfirmCallback != null)
			{
				this.autoConfirmCallback();
			}
		}

		// Token: 0x060057D9 RID: 22489 RVA: 0x002045F3 File Offset: 0x002029F3
		public void SetAutoConfirmCallback(UserConfirmPanel.ResponseCallback callback)
		{
			this.autoConfirmCallback = callback;
		}

		// Token: 0x060057DA RID: 22490 RVA: 0x002045FC File Offset: 0x002029FC
		public void ConfirmSticky()
		{
			if (this.confirmStickyCallback != null)
			{
				this.confirmStickyCallback();
			}
		}

		// Token: 0x060057DB RID: 22491 RVA: 0x00204614 File Offset: 0x00202A14
		public void SetConfirmStickyCallback(UserConfirmPanel.ResponseCallback callback)
		{
			this.confirmStickyCallback = callback;
			if (this.confirmStickyButton != null)
			{
				this.confirmStickyButton.onClick.RemoveAllListeners();
				if (callback != null)
				{
					this.confirmStickyButton.onClick.AddListener(new UnityAction(this.ConfirmSticky));
				}
			}
		}

		// Token: 0x060057DC RID: 22492 RVA: 0x0020466B File Offset: 0x00202A6B
		public void Deny()
		{
			if (this.denyCallback != null)
			{
				this.denyCallback();
			}
		}

		// Token: 0x060057DD RID: 22493 RVA: 0x00204684 File Offset: 0x00202A84
		public void SetDenyCallback(UserConfirmPanel.ResponseCallback callback)
		{
			this.denyCallback = callback;
			if (this.denyButton != null)
			{
				this.denyButton.onClick.RemoveAllListeners();
				if (callback != null)
				{
					this.denyButton.onClick.AddListener(new UnityAction(this.Deny));
				}
			}
		}

		// Token: 0x060057DE RID: 22494 RVA: 0x002046DB File Offset: 0x00202ADB
		public void AutoDeny()
		{
			if (this.autoDenyCallback != null)
			{
				this.autoDenyCallback();
			}
		}

		// Token: 0x060057DF RID: 22495 RVA: 0x002046F3 File Offset: 0x00202AF3
		public void SetAutoDenyCallback(UserConfirmPanel.ResponseCallback callback)
		{
			this.autoDenyCallback = callback;
		}

		// Token: 0x060057E0 RID: 22496 RVA: 0x002046FC File Offset: 0x00202AFC
		public void DenySticky()
		{
			if (this.denyStickyCallback != null)
			{
				this.denyStickyCallback();
			}
		}

		// Token: 0x060057E1 RID: 22497 RVA: 0x00204714 File Offset: 0x00202B14
		public void SetDenyStickyCallback(UserConfirmPanel.ResponseCallback callback)
		{
			this.denyStickyCallback = callback;
			if (this.denyStickyButton != null)
			{
				this.denyStickyButton.onClick.RemoveAllListeners();
				if (callback != null)
				{
					this.denyStickyButton.onClick.AddListener(new UnityAction(this.DenySticky));
				}
			}
		}

		// Token: 0x04004891 RID: 18577
		public string signature;

		// Token: 0x04004892 RID: 18578
		public Text promptText;

		// Token: 0x04004893 RID: 18579
		public Button confirmButton;

		// Token: 0x04004894 RID: 18580
		public Button confirmStickyButton;

		// Token: 0x04004895 RID: 18581
		public Button denyButton;

		// Token: 0x04004896 RID: 18582
		public Button denyStickyButton;

		// Token: 0x04004897 RID: 18583
		protected UserConfirmPanel.ResponseCallback confirmCallback;

		// Token: 0x04004898 RID: 18584
		protected UserConfirmPanel.ResponseCallback autoConfirmCallback;

		// Token: 0x04004899 RID: 18585
		protected UserConfirmPanel.ResponseCallback confirmStickyCallback;

		// Token: 0x0400489A RID: 18586
		protected UserConfirmPanel.ResponseCallback denyCallback;

		// Token: 0x0400489B RID: 18587
		protected UserConfirmPanel.ResponseCallback autoDenyCallback;

		// Token: 0x0400489C RID: 18588
		protected UserConfirmPanel.ResponseCallback denyStickyCallback;

		// Token: 0x02000BF2 RID: 3058
		// (Invoke) Token: 0x060057E3 RID: 22499
		public delegate void ResponseCallback();
	}
}
