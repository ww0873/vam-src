using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using GPUTools.Cloth.Scripts.Types;
using GPUTools.Common.Scripts.PL.Abstract;
using GPUTools.Common.Scripts.PL.Attributes;
using GPUTools.Common.Scripts.PL.Tools;
using GPUTools.Physics.Scripts.Types.Dynamic;
using UnityEngine;

namespace GPUTools.Cloth.Scripts.Runtime.Kernels
{
	// Token: 0x020009A4 RID: 2468
	public class CreateVertexDataKernel : KernelBase
	{
		// Token: 0x06003DE6 RID: 15846 RVA: 0x0012B4F0 File Offset: 0x001298F0
		public CreateVertexDataKernel() : base("Compute/CreateVertexData", "CSCreateVertexData")
		{
		}

		// Token: 0x17000711 RID: 1809
		// (get) Token: 0x06003DE8 RID: 15848 RVA: 0x0012B50B File Offset: 0x0012990B
		// (set) Token: 0x06003DE7 RID: 15847 RVA: 0x0012B502 File Offset: 0x00129902
		[GpuData("facesForNormalNum")]
		public GpuValue<int> FacesForNormalNum
		{
			[CompilerGenerated]
			get
			{
				return this.<FacesForNormalNum>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<FacesForNormalNum>k__BackingField = value;
			}
		}

		// Token: 0x17000712 RID: 1810
		// (get) Token: 0x06003DEA RID: 15850 RVA: 0x0012B51C File Offset: 0x0012991C
		// (set) Token: 0x06003DE9 RID: 15849 RVA: 0x0012B513 File Offset: 0x00129913
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

		// Token: 0x17000713 RID: 1811
		// (get) Token: 0x06003DEB RID: 15851 RVA: 0x0012B524 File Offset: 0x00129924
		// (set) Token: 0x06003DEC RID: 15852 RVA: 0x0012B52C File Offset: 0x0012992C
		[GpuData("clothVertices")]
		public GpuBuffer<ClothVertex> ClothVertices
		{
			[CompilerGenerated]
			get
			{
				return this.<ClothVertices>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<ClothVertices>k__BackingField = value;
			}
		}

		// Token: 0x17000714 RID: 1812
		// (get) Token: 0x06003DED RID: 15853 RVA: 0x0012B535 File Offset: 0x00129935
		// (set) Token: 0x06003DEE RID: 15854 RVA: 0x0012B53D File Offset: 0x0012993D
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

		// Token: 0x17000715 RID: 1813
		// (get) Token: 0x06003DEF RID: 15855 RVA: 0x0012B546 File Offset: 0x00129946
		// (set) Token: 0x06003DF0 RID: 15856 RVA: 0x0012B54E File Offset: 0x0012994E
		[GpuData("meshVertexToNeiborsMap")]
		public GpuBuffer<int> MeshVertexToNeiborsMap
		{
			[CompilerGenerated]
			get
			{
				return this.<MeshVertexToNeiborsMap>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<MeshVertexToNeiborsMap>k__BackingField = value;
			}
		}

		// Token: 0x17000716 RID: 1814
		// (get) Token: 0x06003DF1 RID: 15857 RVA: 0x0012B557 File Offset: 0x00129957
		// (set) Token: 0x06003DF2 RID: 15858 RVA: 0x0012B55F File Offset: 0x0012995F
		[GpuData("meshVertexToNeiborsMapCounts")]
		public GpuBuffer<int> MeshVertexToNeiborsMapCounts
		{
			[CompilerGenerated]
			get
			{
				return this.<MeshVertexToNeiborsMapCounts>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<MeshVertexToNeiborsMapCounts>k__BackingField = value;
			}
		}

		// Token: 0x06003DF3 RID: 15859 RVA: 0x0012B568 File Offset: 0x00129968
		public override int GetGroupsNumX()
		{
			if (this.MeshToPhysicsVerticesMap != null)
			{
				return Mathf.CeilToInt((float)this.MeshToPhysicsVerticesMap.ComputeBuffer.count / 256f);
			}
			return 0;
		}

		// Token: 0x04002F55 RID: 12117
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private GpuValue<int> <FacesForNormalNum>k__BackingField;

		// Token: 0x04002F56 RID: 12118
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private GpuBuffer<GPParticle> <Particles>k__BackingField;

		// Token: 0x04002F57 RID: 12119
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private GpuBuffer<ClothVertex> <ClothVertices>k__BackingField;

		// Token: 0x04002F58 RID: 12120
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private GpuBuffer<int> <MeshToPhysicsVerticesMap>k__BackingField;

		// Token: 0x04002F59 RID: 12121
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private GpuBuffer<int> <MeshVertexToNeiborsMap>k__BackingField;

		// Token: 0x04002F5A RID: 12122
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private GpuBuffer<int> <MeshVertexToNeiborsMapCounts>k__BackingField;
	}
}
