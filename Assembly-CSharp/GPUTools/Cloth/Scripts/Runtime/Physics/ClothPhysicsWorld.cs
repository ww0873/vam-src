using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using GPUTools.Cloth.Scripts.Runtime.Data;
using GPUTools.Cloth.Scripts.Runtime.Kernels;
using GPUTools.Cloth.Scripts.Types;
using GPUTools.Common.Scripts.PL.Abstract;
using GPUTools.Common.Scripts.PL.Attributes;
using GPUTools.Common.Scripts.PL.Tools;
using GPUTools.Common.Scripts.Tools.Kernels;
using GPUTools.Physics.Scripts.DebugDraw;
using GPUTools.Physics.Scripts.Kernels;
using GPUTools.Physics.Scripts.Types.Dynamic;
using GPUTools.Physics.Scripts.Types.Joints;
using GPUTools.Physics.Scripts.Types.Shapes;
using GPUTools.Skinner.Scripts.Providers;
using UnityEngine;

namespace GPUTools.Cloth.Scripts.Runtime.Physics
{
	// Token: 0x020009A8 RID: 2472
	public class ClothPhysicsWorld : PrimitiveBase
	{
		// Token: 0x06003E13 RID: 15891 RVA: 0x0012BA10 File Offset: 0x00129E10
		public ClothPhysicsWorld(ClothDataFacade data)
		{
			this.data = data;
			this.T = new GpuValue<float>(0f);
			this.DT = new GpuValue<float>(0f);
			this.DTRecip = new GpuValue<float>(0f);
			this.Weight = new GpuValue<float>(0f);
			this.Step = new GpuValue<float>(0f);
			this.AccelDT2 = new GpuValue<Vector3>(default(Vector3));
			this.InvDrag = new GpuValue<float>(0f);
			this.Stiffness = new GpuValue<float>(0f);
			this.CompressionResistance = new GpuValue<float>(0f);
			this.DistanceScale = new GpuValue<float>(0f);
			this.BreakThreshold = new GpuValue<float>(0f);
			this.JointStrength = new GpuValue<float>(0f);
			this.JointPrediction = new GpuValue<float>(0f);
			this.Friction = new GpuValue<float>(0f);
			this.StaticFriction = new GpuValue<float>(0f);
			this.CollisionPower = new GpuValue<float>(0f);
			this.InitData();
			this.InitBuffers();
			this.InitPasses();
			base.Bind();
		}

		// Token: 0x17000720 RID: 1824
		// (get) Token: 0x06003E15 RID: 15893 RVA: 0x0012BB4F File Offset: 0x00129F4F
		// (set) Token: 0x06003E14 RID: 15892 RVA: 0x0012BB46 File Offset: 0x00129F46
		[GpuData("weight")]
		public GpuValue<float> Weight
		{
			[CompilerGenerated]
			get
			{
				return this.<Weight>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<Weight>k__BackingField = value;
			}
		}

		// Token: 0x17000721 RID: 1825
		// (get) Token: 0x06003E17 RID: 15895 RVA: 0x0012BB60 File Offset: 0x00129F60
		// (set) Token: 0x06003E16 RID: 15894 RVA: 0x0012BB57 File Offset: 0x00129F57
		[GpuData("step")]
		public GpuValue<float> Step
		{
			[CompilerGenerated]
			get
			{
				return this.<Step>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<Step>k__BackingField = value;
			}
		}

		// Token: 0x17000722 RID: 1826
		// (get) Token: 0x06003E19 RID: 15897 RVA: 0x0012BB71 File Offset: 0x00129F71
		// (set) Token: 0x06003E18 RID: 15896 RVA: 0x0012BB68 File Offset: 0x00129F68
		[GpuData("dt")]
		public GpuValue<float> DT
		{
			[CompilerGenerated]
			get
			{
				return this.<DT>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<DT>k__BackingField = value;
			}
		}

		// Token: 0x17000723 RID: 1827
		// (get) Token: 0x06003E1B RID: 15899 RVA: 0x0012BB82 File Offset: 0x00129F82
		// (set) Token: 0x06003E1A RID: 15898 RVA: 0x0012BB79 File Offset: 0x00129F79
		[GpuData("dtrecip")]
		public GpuValue<float> DTRecip
		{
			[CompilerGenerated]
			get
			{
				return this.<DTRecip>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<DTRecip>k__BackingField = value;
			}
		}

		// Token: 0x17000724 RID: 1828
		// (get) Token: 0x06003E1D RID: 15901 RVA: 0x0012BB93 File Offset: 0x00129F93
		// (set) Token: 0x06003E1C RID: 15900 RVA: 0x0012BB8A File Offset: 0x00129F8A
		[GpuData("t")]
		public GpuValue<float> T
		{
			[CompilerGenerated]
			get
			{
				return this.<T>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<T>k__BackingField = value;
			}
		}

