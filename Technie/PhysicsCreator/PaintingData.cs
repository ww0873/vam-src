using System;
using System.Collections.Generic;
using Technie.PhysicsCreator.QHull;
using UnityEngine;

namespace Technie.PhysicsCreator
{
	// Token: 0x02000456 RID: 1110
	public class PaintingData : ScriptableObject
	{
		// Token: 0x06001B86 RID: 7046 RVA: 0x0009AC10 File Offset: 0x00099010
		public PaintingData()
		{
		}

		// Token: 0x06001B87 RID: 7047 RVA: 0x0009AE34 File Offset: 0x00099234
		public void AddHull(HullType type, PhysicMaterial material, bool isTrigger)
		{
			this.hulls.Add(new Hull());
			this.hulls[this.hulls.Count - 1].name = "Hull " + this.hulls.Count;
			this.activeHull = this.hulls.Count - 1;
			this.hulls[this.hulls.Count - 1].colour = this.hullColours[this.activeHull % this.hullColours.Length];
			this.hulls[this.hulls.Count - 1].type = type;
			this.hulls[this.hulls.Count - 1].material = material;
			this.hulls[this.hulls.Count - 1].isTrigger = isTrigger;
		}

		// Token: 0x06001B88 RID: 7048 RVA: 0x0009AF31 File Offset: 0x00099331
		public void RemoveHull(int index)
		{
			this.hulls[index].Destroy();
			this.hulls.RemoveAt(index);
		}

		// Token: 0x06001B89 RID: 7049 RVA: 0x0009AF50 File Offset: 0x00099350
		public void RemoveAllHulls()
		{
			for (int i = 0; i < this.hulls.Count; i++)
			{
				this.hulls[i].Destroy();
			}
			this.hulls.Clear();
		}

		// Token: 0x06001B8A RID: 7050 RVA: 0x0009AF95 File Offset: 0x00099395
		public bool HasActiveHull()
		{
			return this.activeHull >= 0 && this.activeHull < this.hulls.Count;
		}

		// Token: 0x06001B8B RID: 7051 RVA: 0x0009AFB9 File Offset: 0x000993B9
		public Hull GetActiveHull()
		{
			if (this.activeHull < 0 || this.activeHull >= this.hulls.Count)
			{
				return null;
			}
			return this.hulls[this.activeHull];
		}

		// Token: 0x06001B8C RID: 7052 RVA: 0x0009AFF0 File Offset: 0x000993F0
		public void GenerateCollisionMesh(Hull hull, Vector3[] meshVertices, int[] meshIndices)
		{
			hull.hasColliderError = false;
			if (hull.type == HullType.Box)
			{
				if (hull.selectedFaces.Count > 0)
				{
					Vector3 vector = meshVertices[meshIndices[hull.selectedFaces[0] * 3]];
					Vector3 vector2 = vector;
					Vector3 vector3 = vector;
					for (int i = 0; i < hull.selectedFaces.Count; i++)
					{
						int num = hull.selectedFaces[i];
						Vector3 point = meshVertices[meshIndices[num * 3]];
						Vector3 point2 = meshVertices[meshIndices[num * 3 + 1]];
						Vector3 point3 = meshVertices[meshIndices[num * 3 + 2]];
						PaintingData.Inflate(point, ref vector2, ref vector3);
						PaintingData.Inflate(point2, ref vector2, ref vector3);
						PaintingData.Inflate(point3, ref vector2, ref vector3);
					}
					hull.collisionBox.center = (vector2 + vector3) * 0.5f;
					hull.collisionBox.size = vector3 - vector2;
				}
			}
			else if (hull.type == HullType.Sphere)
			{
				Vector3 center;
				float radius;
				if (this.CalculateBoundingSphere(hull, meshVertices, meshIndices, out center, out radius))
				{
					if (hull.collisionSphere == null)
					{
						hull.collisionSphere = new Sphere();
					}
					hull.collisionSphere.center = center;
					hull.collisionSphere.radius = radius;
				}
			}
			else if (hull.type == HullType.ConvexHull)
			{
				if (hull.collisionMesh == null)
				{
					hull.collisionMesh = new Mesh();
				}
				hull.collisionMesh.name = hull.name;
				hull.collisionMesh.triangles = new int[0];
				hull.collisionMesh.vertices = new Vector3[0];
				this.GenerateConvexHull(hull, meshVertices, meshIndices, hull.collisionMesh);
			}
			else if (hull.type == HullType.Face)
			{
				if (hull.faceCollisionMesh == null)
				{
					hull.faceCollisionMesh = new Mesh();
				}
				hull.faceCollisionMesh.name = hull.name;
				hull.faceCollisionMesh.triangles = new int[0];
				hull.faceCollisionMesh.vertices = new Vector3[0];
				this.GenerateFace(hull, meshVertices, meshIndices, this.faceThickness);
			}
		}

