using System;
using System.Collections.Generic;
using GPUTools.Common.Scripts.PL.Tools;
using GPUTools.Hair.Scripts.Geometry.Constrains;
using GPUTools.Hair.Scripts.Runtime.Render;
using GPUTools.Physics.Scripts.Types.Dynamic;
using GPUTools.Physics.Scripts.Types.Joints;
using GPUTools.Physics.Scripts.Types.Shapes;
using UnityEngine;

namespace GPUTools.Hair.Scripts.Runtime.Data
{
	// Token: 0x02000A19 RID: 2585
	public class HairDataFacade
	{
		// Token: 0x06004188 RID: 16776 RVA: 0x00137FCE File Offset: 0x001363CE
		public HairDataFacade(HairSettings settings)
		{
			this.settings = settings;
		}

		// Token: 0x170007A8 RID: 1960
		// (get) Token: 0x06004189 RID: 16777 RVA: 0x00137FDD File Offset: 0x001363DD
		public bool DebugDraw
		{
			get
			{
				return this.settings.PhysicsSettings.DebugDraw;
			}
		}

		// Token: 0x170007A9 RID: 1961
		// (get) Token: 0x0600418A RID: 16778 RVA: 0x00137FEF File Offset: 0x001363EF
		public bool DebugDrawNearbyJoints
		{
			get
			{
				return this.settings.PhysicsSettings.DebugDrawNearbyJoints;
			}
		}

		// Token: 0x170007AA RID: 1962
		// (get) Token: 0x0600418B RID: 16779 RVA: 0x00138001 File Offset: 0x00136401
		public int Iterations
		{
			get
			{
				return this.settings.PhysicsSettings.Iterations;
			}
		}

		// Token: 0x170007AB RID: 1963
		// (get) Token: 0x0600418C RID: 16780 RVA: 0x00138013 File Offset: 0x00136413
		public float Drag
		{
			get
			{
				return this.settings.PhysicsSettings.Drag;
			}
		}

		// Token: 0x170007AC RID: 1964
		// (get) Token: 0x0600418D RID: 16781 RVA: 0x00138028 File Offset: 0x00136428
		public float InvDrag
		{
			get
			{
				float num = this.settings.PhysicsSettings.Drag * (1f / (float)this.Iterations);
				return 1f - num;
			}
		}

		// Token: 0x170007AD RID: 1965
		// (get) Token: 0x0600418E RID: 16782 RVA: 0x0013805B File Offset: 0x0013645B
		public float WorldScale
		{
			get
			{
				return this.settings.PhysicsSettings.WorldScale;
			}
		}

		// Token: 0x170007AE RID: 1966
		// (get) Token: 0x0600418F RID: 16783 RVA: 0x0013806D File Offset: 0x0013646D
		public bool FastMovement
		{
			get
			{
				return this.settings.PhysicsSettings.FastMovement;
			}
		}

		// Token: 0x170007AF RID: 1967
		// (get) Token: 0x06004190 RID: 16784 RVA: 0x00138080 File Offset: 0x00136480
		public Vector3 Gravity
		{
			get
			{
				if (this.StyleMode)
				{
					return Physics.gravity * this.settings.PhysicsSettings.StyleModeGravityMultiplier;
				}
				return Physics.gravity * this.settings.PhysicsSettings.GravityMultiplier;
			}
		}

		// Token: 0x170007B0 RID: 1968
		// (get) Token: 0x06004191 RID: 16785 RVA: 0x001380CD File Offset: 0x001364CD
		public Vector3 Wind
		{
			get
			{
				return this.settings.RuntimeData.Wind;
			}
		}

		// Token: 0x170007B1 RID: 1969
		// (get) Token: 0x06004192 RID: 16786 RVA: 0x001380DF File Offset: 0x001364DF
		public bool IsPhysicsEnabled
		{
			get
			{
				return this.settings.PhysicsSettings.IsEnabled;
			}
		}

		// Token: 0x170007B2 RID: 1970
		// (get) Token: 0x06004193 RID: 16787 RVA: 0x001380F1 File Offset: 0x001364F1
		public bool IsCollisionEnabled
		{
			get
			{
				return this.settings.PhysicsSettings.IsCollisionEnabled;
			}
		}

