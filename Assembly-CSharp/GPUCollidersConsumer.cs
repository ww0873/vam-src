using System;
using UnityEngine;

// Token: 0x020009AD RID: 2477
public class GPUCollidersConsumer : MonoBehaviour
{
	// Token: 0x06003E6E RID: 15982 RVA: 0x00126BE9 File Offset: 0x00124FE9
	public GPUCollidersConsumer()
	{
	}

	// Token: 0x06003E6F RID: 15983 RVA: 0x00126BF1 File Offset: 0x00124FF1
	protected virtual void OnEnable()
	{
		if (Application.isPlaying)
		{
			GPUCollidersManager.RegisterConsumer(this);
		}
	}

	// Token: 0x06003E70 RID: 15984 RVA: 0x00126C03 File Offset: 0x00125003
	protected virtual void OnDisable()
	{
		if (Application.isPlaying)
		{
			GPUCollidersManager.DeregisterConsumer(this);
		}
	}
}
