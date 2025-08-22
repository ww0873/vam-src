using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine.EventSystems;

namespace UnityEngine.UI.Extensions
{
	// Token: 0x02000554 RID: 1364
	[AddComponentMenu("UI/Extensions/Selectable Scalar")]
	[RequireComponent(typeof(Button))]
	public class SelectableScaler : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IEventSystemHandler
	{
		// Token: 0x060022B9 RID: 8889 RVA: 0x000C5EFD File Offset: 0x000C42FD
		public SelectableScaler()
		{
		}

		// Token: 0x17000420 RID: 1056
		// (get) Token: 0x060022BA RID: 8890 RVA: 0x000C5F10 File Offset: 0x000C4310
		public Selectable Target
		{
			get
			{
				if (this.selectable == null)
				{
					this.selectable = base.GetComponent<Selectable>();
				}
				return this.selectable;
			}
		}

		// Token: 0x060022BB RID: 8891 RVA: 0x000C5F35 File Offset: 0x000C4335
		private void Awake()
		{
			if (this.target == null)
			{
				this.target = base.transform;
			}
			this.initScale = this.target.localScale;
		}

		// Token: 0x060022BC RID: 8892 RVA: 0x000C5F65 File Offset: 0x000C4365
		private void OnEnable()
		{
			this.target.localScale = this.initScale;
		}

		// Token: 0x060022BD RID: 8893 RVA: 0x000C5F78 File Offset: 0x000C4378
		public void OnPointerDown(PointerEventData eventData)
		{
			if (this.Target != null && !this.Target.interactable)
			{
				return;
			}
			base.StopCoroutine("ScaleOUT");
			base.StartCoroutine("ScaleIN");
		}

		// Token: 0x060022BE RID: 8894 RVA: 0x000C5FB3 File Offset: 0x000C43B3
		public void OnPointerUp(PointerEventData eventData)
		{
			if (this.Target != null && !this.Target.interactable)
			{
				return;
			}
			base.StopCoroutine("ScaleIN");
			base.StartCoroutine("ScaleOUT");
		}

		// Token: 0x060022BF RID: 8895 RVA: 0x000C5FF0 File Offset: 0x000C43F0
		private IEnumerator ScaleIN()
		{
			if (this.animCurve.keys.Length > 0)
			{
				this.target.localScale = this.initScale;
				float t = 0f;
				float maxT = this.animCurve.keys[this.animCurve.length - 1].time;
				while (t < maxT)
				{
					t += this.speed * Time.unscaledDeltaTime;
					this.target.localScale = Vector3.one * this.animCurve.Evaluate(t);
					yield return null;
				}
			}
			yield break;
		}

		// Token: 0x060022C0 RID: 8896 RVA: 0x000C600C File Offset: 0x000C440C
		private IEnumerator ScaleOUT()
		{
			if (this.animCurve.keys.Length > 0)
			{
				float t = 0f;
				float maxT = this.animCurve.keys[this.animCurve.length - 1].time;
				while (t < maxT)
				{
					t += this.speed * Time.unscaledDeltaTime;
					this.target.localScale = Vector3.one * this.animCurve.Evaluate(maxT - t);
					yield return null;
				}
				base.transform.localScale = this.initScale;
			}
			yield break;
		}

		// Token: 0x04001CAE RID: 7342
		public AnimationCurve animCurve;

		// Token: 0x04001CAF RID: 7343
		[Tooltip("Animation speed multiplier")]
		public float speed = 1f;

		// Token: 0x04001CB0 RID: 7344
		private Vector3 initScale;

		// Token: 0x04001CB1 RID: 7345
		public Transform target;

		// Token: 0x04001CB2 RID: 7346
		private Selectable selectable;

		// Token: 0x02000F7B RID: 3963
		[CompilerGenerated]
		private sealed class <ScaleIN>c__Iterator0 : IEnumerator, IDisposable, IEnumerator<object>
		{
			// Token: 0x060073FB RID: 29691 RVA: 0x000C6027 File Offset: 0x000C4427
			[DebuggerHidden]
			public <ScaleIN>c__Iterator0()
			{
			}

			// Token: 0x060073FC RID: 29692 RVA: 0x000C6030 File Offset: 0x000C4430
			public bool MoveNext()
			{
				uint num = (uint)this.$PC;
				this.$PC = -1;
				switch (num)
				{
				case 0U:
					if (this.animCurve.keys.Length <= 0)
					{
						goto IL_110;
					}
					this.target.localScale = this.initScale;
					t = 0f;
					maxT = this.animCurve.keys[this.animCurve.length - 1].time;
					break;
				case 1U:
					break;
				default:
					return false;
				}
				if (t < maxT)
				{
					t += this.speed * Time.unscaledDeltaTime;
					this.target.localScale = Vector3.one * this.animCurve.Evaluate(t);
					this.$current = null;
					if (!this.$disposing)
					{
						this.$PC = 1;
					}
					return true;
				}
				IL_110:
				this.$PC = -1;
				return false;
			}