		// Token: 0x170007B3 RID: 1971
		// (get) Token: 0x06004194 RID: 16788 RVA: 0x00138103 File Offset: 0x00136503
		public float Weight
		{
			get
			{
				return this.settings.PhysicsSettings.Weight;
			}
		}

		// Token: 0x170007B4 RID: 1972
		// (get) Token: 0x06004195 RID: 16789 RVA: 0x00138115 File Offset: 0x00136515
		public float Friction
		{
			get
			{
				return this.settings.PhysicsSettings.Friction;
			}
		}

		// Token: 0x170007B5 RID: 1973
		// (get) Token: 0x06004196 RID: 16790 RVA: 0x00138127 File Offset: 0x00136527
		public float CollisionPower
		{
			get
			{
				return this.settings.PhysicsSettings.CollisionPower;
			}
		}

		// Token: 0x170007B6 RID: 1974
		// (get) Token: 0x06004197 RID: 16791 RVA: 0x00138139 File Offset: 0x00136539
		public float CompressionJointPower
		{
			get
			{
				return this.settings.PhysicsSettings.CompressionJointPower;
			}
		}

		// Token: 0x170007B7 RID: 1975
		// (get) Token: 0x06004198 RID: 16792 RVA: 0x0013814B File Offset: 0x0013654B
		public float NearbyJointPower
		{
			get
			{
				return this.settings.PhysicsSettings.NearbyJointPower;
			}
		}

		// Token: 0x170007B8 RID: 1976
		// (get) Token: 0x06004199 RID: 16793 RVA: 0x0013815D File Offset: 0x0013655D
		public float NearbyJointPowerRolloff
		{
			get
			{
				return this.settings.PhysicsSettings.NearbyJointPowerRolloff;
			}
		}

		// Token: 0x170007B9 RID: 1977
		// (get) Token: 0x0600419A RID: 16794 RVA: 0x0013816F File Offset: 0x0013656F
		public float SplineJointPower
		{
			get
			{
				return this.settings.PhysicsSettings.SplineJointPower;
			}
		}

		// Token: 0x170007BA RID: 1978
		// (get) Token: 0x0600419B RID: 16795 RVA: 0x00138181 File Offset: 0x00136581
		public float ReverseSplineJointPower
		{
			get
			{
				return this.settings.PhysicsSettings.ReverseSplineJointPower;
			}
		}

		// Token: 0x170007BB RID: 1979
		// (get) Token: 0x0600419C RID: 16796 RVA: 0x00138193 File Offset: 0x00136593
		public bool RunPhysicsOnUpdate
		{
			get
			{
				return this.settings.PhysicsSettings.RunPhysicsOnUpdate;
			}
		}

		// Token: 0x170007BC RID: 1980
		// (get) Token: 0x0600419D RID: 16797 RVA: 0x001381A5 File Offset: 0x001365A5
		public bool UsePaintedRigidity
		{
			get
			{
				return this.settings.PhysicsSettings.UsePaintedRigidity;
			}
		}

		// Token: 0x170007BD RID: 1981
		// (get) Token: 0x0600419E RID: 16798 RVA: 0x001381B7 File Offset: 0x001365B7
		public bool StyleMode
		{
			get
			{
				return this.settings.PhysicsSettings.StyleMode;
			}
		}

		// Token: 0x170007BE RID: 1982
		// (get) Token: 0x0600419F RID: 16799 RVA: 0x001381C9 File Offset: 0x001365C9
		public GpuBuffer<Matrix4x4> MatricesBuffer
		{
			get
			{
				return this.settings.StandsSettings.Provider.GetTransformsBuffer();
			}
		}

		// Token: 0x170007BF RID: 1983
		// (get) Token: 0x060041A0 RID: 16800 RVA: 0x001381E0 File Offset: 0x001365E0
		public GpuBuffer<Vector3> NormalsBuffer
		{
			get
			{
				return this.settings.StandsSettings.Provider.GetNormalsBuffer();
			}
		}

