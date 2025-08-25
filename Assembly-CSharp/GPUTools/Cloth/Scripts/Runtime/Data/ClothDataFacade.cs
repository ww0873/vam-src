using System;
using GPUTools.Cloth.Scripts.Types;
using GPUTools.Common.Scripts.PL.Tools;
using GPUTools.Physics.Scripts.Types.Dynamic;
using GPUTools.Physics.Scripts.Types.Joints;
using GPUTools.Physics.Scripts.Types.Shapes;
using GPUTools.Skinner.Scripts.Providers;
using UnityEngine;

namespace GPUTools.Cloth.Scripts.Runtime.Data
{
	// Token: 0x020009A2 RID: 2466
	public class ClothDataFacade
	{
		// Token: 0x06003D89 RID: 15753 RVA: 0x0012AC22 File Offset: 0x00129022
		public ClothDataFacade(ClothSettings settings)
		{
			this.settings = settings;
		}

		// Token: 0x170006CB RID: 1739
		// (get) Token: 0x06003D8A RID: 15754 RVA: 0x0012AC31 File Offset: 0x00129031
		public MeshProvider MeshProvider
		{
			get
			{
				return this.settings.MeshProvider;
			}
		}

		// Token: 0x170006CC RID: 1740
		// (get) Token: 0x06003D8B RID: 15755 RVA: 0x0012AC3E File Offset: 0x0012903E
		public bool DebugDraw
		{
			get
			{
				return this.settings.PhysicsDebugDraw;
			}
		}

		// Token: 0x170006CD RID: 1741
		// (get) Token: 0x06003D8C RID: 15756 RVA: 0x0012AC4B File Offset: 0x0012904B
		public int Iterations
		{
			get
			{
				return this.settings.Iterations;
			}
		}

		// Token: 0x170006CE RID: 1742
		// (get) Token: 0x06003D8D RID: 15757 RVA: 0x0012AC58 File Offset: 0x00129058
		public int InnerIterations
		{
			get
			{
				return this.settings.InnerIterations;
			}
		}

		// Token: 0x170006CF RID: 1743
		// (get) Token: 0x06003D8E RID: 15758 RVA: 0x0012AC65 File Offset: 0x00129065
		public float Drag
		{
			get
			{
				return this.settings.Drag;
			}
		}

		// Token: 0x170006D0 RID: 1744
		// (get) Token: 0x06003D8F RID: 15759 RVA: 0x0012AC74 File Offset: 0x00129074
		public float InvDrag
		{
			get
			{
				float num = this.settings.Drag * (1f / (float)this.Iterations);
				return 1f - num;
			}
		}

		// Token: 0x170006D1 RID: 1745
		// (get) Token: 0x06003D90 RID: 15760 RVA: 0x0012ACA2 File Offset: 0x001290A2
		public bool IntegrateEnabled
		{
			get
			{
				return this.settings.IntegrateEnabled;
			}
		}

		// Token: 0x170006D2 RID: 1746
		// (get) Token: 0x06003D91 RID: 15761 RVA: 0x0012ACAF File Offset: 0x001290AF
		public Vector3 Gravity
		{
			get
			{
				return Physics.gravity * this.settings.GravityMultiplier;
			}
		}

		// Token: 0x170006D3 RID: 1747
		// (get) Token: 0x06003D92 RID: 15762 RVA: 0x0012ACC6 File Offset: 0x001290C6
		public float GravityMultiplier
		{
			get
			{
				return this.settings.GravityMultiplier;
			}
		}

		// Token: 0x170006D4 RID: 1748
		// (get) Token: 0x06003D93 RID: 15763 RVA: 0x0012ACD3 File Offset: 0x001290D3
		public Vector3 Wind
		{
			get
			{
				return this.settings.Runtime.Wind;
			}
		}

		// Token: 0x170006D5 RID: 1749
		// (get) Token: 0x06003D94 RID: 15764 RVA: 0x0012ACE5 File Offset: 0x001290E5
		public float Stretchability
		{
			get
			{
				return this.settings.Stretchability;
			}
		}

