using System;
using UnityEngine;

namespace Obi
{
	// Token: 0x020003A6 RID: 934
	public class ObiCollisionMaterial : ScriptableObject
	{
		// Token: 0x0600179B RID: 6043 RVA: 0x00086DBC File Offset: 0x000851BC
		public ObiCollisionMaterial()
		{
		}

		// Token: 0x170002BC RID: 700
		// (get) Token: 0x0600179C RID: 6044 RVA: 0x00086DE9 File Offset: 0x000851E9
		public IntPtr OniCollisionMaterial
		{
			get
			{
				return this.oniCollisionMaterial;
			}
		}

		// Token: 0x0600179D RID: 6045 RVA: 0x00086DF1 File Offset: 0x000851F1
		public void OnEnable()
		{
			this.oniCollisionMaterial = Oni.CreateCollisionMaterial();
			this.OnValidate();
		}

		// Token: 0x0600179E RID: 6046 RVA: 0x00086E04 File Offset: 0x00085204
		public void OnDisable()
		{
			Oni.DestroyCollisionMaterial(this.oniCollisionMaterial);
			this.oniCollisionMaterial = IntPtr.Zero;
		}

		// Token: 0x0600179F RID: 6047 RVA: 0x00086E1C File Offset: 0x0008521C
		public void OnValidate()
		{
			this.adaptor.friction = this.friction;
			this.adaptor.stickiness = this.stickiness;
			this.adaptor.stickDistance = this.stickDistance;
			this.adaptor.frictionCombine = this.frictionCombine;
			this.adaptor.stickinessCombine = this.stickinessCombine;
			Oni.UpdateCollisionMaterial(this.oniCollisionMaterial, ref this.adaptor);
		}

		// Token: 0x0400136A RID: 4970
		private IntPtr oniCollisionMaterial = IntPtr.Zero;

		// Token: 0x0400136B RID: 4971
		private Oni.CollisionMaterial adaptor = default(Oni.CollisionMaterial);

		// Token: 0x0400136C RID: 4972
		public float friction;

		// Token: 0x0400136D RID: 4973
		public float stickiness;

		// Token: 0x0400136E RID: 4974
		public float stickDistance;

		// Token: 0x0400136F RID: 4975
		public Oni.MaterialCombineMode frictionCombine;

		// Token: 0x04001370 RID: 4976
		public Oni.MaterialCombineMode stickinessCombine;
	}
}
