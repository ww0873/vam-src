using System;
using System.Collections.Generic;
using System.Linq;
using GPUTools.Cloth.Scripts.Geometry.Data;
using GPUTools.Common.Scripts.Tools.Commands;
using UnityEngine;

namespace GPUTools.Cloth.Scripts.Geometry.Passes
{
	// Token: 0x02000992 RID: 2450
	public class PhysicsVerticesPass : ICacheCommand
	{
		// Token: 0x06003D31 RID: 15665 RVA: 0x00129365 File Offset: 0x00127765
		public PhysicsVerticesPass(ClothSettings settings)
		{
			this.data = settings.GeometryData;
			this.mesh = settings.MeshProvider.MeshForImport;
		}

		// Token: 0x06003D32 RID: 15666 RVA: 0x0012938C File Offset: 0x0012778C
		public void Cache()
		{
			if (this.data != null && this.mesh != null)
			{
				Vector3[] vertices = this.mesh.vertices;
				this.data.Particles = this.CreatePhysicsVerticesArray(vertices);
				this.CreateMeshToPhysicsVerticesMap(vertices, this.data.Particles);
				this.data.MeshToPhysicsVerticesMap = this.meshToPhysicsVerticesMap;
				this.data.PhysicsToMeshVerticesMap = this.physicsToMeshVerticesMap;
			}
		}

		// Token: 0x06003D33 RID: 15667 RVA: 0x00129408 File Offset: 0x00127808
		private Vector3[] CreatePhysicsVerticesArray(Vector3[] vertices)
		{
			HashSet<Vector3> hashSet = new HashSet<Vector3>();
			for (int i = 0; i < vertices.Length; i++)
			{
				hashSet.Add(vertices[i]);
			}
			return hashSet.ToArray<Vector3>();
		}

		// Token: 0x06003D34 RID: 15668 RVA: 0x00129448 File Offset: 0x00127848
		private void CreateMeshToPhysicsVerticesMap(Vector3[] vertices, Vector3[] physicsVertices)
		{
			Dictionary<Vector3, int> dictionary = new Dictionary<Vector3, int>();
			for (int i = 0; i < physicsVertices.Length; i++)
			{
				Vector3 key = physicsVertices[i];
				dictionary.Add(key, i);
			}
			this.meshToPhysicsVerticesMap = new int[vertices.Length];
			this.physicsToMeshVerticesMap = new int[physicsVertices.Length];
			for (int j = 0; j < vertices.Length; j++)
			{
				Vector3 key2 = vertices[j];
				if (dictionary.ContainsKey(key2))
				{
					int num = dictionary[key2];
					this.meshToPhysicsVerticesMap[j] = num;
					this.physicsToMeshVerticesMap[num] = j;
				}
				else
				{
					this.meshToPhysicsVerticesMap[j] = -1;
				}
			}
		}

		// Token: 0x04002F0F RID: 12047
		private readonly Mesh mesh;

		// Token: 0x04002F10 RID: 12048
		private readonly ClothGeometryData data;

		// Token: 0x04002F11 RID: 12049
		private int[] meshToPhysicsVerticesMap;

		// Token: 0x04002F12 RID: 12050
		private int[] physicsToMeshVerticesMap;
	}
}
