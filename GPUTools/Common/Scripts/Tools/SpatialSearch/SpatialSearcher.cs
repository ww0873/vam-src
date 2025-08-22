using System;
using System.Collections.Generic;
using UnityEngine;

namespace GPUTools.Common.Scripts.Tools.SpatialSearch
{
	// Token: 0x020009D8 RID: 2520
	public class SpatialSearcher
	{
		// Token: 0x06003F8F RID: 16271 RVA: 0x0012F483 File Offset: 0x0012D883
		public SpatialSearcher(List<Vector3> vertices, Bounds bounds, int splitX, int splitY, int splitZ)
		{
			this.vertices = vertices;
			this.bounds = bounds;
			this.voxels = this.CreateVoxels(splitX, splitY, splitZ);
			this.fixedList = new FixedList<int>(vertices.Count);
		}

		// Token: 0x06003F90 RID: 16272 RVA: 0x0012F4BC File Offset: 0x0012D8BC
		private List<SearchVoxel> CreateVoxels(int splitX, int splitY, int splitZ)
		{
			Vector3 size = new Vector3(this.bounds.size.x / (float)splitX, this.bounds.size.y / (float)splitY, this.bounds.size.z / (float)splitZ);
			List<SearchVoxel> list = new List<SearchVoxel>();
			for (int i = 0; i <= splitX; i++)
			{
				for (int j = 0; j <= splitY; j++)
				{
					for (int k = 0; k <= splitZ; k++)
					{
						Vector3 center = this.bounds.center + new Vector3(size.x * (float)i, size.y * (float)j, size.z * (float)k) - this.bounds.size * 0.5f;
						Bounds bounds = new Bounds(center, size);
						SearchVoxel searchVoxel = new SearchVoxel(this.vertices, bounds);
						if (searchVoxel.TotalVertices > 0)
						{
							list.Add(searchVoxel);
						}
					}
				}
			}
			return list;
		}

		// Token: 0x06003F91 RID: 16273 RVA: 0x0012F5F4 File Offset: 0x0012D9F4
		public FixedList<int> SearchInSphereSlow(Vector3 center, float radius)
		{
			this.fixedList.Reset();
			for (int i = 0; i < this.vertices.Count; i++)
			{
				Vector3 b = this.vertices[i];
				if ((center - b).sqrMagnitude < radius * radius)
				{
					this.fixedList.Add(i);
				}
			}
			return this.fixedList;
		}

		// Token: 0x06003F92 RID: 16274 RVA: 0x0012F660 File Offset: 0x0012DA60
		public List<int> SearchInSphereSlow(Ray ray, float radius)
		{
			List<int> list = new List<int>();
			float num = radius * radius;
			for (int i = 0; i < this.vertices.Count; i++)
			{
				Vector3 a = this.vertices[i];
				float sqrMagnitude = Vector3.Cross(ray.direction, a - ray.origin).sqrMagnitude;
				if (sqrMagnitude < num)
				{
					list.Add(i);
				}
			}
			return list;
		}

		// Token: 0x06003F93 RID: 16275 RVA: 0x0012F6D4 File Offset: 0x0012DAD4
		public List<int> SearchInSphere(Vector3 center, float radius)
		{
			List<SearchVoxel> list = this.SearchVoxelsInSphere(center, radius);
			List<int> list2 = new List<int>();
			foreach (SearchVoxel searchVoxel in list)
			{
				list2.AddRange(searchVoxel.SearchInSphere(center, radius));
			}
			return list2;
		}

		// Token: 0x06003F94 RID: 16276 RVA: 0x0012F744 File Offset: 0x0012DB44
		public List<int> SearchInSphere(Ray ray, float radius)
		{
			List<SearchVoxel> list = this.SearchVoxelsInSphere(ray, radius);
			List<int> list2 = new List<int>();
			foreach (SearchVoxel searchVoxel in list)
			{
				list2.AddRange(searchVoxel.SearchInSphere(ray, radius));
			}
			return list2;
		}

		// Token: 0x06003F95 RID: 16277 RVA: 0x0012F7B4 File Offset: 0x0012DBB4
		private List<SearchVoxel> SearchVoxelsInSphere(Vector3 center, float radius)
		{
			List<SearchVoxel> list = new List<SearchVoxel>();
			foreach (SearchVoxel searchVoxel in this.voxels)
			{
				Vector3 a = searchVoxel.Bounds.ClosestPoint(center);
				if ((a - center).sqrMagnitude < radius * radius)
				{
					list.Add(searchVoxel);
				}
			}
			return list;
		}

		// Token: 0x06003F96 RID: 16278 RVA: 0x0012F840 File Offset: 0x0012DC40
		private List<SearchVoxel> SearchVoxelsInSphere(Ray ray, float radius)
		{
			List<SearchVoxel> list = new List<SearchVoxel>();
			foreach (SearchVoxel searchVoxel in this.voxels)
			{
				if (searchVoxel.Bounds.IntersectRay(ray))
				{
					list.Add(searchVoxel);
				}
			}
			return list;
		}

		// Token: 0x06003F97 RID: 16279 RVA: 0x0012F8B8 File Offset: 0x0012DCB8
		public void DebugDraw(Transform transform)
		{
			foreach (SearchVoxel searchVoxel in this.voxels)
			{
				searchVoxel.DebugDraw(transform);
			}
		}

		// Token: 0x0400301B RID: 12315
		private readonly List<SearchVoxel> voxels;

		// Token: 0x0400301C RID: 12316
		private readonly List<Vector3> vertices;

		// Token: 0x0400301D RID: 12317
		private readonly Bounds bounds;

		// Token: 0x0400301E RID: 12318
		private FixedList<int> fixedList;
	}
}
