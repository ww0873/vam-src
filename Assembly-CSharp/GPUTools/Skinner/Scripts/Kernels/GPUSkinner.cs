using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using GPUTools.Common.Scripts.PL.Abstract;
using GPUTools.Common.Scripts.PL.Attributes;
using GPUTools.Common.Scripts.PL.Tools;
using UnityEngine;

namespace GPUTools.Skinner.Scripts.Kernels
{
	// Token: 0x02000A8C RID: 2700
	public class GPUSkinner : KernelBase
	{
		// Token: 0x06004600 RID: 17920 RVA: 0x0014026C File Offset: 0x0013E66C
		public GPUSkinner(SkinnedMeshRenderer skin) : base("Compute/Skinner", "CSComputeMatrices")
		{
			this.skin = skin;
			Mesh sharedMesh = skin.sharedMesh;
			this.bones = skin.bones;
			this.bindposes = sharedMesh.bindposes;
			this.TransformMatricesBuffer = new GpuBuffer<Matrix4x4>(sharedMesh.vertexCount, 64);
			this.BonesBuffer = new GpuBuffer<Matrix4x4>(new Matrix4x4[skin.bones.Length], 64);
			this.WeightsBuffer = new GpuBuffer<Weight>(this.GetWeightsArray(sharedMesh), 32);
		}

		// Token: 0x17000981 RID: 2433
		// (get) Token: 0x06004601 RID: 17921 RVA: 0x001402F0 File Offset: 0x0013E6F0
		// (set) Token: 0x06004602 RID: 17922 RVA: 0x001402F8 File Offset: 0x0013E6F8
		[GpuData("weights")]
		public GpuBuffer<Weight> WeightsBuffer
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

		// Token: 0x17000982 RID: 2434
		// (get) Token: 0x06004603 RID: 17923 RVA: 0x00140301 File Offset: 0x0013E701
		// (set) Token: 0x06004604 RID: 17924 RVA: 0x00140309 File Offset: 0x0013E709
		[GpuData("bones")]
		public GpuBuffer<Matrix4x4> BonesBuffer
		{
			[CompilerGenerated]
			get
			{
				return this.<BonesBuffer>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<BonesBuffer>k__BackingField = value;
			}
		}

		// Token: 0x17000983 RID: 2435
		// (get) Token: 0x06004605 RID: 17925 RVA: 0x00140312 File Offset: 0x0013E712
		// (set) Token: 0x06004606 RID: 17926 RVA: 0x0014031A File Offset: 0x0013E71A
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

		// Token: 0x06004607 RID: 17927 RVA: 0x00140323 File Offset: 0x0013E723
		public override void Dispatch()
		{
			this.CalculateBones();
			base.Dispatch();
		}

		// Token: 0x06004608 RID: 17928 RVA: 0x00140331 File Offset: 0x0013E731
		public override void Dispose()
		{
			this.TransformMatricesBuffer.Dispose();
			this.BonesBuffer.Dispose();
			this.WeightsBuffer.Dispose();
		}

		// Token: 0x06004609 RID: 17929 RVA: 0x00140354 File Offset: 0x0013E754
		public override int GetGroupsNumX()
		{
			return Mathf.CeilToInt((float)this.skin.sharedMesh.vertexCount / 256f);
		}

		// Token: 0x0600460A RID: 17930 RVA: 0x00140374 File Offset: 0x0013E774
		private void CalculateBones()
		{
			for (int i = 0; i < this.BonesBuffer.Data.Length; i++)
			{
				this.BonesBuffer.Data[i] = this.bones[i].localToWorldMatrix * this.bindposes[i];
			}
			this.BonesBuffer.PushData();
		}

		// Token: 0x0600460B RID: 17931 RVA: 0x001403E4 File Offset: 0x0013E7E4
		private Weight[] GetWeightsArray(Mesh mesh)
		{
			Weight[] array = new Weight[mesh.boneWeights.Length];
			BoneWeight[] boneWeights = mesh.boneWeights;
			for (int i = 0; i < boneWeights.Length; i++)
			{
				BoneWeight boneWeight = boneWeights[i];
				Weight weight = new Weight
				{
					bi0 = boneWeight.boneIndex0,
					bi1 = boneWeight.boneIndex1,
					bi2 = boneWeight.boneIndex2,
					bi3 = boneWeight.boneIndex3,
					w0 = boneWeight.weight0,
					w1 = boneWeight.weight1,
					w2 = boneWeight.weight2,
					w3 = boneWeight.weight3
				};
				array[i] = weight;
			}
			return array;
		}

		// Token: 0x040033A4 RID: 13220
		private readonly SkinnedMeshRenderer skin;

		// Token: 0x040033A5 RID: 13221
		private Transform[] bones;

		// Token: 0x040033A6 RID: 13222
		private Matrix4x4[] bindposes;

		// Token: 0x040033A7 RID: 13223
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private GpuBuffer<Weight> <WeightsBuffer>k__BackingField;

		// Token: 0x040033A8 RID: 13224
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private GpuBuffer<Matrix4x4> <BonesBuffer>k__BackingField;

		// Token: 0x040033A9 RID: 13225
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private GpuBuffer<Matrix4x4> <TransformMatricesBuffer>k__BackingField;
	}
}