		// Token: 0x170006D6 RID: 1750
		// (get) Token: 0x06003D95 RID: 15765 RVA: 0x0012ACF2 File Offset: 0x001290F2
		public float Stiffness
		{
			get
			{
				return this.settings.Stiffness;
			}
		}

		// Token: 0x170006D7 RID: 1751
		// (get) Token: 0x06003D96 RID: 15766 RVA: 0x0012ACFF File Offset: 0x001290FF
		public float CompressionResistance
		{
			get
			{
				return this.settings.CompressionResistance;
			}
		}

		// Token: 0x170006D8 RID: 1752
		// (get) Token: 0x06003D97 RID: 15767 RVA: 0x0012AD0C File Offset: 0x0012910C
		public float WorldScale
		{
			get
			{
				return this.settings.WorldScale;
			}
		}

		// Token: 0x170006D9 RID: 1753
		// (get) Token: 0x06003D98 RID: 15768 RVA: 0x0012AD19 File Offset: 0x00129119
		public float DistanceScale
		{
			get
			{
				return this.settings.DistanceScale;
			}
		}

		// Token: 0x170006DA RID: 1754
		// (get) Token: 0x06003D99 RID: 15769 RVA: 0x0012AD26 File Offset: 0x00129126
		public bool BreakEnabled
		{
			get
			{
				return this.settings.BreakEnabled;
			}
		}

		// Token: 0x170006DB RID: 1755
		// (get) Token: 0x06003D9A RID: 15770 RVA: 0x0012AD33 File Offset: 0x00129133
		public float BreakThreshold
		{
			get
			{
				return this.settings.BreakThreshold;
			}
		}

		// Token: 0x170006DC RID: 1756
		// (get) Token: 0x06003D9B RID: 15771 RVA: 0x0012AD40 File Offset: 0x00129140
		public float JointStrength
		{
			get
			{
				return this.settings.JointStrength;
			}
		}

		// Token: 0x170006DD RID: 1757
		// (get) Token: 0x06003D9C RID: 15772 RVA: 0x0012AD4D File Offset: 0x0012914D
		public float Weight
		{
			get
			{
				return this.settings.Weight;
			}
		}

		// Token: 0x170006DE RID: 1758
		// (get) Token: 0x06003D9D RID: 15773 RVA: 0x0012AD5A File Offset: 0x0012915A
		public float Friction
		{
			get
			{
				return this.settings.Friction;
			}
		}

		// Token: 0x170006DF RID: 1759
		// (get) Token: 0x06003D9E RID: 15774 RVA: 0x0012AD67 File Offset: 0x00129167
		public float StaticMultiplier
		{
			get
			{
				return this.settings.StaticMultiplier;
			}
		}

		// Token: 0x170006E0 RID: 1760
		// (get) Token: 0x06003D9F RID: 15775 RVA: 0x0012AD74 File Offset: 0x00129174
		public bool CollisionEnabled
		{
			get
			{
				return this.settings.CollisionEnabled;
			}
		}

		// Token: 0x170006E1 RID: 1761
		// (get) Token: 0x06003DA0 RID: 15776 RVA: 0x0012AD81 File Offset: 0x00129181
		public float CollisionPower
		{
			get
			{
				return this.settings.CollisionPower;
			}
		}

		// Token: 0x170006E2 RID: 1762
		// (get) Token: 0x06003DA1 RID: 15777 RVA: 0x0012AD8E File Offset: 0x0012918E
		public GpuBuffer<Matrix4x4> MatricesBuffer
		{
			get
			{
				return this.settings.MeshProvider.ToWorldMatricesBuffer;
			}
		}

		// Token: 0x170006E3 RID: 1763
		// (get) Token: 0x06003DA2 RID: 15778 RVA: 0x0012ADA0 File Offset: 0x001291A0
		public GpuBuffer<Vector3> PreCalculatedVerticesBuffer
		{
			get
			{
				return this.settings.MeshProvider.PreCalculatedVerticesBuffer;
			}
		}

