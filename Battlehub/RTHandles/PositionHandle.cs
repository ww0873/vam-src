using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Battlehub.RTCommon;
using UnityEngine;

namespace Battlehub.RTHandles
{
	// Token: 0x020000FC RID: 252
	public class PositionHandle : BaseHandle
	{
		// Token: 0x060005C5 RID: 1477 RVA: 0x00020362 File Offset: 0x0001E762
		public PositionHandle()
		{
		}

		// Token: 0x170000F4 RID: 244
		// (get) Token: 0x060005C6 RID: 1478 RVA: 0x0002039C File Offset: 0x0001E79C
		private bool IsInSnappingMode
		{
			get
			{
				return this.m_isInSnappingMode || RuntimeTools.IsSnapping;
			}
		}

		// Token: 0x170000F5 RID: 245
		// (get) Token: 0x060005C7 RID: 1479 RVA: 0x000203B1 File Offset: 0x0001E7B1
		// (set) Token: 0x060005C8 RID: 1480 RVA: 0x000203C9 File Offset: 0x0001E7C9
		protected override Vector3 HandlePosition
		{
			get
			{
				return base.transform.position + this.m_handleOffset;
			}
			set
			{
				base.transform.position = value - this.m_handleOffset;
			}
		}

		// Token: 0x170000F6 RID: 246
		// (get) Token: 0x060005C9 RID: 1481 RVA: 0x000203E2 File Offset: 0x0001E7E2
		protected override RuntimeTool Tool
		{
			get
			{
				return RuntimeTool.Move;
			}
		}

		// Token: 0x170000F7 RID: 247
		// (get) Token: 0x060005CA RID: 1482 RVA: 0x000203E5 File Offset: 0x0001E7E5
		protected override float CurrentGridUnitSize
		{
			get
			{
				return this.GridSize;
			}
		}

		// Token: 0x060005CB RID: 1483 RVA: 0x000203F0 File Offset: 0x0001E7F0
		protected override void OnEnableOverride()
		{
			base.OnEnableOverride();
			this.m_isInSnappingMode = false;
			RuntimeTools.IsSnapping = false;
			this.m_handleOffset = Vector3.zero;
			this.m_targetLayers = null;
			this.m_snapTargets = null;
			this.m_snapTargetsBounds = null;
			this.m_allExposedToEditor = null;
			RuntimeTools.IsSnappingChanged += this.OnSnappingChanged;
			this.OnSnappingChanged();
		}

		// Token: 0x060005CC RID: 1484 RVA: 0x0002044E File Offset: 0x0001E84E
		protected override void OnDisableOverride()
		{
			base.OnDisableOverride();
			RuntimeTools.IsSnapping = false;
			this.m_targetLayers = null;
			this.m_snapTargets = null;
			this.m_snapTargetsBounds = null;
			this.m_allExposedToEditor = null;
			RuntimeTools.IsSnappingChanged -= this.OnSnappingChanged;
		}

