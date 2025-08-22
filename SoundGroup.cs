using System;
using UnityEngine;
using UnityEngine.Audio;

// Token: 0x0200075E RID: 1886
[Serializable]
public class SoundGroup
{
	// Token: 0x0600307B RID: 12411 RVA: 0x000FC243 File Offset: 0x000FA643
	public SoundGroup(string name)
	{
		this.name = name;
	}

	// Token: 0x0600307C RID: 12412 RVA: 0x000FC274 File Offset: 0x000FA674
	public SoundGroup()
	{
		this.mixerGroup = null;
		this.maxPlayingSounds = 0;
		this.preloadAudio = PreloadSounds.Default;
		this.volumeOverride = 1f;
	}

	// Token: 0x0600307D RID: 12413 RVA: 0x000FC2CC File Offset: 0x000FA6CC
	public void IncrementPlayCount()
	{
		this.playingSoundCount = Mathf.Clamp(++this.playingSoundCount, 0, this.maxPlayingSounds);
	}

	// Token: 0x0600307E RID: 12414 RVA: 0x000FC2FC File Offset: 0x000FA6FC
	public void DecrementPlayCount()
	{
		this.playingSoundCount = Mathf.Clamp(--this.playingSoundCount, 0, this.maxPlayingSounds);
	}

	// Token: 0x0600307F RID: 12415 RVA: 0x000FC32C File Offset: 0x000FA72C
	public bool CanPlaySound()
	{
		return this.maxPlayingSounds == 0 || this.playingSoundCount < this.maxPlayingSounds;
	}

	// Token: 0x0400246E RID: 9326
	public string name = string.Empty;

	// Token: 0x0400246F RID: 9327
	public SoundFX[] soundList = new SoundFX[0];

	// Token: 0x04002470 RID: 9328
	public AudioMixerGroup mixerGroup;

	// Token: 0x04002471 RID: 9329
	[Range(0f, 64f)]
	public int maxPlayingSounds;

	// Token: 0x04002472 RID: 9330
	public PreloadSounds preloadAudio;

	// Token: 0x04002473 RID: 9331
	public float volumeOverride = 1f;

	// Token: 0x04002474 RID: 9332
	[HideInInspector]
	public int playingSoundCount;
}
