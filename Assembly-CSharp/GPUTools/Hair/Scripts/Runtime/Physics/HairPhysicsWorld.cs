using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using GPUTools.Common.Scripts.PL.Abstract;
using GPUTools.Common.Scripts.PL.Attributes;
using GPUTools.Common.Scripts.PL.Tools;
using GPUTools.Hair.Scripts.Runtime.Data;
using GPUTools.Hair.Scripts.Runtime.Kernels;
using GPUTools.Hair.Scripts.Runtime.Render;
using GPUTools.Physics.Scripts.DebugDraw;
using GPUTools.Physics.Scripts.Kernels;
using GPUTools.Physics.Scripts.Types.Dynamic;
using GPUTools.Physics.Scripts.Types.Joints;
using GPUTools.Physics.Scripts.Types.Shapes;
using UnityEngine;

namespace GPUTools.Hair.Scripts.Runtime.Physics
{
	// Token: 0x02000A1F RID: 2591
	public class HairPhysicsWorld : PrimitiveBase
	{
		// Token: 0x06004291 RID: 17041 RVA: 0x00138DA8 File Offset: 0x001371A8
		public HairPhysicsWorld(HairDataFacade data)
		{
			this.data = data;
			this.T = new GpuValue<float>(0f);
			this.DT = new GpuValue<float>(0f);
			this.DTRecip = new GpuValue<float>(0f);
			this.Weight = new GpuValue<float>(0f);
			this.Step = new GpuValue<float>(0f);
			this.AccelDT2 = new GpuValue<Vector3>(default(Vector3));
			this.InvDrag = new GpuValue<float>(0f);
			this.DistanceScale = new GpuValue<float>(0f);
			this.CompressionDistanceScale = new GpuValue<float>(0f);
			this.NearbyDistanceScale = new GpuValue<float>(0f);
			this.Friction = new GpuValue<float>(0f);
			this.StaticFriction = new GpuValue<float>(0f);
			this.CollisionPower = new GpuValue<float>(0f);
			this.CompressionJointPower = new GpuValue<float>(0f);
			this.NearbyJointPower = new GpuValue<float>(0f);
			this.NearbyJointPowerRolloff = new GpuValue<float>(0f);
			this.SplineJointPower = new GpuValue<float>(0f);
			this.ReverseSplineJointPower = new GpuValue<float>(0f);
			this.DistanceJointPower = new GpuValue<float>(0f);
			this.Segments = new GpuValue<int>(0);
			this.TessSegments = new GpuValue<int>(0);
			this.TessRenderParticlesCount = new GpuValue<int>(0);
			this.WavinessAxis = new GpuValue<Vector3>(default(Vector3));
			this.WavinessFrequencyRandomness = new GpuValue<float>(0f);
			this.WavinessScaleRandomness = new GpuValue<float>(0f);
			this.WavinessAllowReverse = new GpuValue<bool>(false);
			this.WavinessAllowFlipAxis = new GpuValue<bool>(false);
			this.WavinessNormalAdjust = new GpuValue<float>(0f);
			this.IsFixed = new GpuValue<int>(0);
			this.FixedRigidity = new GpuValue<float>(0f);
			this.LightCenter = new GpuValue<Vector3>(default(Vector3));
			this.NormalRandomize = new GpuValue<float>(0f);
			this.InitData();
			this.InitBuffers();
			this.InitPasses();
			base.Bind();
		}

		// Token: 0x17000850 RID: 2128
		// (get) Token: 0x06004293 RID: 17043 RVA: 0x00138FE2 File Offset: 0x001373E2
		// (set) Token: 0x06004292 RID: 17042 RVA: 0x00138FD9 File Offset: 0x001373D9
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

		// Token: 0x17000851 RID: 2129
		// (get) Token: 0x06004295 RID: 17045 RVA: 0x00138FF3 File Offset: 0x001373F3
		// (set) Token: 0x06004294 RID: 17044 RVA: 0x00138FEA File Offset: 0x001373EA
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

		// Token: 0x17000852 RID: 2130
		// (get) Token: 0x06004297 RID: 17047 RVA: 0x00139004 File Offset: 0x00137404
		// (set) Token: 0x06004296 RID: 17046 RVA: 0x00138FFB File Offset: 0x001373FB
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

		// Token: 0x17000853 RID: 2131
		// (get) Token: 0x06004299 RID: 17049 RVA: 0x00139015 File Offset: 0x00137415
		// (set) Token: 0x06004298 RID: 17048 RVA: 0x0013900C File Offset: 0x0013740C
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

		// Token: 0x17000854 RID: 2132
		// (get) Token: 0x0600429B RID: 17051 RVA: 0x00139026 File Offset: 0x00137426
		// (set) Token: 0x0600429A RID: 17050 RVA: 0x0013901D File Offset: 0x0013741D
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

		// Token: 0x17000855 RID: 2133
		// (get) Token: 0x0600429D RID: 17053 RVA: 0x00139037 File Offset: 0x00137437
		// (set) Token: 0x0600429C RID: 17052 RVA: 0x0013902E File Offset: 0x0013742E
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

		// Token: 0x17000856 RID: 2134
		// (get) Token: 0x0600429F RID: 17055 RVA: 0x00139048 File Offset: 0x00137448
		// (set) Token: 0x0600429E RID: 17054 RVA: 0x0013903F File Offset: 0x0013743F
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

		// Token: 0x17000857 RID: 2135
		// (get) Token: 0x060042A1 RID: 17057 RVA: 0x00139059 File Offset: 0x00137459
		// (set) Token: 0x060042A0 RID: 17056 RVA: 0x00139050 File Offset: 0x00137450
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

		// Token: 0x17000858 RID: 2136
		// (get) Token: 0x060042A3 RID: 17059 RVA: 0x0013906A File Offset: 0x0013746A
		// (set) Token: 0x060042A2 RID: 17058 RVA: 0x00139061 File Offset: 0x00137461
		[GpuData("compressionDistanceScale")]
		public GpuValue<float> CompressionDistanceScale
		{
			[CompilerGenerated]
			get
			{
				return this.<CompressionDistanceScale>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<CompressionDistanceScale>k__BackingField = value;
			}
		}

		// Token: 0x17000859 RID: 2137
		// (get) Token: 0x060042A5 RID: 17061 RVA: 0x0013907B File Offset: 0x0013747B
		// (set) Token: 0x060042A4 RID: 17060 RVA: 0x00139072 File Offset: 0x00137472
		[GpuData("nearbyDistanceScale")]
		public GpuValue<float> NearbyDistanceScale
		{
			[CompilerGenerated]
			get
			{
				return this.<NearbyDistanceScale>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<NearbyDistanceScale>k__BackingField = value;
			}
		}

		// Token: 0x1700085A RID: 2138
		// (get) Token: 0x060042A7 RID: 17063 RVA: 0x0013908C File Offset: 0x0013748C
		// (set) Token: 0x060042A6 RID: 17062 RVA: 0x00139083 File Offset: 0x00137483
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

		// Token: 0x1700085B RID: 2139
		// (get) Token: 0x060042A9 RID: 17065 RVA: 0x0013909D File Offset: 0x0013749D
		// (set) Token: 0x060042A8 RID: 17064 RVA: 0x00139094 File Offset: 0x00137494
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

		// Token: 0x1700085C RID: 2140
		// (get) Token: 0x060042AB RID: 17067 RVA: 0x001390AE File Offset: 0x001374AE
		// (set) Token: 0x060042AA RID: 17066 RVA: 0x001390A5 File Offset: 0x001374A5
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

		// Token: 0x1700085D RID: 2141
		// (get) Token: 0x060042AD RID: 17069 RVA: 0x001390BF File Offset: 0x001374BF
		// (set) Token: 0x060042AC RID: 17068 RVA: 0x001390B6 File Offset: 0x001374B6
		[GpuData("compressionJointPower")]
		public GpuValue<float> CompressionJointPower
		{
			[CompilerGenerated]
			get
			{
				return this.<CompressionJointPower>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<CompressionJointPower>k__BackingField = value;
			}
		}