		// Token: 0x06001B8D RID: 7053 RVA: 0x0009B228 File Offset: 0x00099628
		private bool CalculateBoundingSphere(Hull hull, Vector3[] meshVertices, int[] meshIndices, out Vector3 sphereCenter, out float sphereRadius)
		{
			if (hull.selectedFaces.Count == 0)
			{
				sphereCenter = Vector3.zero;
				sphereRadius = 0f;
				return false;
			}
			List<Vector3> list = new List<Vector3>();
			for (int i = 0; i < hull.selectedFaces.Count; i++)
			{
				int num = hull.selectedFaces[i];
				Vector3 item = meshVertices[meshIndices[num * 3]];
				Vector3 item2 = meshVertices[meshIndices[num * 3 + 1]];
				Vector3 item3 = meshVertices[meshIndices[num * 3 + 2]];
				list.Add(item);
				list.Add(item2);
				list.Add(item3);
			}
			Sphere sphere = SphereUtils.MinSphere(list);
			sphereCenter = sphere.center;
			sphereRadius = sphere.radius;
			return true;
		}

		// Token: 0x06001B8E RID: 7054 RVA: 0x0009B2FC File Offset: 0x000996FC
		private void GenerateConvexHull(Hull hull, Vector3[] meshVertices, int[] meshIndices, Mesh destMesh)
		{
			int count = hull.selectedFaces.Count;
			Point3d[] array = new Point3d[count * 3];
			for (int i = 0; i < hull.selectedFaces.Count; i++)
			{
				int num = hull.selectedFaces[i];
				Vector3 vector = meshVertices[meshIndices[num * 3]];
				Vector3 vector2 = meshVertices[meshIndices[num * 3 + 1]];
				Vector3 vector3 = meshVertices[meshIndices[num * 3 + 2]];
				array[i * 3] = new Point3d((double)vector.x, (double)vector.y, (double)vector.z);
				array[i * 3 + 1] = new Point3d((double)vector2.x, (double)vector2.y, (double)vector2.z);
				array[i * 3 + 2] = new Point3d((double)vector3.x, (double)vector3.y, (double)vector3.z);
			}
			QuickHull3D quickHull3D = new QuickHull3D();
			try
			{
				quickHull3D.build(array);
			}
			catch (Exception)
			{
				Debug.LogError(string.Concat(new object[]
				{
					"Could not generate hull for ",
					base.name,
					"'s '",
					hull.name,
					"' (input ",
					array.Length,
					" points)"
				}));
			}
			Point3d[] vertices = quickHull3D.getVertices();
			int[][] faces = quickHull3D.getFaces();
			hull.numColliderFaces = faces.Length;
			Debug.Log(string.Concat(new object[]
			{
				"Calculated collider for '",
				hull.name,
				"' has ",
				faces.Length,
				" faces"
			}));
			if (faces.Length >= 256)
			{
				hull.hasColliderError = true;
				return;
			}
			Vector3[] array2 = new Vector3[vertices.Length];
			for (int j = 0; j < array2.Length; j++)
			{
				array2[j] = new Vector3((float)vertices[j].x, (float)vertices[j].y, (float)vertices[j].z);
			}
			List<int> list = new List<int>();
			for (int k = 0; k < faces.Length; k++)
			{
				int num2 = faces[k].Length;
				for (int l = 1; l < num2 - 1; l++)
				{
					list.Add(faces[k][0]);
					list.Add(faces[k][l]);
					list.Add(faces[k][l + 1]);
				}
			}
			int[] array3 = new int[list.Count];
			for (int m = 0; m < list.Count; m++)
			{
				array3[m] = list[m];
			}
			hull.collisionMesh.vertices = array2;
			hull.collisionMesh.triangles = array3;
			hull.collisionMesh.RecalculateBounds();
		}

