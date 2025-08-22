using System;
using UnityEngine;

namespace Obi
{
	// Token: 0x02000399 RID: 921
	public class ObiCapsuleShapeTracker2D : ObiShapeTracker
	{
		// Token: 0x0600175B RID: 5979 RVA: 0x0008581B File Offset: 0x00083C1B
		public ObiCapsuleShapeTracker2D(CapsuleCollider2D collider)
		{
			this.collider = collider;
			this.adaptor.is2D = true;
			this.oniShape = Oni.CreateShape(Oni.ShapeType.Capsule);
		}

		// Token: 0x0600175C RID: 5980 RVA: 0x00085844 File Offset: 0x00083C44
		public override void UpdateIfNeeded()
		{
			CapsuleCollider2D capsuleCollider2D = this.collider as CapsuleCollider2D;
			if (capsuleCollider2D != null && (capsuleCollider2D.size != this.size || capsuleCollider2D.direction != this.direction || capsuleCollider2D.offset != this.center))
			{
				this.size = capsuleCollider2D.size;
				this.direction = capsuleCollider2D.direction;
				this.center = capsuleCollider2D.offset;
				this.adaptor.Set(this.center, ((capsuleCollider2D.direction != CapsuleDirection2D.Horizontal) ? this.size.x : this.size.y) * 0.5f, Mathf.Max(this.size.x, this.size.y), (capsuleCollider2D.direction != CapsuleDirection2D.Horizontal) ? 1 : 0);
				Oni.UpdateShape(this.oniShape, ref this.adaptor);
			}
		}

		// Token: 0x0400133D RID: 4925
		private CapsuleDirection2D direction;

		// Token: 0x0400133E RID: 4926
		private Vector2 size;

		// Token: 0x0400133F RID: 4927
		private Vector2 center;
	}
}
