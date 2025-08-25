using System;
using UnityEngine;

// Token: 0x02000306 RID: 774
[ExecuteInEditMode]
public class DepthFix : MonoBehaviour
{
	// Token: 0x0600124E RID: 4686 RVA: 0x00065F9C File Offset: 0x0006439C
	public DepthFix()
	{
	}

	// Token: 0x0600124F RID: 4687 RVA: 0x00065FA4 File Offset: 0x000643A4
	private void OnWillRenderObject()
	{
		Camera.current.depthTextureMode |= DepthTextureMode.Depth;
	}
}
