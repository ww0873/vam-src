using System;
using UnityEngine;

// Token: 0x02000B82 RID: 2946
public class RhythmAudioSourceControl : AudioSourceControl
{
	// Token: 0x060052E0 RID: 21216 RVA: 0x001DF241 File Offset: 0x001DD641
	public RhythmAudioSourceControl()
	{
	}

	// Token: 0x060052E1 RID: 21217 RVA: 0x001DF24C File Offset: 0x001DD64C
	protected override void PlayClip(NamedAudioClip nac, bool loopClip = false)
	{
		if (this.rhythmController != null && this.audioSource != null && nac.clipToPlay != null)
		{
			base.loop = loopClip;
			this._playingClip = nac;
			this.timeSinceClipFinished = 0f;
			this.isPaused = false;
			this.rhythmController.StartSong(nac.clipToPlay);
			if (this.playingClipNameText != null)
			{
				this.playingClipNameText.text = this._playingClip.displayName;
			}
			if (this.playingClipNameTextAlt != null)
			{
				this.playingClipNameTextAlt.text = this._playingClip.displayName;
			}
		}
	}

	// Token: 0x060052E2 RID: 21218 RVA: 0x001DF30C File Offset: 0x001DD70C
	protected override void Awake()
	{
		if (!this.awakecalled)
		{
			base.Awake();
			if (this.audioSource != null && this.startClip != null)
			{
				this.rhythmController.StartSong(this.startClip);
			}
		}
	}

	// Token: 0x060052E3 RID: 21219 RVA: 0x001DF35D File Offset: 0x001DD75D
	protected override void OnEnable()
	{
	}

	// Token: 0x060052E4 RID: 21220 RVA: 0x001DF35F File Offset: 0x001DD75F
	protected override void Update()
	{
		base.Update();
	}

	// Token: 0x040042AB RID: 17067
	public RhythmController rhythmController;

	// Token: 0x040042AC RID: 17068
	public AudioClip startClip;
}
