using System;
using Battlehub.RTCommon;
using UnityEngine;

namespace Battlehub.RTHandles
{
	// Token: 0x020000FE RID: 254
	public class RotationHandle : BaseHandle
	{
		// Token: 0x060005E9 RID: 1513 RVA: 0x000232AC File Offset: 0x000216AC
		public RotationHandle()
		{
		}

		// Token: 0x170000F9 RID: 249
		// (get) Token: 0x060005EA RID: 1514 RVA: 0x00023317 File Offset: 0x00021717
		protected override RuntimeTool Tool
		{
			get
			{
				return RuntimeTool.Rotate;
			}
		}

		// Token: 0x170000FA RID: 250
		// (get) Token: 0x060005EB RID: 1515 RVA: 0x0002331A File Offset: 0x0002171A
		private Quaternion StartingRotation
		{
			get
			{
				return (RuntimeTools.PivotRotation != RuntimePivotRotation.Global) ? Quaternion.identity : this.m_startingRotation;
			}
		}

		// Token: 0x170000FB RID: 251
		// (get) Token: 0x060005EC RID: 1516 RVA: 0x00023337 File Offset: 0x00021737
		private Quaternion StartingRotationInv
		{
			get
			{
				return (RuntimeTools.PivotRotation != RuntimePivotRotation.Global) ? Quaternion.identity : this.m_startinRotationInv;
			}
		}

		// Token: 0x170000FC RID: 252
		// (get) Token: 0x060005ED RID: 1517 RVA: 0x00023354 File Offset: 0x00021754
		protected override float CurrentGridUnitSize
		{
			get
			{
				return this.GridSize;
			}
		}

		// Token: 0x060005EE RID: 1518 RVA: 0x0002335C File Offset: 0x0002175C
		protected override void AwakeOverride()
		{
			RuntimeTools.PivotRotationChanged += this.OnPivotRotationChanged;
		}

		// Token: 0x060005EF RID: 1519 RVA: 0x0002336F File Offset: 0x0002176F
		protected override void OnDestroyOverride()
		{
			base.OnDestroyOverride();
			RuntimeTools.PivotRotationChanged -= this.OnPivotRotationChanged;
		}

		// Token: 0x060005F0 RID: 1520 RVA: 0x00023388 File Offset: 0x00021788
		protected override void StartOverride()
		{
			base.StartOverride();
			this.OnPivotRotationChanged();
		}

		// Token: 0x060005F1 RID: 1521 RVA: 0x00023396 File Offset: 0x00021796
		protected override void OnEnableOverride()
		{
			base.OnEnableOverride();
			this.OnPivotRotationChanged();
		}

		// Token: 0x060005F2 RID: 1522 RVA: 0x000233A4 File Offset: 0x000217A4
		protected override void UpdateOverride()
		{
			base.UpdateOverride();
			this.lastMousePosition = this.currentMousePosition;
			this.currentMousePosition = Input.mousePosition;
			if (RuntimeTools.IsPointerOverGameObject())
			{
				return;
			}
			if (!base.IsDragging)
			{
				if (this.HightlightOnHover)
				{
					this.m_targetInverseMatrix = Matrix4x4.TRS(base.Target.position, base.Target.rotation * this.StartingRotationInv, Vector3.one).inverse;
					this.SelectedAxis = this.Hit();
				}
				if (this.m_targetRotation != base.Target.rotation)
				{
					this.m_startingRotation = base.Target.rotation;
					this.m_startinRotationInv = Quaternion.Inverse(this.m_startingRotation);
					this.m_targetRotation = base.Target.rotation;
				}
			}
		}

		// Token: 0x060005F3 RID: 1523 RVA: 0x00023484 File Offset: 0x00021884
		private void OnPivotRotationChanged()
		{
			if (base.Target != null)
			{
				this.m_startingRotation = base.Target.rotation;
				this.m_startinRotationInv = Quaternion.Inverse(base.Target.rotation);
				this.m_targetRotation = base.Target.rotation;
			}
		}

		// Token: 0x060005F4 RID: 1524 RVA: 0x000234DC File Offset: 0x000218DC
		private bool Intersect(Ray r, Vector3 sphereCenter, float sphereRadius, out float hit1Distance, out float hit2Distance)
		{
			hit1Distance = 0f;
			hit2Distance = 0f;
			Vector3 vector = sphereCenter - r.origin;
			float num = Vector3.Dot(vector, r.direction);
			if ((double)num < 0.0)
			{
				return false;
			}
			float num2 = Vector3.Dot(vector, vector) - num * num;
			float num3 = sphereRadius * sphereRadius;
			if (num2 > num3)
			{
				return false;
			}
			float num4 = Mathf.Sqrt(num3 - num2);
			hit1Distance = num - num4;
			hit2Distance = num + num4;
			return true;
		}

