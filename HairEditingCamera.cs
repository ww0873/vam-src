using System;
using UnityEngine;

// Token: 0x020009EB RID: 2539
public class HairEditingCamera : MonoBehaviour
{
	// Token: 0x06003FEE RID: 16366 RVA: 0x00130BED File Offset: 0x0012EFED
	public HairEditingCamera()
	{
	}

	// Token: 0x06003FEF RID: 16367 RVA: 0x00130C00 File Offset: 0x0012F000
	private void Update()
	{
		if (Input.GetMouseButton(0))
		{
			float y = this.yawT.localEulerAngles.y + Input.GetAxis("Mouse X") * Time.deltaTime * this.speed;
			this.yawT.localEulerAngles = new Vector3(0f, y, 0f);
			float x = this.pitchT.localEulerAngles.x + Input.GetAxis("Mouse Y") * Time.deltaTime * this.speed;
			this.pitchT.localEulerAngles = new Vector3(x, 0f, 0f);
		}
		Rect rect = new Rect(0f, 0f, (float)Screen.width, (float)Screen.height);
		if (!rect.Contains(Input.mousePosition))
		{
			return;
		}
		base.transform.Translate(Vector3.forward * Input.GetAxis("Mouse ScrollWheel") * this.speed * 0.02f);
	}

	// Token: 0x0400304B RID: 12363
	public Transform yawT;

	// Token: 0x0400304C RID: 12364
	public Transform pitchT;

	// Token: 0x0400304D RID: 12365
	public float speed = 100f;
}
