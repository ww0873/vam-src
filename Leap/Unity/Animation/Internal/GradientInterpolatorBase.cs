using System;
using UnityEngine;

namespace Leap.Unity.Animation.Internal
{
	// Token: 0x02000648 RID: 1608
	public abstract class GradientInterpolatorBase : IInterpolator, IPoolable, IDisposable
	{
		// Token: 0x0600273D RID: 10045 RVA: 0x000DB2CE File Offset: 0x000D96CE
		protected GradientInterpolatorBase()
		{
		}

		// Token: 0x0600273E RID: 10046 RVA: 0x000DB2D6 File Offset: 0x000D96D6
		public GradientInterpolatorBase Init(Gradient gradient)
		{
			this._gradient = gradient;
			return this;
		}

		// Token: 0x170004D2 RID: 1234
		// (get) Token: 0x0600273F RID: 10047 RVA: 0x000DB2E0 File Offset: 0x000D96E0
		public float length
		{
			get
			{
				return 1f;
			}
		}

		// Token: 0x06002740 RID: 10048
		public abstract void Interpolate(float percent);

		// Token: 0x170004D3 RID: 1235
		// (get) Token: 0x06002741 RID: 10049
		public abstract bool isValid { get; }

		// Token: 0x06002742 RID: 10050 RVA: 0x000DB2E7 File Offset: 0x000D96E7
		public void OnSpawn()
		{
		}

		// Token: 0x06002743 RID: 10051 RVA: 0x000DB2E9 File Offset: 0x000D96E9
		public void Dispose()
		{
		}

		// Token: 0x06002744 RID: 10052 RVA: 0x000DB2EB File Offset: 0x000D96EB
		public void OnRecycle()
		{
		}

		// Token: 0x04002132 RID: 8498
		protected Gradient _gradient;
	}
}
