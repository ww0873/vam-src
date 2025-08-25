using System;
using UnityEngine.Events;

namespace UnityEngine.UI.Extensions
{
	// Token: 0x02000548 RID: 1352
	[RequireComponent(typeof(InputField))]
	[AddComponentMenu("UI/Extensions/Input Field Submit")]
	public class InputFieldEnterSubmit : MonoBehaviour
	{
		// Token: 0x0600227E RID: 8830 RVA: 0x000C51CC File Offset: 0x000C35CC
		public InputFieldEnterSubmit()
		{
		}

		// Token: 0x0600227F RID: 8831 RVA: 0x000C51D4 File Offset: 0x000C35D4
		private void Awake()
		{
			this._input = base.GetComponent<InputField>();
			this._input.onEndEdit.AddListener(new UnityAction<string>(this.OnEndEdit));
		}

		// Token: 0x06002280 RID: 8832 RVA: 0x000C51FE File Offset: 0x000C35FE
		public void OnEndEdit(string txt)
		{
			if (!Input.GetKeyDown(KeyCode.Return) && !Input.GetKeyDown(KeyCode.KeypadEnter))
			{
				return;
			}
			this.EnterSubmit.Invoke(txt);
		}

		// Token: 0x04001C95 RID: 7317
		public InputFieldEnterSubmit.EnterSubmitEvent EnterSubmit;

		// Token: 0x04001C96 RID: 7318
		private InputField _input;

		// Token: 0x02000549 RID: 1353
		[Serializable]
		public class EnterSubmitEvent : UnityEvent<string>
		{
			// Token: 0x06002281 RID: 8833 RVA: 0x000C5228 File Offset: 0x000C3628
			public EnterSubmitEvent()
			{
			}
		}
	}
}