		// Token: 0x060005CD RID: 1485 RVA: 0x0002048C File Offset: 0x0001E88C
		protected override void UpdateOverride()
		{
			base.UpdateOverride();
			if (RuntimeTools.IsPointerOverGameObject())
			{
				return;
			}
			if (base.IsDragging && (this.SnapToGround || InputController.GetKey(this.SnapToGroundKey)) && this.SelectedAxis != RuntimeHandleAxis.Y)
			{
				PositionHandle.SnapActiveTargetsToGround(base.ActiveTargets, this.SceneCamera, true);
				base.transform.position = base.Targets[0].position;
			}
			if (this.HightlightOnHover && !base.IsDragging)
			{
				this.SelectedAxis = this.Hit();
			}
			if (InputController.GetKeyDown(this.SnappingKey))
			{
				if (base.LockObject == null || !base.LockObject.IsPositionLocked)
				{
					this.m_isInSnappingMode = true;
					if (InputController.GetKey(this.SnappingToggle))
					{
						RuntimeTools.IsSnapping = !RuntimeTools.IsSnapping;
					}
					this.BeginSnap();
					this.m_prevMousePosition = Input.mousePosition;
				}
			}
			else if (InputController.GetKeyUp(this.SnappingKey))
			{
				this.SelectedAxis = RuntimeHandleAxis.None;
				this.m_isInSnappingMode = false;
				if (!this.IsInSnappingMode)
				{
					this.m_handleOffset = Vector3.zero;
				}
				if (this.Model != null && this.Model is PositionHandleModel)
				{
					((PositionHandleModel)this.Model).IsVertexSnapping = false;
				}
			}
			if (this.IsInSnappingMode)
			{
				Vector2 vector = Input.mousePosition;
				if (RuntimeTools.SnappingMode == SnappingMode.BoundingBox)
				{
					if (base.IsDragging)
					{
						this.SelectedAxis = RuntimeHandleAxis.Snap;
						if (this.m_prevMousePosition != vector)
						{
							this.m_prevMousePosition = vector;
							float maxValue = float.MaxValue;
							Vector3 zero = Vector3.zero;
							bool flag = false;
							for (int i = 0; i < this.m_allExposedToEditor.Length; i++)
							{
								ExposeToEditor exposeToEditor = this.m_allExposedToEditor[i];
								Bounds bounds = exposeToEditor.Bounds;
								this.m_boundingBoxCorners[0] = bounds.center + new Vector3(bounds.extents.x, bounds.extents.y, bounds.extents.z);
								this.m_boundingBoxCorners[1] = bounds.center + new Vector3(bounds.extents.x, bounds.extents.y, -bounds.extents.z);
								this.m_boundingBoxCorners[2] = bounds.center + new Vector3(bounds.extents.x, -bounds.extents.y, bounds.extents.z);
								this.m_boundingBoxCorners[3] = bounds.center + new Vector3(bounds.extents.x, -bounds.extents.y, -bounds.extents.z);
								this.m_boundingBoxCorners[4] = bounds.center + new Vector3(-bounds.extents.x, bounds.extents.y, bounds.extents.z);
								this.m_boundingBoxCorners[5] = bounds.center + new Vector3(-bounds.extents.x, bounds.extents.y, -bounds.extents.z);
								this.m_boundingBoxCorners[6] = bounds.center + new Vector3(-bounds.extents.x, -bounds.extents.y, bounds.extents.z);
								this.m_boundingBoxCorners[7] = bounds.center + new Vector3(-bounds.extents.x, -bounds.extents.y, -bounds.extents.z);
								this.GetMinPoint(ref maxValue, ref zero, ref flag, exposeToEditor.BoundsObject.transform);
							}
							if (flag)
							{
								this.HandlePosition = zero;
							}
						}
					}
					else
					{
						this.SelectedAxis = RuntimeHandleAxis.None;
						if (this.m_prevMousePosition != vector)
						{
							this.m_prevMousePosition = vector;
							float maxValue2 = float.MaxValue;
							Vector3 zero2 = Vector3.zero;
							bool flag2 = false;
							for (int j = 0; j < this.m_snapTargets.Length; j++)
							{
								Transform tr = this.m_snapTargets[j];
								Bounds bounds2 = this.m_snapTargetsBounds[j];
								this.m_boundingBoxCorners[0] = bounds2.center + new Vector3(bounds2.extents.x, bounds2.extents.y, bounds2.extents.z);
								this.m_boundingBoxCorners[1] = bounds2.center + new Vector3(bounds2.extents.x, bounds2.extents.y, -bounds2.extents.z);
								this.m_boundingBoxCorners[2] = bounds2.center + new Vector3(bounds2.extents.x, -bounds2.extents.y, bounds2.extents.z);
								this.m_boundingBoxCorners[3] = bounds2.center + new Vector3(bounds2.extents.x, -bounds2.extents.y, -bounds2.extents.z);
								this.m_boundingBoxCorners[4] = bounds2.center + new Vector3(-bounds2.extents.x, bounds2.extents.y, bounds2.extents.z);
								this.m_boundingBoxCorners[5] = bounds2.center + new Vector3(-bounds2.extents.x, bounds2.extents.y, -bounds2.extents.z);
								this.m_boundingBoxCorners[6] = bounds2.center + new Vector3(-bounds2.extents.x, -bounds2.extents.y, bounds2.extents.z);
								this.m_boundingBoxCorners[7] = bounds2.center + new Vector3(-bounds2.extents.x, -bounds2.extents.y, -bounds2.extents.z);
								if (base.Targets[j] != null)
								{
									this.GetMinPoint(ref maxValue2, ref zero2, ref flag2, tr);
								}
							}
							if (flag2)
							{
								this.m_handleOffset = zero2 - base.transform.position;
							}
						}
					}
				}
				else if (base.IsDragging)
				{
					this.SelectedAxis = RuntimeHandleAxis.Snap;
					if (this.m_prevMousePosition != vector)
					{
						this.m_prevMousePosition = vector;
						Ray ray = this.SceneCamera.ScreenPointToRay(vector);
						LayerMask mask = 16;
						mask = ~mask;
						for (int k = 0; k < this.m_snapTargets.Length; k++)
						{
							this.m_targetLayers[k] = this.m_snapTargets[k].gameObject.layer;
							this.m_snapTargets[k].gameObject.layer = 4;
						}
						GameObject gameObject = null;
						RaycastHit raycastHit;
						if (Physics.Raycast(ray, out raycastHit, float.PositiveInfinity, mask))
						{
							gameObject = raycastHit.collider.gameObject;
						}
						else
						{
							float num = float.MaxValue;
							for (int l = 0; l < this.m_allExposedToEditor.Length; l++)
							{
								ExposeToEditor exposeToEditor2 = this.m_allExposedToEditor[l];
								Bounds bounds3 = exposeToEditor2.Bounds;
								this.m_boundingBoxCorners[0] = bounds3.center + new Vector3(bounds3.extents.x, bounds3.extents.y, bounds3.extents.z);
								this.m_boundingBoxCorners[1] = bounds3.center + new Vector3(bounds3.extents.x, bounds3.extents.y, -bounds3.extents.z);
								this.m_boundingBoxCorners[2] = bounds3.center + new Vector3(bounds3.extents.x, -bounds3.extents.y, bounds3.extents.z);
								this.m_boundingBoxCorners[3] = bounds3.center + new Vector3(bounds3.extents.x, -bounds3.extents.y, -bounds3.extents.z);
								this.m_boundingBoxCorners[4] = bounds3.center + new Vector3(-bounds3.extents.x, bounds3.extents.y, bounds3.extents.z);
								this.m_boundingBoxCorners[5] = bounds3.center + new Vector3(-bounds3.extents.x, bounds3.extents.y, -bounds3.extents.z);
								this.m_boundingBoxCorners[6] = bounds3.center + new Vector3(-bounds3.extents.x, -bounds3.extents.y, bounds3.extents.z);
								this.m_boundingBoxCorners[7] = bounds3.center + new Vector3(-bounds3.extents.x, -bounds3.extents.y, -bounds3.extents.z);
								for (int m = 0; m < this.m_boundingBoxCorners.Length; m++)
								{
									Vector2 a = this.SceneCamera.WorldToScreenPoint(exposeToEditor2.BoundsObject.transform.TransformPoint(this.m_boundingBoxCorners[m]));
									float magnitude = (a - vector).magnitude;
									if (magnitude < num)
									{
										gameObject = exposeToEditor2.gameObject;
										num = magnitude;
									}
								}
							}
						}
						if (gameObject != null)
						{
							float maxValue3 = float.MaxValue;
							Vector3 zero3 = Vector3.zero;
							bool flag3 = false;
							Transform meshTransform;
							Mesh mesh = PositionHandle.GetMesh(gameObject, out meshTransform);
							this.GetMinPoint(meshTransform, ref maxValue3, ref zero3, ref flag3, mesh);
							if (flag3)
							{
								this.HandlePosition = zero3;
							}
						}
						for (int n = 0; n < this.m_snapTargets.Length; n++)
						{
							this.m_snapTargets[n].gameObject.layer = this.m_targetLayers[n];
						}
					}
				}
				else
				{
					this.SelectedAxis = RuntimeHandleAxis.None;
					if (this.m_prevMousePosition != vector)
					{
						this.m_prevMousePosition = vector;
						float maxValue4 = float.MaxValue;
						Vector3 zero4 = Vector3.zero;
						bool flag4 = false;
						for (int num2 = 0; num2 < base.RealTargets.Length; num2++)
						{
							Transform transform = base.RealTargets[num2];
							Transform meshTransform2;
							Mesh mesh2 = PositionHandle.GetMesh(transform.gameObject, out meshTransform2);
							this.GetMinPoint(meshTransform2, ref maxValue4, ref zero4, ref flag4, mesh2);
						}
						if (flag4)
						{
							this.m_handleOffset = zero4 - base.transform.position;
						}
					}
				}
			}
		}

