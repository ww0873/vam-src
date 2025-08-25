using System;
using UnityEngine;

namespace Leap.Unity.Animation.Internal
{
	// Token: 0x02000646 RID: 1606
	public abstract class QuaternionInterpolatorBase<ObjType> : InterpolatorBase<Quaternion, ObjType>
	{
		// Token: 0x06002737 RID: 10039 RVA: 0x000DB252 File Offset: 0x000D9652
		protected QuaternionInterpolatorBase()
		{
		}

		// Token: 0x170004D0 RID: 1232
		// (get) Token: 0x06002738 RID: 10040 RVA: 0x000DB25A File Offset: 0x000D965A
		public override float length
		{
			get
			{
				return Quaternion.Angle(this._a, this._b);
			}
		}

		// Token: 0x06002739 RID: 10041 RVA: 0x000DB26D File Offset: 0x000D966D
		public new QuaternionInterpolatorBase<ObjType> Init(Quaternion a, Quaternion b, ObjType target)
		{
			this._a = a;
			this._b = b;
			this._target = target;
			return this;
		}
	}
}
