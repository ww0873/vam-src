using System;
using UnityEngine;

// Token: 0x02000754 RID: 1876
public class CenterTransformOnCam : MonoBehaviour
{
	// Token: 0x06003045 RID: 12357 RVA: 0x000FA1C2 File Offset: 0x000F85C2
	public CenterTransformOnCam()
	{
	}

	// Token: 0x06003046 RID: 12358 RVA: 0x000FA1CC File Offset: 0x000F85CC
	private void Update()
	{
		Vector3 position = new Vector3(this.Camera.position.x, this.Camera.position.y - 0.2f, this.Camera.position.z);
		base.transform.position = position;
	}

	// Token: 0x04002427 RID: 9255
	public Transform Camera;
}
