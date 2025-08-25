using System;
using UnityEngine;

namespace Obi
{
	// Token: 0x0200039D RID: 925
	public class ObiCapsuleShapeTracker : ObiShapeTracker
	{
		// Token: 0x06001765 RID: 5989 RVA: 0x00085C85 File Offset: 0x00084085
		public ObiCapsuleShapeTracker(CapsuleCollider collider)
		{
			this.collider = collider;
			this.adaptor.is2D = false;
			this.oniShape = Oni.CreateShape(Oni.ShapeType.Capsule);
		}

		// Token: 0x06001766 RID: 5990 RVA: 0x00085CAC File Offset: 0x000840AC
		public ObiCapsuleShapeTracker(CharacterController collider)
		{
			this.collider = collider;
			this.adaptor.is2D = false;
			this.oniShape = Oni.CreateShape(Oni.ShapeType.Capsule);
		}

		// Token: 0x06001767 RID: 5991 RVA: 0x00085CD4 File Offset: 0x000840D4
		public override void UpdateIfNeeded()
		{
			CapsuleCollider capsuleCollider = this.collider as CapsuleCollider;
			if (capsuleCollider != null && (capsuleCollider.radius != this.radius || capsuleCollider.height != this.height || capsuleCollider.direction != this.direction || capsuleCollider.center != this.center))
			{
				this.radius = capsuleCollider.radius;
				this.height = capsuleCollider.height;
				this.direction = capsuleCollider.direction;
				this.center = capsuleCollider.center;
				this.adaptor.Set(this.center, this.radius, this.height, this.direction);
				Oni.UpdateShape(this.oniShape, ref this.adaptor);
			}
			CharacterController characterController = this.collider as CharacterController;
			if (characterController != null && (characterController.radius != this.radius || characterController.height != this.height || characterController.center != this.center))
			{
				this.radius = characterController.radius;
				this.height = characterController.height;
				this.center = characterController.center;
				this.adaptor.Set(this.center, this.radius, this.height, 1);
				Oni.UpdateShape(this.oniShape, ref this.adaptor);
			}
		}

		// Token: 0x04001348 RID: 4936
		private int direction;

		// Token: 0x04001349 RID: 4937
		private float radius;

		// Token: 0x0400134A RID: 4938
		private float height;

		// Token: 0x0400134B RID: 4939
		private Vector3 center;
	}
}