		// Token: 0x17000725 RID: 1829
		// (get) Token: 0x06003E1F RID: 15903 RVA: 0x0012BBA4 File Offset: 0x00129FA4
		// (set) Token: 0x06003E1E RID: 15902 RVA: 0x0012BB9B File Offset: 0x00129F9B
		[GpuData("accelDT2")]
		public GpuValue<Vector3> AccelDT2
		{
			[CompilerGenerated]
			get
			{
				return this.<AccelDT2>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<AccelDT2>k__BackingField = value;
			}
		}

		// Token: 0x17000726 RID: 1830
		// (get) Token: 0x06003E21 RID: 15905 RVA: 0x0012BBB5 File Offset: 0x00129FB5
		// (set) Token: 0x06003E20 RID: 15904 RVA: 0x0012BBAC File Offset: 0x00129FAC
		[GpuData("invDrag")]
		public GpuValue<float> InvDrag
		{
			[CompilerGenerated]
			get
			{
				return this.<InvDrag>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<InvDrag>k__BackingField = value;
			}
		}

		// Token: 0x17000727 RID: 1831
		// (get) Token: 0x06003E23 RID: 15907 RVA: 0x0012BBC6 File Offset: 0x00129FC6
		// (set) Token: 0x06003E22 RID: 15906 RVA: 0x0012BBBD File Offset: 0x00129FBD
		[GpuData("stiffness")]
		public GpuValue<float> Stiffness
		{
			[CompilerGenerated]
			get
			{
				return this.<Stiffness>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<Stiffness>k__BackingField = value;
			}
		}

		// Token: 0x17000728 RID: 1832
		// (get) Token: 0x06003E25 RID: 15909 RVA: 0x0012BBD7 File Offset: 0x00129FD7
		// (set) Token: 0x06003E24 RID: 15908 RVA: 0x0012BBCE File Offset: 0x00129FCE
		[GpuData("distanceScale")]
		public GpuValue<float> DistanceScale
		{
			[CompilerGenerated]
			get
			{
				return this.<DistanceScale>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<DistanceScale>k__BackingField = value;
			}
		}

		// Token: 0x17000729 RID: 1833
		// (get) Token: 0x06003E27 RID: 15911 RVA: 0x0012BBE8 File Offset: 0x00129FE8
		// (set) Token: 0x06003E26 RID: 15910 RVA: 0x0012BBDF File Offset: 0x00129FDF
		[GpuData("compressionResistance")]
		public GpuValue<float> CompressionResistance
		{
			[CompilerGenerated]
			get
			{
				return this.<CompressionResistance>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<CompressionResistance>k__BackingField = value;
			}
		}

		// Token: 0x1700072A RID: 1834
		// (get) Token: 0x06003E29 RID: 15913 RVA: 0x0012BBF9 File Offset: 0x00129FF9
		// (set) Token: 0x06003E28 RID: 15912 RVA: 0x0012BBF0 File Offset: 0x00129FF0
		[GpuData("breakThreshold")]
		public GpuValue<float> BreakThreshold
		{
			[CompilerGenerated]
			get
			{
				return this.<BreakThreshold>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<BreakThreshold>k__BackingField = value;
			}
		}

		// Token: 0x1700072B RID: 1835
		// (get) Token: 0x06003E2B RID: 15915 RVA: 0x0012BC0A File Offset: 0x0012A00A
		// (set) Token: 0x06003E2A RID: 15914 RVA: 0x0012BC01 File Offset: 0x0012A001
		[GpuData("jointStrength")]
		public GpuValue<float> JointStrength
		{
			[CompilerGenerated]
			get
			{
				return this.<JointStrength>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<JointStrength>k__BackingField = value;
			}
		}

		// Token: 0x1700072C RID: 1836
		// (get) Token: 0x06003E2D RID: 15917 RVA: 0x0012BC1B File Offset: 0x0012A01B
		// (set) Token: 0x06003E2C RID: 15916 RVA: 0x0012BC12 File Offset: 0x0012A012
		[GpuData("jointPrediction")]
		public GpuValue<float> JointPrediction
		{
			[CompilerGenerated]
			get
			{
				return this.<JointPrediction>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<JointPrediction>k__BackingField = value;
			}
		}

		// Token: 0x1700072D RID: 1837
		// (get) Token: 0x06003E2F RID: 15919 RVA: 0x0012BC2C File Offset: 0x0012A02C
		// (set) Token: 0x06003E2E RID: 15918 RVA: 0x0012BC23 File Offset: 0x0012A023
		[GpuData("friction")]
		public GpuValue<float> Friction
		{
			[CompilerGenerated]
			get
			{
				return this.<Friction>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<Friction>k__BackingField = value;
			}
		}

