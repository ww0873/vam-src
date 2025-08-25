using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using GPUTools.Common.Scripts.PL.Abstract;
using GPUTools.Common.Scripts.PL.Attributes;
using GPUTools.Common.Scripts.PL.Tools;
using GPUTools.Common.Scripts.Tools;
using GPUTools.Physics.Scripts.Behaviours;
using GPUTools.Physics.Scripts.Types.Shapes;
using UnityEngine;

namespace GPUTools.Hair.Scripts.Geometry.Create.Kernels
{
	// Token: 0x020009F6 RID: 2550
	public class HairEditorKernel : KernelBase
	{
		// Token: 0x0600403E RID: 16446 RVA: 0x00131B9C File Offset: 0x0012FF9C
		public HairEditorKernel(Vector3[] vertices, Color[] colors, float[] distances, HairGeometryCreator creator, string kernelName) : base("Compute/HairEditor", kernelName)
		{
			this.creator = creator;
			this.sphereCollidersCache = new CacheProvider<SphereCollider>(creator.ColliderProviders);
			this.lineSphereCollidersCache = new CacheProvider<LineSphereCollider>(creator.ColliderProviders);
			this.Vertices = new GpuBuffer<Vector3>(vertices, 12);
			this.Distances = new GpuBuffer<float>(distances, 4);
			this.Colors = new GpuBuffer<Color>(colors, 16);
			this.Matrices = new GpuBuffer<Matrix4x4>(new Matrix4x4[3], 64);
			if (this.sphereCollidersCache.Items.Count > 0)
			{
				this.StaticSpheres = new GpuBuffer<GPSphere>(this.sphereCollidersCache.Items.Count, GPSphere.Size());
			}
			else
			{
				this.StaticSpheres = new GpuBuffer<GPSphere>(1, GPSphere.Size());
			}
			if (this.lineSphereCollidersCache.Items.Count > 0)
			{
				this.StaticLineSpheres = new GpuBuffer<GPLineSphere>(this.lineSphereCollidersCache.Items.Count, GPLineSphere.Size());
			}
			else
			{
				this.StaticLineSpheres = new GpuBuffer<GPLineSphere>(1, GPSphere.Size());
			}
			this.Segments = new GpuValue<int>(0);
			this.BrushPosition = new GpuValue<Vector3>(default(Vector3));
			this.BrushRadius = new GpuValue<float>(0f);
			this.BrushLenght1 = new GpuValue<float>(0f);
			this.BrushLenght2 = new GpuValue<float>(0f);
			this.BrushStrength = new GpuValue<float>(0f);
			this.BrushCollisionDistance = new GpuValue<float>(0f);
			this.BrushSpeed = new GpuValue<Vector3>(default(Vector3));
			this.BrushLengthSpeed = new GpuValue<float>(0f);
			this.BrushColor = new GpuValue<Vector3>(default(Vector3));
		}

		// Token: 0x17000781 RID: 1921
		// (get) Token: 0x0600403F RID: 16447 RVA: 0x00131D5E File Offset: 0x0013015E
		// (set) Token: 0x06004040 RID: 16448 RVA: 0x00131D66 File Offset: 0x00130166
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

		// Token: 0x17000782 RID: 1922
		// (get) Token: 0x06004041 RID: 16449 RVA: 0x00131D6F File Offset: 0x0013016F
		// (set) Token: 0x06004042 RID: 16450 RVA: 0x00131D77 File Offset: 0x00130177
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

		// Token: 0x17000783 RID: 1923
		// (get) Token: 0x06004043 RID: 16451 RVA: 0x00131D80 File Offset: 0x00130180
		// (set) Token: 0x06004044 RID: 16452 RVA: 0x00131D88 File Offset: 0x00130188
		[GpuData("distances")]
		public GpuBuffer<float> Distances
		{
			[CompilerGenerated]
			get
			{
				return this.<Distances>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<Distances>k__BackingField = value;
			}
		}

