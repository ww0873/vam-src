using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using GPUTools.Common.Scripts.PL.Abstract;
using GPUTools.Common.Scripts.PL.Attributes;
using GPUTools.Common.Scripts.PL.Tools;
using GPUTools.Physics.Scripts.Types.Dynamic;
using UnityEngine;

namespace GPUTools.Cloth.Scripts.Runtime.Kernels
{
	// Token: 0x020009A5 RID: 2469
	public class CreateVertexOnlyDataKernel : KernelBase
	{
		// Token: 0x06003DF4 RID: 15860 RVA: 0x0012B593 File Offset: 0x00129993
		public CreateVertexOnlyDataKernel() : base("Compute/CreateVertexOnlyData", "CSCreateVertexOnlyData")
		{
		}

		// Token: 0x17000717 RID: 1815
		// (get) Token: 0x06003DF6 RID: 15862 RVA: 0x0012B5AE File Offset: 0x001299AE
		// (set) Token: 0x06003DF5 RID: 15861 RVA: 0x0012B5A5 File Offset: 0x001299A5
		[GpuData("particles")]
		public GpuBuffer<GPParticle> Particles
		{
			[CompilerGenerated]
			get
			{
				return this.<Particles>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<Particles>k__BackingField = value;
			}
		}

		// Token: 0x17000718 RID: 1816
		// (get) Token: 0x06003DF7 RID: 15863 RVA: 0x0012B5B6 File Offset: 0x001299B6
		// (set) Token: 0x06003DF8 RID: 15864 RVA: 0x0012B5BE File Offset: 0x001299BE
		[GpuData("clothOnlyVertices")]
		public GpuBuffer<Vector3> ClothOnlyVertices
		{
			[CompilerGenerated]
			get
			{
				return this.<ClothOnlyVertices>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<ClothOnlyVertices>k__BackingField = value;
			}
		}

		// Token: 0x17000719 RID: 1817
		// (get) Token: 0x06003DF9 RID: 15865 RVA: 0x0012B5C7 File Offset: 0x001299C7
		// (set) Token: 0x06003DFA RID: 15866 RVA: 0x0012B5CF File Offset: 0x001299CF
		[GpuData("meshToPhysicsVerticesMap")]
		public GpuBuffer<int> MeshToPhysicsVerticesMap
		{
			[CompilerGenerated]
			get
			{
				return this.<MeshToPhysicsVerticesMap>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<MeshToPhysicsVerticesMap>k__BackingField = value;
			}
		}

		// Token: 0x06003DFB RID: 15867 RVA: 0x0012B5D8 File Offset: 0x001299D8
		public override int GetGroupsNumX()
		{
			if (this.MeshToPhysicsVerticesMap != null)
			{
				return Mathf.CeilToInt((float)this.MeshToPhysicsVerticesMap.ComputeBuffer.count / 256f);
			}
			return 0;
		}

		// Token: 0x04002F5B RID: 12123
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private GpuBuffer<GPParticle> <Particles>k__BackingField;

		// Token: 0x04002F5C RID: 12124
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private GpuBuffer<Vector3> <ClothOnlyVertices>k__BackingField;

		// Token: 0x04002F5D RID: 12125
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private GpuBuffer<int> <MeshToPhysicsVerticesMap>k__BackingField;
	}
}