		// Token: 0x06001B8F RID: 7055 RVA: 0x0009B5F8 File Offset: 0x000999F8
		private void GenerateFace(Hull hull, Vector3[] meshVertices, int[] meshIndices, float thickness)
		{
			int count = hull.selectedFaces.Count;
			Vector3[] array = new Vector3[count * 3 * 2];
			for (int i = 0; i < hull.selectedFaces.Count; i++)
			{
				int num = hull.selectedFaces[i];
				Vector3 vector = meshVertices[meshIndices[num * 3]];
				Vector3 vector2 = meshVertices[meshIndices[num * 3 + 1]];
				Vector3 vector3 = meshVertices[meshIndices[num * 3 + 2]];
				Vector3 normalized = (vector2 - vector).normalized;
				Vector3 normalized2 = (vector3 - vector).normalized;
				Vector3 a = Vector3.Cross(normalized2, normalized);
				int num2 = i * 3 * 2;
				array[num2] = vector;
				array[num2 + 1] = vector2;
				array[num2 + 2] = vector3;
				array[num2 + 3] = vector + a * thickness;
				array[num2 + 4] = vector2 + a * thickness;
				array[num2 + 5] = vector3 + a * thickness;
			}
			int[] array2 = new int[count * 3 * 2];
			for (int j = 0; j < array2.Length; j++)
			{
				array2[j] = j;
			}
			hull.faceCollisionMesh.vertices = array;
			hull.faceCollisionMesh.triangles = array2;
			hull.faceCollisionMesh.RecalculateBounds();
		}

		// Token: 0x06001B90 RID: 7056 RVA: 0x0009B798 File Offset: 0x00099B98
		public bool ContainsMesh(Mesh m)
		{
			foreach (Hull hull in this.hulls)
			{
				if (hull.collisionMesh == m)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06001B91 RID: 7057 RVA: 0x0009B808 File Offset: 0x00099C08
		private static void Inflate(Vector3 point, ref Vector3 min, ref Vector3 max)
		{
			min.x = Mathf.Min(min.x, point.x);
			min.y = Mathf.Min(min.y, point.y);
			min.z = Mathf.Min(min.z, point.z);
			max.x = Mathf.Max(max.x, point.x);
			max.y = Mathf.Max(max.y, point.y);
			max.z = Mathf.Max(max.z, point.z);
		}

		// Token: 0x0400178E RID: 6030
		public readonly Color[] hullColours = new Color[]
		{
			new Color(0f, 1f, 1f, 0.3f),
			new Color(1f, 0f, 1f, 0.3f),
			new Color(1f, 1f, 0f, 0.3f),
			new Color(1f, 0f, 0f, 0.3f),
			new Color(0f, 1f, 0f, 0.3f),
			new Color(0f, 0f, 1f, 0.3f),
			new Color(1f, 1f, 1f, 0.3f),
			new Color(1f, 0.5f, 0f, 0.3f),
			new Color(1f, 0f, 0.5f, 0.3f),
			new Color(0.5f, 1f, 0f, 0.3f),
			new Color(0f, 1f, 0.5f, 0.3f),
			new Color(0.5f, 0f, 1f, 0.3f),
			new Color(0f, 0.5f, 1f, 0.3f)
		};

		// Token: 0x0400178F RID: 6031
		public HullData hullData;

		// Token: 0x04001790 RID: 6032
		public Mesh sourceMesh;

		// Token: 0x04001791 RID: 6033
		public int activeHull = -1;

		// Token: 0x04001792 RID: 6034
		public float faceThickness = 0.1f;

		// Token: 0x04001793 RID: 6035
		public List<Hull> hulls = new List<Hull>();
	}
}
