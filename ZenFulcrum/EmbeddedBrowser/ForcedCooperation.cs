using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace ZenFulcrum.EmbeddedBrowser
{
	// Token: 0x020005A3 RID: 1443
	public class ForcedCooperation : MonoBehaviour
	{
		// Token: 0x0600242C RID: 9260 RVA: 0x000D147F File Offset: 0x000CF87F
		public ForcedCooperation()
		{
		}

		// Token: 0x0600242D RID: 9261 RVA: 0x000D1487 File Offset: 0x000CF887
		public void Comply()
		{
			base.StartCoroutine(this._Comply());
		}

		// Token: 0x0600242E RID: 9262 RVA: 0x000D1498 File Offset: 0x000CF898
		protected IEnumerator _Comply()
		{
			float t0 = Time.time;
			do
			{
				Vector3 pos = base.transform.InverseTransformPoint(this.whoWillComply.position);
				if (pos.z > 0f)
				{
					pos.z = 0f;
					this.whoWillComply.position = base.transform.TransformPoint(pos);
				}
				yield return null;
			}
			while (Time.time - t0 < this.howLongWillTheyComply);
			yield break;
		}

		// Token: 0x04001E78 RID: 7800
		public Transform whoWillComply;

		// Token: 0x04001E79 RID: 7801
		public float howLongWillTheyComply;

		// Token: 0x02000F84 RID: 3972
		[CompilerGenerated]
		private sealed class <_Comply>c__Iterator0 : IEnumerator, IDisposable, IEnumerator<object>
		{
			// Token: 0x06007426 RID: 29734 RVA: 0x000D14B3 File Offset: 0x000CF8B3
			[DebuggerHidden]
			public <_Comply>c__Iterator0()
			{
			}

			// Token: 0x06007427 RID: 29735 RVA: 0x000D14BC File Offset: 0x000CF8BC
			public bool MoveNext()
			{
				uint num = (uint)this.$PC;
				this.$PC = -1;
				switch (num)
				{
				case 0U:
					t0 = Time.time;
					break;
				case 1U:
					if (Time.time - t0 >= this.howLongWillTheyComply)
					{
						this.$PC = -1;
						return false;
					}
					break;
				default:
					return false;
				}
				pos = base.transform.InverseTransformPoint(this.whoWillComply.position);
				if (pos.z > 0f)
				{
					pos.z = 0f;
					this.whoWillComply.position = base.transform.TransformPoint(pos);
				}
				this.$current = null;
				if (!this.$disposing)
				{
					this.$PC = 1;
				}
				return true;
			}

			// Token: 0x17001113 RID: 4371
			// (get) Token: 0x06007428 RID: 29736 RVA: 0x000D15A7 File Offset: 0x000CF9A7
			object IEnumerator<object>.Current
			{
				[DebuggerHidden]
				get
				{
					return this.$current;
				}
			}

			// Token: 0x17001114 RID: 4372
			// (get) Token: 0x06007429 RID: 29737 RVA: 0x000D15AF File Offset: 0x000CF9AF
			object IEnumerator.Current
			{
				[DebuggerHidden]
				get
				{
					return this.$current;
				}
			}

			// Token: 0x0600742A RID: 29738 RVA: 0x000D15B7 File Offset: 0x000CF9B7
			[DebuggerHidden]
			public void Dispose()
			{
				this.$disposing = true;
				this.$PC = -1;
			}

			// Token: 0x0600742B RID: 29739 RVA: 0x000D15C7 File Offset: 0x000CF9C7
			[DebuggerHidden]
			public void Reset()
			{
				throw new NotSupportedException();
			}

			// Token: 0x0400683C RID: 26684
			internal float <t0>__0;

			// Token: 0x0400683D RID: 26685
			internal Vector3 <pos>__1;

			// Token: 0x0400683E RID: 26686
			internal ForcedCooperation $this;

			// Token: 0x0400683F RID: 26687
			internal object $current;

			// Token: 0x04006840 RID: 26688
			internal bool $disposing;

			// Token: 0x04006841 RID: 26689
			internal int $PC;
		}
	}
}
