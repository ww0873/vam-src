using System;

namespace UnityEngine.PostProcessing
{
	// Token: 0x0200008A RID: 138
	public abstract class PostProcessingComponentBase
	{
		// Token: 0x060001EE RID: 494 RVA: 0x00008334 File Offset: 0x00006734
		protected PostProcessingComponentBase()
		{
		}

		// Token: 0x060001EF RID: 495 RVA: 0x0000833C File Offset: 0x0000673C
		public virtual DepthTextureMode GetCameraFlags()
		{
			return DepthTextureMode.None;
		}

		// Token: 0x17000063 RID: 99
		// (get) Token: 0x060001F0 RID: 496
		public abstract bool active { get; }

		// Token: 0x060001F1 RID: 497 RVA: 0x0000833F File Offset: 0x0000673F
		public virtual void OnEnable()
		{
		}

		// Token: 0x060001F2 RID: 498 RVA: 0x00008341 File Offset: 0x00006741
		public virtual void OnDisable()
		{
		}

		// Token: 0x060001F3 RID: 499
		public abstract PostProcessingModel GetModel();

		// Token: 0x040002F4 RID: 756
		public PostProcessingContext context;
	}
}
