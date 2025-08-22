using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using GPUTools.Common.Scripts.PL.Abstract;
using GPUTools.Common.Scripts.PL.Attributes;
using GPUTools.Common.Scripts.PL.Tools;
using GPUTools.Physics.Scripts.Types.Shapes;
using UnityEngine;

namespace GPUTools.Physics.Scripts.Kernels
{
	// Token: 0x02000A6B RID: 2667
	public class ParticleSphereProcessKernel : KernelBase
	{
		// Token: 0x060044F6 RID: 17654 RVA: 0x0013E879 File Offset: 0x0013CC79
		public ParticleSphereProcessKernel() : base("Compute/ParticleSphereProcess", "CSParticleSphereProcess")
		{
		}

		// Token: 0x17000927 RID: 2343
		// (get) Token: 0x060044F8 RID: 17656 RVA: 0x0013E894 File Offset: 0x0013CC94
		// (set) Token: 0x060044F7 RID: 17655 RVA: 0x0013E88B File Offset: 0x0013CC8B
		[GpuData("collisionPrediction")]
		public GpuValue<float> CollisionPrediction
		{
			[CompilerGenerated]
			get
			{
				return this.<CollisionPrediction>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<CollisionPrediction>k__BackingField = value;
			}
		}

		// Token: 0x17000928 RID: 2344
		// (get) Token: 0x060044FA RID: 17658 RVA: 0x0013E8A5 File Offset: 0x0013CCA5
		// (set) Token: 0x060044F9 RID: 17657 RVA: 0x0013E89C File Offset: 0x0013CC9C
		[GpuData("spheres")]
		public GpuBuffer<GPSphere> Spheres
		{
			[CompilerGenerated]
			get
			{
				return this.<Spheres>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<Spheres>k__BackingField = value;
			}
		}

		// Token: 0x17000929 RID: 2345
		// (get) Token: 0x060044FC RID: 17660 RVA: 0x0013E8B6 File Offset: 0x0013CCB6
		// (set) Token: 0x060044FB RID: 17659 RVA: 0x0013E8AD File Offset: 0x0013CCAD
		[GpuData("oldSpheres")]
		public GpuBuffer<GPSphere> OldSpheres
		{
			[CompilerGenerated]
			get
			{
				return this.<OldSpheres>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<OldSpheres>k__BackingField = value;
			}
		}

		// Token: 0x1700092A RID: 2346
		// (get) Token: 0x060044FE RID: 17662 RVA: 0x0013E8C7 File Offset: 0x0013CCC7
		// (set) Token: 0x060044FD RID: 17661 RVA: 0x0013E8BE File Offset: 0x0013CCBE
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

		// Token: 0x060044FF RID: 17663 RVA: 0x0013E8CF File Offset: 0x0013CCCF
		public override int GetGroupsNumX()
		{
			if (this.Spheres != null)
			{
				return Mathf.CeilToInt((float)this.Spheres.Count / 256f);
			}
			return 0;
		}

		// Token: 0x04003300 RID: 13056
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private GpuValue<float> <CollisionPrediction>k__BackingField;

		// Token: 0x04003301 RID: 13057
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private GpuBuffer<GPSphere> <Spheres>k__BackingField;

		// Token: 0x04003302 RID: 13058
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private GpuBuffer<GPSphere> <OldSpheres>k__BackingField;

		// Token: 0x04003303 RID: 13059
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private GpuBuffer<GPSphereWithDelta> <ProcessedSpheres>k__BackingField;
	}
}