		// Token: 0x060005F5 RID: 1525 RVA: 0x0002355C File Offset: 0x0002195C
		private RuntimeHandleAxis Hit()
		{
			Ray r = this.SceneCamera.ScreenPointToRay(Input.mousePosition);
			float num = RuntimeHandles.GetScreenScale(base.Target.position, this.SceneCamera) * 1f;
			float num2;
			float num3;
			if (!this.Intersect(r, base.Target.position, 1.2f * num, out num2, out num3))
			{
				return RuntimeHandleAxis.None;
			}
			Vector3 a;
			this.GetPointOnDragPlane(this.GetDragPlane(), Input.mousePosition, out a);
			RuntimeHandleAxis runtimeHandleAxis = this.HitAxis();
			if (runtimeHandleAxis != RuntimeHandleAxis.None)
			{
				return runtimeHandleAxis;
			}
			bool flag = (a - base.Target.position).magnitude <= 1f * num;
			if (flag)
			{
				return RuntimeHandleAxis.None;
			}
			return RuntimeHandleAxis.Screen;
		}

		// Token: 0x060005F6 RID: 1526 RVA: 0x00023614 File Offset: 0x00021A14
		private RuntimeHandleAxis HitAxis()
		{
			float num = RuntimeHandles.GetScreenScale(base.Target.position, this.SceneCamera) * 1f;
			Vector3 s = new Vector3(num, num, num);
			Matrix4x4 transform = Matrix4x4.TRS(Vector3.zero, base.Target.rotation * this.StartingRotationInv * Quaternion.AngleAxis(-90f, Vector3.up), Vector3.one);
			Matrix4x4 transform2 = Matrix4x4.TRS(Vector3.zero, base.Target.rotation * this.StartingRotationInv * Quaternion.AngleAxis(-90f, Vector3.right), Vector3.one);
			Matrix4x4 transform3 = Matrix4x4.TRS(Vector3.zero, base.Target.rotation * this.StartingRotationInv, Vector3.one);
			Matrix4x4 objToWorld = Matrix4x4.TRS(base.Target.position, Quaternion.identity, s);
			float num2;
			bool flag = this.HitAxis(transform, objToWorld, out num2);
			float num3;
			bool flag2 = this.HitAxis(transform2, objToWorld, out num3);
			float num4;
			bool flag3 = this.HitAxis(transform3, objToWorld, out num4);
			if (flag && num2 < num3 && num2 < num4)
			{
				return RuntimeHandleAxis.X;
			}
			if (flag2 && num3 < num2 && num3 < num4)
			{
				return RuntimeHandleAxis.Y;
			}
			if (flag3 && num4 < num2 && num4 < num3)
			{
				return RuntimeHandleAxis.Z;
			}
			return RuntimeHandleAxis.None;
		}

		// Token: 0x060005F7 RID: 1527 RVA: 0x00023778 File Offset: 0x00021B78
		private bool HitAxis(Matrix4x4 transform, Matrix4x4 objToWorld, out float minDistance)
		{
			bool result = false;
			minDistance = float.PositiveInfinity;
			float num = 0f;
			float z = 0f;
			Vector3 point = transform.MultiplyPoint(Vector3.zero);
			point = objToWorld.MultiplyPoint(point);
			point = this.SceneCamera.worldToCameraMatrix.MultiplyPoint(point);
			Vector3 vector = transform.MultiplyPoint(new Vector3(1f, 0f, z));
			vector = objToWorld.MultiplyPoint(vector);
			for (int i = 0; i < 32; i++)
			{
				num += 0.19634955f;
				float x = 1f * Mathf.Cos(num);
				float y = 1f * Mathf.Sin(num);
				Vector3 vector2 = transform.MultiplyPoint(new Vector3(x, y, z));
				vector2 = objToWorld.MultiplyPoint(vector2);
				if (this.SceneCamera.worldToCameraMatrix.MultiplyPoint(vector2).z > point.z)
				{
					Vector3 vector3 = this.SceneCamera.WorldToScreenPoint(vector2) - this.SceneCamera.WorldToScreenPoint(vector);
					float magnitude = vector3.magnitude;
					vector3.Normalize();
					float num2;
					if (vector3 != Vector3.zero && this.HitScreenAxis(out num2, this.SceneCamera.WorldToScreenPoint(vector), vector3, magnitude) && num2 < minDistance)
					{
						minDistance = num2;
						result = true;
					}
				}
				vector = vector2;
			}
			return result;
		}

