using System;
using UnityEngine;

// Token: 0x02000763 RID: 1891
public class MinMaxAttribute : PropertyAttribute
{
	// Token: 0x060030B5 RID: 12469 RVA: 0x000FDCE0 File Offset: 0x000FC0E0
	public MinMaxAttribute(float minDefaultVal, float maxDefaultVal, float min, float max)
	{
		this.minDefaultVal = minDefaultVal;
		this.maxDefaultVal = maxDefaultVal;
		this.min = min;
		this.max = max;
	}

	// Token: 0x0400249E RID: 9374
	public float minDefaultVal = 1f;

	// Token: 0x0400249F RID: 9375
	public float maxDefaultVal = 1f;

	// Token: 0x040024A0 RID: 9376
	public float min;

	// Token: 0x040024A1 RID: 9377
	public float max = 1f;
}
