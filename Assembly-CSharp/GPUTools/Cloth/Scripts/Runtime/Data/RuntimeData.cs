using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using GPUTools.Cloth.Scripts.Types;
using GPUTools.Common.Scripts.PL.Tools;
using GPUTools.Physics.Scripts.Types.Dynamic;
using GPUTools.Physics.Scripts.Types.Joints;
using GPUTools.Physics.Scripts.Types.Shapes;
using UnityEngine;

namespace GPUTools.Cloth.Scripts.Runtime.Data
{
	// Token: 0x020009A3 RID: 2467
	public class RuntimeData
	{
		// Token: 0x06003DBB RID: 15803 RVA: 0x0012AF4E File Offset: 0x0012934E
		public RuntimeData()
		{
		}

		// Token: 0x170006FC RID: 1788
		// (get) Token: 0x06003DBC RID: 15804 RVA: 0x0012AF56 File Offset: 0x00129356
		// (set) Token: 0x06003DBD RID: 15805 RVA: 0x0012AF5E File Offset: 0x0012935E
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

		// Token: 0x170006FD RID: 1789
		// (get) Token: 0x06003DBE RID: 15806 RVA: 0x0012AF67 File Offset: 0x00129367
		// (set) Token: 0x06003DBF RID: 15807 RVA: 0x0012AF6F File Offset: 0x0012936F
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

		// Token: 0x170006FE RID: 1790
		// (get) Token: 0x06003DC0 RID: 15808 RVA: 0x0012AF78 File Offset: 0x00129378
		// (set) Token: 0x06003DC1 RID: 15809 RVA: 0x0012AF80 File Offset: 0x00129380
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

		// Token: 0x170006FF RID: 1791
		// (get) Token: 0x06003DC2 RID: 15810 RVA: 0x0012AF89 File Offset: 0x00129389
		// (set) Token: 0x06003DC3 RID: 15811 RVA: 0x0012AF91 File Offset: 0x00129391
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

		// Token: 0x17000700 RID: 1792
		// (get) Token: 0x06003DC4 RID: 15812 RVA: 0x0012AF9A File Offset: 0x0012939A
		// (set) Token: 0x06003DC5 RID: 15813 RVA: 0x0012AFA2 File Offset: 0x001293A2
		public GpuBuffer<GPPointJoint> AllPointJoints
		{
			[CompilerGenerated]
			get
			{
				return this.<AllPointJoints>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<AllPointJoints>k__BackingField = value;
			}
		}

		// Token: 0x17000701 RID: 1793
		// (get) Token: 0x06003DC6 RID: 15814 RVA: 0x0012AFAB File Offset: 0x001293AB
		// (set) Token: 0x06003DC7 RID: 15815 RVA: 0x0012AFB3 File Offset: 0x001293B3
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

		// Token: 0x17000702 RID: 1794
		// (get) Token: 0x06003DC8 RID: 15816 RVA: 0x0012AFBC File Offset: 0x001293BC
		// (set) Token: 0x06003DC9 RID: 15817 RVA: 0x0012AFC4 File Offset: 0x001293C4
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

		// Token: 0x17000703 RID: 1795
		// (get) Token: 0x06003DCA RID: 15818 RVA: 0x0012AFCD File Offset: 0x001293CD
		// (set) Token: 0x06003DCB RID: 15819 RVA: 0x0012AFD5 File Offset: 0x001293D5
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

		// Token: 0x17000704 RID: 1796
		// (get) Token: 0x06003DCC RID: 15820 RVA: 0x0012AFDE File Offset: 0x001293DE
		// (set) Token: 0x06003DCD RID: 15821 RVA: 0x0012AFE6 File Offset: 0x001293E6
		public GroupedData<GPDistanceJoint> StiffnessJoints
		{
			[CompilerGenerated]
			get
			{
				return this.<StiffnessJoints>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<StiffnessJoints>k__BackingField = value;
			}
		}

		// Token: 0x17000705 RID: 1797
		// (get) Token: 0x06003DCE RID: 15822 RVA: 0x0012AFEF File Offset: 0x001293EF
		// (set) Token: 0x06003DCF RID: 15823 RVA: 0x0012AFF7 File Offset: 0x001293F7
		public GpuBuffer<GPDistanceJoint> StiffnessJointsBuffer
		{
			[CompilerGenerated]
			get
			{
				return this.<StiffnessJointsBuffer>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<StiffnessJointsBuffer>k__BackingField = value;
			}
		}

		// Token: 0x17000706 RID: 1798
		// (get) Token: 0x06003DD0 RID: 15824 RVA: 0x0012B000 File Offset: 0x00129400
		// (set) Token: 0x06003DD1 RID: 15825 RVA: 0x0012B008 File Offset: 0x00129408
		public GroupedData<GPDistanceJoint> NearbyJoints
		{
			[CompilerGenerated]
			get
			{
				return this.<NearbyJoints>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<NearbyJoints>k__BackingField = value;
			}
		}

		// Token: 0x17000707 RID: 1799
		// (get) Token: 0x06003DD2 RID: 15826 RVA: 0x0012B011 File Offset: 0x00129411
		// (set) Token: 0x06003DD3 RID: 15827 RVA: 0x0012B019 File Offset: 0x00129419
		public GpuBuffer<GPDistanceJoint> NearbyJointsBuffer
		{
			[CompilerGenerated]
			get
			{
				return this.<NearbyJointsBuffer>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<NearbyJointsBuffer>k__BackingField = value;
			}
		}

