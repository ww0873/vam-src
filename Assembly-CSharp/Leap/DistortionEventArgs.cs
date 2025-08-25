using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace Leap
{
	// Token: 0x020005C8 RID: 1480
	public class DistortionEventArgs : LeapEventArgs
	{
		// Token: 0x06002583 RID: 9603 RVA: 0x000D6BB9 File Offset: 0x000D4FB9
		public DistortionEventArgs(DistortionData distortion, Image.CameraType camera) : base(LeapEvent.EVENT_DISTORTION_CHANGE)
		{
			this.distortion = distortion;
			this.camera = camera;
		}

		// Token: 0x17000477 RID: 1143
		// (get) Token: 0x06002584 RID: 9604 RVA: 0x000D6BD1 File Offset: 0x000D4FD1
		// (set) Token: 0x06002585 RID: 9605 RVA: 0x000D6BD9 File Offset: 0x000D4FD9
		public DistortionData distortion
		{
			[CompilerGenerated]
			get
			{
				return this.<distortion>k__BackingField;
			}
			[CompilerGenerated]
			protected set
			{
				this.<distortion>k__BackingField = value;
			}
		}

		// Token: 0x17000478 RID: 1144
		// (get) Token: 0x06002586 RID: 9606 RVA: 0x000D6BE2 File Offset: 0x000D4FE2
		// (set) Token: 0x06002587 RID: 9607 RVA: 0x000D6BEA File Offset: 0x000D4FEA
		public Image.CameraType camera
		{
			[CompilerGenerated]
			get
			{
				return this.<camera>k__BackingField;
			}
			[CompilerGenerated]
			protected set
			{
				this.<camera>k__BackingField = value;
			}
		}

		// Token: 0x04001F4B RID: 8011
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private DistortionData <distortion>k__BackingField;

		// Token: 0x04001F4C RID: 8012
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private Image.CameraType <camera>k__BackingField;
	}
}
