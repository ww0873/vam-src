using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using LeapInternal;

namespace Leap
{
	// Token: 0x020005D2 RID: 1490
	public class HeadPoseEventArgs : LeapEventArgs
	{
		// Token: 0x060025B3 RID: 9651 RVA: 0x000D6DE0 File Offset: 0x000D51E0
		public HeadPoseEventArgs(LEAP_VECTOR head_position, LEAP_QUATERNION head_orientation) : base(LeapEvent.EVENT_POINT_MAPPING_CHANGE)
		{
			this.headPosition = head_position;
			this.headOrientation = head_orientation;
		}

		// Token: 0x1700048A RID: 1162
		// (get) Token: 0x060025B4 RID: 9652 RVA: 0x000D6DF8 File Offset: 0x000D51F8
		// (set) Token: 0x060025B5 RID: 9653 RVA: 0x000D6E00 File Offset: 0x000D5200
		public LEAP_VECTOR headPosition
		{
			[CompilerGenerated]
			get
			{
				return this.<headPosition>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<headPosition>k__BackingField = value;
			}
		}

		// Token: 0x1700048B RID: 1163
		// (get) Token: 0x060025B6 RID: 9654 RVA: 0x000D6E09 File Offset: 0x000D5209
		// (set) Token: 0x060025B7 RID: 9655 RVA: 0x000D6E11 File Offset: 0x000D5211
		public LEAP_QUATERNION headOrientation
		{
			[CompilerGenerated]
			get
			{
				return this.<headOrientation>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<headOrientation>k__BackingField = value;
			}
		}

		// Token: 0x04001F5E RID: 8030
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private LEAP_VECTOR <headPosition>k__BackingField;

		// Token: 0x04001F5F RID: 8031
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private LEAP_QUATERNION <headOrientation>k__BackingField;
	}
}
