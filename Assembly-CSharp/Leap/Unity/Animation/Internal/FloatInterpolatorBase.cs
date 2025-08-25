using System;
using UnityEngine;

namespace Leap.Unity.Animation.Internal
{
	// Token: 0x02000642 RID: 1602
	public abstract class FloatInterpolatorBase<ObjType> : InterpolatorBase<float, ObjType>
	{
		// Token: 0x0600272B RID: 10027 RVA: 0x000DB18A File Offset: 0x000D958A
		protected FloatInterpolatorBase()
		{
		}

		// Token: 0x170004CC RID: 1228
		// (get) Token: 0x0600272C RID: 10028 RVA: 0x000DB192 File Offset: 0x000D9592
		public override float length
		{
			get
			{
				return Mathf.Abs(this._b);
			}
		}

		// Token: 0x0600272D RID: 10029 RVA: 0x000DB19F File Offset: 0x000D959F
		public new FloatInterpolatorBase<ObjType> Init(float a, float b, ObjType target)
		{
			this._a = a;
			this._b = b - a;
			this._target = target;
			return this;
		}
	}
}
