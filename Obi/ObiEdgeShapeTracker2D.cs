using System;
using System.Runtime.InteropServices;
using UnityEngine;

namespace Obi
{
	// Token: 0x0200039B RID: 923
	public class ObiEdgeShapeTracker2D : ObiShapeTracker
	{
		// Token: 0x0600175F RID: 5983 RVA: 0x00085A05 File Offset: 0x00083E05
		public ObiEdgeShapeTracker2D(EdgeCollider2D collider)
		{
			this.collider = collider;
			this.adaptor.is2D = true;
			this.oniShape = Oni.CreateShape(Oni.ShapeType.EdgeMesh);
			this.UpdateEdgeData();
		}

		// Token: 0x06001760 RID: 5984 RVA: 0x00085A34 File Offset: 0x00083E34
		public void UpdateEdgeData()
		{
			EdgeCollider2D edgeCollider2D = this.collider as EdgeCollider2D;
			if (edgeCollider2D != null)
			{
				Vector3[] array = new Vector3[edgeCollider2D.pointCount];
				int[] array2 = new int[edgeCollider2D.edgeCount * 2];
				Vector2[] points = edgeCollider2D.points;
				for (int i = 0; i < edgeCollider2D.pointCount; i++)
				{
					array[i] = points[i];
				}
				for (int j = 0; j < edgeCollider2D.edgeCount; j++)
				{
					array2[j * 2] = j;
					array2[j * 2 + 1] = j + 1;
				}
				Oni.UnpinMemory(this.pointsHandle);
				Oni.UnpinMemory(this.indicesHandle);
				this.pointsHandle = Oni.PinMemory(array);
				this.indicesHandle = Oni.PinMemory(array2);
				this.edgeDataHasChanged = true;
			}
		}

		// Token: 0x06001761 RID: 5985 RVA: 0x00085B18 File Offset: 0x00083F18
		public override void UpdateIfNeeded()
		{
			EdgeCollider2D edgeCollider2D = this.collider as EdgeCollider2D;
			if (edgeCollider2D != null && (edgeCollider2D.pointCount != this.pointCount || this.edgeDataHasChanged))
			{
				this.pointCount = edgeCollider2D.pointCount;
				this.edgeDataHasChanged = false;
				this.adaptor.Set(this.pointsHandle.AddrOfPinnedObject(), this.indicesHandle.AddrOfPinnedObject(), edgeCollider2D.pointCount, edgeCollider2D.edgeCount * 2);
				Oni.UpdateShape(this.oniShape, ref this.adaptor);
			}
		}

		// Token: 0x06001762 RID: 5986 RVA: 0x00085BAC File Offset: 0x00083FAC
		public override void Destroy()
		{
			base.Destroy();
			Oni.UnpinMemory(this.pointsHandle);
			Oni.UnpinMemory(this.indicesHandle);
		}

		// Token: 0x04001342 RID: 4930
		private int pointCount;

		// Token: 0x04001343 RID: 4931
		private GCHandle pointsHandle;

		// Token: 0x04001344 RID: 4932
		private GCHandle indicesHandle;

		// Token: 0x04001345 RID: 4933
		private bool edgeDataHasChanged;
	}
}