		// Token: 0x170007C0 RID: 1984
		// (get) Token: 0x060041A1 RID: 16801 RVA: 0x001381F7 File Offset: 0x001365F7
		public GpuBuffer<GPParticle> Particles
		{
			get
			{
				return this.settings.RuntimeData.Particles;
			}
		}

		// Token: 0x170007C1 RID: 1985
		// (get) Token: 0x060041A2 RID: 16802 RVA: 0x00138209 File Offset: 0x00136609
		public GroupedData<GPDistanceJoint> DistanceJoints
		{
			get
			{
				return this.settings.RuntimeData.DistanceJoints;
			}
		}

		// Token: 0x170007C2 RID: 1986
		// (get) Token: 0x060041A3 RID: 16803 RVA: 0x0013821B File Offset: 0x0013661B
		public GpuBuffer<GPDistanceJoint> DistanceJointsBuffer
		{
			get
			{
				return this.settings.RuntimeData.DistanceJointsBuffer;
			}
		}

		// Token: 0x170007C3 RID: 1987
		// (get) Token: 0x060041A4 RID: 16804 RVA: 0x0013822D File Offset: 0x0013662D
		public GroupedData<GPDistanceJoint> CompressionJoints
		{
			get
			{
				return this.settings.RuntimeData.CompressionJoints;
			}
		}

		// Token: 0x170007C4 RID: 1988
		// (get) Token: 0x060041A5 RID: 16805 RVA: 0x0013823F File Offset: 0x0013663F
		public GpuBuffer<GPDistanceJoint> CompressionJointsBuffer
		{
			get
			{
				return this.settings.RuntimeData.CompressionJointsBuffer;
			}
		}

		// Token: 0x170007C5 RID: 1989
		// (get) Token: 0x060041A6 RID: 16806 RVA: 0x00138251 File Offset: 0x00136651
		public GroupedData<GPDistanceJoint> NearbyDistanceJoints
		{
			get
			{
				return this.settings.RuntimeData.NearbyDistanceJoints;
			}
		}

		// Token: 0x170007C6 RID: 1990
		// (get) Token: 0x060041A7 RID: 16807 RVA: 0x00138263 File Offset: 0x00136663
		public GpuBuffer<GPDistanceJoint> NearbyDistanceJointsBuffer
		{
			get
			{
				return this.settings.RuntimeData.NearbyDistanceJointsBuffer;
			}
		}

		// Token: 0x170007C7 RID: 1991
		// (get) Token: 0x060041A8 RID: 16808 RVA: 0x00138275 File Offset: 0x00136675
		public GpuBuffer<GPPointJoint> PointJoints
		{
			get
			{
				return this.settings.RuntimeData.PointJoints;
			}
		}

		// Token: 0x170007C8 RID: 1992
		// (get) Token: 0x060041A9 RID: 16809 RVA: 0x00138287 File Offset: 0x00136687
		public GpuBuffer<float> PointToPreviousPointDistances
		{
			get
			{
				return this.settings.RuntimeData.PointToPreviousPointDistances;
			}
		}

		// Token: 0x170007C9 RID: 1993
		// (get) Token: 0x060041AA RID: 16810 RVA: 0x00138299 File Offset: 0x00136699
		public List<HairJointArea> JointAreas
		{
			get
			{
				return this.settings.PhysicsSettings.JointAreas;
			}
		}

		// Token: 0x170007CA RID: 1994
		// (get) Token: 0x060041AB RID: 16811 RVA: 0x001382AC File Offset: 0x001366AC
		public Vector4 Size
		{
			get
			{
				int standsNum = this.settings.StandsSettings.Provider.GetStandsNum();
				int segmentsNum = this.settings.StandsSettings.Provider.GetSegmentsNum();
				return new Vector4((float)standsNum, (float)segmentsNum);
			}
		}

		// Token: 0x170007CB RID: 1995
		// (get) Token: 0x060041AC RID: 16812 RVA: 0x001382EE File Offset: 0x001366EE
		public GpuBuffer<GPSphereWithDelta> ProcessedSpheres
		{
			get
			{
				return this.settings.RuntimeData.ProcessedSpheres;
			}
		}

