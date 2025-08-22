using System;

namespace Battlehub.RTCommon
{
	// Token: 0x020000D0 RID: 208
	public class Record
	{
		// Token: 0x060003DF RID: 991 RVA: 0x000166F7 File Offset: 0x00014AF7
		public Record(object target, object state, ApplyCallback applyCallback, PurgeCallback purgeCallback)
		{
			if (applyCallback == null)
			{
				throw new ArgumentNullException("callback");
			}
			this.m_target = target;
			this.m_applyCallback = applyCallback;
			this.m_purgeCallback = purgeCallback;
			if (state != null)
			{
				this.m_state = state;
			}
		}

		// Token: 0x170000A2 RID: 162
		// (get) Token: 0x060003E0 RID: 992 RVA: 0x00016733 File Offset: 0x00014B33
		public object Target
		{
			get
			{
				return this.m_target;
			}
		}

		// Token: 0x170000A3 RID: 163
		// (get) Token: 0x060003E1 RID: 993 RVA: 0x0001673B File Offset: 0x00014B3B
		public object State
		{
			get
			{
				return this.m_state;
			}
		}

		// Token: 0x060003E2 RID: 994 RVA: 0x00016743 File Offset: 0x00014B43
		public bool Apply()
		{
			return this.m_applyCallback(this);
		}

		// Token: 0x060003E3 RID: 995 RVA: 0x00016751 File Offset: 0x00014B51
		public void Purge()
		{
			this.m_purgeCallback(this);
		}

		// Token: 0x04000429 RID: 1065
		private object m_state;

		// Token: 0x0400042A RID: 1066
		private object m_target;

		// Token: 0x0400042B RID: 1067
		private ApplyCallback m_applyCallback;

		// Token: 0x0400042C RID: 1068
		private PurgeCallback m_purgeCallback;
	}
}
