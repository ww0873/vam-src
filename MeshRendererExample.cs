using System;
using UnityEngine;

// Token: 0x02000006 RID: 6
public class MeshRendererExample : MonoBehaviour
{
	// Token: 0x06000042 RID: 66 RVA: 0x00002D3A File Offset: 0x0000113A
	public MeshRendererExample()
	{
	}

	// Token: 0x06000043 RID: 67 RVA: 0x00002D50 File Offset: 0x00001150
	private void Update()
	{
		base.transform.Rotate(new Vector3(0f, this.dir, 0f) * 50f * Time.deltaTime);
		if ((double)base.transform.rotation.y > 0.8)
		{
			this.dir = -1f;
		}
		if ((double)base.transform.rotation.y < 0.3)
		{
			this.dir = 1f;
		}
	}

	// Token: 0x0400002C RID: 44
	private float dir = 1f;
}
