using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Battlehub.RTHandles
{
	// Token: 0x020000F9 RID: 249
	public class FilteringArgs : EventArgs
	{
		// Token: 0x060005A9 RID: 1449 RVA: 0x0001F4EF File Offset: 0x0001D8EF
		public FilteringArgs()
		{
		}

		// Token: 0x170000F0 RID: 240
		// (get) Token: 0x060005AA RID: 1450 RVA: 0x0001F4F7 File Offset: 0x0001D8F7
		// (set) Token: 0x060005AB RID: 1451 RVA: 0x0001F4FF File Offset: 0x0001D8FF
		public bool Cancel
		{
			get
			{
				return this.m_cancel;
			}
			set
			{
				if (value)
				{
					this.m_cancel = true;
				}
			}
		}

		// Token: 0x170000F1 RID: 241
		// (get) Token: 0x060005AC RID: 1452 RVA: 0x0001F50E File Offset: 0x0001D90E
		// (set) Token: 0x060005AD RID: 1453 RVA: 0x0001F516 File Offset: 0x0001D916
		public GameObject Object
		{
			[CompilerGenerated]
			get
			{
				return this.<Object>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<Object>k__BackingField = value;
			}
		}

		// Token: 0x060005AE RID: 1454 RVA: 0x0001F51F File Offset: 0x0001D91F
		public void Reset()
		{
			this.m_cancel = false;
		}

		// Token: 0x04000517 RID: 1303
		private bool m_cancel;

		// Token: 0x04000518 RID: 1304
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private GameObject <Object>k__BackingField;
	}
}