		// Token: 0x1700072E RID: 1838
		// (get) Token: 0x06003E31 RID: 15921 RVA: 0x0012BC3D File Offset: 0x0012A03D
		// (set) Token: 0x06003E30 RID: 15920 RVA: 0x0012BC34 File Offset: 0x0012A034
		[GpuData("staticFriction")]
		public GpuValue<float> StaticFriction
		{
			[CompilerGenerated]
			get
			{
				return this.<StaticFriction>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<StaticFriction>k__BackingField = value;
			}
		}

		// Token: 0x1700072F RID: 1839
		// (get) Token: 0x06003E33 RID: 15923 RVA: 0x0012BC4E File Offset: 0x0012A04E
		// (set) Token: 0x06003E32 RID: 15922 RVA: 0x0012BC45 File Offset: 0x0012A045
		[GpuData("collisionPower")]
		public GpuValue<float> CollisionPower
		{
			[CompilerGenerated]
			get
			{
				return this.<CollisionPower>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<CollisionPower>k__BackingField = value;
			}
		}

		// Token: 0x17000730 RID: 1840
		// (get) Token: 0x06003E35 RID: 15925 RVA: 0x0012BC5F File Offset: 0x0012A05F
		// (set) Token: 0x06003E34 RID: 15924 RVA: 0x0012BC56 File Offset: 0x0012A056
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

		// Token: 0x17000731 RID: 1841
		// (get) Token: 0x06003E37 RID: 15927 RVA: 0x0012BC70 File Offset: 0x0012A070
		// (set) Token: 0x06003E36 RID: 15926 RVA: 0x0012BC67 File Offset: 0x0012A067
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

		// Token: 0x17000732 RID: 1842
		// (get) Token: 0x06003E39 RID: 15929 RVA: 0x0012BC81 File Offset: 0x0012A081
		// (set) Token: 0x06003E38 RID: 15928 RVA: 0x0012BC78 File Offset: 0x0012A078
		[GpuData("oldTransforms")]
		public GpuBuffer<Matrix4x4> OldTransforms
		{
			[CompilerGenerated]
			get
			{
				return this.<OldTransforms>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<OldTransforms>k__BackingField = value;
			}
		}

		// Token: 0x17000733 RID: 1843
		// (get) Token: 0x06003E3B RID: 15931 RVA: 0x0012BC92 File Offset: 0x0012A092
		// (set) Token: 0x06003E3A RID: 15930 RVA: 0x0012BC89 File Offset: 0x0012A089
		[GpuData("positions")]
		public GpuBuffer<Vector3> PreCalculatedPositions
		{
			[CompilerGenerated]
			get
			{
				return this.<PreCalculatedPositions>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<PreCalculatedPositions>k__BackingField = value;
			}
		}

		// Token: 0x17000734 RID: 1844
		// (get) Token: 0x06003E3D RID: 15933 RVA: 0x0012BCA3 File Offset: 0x0012A0A3
		// (set) Token: 0x06003E3C RID: 15932 RVA: 0x0012BC9A File Offset: 0x0012A09A
		[GpuData("oldPositions")]
		public GpuBuffer<Vector3> OldPreCalculatedPositions
		{
			[CompilerGenerated]
			get
			{
				return this.<OldPreCalculatedPositions>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<OldPreCalculatedPositions>k__BackingField = value;
			}
		}

		// Token: 0x17000735 RID: 1845
		// (get) Token: 0x06003E3F RID: 15935 RVA: 0x0012BCB4 File Offset: 0x0012A0B4
		// (set) Token: 0x06003E3E RID: 15934 RVA: 0x0012BCAB File Offset: 0x0012A0AB
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

		// Token: 0x17000736 RID: 1846
		// (get) Token: 0x06003E41 RID: 15937 RVA: 0x0012BCC5 File Offset: 0x0012A0C5
		// (set) Token: 0x06003E40 RID: 15936 RVA: 0x0012BCBC File Offset: 0x0012A0BC
		[GpuData("allPointJoints")]
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

		// Token: 0x17000737 RID: 1847
		// (get) Token: 0x06003E43 RID: 15939 RVA: 0x0012BCD6 File Offset: 0x0012A0D6
		// (set) Token: 0x06003E42 RID: 15938 RVA: 0x0012BCCD File Offset: 0x0012A0CD
		[GpuData("grabSpheres")]
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

		// Token: 0x17000738 RID: 1848
		// (get) Token: 0x06003E45 RID: 15941 RVA: 0x0012BCE7 File Offset: 0x0012A0E7
		// (set) Token: 0x06003E44 RID: 15940 RVA: 0x0012BCDE File Offset: 0x0012A0DE
		[GpuData("processedSpheres")]
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

		// Token: 0x17000739 RID: 1849
		// (get) Token: 0x06003E47 RID: 15943 RVA: 0x0012BCF8 File Offset: 0x0012A0F8
		// (set) Token: 0x06003E46 RID: 15942 RVA: 0x0012BCEF File Offset: 0x0012A0EF
		[GpuData("processedLineSpheres")]
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

