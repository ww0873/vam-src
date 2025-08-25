using System;
using UnityEngine.Audio;

// Token: 0x02000761 RID: 1889
[Serializable]
public class MixerSnapshot
{
	// Token: 0x060030B4 RID: 12468 RVA: 0x000FDCCC File Offset: 0x000FC0CC
	public MixerSnapshot()
	{
	}

	// Token: 0x04002499 RID: 9369
	public AudioMixerSnapshot snapshot;

	// Token: 0x0400249A RID: 9370
	public float transitionTime = 0.25f;
}
