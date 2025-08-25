using System;
using UnityEngine;

namespace Leap.Unity.Animation.Internal
{
	// Token: 0x02000644 RID: 1604
	public abstract class Vector3InterpolatorBase<ObjType> : InterpolatorBase<Vector3, ObjType>
	{
		// Token: 0x06002731 RID: 10033 RVA: 0x000DB1EC File Offset: 0x000D95EC
		protected Vector3InterpolatorBase()
		{
		}

		// Token: 0x170004CE RID: 1230
		// (get) Token: 0x06002732 RID: 10034 RVA: 0x000DB1F4 File Offset: 0x000D95F4
		public override float length
		{
			get
			{
				return this._b.magnitude;
			}
		}

		// Token: 0x06002733 RID: 10035 RVA: 0x000DB201 File Offset: 0x000D9601
		public new Vector3InterpolatorBase<ObjType> Init(Vector3 a, Vector3 b, ObjType target)
		{
			this._a = a;
			this._b = b - a;
			this._target = target;
			return this;
		}
	}
}
