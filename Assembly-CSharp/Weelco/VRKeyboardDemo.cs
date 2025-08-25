using System;
using UnityEngine;
using UnityEngine.UI;
using Weelco.VRKeyboard;

namespace Weelco
{
	// Token: 0x02000586 RID: 1414
	public class VRKeyboardDemo : MonoBehaviour
	{
		// Token: 0x06002396 RID: 9110 RVA: 0x000CEB69 File Offset: 0x000CCF69
		public VRKeyboardDemo()
		{
		}

		// Token: 0x06002397 RID: 9111 RVA: 0x000CEB7C File Offset: 0x000CCF7C
		private void Start()
		{
			if (this.keyboard)
			{
				VRKeyboardFull vrkeyboardFull = this.keyboard;
				vrkeyboardFull.OnVRKeyboardBtnClick = (VRKeyboardBase.VRKeyboardBtnClick)Delegate.Combine(vrkeyboardFull.OnVRKeyboardBtnClick, new VRKeyboardBase.VRKeyboardBtnClick(this.HandleClick));
				this.keyboard.Init();
			}
		}

		// Token: 0x06002398 RID: 9112 RVA: 0x000CEBCB File Offset: 0x000CCFCB
		private void OnDestroy()
		{
			if (this.keyboard)
			{
				VRKeyboardFull vrkeyboardFull = this.keyboard;
				vrkeyboardFull.OnVRKeyboardBtnClick = (VRKeyboardBase.VRKeyboardBtnClick)Delegate.Remove(vrkeyboardFull.OnVRKeyboardBtnClick, new VRKeyboardBase.VRKeyboardBtnClick(this.HandleClick));
			}
		}

		// Token: 0x06002399 RID: 9113 RVA: 0x000CEC04 File Offset: 0x000CD004
		private void HandleClick(string value)
		{
			if (value.Equals("BACK"))
			{
				this.BackspaceKey();
			}
			else if (value.Equals("ENTER"))
			{
				this.EnterKey();
			}
			else
			{
				this.TypeKey(value);
			}
		}

		// Token: 0x0600239A RID: 9114 RVA: 0x000CEC44 File Offset: 0x000CD044
		private void BackspaceKey()
		{
			if (this.inputFieldLabel.text.Length >= 1)
			{
				this.inputFieldLabel.text = this.inputFieldLabel.text.Remove(this.inputFieldLabel.text.Length - 1, 1);
			}
		}

		// Token: 0x0600239B RID: 9115 RVA: 0x000CEC95 File Offset: 0x000CD095
		private void EnterKey()
		{
		}

		// Token: 0x0600239C RID: 9116 RVA: 0x000CEC98 File Offset: 0x000CD098
		private void TypeKey(string value)
		{
			char[] array = value.ToCharArray();
			for (int i = 0; i < array.Length; i++)
			{
				this.TypeKey(array[i]);
			}
		}

		// Token: 0x0600239D RID: 9117 RVA: 0x000CECC9 File Offset: 0x000CD0C9
		private void TypeKey(char key)
		{
			if (this.inputFieldLabel.text.Length < this.maxOutputChars)
			{
				Text text = this.inputFieldLabel;
				text.text += key.ToString();
			}
		}

		// Token: 0x04001E15 RID: 7701
		public int maxOutputChars = 30;

		// Token: 0x04001E16 RID: 7702
		public Text inputFieldLabel;

		// Token: 0x04001E17 RID: 7703
		public VRKeyboardFull keyboard;
	}
}
