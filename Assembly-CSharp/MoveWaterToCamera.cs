using System;
using UnityEngine;

// Token: 0x02000308 RID: 776
public class MoveWaterToCamera : MonoBehaviour
{
	// Token: 0x06001252 RID: 4690 RVA: 0x00065FDD File Offset: 0x000643DD
	public MoveWaterToCamera()
	{
	}

	// Token: 0x06001253 RID: 4691 RVA: 0x00065FE8 File Offset: 0x000643E8
	private void Update()
	{
		if (this.CurrenCamera == null)
		{
			return;
		}
		Vector3 position = base.transform.position;
		position.x = this.CurrenCamera.transform.position.x;
		position.z = this.CurrenCamera.transform.position.z;
		base.transform.position = position;
		Quaternion rotation = this.CurrenCamera.transform.rotation;
		rotation.eulerAngles = new Vector3(rotation.eulerAngles.x, 0f, rotation.eulerAngles.z);
	}

	// Token: 0x04000FB1 RID: 4017
	public GameObject CurrenCamera;
}
