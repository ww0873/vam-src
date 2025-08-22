using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using GPUTools.Common.Scripts.PL.Abstract;
using GPUTools.Common.Scripts.PL.Attributes;
using GPUTools.Common.Scripts.PL.Tools;
using GPUTools.Physics.Scripts.Types.Dynamic;
using GPUTools.Physics.Scripts.Types.Shapes;
using UnityEngine;

namespace GPUTools.Physics.Scripts.Kernels
{
	// Token: 0x02000A4F RID: 2639
	public class GrabJointsKernel : KernelBase
	{
		// Token: 0x06004404 RID: 17412 RVA: 0x0013DB29 File Offset: 0x0013BF29
		public GrabJointsKernel() : base("Compute/GrabJoints", "CSGrabJoints")
		{
		}

		// Token: 0x170008CA RID: 2250
		// (get) Token: 0x06004406 RID: 17414 RVA: 0x0013DB44 File Offset: 0x0013BF44
		// (set) Token: 0x06004405 RID: 17413 RVA: 0x0013DB3B File Offset: 0x0013BF3B
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

		// Token: 0x170008CB RID: 2251
		// (get) Token: 0x06004408 RID: 17416 RVA: 0x0013DB55 File Offset: 0x0013BF55
		// (set) Token: 0x06004407 RID: 17415 RVA: 0x0013DB4C File Offset: 0x0013BF4C
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

		// Token: 0x06004409 RID: 17417 RVA: 0x0013DB5D File Offset: 0x0013BF5D
		public override int GetGroupsNumX()
		{
			if (this.Particles != null)
			{
				return Mathf.CeilToInt((float)this.Particles.ComputeBuffer.count / 256f);
			}
			return 0;
		}

		// Token: 0x040032A3 RID: 12963
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private GpuBuffer<GPGrabSphere> <GrabSpheres>k__BackingField;

		// Token: 0x040032A4 RID: 12964
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private GpuBuffer<GPParticle> <Particles>k__BackingField;
	}
}