		// Token: 0x1700073A RID: 1850
		// (get) Token: 0x06003E49 RID: 15945 RVA: 0x0012BD09 File Offset: 0x0012A109
		// (set) Token: 0x06003E48 RID: 15944 RVA: 0x0012BD00 File Offset: 0x0012A100
		[GpuData("planes")]
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

		// Token: 0x1700073B RID: 1851
		// (get) Token: 0x06003E4B RID: 15947 RVA: 0x0012BD1A File Offset: 0x0012A11A
		// (set) Token: 0x06003E4A RID: 15946 RVA: 0x0012BD11 File Offset: 0x0012A111
		[GpuData("outParticles")]
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

		// Token: 0x1700073C RID: 1852
		// (get) Token: 0x06003E4D RID: 15949 RVA: 0x0012BD2B File Offset: 0x0012A12B
		// (set) Token: 0x06003E4C RID: 15948 RVA: 0x0012BD22 File Offset: 0x0012A122
		[GpuData("outParticlesMap")]
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

		// Token: 0x1700073D RID: 1853
		// (get) Token: 0x06003E4E RID: 15950 RVA: 0x0012BD33 File Offset: 0x0012A133
		// (set) Token: 0x06003E4F RID: 15951 RVA: 0x0012BD3B File Offset: 0x0012A13B
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

		// Token: 0x1700073E RID: 1854
		// (get) Token: 0x06003E50 RID: 15952 RVA: 0x0012BD44 File Offset: 0x0012A144
		// (set) Token: 0x06003E51 RID: 15953 RVA: 0x0012BD4C File Offset: 0x0012A14C
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

		// Token: 0x1700073F RID: 1855
		// (get) Token: 0x06003E52 RID: 15954 RVA: 0x0012BD55 File Offset: 0x0012A155
		// (set) Token: 0x06003E53 RID: 15955 RVA: 0x0012BD5D File Offset: 0x0012A15D
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

		// Token: 0x17000740 RID: 1856
		// (get) Token: 0x06003E54 RID: 15956 RVA: 0x0012BD66 File Offset: 0x0012A166
		// (set) Token: 0x06003E55 RID: 15957 RVA: 0x0012BD6E File Offset: 0x0012A16E
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

		// Token: 0x17000741 RID: 1857
		// (get) Token: 0x06003E56 RID: 15958 RVA: 0x0012BD77 File Offset: 0x0012A177
		// (set) Token: 0x06003E57 RID: 15959 RVA: 0x0012BD7F File Offset: 0x0012A17F
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

		// Token: 0x06003E58 RID: 15960 RVA: 0x0012BD88 File Offset: 0x0012A188
		private void InitData()
		{
			if (Time.fixedDeltaTime > 0.02f)
			{
				this.outerIterations = 2;
				this.iterations = this.data.Iterations;
			}
			else
			{
				this.outerIterations = 1;
				this.iterations = this.data.Iterations;
			}
			this.Weight.Value = this.data.Weight;
			this.InvDrag.Value = this.data.InvDrag;
			this.Stiffness.Value = this.data.Stiffness;
			this.CompressionResistance.Value = this.data.CompressionResistance;
			this.DistanceScale.Value = this.data.DistanceScale * this.data.WorldScale;
			this.Friction.Value = this.data.Friction;
			this.StaticFriction.Value = this.data.Friction * this.data.StaticMultiplier;
			this.CollisionPower.Value = this.data.CollisionPower;
			if (this.fixedDispatchCount == 1)
			{
				this.JointPrediction.Value = 1f;
			}
			else if (this.fixedDispatchCount == 2)
			{
				this.JointPrediction.Value = 1.05f;
			}
			else
			{
				this.JointPrediction.Value = 1.1f;
			}
			this.JointStrength.Value = this.data.JointStrength;
			if (this.data.BreakEnabled && this.frame > 15)
			{
				this.BreakThreshold.Value = this.data.BreakThreshold;
			}
			else
			{
				this.BreakThreshold.Value = 1000000f;
			}
		}

		// Token: 0x06003E59 RID: 15961 RVA: 0x0012BF54 File Offset: 0x0012A354
		private void InitBuffers()
		{
			this.usesPreCalcVerts = (this.data.MeshProvider.Type == ScalpMeshType.PreCalc);
			this.Particles = this.data.Particles;
			if (this.usesPreCalcVerts)
			{
				this.PreCalculatedPositions = this.data.PreCalculatedVerticesBuffer;
				this.OldPreCalculatedPositions = new GpuBuffer<Vector3>(this.PreCalculatedPositions.Data, 12);
				this.OldPreCalculatedPositions.PushData();
			}
			else
			{
				this.Transforms = this.data.MatricesBuffer;
				this.OldTransforms = new GpuBuffer<Matrix4x4>(this.Transforms.Data, 64);
				this.OldTransforms.PushData();
			}
			this.AllPointJoints = this.data.AllPointJoints;
			this.PointJoints = this.data.PointJoints;
			this.ProcessedSpheres = this.data.ProcessedSpheres;
			this.ProcessedLineSpheres = this.data.ProcessedLineSpheres;
			this.Planes = this.data.Planes;
			this.GrabSpheres = this.data.GrabSpheres;
			this.ClothVertices = this.data.ClothVertices;
			this.ClothOnlyVertices = this.data.ClothOnlyVertices;
			this.MeshToPhysicsVerticesMap = this.data.MeshToPhysicsVerticesMap;
			this.MeshVertexToNeiborsMap = this.data.MeshVertexToNeiborsMap;
			this.MeshVertexToNeiborsMapCounts = this.data.MeshVertexToNeiborsMapCounts;
			this.OutParticles = this.data.OutParticles;
			this.OutParticlesMap = this.data.OutParticlesMap;
		}

