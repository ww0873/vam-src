using System;
using UnityEngine;

// Token: 0x02000E12 RID: 3602
public class DelayFrameForTesting : MonoBehaviour
{
	// Token: 0x06006F08 RID: 28424 RVA: 0x0029A5A3 File Offset: 0x002989A3
	public DelayFrameForTesting()
	{
	}

	// Token: 0x06006F09 RID: 28425 RVA: 0x0029A5AC File Offset: 0x002989AC
	private void Update()
	{
		float num = 0f;
		for (int i = 0; i < this.delayCount; i++)
		{
			num += 0.001f;
		}
	}

	// Token: 0x04006007 RID: 24583
	public int delayCount;
}
