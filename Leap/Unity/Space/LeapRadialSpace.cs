using System;
using Leap.Unity.Attributes;
using UnityEngine;

namespace Leap.Unity.Space
{
	// Token: 0x02000713 RID: 1811
	public abstract class LeapRadialSpace : LeapSpace
	{
		// Token: 0x06002C1D RID: 11293 RVA: 0x000ECF7F File Offset: 0x000EB37F
		protected LeapRadialSpace()
		{
		}

		// Token: 0x17000584 RID: 1412
		// (get) Token: 0x06002C1E RID: 11294 RVA: 0x000ECF92 File Offset: 0x000EB392
		// (set) Token: 0x06002C1F RID: 11295 RVA: 0x000ECF9A File Offset: 0x000EB39A
		public float radius
		{
			get
			{
				return this._radius;
			}
			set
			{
				this._radius = value;
			}
		}

		// Token: 0x06002C20 RID: 11296 RVA: 0x000ECFA4 File Offset: 0x000EB3A4
		public override Hash GetSettingHash()
		{
			return new Hash
			{
				this._radius
			};
		}

		// Token: 0x06002C21 RID: 11297 RVA: 0x000ECFC8 File Offset: 0x000EB3C8
		protected sealed override void UpdateTransformer(ITransformer transformer, ITransformer parent)
		{
			Vector3 a = base.transform.InverseTransformPoint(transformer.anchor.transform.position);
			Vector3 b = base.transform.InverseTransformPoint(parent.anchor.transform.position);
			Vector3 rectSpaceDelta = a - b;
			this.UpdateRadialTransformer(transformer, parent, rectSpaceDelta);
		}

		// Token: 0x06002C22 RID: 11298
		protected abstract void UpdateRadialTransformer(ITransformer transformer, ITransformer parent, Vector3 rectSpaceDelta);

		// Token: 0x04002366 RID: 9062
		[MinValue(0.001f)]
		[SerializeField]
		private float _radius = 1f;
	}
}