		// Token: 0x170007CC RID: 1996
		// (get) Token: 0x060041AD RID: 16813 RVA: 0x00138300 File Offset: 0x00136700
		public GpuBuffer<GPLineSphereWithDelta> ProcessedLineSpheres
		{
			get
			{
				return this.settings.RuntimeData.ProcessedLineSpheres;
			}
		}

		// Token: 0x170007CD RID: 1997
		// (get) Token: 0x060041AE RID: 16814 RVA: 0x00138312 File Offset: 0x00136712
		public GpuBuffer<Vector4> Planes
		{
			get
			{
				return this.settings.RuntimeData.Planes;
			}
		}

		// Token: 0x170007CE RID: 1998
		// (get) Token: 0x060041AF RID: 16815 RVA: 0x00138324 File Offset: 0x00136724
		public GpuBuffer<GPLineSphere> CutLineSpheres
		{
			get
			{
				return this.settings.RuntimeData.CutLineSpheres;
			}
		}

		// Token: 0x170007CF RID: 1999
		// (get) Token: 0x060041B0 RID: 16816 RVA: 0x00138336 File Offset: 0x00136736
		public GpuBuffer<GPLineSphere> GrowLineSpheres
		{
			get
			{
				return this.settings.RuntimeData.GrowLineSpheres;
			}
		}

		// Token: 0x170007D0 RID: 2000
		// (get) Token: 0x060041B1 RID: 16817 RVA: 0x00138348 File Offset: 0x00136748
		public GpuBuffer<GPLineSphere> HoldLineSpheres
		{
			get
			{
				return this.settings.RuntimeData.HoldLineSpheres;
			}
		}

		// Token: 0x170007D1 RID: 2001
		// (get) Token: 0x060041B2 RID: 16818 RVA: 0x0013835A File Offset: 0x0013675A
		public GpuBuffer<GPLineSphereWithMatrixDelta> GrabLineSpheres
		{
			get
			{
				return this.settings.RuntimeData.GrabLineSpheres;
			}
		}

		// Token: 0x170007D2 RID: 2002
		// (get) Token: 0x060041B3 RID: 16819 RVA: 0x0013836C File Offset: 0x0013676C
		public GpuBuffer<GPLineSphere> PushLineSpheres
		{
			get
			{
				return this.settings.RuntimeData.PushLineSpheres;
			}
		}

		// Token: 0x170007D3 RID: 2003
		// (get) Token: 0x060041B4 RID: 16820 RVA: 0x0013837E File Offset: 0x0013677E
		public GpuBuffer<GPLineSphere> PullLineSpheres
		{
			get
			{
				return this.settings.RuntimeData.PullLineSpheres;
			}
		}

		// Token: 0x170007D4 RID: 2004
		// (get) Token: 0x060041B5 RID: 16821 RVA: 0x00138390 File Offset: 0x00136790
		public GpuBuffer<GPLineSphereWithDelta> BrushLineSpheres
		{
			get
			{
				return this.settings.RuntimeData.BrushLineSpheres;
			}
		}

		// Token: 0x170007D5 RID: 2005
		// (get) Token: 0x060041B6 RID: 16822 RVA: 0x001383A2 File Offset: 0x001367A2
		public GpuBuffer<GPLineSphere> RigidityIncreaseLineSpheres
		{
			get
			{
				return this.settings.RuntimeData.RigidityIncreaseLineSpheres;
			}
		}

		// Token: 0x170007D6 RID: 2006
		// (get) Token: 0x060041B7 RID: 16823 RVA: 0x001383B4 File Offset: 0x001367B4
		public GpuBuffer<GPLineSphere> RigidityDecreaseLineSpheres
		{
			get
			{
				return this.settings.RuntimeData.RigidityDecreaseLineSpheres;
			}
		}

		// Token: 0x170007D7 RID: 2007
		// (get) Token: 0x060041B8 RID: 16824 RVA: 0x001383C6 File Offset: 0x001367C6
		public GpuBuffer<GPLineSphere> RigiditySetLineSpheres
		{
			get
			{
				return this.settings.RuntimeData.RigiditySetLineSpheres;
			}
		}