		// Token: 0x06003E5A RID: 15962 RVA: 0x0012C0E0 File Offset: 0x0012A4E0
		private void UpdateBuffers()
		{
			if (this.ProcessedLineSpheres != this.data.ProcessedLineSpheres)
			{
				this.ProcessedLineSpheres = this.data.ProcessedLineSpheres;
				this.lineSphereCollisionKernel.ProcessedLineSpheres = this.ProcessedLineSpheres;
				this.lineSphereCollisionKernel.ClearCacheAttributes();
			}
			if (this.ProcessedSpheres != this.data.ProcessedSpheres)
			{
				this.ProcessedSpheres = this.data.ProcessedSpheres;
				this.sphereCollisionKernel.ProcessedSpheres = this.ProcessedSpheres;
				this.sphereCollisionKernel.ClearCacheAttributes();
			}
			if (this.Planes != this.data.Planes)
			{
				this.Planes = this.data.Planes;
				this.particlePlaneCollisionKernel.Planes = this.Planes;
				this.particlePlaneCollisionKernel.ClearCacheAttributes();
			}
			if (this.GrabSpheres != this.data.GrabSpheres)
			{
				this.GrabSpheres = this.data.GrabSpheres;
				this.grabJointsKernel.GrabSpheres = this.GrabSpheres;
				this.grabJointsKernel.ClearCacheAttributes();
			}
			if (this.usesPreCalcVerts && this.PreCalculatedPositions != this.data.PreCalculatedVerticesBuffer)
			{
				this.PreCalculatedPositions = this.data.PreCalculatedVerticesBuffer;
				this.vector3CopyPaster.Vector3From = this.PreCalculatedPositions;
				this.vector3CopyPaster.ClearCacheAttributes();
				this.pointJointsPreCalculatedKernel.Positions = this.PreCalculatedPositions;
				this.pointJointsPreCalculatedKernel.ClearCacheAttributes();
				this.pointJointsPreCalculatedFinalKernel.Positions = this.PreCalculatedPositions;
				this.pointJointsPreCalculatedFinalKernel.ClearCacheAttributes();
				this.resetToPointJointsPreCalculatedKernel.Positions = this.PreCalculatedPositions;
				this.resetToPointJointsPreCalculatedKernel.ClearCacheAttributes();
				this.resetToPointJointsPreCalculatedKernel.Dispatch();
			}
		}

		// Token: 0x06003E5B RID: 15963 RVA: 0x0012C2A8 File Offset: 0x0012A6A8
		private void InitPasses()
		{
			if (this.usesPreCalcVerts)
			{
				base.AddPass(this.resetToPointJointsPreCalculatedKernel = new ResetToPointJointsPreCalculatedKernel());
			}
			else
			{
				base.AddPass(this.resetKernel = new ResetToPointJointsKernel());
			}
			base.AddPass(this.integrateVelocityKernel = new IntegrateVelocityKernel());
			base.AddPass(this.integrateVelocityInnerKernel = new IntegrateVelocityInnerKernel());
			base.AddPass(this.integrateIterKernel = new IntegrateIterKernel());
			base.AddPass(this.stiffnessJointsKernel = new StiffnessJointsKernel(this.data.StiffnessJoints, this.data.StiffnessJointsBuffer));
			base.AddPass(this.nearbyJointsKernel = new StiffnessJointsKernel(this.data.NearbyJoints, this.data.NearbyJointsBuffer));
			base.AddPass(this.particleCollisionResetKernel = new ParticleCollisionResetKernel());
			base.AddPass(this.particlePlaneCollisionKernel = new ParticlePlaneCollisionKernel());
			base.AddPass(this.sphereCollisionKernel = new ParticleSphereCollisionKernel());
			base.AddPass(this.lineSphereCollisionKernel = new ParticleLineSphereCollisionKernel());
			if (this.data.PointJoints != null)
			{
				if (this.usesPreCalcVerts)
				{
					base.AddPass(this.pointJointsPreCalculatedKernel = new PointJointsPreCalculatedKernel());
					base.AddPass(this.pointJointsPreCalculatedFinalKernel = new PointJointsPreCalculatedFinalKernel());
				}
				else
				{
					base.AddPass(this.pointJointsKernel = new PointJointsKernel());
					base.AddPass(this.pointJointsFinalKernel = new PointJointsFinalKernel());
				}
			}
			base.AddPass(this.grabJointsKernel = new GrabJointsKernel());
			if (this.data.OutParticles != null)
			{
				base.AddPass(this.copySpecificParticlesKernel = new CopySpecificParticlesKernel());
			}
			if (this.usesPreCalcVerts)
			{
				base.AddPass(this.createVertexOnlyDataKernel = new CreateVertexOnlyDataKernel());
				base.AddPass(this.vector3CopyPaster = new GPUVector3CopyPaster(this.PreCalculatedPositions, this.OldPreCalculatedPositions));
			}
			else
			{
				base.AddPass(this.createVertexDataKernel = new CreateVertexDataKernel());
				base.AddPass(this.matrixCopyPaster = new GPUMatrixCopyPaster(this.Transforms, this.OldTransforms));
			}
		}

