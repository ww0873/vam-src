using System;
using UnityEngine;

namespace GPUTools.Hair.Scripts.Geometry.Create
{
	// Token: 0x020009F0 RID: 2544
	[Serializable]
	public class GeometryBrush
	{
		// Token: 0x06004008 RID: 16392 RVA: 0x00130DB8 File Offset: 0x0012F1B8
		public GeometryBrush()
		{
		}

		// Token: 0x06004009 RID: 16393 RVA: 0x00130E0D File Offset: 0x0012F20D
		public Vector3 ToWorld(Matrix4x4 m, Vector3 local)
		{
			return this.Position + m * local;
		}

		// Token: 0x0600400A RID: 16394 RVA: 0x00130E2C File Offset: 0x0012F22C
		public bool Contains(Vector3 point)
		{
			Camera current = Camera.current;
			Vector3 v = current.transform.InverseTransformPoint(point);
			Vector3 v2 = current.transform.InverseTransformPoint(this.Position);
			bool flag = (v - v2).magnitude < this.Radius;
			bool flag2 = v2.z - v.z > -this.Lenght1;
			bool flag3 = v2.z - v.z < this.Lenght2;
			return flag && flag2 && flag3;
		}

		// Token: 0x1700077B RID: 1915
		// (get) Token: 0x0600400C RID: 16396 RVA: 0x00130EDB File Offset: 0x0012F2DB
		// (set) Token: 0x0600400B RID: 16395 RVA: 0x00130EC6 File Offset: 0x0012F2C6
		public Vector3 Position
		{
			get
			{
				return this.position;
			}
			set
			{
				this.OldPosition = this.position;
				this.position = value;
			}
		}

		// Token: 0x1700077C RID: 1916
		// (get) Token: 0x0600400D RID: 16397 RVA: 0x00130EE3 File Offset: 0x0012F2E3
		public Vector3 Speed
		{
			get
			{
				return this.position - this.OldPosition;
			}
		}

		// Token: 0x04003058 RID: 12376
		public Vector3 Dirrection;

		// Token: 0x04003059 RID: 12377
		public float Radius = 0.01f;

		// Token: 0x0400305A RID: 12378
		public float Lenght1 = 1f;

		// Token: 0x0400305B RID: 12379
		public float Lenght2 = 1f;

		// Token: 0x0400305C RID: 12380
		public float Strength = 0.25f;

		// Token: 0x0400305D RID: 12381
		public Color Color = Color.white;

		// Token: 0x0400305E RID: 12382
		public float CollisionDistance = 0.01f;

		// Token: 0x0400305F RID: 12383
		public GeometryBrushBehaviour Behaviour;

		// Token: 0x04003060 RID: 12384
		[NonSerialized]
		public bool IsBrushEnabled;

		// Token: 0x04003061 RID: 12385
		private Vector3 position;

		// Token: 0x04003062 RID: 12386
		public Vector3 OldPosition;
	}
}