		// Token: 0x17000784 RID: 1924
		// (get) Token: 0x06004045 RID: 16453 RVA: 0x00131D91 File Offset: 0x00130191
		// (set) Token: 0x06004046 RID: 16454 RVA: 0x00131D99 File Offset: 0x00130199
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

		// Token: 0x17000785 RID: 1925
		// (get) Token: 0x06004047 RID: 16455 RVA: 0x00131DA2 File Offset: 0x001301A2
		// (set) Token: 0x06004048 RID: 16456 RVA: 0x00131DAA File Offset: 0x001301AA
		[GpuData("staticSpheres")]
		public GpuBuffer<GPSphere> StaticSpheres
		{
			[CompilerGenerated]
			get
			{
				return this.<StaticSpheres>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<StaticSpheres>k__BackingField = value;
			}
		}

		// Token: 0x17000786 RID: 1926
		// (get) Token: 0x06004049 RID: 16457 RVA: 0x00131DB3 File Offset: 0x001301B3
		// (set) Token: 0x0600404A RID: 16458 RVA: 0x00131DBB File Offset: 0x001301BB
		[GpuData("staticLineSpheres")]
		public GpuBuffer<GPLineSphere> StaticLineSpheres
		{
			[CompilerGenerated]
			get
			{
				return this.<StaticLineSpheres>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<StaticLineSpheres>k__BackingField = value;
			}
		}

		// Token: 0x17000787 RID: 1927
		// (get) Token: 0x0600404B RID: 16459 RVA: 0x00131DC4 File Offset: 0x001301C4
		// (set) Token: 0x0600404C RID: 16460 RVA: 0x00131DCC File Offset: 0x001301CC
		[GpuData("segments")]
		public GpuValue<int> Segments
		{
			[CompilerGenerated]
			get
			{
				return this.<Segments>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<Segments>k__BackingField = value;
			}
		}

		// Token: 0x17000788 RID: 1928
		// (get) Token: 0x0600404D RID: 16461 RVA: 0x00131DD5 File Offset: 0x001301D5
		// (set) Token: 0x0600404E RID: 16462 RVA: 0x00131DDD File Offset: 0x001301DD
		[GpuData("brushPosition")]
		public GpuValue<Vector3> BrushPosition
		{
			[CompilerGenerated]
			get
			{
				return this.<BrushPosition>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<BrushPosition>k__BackingField = value;
			}
		}

		// Token: 0x17000789 RID: 1929
		// (get) Token: 0x0600404F RID: 16463 RVA: 0x00131DE6 File Offset: 0x001301E6
		// (set) Token: 0x06004050 RID: 16464 RVA: 0x00131DEE File Offset: 0x001301EE
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

		// Token: 0x1700078A RID: 1930
		// (get) Token: 0x06004051 RID: 16465 RVA: 0x00131DF7 File Offset: 0x001301F7
		// (set) Token: 0x06004052 RID: 16466 RVA: 0x00131DFF File Offset: 0x001301FF
		[GpuData("brushLenght1")]
		public GpuValue<float> BrushLenght1
		{
			[CompilerGenerated]
			get
			{
				return this.<BrushLenght1>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<BrushLenght1>k__BackingField = value;
			}
		}

		// Token: 0x1700078B RID: 1931
		// (get) Token: 0x06004053 RID: 16467 RVA: 0x00131E08 File Offset: 0x00130208
		// (set) Token: 0x06004054 RID: 16468 RVA: 0x00131E10 File Offset: 0x00130210
		[GpuData("brushLenght2")]
		public GpuValue<float> BrushLenght2
		{
			[CompilerGenerated]
			get
			{
				return this.<BrushLenght2>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<BrushLenght2>k__BackingField = value;
			}
		}

		// Token: 0x1700078C RID: 1932
		// (get) Token: 0x06004055 RID: 16469 RVA: 0x00131E19 File Offset: 0x00130219
		// (set) Token: 0x06004056 RID: 16470 RVA: 0x00131E21 File Offset: 0x00130221
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