		// Token: 0x060005CE RID: 1486 RVA: 0x000211D8 File Offset: 0x0001F5D8
		private void GetMinPoint(Transform meshTransform, ref float minDistance, ref Vector3 minPoint, ref bool minPointFound, Mesh mesh)
		{
			if (mesh != null && mesh.isReadable)
			{
				foreach (Vector3 vector in mesh.vertices)
				{
					vector = meshTransform.TransformPoint(vector);
					Vector3 a = this.SceneCamera.WorldToScreenPoint(vector);
					a.z = 0f;
					Vector3 mousePosition = Input.mousePosition;
					mousePosition.z = 0f;
					float magnitude = (a - mousePosition).magnitude;
					if (magnitude < minDistance)
					{
						minPointFound = true;
						minDistance = magnitude;
						minPoint = vector;
					}
				}
			}
		}

		// Token: 0x060005CF RID: 1487 RVA: 0x00021288 File Offset: 0x0001F688
		private static Mesh GetMesh(GameObject go, out Transform meshTransform)
		{
			Mesh result = null;
			meshTransform = null;
			MeshFilter componentInChildren = go.GetComponentInChildren<MeshFilter>();
			if (componentInChildren != null)
			{
				result = componentInChildren.sharedMesh;
				meshTransform = componentInChildren.transform;
			}
			else
			{
				SkinnedMeshRenderer componentInChildren2 = go.GetComponentInChildren<SkinnedMeshRenderer>();
				if (componentInChildren2 != null)
				{
					result = componentInChildren2.sharedMesh;
					meshTransform = componentInChildren2.transform;
				}
				else
				{
					MeshCollider componentInChildren3 = go.GetComponentInChildren<MeshCollider>();
					if (componentInChildren3 != null)
					{
						result = componentInChildren3.sharedMesh;
						meshTransform = componentInChildren3.transform;
					}
				}
			}
			return result;
		}

