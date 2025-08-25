using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using GPUTools.Cloth.Scripts.Geometry.Data;
using GPUTools.Common.Scripts.Tools.Commands;
using UnityEngine;

namespace GPUTools.Cloth.Scripts.Geometry.Passes
{
	// Token: 0x02000991 RID: 2449
	public class NeighborsPass2 : ICacheCommand
	{
		// Token: 0x06003D29 RID: 15657 RVA: 0x001290A9 File Offset: 0x001274A9
		public NeighborsPass2(ClothSettings settings)
		{
			this.mesh = settings.MeshProvider.MeshForImport;
			this.data = settings.GeometryData;
		}

		// Token: 0x06003D2A RID: 15658 RVA: 0x001290D0 File Offset: 0x001274D0
		public void Cache()
		{
			List<int>[] list = this.CreateAllTrianglesList(this.data.AllTringles, this.data.MeshToPhysicsVerticesMap);
			this.SortList(list);
			this.data.ParticleToNeiborCounts = this.CreateConts(list);
			this.data.ParticleToNeibor = this.ConvertTo1DArray(list);
		}

		// Token: 0x06003D2B RID: 15659 RVA: 0x00129128 File Offset: 0x00127528
		private List<int>[] CreateAllTrianglesList(int[] triangles, int[] meshToPhysicsVerticesMap)
		{
			List<int>[] array = new List<int>[this.data.MeshToPhysicsVerticesMap.Length];
			for (int i = 0; i < triangles.Length; i += 3)
			{
				int num = triangles[i];
				int num2 = triangles[i + 1];
				int num3 = triangles[i + 2];
				int num4 = meshToPhysicsVerticesMap[num];
				int num5 = meshToPhysicsVerticesMap[num2];
				int num6 = meshToPhysicsVerticesMap[num3];
				this.Add(array, num, num5, num6);
				this.Add(array, num2, num6, num4);
				this.Add(array, num3, num4, num5);
			}
			return array;
		}

		// Token: 0x06003D2C RID: 15660 RVA: 0x001291A4 File Offset: 0x001275A4
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

		// Token: 0x06003D2D RID: 15661 RVA: 0x001291F4 File Offset: 0x001275F4
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

		// Token: 0x06003D2E RID: 15662 RVA: 0x00129238 File Offset: 0x00127638
		private void SortList(List<int>[] list)
		{
			Vector3[] vertices = this.mesh.vertices;
			Vector3[] normals = this.mesh.normals;
			for (int i = 0; i < list.Length; i++)
			{
				NeighborsPass2.<SortList>c__AnonStorey0 <SortList>c__AnonStorey = new NeighborsPass2.<SortList>c__AnonStorey0();
				<SortList>c__AnonStorey.$this = this;
				List<int> list2 = list[i];
				<SortList>c__AnonStorey.normal = normals[i];
				<SortList>c__AnonStorey.vertex = vertices[i];
				list2.Sort(new Comparison<int>(<SortList>c__AnonStorey.<>m__0));
			}
		}

		// Token: 0x06003D2F RID: 15663 RVA: 0x001292BA File Offset: 0x001276BA
		private int[] ConvertTo1DArray(List<int>[] list)
		{
			if (NeighborsPass2.<>f__am$cache0 == null)
			{
				NeighborsPass2.<>f__am$cache0 = new Func<List<int>, IEnumerable<int>>(NeighborsPass2.<ConvertTo1DArray>m__0);
			}
			return list.SelectMany(NeighborsPass2.<>f__am$cache0).ToArray<int>();
		}

		// Token: 0x06003D30 RID: 15664 RVA: 0x001292E4 File Offset: 0x001276E4
		[CompilerGenerated]
		private static IEnumerable<int> <ConvertTo1DArray>m__0(List<int> neibors)
		{
			return neibors;
		}

		// Token: 0x04002F0C RID: 12044
		private readonly ClothGeometryData data;

		// Token: 0x04002F0D RID: 12045
		private readonly Mesh mesh;

		// Token: 0x04002F0E RID: 12046
		[CompilerGenerated]
		private static Func<List<int>, IEnumerable<int>> <>f__am$cache0;

		// Token: 0x02000FBC RID: 4028
		[CompilerGenerated]
		private sealed class <SortList>c__AnonStorey0
		{
			// Token: 0x060074FA RID: 29946 RVA: 0x001292E7 File Offset: 0x001276E7
			public <SortList>c__AnonStorey0()
			{
			}

			// Token: 0x060074FB RID: 29947 RVA: 0x001292F0 File Offset: 0x001276F0
			internal int <>m__0(int i1, int i2)
			{
				Vector3 lhs = this.$this.data.Particles[i1] - this.vertex;
				Vector3 rhs = this.$this.data.Particles[i2] - this.vertex;
				Vector3 rhs2 = Vector3.Cross(lhs, rhs);
				return (int)Mathf.Sign(Vector3.Dot(this.normal, rhs2));
			}

			// Token: 0x04006912 RID: 26898
			internal Vector3 vertex;

			// Token: 0x04006913 RID: 26899
			internal Vector3 normal;

			// Token: 0x04006914 RID: 26900
			internal NeighborsPass2 $this;
		}
	}
}
