using System;
using UnityEngine;

namespace Battlehub.RTGizmos
{
	// Token: 0x020000E2 RID: 226
	public abstract class ConeGizmo : BaseGizmo
	{
		// Token: 0x06000499 RID: 1177 RVA: 0x00019856 File Offset: 0x00017C56
		protected ConeGizmo()
		{
		}

		// Token: 0x170000C4 RID: 196
		// (get) Token: 0x0600049A RID: 1178
		// (set) Token: 0x0600049B RID: 1179
		protected abstract float Radius { get; set; }

		// Token: 0x170000C5 RID: 197
		// (get) Token: 0x0600049C RID: 1180
		// (set) Token: 0x0600049D RID: 1181
		protected abstract float Height { get; set; }

		// Token: 0x170000C6 RID: 198
		// (get) Token: 0x0600049E RID: 1182 RVA: 0x0001985E File Offset: 0x00017C5E
		private Vector3 Scale
		{
			get
			{
				return new Vector3(Mathf.Max(Mathf.Abs(this.Radius), 0.001f), Mathf.Max(Mathf.Abs(this.Radius), 0.001f), 1f);
			}
		}

		// Token: 0x170000C7 RID: 199
		// (get) Token: 0x0600049F RID: 1183 RVA: 0x00019894 File Offset: 0x00017C94
		protected override Matrix4x4 HandlesTransform
		{
			get
			{
				return Matrix4x4.TRS(this.Target.TransformPoint(Vector3.forward * this.Height), this.Target.rotation, this.Scale);
			}
		}

		// Token: 0x170000C8 RID: 200
		// (get) Token: 0x060004A0 RID: 1184 RVA: 0x000198C7 File Offset: 0x00017CC7
		protected override Vector3[] HandlesPositions
		{
			get
			{
				return this.m_coneHandlesPositions;
			}
		}

		// Token: 0x170000C9 RID: 201
		// (get) Token: 0x060004A1 RID: 1185 RVA: 0x000198CF File Offset: 0x00017CCF
		protected override Vector3[] HandlesNormals
		{
			get
			{
				return this.m_coneHandesNormals;
			}
		}

		// Token: 0x060004A2 RID: 1186 RVA: 0x000198D7 File Offset: 0x00017CD7
		protected override void AwakeOverride()
		{
			base.AwakeOverride();
			this.m_coneHandlesPositions = RuntimeGizmos.GetConeHandlesPositions();
			this.m_coneHandesNormals = RuntimeGizmos.GetConeHandlesNormals();
		}

		// Token: 0x060004A3 RID: 1187 RVA: 0x000198F8 File Offset: 0x00017CF8
		protected override bool OnDrag(int index, Vector3 offset)
		{
			float num = (float)Math.Sign(Vector3.Dot(offset.normalized, this.HandlesNormals[index]));
			if (index == 0)
			{
				this.Height += offset.magnitude * num;
			}
			else
			{
				this.Radius += offset.magnitude * num;
			}
			return true;
		}

		// Token: 0x060004A4 RID: 1188 RVA: 0x00019961 File Offset: 0x00017D61
		protected override bool HitOverride(int index, Vector3 vertex, Vector3 normal)
		{
			return index != 0 || Mathf.Abs(this.Radius) >= 0.0001f;
		}

		// Token: 0x060004A5 RID: 1189 RVA: 0x00019984 File Offset: 0x00017D84
		protected override void DrawOverride()
		{
			base.DrawOverride();
			RuntimeGizmos.DrawConeHandles(this.Target.TransformPoint(Vector3.forward * this.Height), this.Target.rotation, this.Scale, this.HandlesColor);
			RuntimeGizmos.DrawWireConeGL(this.Height, this.Radius, this.Target.position, this.Target.rotation, Vector3.one, this.LineColor);
			if (base.IsDragging)
			{
				RuntimeGizmos.DrawSelection(this.Target.TransformPoint(Vector3.forward * this.Height + Vector3.Scale(this.HandlesPositions[base.DragIndex], this.Scale)), this.Target.rotation, this.Scale, this.SelectionColor);
			}
		}

		// Token: 0x04000475 RID: 1141
		private Vector3[] m_coneHandlesPositions;

		// Token: 0x04000476 RID: 1142
		private Vector3[] m_coneHandesNormals;
	}
}