		// Token: 0x060005D0 RID: 1488 RVA: 0x0002130C File Offset: 0x0001F70C
		protected override void OnDrop()
		{
			base.OnDrop();
			if (this.SnapToGround || InputController.GetKey(this.SnapToGroundKey))
			{
				PositionHandle.SnapActiveTargetsToGround(base.ActiveTargets, this.SceneCamera, true);
				base.transform.position = base.Targets[0].position;
			}
		}

		// Token: 0x060005D1 RID: 1489 RVA: 0x00021364 File Offset: 0x0001F764
		private static void SnapActiveTargetsToGround(Transform[] targets, Camera camera, bool rotate)
		{
			Plane[] array = GeometryUtility.CalculateFrustumPlanes(camera);
			for (int i = 0; i < targets.Length; i++)
			{
				PositionHandle.<SnapActiveTargetsToGround>c__AnonStorey0 <SnapActiveTargetsToGround>c__AnonStorey = new PositionHandle.<SnapActiveTargetsToGround>c__AnonStorey0();
				<SnapActiveTargetsToGround>c__AnonStorey.activeTarget = targets[i];
				Ray ray = new Ray(<SnapActiveTargetsToGround>c__AnonStorey.activeTarget.position, Vector3.up);
				bool flag = false;
				Vector3 origin = <SnapActiveTargetsToGround>c__AnonStorey.activeTarget.position;
				for (int j = 0; j < array.Length; j++)
				{
					float distance;
					if (array[j].Raycast(ray, out distance))
					{
						flag = true;
						origin = ray.GetPoint(distance);
					}
				}
				if (flag)
				{
					ray = new Ray(origin, Vector3.down);
					RaycastHit[] array2 = Physics.RaycastAll(ray).Where(new Func<RaycastHit, bool>(<SnapActiveTargetsToGround>c__AnonStorey.<>m__0)).ToArray<RaycastHit>();
					if (array2.Length != 0)
					{
						float num = float.PositiveInfinity;
						RaycastHit raycastHit = array2[0];
						for (int k = 0; k < array2.Length; k++)
						{
							float magnitude = (<SnapActiveTargetsToGround>c__AnonStorey.activeTarget.position - array2[k].point).magnitude;
							if (magnitude < num)
							{
								num = magnitude;
								raycastHit = array2[k];
							}
						}
						<SnapActiveTargetsToGround>c__AnonStorey.activeTarget.position += raycastHit.point - <SnapActiveTargetsToGround>c__AnonStorey.activeTarget.position;
						if (rotate)
						{
							<SnapActiveTargetsToGround>c__AnonStorey.activeTarget.rotation = Quaternion.FromToRotation(<SnapActiveTargetsToGround>c__AnonStorey.activeTarget.up, raycastHit.normal) * <SnapActiveTargetsToGround>c__AnonStorey.activeTarget.rotation;
						}
					}
				}
			}
		}

