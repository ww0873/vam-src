using System;
using GPUTools.Common.Scripts.PL.Abstract;
using GPUTools.Common.Scripts.PL.Tools;
using GPUTools.Common.Scripts.Tools.Kernels;
using UnityEngine;

namespace GPUTools.Skinner.Scripts.Kernels
{
	// Token: 0x02000A8D RID: 2701
	public class GPUSkinnerPro : PrimitiveBase
	{
		// Token: 0x0600460C RID: 17932 RVA: 0x001404B4 File Offset: 0x0013E8B4
		public GPUSkinnerPro(SkinnedMeshRenderer skin)
		{
			this.skinner = new GPUSkinner(skin);
			base.AddPass(this.skinner);
			if (skin.sharedMesh.blendShapeCount == 0)
			{
				return;
			}
			this.blendShapePlayer = new GPUBlendShapePlayer(skin);
			this.matrixMultiplier = new GPUMatrixMultiplier(this.blendShapePlayer.TransformMatricesBuffer, this.skinner.TransformMatricesBuffer);
			base.AddPass(this.blendShapePlayer);
			base.AddPass(this.matrixMultiplier);
		}

		// Token: 0x17000984 RID: 2436
		// (get) Token: 0x0600460D RID: 17933 RVA: 0x00140535 File Offset: 0x0013E935
		public GpuBuffer<Matrix4x4> TransformMatricesBuffer
		{
			get
			{
				if (this.matrixMultiplier != null)
				{
					return this.matrixMultiplier.ResultMatrices;
				}
				return this.skinner.TransformMatricesBuffer;
			}
		}

		// Token: 0x040033AA RID: 13226
		private GPUSkinner skinner;

		// Token: 0x040033AB RID: 13227
		private GPUBlendShapePlayer blendShapePlayer;

		// Token: 0x040033AC RID: 13228
		private GPUMatrixMultiplier matrixMultiplier;
	}
}
