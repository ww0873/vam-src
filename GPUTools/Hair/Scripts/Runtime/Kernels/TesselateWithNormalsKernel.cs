using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using GPUTools.Common.Scripts.PL.Abstract;
using GPUTools.Common.Scripts.PL.Attributes;
using GPUTools.Common.Scripts.PL.Tools;
using GPUTools.Hair.Scripts.Runtime.Render;
using GPUTools.Physics.Scripts.Types.Dynamic;
using GPUTools.Physics.Scripts.Types.Joints;
using UnityEngine;

namespace GPUTools.Hair.Scripts.Runtime.Kernels
{
	// Token: 0x02000A1C RID: 2588
	public class TesselateWithNormalsKernel : KernelBase
	{
		// Token: 0x0600423C RID: 16956 RVA: 0x00138A63 File Offset: 0x00136E63
		public TesselateWithNormalsKernel() : base("Compute/TesselateWithNormals", "CSTesselateWithNormals")
		{
		}

		// Token: 0x1700082C RID: 2092
		// (get) Token: 0x0600423E RID: 16958 RVA: 0x00138A7E File Offset: 0x00136E7E
		// (set) Token: 0x0600423D RID: 16957 RVA: 0x00138A75 File Offset: 0x00136E75
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

		// Token: 0x1700082D RID: 2093
		// (get) Token: 0x06004240 RID: 16960 RVA: 0x00138A8F File Offset: 0x00136E8F
		// (set) Token: 0x0600423F RID: 16959 RVA: 0x00138A86 File Offset: 0x00136E86
		[GpuData("renderParticles")]
		public GpuBuffer<RenderParticle> RenderParticles
		{
			[CompilerGenerated]
			get
			{
				return this.<RenderParticles>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<RenderParticles>k__BackingField = value;
			}
		}

		// Token: 0x1700082E RID: 2094
		// (get) Token: 0x06004242 RID: 16962 RVA: 0x00138AA0 File Offset: 0x00136EA0
		// (set) Token: 0x06004241 RID: 16961 RVA: 0x00138A97 File Offset: 0x00136E97
		[GpuData("tessRenderParticles")]
		public GpuBuffer<TessRenderParticle> TessRenderParticles
		{
			[CompilerGenerated]
			get
			{
				return this.<TessRenderParticles>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<TessRenderParticles>k__BackingField = value;
			}
		}

		// Token: 0x1700082F RID: 2095
		// (get) Token: 0x06004244 RID: 16964 RVA: 0x00138AB1 File Offset: 0x00136EB1
		// (set) Token: 0x06004243 RID: 16963 RVA: 0x00138AA8 File Offset: 0x00136EA8
		[GpuData("tessRenderParticlesCount")]
		public GpuValue<int> TessRenderParticlesCount
		{
			[CompilerGenerated]
			get
			{
				return this.<TessRenderParticlesCount>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<TessRenderParticlesCount>k__BackingField = value;
			}
		}

		// Token: 0x17000830 RID: 2096
		// (get) Token: 0x06004246 RID: 16966 RVA: 0x00138AC2 File Offset: 0x00136EC2
		// (set) Token: 0x06004245 RID: 16965 RVA: 0x00138AB9 File Offset: 0x00136EB9
		[GpuData("randomsPerStrand")]
		public GpuBuffer<Vector3> RandomsPerStrand
		{
			[CompilerGenerated]
			get
			{
				return this.<RandomsPerStrand>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<RandomsPerStrand>k__BackingField = value;
			}
		}

		// Token: 0x17000831 RID: 2097
		// (get) Token: 0x06004248 RID: 16968 RVA: 0x00138AD3 File Offset: 0x00136ED3
		// (set) Token: 0x06004247 RID: 16967 RVA: 0x00138ACA File Offset: 0x00136ECA
		[GpuData("transforms")]
		public GpuBuffer<Matrix4x4> Transforms
		{
			[CompilerGenerated]
			get
			{
				return this.<Transforms>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<Transforms>k__BackingField = value;
			}
		}

		// Token: 0x17000832 RID: 2098
		// (get) Token: 0x0600424A RID: 16970 RVA: 0x00138AE4 File Offset: 0x00136EE4
		// (set) Token: 0x06004249 RID: 16969 RVA: 0x00138ADB File Offset: 0x00136EDB
		[GpuData("normals")]
		public GpuBuffer<Vector3> Normals
		{
			[CompilerGenerated]
			get
			{
				return this.<Normals>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<Normals>k__BackingField = value;
			}
		}

