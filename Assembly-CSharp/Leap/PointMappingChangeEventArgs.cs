using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace Leap
{
	// Token: 0x020005D1 RID: 1489
	public class PointMappingChangeEventArgs : LeapEventArgs
	{
		// Token: 0x060025AC RID: 9644 RVA: 0x000D6D8E File Offset: 0x000D518E
		public PointMappingChangeEventArgs(long frame_id, long timestamp, uint nPoints) : base(LeapEvent.EVENT_POINT_MAPPING_CHANGE)
		{
			this.frameID = frame_id;
			this.timestamp = timestamp;
			this.nPoints = nPoints;
		}

		// Token: 0x17000487 RID: 1159
		// (get) Token: 0x060025AD RID: 9645 RVA: 0x000D6DAD File Offset: 0x000D51AD
		// (set) Token: 0x060025AE RID: 9646 RVA: 0x000D6DB5 File Offset: 0x000D51B5
		public long frameID
		{
			[CompilerGenerated]
			get
			{
				return this.<frameID>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<frameID>k__BackingField = value;
			}
		}

		// Token: 0x17000488 RID: 1160
		// (get) Token: 0x060025AF RID: 9647 RVA: 0x000D6DBE File Offset: 0x000D51BE
		// (set) Token: 0x060025B0 RID: 9648 RVA: 0x000D6DC6 File Offset: 0x000D51C6
		public long timestamp
		{
			[CompilerGenerated]
			get
			{
				return this.<timestamp>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<timestamp>k__BackingField = value;
			}
		}

		// Token: 0x17000489 RID: 1161
		// (get) Token: 0x060025B1 RID: 9649 RVA: 0x000D6DCF File Offset: 0x000D51CF
		// (set) Token: 0x060025B2 RID: 9650 RVA: 0x000D6DD7 File Offset: 0x000D51D7
		public uint nPoints
		{
			[CompilerGenerated]
			get
			{
				return this.<nPoints>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<nPoints>k__BackingField = value;
			}
		}

		// Token: 0x04001F5B RID: 8027
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private long <frameID>k__BackingField;

		// Token: 0x04001F5C RID: 8028
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private long <timestamp>k__BackingField;

		// Token: 0x04001F5D RID: 8029
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private uint <nPoints>k__BackingField;
	}
}