		// Token: 0x170006E4 RID: 1764
		// (get) Token: 0x06003DA3 RID: 15779 RVA: 0x0012ADB2 File Offset: 0x001291B2
		public GpuBuffer<GPParticle> Particles
		{
			get
			{
				return this.settings.Runtime.Particles;
			}
		}

		// Token: 0x170006E5 RID: 1765
		// (get) Token: 0x06003DA4 RID: 15780 RVA: 0x0012ADC4 File Offset: 0x001291C4
		public GpuBuffer<GPSphereWithDelta> ProcessedSpheres
		{
			get
			{
				return this.settings.Runtime.ProcessedSpheres;
			}
		}

		// Token: 0x170006E6 RID: 1766
		// (get) Token: 0x06003DA5 RID: 15781 RVA: 0x0012ADD6 File Offset: 0x001291D6
		public GpuBuffer<GPLineSphereWithDelta> ProcessedLineSpheres
		{
			get
			{
				return this.settings.Runtime.ProcessedLineSpheres;
			}
		}

		// Token: 0x170006E7 RID: 1767
		// (get) Token: 0x06003DA6 RID: 15782 RVA: 0x0012ADE8 File Offset: 0x001291E8
		public GpuBuffer<Vector4> Planes
		{
			get
			{
				return this.settings.Runtime.Planes;
			}
		}

		// Token: 0x170006E8 RID: 1768
		// (get) Token: 0x06003DA7 RID: 15783 RVA: 0x0012ADFA File Offset: 0x001291FA
		public GpuBuffer<GPGrabSphere> GrabSpheres
		{
			get
			{
				return this.settings.Runtime.GrabSpheres;
			}
		}

		// Token: 0x170006E9 RID: 1769
		// (get) Token: 0x06003DA8 RID: 15784 RVA: 0x0012AE0C File Offset: 0x0012920C
		public GroupedData<GPDistanceJoint> DistanceJoints
		{
			get
			{
				return this.settings.Runtime.DistanceJoints;
			}
		}

		// Token: 0x170006EA RID: 1770
		// (get) Token: 0x06003DA9 RID: 15785 RVA: 0x0012AE1E File Offset: 0x0012921E
		public GpuBuffer<GPDistanceJoint> DistanceJointsBuffer
		{
			get
			{
				return this.settings.Runtime.DistanceJointsBuffer;
			}
		}

		// Token: 0x170006EB RID: 1771
		// (get) Token: 0x06003DAA RID: 15786 RVA: 0x0012AE30 File Offset: 0x00129230
		public GroupedData<GPDistanceJoint> StiffnessJoints
		{
			get
			{
				return this.settings.Runtime.StiffnessJoints;
			}
		}

		// Token: 0x170006EC RID: 1772
		// (get) Token: 0x06003DAB RID: 15787 RVA: 0x0012AE42 File Offset: 0x00129242
		public GpuBuffer<GPDistanceJoint> StiffnessJointsBuffer
		{
			get
			{
				return this.settings.Runtime.StiffnessJointsBuffer;
			}
		}

		// Token: 0x170006ED RID: 1773
		// (get) Token: 0x06003DAC RID: 15788 RVA: 0x0012AE54 File Offset: 0x00129254
		public GroupedData<GPDistanceJoint> NearbyJoints
		{
			get
			{
				return this.settings.Runtime.NearbyJoints;
			}
		}

		// Token: 0x170006EE RID: 1774
		// (get) Token: 0x06003DAD RID: 15789 RVA: 0x0012AE66 File Offset: 0x00129266
		public GpuBuffer<GPDistanceJoint> NearbyJointsBuffer
		{
			get
			{
				return this.settings.Runtime.NearbyJointsBuffer;
			}
		}

		// Token: 0x170006EF RID: 1775
		// (get) Token: 0x06003DAE RID: 15790 RVA: 0x0012AE78 File Offset: 0x00129278
		public GpuBuffer<GPPointJoint> PointJoints
		{
			get
			{
				return this.settings.Runtime.PointJoints;
			}
		}

