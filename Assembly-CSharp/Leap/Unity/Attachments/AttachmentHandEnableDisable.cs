using System;
using UnityEngine;

namespace Leap.Unity.Attachments
{
	// Token: 0x02000668 RID: 1640
	public class AttachmentHandEnableDisable : MonoBehaviour
	{
		// Token: 0x0600282E RID: 10286 RVA: 0x000DDA9F File Offset: 0x000DBE9F
		public AttachmentHandEnableDisable()
		{
		}

		// Token: 0x0600282F RID: 10287 RVA: 0x000DDAA8 File Offset: 0x000DBEA8
		private void Update()
		{
			if (!this.attachmentHand.isTracked && this.attachmentHand.gameObject.activeSelf)
			{
				this.attachmentHand.gameObject.SetActive(false);
			}
			if (this.attachmentHand.isTracked && !this.attachmentHand.gameObject.activeSelf)
			{
				this.attachmentHand.gameObject.SetActive(true);
			}
		}

		// Token: 0x0400217D RID: 8573
		public AttachmentHand attachmentHand;
	}
}
