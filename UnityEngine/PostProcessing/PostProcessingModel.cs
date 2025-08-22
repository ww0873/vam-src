using System;

namespace UnityEngine.PostProcessing
{
	// Token: 0x0200008F RID: 143
	[Serializable]
	public abstract class PostProcessingModel
	{
		// Token: 0x06000209 RID: 521 RVA: 0x0000DCF8 File Offset: 0x0000C0F8
		protected PostProcessingModel()
		{
		}

		// Token: 0x1700006B RID: 107
		// (get) Token: 0x0600020A RID: 522 RVA: 0x0000DD00 File Offset: 0x0000C100
		// (set) Token: 0x0600020B RID: 523 RVA: 0x0000DD08 File Offset: 0x0000C108
		public bool enabled
		{
			get
			{
				return this.m_Enabled;
			}
			set
			{
				this.m_Enabled = value;
				if (value)
				{
					this.OnValidate();
				}
			}
		}

		// Token: 0x0600020C RID: 524
		public abstract void Reset();

		// Token: 0x0600020D RID: 525 RVA: 0x0000DD1D File Offset: 0x0000C11D
		public virtual void OnValidate()
		{
		}

		// Token: 0x040002FB RID: 763
		[SerializeField]
		[GetSet("enabled")]
		private bool m_Enabled;
	}
}
