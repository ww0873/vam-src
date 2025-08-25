using System;
using UnityEngine;

namespace Battlehub.RTGizmos
{
	// Token: 0x020000DF RID: 223
	public abstract class BoxGizmo : BaseGizmo
	{
		// Token: 0x06000477 RID: 1143 RVA: 0x00018DC1 File Offset: 0x000171C1
		protected BoxGizmo()
		{
		}

		// Token: 0x170000B9 RID: 185
		// (get) Token: 0x06000478 RID: 1144
		// (set) Token: 0x06000479 RID: 1145
		protected abstract Bounds Bounds { get; set; }

		// Token: 0x170000BA RID: 186
		// (get) Token: 0x0600047A RID: 1146 RVA: 0x00018DCC File Offset: 0x000171CC
		protected override Matrix4x4 HandlesTransform
		{
			get
			{
				return Matrix4x4.TRS(this.Target.TransformPoint(this.Bounds.center), this.Target.rotation, Vector3.Scale(this.Bounds.extents, this.Target.localScale));
			}
		}

		// Token: 0x0600047B RID: 1147 RVA: 0x00018E20 File Offset: 0x00017220
		protected override bool OnDrag(int index, Vector3 offset)
		{
			Bounds bounds = this.Bounds;
			bounds.center += offset / 2f;
			bounds.extents += Vector3.Scale(offset / 2f, this.HandlesPositions[index]);
			this.Bounds = bounds;
			return true;
		}

		// Token: 0x0600047C RID: 1148 RVA: 0x00018E8C File Offset: 0x0001728C
		protected override void DrawOverride()
		{
			base.DrawOverride();
			Bounds bounds = this.Bounds;
			Vector3 vector = Vector3.Scale(bounds.extents, this.Target.localScale);
			RuntimeGizmos.DrawCubeHandles(this.Target.TransformPoint(bounds.center), this.Target.rotation, vector, this.HandlesColor);
			RuntimeGizmos.DrawWireCubeGL(ref bounds, this.Target.TransformPoint(bounds.center), this.Target.rotation, this.Target.localScale, this.LineColor);
			if (base.IsDragging)
			{
				RuntimeGizmos.DrawSelection(this.Target.TransformPoint(bounds.center + Vector3.Scale(this.HandlesPositions[base.DragIndex], vector)), this.Target.rotation, vector, this.SelectionColor);
			}
		}
	}
}
