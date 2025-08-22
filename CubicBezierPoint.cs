using System;
using UnityEngine;

// Token: 0x02000C8B RID: 3211
public class CubicBezierPoint : JSONStorable
{
	// Token: 0x060060F0 RID: 24816 RVA: 0x001D4BDF File Offset: 0x001D2FDF
	public CubicBezierPoint()
	{
	}

	// Token: 0x0400507C RID: 20604
	public Transform point;

	// Token: 0x0400507D RID: 20605
	public CubicBezierControlPoint controlPointIn;

	// Token: 0x0400507E RID: 20606
	public CubicBezierControlPoint controlPointOut;
}
