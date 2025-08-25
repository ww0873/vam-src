using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace Leap
{
	// Token: 0x020005D7 RID: 1495
	public class FailedDevice : IEquatable<FailedDevice>
	{
		// Token: 0x060025BB RID: 9659 RVA: 0x000D6E3C File Offset: 0x000D523C
		public FailedDevice()
		{
			this.Failure = FailedDevice.FailureType.FAIL_UNKNOWN;
			this.PnpId = "0";
		}

		// Token: 0x060025BC RID: 9660 RVA: 0x000D6E56 File Offset: 0x000D5256
		public bool Equals(FailedDevice other)
		{
			return this.PnpId == other.PnpId;
		}

		// Token: 0x1700048C RID: 1164
		// (get) Token: 0x060025BD RID: 9661 RVA: 0x000D6E69 File Offset: 0x000D5269
		// (set) Token: 0x060025BE RID: 9662 RVA: 0x000D6E71 File Offset: 0x000D5271
		public string PnpId
		{
			[CompilerGenerated]
			get
			{
				return this.<PnpId>k__BackingField;
			}
			[CompilerGenerated]
			private set
			{
				this.<PnpId>k__BackingField = value;
			}
		}

		// Token: 0x1700048D RID: 1165
		// (get) Token: 0x060025BF RID: 9663 RVA: 0x000D6E7A File Offset: 0x000D527A
		// (set) Token: 0x060025C0 RID: 9664 RVA: 0x000D6E82 File Offset: 0x000D5282
		public FailedDevice.FailureType Failure
		{
			[CompilerGenerated]
			get
			{
				return this.<Failure>k__BackingField;
			}
			[CompilerGenerated]
			private set
			{
				this.<Failure>k__BackingField = value;
			}
		}

		// Token: 0x04001F64 RID: 8036
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private string <PnpId>k__BackingField;

		// Token: 0x04001F65 RID: 8037
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private FailedDevice.FailureType <Failure>k__BackingField;

		// Token: 0x020005D8 RID: 1496
		public enum FailureType
		{
			// Token: 0x04001F67 RID: 8039
			FAIL_UNKNOWN,
			// Token: 0x04001F68 RID: 8040
			FAIL_CALIBRATION,
			// Token: 0x04001F69 RID: 8041
			FAIL_FIRMWARE,
			// Token: 0x04001F6A RID: 8042
			FAIL_TRANSPORT,
			// Token: 0x04001F6B RID: 8043
			FAIL_CONTROl
		}
	}
}
