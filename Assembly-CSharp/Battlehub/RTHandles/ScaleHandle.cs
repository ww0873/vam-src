using System;
using Battlehub.RTCommon;
using UnityEngine;

namespace Battlehub.RTHandles
{
	// Token: 0x02000108 RID: 264
	public class ScaleHandle : BaseHandle
	{
		// Token: 0x06000658 RID: 1624 RVA: 0x00028E2F File Offset: 0x0002722F
		public ScaleHandle()
		{
		}

		// Token: 0x17000104 RID: 260
		// (get) Token: 0x06000659 RID: 1625 RVA: 0x00028E42 File Offset: 0x00027242
		protected override RuntimeTool Tool
		{
			get
			{
				return RuntimeTool.Scale;
			}
		}

		// Token: 0x17000105 RID: 261
		// (get) Token: 0x0600065A RID: 1626 RVA: 0x00028E45 File Offset: 0x00027245
		protected override float CurrentGridUnitSize
		{
			get
			{
				return this.GridSize;
			}
		}

		// Token: 0x0600065B RID: 1627 RVA: 0x00028E4D File Offset: 0x0002724D
		protected override void AwakeOverride()
		{
			this.m_scale = Vector3.one;
			this.m_roundedScale = this.m_scale;
		}

		// Token: 0x0600065C RID: 1628 RVA: 0x00028E66 File Offset: 0x00027266
		protected override void OnDestroyOverride()
		{
			base.OnDestroyOverride();
		}

		// Token: 0x0600065D RID: 1629 RVA: 0x00028E6E File Offset: 0x0002726E
		protected override void UpdateOverride()
		{
			base.UpdateOverride();
			if (this.HightlightOnHover && !base.IsDragging)
			{
				if (RuntimeTools.IsPointerOverGameObject())
				{
					return;
				}
				this.SelectedAxis = this.Hit();
			}
		}

		// Token: 0x0600065E RID: 1630 RVA: 0x00028EA4 File Offset: 0x000272A4
		private RuntimeHandleAxis Hit()
		{
			this.m_screenScale = RuntimeHandles.GetScreenScale(base.transform.position, this.SceneCamera) * 1f;
			this.m_matrix = Matrix4x4.TRS(base.transform.position, base.Rotation, Vector3.one);
			this.m_inverse = this.m_matrix.inverse;
			Matrix4x4 matrix = Matrix4x4.TRS(base.transform.position, base.Rotation, new Vector3(this.m_screenScale, this.m_screenScale, this.m_screenScale));
			if (this.HitCenter())
			{
				return RuntimeHandleAxis.Free;
			}
			float num;
			bool flag = this.HitAxis(Vector3.up, matrix, out num);
			float num2;
			flag |= this.HitAxis(Vector3.forward, matrix, out num2);
			float num3;
			flag |= this.HitAxis(Vector3.right, matrix, out num3);
			if (!flag)
			{
				return RuntimeHandleAxis.None;
			}
			if (num <= num2 && num <= num3)
			{
				return RuntimeHandleAxis.Y;
			}
			if (num3 <= num && num3 <= num2)
			{
				return RuntimeHandleAxis.X;
			}
			return RuntimeHandleAxis.Z;
		}

		// Token: 0x0600065F RID: 1631 RVA: 0x00028FA4 File Offset: 0x000273A4
		protected override bool OnBeginDrag()
		{
			this.SelectedAxis = this.Hit();
			if (this.SelectedAxis == RuntimeHandleAxis.Free)
			{
				base.DragPlane = this.GetDragPlane();
			}
			else if (this.SelectedAxis == RuntimeHandleAxis.None)
			{
				return false;
			}
			this.m_refScales = new Vector3[base.ActiveTargets.Length];
			for (int i = 0; i < this.m_refScales.Length; i++)
			{
				Quaternion rotation = (RuntimeTools.PivotRotation != RuntimePivotRotation.Global) ? Quaternion.identity : base.ActiveTargets[i].rotation;
				this.m_refScales[i] = rotation * base.ActiveTargets[i].localScale;
			}
			base.DragPlane = this.GetDragPlane();
			bool pointOnDragPlane = this.GetPointOnDragPlane(Input.mousePosition, out this.m_prevPoint);
			if (!pointOnDragPlane)
			{
				this.SelectedAxis = RuntimeHandleAxis.None;
			}
			return pointOnDragPlane;
		}