		// Token: 0x060005D2 RID: 1490 RVA: 0x00021520 File Offset: 0x0001F920
		private void OnSnappingChanged()
		{
			if (RuntimeTools.IsSnapping)
			{
				this.BeginSnap();
			}
			else
			{
				this.m_handleOffset = Vector3.zero;
				if (this.Model != null && this.Model is PositionHandleModel)
				{
					((PositionHandleModel)this.Model).IsVertexSnapping = false;
				}
			}
		}

		// Token: 0x060005D3 RID: 1491 RVA: 0x00021580 File Offset: 0x0001F980
		private void BeginSnap()
		{
			if (this.SceneCamera == null)
			{
				return;
			}
			if (this.Model != null && this.Model is PositionHandleModel)
			{
				((PositionHandleModel)this.Model).IsVertexSnapping = true;
			}
			HashSet<Transform> hashSet = new HashSet<Transform>();
			List<Transform> list = new List<Transform>();
			List<Bounds> list2 = new List<Bounds>();
			if (base.Target != null)
			{
				for (int i = 0; i < base.RealTargets.Length; i++)
				{
					Transform transform = base.RealTargets[i];
					if (transform != null)
					{
						list.Add(transform);
						hashSet.Add(transform);
						ExposeToEditor component = transform.GetComponent<ExposeToEditor>();
						if (component != null)
						{
							list2.Add(component.Bounds);
						}
						else
						{
							MeshFilter component2 = transform.GetComponent<MeshFilter>();
							if (component2 != null && component2.sharedMesh != null)
							{
								list2.Add(component2.sharedMesh.bounds);
							}
							else
							{
								SkinnedMeshRenderer component3 = transform.GetComponent<SkinnedMeshRenderer>();
								if (component3 != null && component3.sharedMesh != null)
								{
									list2.Add(component3.sharedMesh.bounds);
								}
								else
								{
									Bounds item = new Bounds(Vector3.zero, Vector3.zero);
									list2.Add(item);
								}
							}
						}
					}
				}
			}
			this.m_snapTargets = list.ToArray();
			this.m_targetLayers = new int[this.m_snapTargets.Length];
			this.m_snapTargetsBounds = list2.ToArray();
			Plane[] planes = GeometryUtility.CalculateFrustumPlanes(this.SceneCamera);
			ExposeToEditor[] array = UnityEngine.Object.FindObjectsOfType<ExposeToEditor>();
			List<ExposeToEditor> list3 = new List<ExposeToEditor>();
			foreach (ExposeToEditor exposeToEditor in array)
			{
				if (exposeToEditor.CanSnap && GeometryUtility.TestPlanesAABB(planes, new Bounds(exposeToEditor.transform.TransformPoint(exposeToEditor.Bounds.center), Vector3.zero)) && !hashSet.Contains(exposeToEditor.transform))
				{
					list3.Add(exposeToEditor);
				}
			}
			this.m_allExposedToEditor = list3.ToArray();
		}

