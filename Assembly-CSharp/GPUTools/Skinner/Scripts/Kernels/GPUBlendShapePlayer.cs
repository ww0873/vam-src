using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using GPUTools.Common.Scripts.PL.Abstract;
using GPUTools.Common.Scripts.PL.Attributes;
using GPUTools.Common.Scripts.PL.Tools;
using UnityEngine;

namespace GPUTools.Skinner.Scripts.Kernels
{
	// Token: 0x02000A8A RID: 2698
	public class GPUBlendShapePlayer : KernelBase
	{
		// Token: 0x060045EE RID: 17902 RVA: 0x00140030 File Offset: 0x0013E430
		public GPUBlendShapePlayer(SkinnedMeshRenderer skin) : base("Compute/BlendShaper", "CSBlendShaper")
		{
			this.skin = skin;
			this.mesh = skin.sharedMesh;
			this.VertexCount = new GpuValue<int>(this.mesh.vertexCount);
			this.ShapesCount = new GpuValue<int>(this.mesh.blendShapeCount);
			this.LocalToWorld = new GpuValue<GpuMatrix4x4>(new GpuMatrix4x4(skin.localToWorldMatrix));
			this.ShapesBuffer = new GpuBuffer<Vector3>(this.GetAllShapes(), 12);
			this.WeightsBuffer = new GpuBuffer<float>(this.mesh.blendShapeCount, 4);
			this.TransformMatricesBuffer = new GpuBuffer<Matrix4x4>(this.mesh.vertexCount, 64);
		}

		// Token: 0x1700097B RID: 2427
		// (get) Token: 0x060045EF RID: 17903 RVA: 0x001400E4 File Offset: 0x0013E4E4
		// (set) Token: 0x060045F0 RID: 17904 RVA: 0x001400EC File Offset: 0x0013E4EC
		[GpuData("vertexCount")]
		public GpuValue<int> VertexCount
		{
			[CompilerGenerated]
			get
			{
				return this.<VertexCount>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<VertexCount>k__BackingField = value;
			}
		}

		// Token: 0x1700097C RID: 2428
		// (get) Token: 0x060045F1 RID: 17905 RVA: 0x001400F5 File Offset: 0x0013E4F5
		// (set) Token: 0x060045F2 RID: 17906 RVA: 0x001400FD File Offset: 0x0013E4FD
		[GpuData("shapesCount")]
		public GpuValue<int> ShapesCount
		{
			[CompilerGenerated]
			get
			{
				return this.<ShapesCount>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<ShapesCount>k__BackingField = value;
			}
		}

		// Token: 0x1700097D RID: 2429
		// (get) Token: 0x060045F3 RID: 17907 RVA: 0x00140106 File Offset: 0x0013E506
		// (set) Token: 0x060045F4 RID: 17908 RVA: 0x0014010E File Offset: 0x0013E50E
		[GpuData("localToWorld")]
		public GpuValue<GpuMatrix4x4> LocalToWorld
		{
			[CompilerGenerated]
			get
			{
				return this.<LocalToWorld>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<LocalToWorld>k__BackingField = value;
			}
		}

		// Token: 0x1700097E RID: 2430
		// (get) Token: 0x060045F5 RID: 17909 RVA: 0x00140117 File Offset: 0x0013E517
		// (set) Token: 0x060045F6 RID: 17910 RVA: 0x0014011F File Offset: 0x0013E51F
		[GpuData("shapes")]
		public GpuBuffer<Vector3> ShapesBuffer
		{
			[CompilerGenerated]
			get
			{
				return this.<ShapesBuffer>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<ShapesBuffer>k__BackingField = value;
			}
		}

		// Token: 0x1700097F RID: 2431
		// (get) Token: 0x060045F7 RID: 17911 RVA: 0x00140128 File Offset: 0x0013E528
		// (set) Token: 0x060045F8 RID: 17912 RVA: 0x00140130 File Offset: 0x0013E530
		[GpuData("weights")]
		public GpuBuffer<float> WeightsBuffer
		{
			[CompilerGenerated]
			get
			{
				return this.<WeightsBuffer>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<WeightsBuffer>k__BackingField = value;
			}
		}

		// Token: 0x17000980 RID: 2432
		// (get) Token: 0x060045F9 RID: 17913 RVA: 0x00140139 File Offset: 0x0013E539
		// (set) Token: 0x060045FA RID: 17914 RVA: 0x00140141 File Offset: 0x0013E541
		[GpuData("transforms")]
		public GpuBuffer<Matrix4x4> TransformMatricesBuffer
		{
			[CompilerGenerated]
			get
			{
				return this.<TransformMatricesBuffer>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<TransformMatricesBuffer>k__BackingField = value;
			}
		}

		// Token: 0x060045FB RID: 17915 RVA: 0x0014014C File Offset: 0x0013E54C
		private Vector3[] GetAllShapes()
		{
			List<Vector3> list = new List<Vector3>();
			Vector3[] array = new Vector3[this.VertexCount.Value];
			for (int i = 0; i < this.mesh.blendShapeCount; i++)
			{
				this.mesh.GetBlendShapeFrameVertices(i, 0, array, null, null);
				list.AddRange(array);
			}
			return list.ToArray();
		}

		// Token: 0x060045FC RID: 17916 RVA: 0x001401A9 File Offset: 0x0013E5A9
		public override void Dispatch()
		{
			this.LocalToWorld.Value.Data = this.skin.localToWorldMatrix;
			this.PushWeights();
			base.Dispatch();
		}

		// Token: 0x060045FD RID: 17917 RVA: 0x001401D4 File Offset: 0x0013E5D4
		private void PushWeights()
		{
			for (int i = 0; i < this.mesh.blendShapeCount; i++)
			{
				float num = Mathf.Clamp01(this.skin.GetBlendShapeWeight(i) * 0.01f);
				this.WeightsBuffer.Data[i] = num;
			}
			this.WeightsBuffer.PushData();
		}

		// Token: 0x060045FE RID: 17918 RVA: 0x0014022E File Offset: 0x0013E62E
		public override int GetGroupsNumX()
		{
			return Mathf.CeilToInt((float)this.VertexCount.Value / 256f);
		}

		// Token: 0x060045FF RID: 17919 RVA: 0x00140247 File Offset: 0x0013E647
		public override void Dispose()
		{
			this.ShapesBuffer.Dispose();
			this.WeightsBuffer.Dispose();
			this.TransformMatricesBuffer.Dispose();
		}

		// Token: 0x04003394 RID: 13204
		private readonly SkinnedMeshRenderer skin;

		// Token: 0x04003395 RID: 13205
		private readonly Mesh mesh;

		// Token: 0x04003396 RID: 13206
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private GpuValue<int> <VertexCount>k__BackingField;

		// Token: 0x04003397 RID: 13207
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private GpuValue<int> <ShapesCount>k__BackingField;

		// Token: 0x04003398 RID: 13208
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private GpuValue<GpuMatrix4x4> <LocalToWorld>k__BackingField;

		// Token: 0x04003399 RID: 13209
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private GpuBuffer<Vector3> <ShapesBuffer>k__BackingField;

		// Token: 0x0400339A RID: 13210
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private GpuBuffer<float> <WeightsBuffer>k__BackingField;

		// Token: 0x0400339B RID: 13211
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private GpuBuffer<Matrix4x4> <TransformMatricesBuffer>k__BackingField;
	}
}
