using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using GPUTools.Common.Scripts.PL.Abstract;
using GPUTools.Common.Scripts.PL.Tools;
using GPUTools.Common.Scripts.Tools.Commands;
using GPUTools.Common.Scripts.Tools.Kernels;
using GPUTools.Skinner.Scripts.Providers;
using UnityEngine;

namespace GPUTools.Skinner.Scripts.Commands
{
	// Token: 0x02000A89 RID: 2697
	public class SkinnerCommand : IBuildCommand
	{
		// Token: 0x060045DC RID: 17884 RVA: 0x0013FDA3 File Offset: 0x0013E1A3
		public SkinnerCommand(SkinnedMeshProvider provider, int[] indices)
		{
			this.provider = provider;
			this.indices = indices;
		}

		// Token: 0x17000975 RID: 2421
		// (get) Token: 0x060045DD RID: 17885 RVA: 0x0013FDC4 File Offset: 0x0013E1C4
		// (set) Token: 0x060045DE RID: 17886 RVA: 0x0013FDCC File Offset: 0x0013E1CC
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

		// Token: 0x17000976 RID: 2422
		// (get) Token: 0x060045DF RID: 17887 RVA: 0x0013FDD5 File Offset: 0x0013E1D5
		// (set) Token: 0x060045E0 RID: 17888 RVA: 0x0013FDDD File Offset: 0x0013E1DD
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

		// Token: 0x17000977 RID: 2423
		// (get) Token: 0x060045E1 RID: 17889 RVA: 0x0013FDE6 File Offset: 0x0013E1E6
		// (set) Token: 0x060045E2 RID: 17890 RVA: 0x0013FDEE File Offset: 0x0013E1EE
		public GpuBuffer<Vector3> LocalPoints
		{
			[CompilerGenerated]
			get
			{
				return this.<LocalPoints>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<LocalPoints>k__BackingField = value;
			}
		}

		// Token: 0x17000978 RID: 2424
		// (get) Token: 0x060045E3 RID: 17891 RVA: 0x0013FDF7 File Offset: 0x0013E1F7
		// (set) Token: 0x060045E4 RID: 17892 RVA: 0x0013FDFF File Offset: 0x0013E1FF
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

		// Token: 0x17000979 RID: 2425
		// (get) Token: 0x060045E5 RID: 17893 RVA: 0x0013FE08 File Offset: 0x0013E208
		// (set) Token: 0x060045E6 RID: 17894 RVA: 0x0013FE10 File Offset: 0x0013E210
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

		// Token: 0x1700097A RID: 2426
		// (get) Token: 0x060045E7 RID: 17895 RVA: 0x0013FE19 File Offset: 0x0013E219
		// (set) Token: 0x060045E8 RID: 17896 RVA: 0x0013FE21 File Offset: 0x0013E221
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

		// Token: 0x060045E9 RID: 17897 RVA: 0x0013FE2C File Offset: 0x0013E22C
		public void Build()
		{
			this.Matrices = this.provider.ToWorldMatricesBuffer;
			this.LocalPoints = new GpuBuffer<Vector3>(this.provider.Mesh.vertices, 12);
			this.Points = new GpuBuffer<Vector3>(this.provider.Mesh.vertexCount, 12);
			GPUMatrixPointMultiplier item = new GPUMatrixPointMultiplier
			{
				InPoints = this.LocalPoints,
				OutPoints = this.Points,
				Matrices = this.Matrices
			};
			this.kernels.Add(item);
			if (this.indices != null && this.indices.Length > 0)
			{
				this.Indices = new GpuBuffer<int>(this.indices, 4);
				this.SelectedPoints = new GpuBuffer<Vector3>(this.indices.Length, 12);
				this.SelectedMatrices = new GpuBuffer<Matrix4x4>(this.indices.Length, 64);
				GPUMatrixSelector item2 = new GPUMatrixSelector
				{
					Indices = this.Indices,
					Matrices = this.Matrices,
					SelectedMatrices = this.SelectedMatrices
				};
				GPUPointsSelector item3 = new GPUPointsSelector
				{
					Indices = this.Indices,
					Points = this.Points,
					SelectedPoints = this.SelectedPoints
				};
				this.kernels.Add(item2);
				this.kernels.Add(item3);
			}
		}

		// Token: 0x060045EA RID: 17898 RVA: 0x0013FF88 File Offset: 0x0013E388
		public void Dispatch()
		{
			for (int i = 0; i < this.kernels.Count; i++)
			{
				this.kernels[i].Dispatch();
			}
		}

		// Token: 0x060045EB RID: 17899 RVA: 0x0013FFC2 File Offset: 0x0013E3C2
		public void FixedDispatch()
		{
		}

		// Token: 0x060045EC RID: 17900 RVA: 0x0013FFC4 File Offset: 0x0013E3C4
		public void UpdateSettings()
		{
		}

		// Token: 0x060045ED RID: 17901 RVA: 0x0013FFC8 File Offset: 0x0013E3C8
		public void Dispose()
		{
			if (this.Indices != null)
			{
				this.Indices.Dispose();
			}
			if (this.SelectedPoints != null)
			{
				this.SelectedPoints.Dispose();
			}
			if (this.SelectedMatrices != null)
			{
				this.SelectedMatrices.Dispose();
			}
			this.LocalPoints.Dispose();
			this.Points.Dispose();
		}

		// Token: 0x0400338B RID: 13195
		private readonly SkinnedMeshProvider provider;

		// Token: 0x0400338C RID: 13196
		private readonly int[] indices;

		// Token: 0x0400338D RID: 13197
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private GpuBuffer<int> <Indices>k__BackingField;

		// Token: 0x0400338E RID: 13198
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private GpuBuffer<Matrix4x4> <Matrices>k__BackingField;

		// Token: 0x0400338F RID: 13199
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private GpuBuffer<Vector3> <LocalPoints>k__BackingField;

		// Token: 0x04003390 RID: 13200
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private GpuBuffer<Vector3> <Points>k__BackingField;

		// Token: 0x04003391 RID: 13201
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private GpuBuffer<Matrix4x4> <SelectedMatrices>k__BackingField;

		// Token: 0x04003392 RID: 13202
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private GpuBuffer<Vector3> <SelectedPoints>k__BackingField;

		// Token: 0x04003393 RID: 13203
		private List<KernelBase> kernels = new List<KernelBase>();
	}
}
