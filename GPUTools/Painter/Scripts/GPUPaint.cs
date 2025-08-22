using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using GPUTools.Common.Scripts.PL.Abstract;
using GPUTools.Common.Scripts.PL.Attributes;
using GPUTools.Common.Scripts.PL.Tools;
using GPUTools.Painter.Scripts.Kernels;
using UnityEngine;

namespace GPUTools.Painter.Scripts
{
	// Token: 0x02000A33 RID: 2611
	public class GPUPaint : PrimitiveBase
	{
		// Token: 0x06004346 RID: 17222 RVA: 0x0013C074 File Offset: 0x0013A474
		public GPUPaint(Vector3[] vertices, Vector3[] normals, Color[] colors)
		{
			this.Vertices = new GpuBuffer<Vector3>(vertices, 12);
			this.Normals = new GpuBuffer<Vector3>(normals, 12);
			this.Colors = new GpuBuffer<Color>(colors, 16);
			this.RayOrigin = new GpuValue<Vector3>(default(Vector3));
			this.RayDirection = new GpuValue<Vector3>(default(Vector3));
			this.BrushColor = new GpuValue<Color>(default(Color));
			this.BrushRadius = new GpuValue<float>(0f);
			this.BrushStrength = new GpuValue<float>(0f);
			this.Channel = new GpuValue<int>(0);
			base.AddPass(new PaintKernel());
			base.Bind();
		}

		// Token: 0x1700088E RID: 2190
		// (get) Token: 0x06004347 RID: 17223 RVA: 0x0013C12A File Offset: 0x0013A52A
		// (set) Token: 0x06004348 RID: 17224 RVA: 0x0013C132 File Offset: 0x0013A532
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

		// Token: 0x1700088F RID: 2191
		// (get) Token: 0x06004349 RID: 17225 RVA: 0x0013C13B File Offset: 0x0013A53B
		// (set) Token: 0x0600434A RID: 17226 RVA: 0x0013C143 File Offset: 0x0013A543
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

		// Token: 0x17000890 RID: 2192
		// (get) Token: 0x0600434B RID: 17227 RVA: 0x0013C14C File Offset: 0x0013A54C
		// (set) Token: 0x0600434C RID: 17228 RVA: 0x0013C154 File Offset: 0x0013A554
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

		// Token: 0x17000891 RID: 2193
		// (get) Token: 0x0600434D RID: 17229 RVA: 0x0013C15D File Offset: 0x0013A55D
		// (set) Token: 0x0600434E RID: 17230 RVA: 0x0013C165 File Offset: 0x0013A565
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

		// Token: 0x17000892 RID: 2194
		// (get) Token: 0x0600434F RID: 17231 RVA: 0x0013C16E File Offset: 0x0013A56E
		// (set) Token: 0x06004350 RID: 17232 RVA: 0x0013C176 File Offset: 0x0013A576
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

		// Token: 0x17000893 RID: 2195
		// (get) Token: 0x06004351 RID: 17233 RVA: 0x0013C17F File Offset: 0x0013A57F
		// (set) Token: 0x06004352 RID: 17234 RVA: 0x0013C187 File Offset: 0x0013A587
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

		// Token: 0x17000894 RID: 2196
		// (get) Token: 0x06004353 RID: 17235 RVA: 0x0013C190 File Offset: 0x0013A590
		// (set) Token: 0x06004354 RID: 17236 RVA: 0x0013C198 File Offset: 0x0013A598
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

		// Token: 0x17000895 RID: 2197
		// (get) Token: 0x06004355 RID: 17237 RVA: 0x0013C1A1 File Offset: 0x0013A5A1
		// (set) Token: 0x06004356 RID: 17238 RVA: 0x0013C1A9 File Offset: 0x0013A5A9
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

		// Token: 0x17000896 RID: 2198
		// (get) Token: 0x06004357 RID: 17239 RVA: 0x0013C1B2 File Offset: 0x0013A5B2
		// (set) Token: 0x06004358 RID: 17240 RVA: 0x0013C1BA File Offset: 0x0013A5BA
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

		// Token: 0x06004359 RID: 17241 RVA: 0x0013C1C4 File Offset: 0x0013A5C4
		public void Draw(ColorBrush brush, Ray ray)
		{
			this.RayOrigin.Value = ray.origin;
			this.RayDirection.Value = ray.direction;
			this.BrushColor.Value = brush.Color;
			this.BrushRadius.Value = brush.Radius;
			this.BrushStrength.Value = brush.Strength;
			this.Channel.Value = (int)brush.Channel;
			this.Dispatch();
		}

		// Token: 0x0600435A RID: 17242 RVA: 0x0013C23F File Offset: 0x0013A63F
		public override void Dispose()
		{
			base.Dispose();
			this.Vertices.Dispose();
			this.Normals.Dispose();
			this.Colors.Dispose();
		}

		// Token: 0x04003256 RID: 12886
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private GpuBuffer<Vector3> <Vertices>k__BackingField;

		// Token: 0x04003257 RID: 12887
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private GpuBuffer<Vector3> <Normals>k__BackingField;

		// Token: 0x04003258 RID: 12888
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private GpuBuffer<Color> <Colors>k__BackingField;

		// Token: 0x04003259 RID: 12889
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private GpuValue<Vector3> <RayOrigin>k__BackingField;

		// Token: 0x0400325A RID: 12890
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private GpuValue<Vector3> <RayDirection>k__BackingField;

		// Token: 0x0400325B RID: 12891
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private GpuValue<Color> <BrushColor>k__BackingField;

		// Token: 0x0400325C RID: 12892
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private GpuValue<float> <BrushRadius>k__BackingField;

		// Token: 0x0400325D RID: 12893
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private GpuValue<float> <BrushStrength>k__BackingField;

		// Token: 0x0400325E RID: 12894
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private GpuValue<int> <Channel>k__BackingField;
	}
}
