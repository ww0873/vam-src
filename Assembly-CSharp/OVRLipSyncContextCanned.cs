using System;
using UnityEngine;

// Token: 0x020007D9 RID: 2009
[RequireComponent(typeof(AudioSource))]
public class OVRLipSyncContextCanned : OVRLipSyncContextBase
{
	// Token: 0x060032DC RID: 13020 RVA: 0x0010844D File Offset: 0x0010684D
	public OVRLipSyncContextCanned()
	{
	}

	// Token: 0x060032DD RID: 13021 RVA: 0x00108458 File Offset: 0x00106858
	private void Update()
	{
		if (this.audioSource.isPlaying && this.currentSequence != null)
		{
			OVRLipSync.Frame frameAtTime = this.currentSequence.GetFrameAtTime(this.audioSource.time);
			base.Frame.CopyInput(frameAtTime);
		}
	}

	// Token: 0x040026DC RID: 9948
	public OVRLipSyncSequence currentSequence;
}