		// Token: 0x060005F8 RID: 1528 RVA: 0x000238E8 File Offset: 0x00021CE8
		protected override bool OnBeginDrag()
		{
			this.m_targetRotation = base.Target.rotation;
			this.m_targetInverseMatrix = Matrix4x4.TRS(base.Target.position, base.Target.rotation * this.StartingRotationInv, Vector3.one).inverse;
			this.SelectedAxis = this.Hit();
			this.m_deltaX = 0f;
			this.m_deltaY = 0f;
			if (this.SelectedAxis == RuntimeHandleAxis.Screen)
			{
				Vector2 vector = this.SceneCamera.WorldToScreenPoint(base.Target.position);
				Vector2 vector2 = Input.mousePosition;
				float num = Mathf.Atan2(vector2.y - vector.y, vector2.x - vector.x);
				this.m_targetInverse = Quaternion.Inverse(Quaternion.AngleAxis(57.29578f * num, Vector3.forward));
				this.m_targetInverseMatrix = Matrix4x4.TRS(base.Target.position, base.Target.rotation, Vector3.one).inverse;
			}
			else
			{
				if (this.SelectedAxis == RuntimeHandleAxis.X)
				{
					this.m_startingRotationAxis = base.Target.rotation * Quaternion.Inverse(this.StartingRotation) * Vector3.right;
				}
				else if (this.SelectedAxis == RuntimeHandleAxis.Y)
				{
					this.m_startingRotationAxis = base.Target.rotation * Quaternion.Inverse(this.StartingRotation) * Vector3.up;
				}
				else if (this.SelectedAxis == RuntimeHandleAxis.Z)
				{
					this.m_startingRotationAxis = base.Target.rotation * Quaternion.Inverse(this.StartingRotation) * Vector3.forward;
				}
				this.m_targetInverse = Quaternion.Inverse(base.Target.rotation);
			}
			return this.SelectedAxis != RuntimeHandleAxis.None;
		}

