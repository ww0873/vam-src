using System;

namespace Weelco.VRKeyboard
{
	// Token: 0x0200059A RID: 1434
	public class VRKeyboardFull : VRKeyboardBase
	{
		// Token: 0x06002409 RID: 9225 RVA: 0x000D08D4 File Offset: 0x000CECD4
		public VRKeyboardFull()
		{
		}

		// Token: 0x0600240A RID: 9226 RVA: 0x000D08EC File Offset: 0x000CECEC
		private void OnDestroy()
		{
			if (this.Initialized)
			{
				foreach (VRKeyboardButton vrkeyboardButton in this.keys)
				{
					VRKeyboardButton vrkeyboardButton2 = vrkeyboardButton;
					vrkeyboardButton2.OnVRKeyboardBtnClick = (VRKeyboardBase.VRKeyboardBtnClick)Delegate.Remove(vrkeyboardButton2.OnVRKeyboardBtnClick, new VRKeyboardBase.VRKeyboardBtnClick(this.HandleClick));
				}
			}
		}

		// Token: 0x0600240B RID: 9227 RVA: 0x000D0948 File Offset: 0x000CED48
		public void Init()
		{
			if (!this.Initialized)
			{
				this.keys = base.transform.GetComponentsInChildren<VRKeyboardButton>();
				for (int i = 0; i < this.keys.Length; i++)
				{
					this.keys[i].Init();
					this.keys[i].SetKeyText(VRKeyboardData.allLettersLowercase[i], false);
					VRKeyboardButton vrkeyboardButton = this.keys[i];
					vrkeyboardButton.OnVRKeyboardBtnClick = (VRKeyboardBase.VRKeyboardBtnClick)Delegate.Combine(vrkeyboardButton.OnVRKeyboardBtnClick, new VRKeyboardBase.VRKeyboardBtnClick(this.HandleClick));
				}
				this.Initialized = true;
			}
		}

		// Token: 0x0600240C RID: 9228 RVA: 0x000D09DC File Offset: 0x000CEDDC
		private void HandleClick(string value)
		{
			if (value.Equals("sym") || value.Equals("abc"))
			{
				this.ChangeSpecialLetters();
			}
			else if (value.Equals("UP") || value.Equals("LOW"))
			{
				this.LowerUpperKeys();
			}
			else if (this.OnVRKeyboardBtnClick != null)
			{
				this.OnVRKeyboardBtnClick(value);
			}
		}

		// Token: 0x0600240D RID: 9229 RVA: 0x000D0A58 File Offset: 0x000CEE58
		private void ChangeSpecialLetters()
		{
			this.areLettersActive = !this.areLettersActive;
			string[] array = (!this.areLettersActive) ? VRKeyboardData.allSpecials : ((!this.isLowercase) ? VRKeyboardData.allLettersUppercase : VRKeyboardData.allLettersLowercase);
			for (int i = 0; i < this.keys.Length; i++)
			{
				bool ignoreIcon = !this.areLettersActive && array[i].Equals("№");
				this.keys[i].SetKeyText(array[i], ignoreIcon);
			}
		}

		// Token: 0x0600240E RID: 9230 RVA: 0x000D0AEC File Offset: 0x000CEEEC
		private void LowerUpperKeys()
		{
			this.isLowercase = !this.isLowercase;
			string[] array = (!this.isLowercase) ? VRKeyboardData.allLettersUppercase : VRKeyboardData.allLettersLowercase;
			for (int i = 0; i < this.keys.Length; i++)
			{
				this.keys[i].SetKeyText(array[i], false);
			}
		}

		// Token: 0x04001E5B RID: 7771
		private VRKeyboardButton[] keys;

		// Token: 0x04001E5C RID: 7772
		private bool areLettersActive = true;

		// Token: 0x04001E5D RID: 7773
		private bool isLowercase = true;
	}
}