		// Token: 0x06003E5C RID: 15964 RVA: 0x0012C50F File Offset: 0x0012A90F
		public void FixedDispatch()
		{
			this.fixedDispatchCount++;
			this.DispatchPhysicsImpl();
		}

		// Token: 0x06003E5D RID: 15965 RVA: 0x0012C525 File Offset: 0x0012A925
		public void DispatchCopyToOld()
		{
			if (this.usesPreCalcVerts)
			{
				this.vector3CopyPaster.Dispatch();
			}
			else
			{
				this.matrixCopyPaster.Dispatch();
			}
		}

		// Token: 0x06003E5E RID: 15966 RVA: 0x0012C54D File Offset: 0x0012A94D
		public override void Dispatch()
		{
			this.fixedDispatchCount = 0;
			this.DispatchImpl();
		}

		// Token: 0x06003E5F RID: 15967 RVA: 0x0012C55C File Offset: 0x0012A95C
		public void Reset()
		{
			this.frame = 0;
		}

		// Token: 0x06003E60 RID: 15968 RVA: 0x0012C565 File Offset: 0x0012A965
		public void PartialReset()
		{
			if (this.frame > 10)
			{
				this.frame = 10;
			}
		}

		// Token: 0x06003E61 RID: 15969 RVA: 0x0012C57C File Offset: 0x0012A97C
		private void DispatchPhysicsImpl()
		{
			this.InitData();
			this.UpdateBuffers();
			if (this.frame < 10)
			{
				if (this.usesPreCalcVerts)
				{
					this.vector3CopyPaster.Dispatch();
					this.resetToPointJointsPreCalculatedKernel.Dispatch();
				}
				else
				{
					this.matrixCopyPaster.Dispatch();
					this.resetKernel.Dispatch();
				}
				this.particleCollisionResetKernel.Dispatch();
			}
			else
			{
				for (int i = 1; i <= this.outerIterations; i++)
				{
					float num = Time.fixedDeltaTime * Mathf.Max(this.data.Weight, 0.01f) / (float)this.outerIterations;
					this.Step.Value = 1f / (float)(this.iterations * this.outerIterations);
					float num2 = num / (float)this.iterations;
					this.DT.Value = num2;
					this.DTRecip.Value = 1f / num2;
					this.AccelDT2.Value = num2 * num2 * 0.5f * (this.data.Gravity + this.data.Wind);
					for (int j = 1; j <= this.iterations; j++)
					{
						this.T.Value = (float)j / (float)this.iterations;
						if (this.data.IntegrateEnabled && this.frame > 20)
						{
							this.integrateIterKernel.Dispatch();
						}
						if (this.pointJointsPreCalculatedKernel != null)
						{
							this.pointJointsPreCalculatedKernel.Dispatch();
						}
						else if (this.pointJointsKernel != null)
						{
							this.pointJointsKernel.Dispatch();
						}
						if (this.data.Stiffness > 0f)
						{
							this.stiffnessJointsKernel.Dispatch();
							this.nearbyJointsKernel.Dispatch();
						}
						this.integrateVelocityInnerKernel.Dispatch();
					}
					this.Step.Value = 1f / (float)this.outerIterations;
					this.DT.Value = num;
					this.DTRecip.Value = 1f / num;
					if (this.data.CollisionEnabled)
					{
						this.particleCollisionResetKernel.Dispatch();
						if (this.particleNeiborsCollisionKernel != null)
						{
							this.particleNeiborsCollisionKernel.Dispatch();
						}
						if (this.particlePlaneCollisionKernel.Planes != null)
						{
							this.particlePlaneCollisionKernel.Dispatch();
						}
						if (this.sphereCollisionKernel.ProcessedSpheres != null)
						{
							this.sphereCollisionKernel.Dispatch();
						}
						if (this.lineSphereCollisionKernel.ProcessedLineSpheres != null)
						{
							this.lineSphereCollisionKernel.Dispatch();
						}
						if (this.grabJointsKernel.GrabSpheres != null)
						{
							this.grabJointsKernel.Dispatch();
						}
					}
					this.integrateVelocityKernel.Dispatch();
				}
			}
		}