		// Token: 0x170006F0 RID: 1776
		// (get) Token: 0x06003DAF RID: 15791 RVA: 0x0012AE8A File Offset: 0x0012928A
		public GpuBuffer<GPPointJoint> AllPointJoints
		{
			get
			{
				return this.settings.Runtime.AllPointJoints;
			}
		}

		// Token: 0x170006F1 RID: 1777
		// (get) Token: 0x06003DB0 RID: 15792 RVA: 0x0012AE9C File Offset: 0x0012929C
		public GpuBuffer<ClothVertex> ClothVertices
		{
			get
			{
				return this.settings.Runtime.ClothVertices;
			}
		}

		// Token: 0x170006F2 RID: 1778
		// (get) Token: 0x06003DB1 RID: 15793 RVA: 0x0012AEAE File Offset: 0x001292AE
		public GpuBuffer<Vector3> ClothOnlyVertices
		{
			get
			{
				return this.settings.Runtime.ClothOnlyVertices;
			}
		}

		// Token: 0x170006F3 RID: 1779
		// (get) Token: 0x06003DB2 RID: 15794 RVA: 0x0012AEC0 File Offset: 0x001292C0
		public GpuBuffer<int> MeshVertexToNeiborsMap
		{
			get
			{
				return this.settings.Runtime.MeshVertexToNeiborsMap;
			}
		}

		// Token: 0x170006F4 RID: 1780
		// (get) Token: 0x06003DB3 RID: 15795 RVA: 0x0012AED2 File Offset: 0x001292D2
		public GpuBuffer<int> MeshVertexToNeiborsMapCounts
		{
			get
			{
				return this.settings.Runtime.MeshVertexToNeiborsMapCounts;
			}
		}

		// Token: 0x170006F5 RID: 1781
		// (get) Token: 0x06003DB4 RID: 15796 RVA: 0x0012AEE4 File Offset: 0x001292E4
		public GpuBuffer<int> MeshToPhysicsVerticesMap
		{
			get
			{
				return this.settings.Runtime.MeshToPhysicsVerticesMap;
			}
		}

		// Token: 0x170006F6 RID: 1782
		// (get) Token: 0x06003DB5 RID: 15797 RVA: 0x0012AEF6 File Offset: 0x001292F6
		public GpuBuffer<GPParticle> OutParticles
		{
			get
			{
				return this.settings.Runtime.OutParticles;
			}
		}

		// Token: 0x170006F7 RID: 1783
		// (get) Token: 0x06003DB6 RID: 15798 RVA: 0x0012AF08 File Offset: 0x00129308
		public GpuBuffer<float> OutParticlesMap
		{
			get
			{
				return this.settings.Runtime.OutParticlesMap;
			}
		}

		// Token: 0x170006F8 RID: 1784
		// (get) Token: 0x06003DB7 RID: 15799 RVA: 0x0012AF1A File Offset: 0x0012931A
		public Material Material
		{
			get
			{
				return this.settings.Material;
			}
		}

		// Token: 0x170006F9 RID: 1785
		// (get) Token: 0x06003DB8 RID: 15800 RVA: 0x0012AF27 File Offset: 0x00129327
		public Material[] Materials
		{
			get
			{
				return this.settings.Materials;
			}
		}

		// Token: 0x170006FA RID: 1786
		// (get) Token: 0x06003DB9 RID: 15801 RVA: 0x0012AF34 File Offset: 0x00129334
		public Bounds Bounds
		{
			get
			{
				return this.settings.Bounds;
			}
		}

		// Token: 0x170006FB RID: 1787
		// (get) Token: 0x06003DBA RID: 15802 RVA: 0x0012AF41 File Offset: 0x00129341
		public bool CustomBounds
		{
			get
			{
				return this.settings.CustomBounds;
			}
		}

		// Token: 0x04002F3F RID: 12095
		private readonly ClothSettings settings;
	}
}
