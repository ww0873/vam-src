using System;
using UnityEngine;

// Token: 0x02000004 RID: 4
public class IconSpin : MonoBehaviour
{
	// Token: 0x06000038 RID: 56 RVA: 0x00002B9A File Offset: 0x00000F9A
	public IconSpin()
	{
	}

	// Token: 0x06000039 RID: 57 RVA: 0x00002BAD File Offset: 0x00000FAD
	private void Start()
	{
	}

	// Token: 0x0600003A RID: 58 RVA: 0x00002BB0 File Offset: 0x00000FB0
	private void Update()
	{
		Vector3 localEulerAngles = base.transform.localEulerAngles;
		localEulerAngles.y += this.mRotationSpeed;
		base.transform.localEulerAngles = localEulerAngles;
	}

	// Token: 0x04000028 RID: 40
	[Range(-10f, 10f)]
	public float mRotationSpeed = 0.5f;
}