		// Token: 0x06003E62 RID: 15970 RVA: 0x0012C840 File Offset: 0x0012AC40
		private void DispatchImpl()
		{
			this.InitData();
			this.UpdateBuffers();
			this.Step.Value = 1f;
			if (this.frame < 1)
			{
				if (this.usesPreCalcVerts)
				{
					this.vector3CopyPaster.Dispatch();
					this.resetToPointJointsPreCalculatedKernel.Dispatch();
				}
				else
				{
					this.matrixCopyPaster.Dispatch();
					this.resetKernel.Dispatch();
				}
			}
			if (this.pointJointsPreCalculatedFinalKernel != null)
			{
				this.pointJointsPreCalculatedFinalKernel.Dispatch();
			}
			else if (this.pointJointsFinalKernel != null)
			{
				this.pointJointsFinalKernel.Dispatch();
			}
			if (this.copySpecificParticlesKernel != null)
			{
				this.copySpecificParticlesKernel.Dispatch();
			}
			if (this.createVertexOnlyDataKernel != null)
			{
				this.createVertexOnlyDataKernel.Dispatch();
			}
			if (this.createVertexDataKernel != null)
			{
				this.createVertexDataKernel.Dispatch();
			}
			this.frame++;
		}

		// Token: 0x06003E63 RID: 15971 RVA: 0x0012C932 File Offset: 0x0012AD32
		public override void Dispose()
		{
			base.Dispose();
			if (this.usesPreCalcVerts)
			{
				this.OldPreCalculatedPositions.Dispose();
			}
			else
			{
				this.OldTransforms.Dispose();
			}
		}

		// Token: 0x06003E64 RID: 15972 RVA: 0x0012C960 File Offset: 0x0012AD60
		public void DebugDraw()
		{
			if (this.data.DebugDraw)
			{
				GPDebugDraw.Draw(this.stiffnessJointsKernel.StiffnessJointsBuffer, this.nearbyJointsKernel.StiffnessJointsBuffer, this.Particles, false, true, true);
			}
		}

		// Token: 0x04002F65 RID: 12133
		private readonly ClothDataFacade data;

		// Token: 0x04002F66 RID: 12134
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private GpuValue<float> <Weight>k__BackingField;

		// Token: 0x04002F67 RID: 12135
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private GpuValue<float> <Step>k__BackingField;

		// Token: 0x04002F68 RID: 12136
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private GpuValue<float> <DT>k__BackingField;

		// Token: 0x04002F69 RID: 12137
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private GpuValue<float> <DTRecip>k__BackingField;

		// Token: 0x04002F6A RID: 12138
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private GpuValue<float> <T>k__BackingField;

		// Token: 0x04002F6B RID: 12139
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private GpuValue<Vector3> <AccelDT2>k__BackingField;

		// Token: 0x04002F6C RID: 12140
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private GpuValue<float> <InvDrag>k__BackingField;

		// Token: 0x04002F6D RID: 12141
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private GpuValue<float> <Stiffness>k__BackingField;

		// Token: 0x04002F6E RID: 12142
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private GpuValue<float> <DistanceScale>k__BackingField;

		// Token: 0x04002F6F RID: 12143
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private GpuValue<float> <CompressionResistance>k__BackingField;

		// Token: 0x04002F70 RID: 12144
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private GpuValue<float> <BreakThreshold>k__BackingField;

		// Token: 0x04002F71 RID: 12145
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private GpuValue<float> <JointStrength>k__BackingField;

		// Token: 0x04002F72 RID: 12146
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private GpuValue<float> <JointPrediction>k__BackingField;

		// Token: 0x04002F73 RID: 12147
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private GpuValue<float> <Friction>k__BackingField;

		// Token: 0x04002F74 RID: 12148
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private GpuValue<float> <StaticFriction>k__BackingField;

		// Token: 0x04002F75 RID: 12149
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private GpuValue<float> <CollisionPower>k__BackingField;

		// Token: 0x04002F76 RID: 12150
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private GpuBuffer<GPParticle> <Particles>k__BackingField;

		// Token: 0x04002F77 RID: 12151
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private GpuBuffer<Matrix4x4> <Transforms>k__BackingField;

		// Token: 0x04002F78 RID: 12152
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private GpuBuffer<Matrix4x4> <OldTransforms>k__BackingField;

		// Token: 0x04002F79 RID: 12153
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private GpuBuffer<Vector3> <PreCalculatedPositions>k__BackingField;

		// Token: 0x04002F7A RID: 12154
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private GpuBuffer<Vector3> <OldPreCalculatedPositions>k__BackingField;