		// Token: 0x170007D8 RID: 2008
		// (get) Token: 0x060041B9 RID: 16825 RVA: 0x001383D8 File Offset: 0x001367D8
		public GpuBuffer<TessRenderParticle> TessRenderParticles
		{
			get
			{
				return this.settings.RuntimeData.TessRenderParticles;
			}
		}

		// Token: 0x170007D9 RID: 2009
		// (get) Token: 0x060041BA RID: 16826 RVA: 0x001383EA File Offset: 0x001367EA
		public GpuBuffer<GPParticle> OutParticles
		{
			get
			{
				return this.settings.RuntimeData.OutParticles;
			}
		}

		// Token: 0x170007DA RID: 2010
		// (get) Token: 0x060041BB RID: 16827 RVA: 0x001383FC File Offset: 0x001367FC
		public GpuBuffer<float> OutParticlesMap
		{
			get
			{
				return this.settings.RuntimeData.OutParticlesMap;
			}
		}

		// Token: 0x170007DB RID: 2011
		// (get) Token: 0x060041BC RID: 16828 RVA: 0x0013840E File Offset: 0x0013680E
		public GpuBuffer<RenderParticle> RenderParticles
		{
			get
			{
				return this.settings.RuntimeData.RenderParticles;
			}
		}

		// Token: 0x170007DC RID: 2012
		// (get) Token: 0x060041BD RID: 16829 RVA: 0x00138420 File Offset: 0x00136820
		public GpuBuffer<Vector3> RandomsPerStrand
		{
			get
			{
				return this.settings.RuntimeData.RandomsPerStrand;
			}
		}

		// Token: 0x170007DD RID: 2013
		// (get) Token: 0x060041BE RID: 16830 RVA: 0x00138432 File Offset: 0x00136832
		public Vector3 WavinessAxis
		{
			get
			{
				return this.settings.RenderSettings.WavinessAxis;
			}
		}

		// Token: 0x170007DE RID: 2014
		// (get) Token: 0x060041BF RID: 16831 RVA: 0x00138444 File Offset: 0x00136844
		public Vector3 WorldWavinessAxis
		{
			get
			{
				return this.settings.transform.TransformVector(this.WavinessAxis);
			}
		}

		// Token: 0x170007DF RID: 2015
		// (get) Token: 0x060041C0 RID: 16832 RVA: 0x0013845C File Offset: 0x0013685C
		public float WavinessFrequencyRandomness
		{
			get
			{
				return this.settings.RenderSettings.WavinessFrequencyRandomness;
			}
		}

		// Token: 0x170007E0 RID: 2016
		// (get) Token: 0x060041C1 RID: 16833 RVA: 0x0013846E File Offset: 0x0013686E
		public float WavinessScaleRandomness
		{
			get
			{
				return this.settings.RenderSettings.WavinessScaleRandomness;
			}
		}

		// Token: 0x170007E1 RID: 2017
		// (get) Token: 0x060041C2 RID: 16834 RVA: 0x00138480 File Offset: 0x00136880
		public bool WavinessAllowReverse
		{
			get
			{
				return this.settings.RenderSettings.WavinessAllowReverse;
			}
		}

		// Token: 0x170007E2 RID: 2018
		// (get) Token: 0x060041C3 RID: 16835 RVA: 0x00138492 File Offset: 0x00136892
		public bool WavinessAllowFlipAxis
		{
			get
			{
				return this.settings.RenderSettings.WavinessAllowFlipAxis;
			}
		}

		// Token: 0x170007E3 RID: 2019
		// (get) Token: 0x060041C4 RID: 16836 RVA: 0x001384A4 File Offset: 0x001368A4
		public float WavinessNormalAdjust
		{
			get
			{
				return this.settings.RenderSettings.WavinessNormalAdjust;
			}
		}

		// Token: 0x170007E4 RID: 2020
		// (get) Token: 0x060041C5 RID: 16837 RVA: 0x001384B6 File Offset: 0x001368B6
		public bool StyleModeShowCurls
		{
			get
			{
				return this.settings.RenderSettings.StyleModeShowCurls;
			}
		}

