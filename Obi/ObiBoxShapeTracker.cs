using System;
using UnityEngine;

namespace Obi
{
	// Token: 0x0200039C RID: 924
	public class ObiBoxShapeTracker : ObiShapeTracker
	{
		// Token: 0x06001763 RID: 5987 RVA: 0x00085BCA File Offset: 0x00083FCA
		public ObiBoxShapeTracker(BoxCollider collider)
		{
			this.collider = collider;
			this.adaptor.is2D = false;
			this.oniShape = Oni.CreateShape(Oni.ShapeType.Box);
		}

		// Token: 0x06001764 RID: 5988 RVA: 0x00085BF4 File Offset: 0x00083FF4
		public override void UpdateIfNeeded()
		{
			BoxCollider boxCollider = this.collider as BoxCollider;
			if (boxCollider != null && (boxCollider.size != this.size || boxCollider.center != this.center))
			{
				this.size = boxCollider.size;
				this.center = boxCollider.center;
				this.adaptor.Set(this.center, this.size);
				Oni.UpdateShape(this.oniShape, ref this.adaptor);
			}
		}

		// Token: 0x04001346 RID: 4934
		private Vector3 size;

		// Token: 0x04001347 RID: 4935
		private Vector3 center;
	}
}
