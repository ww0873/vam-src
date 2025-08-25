using System;
using UnityEngine;

namespace Obi
{
	// Token: 0x020003A1 RID: 929
	public class ObiSphereShapeTracker : ObiShapeTracker
	{
		// Token: 0x06001778 RID: 6008 RVA: 0x000860C3 File Offset: 0x000844C3
		public ObiSphereShapeTracker(SphereCollider collider)
		{
			this.collider = collider;
			this.adaptor.is2D = false;
			this.oniShape = Oni.CreateShape(Oni.ShapeType.Sphere);
		}

		// Token: 0x06001779 RID: 6009 RVA: 0x000860EC File Offset: 0x000844EC
		public override void UpdateIfNeeded()
		{
			SphereCollider sphereCollider = this.collider as SphereCollider;
			if (sphereCollider != null && (sphereCollider.radius != this.radius || sphereCollider.center != this.center))
			{
				this.radius = sphereCollider.radius;
				this.center = sphereCollider.center;
				this.adaptor.Set(this.center, this.radius);
				Oni.UpdateShape(this.oniShape, ref this.adaptor);
			}
		}

		// Token: 0x04001355 RID: 4949
		private float radius;

		// Token: 0x04001356 RID: 4950
		private Vector3 center;
	}
}
