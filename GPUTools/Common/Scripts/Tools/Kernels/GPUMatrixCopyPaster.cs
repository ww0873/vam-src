using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using GPUTools.Common.Scripts.PL.Abstract;
using GPUTools.Common.Scripts.PL.Attributes;
using GPUTools.Common.Scripts.PL.Tools;
using UnityEngine;

namespace GPUTools.Common.Scripts.Tools.Kernels
{
	// Token: 0x020009CE RID: 2510
	public class GPUMatrixCopyPaster : KernelBase
	{
		// Token: 0x06003F53 RID: 16211 RVA: 0x0012EF87 File Offset: 0x0012D387
		public GPUMatrixCopyPaster(GpuBuffer<Matrix4x4> matricesFrom, GpuBuffer<Matrix4x4> matricesTo) : base("Compute/MatrixCopyPaster", "CSMatrixCopyPaster")
		{
			this.MatricesFrom = matricesFrom;
			this.MatricesTo = matricesTo;
		}

		// Token: 0x17000760 RID: 1888
		// (get) Token: 0x06003F54 RID: 16212 RVA: 0x0012EFA7 File Offset: 0x0012D3A7
		// (set) Token: 0x06003F55 RID: 16213 RVA: 0x0012EFAF File Offset: 0x0012D3AF
		[GpuData("matricesFrom")]
		public GpuBuffer<Matrix4x4> MatricesFrom
		{
			[CompilerGenerated]
			get
			{
				return this.<MatricesFrom>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<MatricesFrom>k__BackingField = value;
			}
		}

		// Token: 0x17000761 RID: 1889
		// (get) Token: 0x06003F56 RID: 16214 RVA: 0x0012EFB8 File Offset: 0x0012D3B8
		// (set) Token: 0x06003F57 RID: 16215 RVA: 0x0012EFC0 File Offset: 0x0012D3C0
		[GpuData("matricesTo")]
		public GpuBuffer<Matrix4x4> MatricesTo
		{
			[CompilerGenerated]
			get
			{
				return this.<MatricesTo>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<MatricesTo>k__BackingField = value;
			}
		}

		// Token: 0x06003F58 RID: 16216 RVA: 0x0012EFC9 File Offset: 0x0012D3C9
		public override int GetGroupsNumX()
		{
			return Mathf.CeilToInt((float)this.MatricesFrom.Count / 256f);
		}

		// Token: 0x04003002 RID: 12290
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private GpuBuffer<Matrix4x4> <MatricesFrom>k__BackingField;

		// Token: 0x04003003 RID: 12291
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private GpuBuffer<Matrix4x4> <MatricesTo>k__BackingField;
	}
}
