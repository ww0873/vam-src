using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace UnityEngine.PostProcessing
{
	// Token: 0x0200008B RID: 139
	public abstract class PostProcessingComponent<T> : PostProcessingComponentBase where T : PostProcessingModel
	{
		// Token: 0x060001F4 RID: 500 RVA: 0x00008343 File Offset: 0x00006743
		protected PostProcessingComponent()
		{
		}

		// Token: 0x17000064 RID: 100
		// (get) Token: 0x060001F5 RID: 501 RVA: 0x0000834B File Offset: 0x0000674B
		// (set) Token: 0x060001F6 RID: 502 RVA: 0x00008353 File Offset: 0x00006753
		public T model
		{
			[CompilerGenerated]
			get
			{
				return this.<model>k__BackingField;
			}
			[CompilerGenerated]
			internal set
			{
				this.<model>k__BackingField = value;
			}
		}

		// Token: 0x060001F7 RID: 503 RVA: 0x0000835C File Offset: 0x0000675C
		public virtual void Init(PostProcessingContext pcontext, T pmodel)
		{
			this.context = pcontext;
			this.model = pmodel;
		}

		// Token: 0x060001F8 RID: 504 RVA: 0x0000836C File Offset: 0x0000676C
		public override PostProcessingModel GetModel()
		{
			return this.model;
		}

		// Token: 0x040002F5 RID: 757
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private T <model>k__BackingField;
	}
}
