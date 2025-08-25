using System;
using UnityEngine;
using UnityEngine.Audio;

// Token: 0x0200076A RID: 1898
[Serializable]
public class SoundFX
{
	// Token: 0x060030E0 RID: 12512 RVA: 0x000FE740 File Offset: 0x000FCB40
	public SoundFX()
	{
		this.playback = SoundFXNext.Random;
		this.volume = 1f;
		this.pitchVariance = Vector2.one;
		this.falloffDistance = new Vector2(1f, 25f);
		this.falloffCurve = AudioRolloffMode.Linear;
		this.volumeFalloffCurve = new AnimationCurve(new Keyframe[]
		{
			new Keyframe(0f, 1f),
			new Keyframe(1f, 1f)
		});
		this.reverbZoneMix = new AnimationCurve(new Keyframe[]
		{
			new Keyframe(0f, 1f),
			new Keyframe(1f, 1f)
		});
		this.spread = 0f;
		this.pctChanceToPlay = 1f;
		this.priority = SoundPriority.Default;
		this.delay = Vector2.zero;
		this.looping = false;
		this.ospProps = new OSPProps();
	}

	// Token: 0x170005E0 RID: 1504
	// (get) Token: 0x060030E1 RID: 12513 RVA: 0x000FE95A File Offset: 0x000FCD5A
	public int Length
	{
		get
		{
			return this.soundClips.Length;
		}
	}

	// Token: 0x170005E1 RID: 1505
	// (get) Token: 0x060030E2 RID: 12514 RVA: 0x000FE964 File Offset: 0x000FCD64
	public bool IsValid
	{
		get
		{
			return this.soundClips.Length != 0 && this.soundClips[0] != null;
		}
	}

	// Token: 0x170005E2 RID: 1506
	// (get) Token: 0x060030E3 RID: 12515 RVA: 0x000FE984 File Offset: 0x000FCD84
	// (set) Token: 0x060030E4 RID: 12516 RVA: 0x000FE98C File Offset: 0x000FCD8C
	public SoundGroup Group
	{
		get
		{
			return this.soundGroup;
		}
		set
		{
			this.soundGroup = value;
		}
	}

	// Token: 0x170005E3 RID: 1507
	// (get) Token: 0x060030E5 RID: 12517 RVA: 0x000FE995 File Offset: 0x000FCD95
	public float MaxFalloffDistSquared
	{
		get
		{
			return this.falloffDistance.y * this.falloffDistance.y;
		}
	}

	// Token: 0x170005E4 RID: 1508
	// (get) Token: 0x060030E6 RID: 12518 RVA: 0x000FE9AE File Offset: 0x000FCDAE
	public float GroupVolumeOverride
	{
		get
		{
			return (this.soundGroup == null) ? 1f : this.soundGroup.volumeOverride;
		}
	}

	// Token: 0x060030E7 RID: 12519 RVA: 0x000FE9D0 File Offset: 0x000FCDD0
	public AudioClip GetClip()
	{
		if (this.soundClips.Length == 0)
		{
			return null;
		}
		if (this.soundClips.Length == 1)
		{
			return this.soundClips[0];
		}
		if (this.playback == SoundFXNext.Random)
		{
			int num;
			for (num = UnityEngine.Random.Range(0, this.soundClips.Length); num == this.lastIdx; num = UnityEngine.Random.Range(0, this.soundClips.Length))
			{
			}
			this.lastIdx = num;
			return this.soundClips[num];
		}
		if (++this.lastIdx >= this.soundClips.Length)
		{
			this.lastIdx = 0;
		}
		return this.soundClips[this.lastIdx];
	}

	// Token: 0x060030E8 RID: 12520 RVA: 0x000FEA7F File Offset: 0x000FCE7F
	public AudioMixerGroup GetMixerGroup(AudioMixerGroup defaultMixerGroup)
	{
		if (this.soundGroup != null)
		{
			return (!(this.soundGroup.mixerGroup != null)) ? defaultMixerGroup : this.soundGroup.mixerGroup;
		}
		return defaultMixerGroup;
	}

