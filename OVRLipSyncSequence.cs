using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020007DE RID: 2014
[Serializable]
public class OVRLipSyncSequence : ScriptableObject
{
	// Token: 0x060032FB RID: 13051 RVA: 0x00108D28 File Offset: 0x00107128
	public OVRLipSyncSequence()
	{
	}

	// Token: 0x060032FC RID: 13052 RVA: 0x00108D3C File Offset: 0x0010713C
	public OVRLipSync.Frame GetFrameAtTime(float time)
	{
		OVRLipSync.Frame result = null;
		if (time < this.length && this.entries.Count > 0)
		{
			float num = time / this.length;
			result = this.entries[(int)((float)this.entries.Count * num)];
		}
		return result;
	}

	// Token: 0x040026F8 RID: 9976
	public List<OVRLipSync.Frame> entries = new List<OVRLipSync.Frame>();

	// Token: 0x040026F9 RID: 9977
	public float length;
}
