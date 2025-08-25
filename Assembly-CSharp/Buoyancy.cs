using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000305 RID: 773
public class Buoyancy : MonoBehaviour
{
	// Token: 0x06001243 RID: 4675 RVA: 0x00065515 File Offset: 0x00063915
	public Buoyancy()
	{
	}

	// Token: 0x06001244 RID: 4676 RVA: 0x00065544 File Offset: 0x00063944
	private void Start()
	{
		this.forces = new List<Vector3[]>();
		Quaternion rotation = base.transform.rotation;
		Vector3 position = base.transform.position;
		base.transform.rotation = Quaternion.identity;
		base.transform.position = Vector3.zero;
		if (base.GetComponent<Collider>() == null)
		{
			base.gameObject.AddComponent<MeshCollider>();
			Debug.LogWarning(string.Format("[Buoyancy.cs] Object \"{0}\" had no collider. MeshCollider has been added.", base.name));
		}
		this.isMeshCollider = (base.GetComponent<MeshCollider>() != null);
		Bounds bounds = base.GetComponent<Collider>().bounds;
		if (bounds.size.x < bounds.size.y)
		{
			this.voxelHalfHeight = bounds.size.x;
		}
		else
		{
			this.voxelHalfHeight = bounds.size.y;
		}
		if (bounds.size.z < this.voxelHalfHeight)
		{
			this.voxelHalfHeight = bounds.size.z;
		}
		this.voxelHalfHeight /= (float)(2 * this.SlicesPerAxis);
		if (base.GetComponent<Rigidbody>() == null)
		{
			base.gameObject.AddComponent<Rigidbody>();
			Debug.LogWarning(string.Format("[Buoyancy.cs] Object \"{0}\" had no Rigidbody. Rigidbody has been added.", base.name));
		}
		base.GetComponent<Rigidbody>().centerOfMass = new Vector3(0f, -bounds.extents.y * 0f, 0f) + base.transform.InverseTransformPoint(bounds.center);
		this.voxels = this.SliceIntoVoxels(this.isMeshCollider && this.IsConcave);
		base.transform.rotation = rotation;
		base.transform.position = position;
		float num = base.GetComponent<Rigidbody>().mass / this.Density;
		Buoyancy.WeldPoints(this.voxels, this.VoxelsLimit);
		float num2 = 1000f * Mathf.Abs(Physics.gravity.y) * num;
		this.localArchimedesForce = num2 / (float)this.voxels.Count;
	}

	// Token: 0x06001245 RID: 4677 RVA: 0x0006578C File Offset: 0x00063B8C
	private List<Vector3> SliceIntoVoxels(bool concave)
	{
		List<Vector3> list = new List<Vector3>(this.SlicesPerAxis * this.SlicesPerAxis * this.SlicesPerAxis);
		if (concave)
		{
			MeshCollider component = base.GetComponent<MeshCollider>();
			bool convex = component.convex;
			component.convex = false;
			Bounds bounds = base.GetComponent<Collider>().bounds;
			for (int i = 0; i < this.SlicesPerAxis; i++)
			{
				for (int j = 0; j < this.SlicesPerAxis; j++)
				{
					for (int k = 0; k < this.SlicesPerAxis; k++)
					{
						float x = bounds.min.x + bounds.size.x / (float)this.SlicesPerAxis * (0.5f + (float)i);
						float y = bounds.min.y + bounds.size.y / (float)this.SlicesPerAxis * (0.5f + (float)j);
						float z = bounds.min.z + bounds.size.z / (float)this.SlicesPerAxis * (0.5f + (float)k);
						Vector3 vector = base.transform.InverseTransformPoint(new Vector3(x, y, z));
						if (Buoyancy.PointIsInsideMeshCollider(component, vector))
						{
							list.Add(vector);
						}
					}
				}
			}
			if (list.Count == 0)
			{
				list.Add(bounds.center);
			}
			component.convex = convex;
		}
		else
		{
			Bounds bounds2 = base.GetComponent<Collider>().bounds;
			for (int l = 0; l < this.SlicesPerAxis; l++)
			{
				for (int m = 0; m < this.SlicesPerAxis; m++)
				{
					for (int n = 0; n < this.SlicesPerAxis; n++)
					{
						float x2 = bounds2.min.x + bounds2.size.x / (float)this.SlicesPerAxis * (0.5f + (float)l);
						float y2 = bounds2.min.y + bounds2.size.y / (float)this.SlicesPerAxis * (0.5f + (float)m);
						float z2 = bounds2.min.z + bounds2.size.z / (float)this.SlicesPerAxis * (0.5f + (float)n);
						Vector3 item = base.transform.InverseTransformPoint(new Vector3(x2, y2, z2));
						list.Add(item);
					}
				}
			}
		}
		return list;
	}

	// Token: 0x06001246 RID: 4678 RVA: 0x00065A3C File Offset: 0x00063E3C
	private static bool PointIsInsideMeshCollider(Collider c, Vector3 p)
	{
		Vector3[] array = new Vector3[]
		{
			Vector3.up,
			Vector3.down,
			Vector3.left,
			Vector3.right,
			Vector3.forward,
			Vector3.back
		};
		foreach (Vector3 vector in array)
		{
			RaycastHit raycastHit;
			if (!c.Raycast(new Ray(p - vector * 1000f, vector), out raycastHit, 1000f))
			{
				return false;
			}
		}
		return true;
	}