	// Token: 0x060030E9 RID: 12521 RVA: 0x000FEAB5 File Offset: 0x000FCEB5
	public bool ReachedGroupPlayLimit()
	{
		return this.soundGroup != null && !this.soundGroup.CanPlaySound();
	}

	// Token: 0x060030EA RID: 12522 RVA: 0x000FEAD4 File Offset: 0x000FCED4
	public float GetClipLength(int idx)
	{
		if (idx == -1 || this.soundClips.Length == 0 || idx >= this.soundClips.Length || this.soundClips[idx] == null)
		{
			return 0f;
		}
		return this.soundClips[idx].length;
	}

	// Token: 0x060030EB RID: 12523 RVA: 0x000FEB29 File Offset: 0x000FCF29
	public float GetPitch()
	{
		return UnityEngine.Random.Range(this.pitchVariance.x, this.pitchVariance.y);
	}

	// Token: 0x060030EC RID: 12524 RVA: 0x000FEB48 File Offset: 0x000FCF48
	public int PlaySound(float delaySecs = 0f)
	{
		this.playingIdx = -1;
		if (!this.IsValid)
		{
			return this.playingIdx;
		}
		if (this.pctChanceToPlay > 0.99f || UnityEngine.Random.value < this.pctChanceToPlay)
		{
			if (this.delay.y > 0f)
			{
				delaySecs = UnityEngine.Random.Range(this.delay.x, this.delay.y);
			}
			this.playingIdx = AudioManager.PlaySound(this, EmitterChannel.Any, delaySecs);
		}
		return this.playingIdx;
	}

	// Token: 0x060030ED RID: 12525 RVA: 0x000FEBD4 File Offset: 0x000FCFD4
	public int PlaySoundAt(Vector3 pos, float delaySecs = 0f, float volumeOverride = 1f, float pitchMultiplier = 1f)
	{
		this.playingIdx = -1;
		if (!this.IsValid)
		{
			return this.playingIdx;
		}
		if (this.pctChanceToPlay > 0.99f || UnityEngine.Random.value < this.pctChanceToPlay)
		{
			if (this.delay.y > 0f)
			{
				delaySecs = UnityEngine.Random.Range(this.delay.x, this.delay.y);
			}
			this.playingIdx = AudioManager.PlaySoundAt(pos, this, EmitterChannel.Any, delaySecs, volumeOverride, pitchMultiplier);
		}
		return this.playingIdx;
	}

	// Token: 0x060030EE RID: 12526 RVA: 0x000FEC64 File Offset: 0x000FD064
	public void SetOnFinished(Action onFinished)
	{
		if (this.playingIdx > -1)
		{
			AudioManager.SetOnFinished(this.playingIdx, onFinished);
		}
	}

	// Token: 0x060030EF RID: 12527 RVA: 0x000FEC7E File Offset: 0x000FD07E
	public void SetOnFinished(Action<object> onFinished, object obj)
	{
		if (this.playingIdx > -1)
		{
			AudioManager.SetOnFinished(this.playingIdx, onFinished, obj);
		}
	}

	// Token: 0x060030F0 RID: 12528 RVA: 0x000FEC9C File Offset: 0x000FD09C
	public bool StopSound()
	{
		bool result = false;
		if (this.playingIdx > -1)
		{
			result = AudioManager.StopSound(this.playingIdx, true, false);
			this.playingIdx = -1;
		}
		return result;
	}

	// Token: 0x060030F1 RID: 12529 RVA: 0x000FECCD File Offset: 0x000FD0CD
	public void AttachToParent(Transform parent)
	{
		if (this.playingIdx > -1)
		{
			AudioManager.AttachSoundToParent(this.playingIdx, parent);
		}
	}