			// Token: 0x17001107 RID: 4359
			// (get) Token: 0x060073FD RID: 29693 RVA: 0x000C6157 File Offset: 0x000C4557
			object IEnumerator<object>.Current
			{
				[DebuggerHidden]
				get
				{
					return this.$current;
				}
			}

			// Token: 0x17001108 RID: 4360
			// (get) Token: 0x060073FE RID: 29694 RVA: 0x000C615F File Offset: 0x000C455F
			object IEnumerator.Current
			{
				[DebuggerHidden]
				get
				{
					return this.$current;
				}
			}

			// Token: 0x060073FF RID: 29695 RVA: 0x000C6167 File Offset: 0x000C4567
			[DebuggerHidden]
			public void Dispose()
			{
				this.$disposing = true;
				this.$PC = -1;
			}

			// Token: 0x06007400 RID: 29696 RVA: 0x000C6177 File Offset: 0x000C4577
			[DebuggerHidden]
			public void Reset()
			{
				throw new NotSupportedException();
			}

			// Token: 0x04006811 RID: 26641
			internal float <t>__1;

			// Token: 0x04006812 RID: 26642
			internal float <maxT>__1;

			// Token: 0x04006813 RID: 26643
			internal SelectableScaler $this;

			// Token: 0x04006814 RID: 26644
			internal object $current;

			// Token: 0x04006815 RID: 26645
			internal bool $disposing;

			// Token: 0x04006816 RID: 26646
			internal int $PC;
		}

		// Token: 0x02000F7C RID: 3964
		[CompilerGenerated]
		private sealed class <ScaleOUT>c__Iterator1 : IEnumerator, IDisposable, IEnumerator<object>
		{
			// Token: 0x06007401 RID: 29697 RVA: 0x000C617E File Offset: 0x000C457E
			[DebuggerHidden]
			public <ScaleOUT>c__Iterator1()
			{
			}

			// Token: 0x06007402 RID: 29698 RVA: 0x000C6188 File Offset: 0x000C4588
			public bool MoveNext()
			{
				uint num = (uint)this.$PC;
				this.$PC = -1;
				switch (num)
				{
				case 0U:
					if (this.animCurve.keys.Length <= 0)
					{
						goto IL_117;
					}
					t = 0f;
					maxT = this.animCurve.keys[this.animCurve.length - 1].time;
					break;
				case 1U:
					break;
				default:
					return false;
				}
				if (t < maxT)
				{
					t += this.speed * Time.unscaledDeltaTime;
					this.target.localScale = Vector3.one * this.animCurve.Evaluate(maxT - t);
					this.$current = null;
					if (!this.$disposing)
					{
						this.$PC = 1;
					}
					return true;
				}
				base.transform.localScale = this.initScale;
				IL_117:
				this.$PC = -1;
				return false;
			}

			// Token: 0x17001109 RID: 4361
			// (get) Token: 0x06007403 RID: 29699 RVA: 0x000C62B6 File Offset: 0x000C46B6
			object IEnumerator<object>.Current
			{
				[DebuggerHidden]
				get
				{
					return this.$current;
				}
			}

			// Token: 0x1700110A RID: 4362
			// (get) Token: 0x06007404 RID: 29700 RVA: 0x000C62BE File Offset: 0x000C46BE
			object IEnumerator.Current
			{
				[DebuggerHidden]
				get
				{
					return this.$current;
				}
			}

			// Token: 0x06007405 RID: 29701 RVA: 0x000C62C6 File Offset: 0x000C46C6
			[DebuggerHidden]
			public void Dispose()
			{
				this.$disposing = true;
				this.$PC = -1;
			}

			// Token: 0x06007406 RID: 29702 RVA: 0x000C62D6 File Offset: 0x000C46D6
			[DebuggerHidden]
			public void Reset()
			{
				throw new NotSupportedException();
			}

			// Token: 0x04006817 RID: 26647
			internal float <t>__1;

			// Token: 0x04006818 RID: 26648
			internal float <maxT>__1;

			// Token: 0x04006819 RID: 26649
			internal SelectableScaler $this;

			// Token: 0x0400681A RID: 26650
			internal object $current;

			// Token: 0x0400681B RID: 26651
			internal bool $disposing;

			// Token: 0x0400681C RID: 26652
			internal int $PC;
		}
	}
}