		// Token: 0x170007E5 RID: 2021
		// (get) Token: 0x060041C6 RID: 16838 RVA: 0x001384C8 File Offset: 0x001368C8
		public Vector3 LightCenter
		{
			get
			{
				return this.settings.StandsSettings.HeadCenterWorld;
			}
		}

		// Token: 0x170007E6 RID: 2022
		// (get) Token: 0x060041C7 RID: 16839 RVA: 0x001384DA File Offset: 0x001368DA
		public float StandWidth
		{
			get
			{
				return this.settings.LODSettings.GetWidth(this.LightCenter);
			}
		}

		// Token: 0x170007E7 RID: 2023
		// (get) Token: 0x060041C8 RID: 16840 RVA: 0x001384F4 File Offset: 0x001368F4
		public Vector3 TessFactor
		{
			get
			{
				int detail = this.settings.LODSettings.GetDetail(this.LightCenter);
				int dencity = this.settings.LODSettings.GetDencity(this.LightCenter);
				return new Vector4((float)detail, (float)dencity, 0.99f / (float)detail, 0.99f / (float)dencity);
			}
		}

		// Token: 0x170007E8 RID: 2024
		// (get) Token: 0x060041C9 RID: 16841 RVA: 0x0013854D File Offset: 0x0013694D
		public bool IsPhysicsEnabledLOD
		{
			get
			{
				return this.settings.LODSettings.IsPhysicsEnabled(this.LightCenter);
			}
		}

		// Token: 0x170007E9 RID: 2025
		// (get) Token: 0x060041CA RID: 16842 RVA: 0x00138565 File Offset: 0x00136965
		public bool CastShadows
		{
			get
			{
				return this.settings.ShadowSettings.CastShadows;
			}
		}

		// Token: 0x170007EA RID: 2026
		// (get) Token: 0x060041CB RID: 16843 RVA: 0x00138577 File Offset: 0x00136977
		public bool ReseiveShadows
		{
			get
			{
				return this.settings.ShadowSettings.ReseiveShadows;
			}
		}

		// Token: 0x170007EB RID: 2027
		// (get) Token: 0x060041CC RID: 16844 RVA: 0x00138589 File Offset: 0x00136989
		public float SpecularShift
		{
			get
			{
				return this.settings.RenderSettings.SpecularShift;
			}
		}

		// Token: 0x170007EC RID: 2028
		// (get) Token: 0x060041CD RID: 16845 RVA: 0x0013859B File Offset: 0x0013699B
		public float PrimarySpecular
		{
			get
			{
				return this.settings.RenderSettings.PrimarySpecular;
			}
		}

		// Token: 0x170007ED RID: 2029
		// (get) Token: 0x060041CE RID: 16846 RVA: 0x001385AD File Offset: 0x001369AD
		public float SecondarySpecular
		{
			get
			{
				return this.settings.RenderSettings.SecondarySpecular;
			}
		}

		// Token: 0x170007EE RID: 2030
		// (get) Token: 0x060041CF RID: 16847 RVA: 0x001385BF File Offset: 0x001369BF
		public Color SpecularColor
		{
			get
			{
				return this.settings.RenderSettings.SpecularColor;
			}
		}

		// Token: 0x170007EF RID: 2031
		// (get) Token: 0x060041D0 RID: 16848 RVA: 0x001385D1 File Offset: 0x001369D1
		public float Diffuse
		{
			get
			{
				return this.settings.RenderSettings.Diffuse;
			}
		}

		// Token: 0x170007F0 RID: 2032
		// (get) Token: 0x060041D1 RID: 16849 RVA: 0x001385E3 File Offset: 0x001369E3
		public float FresnelPower
		{
			get
			{
				return this.settings.RenderSettings.FresnelPower;
			}
		}

		// Token: 0x170007F1 RID: 2033
		// (get) Token: 0x060041D2 RID: 16850 RVA: 0x001385F5 File Offset: 0x001369F5
		public float FresnelAttenuation
		{
			get
			{
				return this.settings.RenderSettings.FresnelAttenuation;
			}
		}

		// Token: 0x170007F2 RID: 2034
		// (get) Token: 0x060041D3 RID: 16851 RVA: 0x00138607 File Offset: 0x00136A07
		public float NormalRandomize
		{
			get
			{
				return this.settings.RenderSettings.NormalRandomize;
			}
		}

