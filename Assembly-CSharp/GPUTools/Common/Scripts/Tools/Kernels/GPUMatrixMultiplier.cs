using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using GPUTools.Common.Scripts.PL.Abstract;
using GPUTools.Common.Scripts.PL.Attributes;
using GPUTools.Common.Scripts.PL.Tools;
using UnityEngine;

namespace GPUTools.Common.Scripts.Tools.Kernels
{
	// Token: 0x020009CF RID: 2511
	public class GPUMatrixMultiplier : KernelBase
	{
		// Token: 0x06003F59 RID: 16217 RVA: 0x0012EFE2 File Offset: 0x0012D3E2
		public GPUMatrixMultiplier(GpuBuffer<Matrix4x4> matrices1, GpuBuffer<Matrix4x4> matrices2) : base("Compute/MatrixMultiplier", "CSMatrixMultiplier")
		{
			this.Matrices1 = matrices1;
			this.Matrices2 = matrices2;
			this.ResultMatrices = new GpuBuffer<Matrix4x4>(matrices1.Count, 64);
		}

		// Token: 0x17000762 RID: 1890
		// (get) Token: 0x06003F5A RID: 16218 RVA: 0x0012F015 File Offset: 0x0012D415
		// (set) Token: 0x06003F5B RID: 16219 RVA: 0x0012F01D File Offset: 0x0012D41D
		[GpuData("matrices1")]
		public GpuBuffer<Matrix4x4> Matrices1
		{
			[CompilerGenerated]
			get
			{
				return this.<Matrices1>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<Matrices1>k__BackingField = value;
			}
		}

		// Token: 0x17000763 RID: 1891
		// (get) Token: 0x06003F5C RID: 16220 RVA: 0x0012F026 File Offset: 0x0012D426
		// (set) Token: 0x06003F5D RID: 16221 RVA: 0x0012F02E File Offset: 0x0012D42E
		[GpuData("matrices2")]
		public GpuBuffer<Matrix4x4> Matrices2
		{
			[CompilerGenerated]
			get
			{
				return this.<Matrices2>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<Matrices2>k__BackingField = value;
			}
		}

		// Token: 0x17000764 RID: 1892
		// (get) Token: 0x06003F5E RID: 16222 RVA: 0x0012F037 File Offset: 0x0012D437
		// (set) Token: 0x06003F5F RID: 16223 RVA: 0x0012F03F File Offset: 0x0012D43F
		[GpuData("resultMatrices")]
		public GpuBuffer<Matrix4x4> ResultMatrices
		{
			[CompilerGenerated]
			get
			{
				return this.<ResultMatrices>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<ResultMatrices>k__BackingField = value;
			}
		}

		// Token: 0x06003F60 RID: 16224 RVA: 0x0012F048 File Offset: 0x0012D448
		public override int GetGroupsNumX()
		{
			return Mathf.CeilToInt((float)this.Matrices1.Count / 256f);
		}

		// Token: 0x06003F61 RID: 16225 RVA: 0x0012F061 File Offset: 0x0012D461
		public override void Dispose()
		{
			this.ResultMatrices.Dispose();
		}

		// Token: 0x04003004 RID: 12292
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private GpuBuffer<Matrix4x4> <Matrices1>k__BackingField;

		// Token: 0x04003005 RID: 12293
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private GpuBuffer<Matrix4x4> <Matrices2>k__BackingField;

		// Token: 0x04003006 RID: 12294
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private GpuBuffer<Matrix4x4> <ResultMatrices>k__BackingField;
	}
}
