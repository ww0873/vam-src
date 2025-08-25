using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace Leap
{
	// Token: 0x020005CD RID: 1485
	public class DeviceEventArgs : LeapEventArgs
	{
		// Token: 0x0600259A RID: 9626 RVA: 0x000D6CC0 File Offset: 0x000D50C0
		public DeviceEventArgs(Device device) : base(LeapEvent.EVENT_DEVICE)
		{
			this.Device = device;
		}

		// Token: 0x17000480 RID: 1152
		// (get) Token: 0x0600259B RID: 9627 RVA: 0x000D6CD0 File Offset: 0x000D50D0
		// (set) Token: 0x0600259C RID: 9628 RVA: 0x000D6CD8 File Offset: 0x000D50D8
		public Device Device
		{
			[CompilerGenerated]
			get
			{
				return this.<Device>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<Device>k__BackingField = value;
			}
		}

		// Token: 0x04001F54 RID: 8020
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private Device <Device>k__BackingField;
	}
}
