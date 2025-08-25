using System;
using UnityEngine;

// Token: 0x02000A3C RID: 2620
public class GpuGrabSphere : MonoBehaviour
{
	// Token: 0x06004387 RID: 17287 RVA: 0x0013CDFB File Offset: 0x0013B1FB
	public GpuGrabSphere()
	{
	}

	// Token: 0x170008A2 RID: 2210
	// (get) Token: 0x06004388 RID: 17288 RVA: 0x0013CE04 File Offset: 0x0013B204
	public float WorldRadius
	{
		get
		{
			return this.radius * base.transform.lossyScale.x;
		}
	}

	// Token: 0x06004389 RID: 17289 RVA: 0x0013CE2B File Offset: 0x0013B22B
	private void OnEnable()
	{
		if (Application.isPlaying)
		{
			this.frameCountdown = this.numFramesToGrabOnEnable;
			GPUCollidersManager.RegisterGrabSphere(this);
		}
	}

	// Token: 0x0600438A RID: 17290 RVA: 0x0013CE49 File Offset: 0x0013B249
	private void OnDisable()
	{
		if (Application.isPlaying)
		{
			GPUCollidersManager.DeregisterGrabSphere(this);
		}
	}

	// Token: 0x04003279 RID: 12921
	public float radius;

	// Token: 0x0400327A RID: 12922
	public int id;

	// Token: 0x0400327B RID: 12923
	public int enabledThisFrame;

	// Token: 0x0400327C RID: 12924
	public int numFramesToGrabOnEnable;

	// Token: 0x0400327D RID: 12925
	public int frameCountdown;
}