		// Token: 0x04002F7B RID: 12155
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private GpuBuffer<GPPointJoint> <PointJoints>k__BackingField;

		// Token: 0x04002F7C RID: 12156
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private GpuBuffer<GPPointJoint> <AllPointJoints>k__BackingField;

		// Token: 0x04002F7D RID: 12157
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private GpuBuffer<GPGrabSphere> <GrabSpheres>k__BackingField;

		// Token: 0x04002F7E RID: 12158
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private GpuBuffer<GPSphereWithDelta> <ProcessedSpheres>k__BackingField;

		// Token: 0x04002F7F RID: 12159
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private GpuBuffer<GPLineSphereWithDelta> <ProcessedLineSpheres>k__BackingField;

		// Token: 0x04002F80 RID: 12160
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private GpuBuffer<Vector4> <Planes>k__BackingField;

		// Token: 0x04002F81 RID: 12161
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private GpuBuffer<GPParticle> <OutParticles>k__BackingField;

		// Token: 0x04002F82 RID: 12162
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private GpuBuffer<float> <OutParticlesMap>k__BackingField;

		// Token: 0x04002F83 RID: 12163
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private GpuBuffer<ClothVertex> <ClothVertices>k__BackingField;

		// Token: 0x04002F84 RID: 12164
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private GpuBuffer<Vector3> <ClothOnlyVertices>k__BackingField;

		// Token: 0x04002F85 RID: 12165
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private GpuBuffer<int> <MeshToPhysicsVerticesMap>k__BackingField;

		// Token: 0x04002F86 RID: 12166
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private GpuBuffer<int> <MeshVertexToNeiborsMap>k__BackingField;

		// Token: 0x04002F87 RID: 12167
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private GpuBuffer<int> <MeshVertexToNeiborsMapCounts>k__BackingField;

		// Token: 0x04002F88 RID: 12168
		private GPUMatrixCopyPaster matrixCopyPaster;

		// Token: 0x04002F89 RID: 12169
		private GPUVector3CopyPaster vector3CopyPaster;

		// Token: 0x04002F8A RID: 12170
		private ResetToPointJointsKernel resetKernel;

		// Token: 0x04002F8B RID: 12171
		private ResetToPointJointsPreCalculatedKernel resetToPointJointsPreCalculatedKernel;

		// Token: 0x04002F8C RID: 12172
		private IntegrateVelocityKernel integrateVelocityKernel;

		// Token: 0x04002F8D RID: 12173
		private IntegrateVelocityInnerKernel integrateVelocityInnerKernel;

		// Token: 0x04002F8E RID: 12174
		private IntegrateIterKernel integrateIterKernel;

		// Token: 0x04002F8F RID: 12175
		private StiffnessJointsKernel stiffnessJointsKernel;

		// Token: 0x04002F90 RID: 12176
		private StiffnessJointsKernel nearbyJointsKernel;

		// Token: 0x04002F91 RID: 12177
		private ParticleCollisionResetKernel particleCollisionResetKernel;

		// Token: 0x04002F92 RID: 12178
		private ParticleNeiborsCollisionKernel particleNeiborsCollisionKernel;

		// Token: 0x04002F93 RID: 12179
		private ParticlePlaneCollisionKernel particlePlaneCollisionKernel;

		// Token: 0x04002F94 RID: 12180
		private ParticleSphereCollisionKernel sphereCollisionKernel;

		// Token: 0x04002F95 RID: 12181
		private ParticleLineSphereCollisionKernel lineSphereCollisionKernel;

		// Token: 0x04002F96 RID: 12182
		private PointJointsPreCalculatedKernel pointJointsPreCalculatedKernel;

		// Token: 0x04002F97 RID: 12183
		private PointJointsKernel pointJointsKernel;

		// Token: 0x04002F98 RID: 12184
		private GrabJointsKernel grabJointsKernel;

		// Token: 0x04002F99 RID: 12185
		private PointJointsPreCalculatedFinalKernel pointJointsPreCalculatedFinalKernel;

		// Token: 0x04002F9A RID: 12186
		private PointJointsFinalKernel pointJointsFinalKernel;

		// Token: 0x04002F9B RID: 12187
		private CopySpecificParticlesKernel copySpecificParticlesKernel;

		// Token: 0x04002F9C RID: 12188
		private CreateVertexDataKernel createVertexDataKernel;

		// Token: 0x04002F9D RID: 12189
		private CreateVertexOnlyDataKernel createVertexOnlyDataKernel;

		// Token: 0x04002F9E RID: 12190
		private int frame;

		// Token: 0x04002F9F RID: 12191
		private int outerIterations;

		// Token: 0x04002FA0 RID: 12192
		private int iterations;

		// Token: 0x04002FA1 RID: 12193
		private bool usesPreCalcVerts;

		// Token: 0x04002FA2 RID: 12194
		protected int fixedDispatchCount;
	}
}
