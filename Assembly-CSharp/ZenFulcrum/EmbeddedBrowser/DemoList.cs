using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace ZenFulcrum.EmbeddedBrowser
{
	// Token: 0x0200059F RID: 1439
	[RequireComponent(typeof(Browser))]
	public class DemoList : MonoBehaviour
	{
		// Token: 0x0600241A RID: 9242 RVA: 0x000D0DF4 File Offset: 0x000CF1F4
		public DemoList()
		{
		}

		// Token: 0x0600241B RID: 9243 RVA: 0x000D0E90 File Offset: 0x000CF290
		protected void Start()
		{
			this.panelBrowser = base.GetComponent<Browser>();
			this.panelBrowser.RegisterFunction("go", new Browser.JSCallback(this.<Start>m__0));
			this.demoBrowser.onLoad += this.<Start>m__1;
			this.demoBrowser.Url = this.demoSites[0];
		}

		// Token: 0x0600241C RID: 9244 RVA: 0x000D0EF4 File Offset: 0x000CF2F4
		private void DemoNav(int dir)
		{
			if (dir > 0)
			{
				this.currentIndex = (this.currentIndex + 1) % this.demoSites.Count;
			}
			else
			{
				this.currentIndex = (this.currentIndex - 1 + this.demoSites.Count) % this.demoSites.Count;
			}
			this.demoBrowser.Url = this.demoSites[this.currentIndex];
		}

		// Token: 0x0600241D RID: 9245 RVA: 0x000D0F69 File Offset: 0x000CF369
		[CompilerGenerated]
		private void <Start>m__0(JSONNode args)
		{
			this.DemoNav(args[0].Check());
		}

		// Token: 0x0600241E RID: 9246 RVA: 0x000D0F82 File Offset: 0x000CF382
		[CompilerGenerated]
		private void <Start>m__1(JSONNode info)
		{
			this.panelBrowser.CallFunction("setDisplayedUrl", new JSONNode[]
			{
				this.demoBrowser.Url
			});
		}

		// Token: 0x04001E67 RID: 7783
		protected List<string> demoSites = new List<string>
		{
			"localGame://demo/MouseShow.html",
			"http://js1k.com/2013-spring/demo/1487",
			"http://js1k.com/2014-dragons/demo/1868",
			"http://js1k.com/2015-hypetrain/demo/2231",
			"http://js1k.com/2015-hypetrain/demo/2313",
			"http://js1k.com/2015-hypetrain/demo/2331",
			"http://js1k.com/2015-hypetrain/demo/2315",
			"http://js1k.com/2015-hypetrain/demo/2161",
			"http://js1k.com/2013-spring/demo/1533",
			"http://js1k.com/2014-dragons/demo/1969",
			"http://www.snappymaria.com/misc/TouchEventTest.html"
		};

		// Token: 0x04001E68 RID: 7784
		public Browser demoBrowser;

		// Token: 0x04001E69 RID: 7785
		private Browser panelBrowser;

		// Token: 0x04001E6A RID: 7786
		private int currentIndex;
	}
}
