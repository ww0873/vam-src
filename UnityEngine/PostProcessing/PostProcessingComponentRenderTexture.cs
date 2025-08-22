using System;

namespace UnityEngine.PostProcessing
{
	// Token: 0x0200008D RID: 141
	public abstract class PostProcessingComponentRenderTexture<T> : PostProcessingComponent<T> where T : PostProcessingModel
	{
		// Token: 0x060001FD RID: 509 RVA: 0x00008967 File Offset: 0x00006D67
		protected PostProcessingComponentRenderTexture()
		{
		}

		// Token: 0x060001FE RID: 510 RVA: 0x0000896F File Offset: 0x00006D6F
		public virtual void Prepare(Material material)
		{
		}
	}
}
