using System;
using UnityEngine;

namespace Obi
{
	// Token: 0x0200039A RID: 922
	public class ObiCircleShapeTracker2D : ObiShapeTracker
	{
		// Token: 0x0600175D RID: 5981 RVA: 0x0008594C File Offset: 0x00083D4C
		public ObiCircleShapeTracker2D(CircleCollider2D collider)
		{
			this.collider = collider;
			this.adaptor.is2D = true;
			this.oniShape = Oni.CreateShape(Oni.ShapeType.Sphere);
		}

		// Token: 0x0600175E RID: 5982 RVA: 0x00085974 File Offset: 0x00083D74
		public override void UpdateIfNeeded()
		{
			CircleCollider2D circleCollider2D = this.collider as CircleCollider2D;
			if (circleCollider2D != null && (circleCollider2D.radius != this.radius || circleCollider2D.offset != this.center))
			{
				this.radius = circleCollider2D.radius;
				this.center = circleCollider2D.offset;
				this.adaptor.Set(this.center, this.radius);
				Oni.UpdateShape(this.oniShape, ref this.adaptor);
			}
		}

		// Token: 0x04001340 RID: 4928
		private float radius;

		// Token: 0x04001341 RID: 4929
		private Vector2 center;
	}
}