		// Token: 0x1700078D RID: 1933
		// (get) Token: 0x06004057 RID: 16471 RVA: 0x00131E2A File Offset: 0x0013022A
		// (set) Token: 0x06004058 RID: 16472 RVA: 0x00131E32 File Offset: 0x00130232
		[GpuData("brushCollisionDistance")]
		public GpuValue<float> BrushCollisionDistance
		{
			[CompilerGenerated]
			get
			{
				return this.<BrushCollisionDistance>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<BrushCollisionDistance>k__BackingField = value;
			}
		}

		// Token: 0x1700078E RID: 1934
		// (get) Token: 0x06004059 RID: 16473 RVA: 0x00131E3B File Offset: 0x0013023B
		// (set) Token: 0x0600405A RID: 16474 RVA: 0x00131E43 File Offset: 0x00130243
		[GpuData("brushSpeed")]
		public GpuValue<Vector3> BrushSpeed
		{
			[CompilerGenerated]
			get
			{
				return this.<BrushSpeed>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<BrushSpeed>k__BackingField = value;
			}
		}

		// Token: 0x1700078F RID: 1935
		// (get) Token: 0x0600405B RID: 16475 RVA: 0x00131E4C File Offset: 0x0013024C
		// (set) Token: 0x0600405C RID: 16476 RVA: 0x00131E54 File Offset: 0x00130254
		[GpuData("brushLengthSpeed")]
		public GpuValue<float> BrushLengthSpeed
		{
			[CompilerGenerated]
			get
			{
				return this.<BrushLengthSpeed>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<BrushLengthSpeed>k__BackingField = value;
			}
		}

		// Token: 0x17000790 RID: 1936
		// (get) Token: 0x0600405D RID: 16477 RVA: 0x00131E5D File Offset: 0x0013025D
		// (set) Token: 0x0600405E RID: 16478 RVA: 0x00131E65 File Offset: 0x00130265
		[GpuData("brushColor")]
		public GpuValue<Vector3> BrushColor
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

		// Token: 0x0600405F RID: 16479 RVA: 0x00131E70 File Offset: 0x00130270
		private void ComputeStaticSpheres(GPSphere[] spheres)
		{
			List<SphereCollider> items = this.sphereCollidersCache.Items;
			if (spheres == null)
			{
				spheres = new GPSphere[items.Count];
			}
			for (int i = 0; i < items.Count; i++)
			{
				SphereCollider sphereCollider = items[i];
				Vector3 position = sphereCollider.transform.TransformPoint(sphereCollider.center);
				float radius = sphereCollider.transform.lossyScale.x * sphereCollider.radius;
				spheres[i] = new GPSphere(position, radius);
			}
		}

		// Token: 0x06004060 RID: 16480 RVA: 0x00131F00 File Offset: 0x00130300
		private void ComputeStaticSpheres(GPLineSphere[] lineSpheres)
		{
			List<LineSphereCollider> items = this.lineSphereCollidersCache.Items;
			if (lineSpheres == null)
			{
				lineSpheres = new GPLineSphere[items.Count];
			}
			for (int i = 0; i < items.Count; i++)
			{
				LineSphereCollider lineSphereCollider = items[i];
				float worldRadiusA = lineSphereCollider.WorldRadiusA;
				float worldRadiusB = lineSphereCollider.WorldRadiusB;
				Vector3 worldA = lineSphereCollider.WorldA;
				Vector3 worldB = lineSphereCollider.WorldB;
				lineSpheres[i] = new GPLineSphere(worldA, worldB, worldRadiusA, worldRadiusB);
			}
		}

