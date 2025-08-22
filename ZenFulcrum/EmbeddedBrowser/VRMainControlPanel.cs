using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace ZenFulcrum.EmbeddedBrowser
{
	// Token: 0x020005AE RID: 1454
	public class VRMainControlPanel : MonoBehaviour
	{
		// Token: 0x0600247B RID: 9339 RVA: 0x000D2D01 File Offset: 0x000D1101
		public VRMainControlPanel()
		{
		}

		// Token: 0x0600247C RID: 9340 RVA: 0x000D2D3C File Offset: 0x000D113C
		public void Awake()
		{
			VRMainControlPanel.instance = this;
			Browser component = base.GetComponent<Browser>();
			component.RegisterFunction("openNewTab", new Browser.JSCallback(this.<Awake>m__0));
			component.RegisterFunction("shiftTabs", new Browser.JSCallback(this.<Awake>m__1));
			this.baseKeyboardScale = this.keyboard.transform.localScale;
			this.OpenNewTab(null);
		}

		// Token: 0x0600247D RID: 9341 RVA: 0x000D2DA4 File Offset: 0x000D11A4
		private void ShiftTabs(int direction)
		{
			VRMainControlPanel.<ShiftTabs>c__AnonStorey1 <ShiftTabs>c__AnonStorey = new VRMainControlPanel.<ShiftTabs>c__AnonStorey1();
			<ShiftTabs>c__AnonStorey.direction = direction;
			<ShiftTabs>c__AnonStorey.$this = this;
			this.allBrowsers = this.allBrowsers.Select(new Func<VRBrowserPanel, int, VRBrowserPanel>(<ShiftTabs>c__AnonStorey.<>m__0)).ToList<VRBrowserPanel>();
		}

		// Token: 0x0600247E RID: 9342 RVA: 0x000D2DE8 File Offset: 0x000D11E8
		public VRBrowserPanel OpenNewTab(VRBrowserPanel nextTo = null)
		{
			VRMainControlPanel.<OpenNewTab>c__AnonStorey2 <OpenNewTab>c__AnonStorey = new VRMainControlPanel.<OpenNewTab>c__AnonStorey2();
			<OpenNewTab>c__AnonStorey.nextTo = nextTo;
			GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.browserPrefab);
			VRBrowserPanel component = gameObject.GetComponent<VRBrowserPanel>();
			int num = -1;
			if (<OpenNewTab>c__AnonStorey.nextTo)
			{
				num = this.allBrowsers.FindIndex(new Predicate<VRBrowserPanel>(<OpenNewTab>c__AnonStorey.<>m__0));
			}
			if (num > 0)
			{
				this.allBrowsers.Insert(num + 1, component);
			}
			else
			{
				this.allBrowsers.Insert(this.allBrowsers.Count / 2, component);
			}
			component.transform.position = base.transform.position;
			component.transform.rotation = base.transform.rotation;
			component.transform.localScale = Vector3.zero;
			return component;
		}

		// Token: 0x0600247F RID: 9343 RVA: 0x000D2EB0 File Offset: 0x000D12B0
		public void MoveKeyboardUnder(VRBrowserPanel panel)
		{
			this.keyboardTarget = panel;
		}

		// Token: 0x06002480 RID: 9344 RVA: 0x000D2EB9 File Offset: 0x000D12B9
		public void DestroyPane(VRBrowserPanel pane)
		{
			base.StartCoroutine(this._DestroyBrowser(pane));
		}

		// Token: 0x06002481 RID: 9345 RVA: 0x000D2ECC File Offset: 0x000D12CC
		private IEnumerator _DestroyBrowser(VRBrowserPanel pane)
		{
			this.allBrowsers.Remove(pane);
			if (!pane)
			{
				yield break;
			}
			Vector3 targetPos = pane.transform.position;
			targetPos.y = 0f;
			float t0 = Time.time;
			while (Time.time < t0 + 3f)
			{
				if (!pane)
				{
					yield break;
				}
				this.MoveToward(pane.transform, targetPos, pane.transform.rotation, Vector3.zero);
				yield return null;
			}
			UnityEngine.Object.Destroy(pane.gameObject);
			yield break;
		}

		// Token: 0x06002482 RID: 9346 RVA: 0x000D2EF0 File Offset: 0x000D12F0
		private void MoveToward(Transform t, Vector3 pos, Quaternion rot, Vector3 scale)
		{
			t.position = Vector3.Lerp(t.position, pos, this.moveSpeed);
			t.rotation = Quaternion.Slerp(t.rotation, rot, this.moveSpeed);
			t.localScale = Vector3.Lerp(t.localScale, scale, this.moveSpeed);
		}

		// Token: 0x06002483 RID: 9347 RVA: 0x000D2F48 File Offset: 0x000D1348
		public void Update()
		{
			float t = Mathf.Clamp01((float)(this.allBrowsers.Count - 1) / (float)this.browsersToFit);
			float num = Mathf.Lerp(0f, 360f, t);
			float num2 = Mathf.Lerp(0f, 180f, t);
			float num3 = Mathf.Lerp(1f, 0f, t);
			for (int i = 0; i < this.allBrowsers.Count; i++)
			{
				float num4 = (float)i / ((float)this.allBrowsers.Count - num3) * num - num2;
				if (i == 0 && this.allBrowsers.Count == 1)
				{
					num4 = num / 2f - num2;
				}
				num4 *= 0.017453292f;
				Vector3 pos = new Vector3(Mathf.Sin(num4) * this.radius, this.height, Mathf.Cos(num4) * this.radius);
				Quaternion rot = Quaternion.LookRotation(new Vector3(pos.x, 0f, pos.z), Vector3.up);
				this.MoveToward(this.allBrowsers[i].transform, pos, rot, Vector3.one);
			}
			Vector3 pos2 = (!this.keyboardTarget) ? Vector3.zero : this.keyboardTarget.keyboardLocation.position;
			Quaternion rot2 = (!this.keyboardTarget) ? Quaternion.LookRotation(Vector3.down, Vector3.forward) : this.keyboardTarget.keyboardLocation.rotation;
			Vector3 scale = (!this.keyboardTarget) ? Vector3.zero : this.baseKeyboardScale;
			this.MoveToward(this.keyboard.transform, pos2, rot2, scale);
		}

		// Token: 0x06002484 RID: 9348 RVA: 0x000D3112 File Offset: 0x000D1512
		[CompilerGenerated]
		private void <Awake>m__0(JSONNode args)
		{
			this.OpenNewTab(null);
		}

		// Token: 0x06002485 RID: 9349 RVA: 0x000D311C File Offset: 0x000D151C
		[CompilerGenerated]
		private void <Awake>m__1(JSONNode args)
		{
			this.ShiftTabs(args[0]);
		}

		// Token: 0x04001EA5 RID: 7845
		public static VRMainControlPanel instance;

		// Token: 0x04001EA6 RID: 7846
		public GameObject browserPrefab;

		// Token: 0x04001EA7 RID: 7847
		public float moveSpeed = 0.01f;

		// Token: 0x04001EA8 RID: 7848
		public float height = 1.5f;

		// Token: 0x04001EA9 RID: 7849
		public float radius = 2f;

		// Token: 0x04001EAA RID: 7850
		public int browsersToFit = 8;

		// Token: 0x04001EAB RID: 7851
		protected List<VRBrowserPanel> allBrowsers = new List<VRBrowserPanel>();

		// Token: 0x04001EAC RID: 7852
		public ExternalKeyboard keyboard;

		// Token: 0x04001EAD RID: 7853
		private Vector3 baseKeyboardScale;

		// Token: 0x04001EAE RID: 7854
		private VRBrowserPanel keyboardTarget;

		// Token: 0x02000F8A RID: 3978
		[CompilerGenerated]
		private sealed class <ShiftTabs>c__AnonStorey1
		{
			// Token: 0x06007447 RID: 29767 RVA: 0x000D3130 File Offset: 0x000D1530
			public <ShiftTabs>c__AnonStorey1()
			{
			}

			// Token: 0x06007448 RID: 29768 RVA: 0x000D3138 File Offset: 0x000D1538
			internal VRBrowserPanel <>m__0(VRBrowserPanel t, int i)
			{
				return this.$this.allBrowsers[(i + this.direction + this.$this.allBrowsers.Count) % this.$this.allBrowsers.Count];
			}

			// Token: 0x04006857 RID: 26711
			internal int direction;

			// Token: 0x04006858 RID: 26712
			internal VRMainControlPanel $this;
		}

		// Token: 0x02000F8B RID: 3979
		[CompilerGenerated]
		private sealed class <OpenNewTab>c__AnonStorey2
		{
			// Token: 0x06007449 RID: 29769 RVA: 0x000D3174 File Offset: 0x000D1574
			public <OpenNewTab>c__AnonStorey2()
			{
			}

			// Token: 0x0600744A RID: 29770 RVA: 0x000D317C File Offset: 0x000D157C
			internal bool <>m__0(VRBrowserPanel x)
			{
				return x == this.nextTo;
			}

			// Token: 0x04006859 RID: 26713
			internal VRBrowserPanel nextTo;
		}

		// Token: 0x02000F8C RID: 3980
		[CompilerGenerated]
		private sealed class <_DestroyBrowser>c__Iterator0 : IEnumerator, IDisposable, IEnumerator<object>
		{
			// Token: 0x0600744B RID: 29771 RVA: 0x000D318A File Offset: 0x000D158A
			[DebuggerHidden]
			public <_DestroyBrowser>c__Iterator0()
			{
			}

			// Token: 0x0600744C RID: 29772 RVA: 0x000D3194 File Offset: 0x000D1594
			public bool MoveNext()
			{
				uint num = (uint)this.$PC;
				this.$PC = -1;
				switch (num)
				{
				case 0U:
					this.allBrowsers.Remove(pane);
					if (!pane)
					{
						return false;
					}
					targetPos = pane.transform.position;
					targetPos.y = 0f;
					t0 = Time.time;
					break;
				case 1U:
					break;
				default:
					return false;
				}
				if (Time.time >= t0 + 3f)
				{
					UnityEngine.Object.Destroy(pane.gameObject);
					this.$PC = -1;
				}
				else if (pane)
				{
					base.MoveToward(pane.transform, targetPos, pane.transform.rotation, Vector3.zero);
					this.$current = null;
					if (!this.$disposing)
					{
						this.$PC = 1;
					}
					return true;
				}
				return false;
			}

			// Token: 0x1700111D RID: 4381
			// (get) Token: 0x0600744D RID: 29773 RVA: 0x000D32B5 File Offset: 0x000D16B5
			object IEnumerator<object>.Current
			{
				[DebuggerHidden]
				get
				{
					return this.$current;
				}
			}

			// Token: 0x1700111E RID: 4382
			// (get) Token: 0x0600744E RID: 29774 RVA: 0x000D32BD File Offset: 0x000D16BD
			object IEnumerator.Current
			{
				[DebuggerHidden]
				get
				{
					return this.$current;
				}
			}

			// Token: 0x0600744F RID: 29775 RVA: 0x000D32C5 File Offset: 0x000D16C5
			[DebuggerHidden]
			public void Dispose()
			{
				this.$disposing = true;
				this.$PC = -1;
			}

			// Token: 0x06007450 RID: 29776 RVA: 0x000D32D5 File Offset: 0x000D16D5
			[DebuggerHidden]
			public void Reset()
			{
				throw new NotSupportedException();
			}

			// Token: 0x0400685A RID: 26714
			internal VRBrowserPanel pane;

			// Token: 0x0400685B RID: 26715
			internal Vector3 <targetPos>__0;

			// Token: 0x0400685C RID: 26716
			internal float <t0>__0;

			// Token: 0x0400685D RID: 26717
			internal VRMainControlPanel $this;

			// Token: 0x0400685E RID: 26718
			internal object $current;

			// Token: 0x0400685F RID: 26719
			internal bool $disposing;

			// Token: 0x04006860 RID: 26720
			internal int $PC;
		}
	}
}
