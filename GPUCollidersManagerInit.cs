using System;
using UnityEngine;

// Token: 0x020009AF RID: 2479
public class GPUCollidersManagerInit : MonoBehaviour
{
	// Token: 0x06003ECE RID: 16078 RVA: 0x0012E249 File Offset: 0x0012C649
	public GPUCollidersManagerInit()
	{
	}

	// Token: 0x06003ECF RID: 16079 RVA: 0x0012E254 File Offset: 0x0012C654
	private void Awake()
	{
		GPUCollidersManager component = base.GetComponent<GPUCollidersManager>();
		if (component != null)
		{
			component.Init();
		}
	}
}