		// Token: 0x060005F9 RID: 1529 RVA: 0x00023ADC File Offset: 0x00021EDC
		protected override void OnDrag()
		{
			float num = Input.GetAxis("Mouse X");
			float num2 = Input.GetAxis("Mouse Y");
			Vector3 vector = this.currentMousePosition - this.lastMousePosition;
			if (num == 0f && num2 == 0f && (vector.x != 0f || vector.y != 0f))
			{
				num = vector.x * 0.1f;
				num2 = vector.y * 0.1f;
			}
			num *= this.XSpeed;
			num2 *= this.YSpeed;
			this.m_deltaX += num;
			this.m_deltaY += num2;
			Vector3 point = this.StartingRotation * Quaternion.Inverse(base.Target.rotation) * this.SceneCamera.cameraToWorldMatrix.MultiplyVector(new Vector3(this.m_deltaY, -this.m_deltaX, 0f));
			Quaternion rhs = Quaternion.identity;
			if (this.SelectedAxis == RuntimeHandleAxis.X)
			{
				Vector3 axis = Quaternion.Inverse(base.Target.rotation) * this.m_startingRotationAxis;
				if (base.EffectiveGridUnitSize != 0f)
				{
					if (Mathf.Abs(point.x) >= base.EffectiveGridUnitSize)
					{
						point.x = Mathf.Sign(point.x) * base.EffectiveGridUnitSize;
						this.m_deltaX = 0f;
						this.m_deltaY = 0f;
					}
					else
					{
						point.x = 0f;
					}
				}
				if (base.LockObject != null && base.LockObject.RotationX)
				{
					point.x = 0f;
				}
				rhs = Quaternion.AngleAxis(point.x, axis);
			}
			else if (this.SelectedAxis == RuntimeHandleAxis.Y)
			{
				Vector3 axis2 = Quaternion.Inverse(base.Target.rotation) * this.m_startingRotationAxis;
				if (base.EffectiveGridUnitSize != 0f)
				{
					if (Mathf.Abs(point.y) >= base.EffectiveGridUnitSize)
					{
						point.y = Mathf.Sign(point.y) * base.EffectiveGridUnitSize;
						this.m_deltaX = 0f;
						this.m_deltaY = 0f;
					}
					else
					{
						point.y = 0f;
					}
				}
				if (base.LockObject != null && base.LockObject.RotationY)
				{
					point.y = 0f;
				}
				rhs = Quaternion.AngleAxis(point.y, axis2);
			}
			else if (this.SelectedAxis == RuntimeHandleAxis.Z)
			{
				Vector3 axis3 = Quaternion.Inverse(base.Target.rotation) * this.m_startingRotationAxis;
				if (base.EffectiveGridUnitSize != 0f)
				{
					if (Mathf.Abs(point.z) >= base.EffectiveGridUnitSize)
					{
						point.z = Mathf.Sign(point.z) * base.EffectiveGridUnitSize;
						this.m_deltaX = 0f;
						this.m_deltaY = 0f;
					}
					else
					{
						point.z = 0f;
					}
				}
				if (base.LockObject != null && base.LockObject.RotationZ)
				{
					point.z = 0f;
				}
				rhs = Quaternion.AngleAxis(point.z, axis3);
			}
			else if (this.SelectedAxis == RuntimeHandleAxis.Free)
			{
				point = this.StartingRotationInv * point;
				if (base.LockObject != null)
				{
					if (base.LockObject.RotationX)
					{
						point.x = 0f;
					}
					if (base.LockObject.RotationY)
					{
						point.y = 0f;
					}
					if (base.LockObject.RotationZ)
					{
						point.z = 0f;
					}
				}
				rhs = Quaternion.Euler(point.x, point.y, point.z);
				this.m_deltaX = 0f;
				this.m_deltaY = 0f;
			}
			else
			{
				point = this.m_targetInverse * new Vector3(this.m_deltaY, -this.m_deltaX, 0f);
				if (base.EffectiveGridUnitSize != 0f)
				{
					if (Mathf.Abs(point.x) >= base.EffectiveGridUnitSize)
					{
						point.x = Mathf.Sign(point.x) * base.EffectiveGridUnitSize;
						this.m_deltaX = 0f;
						this.m_deltaY = 0f;
					}
					else
					{
						point.x = 0f;
					}
				}
				Vector3 axis4 = this.m_targetInverseMatrix.MultiplyVector(this.SceneCamera.cameraToWorldMatrix.MultiplyVector(-Vector3.forward));
				if (base.LockObject == null || !base.LockObject.RotationScreen)
				{
					rhs = Quaternion.AngleAxis(point.x, axis4);
				}
			}
			if (base.EffectiveGridUnitSize == 0f)
			{
				this.m_deltaX = 0f;
				this.m_deltaY = 0f;
			}
			for (int i = 0; i < base.ActiveTargets.Length; i++)
			{
				base.ActiveTargets[i].rotation *= rhs;
			}
		}

		// Token: 0x060005FA RID: 1530 RVA: 0x0002403C File Offset: 0x0002243C
		protected override void OnDrop()
		{
			base.OnDrop();
			this.m_targetRotation = base.Target.rotation;
		}

		// Token: 0x060005FB RID: 1531 RVA: 0x00024055 File Offset: 0x00022455
		protected override void DrawOverride()
		{
			RuntimeHandles.DoRotationHandle(base.Target.rotation * this.StartingRotationInv, base.Target.position, this.SelectedAxis, base.LockObject);
		}

		// Token: 0x04000576 RID: 1398
		public float GridSize = 15f;

		// Token: 0x04000577 RID: 1399
		public float XSpeed = 10f;

		// Token: 0x04000578 RID: 1400
		public float YSpeed = 10f;

		// Token: 0x04000579 RID: 1401
		private const float innerRadius = 1f;

		// Token: 0x0400057A RID: 1402
		private const float outerRadius = 1.2f;

		// Token: 0x0400057B RID: 1403
		private const float hitDot = 0.2f;

		// Token: 0x0400057C RID: 1404
		private float m_deltaX;

		// Token: 0x0400057D RID: 1405
		private float m_deltaY;

		// Token: 0x0400057E RID: 1406
		private Quaternion m_targetRotation = Quaternion.identity;

		// Token: 0x0400057F RID: 1407
		private Quaternion m_startingRotation = Quaternion.identity;

		// Token: 0x04000580 RID: 1408
		private Quaternion m_startinRotationInv = Quaternion.identity;

		// Token: 0x04000581 RID: 1409
		private Quaternion m_targetInverse = Quaternion.identity;

		// Token: 0x04000582 RID: 1410
		private Matrix4x4 m_targetInverseMatrix;

		// Token: 0x04000583 RID: 1411
		private Vector3 m_startingRotationAxis = Vector3.zero;

		// Token: 0x04000584 RID: 1412
		protected Vector3 currentMousePosition;

		// Token: 0x04000585 RID: 1413
		protected Vector3 lastMousePosition;
	}
}
