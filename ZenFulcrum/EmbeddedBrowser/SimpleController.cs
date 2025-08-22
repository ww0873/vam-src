using System;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace ZenFulcrum.EmbeddedBrowser
{
	// Token: 0x020005A8 RID: 1448
	[RequireComponent(typeof(Browser))]
	public class SimpleController : MonoBehaviour
	{
		// Token: 0x06002453 RID: 9299 RVA: 0x000D1D74 File Offset: 0x000D0174
		public SimpleController()
		{
		}

		// Token: 0x06002454 RID: 9300 RVA: 0x000D1D7C File Offset: 0x000D017C
		public void Start()
		{
			this.browser = base.GetComponent<Browser>();
			this.browser.onNavStateChange += this.<Start>m__0;
			this.urlInput.onEndEdit.AddListener(new UnityAction<string>(this.<Start>m__1));
		}

		// Token: 0x06002455 RID: 9301 RVA: 0x000D1DC8 File Offset: 0x000D01C8
		public void GoToURLInput()
		{
			this.browser.Url = this.urlInput.text;
		}

		// Token: 0x06002456 RID: 9302 RVA: 0x000D1DE0 File Offset: 0x000D01E0
		[CompilerGenerated]
		private void <Start>m__0()
		{
			if (!this.urlInput.isFocused)
			{
				this.urlInput.text = this.browser.Url;
			}
		}

		// Token: 0x06002457 RID: 9303 RVA: 0x000D1E08 File Offset: 0x000D0208
		[CompilerGenerated]
		private void <Start>m__1(string v)
		{
			if (Input.GetKey(KeyCode.Return) || Input.GetKey(KeyCode.KeypadEnter))
			{
				this.urlInput.DeactivateInputField();
				this.GoToURLInput();
			}
			else
			{
				this.urlInput.text = this.browser.Url;
			}
		}

		// Token: 0x04001E8C RID: 7820
		private Browser browser;

		// Token: 0x04001E8D RID: 7821
		public InputField urlInput;
	}
}
