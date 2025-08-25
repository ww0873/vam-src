using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;
using UnityEngine;

namespace Leap.Unity
{
	// Token: 0x020006FA RID: 1786
	public abstract class LeapProvider : MonoBehaviour
	{
		// Token: 0x06002B48 RID: 11080 RVA: 0x000D331A File Offset: 0x000D171A
		protected LeapProvider()
		{
		}

		// Token: 0x140000B6 RID: 182
		// (add) Token: 0x06002B49 RID: 11081 RVA: 0x000D3324 File Offset: 0x000D1724
		// (remove) Token: 0x06002B4A RID: 11082 RVA: 0x000D335C File Offset: 0x000D175C
		public event Action<Frame> OnUpdateFrame
		{
			add
			{
				Action<Frame> action = this.OnUpdateFrame;
				Action<Frame> action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange<Action<Frame>>(ref this.OnUpdateFrame, (Action<Frame>)Delegate.Combine(action2, value), action);
				}
				while (action != action2);
			}
			remove
			{
				Action<Frame> action = this.OnUpdateFrame;
				Action<Frame> action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange<Action<Frame>>(ref this.OnUpdateFrame, (Action<Frame>)Delegate.Remove(action2, value), action);
				}
				while (action != action2);
			}
		}

		// Token: 0x140000B7 RID: 183
		// (add) Token: 0x06002B4B RID: 11083 RVA: 0x000D3394 File Offset: 0x000D1794
		// (remove) Token: 0x06002B4C RID: 11084 RVA: 0x000D33CC File Offset: 0x000D17CC
		public event Action<Frame> OnFixedFrame
		{
			add
			{
				Action<Frame> action = this.OnFixedFrame;
				Action<Frame> action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange<Action<Frame>>(ref this.OnFixedFrame, (Action<Frame>)Delegate.Combine(action2, value), action);
				}
				while (action != action2);
			}
			remove
			{
				Action<Frame> action = this.OnFixedFrame;
				Action<Frame> action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange<Action<Frame>>(ref this.OnFixedFrame, (Action<Frame>)Delegate.Remove(action2, value), action);
				}
				while (action != action2);
			}
		}

		// Token: 0x17000571 RID: 1393
		// (get) Token: 0x06002B4D RID: 11085
		public abstract Frame CurrentFrame { get; }

		// Token: 0x17000572 RID: 1394
		// (get) Token: 0x06002B4E RID: 11086
		public abstract Frame CurrentFixedFrame { get; }

		// Token: 0x06002B4F RID: 11087 RVA: 0x000D3402 File Offset: 0x000D1802
		protected void DispatchUpdateFrameEvent(Frame frame)
		{
			if (this.OnUpdateFrame != null)
			{
				this.OnUpdateFrame(frame);
			}
		}

		// Token: 0x06002B50 RID: 11088 RVA: 0x000D341B File Offset: 0x000D181B
		protected void DispatchFixedFrameEvent(Frame frame)
		{
			if (this.OnFixedFrame != null)
			{
				this.OnFixedFrame(frame);
			}
		}

		// Token: 0x040022FB RID: 8955
		public TestHandFactory.TestHandPose editTimePose;

		// Token: 0x040022FC RID: 8956
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private Action<Frame> OnUpdateFrame;

		// Token: 0x040022FD RID: 8957
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private Action<Frame> OnFixedFrame;
	}
}
