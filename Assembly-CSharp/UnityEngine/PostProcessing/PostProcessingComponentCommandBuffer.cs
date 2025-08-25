using System;
using UnityEngine.Rendering;

namespace UnityEngine.PostProcessing
{
	// Token: 0x0200008C RID: 140
	public abstract class PostProcessingComponentCommandBuffer<T> : PostProcessingComponent<T> where T : PostProcessingModel
	{
		// Token: 0x060001F9 RID: 505 RVA: 0x00008379 File Offset: 0x00006779
		protected PostProcessingComponentCommandBuffer()
		{
		}

		// Token: 0x060001FA RID: 506
		public abstract CameraEvent GetCameraEvent();

		// Token: 0x060001FB RID: 507
		public abstract string GetName();

		// Token: 0x060001FC RID: 508
		public abstract void PopulateCommandBuffer(CommandBuffer cb);
	}
}
