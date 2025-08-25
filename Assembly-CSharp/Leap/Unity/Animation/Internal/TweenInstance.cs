using System;
using System.Collections;
using UnityEngine;

namespace Leap.Unity.Animation.Internal
{
	// Token: 0x02000663 RID: 1635
	public class TweenInstance : IPoolable, IDisposable
	{
		// Token: 0x060027FD RID: 10237 RVA: 0x000DC8BF File Offset: 0x000DACBF
		public TweenInstance()
		{
			this.ResetDefaults();
		}

		// Token: 0x060027FE RID: 10238 RVA: 0x000DC8E8 File Offset: 0x000DACE8
		public void OnSpawn()
		{
			this.instanceId = TweenInstance._nextInstanceId++;
			this.yieldInstruction = new TweenInstance.TweenYieldInstruction(this);
		}

		// Token: 0x060027FF RID: 10239 RVA: 0x000DC909 File Offset: 0x000DAD09
		public void OnRecycle()
		{
		}

		// Token: 0x06002800 RID: 10240 RVA: 0x000DC90C File Offset: 0x000DAD0C
		public void ResetDefaults()
		{
			this.returnToPoolUponStop = true;
			this.curPercent = 0f;
			this.dstPercent = 1f;
			this.velPercent = 1f;
			this.direction = Direction.Forward;
			this.smoothType = SmoothType.Linear;
			this.smoothFunction = null;
			this.OnProgress = null;
			this.OnLeaveEnd = null;
			this.OnReachEnd = null;
			this.OnLeaveStart = null;
			this.OnReachStart = null;
		}

		// Token: 0x06002801 RID: 10241 RVA: 0x000DC97C File Offset: 0x000DAD7C
		public void Dispose()
		{
			this.instanceId = -1;
			for (int i = 0; i < this.interpolatorCount; i++)
			{
				this.interpolators[i].Dispose();
				this.interpolators[i] = null;
			}
			this.interpolatorCount = 0;
			this.ResetDefaults();
			Pool<TweenInstance>.Recycle(this);
		}

		// Token: 0x06002802 RID: 10242 RVA: 0x000DC9D0 File Offset: 0x000DADD0
		public void Step(TweenRunner runner)
		{
			this.curPercent = Mathf.MoveTowards(this.curPercent, this.dstPercent, Time.deltaTime * this.velPercent);
			this.interpolatePercent();
			if (this.curPercent == this.dstPercent)
			{
				runner.RemoveTween(this);
			}
		}

		// Token: 0x06002803 RID: 10243 RVA: 0x000DCA20 File Offset: 0x000DAE20
		public void interpolatePercent()
		{
			float percent;
			switch (this.smoothType)
			{
			case SmoothType.Linear:
				percent = this.curPercent;
				break;
			case SmoothType.Smooth:
				percent = Mathf.SmoothStep(0f, 1f, this.curPercent);
				break;
			case SmoothType.SmoothEnd:
				percent = 1f - (this.curPercent - 1f) * (this.curPercent - 1f);
				break;
			case SmoothType.SmoothStart:
				percent = this.curPercent * this.curPercent;
				break;
			default:
				percent = this.smoothFunction(this.curPercent);
				break;
			}
			int num = this.interpolatorCount;
			while (num-- != 0)
			{
				IInterpolator interpolator = this.interpolators[num];
				if (interpolator.isValid)
				{
					this.interpolators[num].Interpolate(percent);
				}
				else
				{
					this.interpolators[num] = this.interpolators[--this.interpolatorCount];
				}
			}
			if (this.OnProgress != null)
			{
				this.OnProgress(this.curPercent);
			}
		}

		// Token: 0x06002804 RID: 10244 RVA: 0x000DCB42 File Offset: 0x000DAF42
		// Note: this type is marked as 'beforefieldinit'.
		static TweenInstance()
		{
		}

		// Token: 0x0400213D RID: 8509
		private static int _nextInstanceId = 1;

		// Token: 0x0400213E RID: 8510
		public const int ID_UNUSED = 0;

		// Token: 0x0400213F RID: 8511
		public const int ID_IN_POOL = -1;

		// Token: 0x04002140 RID: 8512
		public const int ID_WAITING_FOR_RECYCLE = -2;

		// Token: 0x04002141 RID: 8513
		public const int ID_INVALID_STATE = -3;

		// Token: 0x04002142 RID: 8514
		public int instanceId = -3;

		// Token: 0x04002143 RID: 8515
		public const int NOT_RUNNING = -1;

		// Token: 0x04002144 RID: 8516
		public int runnerIndex = -1;

		// Token: 0x04002145 RID: 8517
		public bool returnToPoolUponStop;

		// Token: 0x04002146 RID: 8518
		public IInterpolator[] interpolators = new IInterpolator[1];

		// Token: 0x04002147 RID: 8519
		public int interpolatorCount;

		// Token: 0x04002148 RID: 8520
		public float curPercent;

		// Token: 0x04002149 RID: 8521
		public float dstPercent;

		// Token: 0x0400214A RID: 8522
		public float velPercent;

		// Token: 0x0400214B RID: 8523
		public Direction direction;

		// Token: 0x0400214C RID: 8524
		public SmoothType smoothType;

		// Token: 0x0400214D RID: 8525
		public Func<float, float> smoothFunction;

		// Token: 0x0400214E RID: 8526
		public Action<float> OnProgress;

		// Token: 0x0400214F RID: 8527
		public Action OnLeaveEnd;

		// Token: 0x04002150 RID: 8528
		public Action OnReachEnd;

		// Token: 0x04002151 RID: 8529
		public Action OnLeaveStart;

		// Token: 0x04002152 RID: 8530
		public Action OnReachStart;

		// Token: 0x04002153 RID: 8531
		public TweenInstance.TweenYieldInstruction yieldInstruction;

		// Token: 0x02000664 RID: 1636
		public struct TweenYieldInstruction : IEnumerator
		{
			// Token: 0x06002805 RID: 10245 RVA: 0x000DCB4A File Offset: 0x000DAF4A
			public TweenYieldInstruction(TweenInstance instance)
			{
				this._instance = instance;
				this._instanceId = this._instance.instanceId;
			}

			// Token: 0x170004F5 RID: 1269
			// (get) Token: 0x06002806 RID: 10246 RVA: 0x000DCB64 File Offset: 0x000DAF64
			public object Current
			{
				get
				{
					return null;
				}
			}

			// Token: 0x06002807 RID: 10247 RVA: 0x000DCB67 File Offset: 0x000DAF67
			public bool MoveNext()
			{
				return this._instanceId == this._instance.instanceId && this._instance.runnerIndex != -1;
			}

			// Token: 0x06002808 RID: 10248 RVA: 0x000DCB93 File Offset: 0x000DAF93
			public void Reset()
			{
			}

			// Token: 0x04002154 RID: 8532
			private TweenInstance _instance;

			// Token: 0x04002155 RID: 8533
			private int _instanceId;
		}
	}
}
