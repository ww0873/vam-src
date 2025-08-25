using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using GPUTools.Common.Scripts.PL.Tools;
using GPUTools.Hair.Scripts.Runtime.Render;
using GPUTools.Physics.Scripts.Types.Dynamic;
using GPUTools.Physics.Scripts.Types.Joints;
using GPUTools.Physics.Scripts.Types.Shapes;
using UnityEngine;

namespace GPUTools.Hair.Scripts.Runtime.Data
{
	// Token: 0x02000A1A RID: 2586
	public class RuntimeData
	{
		// Token: 0x060041E1 RID: 16865 RVA: 0x00138737 File Offset: 0x00136B37
		public RuntimeData()
		{
		}

		// Token: 0x17000800 RID: 2048
		// (get) Token: 0x060041E2 RID: 16866 RVA: 0x0013873F File Offset: 0x00136B3F
		// (set) Token: 0x060041E3 RID: 16867 RVA: 0x00138747 File Offset: 0x00136B47
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

		// Token: 0x17000801 RID: 2049
		// (get) Token: 0x060041E4 RID: 16868 RVA: 0x00138750 File Offset: 0x00136B50
		// (set) Token: 0x060041E5 RID: 16869 RVA: 0x00138758 File Offset: 0x00136B58
		public float[] ParticleRootToTipRatios
		{
			[CompilerGenerated]
			get
			{
				return this.<ParticleRootToTipRatios>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<ParticleRootToTipRatios>k__BackingField = value;
			}
		}

		// Token: 0x17000802 RID: 2050
		// (get) Token: 0x060041E6 RID: 16870 RVA: 0x00138761 File Offset: 0x00136B61
		// (set) Token: 0x060041E7 RID: 16871 RVA: 0x00138769 File Offset: 0x00136B69
		public GpuBuffer<GPSphereWithDelta> ProcessedSpheres
		{
			[CompilerGenerated]
			get
			{
				return this.<ProcessedSpheres>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<ProcessedSpheres>k__BackingField = value;
			}
		}

		// Token: 0x17000803 RID: 2051
		// (get) Token: 0x060041E8 RID: 16872 RVA: 0x00138772 File Offset: 0x00136B72
		// (set) Token: 0x060041E9 RID: 16873 RVA: 0x0013877A File Offset: 0x00136B7A
		public GpuBuffer<GPLineSphereWithDelta> ProcessedLineSpheres
		{
			[CompilerGenerated]
			get
			{
				return this.<ProcessedLineSpheres>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<ProcessedLineSpheres>k__BackingField = value;
			}
		}

		// Token: 0x17000804 RID: 2052
		// (get) Token: 0x060041EA RID: 16874 RVA: 0x00138783 File Offset: 0x00136B83
		// (set) Token: 0x060041EB RID: 16875 RVA: 0x0013878B File Offset: 0x00136B8B
		public GpuBuffer<GPLineSphere> CutLineSpheres
		{
			[CompilerGenerated]
			get
			{
				return this.<CutLineSpheres>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<CutLineSpheres>k__BackingField = value;
			}
		}

		// Token: 0x17000805 RID: 2053
		// (get) Token: 0x060041EC RID: 16876 RVA: 0x00138794 File Offset: 0x00136B94
		// (set) Token: 0x060041ED RID: 16877 RVA: 0x0013879C File Offset: 0x00136B9C
		public GpuBuffer<GPLineSphere> GrowLineSpheres
		{
			[CompilerGenerated]
			get
			{
				return this.<GrowLineSpheres>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<GrowLineSpheres>k__BackingField = value;
			}
		}

		// Token: 0x17000806 RID: 2054
		// (get) Token: 0x060041EE RID: 16878 RVA: 0x001387A5 File Offset: 0x00136BA5
		// (set) Token: 0x060041EF RID: 16879 RVA: 0x001387AD File Offset: 0x00136BAD
		public GpuBuffer<GPLineSphere> HoldLineSpheres
		{
			[CompilerGenerated]
			get
			{
				return this.<HoldLineSpheres>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<HoldLineSpheres>k__BackingField = value;
			}
		}