	// Token: 0x060030F2 RID: 12530 RVA: 0x000FECE7 File Offset: 0x000FD0E7
	public void DetachFromParent()
	{
		if (this.playingIdx > -1)
		{
			AudioManager.DetachSoundFromParent(this.playingIdx);
		}
	}

	// Token: 0x040024C9 RID: 9417
	[Tooltip("Each sound FX should have a unique name")]
	public string name = string.Empty;

	// Token: 0x040024CA RID: 9418
	[Tooltip("Sound diversity playback option when multiple audio clips are defined, default = Random")]
	public SoundFXNext playback;

	// Token: 0x040024CB RID: 9419
	[Tooltip("Default volume for this sound FX, default = 1.0")]
	[Range(0f, 1f)]
	public float volume = 1f;

	// Token: 0x040024CC RID: 9420
	[Tooltip("Random pitch variance each time a sound FX is played, default = 1.0 (none)")]
	[MinMax(1f, 1f, 0f, 2f)]
	public Vector2 pitchVariance = Vector2.one;

	// Token: 0x040024CD RID: 9421
	[Tooltip("Falloff distance for the sound FX, default = 1m min to 25m max")]
	[MinMax(1f, 25f, 0f, 250f)]
	public Vector2 falloffDistance = new Vector2(1f, 25f);

	// Token: 0x040024CE RID: 9422
	[Tooltip("Volume falloff curve - sets how the sound FX attenuates over distance, default = Linear")]
	public AudioRolloffMode falloffCurve = AudioRolloffMode.Linear;

	// Token: 0x040024CF RID: 9423
	[Tooltip("Defines the custom volume falloff curve")]
	public AnimationCurve volumeFalloffCurve = new AnimationCurve(new Keyframe[]
	{
		new Keyframe(0f, 1f),
		new Keyframe(1f, 1f)
	});

	// Token: 0x040024D0 RID: 9424
	[Tooltip("The amount by which the signal from the AudioSource will be mixed into the global reverb associated with the Reverb Zones | Valid range is 0.0 - 1.1, default = 1.0")]
	public AnimationCurve reverbZoneMix = new AnimationCurve(new Keyframe[]
	{
		new Keyframe(0f, 1f),
		new Keyframe(1f, 1f)
	});

	// Token: 0x040024D1 RID: 9425
	[Tooltip("Sets the spread angle (in degrees) of a 3d stereo or multichannel sound in speaker space, default = 0")]
	[Range(0f, 360f)]
	public float spread;

	// Token: 0x040024D2 RID: 9426
	[Tooltip("The percentage chance that this sound FX will play | 0.0 = none, 1.0 = 100%, default = 1.0")]
	[Range(0f, 1f)]
	public float pctChanceToPlay = 1f;

	// Token: 0x040024D3 RID: 9427
	[Tooltip("Sets the priority for this sound to play and/or to override a currently playing sound FX, default = Default")]
	public SoundPriority priority;

	// Token: 0x040024D4 RID: 9428
	[Tooltip("Specifies the default delay when this sound FX is played, default = 0.0 secs")]
	[MinMax(0f, 0f, 0f, 2f)]
	public Vector2 delay = Vector2.zero;

	// Token: 0x040024D5 RID: 9429
	[Tooltip("Set to true for the sound to loop continuously, default = false")]
	public bool looping;

	// Token: 0x040024D6 RID: 9430
	public OSPProps ospProps = new OSPProps();

	// Token: 0x040024D7 RID: 9431
	[Tooltip("List of the audio clips assigned to this sound FX")]
	public AudioClip[] soundClips = new AudioClip[1];

	// Token: 0x040024D8 RID: 9432
	public bool visibilityToggle;

	// Token: 0x040024D9 RID: 9433
	[NonSerialized]
	private SoundGroup soundGroup;

	// Token: 0x040024DA RID: 9434
	private int lastIdx = -1;

	// Token: 0x040024DB RID: 9435
	private int playingIdx = -1;
}
