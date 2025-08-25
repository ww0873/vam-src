using System;
using System.Collections.Generic;
using UnityEngine;

namespace Technie.PhysicsCreator
{
	// Token: 0x02000453 RID: 1107
	public class HullPainter : MonoBehaviour
	{
		// Token: 0x06001B78 RID: 7032 RVA: 0x0009A2DF File Offset: 0x000986DF
		public HullPainter()
		{
		}

		// Token: 0x06001B79 RID: 7033 RVA: 0x0009A2E7 File Offset: 0x000986E7
		private void OnDestroy()
		{
		}

		// Token: 0x06001B7A RID: 7034 RVA: 0x0009A2EC File Offset: 0x000986EC
		public void CreateColliderComponents()
		{
			this.CreateHullMapping();
			foreach (Hull hull in this.paintingData.hulls)
			{
				this.CreateColliderComponent(hull);
			}
		}

		// Token: 0x06001B7B RID: 7035 RVA: 0x0009A354 File Offset: 0x00098754
		public void RemoveAllColliders()
		{
			this.CreateHullMapping();
			foreach (Collider obj in this.hullMapping.Values)
			{
				UnityEngine.Object.DestroyImmediate(obj);
			}
			this.hullMapping.Clear();
		}

		// Token: 0x06001B7C RID: 7036 RVA: 0x0009A3C8 File Offset: 0x000987C8
		private void CreateHullMapping()
		{
			if (this.hullMapping == null)
			{
				this.hullMapping = new Dictionary<Hull, Collider>();
			}
			List<Hull> list = new List<Hull>(this.hullMapping.Keys);
			foreach (Hull hull in list)
			{
				if (hull == null || this.hullMapping[hull] == null)
				{
					Debug.Log("Removing invalid entry from hull mapping");
					this.hullMapping.Remove(hull);
				}
			}
			foreach (Hull hull2 in this.paintingData.hulls)
			{
				if (this.hullMapping.ContainsKey(hull2))
				{
					Collider collider = this.hullMapping[hull2];
					bool flag = hull2.type == HullType.ConvexHull && collider is MeshCollider;
					bool flag2 = hull2.type == HullType.Box && collider is BoxCollider;
					bool flag3 = hull2.type == HullType.Sphere && collider is SphereCollider;
					bool flag4 = hull2.type == HullType.Face && collider is MeshCollider;
					if (!flag && !flag2 && !flag3 && !flag4)
					{
						UnityEngine.Object.DestroyImmediate(collider);
						this.hullMapping.Remove(hull2);
					}
				}
			}
			List<Hull> list2 = new List<Hull>();
			List<Collider> list3 = new List<Collider>();
			foreach (Hull hull3 in this.paintingData.hulls)
			{
				if (!this.hullMapping.ContainsKey(hull3))
				{
					list2.Add(hull3);
				}
			}
			foreach (Collider collider2 in base.GetComponents<Collider>())
			{
				if (!this.hullMapping.ContainsValue(collider2))
				{
					list3.Add(collider2);
				}
			}
			for (int j = list2.Count - 1; j >= 0; j--)
			{
				Hull hull4 = list2[j];
				for (int k = list3.Count - 1; k >= 0; k--)
				{
					Collider collider3 = list3[k];
					BoxCollider boxCollider = collider3 as BoxCollider;
					SphereCollider sphereCollider = collider3 as SphereCollider;
					MeshCollider meshCollider = collider3 as MeshCollider;
					bool flag5 = hull4.type == HullType.Box && collider3 is BoxCollider && HullPainter.Approximately(hull4.collisionBox.center, boxCollider.center) && HullPainter.Approximately(hull4.collisionBox.size, boxCollider.size);
					bool flag6 = hull4.type == HullType.Sphere && collider3 is SphereCollider && hull4.collisionSphere != null && HullPainter.Approximately(hull4.collisionSphere.center, sphereCollider.center) && HullPainter.Approximately(hull4.collisionSphere.radius, sphereCollider.radius);
					bool flag7 = hull4.type == HullType.ConvexHull && collider3 is MeshCollider && meshCollider.sharedMesh == hull4.collisionMesh;
					bool flag8 = hull4.type == HullType.Face && collider3 is MeshCollider && meshCollider.sharedMesh == hull4.faceCollisionMesh;
					if (flag5 || flag6 || flag7 || flag8)
					{
						this.hullMapping.Add(hull4, collider3);
						list2.RemoveAt(j);
						list3.RemoveAt(k);
						break;
					}
				}
			}
			foreach (Hull hull5 in list2)
			{
				if (hull5.type == HullType.Box)
				{
					BoxCollider value = base.gameObject.AddComponent<BoxCollider>();
					this.hullMapping.Add(hull5, value);
				}
				else if (hull5.type == HullType.Sphere)
				{
					SphereCollider value2 = base.gameObject.AddComponent<SphereCollider>();
					this.hullMapping.Add(hull5, value2);
				}
				else if (hull5.type == HullType.ConvexHull)
				{
					MeshCollider value3 = base.gameObject.AddComponent<MeshCollider>();
					this.hullMapping.Add(hull5, value3);
				}
				else if (hull5.type == HullType.Face)
				{
					MeshCollider value4 = base.gameObject.AddComponent<MeshCollider>();
					this.hullMapping.Add(hull5, value4);
				}
			}
			foreach (Collider obj in list3)
			{
				UnityEngine.Object.DestroyImmediate(obj);
			}
		}

