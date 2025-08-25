using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using GPUTools.Common.Scripts.PL.Abstract;
using GPUTools.Common.Scripts.PL.Attributes;
using GPUTools.Common.Scripts.PL.Tools;
using GPUTools.Physics.Scripts.Kernels;
using GPUTools.Physics.Scripts.Types.Dynamic;
using UnityEngine;

namespace GPUTools.ClothDemo.Scripts
{
	// Token: 0x020009E2 RID: 2530
	public class TestPrimitive : PrimitiveBase
	{
		// Token: 0x06003FBA RID: 16314 RVA: 0x0013033A File Offset: 0x0012E73A
		public TestPrimitive()
		{
			base.AddPass(new IntegrateKernel());
		}

		// Token: 0x17000772 RID: 1906
		// (get) Token: 0x06003FBC RID: 16316 RVA: 0x00130356 File Offset: 0x0012E756
		// (set) Token: 0x06003FBB RID: 16315 RVA: 0x0013034D File Offset: 0x0012E74D
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

		// Token: 0x17000773 RID: 1907
		// (get) Token: 0x06003FBE RID: 16318 RVA: 0x00130367 File Offset: 0x0012E767
		// (set) Token: 0x06003FBD RID: 16317 RVA: 0x0013035E File Offset: 0x0012E75E
		[GpuData("gravity")]
		public GpuValue<Vector3> Gravity
		{
			[CompilerGenerated]
			get
			{
				return this.<Gravity>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<Gravity>k__BackingField = value;
			}
		}

		// Token: 0x17000774 RID: 1908
		// (get) Token: 0x06003FC0 RID: 16320 RVA: 0x00130378 File Offset: 0x0012E778
		// (set) Token: 0x06003FBF RID: 16319 RVA: 0x0013036F File Offset: 0x0012E76F
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

		// Token: 0x17000775 RID: 1909
		// (get) Token: 0x06003FC2 RID: 16322 RVA: 0x00130389 File Offset: 0x0012E789
		// (set) Token: 0x06003FC1 RID: 16321 RVA: 0x00130380 File Offset: 0x0012E780
		[GpuData("dt")]
		public GpuValue<float> Dt
		{
			[CompilerGenerated]
			get
			{
				return this.<Dt>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<Dt>k__BackingField = value;
			}
		}

		// Token: 0x17000776 RID: 1910
		// (get) Token: 0x06003FC4 RID: 16324 RVA: 0x0013039A File Offset: 0x0012E79A
		// (set) Token: 0x06003FC3 RID: 16323 RVA: 0x00130391 File Offset: 0x0012E791
		[GpuData("wind")]
		public GpuValue<Vector3> Wind
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

		// Token: 0x06003FC5 RID: 16325 RVA: 0x001303A2 File Offset: 0x0012E7A2
		public void Start()
		{
			base.Bind();
		}

		// Token: 0x0400302D RID: 12333
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private GpuBuffer<GPParticle> <Particles>k__BackingField;

		// Token: 0x0400302E RID: 12334
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private GpuValue<Vector3> <Gravity>k__BackingField;

		// Token: 0x0400302F RID: 12335
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private GpuValue<float> <InvDrag>k__BackingField;

		// Token: 0x04003030 RID: 12336
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private GpuValue<float> <Dt>k__BackingField;

		// Token: 0x04003031 RID: 12337
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private GpuValue<Vector3> <Wind>k__BackingField;
	}
}
