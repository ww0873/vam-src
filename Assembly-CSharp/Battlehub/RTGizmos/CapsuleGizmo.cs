using System;
using UnityEngine;

namespace Battlehub.RTGizmos
{
	// Token: 0x020000E1 RID: 225
	public abstract class CapsuleGizmo : BaseGizmo
	{
		// Token: 0x06000488 RID: 1160 RVA: 0x000190FF File Offset: 0x000174FF
		protected CapsuleGizmo()
		{
		}

		// Token: 0x170000BF RID: 191
		// (get) Token: 0x06000489 RID: 1161
		// (set) Token: 0x0600048A RID: 1162
		protected abstract Vector3 Center { get; set; }

		// Token: 0x170000C0 RID: 192
		// (get) Token: 0x0600048B RID: 1163
		// (set) Token: 0x0600048C RID: 1164
		protected abstract float Radius { get; set; }

		// Token: 0x170000C1 RID: 193
		// (get) Token: 0x0600048D RID: 1165
		// (set) Token: 0x0600048E RID: 1166
		protected abstract float Height { get; set; }

		// Token: 0x170000C2 RID: 194
		// (get) Token: 0x0600048F RID: 1167
		// (set) Token: 0x06000490 RID: 1168
		protected abstract int Direction { get; set; }

		// Token: 0x170000C3 RID: 195
		// (get) Token: 0x06000491 RID: 1169 RVA: 0x00019107 File Offset: 0x00017507
		protected override Matrix4x4 HandlesTransform
		{
			get
			{
				return Matrix4x4.TRS(this.Target.TransformPoint(this.Center), this.Target.rotation, this.GetHandlesScale());
			}
		}

		// Token: 0x06000492 RID: 1170 RVA: 0x00019130 File Offset: 0x00017530
		protected override bool OnDrag(int index, Vector3 offset)
		{
			Vector3 rhs;
			if (this.Direction == 0)
			{
				rhs = Vector3.right;
			}
			else if (this.Direction == 1)
			{
				rhs = Vector3.up;
			}
			else
			{
				rhs = Vector3.forward;
			}
			if (Mathf.Abs(Vector3.Dot(offset.normalized, rhs)) > 0.99f)
			{
				float num = (float)Math.Sign(Vector3.Dot(offset.normalized, this.HandlesNormals[index]));
				this.Height += 2f * offset.magnitude * num;
				if (this.Height < 0f)
				{
					this.Height = 0f;
					return false;
				}
			}
			else
			{
				float maxHorizontalScale = this.GetMaxHorizontalScale();
				this.Radius += Vector3.Scale(offset, this.Target.localScale).magnitude / maxHorizontalScale * Mathf.Sign(Vector3.Dot(offset, this.HandlesNormals[index]));
				if (this.Radius < 0f)
				{
					this.Radius = 0f;
					return false;
				}
			}
			return true;
		}

		// Token: 0x06000493 RID: 1171 RVA: 0x00019258 File Offset: 0x00017658
		protected override void DrawOverride()
		{
			base.DrawOverride();
			float maxHorizontalScale = this.GetMaxHorizontalScale();
			Vector3 handlesScale = this.GetHandlesScale();
			RuntimeGizmos.DrawCubeHandles(this.Target.TransformPoint(this.Center), this.Target.rotation, this.GetHandlesScale(), this.HandlesColor);
			RuntimeGizmos.DrawWireCapsuleGL(this.Direction, this.GetHeight(), this.Radius, this.Target.TransformPoint(this.Center), this.Target.rotation, new Vector3(maxHorizontalScale, maxHorizontalScale, maxHorizontalScale), this.LineColor);
			if (base.IsDragging)
			{
				RuntimeGizmos.DrawSelection(this.Target.TransformPoint(this.Center + Vector3.Scale(this.HandlesPositions[base.DragIndex], handlesScale)), this.Target.rotation, handlesScale, this.SelectionColor);
			}
		}

		// Token: 0x06000494 RID: 1172 RVA: 0x0001933C File Offset: 0x0001773C
		private float GetHeight()
		{
			float maxHorizontalScale = this.GetMaxHorizontalScale();
			float num;
			if (this.Direction == 0)
			{
				num = this.Target.localScale.x;
			}
			else if (this.Direction == 1)
			{
				num = this.Target.localScale.y;
			}
			else
			{
				num = this.Target.localScale.z;
			}
			return this.Height * num / maxHorizontalScale;
		}

		// Token: 0x06000495 RID: 1173 RVA: 0x000193B8 File Offset: 0x000177B8
		private Vector3 GetHandlesScale()
		{
			float maxHorizontalScale = this.GetMaxHorizontalScale();
			float num;
			float num2;
			float num3;
			if (this.Direction == 0)
			{
				num = this.GetHandlesHeight();
				num2 = maxHorizontalScale * this.Radius;
				num3 = maxHorizontalScale * this.Radius;
			}
			else if (this.Direction == 1)
			{
				num = maxHorizontalScale * this.Radius;
				num2 = this.GetHandlesHeight();
				num3 = maxHorizontalScale * this.Radius;
			}
			else
			{
				num = maxHorizontalScale * this.Radius;
				num2 = maxHorizontalScale * this.Radius;
				num3 = this.GetHandlesHeight();
			}
			if (num < 0.001f && num > -0.001f)
			{
				num = 0.001f;
			}
			if (num2 < 0.001f && num2 > -0.001f)
			{
				num2 = 0.001f;
			}
			if (num3 < 0.001f && num3 > -0.001f)
			{
				num3 = 0.001f;
			}
			return new Vector3(num, num2, num3);
		}

		// Token: 0x06000496 RID: 1174 RVA: 0x00019494 File Offset: 0x00017894
		private float GetHandlesHeight()
		{
			if (this.Direction == 0)
			{
				return this.MaxAbs(this.Target.localScale.x * this.Height / 2f, this.Radius * this.GetMaxHorizontalScale());
			}
			if (this.Direction == 1)
			{
				return this.MaxAbs(this.Target.localScale.y * this.Height / 2f, this.Radius * this.GetMaxHorizontalScale());
			}
			return this.MaxAbs(this.Target.localScale.z * this.Height / 2f, this.Radius * this.GetMaxHorizontalScale());
		}

		// Token: 0x06000497 RID: 1175 RVA: 0x00019554 File Offset: 0x00017954
		private float GetMaxHorizontalScale()
		{
			if (this.Direction == 0)
			{
				return this.MaxAbs(this.Target.localScale.y, this.Target.localScale.z);
			}
			if (this.Direction == 1)
			{
				return this.MaxAbs(this.Target.localScale.x, this.Target.localScale.z);
			}
			return this.MaxAbs(this.Target.localScale.x, this.Target.localScale.y);
		}

		// Token: 0x06000498 RID: 1176 RVA: 0x00019600 File Offset: 0x00017A00
		private float MaxAbs(float a, float b)
		{
			if (Math.Abs(a) > Math.Abs(b))
			{
				return a;
			}
			return b;
		}
	}
}