		// Token: 0x1700085E RID: 2142
		// (get) Token: 0x060042AF RID: 17071 RVA: 0x001390D0 File Offset: 0x001374D0
		// (set) Token: 0x060042AE RID: 17070 RVA: 0x001390C7 File Offset: 0x001374C7
		[GpuData("nearbyJointPower")]
		public GpuValue<float> NearbyJointPower
		{
			[CompilerGenerated]
			get
			{
				return this.<NearbyJointPower>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<NearbyJointPower>k__BackingField = value;
			}
		}

		// Token: 0x1700085F RID: 2143
		// (get) Token: 0x060042B1 RID: 17073 RVA: 0x001390E1 File Offset: 0x001374E1
		// (set) Token: 0x060042B0 RID: 17072 RVA: 0x001390D8 File Offset: 0x001374D8
		[GpuData("nearbyJointPowerRolloff")]
		public GpuValue<float> NearbyJointPowerRolloff
		{
			[CompilerGenerated]
			get
			{
				return this.<NearbyJointPowerRolloff>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<NearbyJointPowerRolloff>k__BackingField = value;
			}
		}

		// Token: 0x17000860 RID: 2144
		// (get) Token: 0x060042B3 RID: 17075 RVA: 0x001390F2 File Offset: 0x001374F2
		// (set) Token: 0x060042B2 RID: 17074 RVA: 0x001390E9 File Offset: 0x001374E9
		[GpuData("splineJointPower")]
		public GpuValue<float> SplineJointPower
		{
			[CompilerGenerated]
			get
			{
				return this.<SplineJointPower>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<SplineJointPower>k__BackingField = value;
			}
		}

		// Token: 0x17000861 RID: 2145
		// (get) Token: 0x060042B5 RID: 17077 RVA: 0x00139103 File Offset: 0x00137503
		// (set) Token: 0x060042B4 RID: 17076 RVA: 0x001390FA File Offset: 0x001374FA
		[GpuData("reverseSplineJointPower")]
		public GpuValue<float> ReverseSplineJointPower
		{
			[CompilerGenerated]
			get
			{
				return this.<ReverseSplineJointPower>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<ReverseSplineJointPower>k__BackingField = value;
			}
		}

		// Token: 0x17000862 RID: 2146
		// (get) Token: 0x060042B7 RID: 17079 RVA: 0x00139114 File Offset: 0x00137514
		// (set) Token: 0x060042B6 RID: 17078 RVA: 0x0013910B File Offset: 0x0013750B
		[GpuData("distanceJointPower")]
		public GpuValue<float> DistanceJointPower
		{
			[CompilerGenerated]
			get
			{
				return this.<DistanceJointPower>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<DistanceJointPower>k__BackingField = value;
			}
		}

		// Token: 0x17000863 RID: 2147
		// (get) Token: 0x060042B9 RID: 17081 RVA: 0x00139125 File Offset: 0x00137525
		// (set) Token: 0x060042B8 RID: 17080 RVA: 0x0013911C File Offset: 0x0013751C
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

		// Token: 0x17000864 RID: 2148
		// (get) Token: 0x060042BB RID: 17083 RVA: 0x00139136 File Offset: 0x00137536
		// (set) Token: 0x060042BA RID: 17082 RVA: 0x0013912D File Offset: 0x0013752D
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

		// Token: 0x17000865 RID: 2149
		// (get) Token: 0x060042BD RID: 17085 RVA: 0x00139147 File Offset: 0x00137547
		// (set) Token: 0x060042BC RID: 17084 RVA: 0x0013913E File Offset: 0x0013753E
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

		// Token: 0x17000866 RID: 2150
		// (get) Token: 0x060042BF RID: 17087 RVA: 0x00139158 File Offset: 0x00137558
		// (set) Token: 0x060042BE RID: 17086 RVA: 0x0013914F File Offset: 0x0013754F
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

		// Token: 0x17000867 RID: 2151
		// (get) Token: 0x060042C1 RID: 17089 RVA: 0x00139169 File Offset: 0x00137569
		// (set) Token: 0x060042C0 RID: 17088 RVA: 0x00139160 File Offset: 0x00137560
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

		// Token: 0x17000868 RID: 2152
		// (get) Token: 0x060042C3 RID: 17091 RVA: 0x0013917A File Offset: 0x0013757A
		// (set) Token: 0x060042C2 RID: 17090 RVA: 0x00139171 File Offset: 0x00137571
		[GpuData("pointToPreviousPointDistances")]
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

		// Token: 0x17000869 RID: 2153
		// (get) Token: 0x060042C5 RID: 17093 RVA: 0x0013918B File Offset: 0x0013758B
		// (set) Token: 0x060042C4 RID: 17092 RVA: 0x00139182 File Offset: 0x00137582
		[GpuData("isFixed")]
		public GpuValue<int> IsFixed
		{
			[CompilerGenerated]
			get
			{
				return this.<IsFixed>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<IsFixed>k__BackingField = value;
			}
		}

		// Token: 0x1700086A RID: 2154
		// (get) Token: 0x060042C7 RID: 17095 RVA: 0x0013919C File Offset: 0x0013759C
		// (set) Token: 0x060042C6 RID: 17094 RVA: 0x00139193 File Offset: 0x00137593
		[GpuData("fixedRigidity")]
		public GpuValue<float> FixedRigidity
		{
			[CompilerGenerated]
			get
			{
				return this.<FixedRigidity>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<FixedRigidity>k__BackingField = value;
			}
		}

		// Token: 0x1700086B RID: 2155
		// (get) Token: 0x060042C9 RID: 17097 RVA: 0x001391AD File Offset: 0x001375AD
		// (set) Token: 0x060042C8 RID: 17096 RVA: 0x001391A4 File Offset: 0x001375A4
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

		// Token: 0x1700086C RID: 2156
		// (get) Token: 0x060042CB RID: 17099 RVA: 0x001391BE File Offset: 0x001375BE
		// (set) Token: 0x060042CA RID: 17098 RVA: 0x001391B5 File Offset: 0x001375B5
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

		// Token: 0x1700086D RID: 2157
		// (get) Token: 0x060042CD RID: 17101 RVA: 0x001391CF File Offset: 0x001375CF
		// (set) Token: 0x060042CC RID: 17100 RVA: 0x001391C6 File Offset: 0x001375C6
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

		// Token: 0x1700086E RID: 2158
		// (get) Token: 0x060042CF RID: 17103 RVA: 0x001391E0 File Offset: 0x001375E0
		// (set) Token: 0x060042CE RID: 17102 RVA: 0x001391D7 File Offset: 0x001375D7
		[GpuData("cutLineSpheres")]
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

		// Token: 0x1700086F RID: 2159
		// (get) Token: 0x060042D1 RID: 17105 RVA: 0x001391F1 File Offset: 0x001375F1
		// (set) Token: 0x060042D0 RID: 17104 RVA: 0x001391E8 File Offset: 0x001375E8
		[GpuData("growLineSpheres")]
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

		// Token: 0x17000870 RID: 2160
		// (get) Token: 0x060042D3 RID: 17107 RVA: 0x00139202 File Offset: 0x00137602
		// (set) Token: 0x060042D2 RID: 17106 RVA: 0x001391F9 File Offset: 0x001375F9
		[GpuData("holdLineSpheres")]
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

		// Token: 0x17000871 RID: 2161
		// (get) Token: 0x060042D5 RID: 17109 RVA: 0x00139213 File Offset: 0x00137613
		// (set) Token: 0x060042D4 RID: 17108 RVA: 0x0013920A File Offset: 0x0013760A
		[GpuData("grabLineSpheres")]
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

