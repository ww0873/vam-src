using System;
using System.Collections.Generic;

namespace Leap
{
	// Token: 0x020005D9 RID: 1497
	public class FailedDeviceList : List<FailedDevice>
	{
		// Token: 0x060025C1 RID: 9665 RVA: 0x000D6E8B File Offset: 0x000D528B
		public FailedDeviceList()
		{
		}

		// Token: 0x060025C2 RID: 9666 RVA: 0x000D6E93 File Offset: 0x000D5293
		public FailedDeviceList Append(FailedDeviceList other)
		{
			base.AddRange(other);
			return this;
		}

		// Token: 0x1700048E RID: 1166
		// (get) Token: 0x060025C3 RID: 9667 RVA: 0x000D6E9D File Offset: 0x000D529D
		public bool IsEmpty
		{
			get
			{
				return base.Count == 0;
			}
		}
	}
}