		// Token: 0x17000833 RID: 2099
		// (get) Token: 0x0600424C RID: 16972 RVA: 0x00138AF5 File Offset: 0x00136EF5
		// (set) Token: 0x0600424B RID: 16971 RVA: 0x00138AEC File Offset: 0x00136EEC
		[GpuData("lightCenter")]
		public GpuValue<Vector3> LightCenter
		{
			[CompilerGenerated]
			get
			{
				return this.<LightCenter>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<LightCenter>k__BackingField = value;
			}
		}

		// Token: 0x17000834 RID: 2100
		// (get) Token: 0x0600424E RID: 16974 RVA: 0x00138B06 File Offset: 0x00136F06
		// (set) Token: 0x0600424D RID: 16973 RVA: 0x00138AFD File Offset: 0x00136EFD
		[GpuData("pointJoints")]
		public GpuBuffer<GPPointJoint> PointJoints
		{
			[CompilerGenerated]
			get
			{
				return this.<PointJoints>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<PointJoints>k__BackingField = value;
			}
		}

		// Token: 0x17000835 RID: 2101
		// (get) Token: 0x06004250 RID: 16976 RVA: 0x00138B17 File Offset: 0x00136F17
		// (set) Token: 0x0600424F RID: 16975 RVA: 0x00138B0E File Offset: 0x00136F0E
		[GpuData("normalRandomize")]
		public GpuValue<float> NormalRandomize
		{
			[CompilerGenerated]
			get
			{
				return this.<NormalRandomize>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<NormalRandomize>k__BackingField = value;
			}
		}

		// Token: 0x17000836 RID: 2102
		// (get) Token: 0x06004252 RID: 16978 RVA: 0x00138B28 File Offset: 0x00136F28
		// (set) Token: 0x06004251 RID: 16977 RVA: 0x00138B1F File Offset: 0x00136F1F
		[GpuData("segments")]
		public GpuValue<int> Segments
		{
			[CompilerGenerated]
			get
			{
				return this.<Segments>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<Segments>k__BackingField = value;
			}
		}

		// Token: 0x17000837 RID: 2103
		// (get) Token: 0x06004254 RID: 16980 RVA: 0x00138B39 File Offset: 0x00136F39
		// (set) Token: 0x06004253 RID: 16979 RVA: 0x00138B30 File Offset: 0x00136F30
		[GpuData("tessSegments")]
		public GpuValue<int> TessSegments
		{
			[CompilerGenerated]
			get
			{
				return this.<TessSegments>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<TessSegments>k__BackingField = value;
			}
		}

		// Token: 0x17000838 RID: 2104
		// (get) Token: 0x06004256 RID: 16982 RVA: 0x00138B4A File Offset: 0x00136F4A
		// (set) Token: 0x06004255 RID: 16981 RVA: 0x00138B41 File Offset: 0x00136F41
		[GpuData("wavinessAxis")]
		public GpuValue<Vector3> WavinessAxis
		{
			[CompilerGenerated]
			get
			{
				return this.<WavinessAxis>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<WavinessAxis>k__BackingField = value;
			}
		}

		// Token: 0x17000839 RID: 2105
		// (get) Token: 0x06004258 RID: 16984 RVA: 0x00138B5B File Offset: 0x00136F5B
		// (set) Token: 0x06004257 RID: 16983 RVA: 0x00138B52 File Offset: 0x00136F52
		[GpuData("wavinessFrequencyRandomness")]
		public GpuValue<float> WavinessFrequencyRandomness
		{
			[CompilerGenerated]
			get
			{
				return this.<WavinessFrequencyRandomness>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<WavinessFrequencyRandomness>k__BackingField = value;
			}
		}

		// Token: 0x1700083A RID: 2106
		// (get) Token: 0x0600425A RID: 16986 RVA: 0x00138B6C File Offset: 0x00136F6C
		// (set) Token: 0x06004259 RID: 16985 RVA: 0x00138B63 File Offset: 0x00136F63
		[GpuData("wavinessScaleRandomness")]
		public GpuValue<float> WavinessScaleRandomness
		{
			[CompilerGenerated]
			get
			{
				return this.<WavinessScaleRandomness>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<WavinessScaleRandomness>k__BackingField = value;
			}
		}

