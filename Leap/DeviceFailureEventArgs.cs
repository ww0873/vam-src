using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace Leap
{
	// Token: 0x020005CE RID: 1486
	public class DeviceFailureEventArgs : LeapEventArgs
	{
		// Token: 0x0600259D RID: 9629 RVA: 0x000D6CE1 File Offset: 0x000D50E1
		public DeviceFailureEventArgs(uint code, string message, string serial) : base(LeapEvent.EVENT_DEVICE_FAILURE)
		{
			this.ErrorCode = code;
			this.ErrorMessage = message;
			this.DeviceSerialNumber = serial;
		}

		// Token: 0x17000481 RID: 1153
		// (get) Token: 0x0600259E RID: 9630 RVA: 0x000D6CFF File Offset: 0x000D50FF
		// (set) Token: 0x0600259F RID: 9631 RVA: 0x000D6D07 File Offset: 0x000D5107
		public uint ErrorCode
		{
			[CompilerGenerated]
			get
			{
				return this.<ErrorCode>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<ErrorCode>k__BackingField = value;
			}
		}

		// Token: 0x17000482 RID: 1154
		// (get) Token: 0x060025A0 RID: 9632 RVA: 0x000D6D10 File Offset: 0x000D5110
		// (set) Token: 0x060025A1 RID: 9633 RVA: 0x000D6D18 File Offset: 0x000D5118
		public string ErrorMessage
		{
			[CompilerGenerated]
			get
			{
				return this.<ErrorMessage>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<ErrorMessage>k__BackingField = value;
			}
		}

		// Token: 0x17000483 RID: 1155
		// (get) Token: 0x060025A2 RID: 9634 RVA: 0x000D6D21 File Offset: 0x000D5121
		// (set) Token: 0x060025A3 RID: 9635 RVA: 0x000D6D29 File Offset: 0x000D5129
		public string DeviceSerialNumber
		{
			[CompilerGenerated]
			get
			{
				return this.<DeviceSerialNumber>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<DeviceSerialNumber>k__BackingField = value;
			}
		}

		// Token: 0x04001F55 RID: 8021
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private uint <ErrorCode>k__BackingField;

		// Token: 0x04001F56 RID: 8022
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private string <ErrorMessage>k__BackingField;

		// Token: 0x04001F57 RID: 8023
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private string <DeviceSerialNumber>k__BackingField;
	}
}