		// Token: 0x17000872 RID: 2162
		// (get) Token: 0x060042D7 RID: 17111 RVA: 0x00139224 File Offset: 0x00137624
		// (set) Token: 0x060042D6 RID: 17110 RVA: 0x0013921B File Offset: 0x0013761B
		[GpuData("pushLineSpheres")]
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

		// Token: 0x17000873 RID: 2163
		// (get) Token: 0x060042D9 RID: 17113 RVA: 0x00139235 File Offset: 0x00137635
		// (set) Token: 0x060042D8 RID: 17112 RVA: 0x0013922C File Offset: 0x0013762C
		[GpuData("pullLineSpheres")]
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

		// Token: 0x17000874 RID: 2164
		// (get) Token: 0x060042DB RID: 17115 RVA: 0x00139246 File Offset: 0x00137646
		// (set) Token: 0x060042DA RID: 17114 RVA: 0x0013923D File Offset: 0x0013763D
		[GpuData("brushLineSpheres")]
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

		// Token: 0x17000875 RID: 2165
		// (get) Token: 0x060042DD RID: 17117 RVA: 0x00139257 File Offset: 0x00137657
		// (set) Token: 0x060042DC RID: 17116 RVA: 0x0013924E File Offset: 0x0013764E
		[GpuData("rigidityIncreaseLineSpheres")]
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

		// Token: 0x17000876 RID: 2166
		// (get) Token: 0x060042DF RID: 17119 RVA: 0x00139268 File Offset: 0x00137668
		// (set) Token: 0x060042DE RID: 17118 RVA: 0x0013925F File Offset: 0x0013765F
		[GpuData("rigidityDecreaseLineSpheres")]
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

		// Token: 0x17000877 RID: 2167
		// (get) Token: 0x060042E1 RID: 17121 RVA: 0x00139279 File Offset: 0x00137679
		// (set) Token: 0x060042E0 RID: 17120 RVA: 0x00139270 File Offset: 0x00137670
		[GpuData("rigiditySetLineSpheres")]
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

		// Token: 0x17000878 RID: 2168
		// (get) Token: 0x060042E3 RID: 17123 RVA: 0x0013928A File Offset: 0x0013768A
		// (set) Token: 0x060042E2 RID: 17122 RVA: 0x00139281 File Offset: 0x00137681
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

		// Token: 0x17000879 RID: 2169
		// (get) Token: 0x060042E5 RID: 17125 RVA: 0x0013929B File Offset: 0x0013769B
		// (set) Token: 0x060042E4 RID: 17124 RVA: 0x00139292 File Offset: 0x00137692
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

		// Token: 0x1700087A RID: 2170
		// (get) Token: 0x060042E7 RID: 17127 RVA: 0x001392AC File Offset: 0x001376AC
		// (set) Token: 0x060042E6 RID: 17126 RVA: 0x001392A3 File Offset: 0x001376A3
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

		// Token: 0x1700087B RID: 2171
		// (get) Token: 0x060042E9 RID: 17129 RVA: 0x001392BD File Offset: 0x001376BD
		// (set) Token: 0x060042E8 RID: 17128 RVA: 0x001392B4 File Offset: 0x001376B4
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

		// Token: 0x1700087C RID: 2172
		// (get) Token: 0x060042EB RID: 17131 RVA: 0x001392CE File Offset: 0x001376CE
		// (set) Token: 0x060042EA RID: 17130 RVA: 0x001392C5 File Offset: 0x001376C5
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

		// Token: 0x1700087D RID: 2173
		// (get) Token: 0x060042ED RID: 17133 RVA: 0x001392DF File Offset: 0x001376DF
		// (set) Token: 0x060042EC RID: 17132 RVA: 0x001392D6 File Offset: 0x001376D6
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

		// Token: 0x1700087E RID: 2174
		// (get) Token: 0x060042EF RID: 17135 RVA: 0x001392F0 File Offset: 0x001376F0
		// (set) Token: 0x060042EE RID: 17134 RVA: 0x001392E7 File Offset: 0x001376E7
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

		// Token: 0x1700087F RID: 2175
		// (get) Token: 0x060042F1 RID: 17137 RVA: 0x00139301 File Offset: 0x00137701
		// (set) Token: 0x060042F0 RID: 17136 RVA: 0x001392F8 File Offset: 0x001376F8
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

		// Token: 0x17000880 RID: 2176
		// (get) Token: 0x060042F3 RID: 17139 RVA: 0x00139312 File Offset: 0x00137712
		// (set) Token: 0x060042F2 RID: 17138 RVA: 0x00139309 File Offset: 0x00137709
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

		// Token: 0x17000881 RID: 2177
		// (get) Token: 0x060042F5 RID: 17141 RVA: 0x00139323 File Offset: 0x00137723
		// (set) Token: 0x060042F4 RID: 17140 RVA: 0x0013931A File Offset: 0x0013771A
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

		// Token: 0x17000882 RID: 2178
		// (get) Token: 0x060042F7 RID: 17143 RVA: 0x00139334 File Offset: 0x00137734
		// (set) Token: 0x060042F6 RID: 17142 RVA: 0x0013932B File Offset: 0x0013772B
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

		// Token: 0x17000883 RID: 2179
		// (get) Token: 0x060042F9 RID: 17145 RVA: 0x00139345 File Offset: 0x00137745
		// (set) Token: 0x060042F8 RID: 17144 RVA: 0x0013933C File Offset: 0x0013773C
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

		// Token: 0x17000884 RID: 2180
		// (get) Token: 0x060042FB RID: 17147 RVA: 0x00139356 File Offset: 0x00137756
		// (set) Token: 0x060042FA RID: 17146 RVA: 0x0013934D File Offset: 0x0013774D
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

		// Token: 0x17000885 RID: 2181
		// (get) Token: 0x060042FD RID: 17149 RVA: 0x00139367 File Offset: 0x00137767
		// (set) Token: 0x060042FC RID: 17148 RVA: 0x0013935E File Offset: 0x0013775E
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

		// Token: 0x17000886 RID: 2182
		// (get) Token: 0x060042FF RID: 17151 RVA: 0x00139378 File Offset: 0x00137778
		// (set) Token: 0x060042FE RID: 17150 RVA: 0x0013936F File Offset: 0x0013776F
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

		// Token: 0x17000887 RID: 2183
		// (get) Token: 0x06004301 RID: 17153 RVA: 0x00139389 File Offset: 0x00137789
		// (set) Token: 0x06004300 RID: 17152 RVA: 0x00139380 File Offset: 0x00137780
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

		// Token: 0x06004302 RID: 17154 RVA: 0x00139394 File Offset: 0x00137794
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
			if (this.data.StyleMode)
			{
				this.InvDrag.Value = 0f;
			}
			else
			{
				this.InvDrag.Value = this.data.InvDrag;
			}
			this.DistanceScale.Value = 1f;
			this.CompressionDistanceScale.Value = 1f;
			this.NearbyDistanceScale.Value = this.data.WorldScale;
			this.Friction.Value = this.data.Friction;
			this.StaticFriction.Value = this.data.Friction * 2f;
			this.CollisionPower.Value = this.data.CollisionPower;
			this.CompressionJointPower.Value = this.data.CompressionJointPower / (float)this.iterations;
			this.NearbyJointPower.Value = this.data.NearbyJointPower * 0.5f / (float)this.iterations;
			this.NearbyJointPowerRolloff.Value = this.data.NearbyJointPowerRolloff;
			this.ReverseSplineJointPower.Value = this.data.ReverseSplineJointPower;
			this.FixedRigidity.Value = 0.1f;
			if (this.data.StyleMode)
			{
				this.SplineJointPower.Value = 1f;
			}
			else
			{
				this.SplineJointPower.Value = Mathf.Min(this.data.SplineJointPower * 2f / (float)this.iterations, 1f);
			}
			this.DistanceJointPower.Value = 0.5f;
			this.Segments.Value = (int)this.data.Size.y;
			this.TessSegments.Value = (int)this.data.TessFactor.y;
			if (this.data.Particles != null)
			{
				this.TessRenderParticlesCount.Value = (int)this.data.TessFactor.y * this.data.Particles.Count;
			}
			else
			{
				this.TessRenderParticlesCount.Value = 0;
			}
			if (this.data.StyleMode && !this.data.StyleModeShowCurls)
			{
				this.WavinessAxis.Value = Vector3.zero;
				this.WavinessNormalAdjust.Value = 0f;
			}
			else
			{
				this.WavinessAxis.Value = this.data.WavinessAxis;
				this.WavinessNormalAdjust.Value = this.data.WavinessNormalAdjust * this.data.WorldScale;
			}
			this.WavinessFrequencyRandomness.Value = this.data.WavinessFrequencyRandomness;
			this.WavinessScaleRandomness.Value = this.data.WavinessScaleRandomness;
			this.WavinessAllowReverse.Value = this.data.WavinessAllowReverse;
			this.WavinessAllowFlipAxis.Value = this.data.WavinessAllowFlipAxis;
			this.IsFixed.Value = ((!this.isPhysics) ? 1 : 0);
			this.LightCenter.Value = this.data.LightCenter;
			this.NormalRandomize.Value = this.data.NormalRandomize;
		}

