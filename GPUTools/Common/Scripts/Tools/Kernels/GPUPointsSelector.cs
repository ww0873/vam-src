using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using GPUTools.Common.Scripts.PL.Abstract;
using GPUTools.Common.Scripts.PL.Attributes;
using GPUTools.Common.Scripts.PL.Tools;
using UnityEngine;

namespace GPUTools.Common.Scripts.Tools.Kernels
{
	// Token: 0x020009D2 RID: 2514
	public class GPUPointsSelector : KernelBase
	{
		// Token: 0x06003F72 RID: 16242 RVA: 0x0012F12A File Offset: 0x0012D52A
		public GPUPointsSelector() : base("Compute/Selector", "CSPointsSelector")
		{
		}

		// Token: 0x1700076B RID: 1899
		// (get) Token: 0x06003F73 RID: 16243 RVA: 0x0012F13C File Offset: 0x0012D53C
		// (set) Token: 0x06003F74 RID: 16244 RVA: 0x0012F144 File Offset: 0x0012D544
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

		// Token: 0x1700076C RID: 1900
		// (get) Token: 0x06003F75 RID: 16245 RVA: 0x0012F14D File Offset: 0x0012D54D
		// (set) Token: 0x06003F76 RID: 16246 RVA: 0x0012F155 File Offset: 0x0012D555
		[GpuData("points")]
		public GpuBuffer<Vector3> Points
		{
			[CompilerGenerated]
			get
			{
				return this.<Points>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<Points>k__BackingField = value;
			}
		}

		// Token: 0x1700076D RID: 1901
		// (get) Token: 0x06003F77 RID: 16247 RVA: 0x0012F15E File Offset: 0x0012D55E
		// (set) Token: 0x06003F78 RID: 16248 RVA: 0x0012F166 File Offset: 0x0012D566
		[GpuData("selectedPoints")]
		public GpuBuffer<Vector3> SelectedPoints
		{
			[CompilerGenerated]
			get
			{
				return this.<SelectedPoints>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<SelectedPoints>k__BackingField = value;
			}
		}

		// Token: 0x06003F79 RID: 16249 RVA: 0x0012F16F File Offset: 0x0012D56F
		public override int GetGroupsNumX()
		{
			return Mathf.CeilToInt((float)this.Indices.Count / 256f);
		}

		// Token: 0x0400300D RID: 12301
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private GpuBuffer<int> <Indices>k__BackingField;

		// Token: 0x0400300E RID: 12302
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private GpuBuffer<Vector3> <Points>k__BackingField;

		// Token: 0x0400300F RID: 12303
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private GpuBuffer<Vector3> <SelectedPoints>k__BackingField;
	}
}
