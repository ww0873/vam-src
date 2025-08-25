using System;
using UnityEngine;

namespace Obi
{
	// Token: 0x02000398 RID: 920
	public class ObiBoxShapeTracker2D : ObiShapeTracker
	{
		// Token: 0x06001759 RID: 5977 RVA: 0x00085759 File Offset: 0x00083B59
		public ObiBoxShapeTracker2D(BoxCollider2D collider)
		{
			this.collider = collider;
			this.adaptor.is2D = true;
			this.oniShape = Oni.CreateShape(Oni.ShapeType.Box);
		}

		// Token: 0x0600175A RID: 5978 RVA: 0x00085780 File Offset: 0x00083B80
		public override void UpdateIfNeeded()
		{
			BoxCollider2D boxCollider2D = this.collider as BoxCollider2D;
			if (boxCollider2D != null && (boxCollider2D.size != this.size || boxCollider2D.offset != this.center))
			{
				this.size = boxCollider2D.size;
				this.center = boxCollider2D.offset;
				this.adaptor.Set(this.center, this.size);
				Oni.UpdateShape(this.oniShape, ref this.adaptor);
			}
		}

		// Token: 0x0400133B RID: 4923
		private Vector2 size;

		// Token: 0x0400133C RID: 4924
		private Vector2 center;
	}
}
