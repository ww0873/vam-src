using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using GPUTools.Common.Scripts.PL.Abstract;
using GPUTools.Common.Scripts.PL.Attributes;
using GPUTools.Common.Scripts.PL.Tools;
using UnityEngine;

namespace GPUTools.Common.Scripts.Tools.Kernels
{
	// Token: 0x020009D0 RID: 2512
	public class GPUMatrixPointMultiplier : KernelBase
	{
		// Token: 0x06003F62 RID: 16226 RVA: 0x0012F06E File Offset: 0x0012D46E
		public GPUMatrixPointMultiplier() : base("Compute/MatrixPointMultiplier", "CSMatrixPointMultiplier")
		{
		}

		// Token: 0x17000765 RID: 1893
		// (get) Token: 0x06003F63 RID: 16227 RVA: 0x0012F080 File Offset: 0x0012D480
		// (set) Token: 0x06003F64 RID: 16228 RVA: 0x0012F088 File Offset: 0x0012D488
		[GpuData("matrices")]
		public GpuBuffer<Matrix4x4> Matrices
		{
			[CompilerGenerated]
			get
			{
				return this.<Matrices>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<Matrices>k__BackingField = value;
			}
		}

		// Token: 0x17000766 RID: 1894
		// (get) Token: 0x06003F65 RID: 16229 RVA: 0x0012F091 File Offset: 0x0012D491
		// (set) Token: 0x06003F66 RID: 16230 RVA: 0x0012F099 File Offset: 0x0012D499
		[GpuData("inPoints")]
		public GpuBuffer<Vector3> InPoints
		{
			[CompilerGenerated]
			get
			{
				return this.<InPoints>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<InPoints>k__BackingField = value;
			}
		}

		// Token: 0x17000767 RID: 1895
		// (get) Token: 0x06003F67 RID: 16231 RVA: 0x0012F0A2 File Offset: 0x0012D4A2
		// (set) Token: 0x06003F68 RID: 16232 RVA: 0x0012F0AA File Offset: 0x0012D4AA
		[GpuData("outPoints")]
		public GpuBuffer<Vector3> OutPoints
		{
			[CompilerGenerated]
			get
			{
				return this.<OutPoints>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<OutPoints>k__BackingField = value;
			}
		}

		// Token: 0x06003F69 RID: 16233 RVA: 0x0012F0B3 File Offset: 0x0012D4B3
		public override int GetGroupsNumX()
		{
			return Mathf.CeilToInt((float)this.Matrices.Count / 256f);
		}

		// Token: 0x04003007 RID: 12295
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private GpuBuffer<Matrix4x4> <Matrices>k__BackingField;

		// Token: 0x04003008 RID: 12296
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private GpuBuffer<Vector3> <InPoints>k__BackingField;

		// Token: 0x04003009 RID: 12297
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private GpuBuffer<Vector3> <OutPoints>k__BackingField;
	}
}
