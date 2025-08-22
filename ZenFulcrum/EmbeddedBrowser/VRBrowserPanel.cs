using System;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace ZenFulcrum.EmbeddedBrowser
{
	// Token: 0x020005AC RID: 1452
	public class VRBrowserPanel : MonoBehaviour, INewWindowHandler
	{
		// Token: 0x0600246A RID: 9322 RVA: 0x000D29F8 File Offset: 0x000D0DF8
		public VRBrowserPanel()
		{
		}

		// Token: 0x0600246B RID: 9323 RVA: 0x000D2A00 File Offset: 0x000D0E00
		public void Awake()
		{
			DestroyDetector destroyDetector = this.contentBrowser.gameObject.AddComponent<DestroyDetector>();
			destroyDetector.onDestroy += this.CloseBrowser;
			this.contentBrowser.SetNewWindowHandler(Browser.NewWindowAction.NewBrowser, this);
			this.contentBrowser.onLoad += this.<Awake>m__0;
			this.controlBrowser.RegisterFunction("demoNavForward", new Browser.JSCallback(this.<Awake>m__1));
			this.controlBrowser.RegisterFunction("demoNavBack", new Browser.JSCallback(this.<Awake>m__2));
			this.controlBrowser.RegisterFunction("demoNavRefresh", new Browser.JSCallback(this.<Awake>m__3));
			this.controlBrowser.RegisterFunction("demoNavClose", new Browser.JSCallback(this.<Awake>m__4));
			this.controlBrowser.RegisterFunction("goTo", new Browser.JSCallback(this.<Awake>m__5));
			VRMainControlPanel.instance.keyboard.onFocusChange += this.OnKeyboardOnOnFocusChange;
		}

		// Token: 0x0600246C RID: 9324 RVA: 0x000D2AFB File Offset: 0x000D0EFB
		public void OnDestroy()
		{
			VRMainControlPanel.instance.keyboard.onFocusChange -= this.OnKeyboardOnOnFocusChange;
		}

		// Token: 0x0600246D RID: 9325 RVA: 0x000D2B18 File Offset: 0x000D0F18
		private void OnKeyboardOnOnFocusChange(Browser browser, bool editable)
		{
			if (!editable || !browser)
			{
				VRMainControlPanel.instance.MoveKeyboardUnder(null);
			}
			else if (browser == this.contentBrowser || browser == this.controlBrowser)
			{
				VRMainControlPanel.instance.MoveKeyboardUnder(this);
			}
		}

		// Token: 0x0600246E RID: 9326 RVA: 0x000D2B73 File Offset: 0x000D0F73
		public void CloseBrowser()
		{
			if (!this || !VRMainControlPanel.instance)
			{
				return;
			}
			VRMainControlPanel.instance.DestroyPane(this);
		}

		// Token: 0x0600246F RID: 9327 RVA: 0x000D2B9C File Offset: 0x000D0F9C
		public Browser CreateBrowser(Browser parent)
		{
			VRBrowserPanel vrbrowserPanel = VRMainControlPanel.instance.OpenNewTab(this);
			vrbrowserPanel.transform.position = base.transform.position;
			vrbrowserPanel.transform.rotation = base.transform.rotation;
			return vrbrowserPanel.contentBrowser;
		}

		// Token: 0x06002470 RID: 9328 RVA: 0x000D2BE7 File Offset: 0x000D0FE7
		[CompilerGenerated]
		private void <Awake>m__0(JSONNode data)
		{
			this.controlBrowser.CallFunction("setURL", new JSONNode[]
			{
				data["url"]
			});
		}

		// Token: 0x06002471 RID: 9329 RVA: 0x000D2C0E File Offset: 0x000D100E
		[CompilerGenerated]
		private void <Awake>m__1(JSONNode args)
		{
			this.contentBrowser.GoForward();
		}

		// Token: 0x06002472 RID: 9330 RVA: 0x000D2C1B File Offset: 0x000D101B
		[CompilerGenerated]
		private void <Awake>m__2(JSONNode args)
		{
			this.contentBrowser.GoBack();
		}

		// Token: 0x06002473 RID: 9331 RVA: 0x000D2C28 File Offset: 0x000D1028
		[CompilerGenerated]
		private void <Awake>m__3(JSONNode args)
		{
			this.contentBrowser.Reload(false);
		}

		// Token: 0x06002474 RID: 9332 RVA: 0x000D2C36 File Offset: 0x000D1036
		[CompilerGenerated]
		private void <Awake>m__4(JSONNode args)
		{
			this.CloseBrowser();
		}

		// Token: 0x06002475 RID: 9333 RVA: 0x000D2C3E File Offset: 0x000D103E
		[CompilerGenerated]
		private void <Awake>m__5(JSONNode args)
		{
			this.contentBrowser.LoadURL(args[0], false);
		}

		// Token: 0x04001EA0 RID: 7840
		public Browser contentBrowser;

		// Token: 0x04001EA1 RID: 7841
		public Browser controlBrowser;

		// Token: 0x04001EA2 RID: 7842
		public Transform keyboardLocation;
	}
}
