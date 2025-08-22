using System;
using UnityEngine;

// Token: 0x02000DC9 RID: 3529
public class IgnoreRaycast : MonoBehaviour, ICanvasRaycastFilter
{
	// Token: 0x06006D6D RID: 28013 RVA: 0x00293109 File Offset: 0x00291509
	public IgnoreRaycast()
	{
	}

	// Token: 0x06006D6E RID: 28014 RVA: 0x00293111 File Offset: 0x00291511
	public bool IsRaycastLocationValid(Vector2 sp, Camera eventCamera)
	{
		return false;
	}
}
