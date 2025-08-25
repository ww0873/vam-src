using System;
using UnityEngine;

// Token: 0x02000B92 RID: 2962
public class EyeCenter : MonoBehaviour
{
	// Token: 0x06005373 RID: 21363 RVA: 0x001E3C8A File Offset: 0x001E208A
	public EyeCenter()
	{
	}

	// Token: 0x06005374 RID: 21364 RVA: 0x001E3C94 File Offset: 0x001E2094
	protected void Update()
	{
		if (this.leftEye != null && this.rightEye != null)
		{
			base.transform.position = (this.leftEye.position + this.rightEye.position) * 0.5f;
		}
	}

	// Token: 0x04004390 RID: 17296
	public Transform leftEye;

	// Token: 0x04004391 RID: 17297
	public Transform rightEye;
}