		// Token: 0x060005D4 RID: 1492 RVA: 0x000217C0 File Offset: 0x0001FBC0
		private void GetMinPoint(ref float minDistance, ref Vector3 minPoint, ref bool minPointFound, Transform tr)
		{
			for (int i = 0; i < this.m_boundingBoxCorners.Length; i++)
			{
				Vector3 vector = tr.TransformPoint(this.m_boundingBoxCorners[i]);
				Vector3 a = this.SceneCamera.WorldToScreenPoint(vector);
				a.z = 0f;
				Vector3 mousePosition = Input.mousePosition;
				mousePosition.z = 0f;
				float magnitude = (a - mousePosition).magnitude;
				if (magnitude < minDistance)
				{
					minPointFound = true;
					minDistance = magnitude;
					minPoint = vector;
				}
			}
		}

		// Token: 0x060005D5 RID: 1493 RVA: 0x00021854 File Offset: 0x0001FC54
		private bool HitSnapHandle()
		{
			Vector3 vector = this.SceneCamera.WorldToScreenPoint(this.HandlePosition);
			Vector3 mousePosition = Input.mousePosition;
			return vector.x - 10f <= mousePosition.x && mousePosition.x <= vector.x + 10f && vector.y - 10f <= mousePosition.y && mousePosition.y <= vector.y + 10f;
		}

		// Token: 0x060005D6 RID: 1494 RVA: 0x000218E0 File Offset: 0x0001FCE0
		private bool HitQuad(Vector3 axis, Matrix4x4 matrix, float size)
		{
			Ray ray = this.SceneCamera.ScreenPointToRay(Input.mousePosition);
			Plane plane = new Plane(matrix.MultiplyVector(axis).normalized, matrix.MultiplyPoint(Vector3.zero));
			float distance;
			if (!plane.Raycast(ray, out distance))
			{
				return false;
			}
			Vector3 point = ray.GetPoint(distance);
			point = matrix.inverse.MultiplyPoint(point);
			Vector3 lhs = matrix.inverse.MultiplyVector(this.SceneCamera.transform.position - this.HandlePosition);
			float num = Mathf.Sign(Vector3.Dot(lhs, Vector3.right));
			float num2 = Mathf.Sign(Vector3.Dot(lhs, Vector3.up));
			float num3 = Mathf.Sign(Vector3.Dot(lhs, Vector3.forward));
			point.x *= num;
			point.y *= num2;
			point.z *= num3;
			float num4 = -0.01f;
			bool flag = point.x >= num4 && point.x <= size && point.y >= num4 && point.y <= size && point.z >= num4 && point.z <= size;
			if (flag)
			{
				base.DragPlane = this.GetDragPlane(matrix, axis);
			}
			return flag;
		}