		// Token: 0x170007F3 RID: 2035
		// (get) Token: 0x060041D4 RID: 16852 RVA: 0x00138619 File Offset: 0x00136A19
		public Vector3 Length
		{
			get
			{
				return new Vector4(this.settings.RenderSettings.Length1, this.settings.RenderSettings.Length2, this.settings.RenderSettings.Length3);
			}
		}

		// Token: 0x170007F4 RID: 2036
		// (get) Token: 0x060041D5 RID: 16853 RVA: 0x00138655 File Offset: 0x00136A55
		public Material material
		{
			get
			{
				return this.settings.RenderSettings.material;
			}
		}

		// Token: 0x170007F5 RID: 2037
		// (get) Token: 0x060041D6 RID: 16854 RVA: 0x00138667 File Offset: 0x00136A67
		public GpuBuffer<RenderParticle> ParticlesData
		{
			get
			{
				return this.settings.RuntimeData.RenderParticles;
			}
		}

		// Token: 0x170007F6 RID: 2038
		// (get) Token: 0x060041D7 RID: 16855 RVA: 0x00138679 File Offset: 0x00136A79
		public GpuBuffer<Vector3> Barycentrics
		{
			get
			{
				return this.settings.RuntimeData.Barycentrics;
			}
		}

		// Token: 0x170007F7 RID: 2039
		// (get) Token: 0x060041D8 RID: 16856 RVA: 0x0013868B File Offset: 0x00136A8B
		public GpuBuffer<Vector3> BarycentricsFixed
		{
			get
			{
				return this.settings.RuntimeData.BarycentricsFixed;
			}
		}

		// Token: 0x170007F8 RID: 2040
		// (get) Token: 0x060041D9 RID: 16857 RVA: 0x0013869D File Offset: 0x00136A9D
		public int[] Indices
		{
			get
			{
				return this.settings.StandsSettings.Provider.GetIndices();
			}
		}

		// Token: 0x170007F9 RID: 2041
		// (get) Token: 0x060041DA RID: 16858 RVA: 0x001386B4 File Offset: 0x00136AB4
		public Bounds Bounds
		{
			get
			{
				return this.settings.StandsSettings.Provider.GetBounds();
			}
		}

		// Token: 0x170007FA RID: 2042
		// (get) Token: 0x060041DB RID: 16859 RVA: 0x001386CB File Offset: 0x00136ACB
		public float Volume
		{
			get
			{
				return this.settings.RenderSettings.Volume;
			}
		}

		// Token: 0x170007FB RID: 2043
		// (get) Token: 0x060041DC RID: 16860 RVA: 0x001386DD File Offset: 0x00136ADD
		public float RandomTexColorPower
		{
			get
			{
				return this.settings.RenderSettings.RandomTexColorPower;
			}
		}

		// Token: 0x170007FC RID: 2044
		// (get) Token: 0x060041DD RID: 16861 RVA: 0x001386EF File Offset: 0x00136AEF
		public float RandomTexColorOffset
		{
			get
			{
				return this.settings.RenderSettings.RandomTexColorOffset;
			}
		}

		// Token: 0x170007FD RID: 2045
		// (get) Token: 0x060041DE RID: 16862 RVA: 0x00138701 File Offset: 0x00136B01
		public float IBLFactor
		{
			get
			{
				return this.settings.RenderSettings.IBLFactor;
			}
		}

		// Token: 0x170007FE RID: 2046
		// (get) Token: 0x060041DF RID: 16863 RVA: 0x00138713 File Offset: 0x00136B13
		public float MaxSpread
		{
			get
			{
				return this.settings.RenderSettings.MaxSpread;
			}
		}

		// Token: 0x170007FF RID: 2047
		// (get) Token: 0x060041E0 RID: 16864 RVA: 0x00138725 File Offset: 0x00136B25
		public bool UpdateRigidityJointsBeforeRender
		{
			get
			{
				return this.settings.PhysicsSettings.UpdateRigidityJointsBeforeRender;
			}
		}

		// Token: 0x04003117 RID: 12567
		private readonly HairSettings settings;
	}
}
