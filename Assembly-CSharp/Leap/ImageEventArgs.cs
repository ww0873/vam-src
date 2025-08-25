using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace Leap
{
	// Token: 0x020005D0 RID: 1488
	public class ImageEventArgs : LeapEventArgs
	{
		// Token: 0x060025A9 RID: 9641 RVA: 0x000D6D6C File Offset: 0x000D516C
		public ImageEventArgs(Image image) : base(LeapEvent.EVENT_IMAGE)
		{
			this.image = image;
		}

		// Token: 0x17000486 RID: 1158
		// (get) Token: 0x060025AA RID: 9642 RVA: 0x000D6D7D File Offset: 0x000D517D
		// (set) Token: 0x060025AB RID: 9643 RVA: 0x000D6D85 File Offset: 0x000D5185
		public Image image
		{
			[CompilerGenerated]
			get
			{
				return this.<image>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<image>k__BackingField = value;
			}
		}

		// Token: 0x04001F5A RID: 8026
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private Image <image>k__BackingField;
	}
}
