using System;
using UnityEngine;
using UnityEngine.XR;

namespace ZenFulcrum.EmbeddedBrowser
{
	// Token: 0x020005AF RID: 1455
	public class VRMode : MonoBehaviour
	{
		// Token: 0x06002486 RID: 9350 RVA: 0x000D32DC File Offset: 0x000D16DC
		public VRMode()
		{
		}

		// Token: 0x06002487 RID: 9351 RVA: 0x000D32E4 File Offset: 0x000D16E4
		public void OnEnable()
		{
			this.oldState = XRSettings.enabled;
			XRSettings.enabled = this.enableVR;
			if (XRSettings.enabled)
			{
				XRDevice.SetTrackingSpaceType(TrackingSpaceType.RoomScale);
			}
		}

		// Token: 0x06002488 RID: 9352 RVA: 0x000D330D File Offset: 0x000D170D
		public void OnDisable()
		{
			XRSettings.enabled = this.oldState;
		}

		// Token: 0x04001EAF RID: 7855
		public bool enableVR;

		// Token: 0x04001EB0 RID: 7856
		private bool oldState;
	}
}
