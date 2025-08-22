using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using GPUTools.Cloth.Scripts.Geometry.Data;
using GPUTools.Common.Scripts.Tools.Commands;
using UnityEngine;

namespace GPUTools.Cloth.Scripts.Geometry.Passes
{
	// Token: 0x02000990 RID: 2448
	public class NeighborsPass : ICacheCommand
	{
		// Token: 0x06003D20 RID: 15648 RVA: 0x00128D8F File Offset: 0x0012718F
		public NeighborsPass(ClothSettings settings)
		{
			this.mesh = settings.MeshProvider.MeshForImport;
			this.data = settings.GeometryData;
		}

		// Token: 0x06003D21 RID: 15649 RVA: 0x00128DB4 File Offset: 0x001271B4
		public void Cache()
		{
			if (this.data != null && this.mesh != null)
			{
				List<int>[] array = this.CreateAllTrianglesList(this.data.AllTringles, this.data.MeshToPhysicsVerticesMap);
				this.FillStrandedVertices(array);
				this.SortList(array);
				this.data.ParticleToNeiborCounts = this.CreateConts(array);
				this.data.ParticleToNeibor = this.ConvertTo1DArray(array);
			}
		}

		// Token: 0x06003D22 RID: 15650 RVA: 0x00128E2C File Offset: 0x0012722C
		private List<int>[] CreateAllTrianglesList(int[] triangles, int[] meshToPhysicsVerticesMap)
		{
			List<int>[] array = new List<int>[this.data.Particles.Length];
			for (int i = 0; i < triangles.Length; i += 3)
			{
				int num = triangles[i];
				int num2 = triangles[i + 1];
				int num3 = triangles[i + 2];
				int num4 = meshToPhysicsVerticesMap[num];
				int num5 = meshToPhysicsVerticesMap[num2];
				int num6 = meshToPhysicsVerticesMap[num3];
				this.Add(array, num4, num5, num6);
				this.Add(array, num5, num6, num4);
				this.Add(array, num6, num4, num5);
			}
			return array;
		}

		// Token: 0x06003D23 RID: 15651 RVA: 0x00128EAC File Offset: 0x001272AC
		private void FillStrandedVertices(List<int>[] physList)
		{
			for (int i = 0; i < physList.Length; i++)
			{
				if (physList[i] == null)
				{
					physList[i] = new List<int>();
				}
			}
		}

		// Token: 0x06003D24 RID: 15652 RVA: 0x00128EE0 File Offset: 0x001272E0
		private void Add(List<int>[] physList, int p, int p1, int p2)
		{
			if (physList[p] == null)
			{
				physList[p] = new List<int>();
			}
			if (!physList[p].Contains(p1))
			{
				physList[p].Add(p1);
			}
			if (!physList[p].Contains(p2))
			{
				physList[p].Add(p2);
			}
		}

		// Token: 0x06003D25 RID: 15653 RVA: 0x00128F30 File Offset: 0x00127330
		private int[] CreateConts(List<int>[] list)
		{
			int[] array = new int[list.Length + 1];
			int num = 0;
			for (int i = 1; i < list.Length + 1; i++)
			{
				num += list[i - 1].Count;
				array[i] = num;
			}
			return array;
		}

		// Token: 0x06003D26 RID: 15654 RVA: 0x00128F74 File Offset: 0x00127374
		private void SortList(List<int>[] list)
		{
			Vector3[] normals = this.mesh.normals;
			for (int i = 0; i < list.Length; i++)
			{
				NeighborsPass.<SortList>c__AnonStorey0 <SortList>c__AnonStorey = new NeighborsPass.<SortList>c__AnonStorey0();
				<SortList>c__AnonStorey.$this = this;
				List<int> list2 = list[i];
				<SortList>c__AnonStorey.normal = normals[this.data.PhysicsToMeshVerticesMap[i]];
				<SortList>c__AnonStorey.vertex = this.data.Particles[i];
				list2.Sort(new Comparison<int>(<SortList>c__AnonStorey.<>m__0));
			}
		}

		// Token: 0x06003D27 RID: 15655 RVA: 0x00128FFE File Offset: 0x001273FE
		private int[] ConvertTo1DArray(List<int>[] list)
		{
			if (NeighborsPass.<>f__am$cache0 == null)
			{
				NeighborsPass.<>f__am$cache0 = new Func<List<int>, IEnumerable<int>>(NeighborsPass.<ConvertTo1DArray>m__0);
			}
			return list.SelectMany(NeighborsPass.<>f__am$cache0).ToArray<int>();
		}

		// Token: 0x06003D28 RID: 15656 RVA: 0x00129028 File Offset: 0x00127428
		[CompilerGenerated]
		private static IEnumerable<int> <ConvertTo1DArray>m__0(List<int> neibors)
		{
			return neibors;
		}

		// Token: 0x04002F09 RID: 12041
		private readonly ClothGeometryData data;

		// Token: 0x04002F0A RID: 12042
		private readonly Mesh mesh;

		// Token: 0x04002F0B RID: 12043
		[CompilerGenerated]
		private static Func<List<int>, IEnumerable<int>> <>f__am$cache0;

		// Token: 0x02000FBB RID: 4027
		[CompilerGenerated]
		private sealed class <SortList>c__AnonStorey0
		{
			// Token: 0x060074F8 RID: 29944 RVA: 0x0012902B File Offset: 0x0012742B
			public <SortList>c__AnonStorey0()
			{
			}

			// Token: 0x060074F9 RID: 29945 RVA: 0x00129034 File Offset: 0x00127434
			internal int <>m__0(int i1, int i2)
			{
				Vector3 lhs = this.$this.data.Particles[i1] - this.vertex;
				Vector3 rhs = this.$this.data.Particles[i2] - this.vertex;
				Vector3 rhs2 = Vector3.Cross(lhs, rhs);
				return (int)Mathf.Sign(Vector3.Dot(this.normal, rhs2));
			}

			// Token: 0x0400690F RID: 26895
			internal Vector3 vertex;

			// Token: 0x04006910 RID: 26896
			internal Vector3 normal;

			// Token: 0x04006911 RID: 26897
			internal NeighborsPass $this;
		}
	}
}