		// Token: 0x17000807 RID: 2055
		// (get) Token: 0x060041F0 RID: 16880 RVA: 0x001387B6 File Offset: 0x00136BB6
		// (set) Token: 0x060041F1 RID: 16881 RVA: 0x001387BE File Offset: 0x00136BBE
		public GpuBuffer<GPLineSphereWithMatrixDelta> GrabLineSpheres
		{
			[CompilerGenerated]
			get
			{
				return this.<GrabLineSpheres>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<GrabLineSpheres>k__BackingField = value;
			}
		}

		// Token: 0x17000808 RID: 2056
		// (get) Token: 0x060041F2 RID: 16882 RVA: 0x001387C7 File Offset: 0x00136BC7
		// (set) Token: 0x060041F3 RID: 16883 RVA: 0x001387CF File Offset: 0x00136BCF
		public GpuBuffer<GPLineSphere> PushLineSpheres
		{
			[CompilerGenerated]
			get
			{
				return this.<PushLineSpheres>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<PushLineSpheres>k__BackingField = value;
			}
		}

		// Token: 0x17000809 RID: 2057
		// (get) Token: 0x060041F4 RID: 16884 RVA: 0x001387D8 File Offset: 0x00136BD8
		// (set) Token: 0x060041F5 RID: 16885 RVA: 0x001387E0 File Offset: 0x00136BE0
		public GpuBuffer<GPLineSphere> PullLineSpheres
		{
			[CompilerGenerated]
			get
			{
				return this.<PullLineSpheres>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<PullLineSpheres>k__BackingField = value;
			}
		}

		// Token: 0x1700080A RID: 2058
		// (get) Token: 0x060041F6 RID: 16886 RVA: 0x001387E9 File Offset: 0x00136BE9
		// (set) Token: 0x060041F7 RID: 16887 RVA: 0x001387F1 File Offset: 0x00136BF1
		public GpuBuffer<GPLineSphereWithDelta> BrushLineSpheres
		{
			[CompilerGenerated]
			get
			{
				return this.<BrushLineSpheres>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<BrushLineSpheres>k__BackingField = value;
			}
		}

		// Token: 0x1700080B RID: 2059
		// (get) Token: 0x060041F8 RID: 16888 RVA: 0x001387FA File Offset: 0x00136BFA
		// (set) Token: 0x060041F9 RID: 16889 RVA: 0x00138802 File Offset: 0x00136C02
		public GpuBuffer<GPLineSphere> RigidityIncreaseLineSpheres
		{
			[CompilerGenerated]
			get
			{
				return this.<RigidityIncreaseLineSpheres>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<RigidityIncreaseLineSpheres>k__BackingField = value;
			}
		}

		// Token: 0x1700080C RID: 2060
		// (get) Token: 0x060041FA RID: 16890 RVA: 0x0013880B File Offset: 0x00136C0B
		// (set) Token: 0x060041FB RID: 16891 RVA: 0x00138813 File Offset: 0x00136C13
		public GpuBuffer<GPLineSphere> RigidityDecreaseLineSpheres
		{
			[CompilerGenerated]
			get
			{
				return this.<RigidityDecreaseLineSpheres>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<RigidityDecreaseLineSpheres>k__BackingField = value;
			}
		}

		// Token: 0x1700080D RID: 2061
		// (get) Token: 0x060041FC RID: 16892 RVA: 0x0013881C File Offset: 0x00136C1C
		// (set) Token: 0x060041FD RID: 16893 RVA: 0x00138824 File Offset: 0x00136C24
		public GpuBuffer<GPLineSphere> RigiditySetLineSpheres
		{
			[CompilerGenerated]
			get
			{
				return this.<RigiditySetLineSpheres>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<RigiditySetLineSpheres>k__BackingField = value;
			}
		}

