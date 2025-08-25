using System;
using UnityEngine;

namespace Battlehub.RTGizmos
{
	// Token: 0x020000E9 RID: 233
	public abstract class SphereGizmo : BaseGizmo
	{
		// Token: 0x060004D9 RID: 1241 RVA: 0x0001874D File Offset: 0x00016B4D
		protected SphereGizmo()
		{
		}

		// Token: 0x170000D0 RID: 208
		// (get) Token: 0x060004DA RID: 1242
		// (set) Token: 0x060004DB RID: 1243
		protected abstract Vector3 Center { get; set; }

		// Token: 0x170000D1 RID: 209
		// (get) Token: 0x060004DC RID: 1244
		// (set) Token: 0x060004DD RID: 1245
		protected abstract float Radius { get; set; }

		// Token: 0x170000D2 RID: 210
		// (get) Token: 0x060004DE RID: 1246 RVA: 0x00018755 File Offset: 0x00016B55
		protected override Matrix4x4 HandlesTransform
		{
			get
			{
				return Matrix4x4.TRS(this.Target.TransformPoint(this.Center), this.Target.rotation, this.Target.localScale * this.Radius);
			}
		}

		// Token: 0x060004DF RID: 1247 RVA: 0x00018790 File Offset: 0x00016B90
		protected override bool OnDrag(int index, Vector3 offset)
		{
			this.Radius += offset.magnitude * (float)Math.Sign(Vector3.Dot(offset, this.HandlesNormals[index]));
			if (this.Radius < 0f)
			{
				this.Radius = 0f;
				return false;
			}
			return true;
		}

		// Token: 0x060004E0 RID: 1248 RVA: 0x000187F0 File Offset: 0x00016BF0
		protected override void DrawOverride()
		{
			base.DrawOverride();
			if (this.Target == null)
			{
				return;
			}
			Vector3 vector = this.Target.localScale * this.Radius;
			RuntimeGizmos.DrawCubeHandles(this.Target.TransformPoint(this.Center), this.Target.rotation, vector, this.HandlesColor);
			RuntimeGizmos.DrawWireSphereGL(this.Target.TransformPoint(this.Center), this.Target.rotation, vector, this.LineColor);
			if (base.IsDragging)
			{
				RuntimeGizmos.DrawSelection(this.Target.TransformPoint(this.Center + Vector3.Scale(this.HandlesPositions[base.DragIndex], vector)), this.Target.rotation, vector, this.SelectionColor);
			}
		}
	}
}
