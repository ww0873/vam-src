using System;
using UnityEngine;

namespace Leap.Unity.Animation.Internal
{
	// Token: 0x02000643 RID: 1603
	public abstract class Vector2InterpolatorBase<ObjType> : InterpolatorBase<Vector2, ObjType>
	{
		// Token: 0x0600272E RID: 10030 RVA: 0x000DB1B9 File Offset: 0x000D95B9
		protected Vector2InterpolatorBase()
		{
		}

		// Token: 0x170004CD RID: 1229
		// (get) Token: 0x0600272F RID: 10031 RVA: 0x000DB1C1 File Offset: 0x000D95C1
		public override float length
		{
			get
			{
				return this._b.magnitude;
			}
		}

		// Token: 0x06002730 RID: 10032 RVA: 0x000DB1CE File Offset: 0x000D95CE
		public new Vector2InterpolatorBase<ObjType> Init(Vector2 a, Vector2 b, ObjType target)
		{
			this._a = a;
			this._b = b - a;
			this._target = target;
			return this;
		}
	}
}