		// Token: 0x17000708 RID: 1800
		// (get) Token: 0x06003DD4 RID: 15828 RVA: 0x0012B022 File Offset: 0x00129422
		// (set) Token: 0x06003DD5 RID: 15829 RVA: 0x0012B02A File Offset: 0x0012942A
		public GpuBuffer<GPGrabSphere> GrabSpheres
		{
			[CompilerGenerated]
			get
			{
				return this.<GrabSpheres>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<GrabSpheres>k__BackingField = value;
			}
		}

		// Token: 0x17000709 RID: 1801
		// (get) Token: 0x06003DD6 RID: 15830 RVA: 0x0012B033 File Offset: 0x00129433
		// (set) Token: 0x06003DD7 RID: 15831 RVA: 0x0012B03B File Offset: 0x0012943B
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

		// Token: 0x1700070A RID: 1802
		// (get) Token: 0x06003DD8 RID: 15832 RVA: 0x0012B044 File Offset: 0x00129444
		// (set) Token: 0x06003DD9 RID: 15833 RVA: 0x0012B04C File Offset: 0x0012944C
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

		// Token: 0x1700070B RID: 1803
		// (get) Token: 0x06003DDA RID: 15834 RVA: 0x0012B055 File Offset: 0x00129455
		// (set) Token: 0x06003DDB RID: 15835 RVA: 0x0012B05D File Offset: 0x0012945D
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

		// Token: 0x1700070C RID: 1804
		// (get) Token: 0x06003DDC RID: 15836 RVA: 0x0012B066 File Offset: 0x00129466
		// (set) Token: 0x06003DDD RID: 15837 RVA: 0x0012B06E File Offset: 0x0012946E
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

		// Token: 0x1700070D RID: 1805
		// (get) Token: 0x06003DDE RID: 15838 RVA: 0x0012B077 File Offset: 0x00129477
		// (set) Token: 0x06003DDF RID: 15839 RVA: 0x0012B07F File Offset: 0x0012947F
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

		// Token: 0x1700070E RID: 1806
		// (get) Token: 0x06003DE0 RID: 15840 RVA: 0x0012B088 File Offset: 0x00129488
		// (set) Token: 0x06003DE1 RID: 15841 RVA: 0x0012B090 File Offset: 0x00129490
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

		// Token: 0x1700070F RID: 1807
		// (get) Token: 0x06003DE2 RID: 15842 RVA: 0x0012B099 File Offset: 0x00129499
		// (set) Token: 0x06003DE3 RID: 15843 RVA: 0x0012B0A1 File Offset: 0x001294A1
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

		// Token: 0x17000710 RID: 1808
		// (get) Token: 0x06003DE4 RID: 15844 RVA: 0x0012B0AA File Offset: 0x001294AA
		// (set) Token: 0x06003DE5 RID: 15845 RVA: 0x0012B0B2 File Offset: 0x001294B2
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

		// Token: 0x04002F40 RID: 12096
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private GpuBuffer<GPParticle> <Particles>k__BackingField;

		// Token: 0x04002F41 RID: 12097
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private GpuBuffer<Vector4> <Planes>k__BackingField;

		// Token: 0x04002F42 RID: 12098
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private GpuBuffer<GPSphereWithDelta> <ProcessedSpheres>k__BackingField;

		// Token: 0x04002F43 RID: 12099
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private GpuBuffer<GPLineSphereWithDelta> <ProcessedLineSpheres>k__BackingField;

		// Token: 0x04002F44 RID: 12100
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private GpuBuffer<GPPointJoint> <AllPointJoints>k__BackingField;

		// Token: 0x04002F45 RID: 12101
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private GpuBuffer<GPPointJoint> <PointJoints>k__BackingField;

		// Token: 0x04002F46 RID: 12102
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private GroupedData<GPDistanceJoint> <DistanceJoints>k__BackingField;

		// Token: 0x04002F47 RID: 12103
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private GpuBuffer<GPDistanceJoint> <DistanceJointsBuffer>k__BackingField;

		// Token: 0x04002F48 RID: 12104
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private GroupedData<GPDistanceJoint> <StiffnessJoints>k__BackingField;

		// Token: 0x04002F49 RID: 12105
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private GpuBuffer<GPDistanceJoint> <StiffnessJointsBuffer>k__BackingField;

		// Token: 0x04002F4A RID: 12106
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private GroupedData<GPDistanceJoint> <NearbyJoints>k__BackingField;

		// Token: 0x04002F4B RID: 12107
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private GpuBuffer<GPDistanceJoint> <NearbyJointsBuffer>k__BackingField;

		// Token: 0x04002F4C RID: 12108
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private GpuBuffer<GPGrabSphere> <GrabSpheres>k__BackingField;

		// Token: 0x04002F4D RID: 12109
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private GpuBuffer<ClothVertex> <ClothVertices>k__BackingField;

		// Token: 0x04002F4E RID: 12110
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private GpuBuffer<Vector3> <ClothOnlyVertices>k__BackingField;

		// Token: 0x04002F4F RID: 12111
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private GpuBuffer<int> <MeshToPhysicsVerticesMap>k__BackingField;

		// Token: 0x04002F50 RID: 12112
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private GpuBuffer<int> <MeshVertexToNeiborsMap>k__BackingField;

		// Token: 0x04002F51 RID: 12113
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private GpuBuffer<int> <MeshVertexToNeiborsMapCounts>k__BackingField;

		// Token: 0x04002F52 RID: 12114
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private GpuBuffer<GPParticle> <OutParticles>k__BackingField;

		// Token: 0x04002F53 RID: 12115
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private GpuBuffer<float> <OutParticlesMap>k__BackingField;

		// Token: 0x04002F54 RID: 12116
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private Vector3 <Wind>k__BackingField;
	}
}
