using System;
using UnityEngine;

namespace Weelco
{
	// Token: 0x02000585 RID: 1413
	public class MouseCameraRotation : MonoBehaviour
	{
		// Token: 0x06002391 RID: 9105 RVA: 0x000CE9CB File Offset: 0x000CCDCB
		public MouseCameraRotation()
		{
		}

		// Token: 0x06002392 RID: 9106 RVA: 0x000CE9FC File Offset: 0x000CCDFC
		private void Start()
		{
			this.yaw = base.transform.rotation.eulerAngles.y;
		}

		// Token: 0x06002393 RID: 9107 RVA: 0x000CEA2C File Offset: 0x000CCE2C
		private void Update()
		{
			if (this.alwaysRotate)
			{
				this.MouseRotate();
			}
			else if (Input.GetKey(KeyCode.LeftControl))
			{
				this.MouseRotate();
			}
			else if (this.lerpBack)
			{
				this.LerpBack();
			}
		}

		// Token: 0x06002394 RID: 9108 RVA: 0x000CEA7C File Offset: 0x000CCE7C
		private void MouseRotate()
		{
			this.yaw += this.speedH * Input.GetAxis("Mouse X");
			this.pitch -= this.speedV * Input.GetAxis("Mouse Y");
			base.transform.eulerAngles = new Vector3(this.pitch, this.yaw, 0f);
		}

		// Token: 0x06002395 RID: 9109 RVA: 0x000CEAE8 File Offset: 0x000CCEE8
		private void LerpBack()
		{
			if (!base.transform.rotation.Equals(Quaternion.Euler(Vector3.zero)))
			{
				this.yaw = 0f;
				this.pitch = 0f;
				base.transform.rotation = Quaternion.Lerp(base.transform.rotation, Quaternion.Euler(Vector3.zero), Time.deltaTime * this.speedL);
			}
		}

		// Token: 0x04001E0E RID: 7694
		[Tooltip("If false, press Left Ctrl button for rotation")]
		public bool alwaysRotate;

		// Token: 0x04001E0F RID: 7695
		public bool lerpBack = true;

		// Token: 0x04001E10 RID: 7696
		public float speedH = 2.5f;

		// Token: 0x04001E11 RID: 7697
		public float speedV = 2.5f;

		// Token: 0x04001E12 RID: 7698
		public float speedL = 7f;

		// Token: 0x04001E13 RID: 7699
		private float yaw;

		// Token: 0x04001E14 RID: 7700
		private float pitch;
	}
}