		// Token: 0x060005D7 RID: 1495 RVA: 0x00021A5C File Offset: 0x0001FE5C
		private RuntimeHandleAxis Hit()
		{
			float screenScale = RuntimeHandles.GetScreenScale(this.HandlePosition, this.SceneCamera);
			this.m_matrix = Matrix4x4.TRS(this.HandlePosition, base.Rotation, Vector3.one);
			this.m_inverse = this.m_matrix.inverse;
			Matrix4x4 matrix = Matrix4x4.TRS(this.HandlePosition, base.Rotation, new Vector3(screenScale, screenScale, screenScale));
			float size = 0.3f * screenScale;
			if (this.HitQuad(Vector3.up * 1f, this.m_matrix, size))
			{
				return RuntimeHandleAxis.XZ;
			}
			if (this.HitQuad(Vector3.right * 1f, this.m_matrix, size))
			{
				return RuntimeHandleAxis.YZ;
			}
			if (this.HitQuad(Vector3.forward * 1f, this.m_matrix, size))
			{
				return RuntimeHandleAxis.XY;
			}
			float num;
			bool flag = this.HitAxis(Vector3.up * 1f, matrix, out num);
			float num2;
			flag |= this.HitAxis(Vector3.forward * 1f, matrix, out num2);
			float num3;
			flag |= this.HitAxis(Vector3.right * 1f, matrix, out num3);
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

		// Token: 0x060005D8 RID: 1496 RVA: 0x00021BB8 File Offset: 0x0001FFB8
		protected override bool OnBeginDrag()
		{
			this.SelectedAxis = this.Hit();
			this.m_currentPosition = this.HandlePosition;
			this.m_cursorPosition = this.HandlePosition;
			if (this.IsInSnappingMode)
			{
				return this.HitSnapHandle();
			}
			if (this.SelectedAxis == RuntimeHandleAxis.XZ)
			{
				return this.GetPointOnDragPlane(Input.mousePosition, out this.m_prevPoint);
			}
			if (this.SelectedAxis == RuntimeHandleAxis.YZ)
			{
				return this.GetPointOnDragPlane(Input.mousePosition, out this.m_prevPoint);
			}
			if (this.SelectedAxis == RuntimeHandleAxis.XY)
			{
				return this.GetPointOnDragPlane(Input.mousePosition, out this.m_prevPoint);
			}
			if (this.SelectedAxis != RuntimeHandleAxis.None)
			{
				base.DragPlane = this.GetDragPlane();
				bool pointOnDragPlane = this.GetPointOnDragPlane(Input.mousePosition, out this.m_prevPoint);
				if (!pointOnDragPlane)
				{
					this.SelectedAxis = RuntimeHandleAxis.None;
				}
				return pointOnDragPlane;
			}
			return false;
		}

		// Token: 0x060005D9 RID: 1497 RVA: 0x00021C90 File Offset: 0x00020090
		protected override void OnDrag()
		{
			if (this.IsInSnappingMode)
			{
				return;
			}
			Vector3 vector;
			if (this.GetPointOnDragPlane(Input.mousePosition, out vector))
			{
				Vector3 vector2 = this.m_inverse.MultiplyVector(vector - this.m_prevPoint);
				float magnitude = vector2.magnitude;
				if (this.SelectedAxis == RuntimeHandleAxis.X)
				{
					vector2.y = (vector2.z = 0f);
				}
				else if (this.SelectedAxis == RuntimeHandleAxis.Y)
				{
					vector2.x = (vector2.z = 0f);
				}
				else if (this.SelectedAxis == RuntimeHandleAxis.Z)
				{
					vector2.x = (vector2.y = 0f);
				}
				if (base.LockObject != null)
				{
					if (base.LockObject.PositionX)
					{
						vector2.x = 0f;
					}
					if (base.LockObject.PositionY)
					{
						vector2.y = 0f;
					}
					if (base.LockObject.PositionZ)
					{
						vector2.z = 0f;
					}
				}
				if ((double)base.EffectiveGridUnitSize == 0.0)
				{
					vector2 = this.m_matrix.MultiplyVector(vector2).normalized * magnitude;
					base.transform.position += vector2;
					this.m_prevPoint = vector;
				}
				else
				{
					vector2 = this.m_matrix.MultiplyVector(vector2).normalized * magnitude;
					this.m_cursorPosition += vector2;
					Vector3 vector3 = this.m_cursorPosition - this.m_currentPosition;
					Vector3 zero = Vector3.zero;
					if (Mathf.Abs(vector3.x * 1.5f) >= base.EffectiveGridUnitSize)
					{
						zero.x = base.EffectiveGridUnitSize * Mathf.Sign(vector3.x);
					}
					if (Mathf.Abs(vector3.y * 1.5f) >= base.EffectiveGridUnitSize)
					{
						zero.y = base.EffectiveGridUnitSize * Mathf.Sign(vector3.y);
					}
					if (Mathf.Abs(vector3.z * 1.5f) >= base.EffectiveGridUnitSize)
					{
						zero.z = base.EffectiveGridUnitSize * Mathf.Sign(vector3.z);
					}
					this.m_currentPosition += zero;
					this.HandlePosition = this.m_currentPosition;
					this.m_prevPoint = vector;
				}
			}
		}

		// Token: 0x060005DA RID: 1498 RVA: 0x00021F11 File Offset: 0x00020311
		protected override void DrawOverride()
		{
			RuntimeHandles.DoPositionHandle(this.HandlePosition, base.Rotation, this.SelectedAxis, this.IsInSnappingMode, base.LockObject);
		}

		// Token: 0x0400052E RID: 1326
		public float GridSize = 1f;

		// Token: 0x0400052F RID: 1327
		private Vector3 m_cursorPosition;

		// Token: 0x04000530 RID: 1328
		private Vector3 m_currentPosition;

		// Token: 0x04000531 RID: 1329
		private Vector3 m_prevPoint;

		// Token: 0x04000532 RID: 1330
		private Matrix4x4 m_matrix;

		// Token: 0x04000533 RID: 1331
		private Matrix4x4 m_inverse;

		// Token: 0x04000534 RID: 1332
		private Vector2 m_prevMousePosition;

		// Token: 0x04000535 RID: 1333
		private int[] m_targetLayers;

		// Token: 0x04000536 RID: 1334
		private Transform[] m_snapTargets;

		// Token: 0x04000537 RID: 1335
		private Bounds[] m_snapTargetsBounds;

		// Token: 0x04000538 RID: 1336
		private ExposeToEditor[] m_allExposedToEditor;

		// Token: 0x04000539 RID: 1337
		public bool SnapToGround;

		// Token: 0x0400053A RID: 1338
		public KeyCode SnapToGroundKey = KeyCode.G;

		// Token: 0x0400053B RID: 1339
		public KeyCode SnappingKey = KeyCode.V;

		// Token: 0x0400053C RID: 1340
		public KeyCode SnappingToggle = KeyCode.LeftShift;

		// Token: 0x0400053D RID: 1341
		private bool m_isInSnappingMode;

		// Token: 0x0400053E RID: 1342
		private Vector3[] m_boundingBoxCorners = new Vector3[8];

		// Token: 0x0400053F RID: 1343
		private Vector3 m_handleOffset;

		// Token: 0x02000EA8 RID: 3752
		[CompilerGenerated]
		private sealed class <SnapActiveTargetsToGround>c__AnonStorey0
		{
			// Token: 0x06007171 RID: 29041 RVA: 0x00021F36 File Offset: 0x00020336
			public <SnapActiveTargetsToGround>c__AnonStorey0()
			{
			}

			// Token: 0x06007172 RID: 29042 RVA: 0x00021F3E File Offset: 0x0002033E
			internal bool <>m__0(RaycastHit hit)
			{
				return !hit.transform.IsChildOf(this.activeTarget);
			}

			// Token: 0x04006542 RID: 25922
			internal Transform activeTarget;
		}
	}
}