		// Token: 0x1700080E RID: 2062
		// (get) Token: 0x060041FE RID: 16894 RVA: 0x0013882D File Offset: 0x00136C2D
		// (set) Token: 0x060041FF RID: 16895 RVA: 0x00138835 File Offset: 0x00136C35
		public GpuBuffer<Vector4> Planes
		{
			[CompilerGenerated]
			get
			{
				return this.<Planes>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<Planes>k__BackingField = value;
			}
		}

		// Token: 0x1700080F RID: 2063
		// (get) Token: 0x06004200 RID: 16896 RVA: 0x0013883E File Offset: 0x00136C3E
		// (set) Token: 0x06004201 RID: 16897 RVA: 0x00138846 File Offset: 0x00136C46
		public GroupedData<GPDistanceJoint> DistanceJoints
		{
			[CompilerGenerated]
			get
			{
				return this.<DistanceJoints>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<DistanceJoints>k__BackingField = value;
			}
		}

		// Token: 0x17000810 RID: 2064
		// (get) Token: 0x06004202 RID: 16898 RVA: 0x0013884F File Offset: 0x00136C4F
		// (set) Token: 0x06004203 RID: 16899 RVA: 0x00138857 File Offset: 0x00136C57
		public GpuBuffer<GPDistanceJoint> DistanceJointsBuffer
		{
			[CompilerGenerated]
			get
			{
				return this.<DistanceJointsBuffer>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<DistanceJointsBuffer>k__BackingField = value;
			}
		}

		// Token: 0x17000811 RID: 2065
		// (get) Token: 0x06004204 RID: 16900 RVA: 0x00138860 File Offset: 0x00136C60
		// (set) Token: 0x06004205 RID: 16901 RVA: 0x00138868 File Offset: 0x00136C68
		public GroupedData<GPDistanceJoint> CompressionJoints
		{
			[CompilerGenerated]
			get
			{
				return this.<CompressionJoints>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<CompressionJoints>k__BackingField = value;
			}
		}

		// Token: 0x17000812 RID: 2066
		// (get) Token: 0x06004206 RID: 16902 RVA: 0x00138871 File Offset: 0x00136C71
		// (set) Token: 0x06004207 RID: 16903 RVA: 0x00138879 File Offset: 0x00136C79
		public GpuBuffer<GPDistanceJoint> CompressionJointsBuffer
		{
			[CompilerGenerated]
			get
			{
				return this.<CompressionJointsBuffer>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<CompressionJointsBuffer>k__BackingField = value;
			}
		}

		// Token: 0x17000813 RID: 2067
		// (get) Token: 0x06004208 RID: 16904 RVA: 0x00138882 File Offset: 0x00136C82
		// (set) Token: 0x06004209 RID: 16905 RVA: 0x0013888A File Offset: 0x00136C8A
		public GroupedData<GPDistanceJoint> NearbyDistanceJoints
		{
			[CompilerGenerated]
			get
			{
				return this.<NearbyDistanceJoints>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<NearbyDistanceJoints>k__BackingField = value;
			}
		}

		// Token: 0x17000814 RID: 2068
		// (get) Token: 0x0600420A RID: 16906 RVA: 0x00138893 File Offset: 0x00136C93
		// (set) Token: 0x0600420B RID: 16907 RVA: 0x0013889B File Offset: 0x00136C9B
		public GpuBuffer<GPDistanceJoint> NearbyDistanceJointsBuffer
		{
			[CompilerGenerated]
			get
			{
				return this.<NearbyDistanceJointsBuffer>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<NearbyDistanceJointsBuffer>k__BackingField = value;
			}
		}

		// Token: 0x17000815 RID: 2069
		// (get) Token: 0x0600420C RID: 16908 RVA: 0x001388A4 File Offset: 0x00136CA4
		// (set) Token: 0x0600420D RID: 16909 RVA: 0x001388AC File Offset: 0x00136CAC
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

