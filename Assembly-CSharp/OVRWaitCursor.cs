using System;
using UnityEngine;

// Token: 0x02000979 RID: 2425
public class OVRWaitCursor : MonoBehaviour
{
	// Token: 0x06003C84 RID: 15492 RVA: 0x001253A8 File Offset: 0x001237A8
	public OVRWaitCursor()
	{
	}

	// Token: 0x06003C85 RID: 15493 RVA: 0x001253CA File Offset: 0x001237CA
	private void Update()
	{
		base.transform.Rotate(this.rotateSpeeds * Time.smoothDeltaTime);
	}

	// Token: 0x04002E69 RID: 11881
	public Vector3 rotateSpeeds = new Vector3(0f, 0f, -60f);
}
