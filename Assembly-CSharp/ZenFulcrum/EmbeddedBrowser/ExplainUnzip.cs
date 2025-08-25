using System;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace ZenFulcrum.EmbeddedBrowser
{
	// Token: 0x020005A2 RID: 1442
	[RequireComponent(typeof(Browser))]
	public class ExplainUnzip : MonoBehaviour
	{
		// Token: 0x0600242A RID: 9258 RVA: 0x000D135C File Offset: 0x000CF75C
		public ExplainUnzip()
		{
		}

		// Token: 0x0600242B RID: 9259 RVA: 0x000D1364 File Offset: 0x000CF764
		public void Start()
		{
			ExplainUnzip.<Start>c__AnonStorey0 <Start>c__AnonStorey = new ExplainUnzip.<Start>c__AnonStorey0();
			<Start>c__AnonStorey.browser = base.GetComponent<Browser>();
			<Start>c__AnonStorey.browser.onLoad += <Start>c__AnonStorey.<>m__0;
			<Start>c__AnonStorey.browser.onFetchError += <Start>c__AnonStorey.<>m__1;
		}

		// Token: 0x02000F83 RID: 3971
		[CompilerGenerated]
		private sealed class <Start>c__AnonStorey0
		{
			// Token: 0x06007422 RID: 29730 RVA: 0x000D13B1 File Offset: 0x000CF7B1
			public <Start>c__AnonStorey0()
			{
			}

			// Token: 0x06007423 RID: 29731 RVA: 0x000D13BC File Offset: 0x000CF7BC
			internal void <>m__0(JSONNode data)
			{
				if (data["status"] == 404)
				{
					this.browser.LoadHTML(Resources.Load<TextAsset>("ExplainUnzip").text, null);
					if (HUDManager.Instance)
					{
						HUDManager.Instance.Pause();
					}
					Time.timeScale = 1f;
				}
			}

			// Token: 0x06007424 RID: 29732 RVA: 0x000D1424 File Offset: 0x000CF824
			internal void <>m__1(JSONNode data)
			{
				if (data["error"] == "ERR_ABORTED")
				{
					Browser browser = this.browser;
					if (ExplainUnzip.<Start>c__AnonStorey0.<>f__am$cache0 == null)
					{
						ExplainUnzip.<Start>c__AnonStorey0.<>f__am$cache0 = new Action(ExplainUnzip.<Start>c__AnonStorey0.<>m__2);
					}
					browser.QueuePageReplacer(ExplainUnzip.<Start>c__AnonStorey0.<>f__am$cache0, 1f);
				}
			}

			// Token: 0x06007425 RID: 29733 RVA: 0x000D147D File Offset: 0x000CF87D
			private static void <>m__2()
			{
			}

			// Token: 0x0400683A RID: 26682
			internal Browser browser;

			// Token: 0x0400683B RID: 26683
			private static Action <>f__am$cache0;
		}
	}
}