		// Token: 0x17000816 RID: 2070
		// (get) Token: 0x0600420E RID: 16910 RVA: 0x001388B5 File Offset: 0x00136CB5
		// (set) Token: 0x0600420F RID: 16911 RVA: 0x001388BD File Offset: 0x00136CBD
		public GpuBuffer<float> PointToPreviousPointDistances
		{
			[CompilerGenerated]
			get
			{
				return this.<PointToPreviousPointDistances>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<PointToPreviousPointDistances>k__BackingField = value;
			}
		}

		// Token: 0x17000817 RID: 2071
		// (get) Token: 0x06004210 RID: 16912 RVA: 0x001388C6 File Offset: 0x00136CC6
		// (set) Token: 0x06004211 RID: 16913 RVA: 0x001388CE File Offset: 0x00136CCE
		public GpuBuffer<Vector3> Barycentrics
		{
			[CompilerGenerated]
			get
			{
				return this.<Barycentrics>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<Barycentrics>k__BackingField = value;
			}
		}

		// Token: 0x17000818 RID: 2072
		// (get) Token: 0x06004212 RID: 16914 RVA: 0x001388D7 File Offset: 0x00136CD7
		// (set) Token: 0x06004213 RID: 16915 RVA: 0x001388DF File Offset: 0x00136CDF
		public GpuBuffer<Vector3> BarycentricsFixed
		{
			[CompilerGenerated]
			get
			{
				return this.<BarycentricsFixed>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<BarycentricsFixed>k__BackingField = value;
			}
		}

		// Token: 0x17000819 RID: 2073
		// (get) Token: 0x06004214 RID: 16916 RVA: 0x001388E8 File Offset: 0x00136CE8
		// (set) Token: 0x06004215 RID: 16917 RVA: 0x001388F0 File Offset: 0x00136CF0
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

		// Token: 0x1700081A RID: 2074
		// (get) Token: 0x06004216 RID: 16918 RVA: 0x001388F9 File Offset: 0x00136CF9
		// (set) Token: 0x06004217 RID: 16919 RVA: 0x00138901 File Offset: 0x00136D01
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

		// Token: 0x1700081B RID: 2075
		// (get) Token: 0x06004218 RID: 16920 RVA: 0x0013890A File Offset: 0x00136D0A
		// (set) Token: 0x06004219 RID: 16921 RVA: 0x00138912 File Offset: 0x00136D12
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

		// Token: 0x1700081C RID: 2076
		// (get) Token: 0x0600421A RID: 16922 RVA: 0x0013891B File Offset: 0x00136D1B
		// (set) Token: 0x0600421B RID: 16923 RVA: 0x00138923 File Offset: 0x00136D23
		public GpuBuffer<GPParticle> OutParticles
		{
			[CompilerGenerated]
			get
			{
				return this.<OutParticles>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<OutParticles>k__BackingField = value;
			}
		}

		// Token: 0x1700081D RID: 2077
		// (get) Token: 0x0600421C RID: 16924 RVA: 0x0013892C File Offset: 0x00136D2C
		// (set) Token: 0x0600421D RID: 16925 RVA: 0x00138934 File Offset: 0x00136D34
		public GpuBuffer<float> OutParticlesMap
		{
			[CompilerGenerated]
			get
			{
				return this.<OutParticlesMap>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<OutParticlesMap>k__BackingField = value;
			}
		}

		// Token: 0x1700081E RID: 2078
		// (get) Token: 0x0600421E RID: 16926 RVA: 0x0013893D File Offset: 0x00136D3D
		// (set) Token: 0x0600421F RID: 16927 RVA: 0x00138945 File Offset: 0x00136D45
		public Vector3 Wind
		{
			[CompilerGenerated]
			get
			{
				return this.<Wind>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<Wind>k__BackingField = value;
			}
		}

		// Token: 0x04003118 RID: 12568
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private GpuBuffer<GPParticle> <Particles>k__BackingField;

		// Token: 0x04003119 RID: 12569
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private float[] <ParticleRootToTipRatios>k__BackingField;

		// Token: 0x0400311A RID: 12570
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private GpuBuffer<GPSphereWithDelta> <ProcessedSpheres>k__BackingField;