		// Token: 0x1700083B RID: 2107
		// (get) Token: 0x0600425C RID: 16988 RVA: 0x00138B7D File Offset: 0x00136F7D
		// (set) Token: 0x0600425B RID: 16987 RVA: 0x00138B74 File Offset: 0x00136F74
		[GpuData("wavinessAllowReverse")]
		public GpuValue<bool> WavinessAllowReverse
		{
			[CompilerGenerated]
			get
			{
				return this.<WavinessAllowReverse>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<WavinessAllowReverse>k__BackingField = value;
			}
		}

		// Token: 0x1700083C RID: 2108
		// (get) Token: 0x0600425E RID: 16990 RVA: 0x00138B8E File Offset: 0x00136F8E
		// (set) Token: 0x0600425D RID: 16989 RVA: 0x00138B85 File Offset: 0x00136F85
		[GpuData("wavinessAllowFlipAxis")]
		public GpuValue<bool> WavinessAllowFlipAxis
		{
			[CompilerGenerated]
			get
			{
				return this.<WavinessAllowFlipAxis>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<WavinessAllowFlipAxis>k__BackingField = value;
			}
		}

		// Token: 0x1700083D RID: 2109
		// (get) Token: 0x06004260 RID: 16992 RVA: 0x00138B9F File Offset: 0x00136F9F
		// (set) Token: 0x0600425F RID: 16991 RVA: 0x00138B96 File Offset: 0x00136F96
		[GpuData("wavinessNormalAdjust")]
		public GpuValue<float> WavinessNormalAdjust
		{
			[CompilerGenerated]
			get
			{
				return this.<WavinessNormalAdjust>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<WavinessNormalAdjust>k__BackingField = value;
			}
		}

		// Token: 0x06004261 RID: 16993 RVA: 0x00138BA7 File Offset: 0x00136FA7
		public override int GetGroupsNumX()
		{
			if (this.TessRenderParticles != null)
			{
				return Mathf.CeilToInt((float)this.TessRenderParticlesCount.Value / 256f);
			}
			return 0;
		}

		// Token: 0x04003144 RID: 12612
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private GpuBuffer<GPParticle> <Particles>k__BackingField;

		// Token: 0x04003145 RID: 12613
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private GpuBuffer<RenderParticle> <RenderParticles>k__BackingField;

		// Token: 0x04003146 RID: 12614
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private GpuBuffer<TessRenderParticle> <TessRenderParticles>k__BackingField;

		// Token: 0x04003147 RID: 12615
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private GpuValue<int> <TessRenderParticlesCount>k__BackingField;

		// Token: 0x04003148 RID: 12616
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private GpuBuffer<Vector3> <RandomsPerStrand>k__BackingField;

		// Token: 0x04003149 RID: 12617
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private GpuBuffer<Matrix4x4> <Transforms>k__BackingField;

		// Token: 0x0400314A RID: 12618
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private GpuBuffer<Vector3> <Normals>k__BackingField;

		// Token: 0x0400314B RID: 12619
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private GpuValue<Vector3> <LightCenter>k__BackingField;

		// Token: 0x0400314C RID: 12620
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private GpuBuffer<GPPointJoint> <PointJoints>k__BackingField;

		// Token: 0x0400314D RID: 12621
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private GpuValue<float> <NormalRandomize>k__BackingField;

		// Token: 0x0400314E RID: 12622
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private GpuValue<int> <Segments>k__BackingField;

		// Token: 0x0400314F RID: 12623
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private GpuValue<int> <TessSegments>k__BackingField;

		// Token: 0x04003150 RID: 12624
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private GpuValue<Vector3> <WavinessAxis>k__BackingField;

		// Token: 0x04003151 RID: 12625
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private GpuValue<float> <WavinessFrequencyRandomness>k__BackingField;

		// Token: 0x04003152 RID: 12626
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private GpuValue<float> <WavinessScaleRandomness>k__BackingField;

		// Token: 0x04003153 RID: 12627
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private GpuValue<bool> <WavinessAllowReverse>k__BackingField;

		// Token: 0x04003154 RID: 12628
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private GpuValue<bool> <WavinessAllowFlipAxis>k__BackingField;

		// Token: 0x04003155 RID: 12629
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private GpuValue<float> <WavinessNormalAdjust>k__BackingField;
	}
}
