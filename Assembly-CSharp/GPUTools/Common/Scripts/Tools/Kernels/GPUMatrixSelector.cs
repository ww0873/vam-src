using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using GPUTools.Common.Scripts.PL.Abstract;
using GPUTools.Common.Scripts.PL.Attributes;
using GPUTools.Common.Scripts.PL.Tools;
using UnityEngine;

namespace GPUTools.Common.Scripts.Tools.Kernels
{
	// Token: 0x020009D1 RID: 2513
	public class GPUMatrixSelector : KernelBase
	{
		// Token: 0x06003F6A RID: 16234 RVA: 0x0012F0CC File Offset: 0x0012D4CC
		public GPUMatrixSelector() : base("Compute/Selector", "CSMatrixSelector")
		{
		}

		// Token: 0x17000768 RID: 1896
		// (get) Token: 0x06003F6B RID: 16235 RVA: 0x0012F0DE File Offset: 0x0012D4DE
		// (set) Token: 0x06003F6C RID: 16236 RVA: 0x0012F0E6 File Offset: 0x0012D4E6
		[GpuData("indices")]
		public GpuBuffer<int> Indices
		{
			[CompilerGenerated]
			get
			{
				return this.<Indices>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<Indices>k__BackingField = value;
			}
		}

		// Token: 0x17000769 RID: 1897
		// (get) Token: 0x06003F6D RID: 16237 RVA: 0x0012F0EF File Offset: 0x0012D4EF
		// (set) Token: 0x06003F6E RID: 16238 RVA: 0x0012F0F7 File Offset: 0x0012D4F7
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

		// Token: 0x1700076A RID: 1898
		// (get) Token: 0x06003F6F RID: 16239 RVA: 0x0012F100 File Offset: 0x0012D500
		// (set) Token: 0x06003F70 RID: 16240 RVA: 0x0012F108 File Offset: 0x0012D508
		[GpuData("selectedMatrices")]
		public GpuBuffer<Matrix4x4> SelectedMatrices
		{
			[CompilerGenerated]
			get
			{
				return this.<SelectedMatrices>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<SelectedMatrices>k__BackingField = value;
			}
		}

		// Token: 0x06003F71 RID: 16241 RVA: 0x0012F111 File Offset: 0x0012D511
		public override int GetGroupsNumX()
		{
			return Mathf.CeilToInt((float)this.Indices.Count / 256f);
		}

		// Token: 0x0400300A RID: 12298
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private GpuBuffer<int> <Indices>k__BackingField;

		// Token: 0x0400300B RID: 12299
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private GpuBuffer<Matrix4x4> <Matrices>k__BackingField;

		// Token: 0x0400300C RID: 12300
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private GpuBuffer<Matrix4x4> <SelectedMatrices>k__BackingField;
	}
}
