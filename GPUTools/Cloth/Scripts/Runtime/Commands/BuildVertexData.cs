using System;
using GPUTools.Cloth.Scripts.Types;
using GPUTools.Common.Scripts.PL.Tools;
using GPUTools.Common.Scripts.Tools.Commands;
using UnityEngine;

namespace GPUTools.Cloth.Scripts.Runtime.Commands
{
	// Token: 0x020009A0 RID: 2464
	public class BuildVertexData : BuildChainCommand
	{
		// Token: 0x06003D84 RID: 15748 RVA: 0x0012AA56 File Offset: 0x00128E56
		public BuildVertexData(ClothSettings settings)
		{
			this.settings = settings;
		}

		// Token: 0x06003D85 RID: 15749 RVA: 0x0012AA68 File Offset: 0x00128E68
		protected override void OnBuild()
		{
			this.settings.Runtime.MeshVertexToNeiborsMap = new GpuBuffer<int>(this.settings.GeometryData.ParticleToNeibor, 4);
			this.settings.Runtime.MeshVertexToNeiborsMapCounts = new GpuBuffer<int>(this.settings.GeometryData.ParticleToNeiborCounts, 4);
			this.settings.Runtime.MeshToPhysicsVerticesMap = new GpuBuffer<int>(this.settings.GeometryData.MeshToPhysicsVerticesMap, 4);
			this.settings.Runtime.ClothVertices = new GpuBuffer<ClothVertex>(new ClothVertex[this.settings.GeometryData.MeshToPhysicsVerticesMap.Length], ClothVertex.Size());
			this.settings.Runtime.ClothOnlyVertices = new GpuBuffer<Vector3>(new Vector3[this.settings.GeometryData.MeshToPhysicsVerticesMap.Length], 12);
		}

		// Token: 0x06003D86 RID: 15750 RVA: 0x0012AB48 File Offset: 0x00128F48
		protected override void OnDispose()
		{
			this.settings.Runtime.MeshVertexToNeiborsMap.Dispose();
			this.settings.Runtime.MeshVertexToNeiborsMapCounts.Dispose();
			this.settings.Runtime.MeshToPhysicsVerticesMap.Dispose();
			this.settings.Runtime.ClothVertices.Dispose();
			this.settings.Runtime.ClothOnlyVertices.Dispose();
		}

		// Token: 0x04002F3C RID: 12092
		private readonly ClothSettings settings;
	}
}