		// Token: 0x06004303 RID: 17155 RVA: 0x00139744 File Offset: 0x00137B44
		private void InitBuffers()
		{
			this.Particles = this.data.Particles;
			this.Normals = this.data.NormalsBuffer;
			this.Transforms = this.data.MatricesBuffer;
			if (this.Transforms != null)
			{
				this.OldTransforms = new GpuBuffer<Matrix4x4>(this.Transforms.Count, 64);
			}
			this.PointJoints = this.data.PointJoints;
			this.PointToPreviousPointDistances = this.data.PointToPreviousPointDistances;
			this.ProcessedSpheres = this.data.ProcessedSpheres;
			this.ProcessedLineSpheres = this.data.ProcessedLineSpheres;
			this.CutLineSpheres = this.data.CutLineSpheres;
			this.GrowLineSpheres = this.data.GrowLineSpheres;
			this.HoldLineSpheres = this.data.HoldLineSpheres;
			this.GrabLineSpheres = this.data.GrabLineSpheres;
			this.PushLineSpheres = this.data.PushLineSpheres;
			this.PullLineSpheres = this.data.PullLineSpheres;
			this.BrushLineSpheres = this.data.BrushLineSpheres;
			this.RigidityIncreaseLineSpheres = this.data.RigidityIncreaseLineSpheres;
			this.RigidityDecreaseLineSpheres = this.data.RigidityDecreaseLineSpheres;
			this.RigiditySetLineSpheres = this.data.RigiditySetLineSpheres;
			this.RenderParticles = this.data.RenderParticles;
			this.TessRenderParticles = this.data.TessRenderParticles;
			this.RandomsPerStrand = this.data.RandomsPerStrand;
			this.OutParticles = this.data.OutParticles;
			this.OutParticlesMap = this.data.OutParticlesMap;
		}

