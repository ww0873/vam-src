using System;
using UnityEngine;

namespace Leap.Unity.Animation.Internal
{
	// Token: 0x02000645 RID: 1605
	public abstract class Vector4InterpolatorBase<ObjType> : InterpolatorBase<Vector4, ObjType>
	{
		// Token: 0x06002734 RID: 10036 RVA: 0x000DB21F File Offset: 0x000D961F
		protected Vector4InterpolatorBase()
		{
		}

		// Token: 0x170004CF RID: 1231
		// (get) Token: 0x06002735 RID: 10037 RVA: 0x000DB227 File Offset: 0x000D9627
		public override float length
		{
			get
			{
				return this._b.magnitude;
			}
		}

		// Token: 0x06002736 RID: 10038 RVA: 0x000DB234 File Offset: 0x000D9634
		public new Vector4InterpolatorBase<ObjType> Init(Vector4 a, Vector4 b, ObjType target)
		{
			this._a = a;
			this._b = b - a;
			this._target = target;
			return this;
		}
	}
}
