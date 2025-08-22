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
	// Token: 0x02000A1D RID: 2589
	public class TesselateWithNormalsRenderRigidityKernel : KernelBase
	{
		// Token: 0x06004262 RID: 16994 RVA: 0x00138BCD File Offset: 0x00136FCD
		public TesselateWithNormalsRenderRigidityKernel() : base("Compute/TesselateWithNormalsRenderRigidity", "CSTesselateWithNormalsRenderRigidity")
		{
		}

		// Token: 0x1700083E RID: 2110
		// (get) Token: 0x06004264 RID: 16996 RVA: 0x00138BE8 File Offset: 0x00136FE8
		// (set) Token: 0x06004263 RID: 16995 RVA: 0x00138BDF File Offset: 0x00136FDF
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

		// Token: 0x1700083F RID: 2111
		// (get) Token: 0x06004266 RID: 16998 RVA: 0x00138BF9 File Offset: 0x00136FF9
		// (set) Token: 0x06004265 RID: 16997 RVA: 0x00138BF0 File Offset: 0x00136FF0
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

		// Token: 0x17000840 RID: 2112
		// (get) Token: 0x06004268 RID: 17000 RVA: 0x00138C0A File Offset: 0x0013700A
		// (set) Token: 0x06004267 RID: 16999 RVA: 0x00138C01 File Offset: 0x00137001
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

		// Token: 0x17000841 RID: 2113
		// (get) Token: 0x0600426A RID: 17002 RVA: 0x00138C1B File Offset: 0x0013701B
		// (set) Token: 0x06004269 RID: 17001 RVA: 0x00138C12 File Offset: 0x00137012
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

		// Token: 0x17000842 RID: 2114
		// (get) Token: 0x0600426C RID: 17004 RVA: 0x00138C2C File Offset: 0x0013702C
		// (set) Token: 0x0600426B RID: 17003 RVA: 0x00138C23 File Offset: 0x00137023
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

		// Token: 0x17000843 RID: 2115
		// (get) Token: 0x0600426E RID: 17006 RVA: 0x00138C3D File Offset: 0x0013703D
		// (set) Token: 0x0600426D RID: 17005 RVA: 0x00138C34 File Offset: 0x00137034
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

		// Token: 0x17000844 RID: 2116
		// (get) Token: 0x06004270 RID: 17008 RVA: 0x00138C4E File Offset: 0x0013704E
		// (set) Token: 0x0600426F RID: 17007 RVA: 0x00138C45 File Offset: 0x00137045
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

		// Token: 0x17000845 RID: 2117
		// (get) Token: 0x06004272 RID: 17010 RVA: 0x00138C5F File Offset: 0x0013705F
		// (set) Token: 0x06004271 RID: 17009 RVA: 0x00138C56 File Offset: 0x00137056
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

		// Token: 0x17000846 RID: 2118
		// (get) Token: 0x06004274 RID: 17012 RVA: 0x00138C70 File Offset: 0x00137070
		// (set) Token: 0x06004273 RID: 17011 RVA: 0x00138C67 File Offset: 0x00137067
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

		// Token: 0x17000847 RID: 2119
		// (get) Token: 0x06004276 RID: 17014 RVA: 0x00138C81 File Offset: 0x00137081
		// (set) Token: 0x06004275 RID: 17013 RVA: 0x00138C78 File Offset: 0x00137078
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

		// Token: 0x17000848 RID: 2120
		// (get) Token: 0x06004278 RID: 17016 RVA: 0x00138C92 File Offset: 0x00137092
		// (set) Token: 0x06004277 RID: 17015 RVA: 0x00138C89 File Offset: 0x00137089
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

		// Token: 0x17000849 RID: 2121
		// (get) Token: 0x0600427A RID: 17018 RVA: 0x00138CA3 File Offset: 0x001370A3
		// (set) Token: 0x06004279 RID: 17017 RVA: 0x00138C9A File Offset: 0x0013709A
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

		// Token: 0x1700084A RID: 2122
		// (get) Token: 0x0600427C RID: 17020 RVA: 0x00138CB4 File Offset: 0x001370B4
		// (set) Token: 0x0600427B RID: 17019 RVA: 0x00138CAB File Offset: 0x001370AB
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

		// Token: 0x1700084B RID: 2123
		// (get) Token: 0x0600427E RID: 17022 RVA: 0x00138CC5 File Offset: 0x001370C5
		// (set) Token: 0x0600427D RID: 17021 RVA: 0x00138CBC File Offset: 0x001370BC
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

		// Token: 0x1700084C RID: 2124
		// (get) Token: 0x06004280 RID: 17024 RVA: 0x00138CD6 File Offset: 0x001370D6
		// (set) Token: 0x0600427F RID: 17023 RVA: 0x00138CCD File Offset: 0x001370CD
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

		// Token: 0x1700084D RID: 2125
		// (get) Token: 0x06004282 RID: 17026 RVA: 0x00138CE7 File Offset: 0x001370E7
		// (set) Token: 0x06004281 RID: 17025 RVA: 0x00138CDE File Offset: 0x001370DE
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

		// Token: 0x1700084E RID: 2126
		// (get) Token: 0x06004284 RID: 17028 RVA: 0x00138CF8 File Offset: 0x001370F8
		// (set) Token: 0x06004283 RID: 17027 RVA: 0x00138CEF File Offset: 0x001370EF
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

		// Token: 0x1700084F RID: 2127
		// (get) Token: 0x06004286 RID: 17030 RVA: 0x00138D09 File Offset: 0x00137109
		// (set) Token: 0x06004285 RID: 17029 RVA: 0x00138D00 File Offset: 0x00137100
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

		// Token: 0x06004287 RID: 17031 RVA: 0x00138D11 File Offset: 0x00137111
		public override int GetGroupsNumX()
		{
			if (this.TessRenderParticles != null)
			{
				return Mathf.CeilToInt((float)this.TessRenderParticlesCount.Value / 256f);
			}
			return 0;
		}

		// Token: 0x04003156 RID: 12630
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private GpuBuffer<GPParticle> <Particles>k__BackingField;

		// Token: 0x04003157 RID: 12631
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private GpuBuffer<RenderParticle> <RenderParticles>k__BackingField;

		// Token: 0x04003158 RID: 12632
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private GpuBuffer<TessRenderParticle> <TessRenderParticles>k__BackingField;

		// Token: 0x04003159 RID: 12633
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private GpuValue<int> <TessRenderParticlesCount>k__BackingField;

		// Token: 0x0400315A RID: 12634
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private GpuBuffer<Vector3> <RandomsPerStrand>k__BackingField;

		// Token: 0x0400315B RID: 12635
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private GpuBuffer<Matrix4x4> <Transforms>k__BackingField;

		// Token: 0x0400315C RID: 12636
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private GpuBuffer<Vector3> <Normals>k__BackingField;

		// Token: 0x0400315D RID: 12637
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private GpuValue<Vector3> <LightCenter>k__BackingField;

		// Token: 0x0400315E RID: 12638
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private GpuBuffer<GPPointJoint> <PointJoints>k__BackingField;

		// Token: 0x0400315F RID: 12639
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private GpuValue<float> <NormalRandomize>k__BackingField;

		// Token: 0x04003160 RID: 12640
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private GpuValue<int> <Segments>k__BackingField;

		// Token: 0x04003161 RID: 12641
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private GpuValue<int> <TessSegments>k__BackingField;

		// Token: 0x04003162 RID: 12642
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private GpuValue<Vector3> <WavinessAxis>k__BackingField;

		// Token: 0x04003163 RID: 12643
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private GpuValue<float> <WavinessFrequencyRandomness>k__BackingField;

		// Token: 0x04003164 RID: 12644
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private GpuValue<float> <WavinessScaleRandomness>k__BackingField;

		// Token: 0x04003165 RID: 12645
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private GpuValue<bool> <WavinessAllowReverse>k__BackingField;

		// Token: 0x04003166 RID: 12646
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private GpuValue<bool> <WavinessAllowFlipAxis>k__BackingField;

		// Token: 0x04003167 RID: 12647
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private GpuValue<float> <WavinessNormalAdjust>k__BackingField;
	}
}