		// Token: 0x06004061 RID: 16481 RVA: 0x00131F84 File Offset: 0x00130384
		public override void Dispatch()
		{
			this.Matrices.Data[0] = Camera.current.transform.worldToLocalMatrix;
			this.Matrices.Data[1] = this.creator.ScalpProvider.ToWorldMatrix;
			this.Matrices.Data[2] = this.creator.ScalpProvider.ToWorldMatrix.inverse;
			this.Matrices.PushData();
			if (this.StaticSpheres != null)
			{
				this.ComputeStaticSpheres(this.StaticSpheres.Data);
				this.StaticSpheres.PushData();
			}
			if (this.StaticLineSpheres != null)
			{
				this.ComputeStaticSpheres(this.StaticLineSpheres.Data);
				this.StaticLineSpheres.PushData();
			}
			this.Segments.Value = this.creator.Segments;
			this.BrushPosition.Value = this.creator.Brush.Position;
			this.BrushRadius.Value = this.creator.Brush.Radius;
			this.BrushLenght1.Value = this.creator.Brush.Lenght1;
			this.BrushLenght2.Value = this.creator.Brush.Lenght2;
			this.BrushStrength.Value = this.creator.Brush.Strength;
			this.BrushCollisionDistance.Value = this.creator.Brush.CollisionDistance;
			this.BrushSpeed.Value = this.creator.Brush.Speed;
			this.BrushColor.Value = new Vector3(this.creator.Brush.Color.r, this.creator.Brush.Color.g, this.creator.Brush.Color.b);
			base.Dispatch();
		}

		// Token: 0x06004062 RID: 16482 RVA: 0x0013218B File Offset: 0x0013058B
		public override int GetGroupsNumX()
		{
			return Mathf.CeilToInt((float)this.Vertices.Count / 256f);
		}

		// Token: 0x06004063 RID: 16483 RVA: 0x001321A4 File Offset: 0x001305A4
		public override void Dispose()
		{
			this.Vertices.Dispose();
			this.Distances.Dispose();
			this.Matrices.Dispose();
			this.Colors.Dispose();
			if (this.StaticSpheres != null)
			{
				this.StaticSpheres.Dispose();
			}
			if (this.StaticLineSpheres != null)
			{
				this.StaticLineSpheres.Dispose();
			}
		}

		// Token: 0x04003083 RID: 12419
		private readonly HairGeometryCreator creator;

		// Token: 0x04003084 RID: 12420
		private readonly CacheProvider<SphereCollider> sphereCollidersCache;

		// Token: 0x04003085 RID: 12421
		private readonly CacheProvider<LineSphereCollider> lineSphereCollidersCache;

		// Token: 0x04003086 RID: 12422
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private GpuBuffer<Vector3> <Vertices>k__BackingField;

		// Token: 0x04003087 RID: 12423
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private GpuBuffer<Color> <Colors>k__BackingField;

		// Token: 0x04003088 RID: 12424
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private GpuBuffer<float> <Distances>k__BackingField;

		// Token: 0x04003089 RID: 12425
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private GpuBuffer<Matrix4x4> <Matrices>k__BackingField;

		// Token: 0x0400308A RID: 12426
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private GpuBuffer<GPSphere> <StaticSpheres>k__BackingField;

		// Token: 0x0400308B RID: 12427
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private GpuBuffer<GPLineSphere> <StaticLineSpheres>k__BackingField;

		// Token: 0x0400308C RID: 12428
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private GpuValue<int> <Segments>k__BackingField;

		// Token: 0x0400308D RID: 12429
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private GpuValue<Vector3> <BrushPosition>k__BackingField;

		// Token: 0x0400308E RID: 12430
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private GpuValue<float> <BrushRadius>k__BackingField;

		// Token: 0x0400308F RID: 12431
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private GpuValue<float> <BrushLenght1>k__BackingField;

		// Token: 0x04003090 RID: 12432
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private GpuValue<float> <BrushLenght2>k__BackingField;

		// Token: 0x04003091 RID: 12433
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private GpuValue<float> <BrushStrength>k__BackingField;

		// Token: 0x04003092 RID: 12434
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private GpuValue<float> <BrushCollisionDistance>k__BackingField;

		// Token: 0x04003093 RID: 12435
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private GpuValue<Vector3> <BrushSpeed>k__BackingField;

		// Token: 0x04003094 RID: 12436
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private GpuValue<float> <BrushLengthSpeed>k__BackingField;

		// Token: 0x04003095 RID: 12437
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private GpuValue<Vector3> <BrushColor>k__BackingField;
	}
}
