using System;
using UnityEngine;

// Token: 0x0200001F RID: 31
[ExecuteInEditMode]
[AddComponentMenu("AQUAS/Render Queue Controller")]
public class AQUAS_RenderQueueEditor : MonoBehaviour
{
	// Token: 0x060000CC RID: 204 RVA: 0x00008129 File Offset: 0x00006529
	public AQUAS_RenderQueueEditor()
	{
	}

	// Token: 0x060000CD RID: 205 RVA: 0x00008138 File Offset: 0x00006538
	private void Update()
	{
		base.gameObject.GetComponent<Renderer>().sharedMaterial.renderQueue = this.renderQueueIndex;
	}

	// Token: 0x0400010B RID: 267
	public int renderQueueIndex = -1;
}
