using System;
using UnityEngine;

// Token: 0x0200041F RID: 1055
[AddComponentMenu("Camera-Control/Mouse Look")]
public class MouseLook : MonoBehaviour
{
	// Token: 0x06001A85 RID: 6789 RVA: 0x00094680 File Offset: 0x00092A80
	public MouseLook()
	{
	}

	// Token: 0x06001A86 RID: 6790 RVA: 0x000946D8 File Offset: 0x00092AD8
	private void Update()
	{
		if (this.axes == MouseLook.RotationAxes.MouseXAndY)
		{
			float y = base.transform.localEulerAngles.y + Input.GetAxis("Mouse X") * this.sensitivityX;
			this.rotationY += Input.GetAxis("Mouse Y") * this.sensitivityY;
			this.rotationY = Mathf.Clamp(this.rotationY, this.minimumY, this.maximumY);
			base.transform.localEulerAngles = new Vector3(-this.rotationY, y, 0f);
		}
		else if (this.axes == MouseLook.RotationAxes.MouseX)
		{
			base.transform.Rotate(0f, Input.GetAxis("Mouse X") * this.sensitivityX, 0f);
		}
		else
		{
			this.rotationY += Input.GetAxis("Mouse Y") * this.sensitivityY;
			this.rotationY = Mathf.Clamp(this.rotationY, this.minimumY, this.maximumY);
			base.transform.localEulerAngles = new Vector3(-this.rotationY, base.transform.localEulerAngles.y, 0f);
		}
	}

	// Token: 0x06001A87 RID: 6791 RVA: 0x00094814 File Offset: 0x00092C14
	private void Start()
	{
		if (base.GetComponent<Rigidbody>())
		{
			base.GetComponent<Rigidbody>().freezeRotation = true;
		}
	}

	// Token: 0x0400159C RID: 5532
	public MouseLook.RotationAxes axes;

	// Token: 0x0400159D RID: 5533
	public float sensitivityX = 15f;

	// Token: 0x0400159E RID: 5534
	public float sensitivityY = 15f;

	// Token: 0x0400159F RID: 5535
	public float minimumX = -360f;

	// Token: 0x040015A0 RID: 5536
	public float maximumX = 360f;

	// Token: 0x040015A1 RID: 5537
	public float minimumY = -60f;

	// Token: 0x040015A2 RID: 5538
	public float maximumY = 60f;

	// Token: 0x040015A3 RID: 5539
	private float rotationY;

	// Token: 0x02000420 RID: 1056
	public enum RotationAxes
	{
		// Token: 0x040015A5 RID: 5541
		MouseXAndY,
		// Token: 0x040015A6 RID: 5542
		MouseX,
		// Token: 0x040015A7 RID: 5543
		MouseY
	}
}
