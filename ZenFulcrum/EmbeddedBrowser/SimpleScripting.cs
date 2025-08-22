using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace ZenFulcrum.EmbeddedBrowser
{
	// Token: 0x020005AA RID: 1450
	[RequireComponent(typeof(Browser))]
	public class SimpleScripting : MonoBehaviour
	{
		// Token: 0x0600245D RID: 9309 RVA: 0x000D216C File Offset: 0x000D056C
		public SimpleScripting()
		{
		}

		// Token: 0x0600245E RID: 9310 RVA: 0x000D220C File Offset: 0x000D060C
		public void Start()
		{
			this.browser = base.GetComponent<Browser>();
			this.browser.LoadHTML("\n<button style='background: green; color: white' onclick='greenButtonClicked(event.x, event.y)'>Green Button</button>\n\n\n<br><br>\n\n\nUsername: <input type='text' id='username' value='CouchPotato47'>\n\n\n<br><br>\n\n\n<div id='box' style='width: 200px; height: 200px;border: 1px solid black'>\n\tClick \"Change Color\"\n</div>\n\n<script>\nfunction changeColor(r, g, b, text) {\n\tvar el = document.getElementById('box');\n\tel.style.background = 'rgba(' + (r * 255) + ', ' + (g * 255) + ', ' + (b * 255) + ', 1)';\n\tel.textContent = text;\n}\n</script>\n", null);
			Browser browser = this.browser;
			string name = "greenButtonClicked";
			if (SimpleScripting.<>f__am$cache0 == null)
			{
				SimpleScripting.<>f__am$cache0 = new Browser.JSCallback(SimpleScripting.<Start>m__0);
			}
			browser.RegisterFunction(name, SimpleScripting.<>f__am$cache0);
		}

		// Token: 0x0600245F RID: 9311 RVA: 0x000D2264 File Offset: 0x000D0664
		public void GetUsername()
		{
			IPromise<JSONNode> promise = this.browser.EvalJS("document.getElementById('username').value", "scripted command");
			if (SimpleScripting.<>f__am$cache1 == null)
			{
				SimpleScripting.<>f__am$cache1 = new Action<JSONNode>(SimpleScripting.<GetUsername>m__1);
			}
			promise.Then(SimpleScripting.<>f__am$cache1).Done();
			UnityEngine.Debug.Log("Fetching username");
		}

		// Token: 0x06002460 RID: 9312 RVA: 0x000D22B8 File Offset: 0x000D06B8
		public void ChangeColor()
		{
			Color color = this.colors[this.colorIdx++ % this.colors.Length];
			this.browser.CallFunction("changeColor", new JSONNode[]
			{
				color.r,
				color.g,
				color.b,
				"Selection Number " + this.colorIdx
			}).Done();
		}

		// Token: 0x06002461 RID: 9313 RVA: 0x000D2355 File Offset: 0x000D0755
		public void GetUsername2()
		{
			base.StartCoroutine(this._GetUsername2());
		}

		// Token: 0x06002462 RID: 9314 RVA: 0x000D2364 File Offset: 0x000D0764
		private IEnumerator _GetUsername2()
		{
			IPromise<JSONNode> promise = this.browser.EvalJS("document.getElementById('username').value", "scripted command");
			UnityEngine.Debug.Log("Fetching username");
			yield return promise.ToWaitFor(false);
			UnityEngine.Debug.Log("The username is: " + promise.Value);
			yield break;
		}

		// Token: 0x06002463 RID: 9315 RVA: 0x000D2380 File Offset: 0x000D0780
		[CompilerGenerated]
		private static void <Start>m__0(JSONNode args)
		{
			int num = args[0];
			int num2 = args[1];
			UnityEngine.Debug.Log(string.Concat(new object[]
			{
				"The <color=green>green</color> button was clicked at ",
				num,
				", ",
				num2
			}));
		}

		// Token: 0x06002464 RID: 9316 RVA: 0x000D23D9 File Offset: 0x000D07D9
		[CompilerGenerated]
		private static void <GetUsername>m__1(JSONNode username)
		{
			UnityEngine.Debug.Log("The username is: " + username);
		}

		// Token: 0x04001E97 RID: 7831
		private Browser browser;

		// Token: 0x04001E98 RID: 7832
		private int colorIdx;

		// Token: 0x04001E99 RID: 7833
		private Color[] colors = new Color[]
		{
			new Color(1f, 0f, 0f),
			new Color(1f, 1f, 0f),
			new Color(1f, 1f, 1f),
			new Color(1f, 1f, 0f)
		};

		// Token: 0x04001E9A RID: 7834
		[CompilerGenerated]
		private static Browser.JSCallback <>f__am$cache0;

		// Token: 0x04001E9B RID: 7835
		[CompilerGenerated]
		private static Action<JSONNode> <>f__am$cache1;

		// Token: 0x02000F88 RID: 3976
		[CompilerGenerated]
		private sealed class <_GetUsername2>c__Iterator0 : IEnumerator, IDisposable, IEnumerator<object>
		{
			// Token: 0x0600743F RID: 29759 RVA: 0x000D23F0 File Offset: 0x000D07F0
			[DebuggerHidden]
			public <_GetUsername2>c__Iterator0()
			{
			}

			// Token: 0x06007440 RID: 29760 RVA: 0x000D23F8 File Offset: 0x000D07F8
			public bool MoveNext()
			{
				uint num = (uint)this.$PC;
				this.$PC = -1;
				switch (num)
				{
				case 0U:
					promise = this.browser.EvalJS("document.getElementById('username').value", "scripted command");
					UnityEngine.Debug.Log("Fetching username");
					this.$current = promise.ToWaitFor(false);
					if (!this.$disposing)
					{
						this.$PC = 1;
					}
					return true;
				case 1U:
					UnityEngine.Debug.Log("The username is: " + promise.Value);
					this.$PC = -1;
					break;
				}
				return false;
			}

			// Token: 0x1700111B RID: 4379
			// (get) Token: 0x06007441 RID: 29761 RVA: 0x000D249F File Offset: 0x000D089F
			object IEnumerator<object>.Current
			{
				[DebuggerHidden]
				get
				{
					return this.$current;
				}
			}

			// Token: 0x1700111C RID: 4380
			// (get) Token: 0x06007442 RID: 29762 RVA: 0x000D24A7 File Offset: 0x000D08A7
			object IEnumerator.Current
			{
				[DebuggerHidden]
				get
				{
					return this.$current;
				}
			}

			// Token: 0x06007443 RID: 29763 RVA: 0x000D24AF File Offset: 0x000D08AF
			[DebuggerHidden]
			public void Dispose()
			{
				this.$disposing = true;
				this.$PC = -1;
			}

			// Token: 0x06007444 RID: 29764 RVA: 0x000D24BF File Offset: 0x000D08BF
			[DebuggerHidden]
			public void Reset()
			{
				throw new NotSupportedException();
			}

			// Token: 0x04006850 RID: 26704
			internal IPromise<JSONNode> <promise>__0;

			// Token: 0x04006851 RID: 26705
			internal SimpleScripting $this;

			// Token: 0x04006852 RID: 26706
			internal object $current;

			// Token: 0x04006853 RID: 26707
			internal bool $disposing;

			// Token: 0x04006854 RID: 26708
			internal int $PC;
		}
	}
}