		// Token: 0x06000660 RID: 1632 RVA: 0x00029088 File Offset: 0x00027488
		protected override void OnDrag()
		{
			Vector3 vector;
			if (this.GetPointOnDragPlane(Input.mousePosition, out vector))
			{
				Vector3 vector2 = this.m_inverse.MultiplyVector((vector - this.m_prevPoint) / this.m_screenScale);
				float magnitude = vector2.magnitude;
				if (this.SelectedAxis == RuntimeHandleAxis.X)
				{
					vector2.y = (vector2.z = 0f);
					if (!base.LockObject.ScaleX)
					{
						this.m_scale.x = this.m_scale.x + Mathf.Sign(vector2.x) * magnitude;
					}
				}
				else if (this.SelectedAxis == RuntimeHandleAxis.Y)
				{
					vector2.x = (vector2.z = 0f);
					if (!base.LockObject.ScaleY)
					{
						this.m_scale.y = this.m_scale.y + Mathf.Sign(vector2.y) * magnitude;
					}
				}
				else if (this.SelectedAxis == RuntimeHandleAxis.Z)
				{
					vector2.x = (vector2.y = 0f);
					if (!base.LockObject.ScaleZ)
					{
						this.m_scale.z = this.m_scale.z + Mathf.Sign(vector2.z) * magnitude;
					}
				}
				if (this.SelectedAxis == RuntimeHandleAxis.Free)
				{
					float num = Mathf.Sign(vector2.x + vector2.y);
					if (!base.LockObject.ScaleX)
					{
						this.m_scale.x = this.m_scale.x + num * magnitude;
					}
					if (!base.LockObject.ScaleY)
					{
						this.m_scale.y = this.m_scale.y + num * magnitude;
					}
					if (!base.LockObject.ScaleZ)
					{
						this.m_scale.z = this.m_scale.z + num * magnitude;
					}
				}
				this.m_roundedScale = this.m_scale;
				if ((double)base.EffectiveGridUnitSize > 0.01)
				{
					this.m_roundedScale.x = (float)Mathf.RoundToInt(this.m_roundedScale.x / base.EffectiveGridUnitSize) * base.EffectiveGridUnitSize;
					this.m_roundedScale.y = (float)Mathf.RoundToInt(this.m_roundedScale.y / base.EffectiveGridUnitSize) * base.EffectiveGridUnitSize;
					this.m_roundedScale.z = (float)Mathf.RoundToInt(this.m_roundedScale.z / base.EffectiveGridUnitSize) * base.EffectiveGridUnitSize;
				}
				if (this.Model != null)
				{
					this.Model.SetScale(this.m_roundedScale);
				}
				for (int i = 0; i < this.m_refScales.Length; i++)
				{
					Quaternion rotation = (RuntimeTools.PivotRotation != RuntimePivotRotation.Global) ? Quaternion.identity : base.Targets[i].rotation;
					base.ActiveTargets[i].localScale = Quaternion.Inverse(rotation) * Vector3.Scale(this.m_refScales[i], this.m_roundedScale);
				}
				this.m_prevPoint = vector;
			}
		}

		// Token: 0x06000661 RID: 1633 RVA: 0x000293A8 File Offset: 0x000277A8
		protected override void OnDrop()
		{
			this.m_scale = Vector3.one;
			this.m_roundedScale = this.m_scale;
			if (this.Model != null)
			{
				this.Model.SetScale(this.m_roundedScale);
			}
		}

		// Token: 0x06000662 RID: 1634 RVA: 0x000293E3 File Offset: 0x000277E3
		protected override void DrawOverride()
		{
			RuntimeHandles.DoScaleHandle(this.m_roundedScale, base.Target.position, base.Rotation, this.SelectedAxis, base.LockObject);
		}

		// Token: 0x04000609 RID: 1545
		public float GridSize = 0.1f;

		// Token: 0x0400060A RID: 1546
		private Vector3 m_prevPoint;

		// Token: 0x0400060B RID: 1547
		private Matrix4x4 m_matrix;

		// Token: 0x0400060C RID: 1548
		private Matrix4x4 m_inverse;

		// Token: 0x0400060D RID: 1549
		private Vector3 m_roundedScale;

		// Token: 0x0400060E RID: 1550
		private Vector3 m_scale;

		// Token: 0x0400060F RID: 1551
		private Vector3[] m_refScales;

		// Token: 0x04000610 RID: 1552
		private float m_screenScale;
	}
}
