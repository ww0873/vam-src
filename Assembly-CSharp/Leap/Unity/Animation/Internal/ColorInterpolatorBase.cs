using System;
using UnityEngine;

namespace Leap.Unity.Animation.Internal
{
	// Token: 0x02000647 RID: 1607
	public abstract class ColorInterpolatorBase<ObjType> : InterpolatorBase<Color, ObjType>
	{
		// Token: 0x0600273A RID: 10042 RVA: 0x000DB285 File Offset: 0x000D9685
		protected ColorInterpolatorBase()
		{
		}

		// Token: 0x170004D1 RID: 1233
		// (get) Token: 0x0600273B RID: 10043 RVA: 0x000DB290 File Offset: 0x000D9690
		public override float length
		{
			get
			{
				return this._b.magnitude;
			}
		}

		// Token: 0x0600273C RID: 10044 RVA: 0x000DB2B0 File Offset: 0x000D96B0
		public new ColorInterpolatorBase<ObjType> Init(Color a, Color b, ObjType target)
		{
			this._a = a;
			this._b = b - a;
			this._target = target;
			return this;
		}
	}
}
