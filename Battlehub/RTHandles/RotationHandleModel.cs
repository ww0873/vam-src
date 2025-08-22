using System;
using Battlehub.RTCommon;
using UnityEngine;

namespace Battlehub.RTHandles
{
	// Token: 0x020000FF RID: 255
	public class RotationHandleModel : BaseHandleModel
	{
		// Token: 0x060005FC RID: 1532 RVA: 0x0002408C File Offset: 0x0002248C
		public RotationHandleModel()
		{
		}

		// Token: 0x060005FD RID: 1533 RVA: 0x000240F8 File Offset: 0x000224F8
		protected override void Awake()
		{
			base.Awake();
			this.m_xyzMesh = this.m_xyz.sharedMesh;
			this.m_innerCircleMesh = this.m_innerCircle.sharedMesh;
			this.m_outerCircleMesh = this.m_outerCircle.sharedMesh;
			Renderer component = this.m_xyz.GetComponent<Renderer>();
			component.sharedMaterials = component.materials;
			this.m_xyzMaterials = component.sharedMaterials;
			component = this.m_innerCircle.GetComponent<Renderer>();
			component.sharedMaterials = component.materials;
			this.m_innerCircleMaterials = component.sharedMaterials;
			component = this.m_outerCircle.GetComponent<Renderer>();
			component.sharedMaterials = component.materials;
			this.m_outerCircleMaterial = component.sharedMaterial;
			Mesh mesh = this.m_xyz.mesh;
			this.m_xyz.sharedMesh = mesh;
			mesh = this.m_innerCircle.mesh;
			this.m_innerCircle.sharedMesh = mesh;
			mesh = this.m_outerCircle.mesh;
			this.m_outerCircle.sharedMesh = mesh;
		}

		// Token: 0x060005FE RID: 1534 RVA: 0x000241F4 File Offset: 0x000225F4
		protected override void Start()
		{
			base.Start();
			this.UpdateXYZ(this.m_xyz.sharedMesh, this.m_majorRadius, this.m_minorRadius);
			this.UpdateCircle(this.m_innerCircle.sharedMesh, this.m_innerCircleMesh, this.m_innerCircle.transform, this.m_majorRadius, this.m_minorRadius);
			this.UpdateCircle(this.m_outerCircle.sharedMesh, this.m_outerCircleMesh, this.m_outerCircle.transform, this.m_outerRadius, this.m_minorRadius);
			this.SetColors();
		}

		// Token: 0x060005FF RID: 1535 RVA: 0x00024286 File Offset: 0x00022686
		public override void Select(RuntimeHandleAxis axis)
		{
			base.Select(axis);
			this.SetColors();
		}

		// Token: 0x06000600 RID: 1536 RVA: 0x00024295 File Offset: 0x00022695
		public override void SetLock(LockObject lockObj)
		{
			base.SetLock(lockObj);
			this.SetColors();
		}

		// Token: 0x06000601 RID: 1537 RVA: 0x000242A4 File Offset: 0x000226A4
		private void SetDefaultColors()
		{
			if (this.m_lockObj.RotationX)
			{
				this.m_xyzMaterials[this.m_xMatIndex].color = this.m_disabledColor;
			}
			else
			{
				this.m_xyzMaterials[this.m_xMatIndex].color = this.m_xColor;
			}
			if (this.m_lockObj.RotationY)
			{
				this.m_xyzMaterials[this.m_yMatIndex].color = this.m_disabledColor;
			}
			else
			{
				this.m_xyzMaterials[this.m_yMatIndex].color = this.m_yColor;
			}
			if (this.m_lockObj.RotationZ)
			{
				this.m_xyzMaterials[this.m_zMatIndex].color = this.m_disabledColor;
			}
			else
			{
				this.m_xyzMaterials[this.m_zMatIndex].color = this.m_zColor;
			}
			if (this.m_lockObj.RotationScreen)
			{
				this.m_outerCircleMaterial.color = this.m_disabledColor;
			}
			else
			{
				this.m_outerCircleMaterial.color = this.m_altColor;
			}
			this.m_outerCircleMaterial.SetInt("_ZTest", 2);
			if (this.m_lockObj.IsPositionLocked)
			{
				this.m_innerCircleMaterials[this.m_innerCircleBorderMatIndex].color = this.m_disabledColor;
			}
			else
			{
				this.m_innerCircleMaterials[this.m_innerCircleBorderMatIndex].color = this.m_altColor2;
			}
			this.m_innerCircleMaterials[this.m_innerCircleFillMatIndex].color = new Color(0f, 0f, 0f, 0f);
		}

		// Token: 0x06000602 RID: 1538 RVA: 0x00024438 File Offset: 0x00022838
		private void SetColors()
		{
			this.SetDefaultColors();
			RuntimeHandleAxis selectedAxis = this.m_selectedAxis;
			switch (selectedAxis)
			{
			case RuntimeHandleAxis.X:
				if (!this.m_lockObj.PositionX)
				{
					this.m_xyzMaterials[this.m_xMatIndex].color = this.m_selectionColor;
				}
				break;
			case RuntimeHandleAxis.Y:
				if (!this.m_lockObj.PositionY)
				{
					this.m_xyzMaterials[this.m_yMatIndex].color = this.m_selectionColor;
				}
				break;
			default:
				if (selectedAxis == RuntimeHandleAxis.Free)
				{
					if (!this.m_lockObj.IsPositionLocked)
					{
						this.m_innerCircleMaterials[this.m_innerCircleFillMatIndex].color = new Color(0f, 0f, 0f, 0.1f);
					}
				}
				break;
			case RuntimeHandleAxis.Z:
				if (!this.m_lockObj.PositionZ)
				{
					this.m_xyzMaterials[this.m_zMatIndex].color = this.m_selectionColor;
				}
				break;
			case RuntimeHandleAxis.Screen:
				if (!this.m_lockObj.RotationScreen)
				{
					this.m_outerCircleMaterial.color = this.m_selectionColor;
					this.m_outerCircleMaterial.SetInt("_ZTest", 0);
				}
				break;
			}
		}

