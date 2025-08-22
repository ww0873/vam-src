using System;
using System.Collections.Generic;
using UnityEngine;

namespace GPUTools.Common.Scripts.Tools.SpatialSearch
{
	// Token: 0x020009D7 RID: 2519
	public class SearchVoxel
	{
		// Token: 0x06003F88 RID: 16264 RVA: 0x0012F2C9 File Offset: 0x0012D6C9
		public SearchVoxel(List<Vector3> vertices, Bounds bounds)
		{
			this.bounds = bounds;
			this.vertices = vertices;
			this.mineIndices = this.SearchMineIndices();
		}

		// Token: 0x06003F89 RID: 16265 RVA: 0x0012F2EC File Offset: 0x0012D6EC
		private List<int> SearchMineIndices()
		{
			List<int> list = new List<int>();
			for (int i = 0; i < this.vertices.Count; i++)
			{
				Vector3 point = this.vertices[i];
				if (this.bounds.Contains(point))
				{
					list.Add(i);
				}
			}
			return list;
		}

		// Token: 0x06003F8A RID: 16266 RVA: 0x0012F344 File Offset: 0x0012D744
		public List<int> SearchInSphere(Vector3 center, float radius)
		{
			List<int> list = new List<int>();
			foreach (int num in this.mineIndices)
			{
				Vector3 a = this.vertices[num];
				if ((a - center).sqrMagnitude < radius * radius)
				{
					list.Add(num);
				}
			}
			return list;
		}

		// Token: 0x06003F8B RID: 16267 RVA: 0x0012F3CC File Offset: 0x0012D7CC
		public List<int> SearchInSphere(Ray ray, float radius)
		{
			List<int> list = new List<int>();
			foreach (int num in this.mineIndices)
			{
				Vector3 a = this.vertices[num];
				float sqrMagnitude = Vector3.Cross(ray.direction, a - ray.origin).sqrMagnitude;
				if (sqrMagnitude < radius * radius)
				{
					list.Add(num);
				}
			}
			return list;
		}

		// Token: 0x06003F8C RID: 16268 RVA: 0x0012F46C File Offset: 0x0012D86C
		public void DebugDraw(Transform transforms)
		{
		}

		// Token: 0x17000770 RID: 1904
		// (get) Token: 0x06003F8D RID: 16269 RVA: 0x0012F46E File Offset: 0x0012D86E
		public Bounds Bounds
		{
			get
			{
				return this.bounds;
			}
		}

		// Token: 0x17000771 RID: 1905
		// (get) Token: 0x06003F8E RID: 16270 RVA: 0x0012F476 File Offset: 0x0012D876
		public int TotalVertices
		{
			get
			{
				return this.mineIndices.Count;
			}
		}

		// Token: 0x04003018 RID: 12312
		private readonly List<Vector3> vertices;

		// Token: 0x04003019 RID: 12313
		private readonly List<int> mineIndices;

		// Token: 0x0400301A RID: 12314
		private readonly Bounds bounds;
	}
}