		// Token: 0x0400311B RID: 12571
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private GpuBuffer<GPLineSphereWithDelta> <ProcessedLineSpheres>k__BackingField;

		// Token: 0x0400311C RID: 12572
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private GpuBuffer<GPLineSphere> <CutLineSpheres>k__BackingField;

		// Token: 0x0400311D RID: 12573
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private GpuBuffer<GPLineSphere> <GrowLineSpheres>k__BackingField;

		// Token: 0x0400311E RID: 12574
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private GpuBuffer<GPLineSphere> <HoldLineSpheres>k__BackingField;

		// Token: 0x0400311F RID: 12575
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private GpuBuffer<GPLineSphereWithMatrixDelta> <GrabLineSpheres>k__BackingField;

		// Token: 0x04003120 RID: 12576
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private GpuBuffer<GPLineSphere> <PushLineSpheres>k__BackingField;

		// Token: 0x04003121 RID: 12577
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private GpuBuffer<GPLineSphere> <PullLineSpheres>k__BackingField;

		// Token: 0x04003122 RID: 12578
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private GpuBuffer<GPLineSphereWithDelta> <BrushLineSpheres>k__BackingField;

		// Token: 0x04003123 RID: 12579
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private GpuBuffer<GPLineSphere> <RigidityIncreaseLineSpheres>k__BackingField;

		// Token: 0x04003124 RID: 12580
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private GpuBuffer<GPLineSphere> <RigidityDecreaseLineSpheres>k__BackingField;

		// Token: 0x04003125 RID: 12581
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private GpuBuffer<GPLineSphere> <RigiditySetLineSpheres>k__BackingField;

		// Token: 0x04003126 RID: 12582
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private GpuBuffer<Vector4> <Planes>k__BackingField;

		// Token: 0x04003127 RID: 12583
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private GroupedData<GPDistanceJoint> <DistanceJoints>k__BackingField;

		// Token: 0x04003128 RID: 12584
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private GpuBuffer<GPDistanceJoint> <DistanceJointsBuffer>k__BackingField;

		// Token: 0x04003129 RID: 12585
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private GroupedData<GPDistanceJoint> <CompressionJoints>k__BackingField;

		// Token: 0x0400312A RID: 12586
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private GpuBuffer<GPDistanceJoint> <CompressionJointsBuffer>k__BackingField;

		// Token: 0x0400312B RID: 12587
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private GroupedData<GPDistanceJoint> <NearbyDistanceJoints>k__BackingField;

		// Token: 0x0400312C RID: 12588
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private GpuBuffer<GPDistanceJoint> <NearbyDistanceJointsBuffer>k__BackingField;

		// Token: 0x0400312D RID: 12589
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private GpuBuffer<GPPointJoint> <PointJoints>k__BackingField;

		// Token: 0x0400312E RID: 12590
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private GpuBuffer<float> <PointToPreviousPointDistances>k__BackingField;

		// Token: 0x0400312F RID: 12591
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private GpuBuffer<Vector3> <Barycentrics>k__BackingField;

		// Token: 0x04003130 RID: 12592
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private GpuBuffer<Vector3> <BarycentricsFixed>k__BackingField;

		// Token: 0x04003131 RID: 12593
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private GpuBuffer<RenderParticle> <RenderParticles>k__BackingField;

		// Token: 0x04003132 RID: 12594
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private GpuBuffer<TessRenderParticle> <TessRenderParticles>k__BackingField;

		// Token: 0x04003133 RID: 12595
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private GpuBuffer<Vector3> <RandomsPerStrand>k__BackingField;

		// Token: 0x04003134 RID: 12596
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private GpuBuffer<GPParticle> <OutParticles>k__BackingField;

		// Token: 0x04003135 RID: 12597
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private GpuBuffer<float> <OutParticlesMap>k__BackingField;

		// Token: 0x04003136 RID: 12598
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private Vector3 <Wind>k__BackingField;
	}
}