		// Token: 0x06000603 RID: 1539 RVA: 0x00024588 File Offset: 0x00022988
		private void UpdateXYZ(Mesh mesh, float majorRadius, float minorRadius)
		{
			this.m_xyz.transform.localScale = Vector3.one * majorRadius;
			minorRadius /= Mathf.Max(0.01f, majorRadius);
			Vector3[] vertices = this.m_xyzMesh.vertices;
			for (int i = 0; i < this.m_xyzMesh.subMeshCount; i++)
			{
				foreach (int num in mesh.GetTriangles(i))
				{
					Vector3 vector = vertices[num];
					Vector3 vector2 = vector;
					if (i == 0)
					{
						vector2.x = 0f;
					}
					else if (i == 1)
					{
						vector2.y = 0f;
					}
					else if (i == 2)
					{
						vector2.z = 0f;
					}
					vector2.Normalize();
					vertices[num] = vector2 + (vector - vector2).normalized * minorRadius;
				}
			}
			mesh.vertices = vertices;
		}

		// Token: 0x06000604 RID: 1540 RVA: 0x00024698 File Offset: 0x00022A98
		private void UpdateCircle(Mesh mesh, Mesh originalMesh, Transform circleTransform, float majorRadius, float minorRadius)
		{
			circleTransform.localScale = Vector3.one * majorRadius;
			minorRadius /= Mathf.Max(0.01f, majorRadius);
			Vector3[] vertices = originalMesh.vertices;
			foreach (int num in mesh.GetTriangles(0))
			{
				Vector3 vector = vertices[num];
				Vector3 vector2 = vector;
				vector2.z = 0f;
				vector2.Normalize();
				vertices[num] = vector2 + (vector - vector2).normalized * minorRadius;
			}
			foreach (int num2 in mesh.GetTriangles(1))
			{
				Vector3 a = vertices[num2];
				a.Normalize();
				vertices[num2] = a * (1f - minorRadius);
			}
			mesh.vertices = vertices;
		}

		// Token: 0x06000605 RID: 1541 RVA: 0x000247A0 File Offset: 0x00022BA0
		protected override void Update()
		{
			if (this.m_prevMinorRadius != this.m_minorRadius || this.m_prevMajorRadius != this.m_majorRadius || this.m_prevOuterRadius != this.m_outerRadius)
			{
				this.m_prevMinorRadius = this.m_minorRadius;
				this.m_prevMajorRadius = this.m_majorRadius;
				this.m_prevOuterRadius = this.m_outerRadius;
				this.UpdateXYZ(this.m_xyz.sharedMesh, this.m_majorRadius, this.m_minorRadius);
				this.UpdateCircle(this.m_innerCircle.sharedMesh, this.m_innerCircleMesh, this.m_innerCircle.transform, this.m_majorRadius, this.m_minorRadius);
				this.UpdateCircle(this.m_outerCircle.sharedMesh, this.m_outerCircleMesh, this.m_outerCircle.transform, this.m_outerRadius, this.m_minorRadius);
			}
		}

		// Token: 0x04000586 RID: 1414
		private const float DefaultMinorRadius = 0.05f;

		// Token: 0x04000587 RID: 1415
		private const float DefaultMajorRadius = 1f;

		// Token: 0x04000588 RID: 1416
		private const float DefaultOuterRadius = 1.11f;

		// Token: 0x04000589 RID: 1417
		[SerializeField]
		private float m_minorRadius = 0.05f;

		// Token: 0x0400058A RID: 1418
		[SerializeField]
		private float m_majorRadius = 1f;

		// Token: 0x0400058B RID: 1419
		[SerializeField]
		private float m_outerRadius = 1.11f;

		// Token: 0x0400058C RID: 1420
		[SerializeField]
		private MeshFilter m_xyz;

		// Token: 0x0400058D RID: 1421
		[SerializeField]
		private MeshFilter m_innerCircle;

		// Token: 0x0400058E RID: 1422
		[SerializeField]
		private MeshFilter m_outerCircle;

		// Token: 0x0400058F RID: 1423
		private Mesh m_xyzMesh;

		// Token: 0x04000590 RID: 1424
		private Mesh m_innerCircleMesh;

		// Token: 0x04000591 RID: 1425
		private Mesh m_outerCircleMesh;

		// Token: 0x04000592 RID: 1426
		[SerializeField]
		private int m_xMatIndex;

		// Token: 0x04000593 RID: 1427
		[SerializeField]
		private int m_yMatIndex = 1;

		// Token: 0x04000594 RID: 1428
		[SerializeField]
		private int m_zMatIndex = 2;

		// Token: 0x04000595 RID: 1429
		[SerializeField]
		private int m_innerCircleBorderMatIndex;

		// Token: 0x04000596 RID: 1430
		[SerializeField]
		private int m_innerCircleFillMatIndex = 1;

		// Token: 0x04000597 RID: 1431
		private Material[] m_xyzMaterials;

		// Token: 0x04000598 RID: 1432
		private Material[] m_innerCircleMaterials;

		// Token: 0x04000599 RID: 1433
		private Material m_outerCircleMaterial;

		// Token: 0x0400059A RID: 1434
		private float m_prevMinorRadius = 0.05f;

		// Token: 0x0400059B RID: 1435
		private float m_prevMajorRadius = 1f;

		// Token: 0x0400059C RID: 1436
		private float m_prevOuterRadius = 1.11f;
	}
}
