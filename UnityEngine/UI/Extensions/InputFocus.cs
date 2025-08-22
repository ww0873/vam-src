using System;

namespace UnityEngine.UI.Extensions
{
	// Token: 0x020004CB RID: 1227
	[RequireComponent(typeof(InputField))]
	[AddComponentMenu("UI/Extensions/InputFocus")]
	public class InputFocus : MonoBehaviour
	{
		// Token: 0x06001EFB RID: 7931 RVA: 0x000B05DE File Offset: 0x000AE9DE
		public InputFocus()
		{
		}

		// Token: 0x06001EFC RID: 7932 RVA: 0x000B05E6 File Offset: 0x000AE9E6
		private void Start()
		{
			this._inputField = base.GetComponent<InputField>();
		}

		// Token: 0x06001EFD RID: 7933 RVA: 0x000B05F4 File Offset: 0x000AE9F4
		private void Update()
		{
			if (Input.GetKeyUp(KeyCode.Return) && !this._inputField.isFocused)
			{
				if (this._ignoreNextActivation)
				{
					this._ignoreNextActivation = false;
				}
				else
				{
					this._inputField.Select();
					this._inputField.ActivateInputField();
				}
			}
		}

		// Token: 0x06001EFE RID: 7934 RVA: 0x000B064C File Offset: 0x000AEA4C
		public void buttonPressed()
		{
			bool flag = this._inputField.text == string.Empty;
			this._inputField.text = string.Empty;
			if (!flag)
			{
				this._inputField.Select();
				this._inputField.ActivateInputField();
			}
		}

		// Token: 0x06001EFF RID: 7935 RVA: 0x000B069C File Offset: 0x000AEA9C
		public void OnEndEdit(string textString)
		{
			if (!Input.GetKeyDown(KeyCode.Return))
			{
				return;
			}
			bool flag = this._inputField.text == string.Empty;
			this._inputField.text = string.Empty;
			if (flag)
			{
				this._ignoreNextActivation = true;
			}
		}

		// Token: 0x04001A2C RID: 6700
		protected InputField _inputField;

		// Token: 0x04001A2D RID: 6701
		public bool _ignoreNextActivation;
	}
}
