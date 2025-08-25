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
	// Token: 0x02000A1B RID: 2587
	public class TesselateKernel : KernelBase
	{
		// Token: 0x06004220 RID: 16928 RVA: 0x0013894E File Offset: 0x00136D4E
		public TesselateKernel() : base("Compute/Tesselate", "CSTesselate")
		{
		}

		// Token: 0x1700081F RID: 2079
		// (get) Token: 0x06004222 RID: 16930 RVA: 0x00138969 File Offset: 0x00136D69
		// (set) Token: 0x06004221 RID: 16929 RVA: 0x00138960 File Offset: 0x00136D60
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

		// Token: 0x17000820 RID: 2080
		// (get) Token: 0x06004224 RID: 16932 RVA: 0x0013897A File Offset: 0x00136D7A
		// (set) Token: 0x06004223 RID: 16931 RVA: 0x00138971 File Offset: 0x00136D71
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

		// Token: 0x17000821 RID: 2081
		// (get) Token: 0x06004226 RID: 16934 RVA: 0x0013898B File Offset: 0x00136D8B
		// (set) Token: 0x06004225 RID: 16933 RVA: 0x00138982 File Offset: 0x00136D82
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

		// Token: 0x17000822 RID: 2082
		// (get) Token: 0x06004228 RID: 16936 RVA: 0x0013899C File Offset: 0x00136D9C
		// (set) Token: 0x06004227 RID: 16935 RVA: 0x00138993 File Offset: 0x00136D93
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

		// Token: 0x17000823 RID: 2083
		// (get) Token: 0x0600422A RID: 16938 RVA: 0x001389AD File Offset: 0x00136DAD
		// (set) Token: 0x06004229 RID: 16937 RVA: 0x001389A4 File Offset: 0x00136DA4
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

		// Token: 0x17000824 RID: 2084
		// (get) Token: 0x0600422C RID: 16940 RVA: 0x001389BE File Offset: 0x00136DBE
		// (set) Token: 0x0600422B RID: 16939 RVA: 0x001389B5 File Offset: 0x00136DB5
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

		// Token: 0x17000825 RID: 2085
		// (get) Token: 0x0600422E RID: 16942 RVA: 0x001389CF File Offset: 0x00136DCF
		// (set) Token: 0x0600422D RID: 16941 RVA: 0x001389C6 File Offset: 0x00136DC6
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

		// Token: 0x17000826 RID: 2086
		// (get) Token: 0x06004230 RID: 16944 RVA: 0x001389E0 File Offset: 0x00136DE0
		// (set) Token: 0x0600422F RID: 16943 RVA: 0x001389D7 File Offset: 0x00136DD7
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

		// Token: 0x17000827 RID: 2087
		// (get) Token: 0x06004232 RID: 16946 RVA: 0x001389F1 File Offset: 0x00136DF1
		// (set) Token: 0x06004231 RID: 16945 RVA: 0x001389E8 File Offset: 0x00136DE8
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

		// Token: 0x17000828 RID: 2088
		// (get) Token: 0x06004234 RID: 16948 RVA: 0x00138A02 File Offset: 0x00136E02
		// (set) Token: 0x06004233 RID: 16947 RVA: 0x001389F9 File Offset: 0x00136DF9
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

		// Token: 0x17000829 RID: 2089
		// (get) Token: 0x06004236 RID: 16950 RVA: 0x00138A13 File Offset: 0x00136E13
		// (set) Token: 0x06004235 RID: 16949 RVA: 0x00138A0A File Offset: 0x00136E0A
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

		// Token: 0x1700082A RID: 2090
		// (get) Token: 0x06004238 RID: 16952 RVA: 0x00138A24 File Offset: 0x00136E24
		// (set) Token: 0x06004237 RID: 16951 RVA: 0x00138A1B File Offset: 0x00136E1B
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

		// Token: 0x1700082B RID: 2091
		// (get) Token: 0x0600423A RID: 16954 RVA: 0x00138A35 File Offset: 0x00136E35
		// (set) Token: 0x06004239 RID: 16953 RVA: 0x00138A2C File Offset: 0x00136E2C
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

		// Token: 0x0600423B RID: 16955 RVA: 0x00138A3D File Offset: 0x00136E3D
		public override int GetGroupsNumX()
		{
			if (this.TessRenderParticles != null)
			{
				return Mathf.CeilToInt((float)this.TessRenderParticlesCount.Value / 256f);
			}
			return 0;
		}

		// Token: 0x04003137 RID: 12599
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private GpuBuffer<GPParticle> <Particles>k__BackingField;

		// Token: 0x04003138 RID: 12600
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private GpuBuffer<RenderParticle> <RenderParticles>k__BackingField;

		// Token: 0x04003139 RID: 12601
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private GpuBuffer<TessRenderParticle> <TessRenderParticles>k__BackingField;

		// Token: 0x0400313A RID: 12602
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private GpuValue<int> <TessRenderParticlesCount>k__BackingField;

		// Token: 0x0400313B RID: 12603
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private GpuBuffer<Matrix4x4> <Transforms>k__BackingField;

		// Token: 0x0400313C RID: 12604
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private GpuBuffer<GPPointJoint> <PointJoints>k__BackingField;

		// Token: 0x0400313D RID: 12605
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private GpuValue<Vector3> <LightCenter>k__BackingField;

		// Token: 0x0400313E RID: 12606
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private GpuValue<int> <Segments>k__BackingField;

		// Token: 0x0400313F RID: 12607
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private GpuValue<int> <TessSegments>k__BackingField;

		// Token: 0x04003140 RID: 12608
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private GpuValue<Vector3> <WavinessAxis>k__BackingField;

		// Token: 0x04003141 RID: 12609
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private GpuValue<float> <WavinessFrequencyRandomness>k__BackingField;

		// Token: 0x04003142 RID: 12610
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private GpuValue<float> <WavinessScaleRandomness>k__BackingField;

		// Token: 0x04003143 RID: 12611
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private GpuValue<bool> <WavinessAllowReverse>k__BackingField;
	}
}