		// Token: 0x06001B7D RID: 7037 RVA: 0x0009A930 File Offset: 0x00098D30
		private static bool Approximately(Vector3 lhs, Vector3 rhs)
		{
			return Mathf.Approximately(lhs.x, rhs.x) && Mathf.Approximately(lhs.y, rhs.y) && Mathf.Approximately(lhs.z, rhs.z);
		}

		// Token: 0x06001B7E RID: 7038 RVA: 0x0009A983 File Offset: 0x00098D83
		private static bool Approximately(float lhs, float rhs)
		{
			return Mathf.Approximately(lhs, rhs);
		}

		// Token: 0x06001B7F RID: 7039 RVA: 0x0009A98C File Offset: 0x00098D8C
		private void CreateColliderComponent(Hull hull)
		{
			Collider collider = null;
			if (hull.type == HullType.Box)
			{
				BoxCollider boxCollider = this.hullMapping[hull] as BoxCollider;
				boxCollider.center = hull.collisionBox.center;
				boxCollider.size = hull.collisionBox.size;
				collider = boxCollider;
			}
			else if (hull.type == HullType.Sphere)
			{
				SphereCollider sphereCollider = this.hullMapping[hull] as SphereCollider;
				sphereCollider.center = hull.collisionSphere.center;
				sphereCollider.radius = hull.collisionSphere.radius;
				collider = sphereCollider;
			}
			else if (hull.type == HullType.ConvexHull)
			{
				MeshCollider meshCollider = this.hullMapping[hull] as MeshCollider;
				meshCollider.sharedMesh = hull.collisionMesh;
				meshCollider.convex = true;
				collider = meshCollider;
			}
			else if (hull.type == HullType.Face)
			{
				MeshCollider meshCollider2 = this.hullMapping[hull] as MeshCollider;
				meshCollider2.sharedMesh = hull.faceCollisionMesh;
				meshCollider2.convex = true;
				collider = meshCollider2;
			}
			collider.material = hull.material;
			collider.isTrigger = hull.isTrigger;
		}

		// Token: 0x06001B80 RID: 7040 RVA: 0x0009AAB0 File Offset: 0x00098EB0
		public void SetAllTypes(HullType newType)
		{
			foreach (Hull hull in this.paintingData.hulls)
			{
				hull.type = newType;
			}
		}

		// Token: 0x06001B81 RID: 7041 RVA: 0x0009AB14 File Offset: 0x00098F14
		public void SetAllMaterials(PhysicMaterial newMaterial)
		{
			foreach (Hull hull in this.paintingData.hulls)
			{
				hull.material = newMaterial;
			}
		}

		// Token: 0x06001B82 RID: 7042 RVA: 0x0009AB78 File Offset: 0x00098F78
		public void SetAllAsTrigger(bool isTrigger)
		{
			foreach (Hull hull in this.paintingData.hulls)
			{
				hull.isTrigger = isTrigger;
			}
		}

		// Token: 0x06001B83 RID: 7043 RVA: 0x0009ABDC File Offset: 0x00098FDC
		public void OnDrawGizmosSelected()
		{
		}

		// Token: 0x0400177A RID: 6010
		public PaintingData paintingData;

		// Token: 0x0400177B RID: 6011
		public HullData hullData;

		// Token: 0x0400177C RID: 6012
		public Dictionary<Hull, Collider> hullMapping;
	}
}