		// Token: 0x06004304 RID: 17156 RVA: 0x001398EC File Offset: 0x00137CEC
		private void UpdateBuffers()
		{
			if (this.Transforms != this.data.MatricesBuffer)
			{
				this.Transforms = this.data.MatricesBuffer;
				if (this.resetKernel != null)
				{
					this.resetKernel.Transforms = this.Transforms;
					this.resetKernel.ClearCacheAttributes();
				}
				if (this.pointJointsKernel != null)
				{
					this.pointJointsKernel.Transforms = this.Transforms;
					this.pointJointsKernel.ClearCacheAttributes();
				}
				if (this.pointJointsFixedRigidityKernel != null)
				{
					this.pointJointsFixedRigidityKernel.Transforms = this.Transforms;
					this.pointJointsFixedRigidityKernel.ClearCacheAttributes();
				}
				if (this.pointJointsFinalKernel != null)
				{
					this.pointJointsFinalKernel.Transforms = this.Transforms;
					this.pointJointsFinalKernel.ClearCacheAttributes();
				}
				if (this.movePointJointsToParticlesKernel != null)
				{
					this.movePointJointsToParticlesKernel.Transforms = this.Transforms;
					this.movePointJointsToParticlesKernel.ClearCacheAttributes();
				}
				if (this.tesselateKernel != null)
				{
					this.tesselateKernel.Transforms = this.Transforms;
					this.tesselateKernel.ClearCacheAttributes();
				}
				else if (this.tesselateWithNormalsKernel != null)
				{
					this.tesselateWithNormalsKernel.Transforms = this.Transforms;
					this.tesselateWithNormalsKernel.ClearCacheAttributes();
					if (this.tesselateWithNormalsRenderRigidityKernel != null)
					{
						this.tesselateWithNormalsRenderRigidityKernel.Transforms = this.Transforms;
						this.tesselateWithNormalsRenderRigidityKernel.ClearCacheAttributes();
					}
				}
			}
			if (this.Normals != this.data.NormalsBuffer)
			{
				this.Normals = this.data.NormalsBuffer;
				if (this.tesselateWithNormalsKernel != null)
				{
					this.tesselateWithNormalsKernel.Normals = this.Normals;
					this.tesselateWithNormalsKernel.ClearCacheAttributes();
					if (this.tesselateWithNormalsRenderRigidityKernel != null)
					{
						this.tesselateWithNormalsRenderRigidityKernel.Normals = this.Normals;
						this.tesselateWithNormalsRenderRigidityKernel.ClearCacheAttributes();
					}
				}
			}
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
				this.planeCollisionKernel.Planes = this.Planes;
				this.planeCollisionKernel.ClearCacheAttributes();
			}
			if (this.GrowLineSpheres != this.data.GrowLineSpheres)
			{
				this.GrowLineSpheres = this.data.GrowLineSpheres;
				this.lineSphereGrowKernel.GrowLineSpheres = this.GrowLineSpheres;
				this.lineSphereGrowKernel.ClearCacheAttributes();
			}
			if (this.CutLineSpheres != this.data.CutLineSpheres)
			{
				this.CutLineSpheres = this.data.CutLineSpheres;
				this.lineSphereCutKernel.CutLineSpheres = this.CutLineSpheres;
				this.lineSphereCutKernel.ClearCacheAttributes();
			}
			if (this.PushLineSpheres != this.data.PushLineSpheres)
			{
				this.PushLineSpheres = this.data.PushLineSpheres;
				this.lineSpherePushKernel.PushLineSpheres = this.PushLineSpheres;
				this.lineSpherePushKernel.ClearCacheAttributes();
			}
			if (this.PullLineSpheres != this.data.PullLineSpheres)
			{
				this.PullLineSpheres = this.data.PullLineSpheres;
				this.lineSpherePullKernel.PullLineSpheres = this.PullLineSpheres;
				this.lineSpherePullKernel.ClearCacheAttributes();
			}
			if (this.BrushLineSpheres != this.data.BrushLineSpheres)
			{
				this.BrushLineSpheres = this.data.BrushLineSpheres;
				this.lineSphereBrushKernel.BrushLineSpheres = this.BrushLineSpheres;
				this.lineSphereBrushKernel.ClearCacheAttributes();
			}
			if (this.HoldLineSpheres != this.data.HoldLineSpheres)
			{
				this.HoldLineSpheres = this.data.HoldLineSpheres;
				this.lineSphereHoldKernel.HoldLineSpheres = this.HoldLineSpheres;
				this.lineSphereHoldKernel.ClearCacheAttributes();
			}
			if (this.GrabLineSpheres != this.data.GrabLineSpheres)
			{
				this.GrabLineSpheres = this.data.GrabLineSpheres;
				this.lineSphereGrabKernel.GrabLineSpheres = this.GrabLineSpheres;
				this.lineSphereGrabKernel.ClearCacheAttributes();
			}
			if (this.RigidityIncreaseLineSpheres != this.data.RigidityIncreaseLineSpheres)
			{
				this.RigidityIncreaseLineSpheres = this.data.RigidityIncreaseLineSpheres;
				this.lineSphereRigidityIncreaseKernel.RigidityIncreaseLineSpheres = this.RigidityIncreaseLineSpheres;
				this.lineSphereRigidityIncreaseKernel.ClearCacheAttributes();
			}
			if (this.RigidityDecreaseLineSpheres != this.data.RigidityDecreaseLineSpheres)
			{
				this.RigidityDecreaseLineSpheres = this.data.RigidityDecreaseLineSpheres;
				this.lineSphereRigidityDecreaseKernel.RigidityDecreaseLineSpheres = this.RigidityDecreaseLineSpheres;
				this.lineSphereRigidityDecreaseKernel.ClearCacheAttributes();
			}
			if (this.RigiditySetLineSpheres != this.data.RigiditySetLineSpheres)
			{
				this.RigiditySetLineSpheres = this.data.RigiditySetLineSpheres;
				this.lineSphereRigiditySetKernel.RigiditySetLineSpheres = this.RigiditySetLineSpheres;
				this.lineSphereRigiditySetKernel.ClearCacheAttributes();
			}
		}

		// Token: 0x06004305 RID: 17157 RVA: 0x00139E39 File Offset: 0x00138239
		private void AddStaticPass(KernelBase kernel)
		{
			base.AddPass(kernel);
			this.staticQueue.Add(kernel);
		}

		// Token: 0x06004306 RID: 17158 RVA: 0x00139E50 File Offset: 0x00138250
		private void InitPasses()
		{
			base.AddPass(this.integrateVelocityKernel = new IntegrateVelocityKernel());
			base.AddPass(this.integrateVelocityInnerKernel = new IntegrateVelocityInnerKernel());
			base.AddPass(this.integrateIterKernel = new IntegrateIterKernel());
			base.AddPass(this.integrateIterWithParticleHoldKernel = new IntegrateIterWithParticleHoldKernel());
			base.AddPass(this.distanceJointsKernel = new DistanceJointsKernel(this.data.DistanceJoints, this.data.DistanceJointsBuffer));
			base.AddPass(this.compressionJointsKernel = new CompressionJointsKernel(this.data.CompressionJoints, this.data.CompressionJointsBuffer));
			base.AddPass(this.nearbyDistanceJointsKernel = new NearbyDistanceJointsKernel(this.data.NearbyDistanceJoints, this.data.NearbyDistanceJointsBuffer));
			base.AddPass(this.splineJointsKernel = new SplineJointsKernel());
			base.AddPass(this.splineJointsReverseKernel = new SplineJointsReverseKernel());
			base.AddPass(this.splineJointsWithParticleHoldKernel = new SplineJointsWithParticleHoldKernel());
			base.AddPass(this.splineJointsReverseWithParticleHoldKernel = new SplineJointsReverseWithParticleHoldKernel());
			base.AddPass(this.particleCollisionResetKernel = new ParticleCollisionResetKernel());
			base.AddPass(this.planeCollisionKernel = new ParticlePlaneCollisionKernel());
			base.AddPass(this.sphereCollisionKernel = new ParticleSphereCollisionKernel());
			base.AddPass(this.lineSphereCollisionKernel = new ParticleLineSphereCollisionKernel());
			base.AddPass(this.lineSphereHoldResetKernel = new ParticleLineSphereHoldResetKernel());
			base.AddPass(this.lineSphereHoldKernel = new ParticleLineSphereHoldKernel());
			base.AddPass(this.lineSphereGrabKernel = new ParticleLineSphereGrabKernel());
			base.AddPass(this.lineSpherePushKernel = new ParticleLineSpherePushKernel());
			base.AddPass(this.lineSpherePullKernel = new ParticleLineSpherePullKernel());
			base.AddPass(this.lineSphereBrushKernel = new ParticleLineSphereBrushKernel());
			base.AddPass(this.lineSphereGrowKernel = new ParticleLineSphereGrowKernel());
			base.AddPass(this.lineSphereCutKernel = new ParticleLineSphereCutKernel());
			base.AddPass(this.lineSphereRigidityIncreaseKernel = new ParticleLineSphereRigidityIncreaseKernel());
			base.AddPass(this.lineSphereRigidityDecreaseKernel = new ParticleLineSphereRigidityDecreaseKernel());
			base.AddPass(this.lineSphereRigiditySetKernel = new ParticleLineSphereRigiditySetKernel());
			base.AddPass(this.distanceJointsAdjustKernel = new DistanceJointsAdjustKernel(this.data.DistanceJoints, this.data.DistanceJointsBuffer));
			base.AddPass(this.pointJointsKernel = new PointJointsKernel());
			base.AddPass(this.pointJointsFixedRigidityKernel = new PointJointsFixedRigidityKernel());
			base.AddPass(this.movePointJointsToParticlesKernel = new MovePointJointsToParticlesKernel());
			this.AddStaticPass(this.pointJointsFinalKernel = new PointJointsFinalKernel());
			if (this.data.OutParticles != null)
			{
				this.AddStaticPass(this.copySpecificParticlesKernel = new CopySpecificParticlesKernel());
			}
			if (this.data.NormalsBuffer != null)
			{
				this.AddStaticPass(this.tesselateWithNormalsKernel = new TesselateWithNormalsKernel());
				this.AddStaticPass(this.tesselateWithNormalsRenderRigidityKernel = new TesselateWithNormalsRenderRigidityKernel());
			}
			else
			{
				this.AddStaticPass(this.tesselateKernel = new TesselateKernel());
			}
			this.AddStaticPass(this.resetKernel = new ResetToPointJointsKernel());
		}

		// Token: 0x06004307 RID: 17159 RVA: 0x0013A1EC File Offset: 0x001385EC
		public void RebindData()
		{
			this.Particles = this.data.Particles;
			this.Transforms = this.data.MatricesBuffer;
			this.Normals = this.data.NormalsBuffer;
			if (this.OldTransforms != null)
			{
				this.OldTransforms.Dispose();
			}
			if (this.Transforms != null)
			{
				this.OldTransforms = new GpuBuffer<Matrix4x4>(this.Transforms.Count, 64);
			}
			else
			{
				this.OldTransforms = null;
			}
			this.PointJoints = this.data.PointJoints;
			this.PointToPreviousPointDistances = this.data.PointToPreviousPointDistances;
			this.RenderParticles = this.data.RenderParticles;
			this.TessRenderParticles = this.data.TessRenderParticles;
			this.RandomsPerStrand = this.data.RandomsPerStrand;
			base.BindAttributes();
			this.integrateIterKernel.ClearCacheAttributes();
			this.integrateIterWithParticleHoldKernel.ClearCacheAttributes();
			this.integrateVelocityKernel.ClearCacheAttributes();
			this.integrateVelocityInnerKernel.ClearCacheAttributes();
			this.distanceJointsKernel.DistanceJoints = this.data.DistanceJoints;
			this.distanceJointsKernel.DistanceJointsBuffer = this.data.DistanceJointsBuffer;
			this.distanceJointsKernel.ClearCacheAttributes();
			this.compressionJointsKernel.CompressionJoints = this.data.CompressionJoints;
			this.compressionJointsKernel.CompressionJointsBuffer = this.data.CompressionJointsBuffer;
			this.compressionJointsKernel.ClearCacheAttributes();
			this.nearbyDistanceJointsKernel.NearbyDistanceJoints = this.data.NearbyDistanceJoints;
			this.nearbyDistanceJointsKernel.NearbyDistanceJointsBuffer = this.data.NearbyDistanceJointsBuffer;
			this.nearbyDistanceJointsKernel.ClearCacheAttributes();
			this.splineJointsKernel.ClearCacheAttributes();
			this.splineJointsReverseKernel.ClearCacheAttributes();
			this.splineJointsWithParticleHoldKernel.ClearCacheAttributes();
			this.splineJointsReverseWithParticleHoldKernel.ClearCacheAttributes();
			this.particleCollisionResetKernel.ClearCacheAttributes();
			this.planeCollisionKernel.ClearCacheAttributes();
			this.sphereCollisionKernel.ClearCacheAttributes();
			this.lineSphereCollisionKernel.ClearCacheAttributes();
			this.lineSphereHoldResetKernel.ClearCacheAttributes();
			this.lineSphereHoldKernel.ClearCacheAttributes();
			this.lineSphereGrabKernel.ClearCacheAttributes();
			this.lineSpherePushKernel.ClearCacheAttributes();
			this.lineSpherePullKernel.ClearCacheAttributes();
			this.lineSphereBrushKernel.ClearCacheAttributes();
			this.lineSphereGrowKernel.ClearCacheAttributes();
			this.lineSphereCutKernel.ClearCacheAttributes();
			this.lineSphereRigidityIncreaseKernel.ClearCacheAttributes();
			this.lineSphereRigidityDecreaseKernel.ClearCacheAttributes();
			this.lineSphereRigiditySetKernel.ClearCacheAttributes();
			this.distanceJointsAdjustKernel.DistanceJoints = this.data.DistanceJoints;
			this.distanceJointsAdjustKernel.DistanceJointsBuffer = this.data.DistanceJointsBuffer;
			this.distanceJointsAdjustKernel.ClearCacheAttributes();
			this.pointJointsKernel.ClearCacheAttributes();
			this.pointJointsFixedRigidityKernel.ClearCacheAttributes();
			this.pointJointsFinalKernel.ClearCacheAttributes();
			this.movePointJointsToParticlesKernel.ClearCacheAttributes();
			if (this.copySpecificParticlesKernel != null)
			{
				this.copySpecificParticlesKernel.ClearCacheAttributes();
			}
			if (this.tesselateKernel != null)
			{
				this.tesselateKernel.ClearCacheAttributes();
			}
			else if (this.tesselateWithNormalsKernel != null)
			{
				this.tesselateWithNormalsKernel.ClearCacheAttributes();
				if (this.tesselateWithNormalsRenderRigidityKernel != null)
				{
					this.tesselateWithNormalsRenderRigidityKernel.ClearCacheAttributes();
				}
			}
			this.resetKernel.ClearCacheAttributes();
		}

		// Token: 0x06004308 RID: 17160 RVA: 0x0013A539 File Offset: 0x00138939
		public void Reset()
		{
			this.frame = 0;
		}

		// Token: 0x06004309 RID: 17161 RVA: 0x0013A542 File Offset: 0x00138942
		public void PartialReset()
		{
			if (this.frame > 10)
			{
				this.frame = 10;
			}
		}

		// Token: 0x0600430A RID: 17162 RVA: 0x0013A559 File Offset: 0x00138959
		public void FixedDispatch()
		{
			if (!this.isPhysics || this.data.RunPhysicsOnUpdate)
			{
				return;
			}
			this.DispatchPhysicsImpl();
		}

		// Token: 0x0600430B RID: 17163 RVA: 0x0013A57D File Offset: 0x0013897D
		public override void Dispatch()
		{
			this.UpdateIsPhysics();
			if (!this.isPhysics)
			{
				this.DispatchStaticImpl();
			}
			else if (this.data.RunPhysicsOnUpdate)
			{
				this.DispatchPhysicsImpl();
			}
			this.DispatchImpl();
		}

		// Token: 0x0600430C RID: 17164 RVA: 0x0013A5B8 File Offset: 0x001389B8
		private void DispatchPhysicsImpl()
		{
			this.InitData();
			this.UpdateBuffers();
			if (this.frame < 10)
			{
				this.resetKernel.Dispatch();
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
					if (this.data.StyleMode)
					{
						this.lineSphereHoldResetKernel.Dispatch();
						if (this.lineSphereHoldKernel.HoldLineSpheres != null)
						{
							this.lineSphereHoldKernel.Dispatch();
						}
						if (this.lineSphereBrushKernel.BrushLineSpheres != null)
						{
							this.lineSphereBrushKernel.Dispatch();
						}
						this.splineJointsReverseWithParticleHoldKernel.Dispatch();
						this.splineJointsWithParticleHoldKernel.Dispatch();
						if (this.lineSphereBrushKernel.BrushLineSpheres != null)
						{
							this.lineSphereBrushKernel.Dispatch();
						}
						if (this.lineSpherePullKernel.PullLineSpheres != null)
						{
							this.lineSpherePullKernel.Dispatch();
						}
						if (this.lineSpherePushKernel.PushLineSpheres != null)
						{
							this.lineSpherePushKernel.Dispatch();
						}
						if (this.lineSphereGrabKernel.GrabLineSpheres != null)
						{
							this.lineSphereGrabKernel.Dispatch();
						}
						this.splineJointsReverseWithParticleHoldKernel.Dispatch();
						this.splineJointsWithParticleHoldKernel.Dispatch();
						if (this.lineSphereGrowKernel.GrowLineSpheres != null)
						{
							this.lineSphereGrowKernel.Dispatch();
							this.splineJointsReverseKernel.Dispatch();
						}
						for (int j = 0; j < 4; j++)
						{
							if (this.lineSphereCutKernel.CutLineSpheres != null)
							{
								this.lineSphereCutKernel.Dispatch();
								this.splineJointsKernel.Dispatch();
							}
						}
						this.movePointJointsToParticlesKernel.Dispatch();
						for (int k = 1; k <= this.iterations; k++)
						{
							this.T.Value = (float)k / (float)this.iterations;
							if (this.frame > 20)
							{
								this.integrateIterWithParticleHoldKernel.Dispatch();
							}
							this.pointJointsFixedRigidityKernel.Dispatch();
							this.splineJointsWithParticleHoldKernel.Dispatch();
							this.splineJointsReverseWithParticleHoldKernel.Dispatch();
							this.integrateVelocityInnerKernel.Dispatch();
						}
						this.Step.Value = 1f / (float)this.outerIterations;
						this.DT.Value = num;
						this.DTRecip.Value = 1f / num;
						this.particleCollisionResetKernel.Dispatch();
						if (this.data.IsCollisionEnabled)
						{
							if (this.planeCollisionKernel.Planes != null)
							{
								this.planeCollisionKernel.Dispatch();
							}
							if (this.sphereCollisionKernel.ProcessedSpheres != null)
							{
								this.sphereCollisionKernel.Dispatch();
							}
							if (this.lineSphereCollisionKernel.ProcessedLineSpheres != null)
							{
								this.lineSphereCollisionKernel.Dispatch();
							}
						}
						this.pointJointsFixedRigidityKernel.Dispatch();
						this.splineJointsWithParticleHoldKernel.Dispatch();
						this.splineJointsReverseWithParticleHoldKernel.Dispatch();
						this.integrateVelocityKernel.Dispatch();
						if (this.data.UsePaintedRigidity)
						{
							if (this.lineSphereRigidityIncreaseKernel.RigidityIncreaseLineSpheres != null)
							{
								this.lineSphereRigidityIncreaseKernel.Dispatch();
							}
							if (this.lineSphereRigidityDecreaseKernel.RigidityDecreaseLineSpheres != null)
							{
								this.lineSphereRigidityDecreaseKernel.Dispatch();
							}
							if (this.lineSphereRigiditySetKernel.RigiditySetLineSpheres != null)
							{
								this.lineSphereRigiditySetKernel.Dispatch();
							}
						}
					}
					else
					{
						for (int l = 1; l <= this.iterations; l++)
						{
							this.T.Value = (float)l / (float)this.iterations;
							if (this.frame > 20)
							{
								this.integrateIterKernel.Dispatch();
							}
							this.pointJointsKernel.Dispatch();
							this.distanceJointsKernel.Dispatch();
							if (this.CompressionJointPower.Value != 0f)
							{
								this.compressionJointsKernel.Dispatch();
							}
							if (this.NearbyJointPower.Value != 0f)
							{
								this.nearbyDistanceJointsKernel.Dispatch();
							}
							this.distanceJointsKernel.Dispatch();
							this.splineJointsKernel.Dispatch();
							this.distanceJointsKernel.Dispatch();
							this.integrateVelocityInnerKernel.Dispatch();
						}
						this.Step.Value = 1f / (float)this.outerIterations;
						this.DT.Value = num;
						this.DTRecip.Value = 1f / num;
						this.particleCollisionResetKernel.Dispatch();
						if (this.data.IsCollisionEnabled)
						{
							if (this.planeCollisionKernel.Planes != null)
							{
								this.planeCollisionKernel.Dispatch();
							}
							if (this.sphereCollisionKernel.ProcessedSpheres != null)
							{
								this.sphereCollisionKernel.Dispatch();
							}
							if (this.lineSphereCollisionKernel.ProcessedLineSpheres != null)
							{
								this.lineSphereCollisionKernel.Dispatch();
							}
						}
						this.pointJointsKernel.Dispatch();
						this.integrateVelocityKernel.Dispatch();
					}
				}
			}
		}

		// Token: 0x0600430D RID: 17165 RVA: 0x0013AB30 File Offset: 0x00138F30
		private void DispatchStaticImpl()
		{
			this.T.Value = 1f;
			for (int i = 0; i < this.staticQueue.Count; i++)
			{
				this.staticQueue[i].Dispatch();
			}
		}

		// Token: 0x0600430E RID: 17166 RVA: 0x0013AB7C File Offset: 0x00138F7C
		private void DispatchImpl()
		{
			this.InitData();
			this.UpdateBuffers();
			this.Step.Value = 1f;
			if (this.frame < 1)
			{
				this.resetKernel.Dispatch();
			}
			if (!this.data.StyleMode && this.data.UpdateRigidityJointsBeforeRender)
			{
				this.pointJointsFinalKernel.Dispatch();
			}
			if (this.copySpecificParticlesKernel != null)
			{
				this.copySpecificParticlesKernel.Dispatch();
			}
			if (this.tesselateKernel != null)
			{
				this.tesselateKernel.Dispatch();
			}
			else if (this.tesselateWithNormalsKernel != null)
			{
				if (this.data.StyleMode && this.tesselateWithNormalsRenderRigidityKernel != null)
				{
					this.tesselateWithNormalsRenderRigidityKernel.Dispatch();
				}
				else
				{
					this.tesselateWithNormalsKernel.Dispatch();
				}
			}
			this.frame++;
		}

		// Token: 0x0600430F RID: 17167 RVA: 0x0013AC67 File Offset: 0x00139067
		public override void Dispose()
		{
			base.Dispose();
			if (this.OldTransforms != null)
			{
				this.OldTransforms.Dispose();
			}
		}

		// Token: 0x06004310 RID: 17168 RVA: 0x0013AC88 File Offset: 0x00139088
		public void DebugDraw()
		{
			if (this.data.DebugDraw && this.data.IsPhysicsEnabled && this.data.IsPhysicsEnabledLOD && this.nearbyDistanceJointsKernel != null)
			{
				GPDebugDraw.Draw(this.distanceJointsKernel.DistanceJointsBuffer, this.nearbyDistanceJointsKernel.NearbyDistanceJointsBuffer, this.Particles, false, !this.data.DebugDrawNearbyJoints, this.data.DebugDrawNearbyJoints);
			}
		}

		// Token: 0x06004311 RID: 17169 RVA: 0x0013AD0C File Offset: 0x0013910C
		public void UpdateIsPhysics()
		{
			bool flag = (this.data.IsPhysicsEnabled && this.data.IsPhysicsEnabledLOD) || this.data.StyleMode;
			if (!this.isPhysics && flag)
			{
				this.resetKernel.Dispatch();
			}
			this.isPhysics = flag;
		}

		// Token: 0x04003169 RID: 12649
		private readonly HairDataFacade data;

		// Token: 0x0400316A RID: 12650
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private GpuValue<float> <Weight>k__BackingField;

		// Token: 0x0400316B RID: 12651
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private GpuValue<float> <Step>k__BackingField;

		// Token: 0x0400316C RID: 12652
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private GpuValue<float> <DT>k__BackingField;

		// Token: 0x0400316D RID: 12653
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private GpuValue<float> <DTRecip>k__BackingField;

		// Token: 0x0400316E RID: 12654
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private GpuValue<float> <T>k__BackingField;

		// Token: 0x0400316F RID: 12655
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private GpuValue<Vector3> <AccelDT2>k__BackingField;

		// Token: 0x04003170 RID: 12656
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private GpuValue<float> <InvDrag>k__BackingField;

		// Token: 0x04003171 RID: 12657
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private GpuValue<float> <DistanceScale>k__BackingField;

		// Token: 0x04003172 RID: 12658
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private GpuValue<float> <CompressionDistanceScale>k__BackingField;

		// Token: 0x04003173 RID: 12659
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private GpuValue<float> <NearbyDistanceScale>k__BackingField;

		// Token: 0x04003174 RID: 12660
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private GpuValue<float> <Friction>k__BackingField;

		// Token: 0x04003175 RID: 12661
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private GpuValue<float> <StaticFriction>k__BackingField;

		// Token: 0x04003176 RID: 12662
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private GpuValue<float> <CollisionPower>k__BackingField;

		// Token: 0x04003177 RID: 12663
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private GpuValue<float> <CompressionJointPower>k__BackingField;

		// Token: 0x04003178 RID: 12664
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private GpuValue<float> <NearbyJointPower>k__BackingField;

		// Token: 0x04003179 RID: 12665
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private GpuValue<float> <NearbyJointPowerRolloff>k__BackingField;

		// Token: 0x0400317A RID: 12666
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private GpuValue<float> <SplineJointPower>k__BackingField;

		// Token: 0x0400317B RID: 12667
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private GpuValue<float> <ReverseSplineJointPower>k__BackingField;

		// Token: 0x0400317C RID: 12668
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private GpuValue<float> <DistanceJointPower>k__BackingField;

		// Token: 0x0400317D RID: 12669
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private GpuBuffer<GPParticle> <Particles>k__BackingField;

		// Token: 0x0400317E RID: 12670
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private GpuBuffer<Vector3> <Normals>k__BackingField;

		// Token: 0x0400317F RID: 12671
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private GpuBuffer<Matrix4x4> <Transforms>k__BackingField;

		// Token: 0x04003180 RID: 12672
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private GpuBuffer<Matrix4x4> <OldTransforms>k__BackingField;

		// Token: 0x04003181 RID: 12673
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private GpuBuffer<GPPointJoint> <PointJoints>k__BackingField;

		// Token: 0x04003182 RID: 12674
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private GpuBuffer<float> <PointToPreviousPointDistances>k__BackingField;

		// Token: 0x04003183 RID: 12675
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private GpuValue<int> <IsFixed>k__BackingField;

		// Token: 0x04003184 RID: 12676
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private GpuValue<float> <FixedRigidity>k__BackingField;

		// Token: 0x04003185 RID: 12677
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private GpuBuffer<GPSphereWithDelta> <ProcessedSpheres>k__BackingField;

		// Token: 0x04003186 RID: 12678
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private GpuBuffer<GPLineSphereWithDelta> <ProcessedLineSpheres>k__BackingField;

		// Token: 0x04003187 RID: 12679
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private GpuBuffer<Vector4> <Planes>k__BackingField;

		// Token: 0x04003188 RID: 12680
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private GpuBuffer<GPLineSphere> <CutLineSpheres>k__BackingField;

		// Token: 0x04003189 RID: 12681
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private GpuBuffer<GPLineSphere> <GrowLineSpheres>k__BackingField;

		// Token: 0x0400318A RID: 12682
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private GpuBuffer<GPLineSphere> <HoldLineSpheres>k__BackingField;

		// Token: 0x0400318B RID: 12683
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private GpuBuffer<GPLineSphereWithMatrixDelta> <GrabLineSpheres>k__BackingField;

		// Token: 0x0400318C RID: 12684
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private GpuBuffer<GPLineSphere> <PushLineSpheres>k__BackingField;

		// Token: 0x0400318D RID: 12685
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private GpuBuffer<GPLineSphere> <PullLineSpheres>k__BackingField;

		// Token: 0x0400318E RID: 12686
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private GpuBuffer<GPLineSphereWithDelta> <BrushLineSpheres>k__BackingField;

		// Token: 0x0400318F RID: 12687
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private GpuBuffer<GPLineSphere> <RigidityIncreaseLineSpheres>k__BackingField;

		// Token: 0x04003190 RID: 12688
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private GpuBuffer<GPLineSphere> <RigidityDecreaseLineSpheres>k__BackingField;

		// Token: 0x04003191 RID: 12689
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private GpuBuffer<GPLineSphere> <RigiditySetLineSpheres>k__BackingField;

		// Token: 0x04003192 RID: 12690
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private GpuBuffer<GPParticle> <OutParticles>k__BackingField;

		// Token: 0x04003193 RID: 12691
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private GpuBuffer<float> <OutParticlesMap>k__BackingField;

		// Token: 0x04003194 RID: 12692
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private GpuBuffer<RenderParticle> <RenderParticles>k__BackingField;

		// Token: 0x04003195 RID: 12693
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private GpuBuffer<TessRenderParticle> <TessRenderParticles>k__BackingField;

		// Token: 0x04003196 RID: 12694
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private GpuValue<int> <TessRenderParticlesCount>k__BackingField;

		// Token: 0x04003197 RID: 12695
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private GpuBuffer<Vector3> <RandomsPerStrand>k__BackingField;

		// Token: 0x04003198 RID: 12696
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private GpuValue<int> <Segments>k__BackingField;

		// Token: 0x04003199 RID: 12697
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private GpuValue<int> <TessSegments>k__BackingField;

		// Token: 0x0400319A RID: 12698
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private GpuValue<Vector3> <WavinessAxis>k__BackingField;

		// Token: 0x0400319B RID: 12699
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private GpuValue<float> <WavinessFrequencyRandomness>k__BackingField;

		// Token: 0x0400319C RID: 12700
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private GpuValue<float> <WavinessScaleRandomness>k__BackingField;

		// Token: 0x0400319D RID: 12701
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private GpuValue<bool> <WavinessAllowReverse>k__BackingField;

		// Token: 0x0400319E RID: 12702
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private GpuValue<bool> <WavinessAllowFlipAxis>k__BackingField;

		// Token: 0x0400319F RID: 12703
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private GpuValue<float> <WavinessNormalAdjust>k__BackingField;

		// Token: 0x040031A0 RID: 12704
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private GpuValue<Vector3> <LightCenter>k__BackingField;

		// Token: 0x040031A1 RID: 12705
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private GpuValue<float> <NormalRandomize>k__BackingField;

		// Token: 0x040031A2 RID: 12706
		private ResetToPointJointsKernel resetKernel;

		// Token: 0x040031A3 RID: 12707
		private IntegrateVelocityKernel integrateVelocityKernel;

		// Token: 0x040031A4 RID: 12708
		private IntegrateVelocityInnerKernel integrateVelocityInnerKernel;

		// Token: 0x040031A5 RID: 12709
		private IntegrateIterKernel integrateIterKernel;

		// Token: 0x040031A6 RID: 12710
		private IntegrateIterWithParticleHoldKernel integrateIterWithParticleHoldKernel;

		// Token: 0x040031A7 RID: 12711
		private DistanceJointsKernel distanceJointsKernel;

		// Token: 0x040031A8 RID: 12712
		private CompressionJointsKernel compressionJointsKernel;

		// Token: 0x040031A9 RID: 12713
		private NearbyDistanceJointsKernel nearbyDistanceJointsKernel;

		// Token: 0x040031AA RID: 12714
		private ParticleCollisionResetKernel particleCollisionResetKernel;

		// Token: 0x040031AB RID: 12715
		private ParticleLineSphereCollisionKernel lineSphereCollisionKernel;

		// Token: 0x040031AC RID: 12716
		private ParticleSphereCollisionKernel sphereCollisionKernel;

		// Token: 0x040031AD RID: 12717
		private ParticlePlaneCollisionKernel planeCollisionKernel;

		// Token: 0x040031AE RID: 12718
		private ParticleLineSphereHoldResetKernel lineSphereHoldResetKernel;

		// Token: 0x040031AF RID: 12719
		private ParticleLineSphereHoldKernel lineSphereHoldKernel;

		// Token: 0x040031B0 RID: 12720
		private ParticleLineSphereGrabKernel lineSphereGrabKernel;

		// Token: 0x040031B1 RID: 12721
		private ParticleLineSpherePushKernel lineSpherePushKernel;

		// Token: 0x040031B2 RID: 12722
		private ParticleLineSpherePullKernel lineSpherePullKernel;

		// Token: 0x040031B3 RID: 12723
		private ParticleLineSphereBrushKernel lineSphereBrushKernel;

		// Token: 0x040031B4 RID: 12724
		private ParticleLineSphereGrowKernel lineSphereGrowKernel;

		// Token: 0x040031B5 RID: 12725
		private ParticleLineSphereCutKernel lineSphereCutKernel;

		// Token: 0x040031B6 RID: 12726
		private ParticleLineSphereRigidityIncreaseKernel lineSphereRigidityIncreaseKernel;

		// Token: 0x040031B7 RID: 12727
		private ParticleLineSphereRigidityDecreaseKernel lineSphereRigidityDecreaseKernel;

		// Token: 0x040031B8 RID: 12728
		private ParticleLineSphereRigiditySetKernel lineSphereRigiditySetKernel;

		// Token: 0x040031B9 RID: 12729
		private DistanceJointsAdjustKernel distanceJointsAdjustKernel;

		// Token: 0x040031BA RID: 12730
		private SplineJointsKernel splineJointsKernel;

		// Token: 0x040031BB RID: 12731
		private SplineJointsReverseKernel splineJointsReverseKernel;

		// Token: 0x040031BC RID: 12732
		private SplineJointsWithParticleHoldKernel splineJointsWithParticleHoldKernel;

		// Token: 0x040031BD RID: 12733
		private SplineJointsReverseWithParticleHoldKernel splineJointsReverseWithParticleHoldKernel;

		// Token: 0x040031BE RID: 12734
		private PointJointsKernel pointJointsKernel;

		// Token: 0x040031BF RID: 12735
		private PointJointsFixedRigidityKernel pointJointsFixedRigidityKernel;

		// Token: 0x040031C0 RID: 12736
		private PointJointsFinalKernel pointJointsFinalKernel;

		// Token: 0x040031C1 RID: 12737
		private MovePointJointsToParticlesKernel movePointJointsToParticlesKernel;

		// Token: 0x040031C2 RID: 12738
		private CopySpecificParticlesKernel copySpecificParticlesKernel;

		// Token: 0x040031C3 RID: 12739
		private TesselateKernel tesselateKernel;

		// Token: 0x040031C4 RID: 12740
		private TesselateWithNormalsKernel tesselateWithNormalsKernel;

		// Token: 0x040031C5 RID: 12741
		private TesselateWithNormalsRenderRigidityKernel tesselateWithNormalsRenderRigidityKernel;

		// Token: 0x040031C6 RID: 12742
		private int frame;

		// Token: 0x040031C7 RID: 12743
		private int outerIterations;

		// Token: 0x040031C8 RID: 12744
		private int iterations;

		// Token: 0x040031C9 RID: 12745
		private List<KernelBase> staticQueue = new List<KernelBase>();

		// Token: 0x040031CA RID: 12746
		private bool isPhysics;
	}
}
