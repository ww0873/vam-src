using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using GPUTools.Common.Scripts.PL.Abstract;
using GPUTools.Common.Scripts.PL.Attributes;
using GPUTools.Common.Scripts.PL.Tools;
using UnityEngine;

namespace GPUTools.Painter.Scripts.Kernels
{
	// Token: 0x02000A34 RID: 2612
	public class PaintKernel : KernelBase
	{
		// Token: 0x0600435B RID: 17243 RVA: 0x0013C268 File Offset: 0x0013A668
		public PaintKernel() : base("Compute/Paint", "CSPaint")
		{
		}

		// Token: 0x17000897 RID: 2199
		// (get) Token: 0x0600435C RID: 17244 RVA: 0x0013C27A File Offset: 0x0013A67A
		// (set) Token: 0x0600435D RID: 17245 RVA: 0x0013C282 File Offset: 0x0013A682
		[GpuData("vertices")]
		public GpuBuffer<Vector3> Vertices
		{
			[CompilerGenerated]
			get
			{
				return this.<Vertices>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<Vertices>k__BackingField = value;
			}
		}

		// Token: 0x17000898 RID: 2200
		// (get) Token: 0x0600435E RID: 17246 RVA: 0x0013C28B File Offset: 0x0013A68B
		// (set) Token: 0x0600435F RID: 17247 RVA: 0x0013C293 File Offset: 0x0013A693
		[GpuData("normals")]
		public GpuBuffer<Vector3> Normals
		{
			[CompilerGenerated]
			get
			{
				return this.<Normals>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<Normals>k__BackingField = value;
			}
		}

		// Token: 0x17000899 RID: 2201
		// (get) Token: 0x06004360 RID: 17248 RVA: 0x0013C29C File Offset: 0x0013A69C
		// (set) Token: 0x06004361 RID: 17249 RVA: 0x0013C2A4 File Offset: 0x0013A6A4
		[GpuData("colors")]
		public GpuBuffer<Color> Colors
		{
			[CompilerGenerated]
			get
			{
				return this.<Colors>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<Colors>k__BackingField = value;
			}
		}

		// Token: 0x1700089A RID: 2202
		// (get) Token: 0x06004362 RID: 17250 RVA: 0x0013C2AD File Offset: 0x0013A6AD
		// (set) Token: 0x06004363 RID: 17251 RVA: 0x0013C2B5 File Offset: 0x0013A6B5
		[GpuData("rayOrigin")]
		public GpuValue<Vector3> RayOrigin
		{
			[CompilerGenerated]
			get
			{
				return this.<RayOrigin>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<RayOrigin>k__BackingField = value;
			}
		}

		// Token: 0x1700089B RID: 2203
		// (get) Token: 0x06004364 RID: 17252 RVA: 0x0013C2BE File Offset: 0x0013A6BE
		// (set) Token: 0x06004365 RID: 17253 RVA: 0x0013C2C6 File Offset: 0x0013A6C6
		[GpuData("rayDirection")]
		public GpuValue<Vector3> RayDirection
		{
			[CompilerGenerated]
			get
			{
				return this.<RayDirection>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<RayDirection>k__BackingField = value;
			}
		}

		// Token: 0x1700089C RID: 2204
		// (get) Token: 0x06004366 RID: 17254 RVA: 0x0013C2CF File Offset: 0x0013A6CF
		// (set) Token: 0x06004367 RID: 17255 RVA: 0x0013C2D7 File Offset: 0x0013A6D7
		[GpuData("brushColor")]
		public GpuValue<Color> BrushColor
		{
			[CompilerGenerated]
			get
			{
				return this.<BrushColor>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<BrushColor>k__BackingField = value;
			}
		}

		// Token: 0x1700089D RID: 2205
		// (get) Token: 0x06004368 RID: 17256 RVA: 0x0013C2E0 File Offset: 0x0013A6E0
		// (set) Token: 0x06004369 RID: 17257 RVA: 0x0013C2E8 File Offset: 0x0013A6E8
		[GpuData("brushRadius")]
		public GpuValue<float> BrushRadius
		{
			[CompilerGenerated]
			get
			{
				return this.<BrushRadius>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<BrushRadius>k__BackingField = value;
			}
		}

		// Token: 0x1700089E RID: 2206
		// (get) Token: 0x0600436A RID: 17258 RVA: 0x0013C2F1 File Offset: 0x0013A6F1
		// (set) Token: 0x0600436B RID: 17259 RVA: 0x0013C2F9 File Offset: 0x0013A6F9
		[GpuData("brushStrength")]
		public GpuValue<float> BrushStrength
		{
			[CompilerGenerated]
			get
			{
				return this.<BrushStrength>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<BrushStrength>k__BackingField = value;
			}
		}

		// Token: 0x1700089F RID: 2207
		// (get) Token: 0x0600436C RID: 17260 RVA: 0x0013C302 File Offset: 0x0013A702
		// (set) Token: 0x0600436D RID: 17261 RVA: 0x0013C30A File Offset: 0x0013A70A
		[GpuData("channel")]
		public GpuValue<int> Channel
		{
			[CompilerGenerated]
			get
			{
				return this.<Channel>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<Channel>k__BackingField = value;
			}
		}

		// Token: 0x0600436E RID: 17262 RVA: 0x0013C313 File Offset: 0x0013A713
		public override int GetGroupsNumX()
		{
			return Mathf.CeilToInt((float)this.Vertices.Count / 256f);
		}

		// Token: 0x0400325F RID: 12895
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private GpuBuffer<Vector3> <Vertices>k__BackingField;

		// Token: 0x04003260 RID: 12896
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private GpuBuffer<Vector3> <Normals>k__BackingField;

		// Token: 0x04003261 RID: 12897
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private GpuBuffer<Color> <Colors>k__BackingField;

		// Token: 0x04003262 RID: 12898
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private GpuValue<Vector3> <RayOrigin>k__BackingField;

		// Token: 0x04003263 RID: 12899
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private GpuValue<Vector3> <RayDirection>k__BackingField;

		// Token: 0x04003264 RID: 12900
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private GpuValue<Color> <BrushColor>k__BackingField;

		// Token: 0x04003265 RID: 12901
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private GpuValue<float> <BrushRadius>k__BackingField;

		// Token: 0x04003266 RID: 12902
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private GpuValue<float> <BrushStrength>k__BackingField;

		// Token: 0x04003267 RID: 12903
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private GpuValue<int> <Channel>k__BackingField;
	}
}