	// Token: 0x06001247 RID: 4679 RVA: 0x00065B08 File Offset: 0x00063F08
	private static void FindClosestPoints(IList<Vector3> list, out int firstIndex, out int secondIndex)
	{
		float num = float.MaxValue;
		float num2 = float.MinValue;
		firstIndex = 0;
		secondIndex = 1;
		for (int i = 0; i < list.Count - 1; i++)
		{
			for (int j = i + 1; j < list.Count; j++)
			{
				float num3 = Vector3.Distance(list[i], list[j]);
				if (num3 < num)
				{
					num = num3;
					firstIndex = i;
					secondIndex = j;
				}
				if (num3 > num2)
				{
					num2 = num3;
				}
			}
		}
	}

	// Token: 0x06001248 RID: 4680 RVA: 0x00065B8C File Offset: 0x00063F8C
	private static void WeldPoints(IList<Vector3> list, int targetCount)
	{
		if (list.Count <= 2 || targetCount < 2)
		{
			return;
		}
		while (list.Count > targetCount)
		{
			int index;
			int index2;
			Buoyancy.FindClosestPoints(list, out index, out index2);
			Vector3 item = (list[index] + list[index2]) * 0.5f;
			list.RemoveAt(index2);
			list.RemoveAt(index);
			list.Add(item);
		}
	}

	// Token: 0x06001249 RID: 4681 RVA: 0x00065BFC File Offset: 0x00063FFC
	private Vector3 GetNormal(Vector3 a, Vector3 b, Vector3 c)
	{
		Vector3 lhs = b - a;
		Vector3 rhs = c - a;
		return Vector3.Cross(lhs, rhs).normalized;
	}

	// Token: 0x0600124A RID: 4682 RVA: 0x00065C28 File Offset: 0x00064028
	private void FixedUpdate()
	{
		if (this.waterRipples == null)
		{
			return;
		}
		this.forces.Clear();
		int count = this.voxels.Count;
		Vector3[] array = new Vector3[count];
		for (int i = 0; i < count; i++)
		{
			Vector3 position = base.transform.TransformPoint(this.voxels[i]);
			array[i] = this.waterRipples.GetOffsetByPosition(position);
		}
		Vector3 normalized = (this.GetNormal(array[0], array[1], array[2]) * this.WaveVelocity + Vector3.up).normalized;
		for (int j = 0; j < count; j++)
		{
			Vector3 vector = base.transform.TransformPoint(this.voxels[j]);
			float y = array[j].y;
			if (vector.y - this.voxelHalfHeight < y)
			{
				float num = (y - vector.y) / (2f * this.voxelHalfHeight) + 0.5f;
				if (num > 1f)
				{
					num = 1f;
				}
				else if (num < 0f)
				{
					num = 0f;
				}
				Vector3 pointVelocity = base.GetComponent<Rigidbody>().GetPointVelocity(vector);
				Vector3 a = -pointVelocity * 0.1f * base.GetComponent<Rigidbody>().mass;
				Vector3 vector2 = a + Mathf.Sqrt(num) * (normalized * this.localArchimedesForce);
				base.GetComponent<Rigidbody>().AddForceAtPosition(vector2, vector);
				this.forces.Add(new Vector3[]
				{
					vector,
					vector2
				});
			}
		}
	}

	// Token: 0x0600124B RID: 4683 RVA: 0x00065E20 File Offset: 0x00064220
	private void OnDrawGizmos()
	{
		if (this.voxels == null || this.forces == null)
		{
			return;
		}
		Gizmos.color = Color.yellow;
		foreach (Vector3 position in this.voxels)
		{
			Gizmos.DrawCube(base.transform.TransformPoint(position), new Vector3(0.05f, 0.05f, 0.05f));
		}
		Gizmos.color = Color.cyan;
		foreach (Vector3[] array in this.forces)
		{
			Gizmos.DrawCube(array[0], new Vector3(0.05f, 0.05f, 0.05f));
			Gizmos.DrawLine(array[0], array[0] + array[1] / base.GetComponent<Rigidbody>().mass);
		}
	}

	// Token: 0x0600124C RID: 4684 RVA: 0x00065F6C File Offset: 0x0006436C
	private void OnTriggerEnter(Collider collidedObj)
	{
		WaterRipples component = collidedObj.GetComponent<WaterRipples>();
		if (component != null)
		{
			this.waterRipples = component;
		}
	}

	// Token: 0x0600124D RID: 4685 RVA: 0x00065F93 File Offset: 0x00064393
	private void OnEnable()
	{
		this.waterRipples = null;
	}

	// Token: 0x04000FA3 RID: 4003
	public float Density = 700f;

	// Token: 0x04000FA4 RID: 4004
	public int SlicesPerAxis = 2;

	// Token: 0x04000FA5 RID: 4005
	public bool IsConcave;

	// Token: 0x04000FA6 RID: 4006
	public int VoxelsLimit = 16;

	// Token: 0x04000FA7 RID: 4007
	public float WaveVelocity = 0.05f;

	// Token: 0x04000FA8 RID: 4008
	private const float Dampfer = 0.1f;

	// Token: 0x04000FA9 RID: 4009
	private const float WaterDensity = 1000f;

	// Token: 0x04000FAA RID: 4010
	private float voxelHalfHeight;

	// Token: 0x04000FAB RID: 4011
	private float localArchimedesForce;

	// Token: 0x04000FAC RID: 4012
	private List<Vector3> voxels;

	// Token: 0x04000FAD RID: 4013
	private bool isMeshCollider;

	// Token: 0x04000FAE RID: 4014
	private List<Vector3[]> forces;

	// Token: 0x04000FAF RID: 4015
	private WaterRipples waterRipples;
}
