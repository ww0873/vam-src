using System;
using UnityEngine;

// Token: 0x020002F2 RID: 754
public class StarPoint
{
	// Token: 0x060011C5 RID: 4549 RVA: 0x00061F2D File Offset: 0x0006032D
	public StarPoint(Vector3 position, float noise, float xRotation, float yRotation)
	{
		this.position = position;
		this.noise = noise;
		this.xRotation = xRotation;
		this.yRotation = yRotation;
	}

	// Token: 0x04000F44 RID: 3908
	public Vector3 position;

	// Token: 0x04000F45 RID: 3909
	public float noise;

	// Token: 0x04000F46 RID: 3910
	public float xRotation;

	// Token: 0x04000F47 RID: 3911
	public float yRotation;
}
