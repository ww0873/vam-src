using System;
using UnityEngine;

// Token: 0x02000013 RID: 19
[AddComponentMenu("AQUAS/AQUAS Camera")]
[RequireComponent(typeof(Camera))]
public class AQUAS_Camera : MonoBehaviour
{
	// Token: 0x060000A8 RID: 168 RVA: 0x00005CA8 File Offset: 0x000040A8
	public AQUAS_Camera()
	{
	}

	// Token: 0x060000A9 RID: 169 RVA: 0x00005CB0 File Offset: 0x000040B0
	private void Start()
	{
		this.Set();
	}

	// Token: 0x060000AA RID: 170 RVA: 0x00005CB8 File Offset: 0x000040B8
	private void Set()
	{
		if (base.GetComponent<Camera>().depthTextureMode == DepthTextureMode.None)
		{
			base.GetComponent<Camera>().depthTextureMode = DepthTextureMode.Depth;
		}
	}
}
