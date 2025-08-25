using System;
using UnityEngine;

// Token: 0x020002F0 RID: 752
public class MoonManager : MonoBehaviour
{
	// Token: 0x060011B3 RID: 4531 RVA: 0x00061CA1 File Offset: 0x000600A1
	public MoonManager()
	{
	}

	// Token: 0x060011B4 RID: 4532 RVA: 0x00061CA9 File Offset: 0x000600A9
	private void Update()
	{
	}

	// Token: 0x060011B5 RID: 4533 RVA: 0x00061CAB File Offset: 0x000600AB
	public void UpdateMoonData()
	{
	}

	// Token: 0x04000F40 RID: 3904
	[Range(0f, 1f)]
	public float moonHeight;

	// Token: 0x04000F41 RID: 3905
	[Range(0f, 1f)]
	public float moonRotation;

	// Token: 0x04000F42 RID: 3906
	private float currentMoonHeight;

	// Token: 0x04000F43 RID: 3907
	private float currentMoonRotation;
}
