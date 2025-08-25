using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Leap.Unity
{
	// Token: 0x020006E4 RID: 1764
	public class HandDrop : HandTransitionBehavior
	{
		// Token: 0x06002AA9 RID: 10921 RVA: 0x000E6AC0 File Offset: 0x000E4EC0
		public HandDrop()
		{
		}

		// Token: 0x06002AAA RID: 10922 RVA: 0x000E6AC8 File Offset: 0x000E4EC8
		protected override void Awake()
		{
			base.Awake();
			this.palm = base.GetComponent<HandModel>().palm;
			this.startingPalmPosition = this.palm.localPosition;
			this.startingOrientation = this.palm.localRotation;
		}

		// Token: 0x06002AAB RID: 10923 RVA: 0x000E6B03 File Offset: 0x000E4F03
		protected override void HandFinish()
		{
			base.StartCoroutine(this.LerpToStart());
		}

		// Token: 0x06002AAC RID: 10924 RVA: 0x000E6B12 File Offset: 0x000E4F12
		protected override void HandReset()
		{
			base.StopAllCoroutines();
		}

		// Token: 0x06002AAD RID: 10925 RVA: 0x000E6B1C File Offset: 0x000E4F1C
		private IEnumerator LerpToStart()
		{
			Vector3 droppedPosition = this.palm.localPosition;
			Quaternion droppedOrientation = this.palm.localRotation;
			float duration = 0.25f;
			float startTime = Time.time;
			float endTime = startTime + duration;
			while (Time.time <= endTime)
			{
				float t = (Time.time - startTime) / duration;
				this.palm.localPosition = Vector3.Lerp(droppedPosition, this.startingPalmPosition, t);
				this.palm.localRotation = Quaternion.Lerp(droppedOrientation, this.startingOrientation, t);
				yield return null;
			}
			yield break;
		}

		// Token: 0x040022AF RID: 8879
		private Vector3 startingPalmPosition;

		// Token: 0x040022B0 RID: 8880
		private Quaternion startingOrientation;

		// Token: 0x040022B1 RID: 8881
		private Transform palm;

		// Token: 0x02000FA7 RID: 4007
		[CompilerGenerated]
		private sealed class <LerpToStart>c__Iterator0 : IEnumerator, IDisposable, IEnumerator<object>
		{
			// Token: 0x060074AE RID: 29870 RVA: 0x000E6B37 File Offset: 0x000E4F37
			[DebuggerHidden]
			public <LerpToStart>c__Iterator0()
			{
			}

			// Token: 0x060074AF RID: 29871 RVA: 0x000E6B40 File Offset: 0x000E4F40
			public bool MoveNext()
			{
				uint num = (uint)this.$PC;
				this.$PC = -1;
				switch (num)
				{
				case 0U:
					droppedPosition = this.palm.localPosition;
					droppedOrientation = this.palm.localRotation;
					duration = 0.25f;
					startTime = Time.time;
					endTime = startTime + duration;
					break;
				case 1U:
					break;
				default:
					return false;
				}
				if (Time.time <= endTime)
				{
					t = (Time.time - startTime) / duration;
					this.palm.localPosition = Vector3.Lerp(droppedPosition, this.startingPalmPosition, t);
					this.palm.localRotation = Quaternion.Lerp(droppedOrientation, this.startingOrientation, t);
					this.$current = null;
					if (!this.$disposing)
					{
						this.$PC = 1;
					}
					return true;
				}
				this.$PC = -1;
				return false;
			}

			// Token: 0x17001133 RID: 4403
			// (get) Token: 0x060074B0 RID: 29872 RVA: 0x000E6C6E File Offset: 0x000E506E
			object IEnumerator<object>.Current
			{
				[DebuggerHidden]
				get
				{
					return this.$current;
				}
			}

			// Token: 0x17001134 RID: 4404
			// (get) Token: 0x060074B1 RID: 29873 RVA: 0x000E6C76 File Offset: 0x000E5076
			object IEnumerator.Current
			{
				[DebuggerHidden]
				get
				{
					return this.$current;
				}
			}

			// Token: 0x060074B2 RID: 29874 RVA: 0x000E6C7E File Offset: 0x000E507E
			[DebuggerHidden]
			public void Dispose()
			{
				this.$disposing = true;
				this.$PC = -1;
			}

			// Token: 0x060074B3 RID: 29875 RVA: 0x000E6C8E File Offset: 0x000E508E
			[DebuggerHidden]
			public void Reset()
			{
				throw new NotSupportedException();
			}

			// Token: 0x040068C5 RID: 26821
			internal Vector3 <droppedPosition>__0;

			// Token: 0x040068C6 RID: 26822
			internal Quaternion <droppedOrientation>__0;

			// Token: 0x040068C7 RID: 26823
			internal float <duration>__0;

			// Token: 0x040068C8 RID: 26824
			internal float <startTime>__0;

			// Token: 0x040068C9 RID: 26825
			internal float <endTime>__0;

			// Token: 0x040068CA RID: 26826
			internal float <t>__1;

			// Token: 0x040068CB RID: 26827
			internal HandDrop $this;

			// Token: 0x040068CC RID: 26828
			internal object $current;

			// Token: 0x040068CD RID: 26829
			internal bool $disposing;

			// Token: 0x040068CE RID: 26830
			internal int $PC;
		}
	}
}
