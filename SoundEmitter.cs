using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Audio;

// Token: 0x02000764 RID: 1892
public class SoundEmitter : MonoBehaviour
{
	// Token: 0x060030B6 RID: 12470 RVA: 0x000FDD31 File Offset: 0x000FC131
	public SoundEmitter()
	{
	}

	// Token: 0x170005D7 RID: 1495
	// (get) Token: 0x060030B7 RID: 12471 RVA: 0x000FDD4B File Offset: 0x000FC14B
	// (set) Token: 0x060030B8 RID: 12472 RVA: 0x000FDD58 File Offset: 0x000FC158
	public float volume
	{
		get
		{
			return this.audioSource.volume;
		}
		set
		{
			this.audioSource.volume = value;
		}
	}

	// Token: 0x170005D8 RID: 1496
	// (get) Token: 0x060030B9 RID: 12473 RVA: 0x000FDD66 File Offset: 0x000FC166
	// (set) Token: 0x060030BA RID: 12474 RVA: 0x000FDD73 File Offset: 0x000FC173
	public float pitch
	{
		get
		{
			return this.audioSource.pitch;
		}
		set
		{
			this.audioSource.pitch = value;
		}
	}

	// Token: 0x170005D9 RID: 1497
	// (get) Token: 0x060030BB RID: 12475 RVA: 0x000FDD81 File Offset: 0x000FC181
	// (set) Token: 0x060030BC RID: 12476 RVA: 0x000FDD8E File Offset: 0x000FC18E
	public AudioClip clip
	{
		get
		{
			return this.audioSource.clip;
		}
		set
		{
			this.audioSource.clip = value;
		}
	}

	// Token: 0x170005DA RID: 1498
	// (get) Token: 0x060030BD RID: 12477 RVA: 0x000FDD9C File Offset: 0x000FC19C
	// (set) Token: 0x060030BE RID: 12478 RVA: 0x000FDDA9 File Offset: 0x000FC1A9
	public float time
	{
		get
		{
			return this.audioSource.time;
		}
		set
		{
			this.audioSource.time = value;
		}
	}

	// Token: 0x170005DB RID: 1499
	// (get) Token: 0x060030BF RID: 12479 RVA: 0x000FDDB7 File Offset: 0x000FC1B7
	public float length
	{
		get
		{
			return (!(this.audioSource.clip != null)) ? 0f : this.audioSource.clip.length;
		}
	}

	// Token: 0x170005DC RID: 1500
	// (get) Token: 0x060030C0 RID: 12480 RVA: 0x000FDDE9 File Offset: 0x000FC1E9
	// (set) Token: 0x060030C1 RID: 12481 RVA: 0x000FDDF6 File Offset: 0x000FC1F6
	public bool loop
	{
		get
		{
			return this.audioSource.loop;
		}
		set
		{
			this.audioSource.loop = value;
		}
	}

	// Token: 0x170005DD RID: 1501
	// (get) Token: 0x060030C2 RID: 12482 RVA: 0x000FDE04 File Offset: 0x000FC204
	// (set) Token: 0x060030C3 RID: 12483 RVA: 0x000FDE11 File Offset: 0x000FC211
	public bool mute
	{
		get
		{
			return this.audioSource.mute;
		}
		set
		{
			this.audioSource.mute = value;
		}
	}

	// Token: 0x170005DE RID: 1502
	// (get) Token: 0x060030C4 RID: 12484 RVA: 0x000FDE1F File Offset: 0x000FC21F
	// (set) Token: 0x060030C5 RID: 12485 RVA: 0x000FDE2C File Offset: 0x000FC22C
	public AudioVelocityUpdateMode velocityUpdateMode
	{
		get
		{
			return this.audioSource.velocityUpdateMode;
		}
		set
		{
			this.audioSource.velocityUpdateMode = value;
		}
	}

	// Token: 0x170005DF RID: 1503
	// (get) Token: 0x060030C6 RID: 12486 RVA: 0x000FDE3A File Offset: 0x000FC23A
	public bool isPlaying
	{
		get
		{
			return this.audioSource.isPlaying;
		}
	}

	// Token: 0x060030C7 RID: 12487 RVA: 0x000FDE48 File Offset: 0x000FC248
	private void Awake()
	{
		this.audioSource = base.GetComponent<AudioSource>();
		if (this.audioSource == null)
		{
			this.audioSource = base.gameObject.AddComponent<AudioSource>();
		}
		if (AudioManager.enableSpatialization && !this.disableSpatialization)
		{
			this.osp = base.GetComponent<ONSPAudioSource>();
			if (this.osp == null)
			{
				this.osp = base.gameObject.AddComponent<ONSPAudioSource>();
			}
		}
		this.audioSource.playOnAwake = false;
		this.audioSource.Stop();
	}

	// Token: 0x060030C8 RID: 12488 RVA: 0x000FDEDD File Offset: 0x000FC2DD
	public void SetPlayingSoundGroup(SoundGroup soundGroup)
	{
		this.playingSoundGroup = soundGroup;
		if (soundGroup != null)
		{
			soundGroup.IncrementPlayCount();
		}
	}

	// Token: 0x060030C9 RID: 12489 RVA: 0x000FDEF2 File Offset: 0x000FC2F2
	public void SetOnFinished(Action onFinished)
	{
		this.onFinished = onFinished;
	}

	// Token: 0x060030CA RID: 12490 RVA: 0x000FDEFB File Offset: 0x000FC2FB
	public void SetOnFinished(Action<object> onFinished, object obj)
	{
		this.onFinishedObject = onFinished;
		this.onFinishedParam = obj;
	}

	// Token: 0x060030CB RID: 12491 RVA: 0x000FDF0B File Offset: 0x000FC30B
	public void SetChannel(int _channel)
	{
		this.channel = (EmitterChannel)_channel;
	}

	// Token: 0x060030CC RID: 12492 RVA: 0x000FDF14 File Offset: 0x000FC314
	public void SetDefaultParent(Transform parent)
	{
		this.defaultParent = parent;
	}

	// Token: 0x060030CD RID: 12493 RVA: 0x000FDF1D File Offset: 0x000FC31D
	public void SetAudioMixer(AudioMixerGroup _mixer)
	{
		if (this.audioSource != null)
		{
			this.audioSource.outputAudioMixerGroup = _mixer;
		}
	}

	// Token: 0x060030CE RID: 12494 RVA: 0x000FDF3C File Offset: 0x000FC33C
	public bool IsPlaying()
	{
		return (this.loop && this.audioSource.isPlaying) || this.endPlayTime > Time.time;
	}

	// Token: 0x060030CF RID: 12495 RVA: 0x000FDF68 File Offset: 0x000FC368
	public void Play()
	{
		this.state = SoundEmitter.FadeState.Null;
		this.endPlayTime = Time.time + this.length;
		base.StopAllCoroutines();
		this.audioSource.Play();
	}

	// Token: 0x060030D0 RID: 12496 RVA: 0x000FDF94 File Offset: 0x000FC394
	public void Pause()
	{
		this.state = SoundEmitter.FadeState.Null;
		base.StopAllCoroutines();
		this.audioSource.Pause();
	}

	// Token: 0x060030D1 RID: 12497 RVA: 0x000FDFB0 File Offset: 0x000FC3B0
	public void Stop()
	{
		this.state = SoundEmitter.FadeState.Null;
		base.StopAllCoroutines();
		if (this.audioSource != null)
		{
			this.audioSource.Stop();
		}
		if (this.onFinished != null)
		{
			this.onFinished();
			this.onFinished = null;
		}
		if (this.onFinishedObject != null)
		{
			this.onFinishedObject(this.onFinishedParam);
			this.onFinishedObject = null;
		}
		if (this.playingSoundGroup != null)
		{
			this.playingSoundGroup.DecrementPlayCount();
			this.playingSoundGroup = null;
		}
	}

	// Token: 0x060030D2 RID: 12498 RVA: 0x000FE043 File Offset: 0x000FC443
	private int GetSampleTime()
	{
		return this.audioSource.clip.samples - this.audioSource.timeSamples;
	}

	// Token: 0x060030D3 RID: 12499 RVA: 0x000FE061 File Offset: 0x000FC461
	public void ParentTo(Transform parent)
	{
		if (this.lastParentTransform != null)
		{
			UnityEngine.Debug.LogError("[SoundEmitter] You must detach the sound emitter before parenting to another object!");
			return;
		}
		this.lastParentTransform = base.transform.parent;
		base.transform.parent = parent;
	}

	// Token: 0x060030D4 RID: 12500 RVA: 0x000FE09C File Offset: 0x000FC49C
	public void DetachFromParent()
	{
		if (this.lastParentTransform == null)
		{
			base.transform.parent = this.defaultParent;
			return;
		}
		base.transform.parent = this.lastParentTransform;
		this.lastParentTransform = null;
	}

	// Token: 0x060030D5 RID: 12501 RVA: 0x000FE0D9 File Offset: 0x000FC4D9
	public void ResetParent(Transform parent)
	{
		base.transform.parent = parent;
		this.lastParentTransform = null;
	}

	// Token: 0x060030D6 RID: 12502 RVA: 0x000FE0EE File Offset: 0x000FC4EE
	public void SyncTo(SoundEmitter other, float fadeTime, float toVolume)
	{
		base.StartCoroutine(this.DelayedSyncTo(other, fadeTime, toVolume));
	}

	// Token: 0x060030D7 RID: 12503 RVA: 0x000FE100 File Offset: 0x000FC500
	private IEnumerator DelayedSyncTo(SoundEmitter other, float fadeTime, float toVolume)
	{
		yield return new WaitForEndOfFrame();
		this.audioSource.time = other.time;
		this.audioSource.Play();
		this.FadeTo(fadeTime, toVolume);
		yield break;
	}

	// Token: 0x060030D8 RID: 12504 RVA: 0x000FE130 File Offset: 0x000FC530
	public void FadeTo(float fadeTime, float toVolume)
	{
		if (this.state == SoundEmitter.FadeState.FadingOut)
		{
			return;
		}
		this.state = SoundEmitter.FadeState.Ducking;
		base.StopAllCoroutines();
		base.StartCoroutine(this.FadeSoundChannelTo(fadeTime, toVolume));
	}

	// Token: 0x060030D9 RID: 12505 RVA: 0x000FE15B File Offset: 0x000FC55B
	public void FadeIn(float fadeTime, float defaultVolume)
	{
		this.audioSource.volume = 0f;
		this.state = SoundEmitter.FadeState.FadingIn;
		base.StopAllCoroutines();
		base.StartCoroutine(this.FadeSoundChannel(0f, fadeTime, Fade.In, defaultVolume));
	}

	// Token: 0x060030DA RID: 12506 RVA: 0x000FE18F File Offset: 0x000FC58F
	public void FadeIn(float fadeTime)
	{
		this.audioSource.volume = 0f;
		this.state = SoundEmitter.FadeState.FadingIn;
		base.StopAllCoroutines();
		base.StartCoroutine(this.FadeSoundChannel(0f, fadeTime, Fade.In, this.defaultVolume));
	}

	// Token: 0x060030DB RID: 12507 RVA: 0x000FE1C8 File Offset: 0x000FC5C8
	public void FadeOut(float fadeTime)
	{
		if (!this.audioSource.isPlaying)
		{
			return;
		}
		this.state = SoundEmitter.FadeState.FadingOut;
		base.StopAllCoroutines();
		base.StartCoroutine(this.FadeSoundChannel(0f, fadeTime, Fade.Out, this.audioSource.volume));
	}

	// Token: 0x060030DC RID: 12508 RVA: 0x000FE207 File Offset: 0x000FC607
	public void FadeOutDelayed(float delayedSecs, float fadeTime)
	{
		if (!this.audioSource.isPlaying)
		{
			return;
		}
		this.state = SoundEmitter.FadeState.FadingOut;
		base.StopAllCoroutines();
		base.StartCoroutine(this.FadeSoundChannel(delayedSecs, fadeTime, Fade.Out, this.audioSource.volume));
	}

	// Token: 0x060030DD RID: 12509 RVA: 0x000FE244 File Offset: 0x000FC644
	private IEnumerator FadeSoundChannelTo(float fadeTime, float toVolume)
	{
		float start = this.audioSource.volume;
		float startTime = Time.realtimeSinceStartup;
		float elapsedTime = 0f;
		while (elapsedTime < fadeTime)
		{
			elapsedTime = Time.realtimeSinceStartup - startTime;
			float t = elapsedTime / fadeTime;
			this.audioSource.volume = Mathf.Lerp(start, toVolume, t);
			yield return 0;
		}
		this.state = SoundEmitter.FadeState.Null;
		yield break;
	}

	// Token: 0x060030DE RID: 12510 RVA: 0x000FE270 File Offset: 0x000FC670
	private IEnumerator FadeSoundChannel(float delaySecs, float fadeTime, Fade fadeType, float defaultVolume)
	{
		if (delaySecs > 0f)
		{
			yield return new WaitForSeconds(delaySecs);
		}
		float start = (fadeType != Fade.In) ? defaultVolume : 0f;
		float end = (fadeType != Fade.In) ? 0f : defaultVolume;
		bool restartPlay = false;
		if (fadeType == Fade.In)
		{
			if (Time.time == 0f)
			{
				restartPlay = true;
			}
			this.audioSource.volume = 0f;
			this.audioSource.Play();
		}
		float startTime = Time.realtimeSinceStartup;
		float elapsedTime = 0f;
		while (elapsedTime < fadeTime)
		{
			elapsedTime = Time.realtimeSinceStartup - startTime;
			float t = elapsedTime / fadeTime;
			this.audioSource.volume = Mathf.Lerp(start, end, t);
			yield return 0;
			if (restartPlay && Time.time > 0f)
			{
				this.audioSource.Play();
				restartPlay = false;
			}
			if (!this.audioSource.isPlaying)
			{
				break;
			}
		}
		if (fadeType == Fade.Out)
		{
			this.Stop();
		}
		this.state = SoundEmitter.FadeState.Null;
		yield break;
	}

	// Token: 0x040024A2 RID: 9378
	public EmitterChannel channel;

	// Token: 0x040024A3 RID: 9379
	public bool disableSpatialization;

	// Token: 0x040024A4 RID: 9380
	private SoundEmitter.FadeState state;

	// Token: 0x040024A5 RID: 9381
	[HideInInspector]
	[NonSerialized]
	public AudioSource audioSource;

	// Token: 0x040024A6 RID: 9382
	[HideInInspector]
	[NonSerialized]
	public SoundPriority priority;

	// Token: 0x040024A7 RID: 9383
	[HideInInspector]
	[NonSerialized]
	public ONSPAudioSource osp;

	// Token: 0x040024A8 RID: 9384
	[HideInInspector]
	[NonSerialized]
	public float endPlayTime;

	// Token: 0x040024A9 RID: 9385
	private Transform lastParentTransform;

	// Token: 0x040024AA RID: 9386
	[HideInInspector]
	[NonSerialized]
	public float defaultVolume = 1f;

	// Token: 0x040024AB RID: 9387
	[HideInInspector]
	[NonSerialized]
	public Transform defaultParent;

	// Token: 0x040024AC RID: 9388
	[HideInInspector]
	[NonSerialized]
	public int originalIdx = -1;

	// Token: 0x040024AD RID: 9389
	[HideInInspector]
	[NonSerialized]
	public Action onFinished;

	// Token: 0x040024AE RID: 9390
	[HideInInspector]
	[NonSerialized]
	public Action<object> onFinishedObject;

	// Token: 0x040024AF RID: 9391
	[HideInInspector]
	[NonSerialized]
	public object onFinishedParam;

	// Token: 0x040024B0 RID: 9392
	[HideInInspector]
	[NonSerialized]
	public SoundGroup playingSoundGroup;

	// Token: 0x02000765 RID: 1893
	public enum FadeState
	{
		// Token: 0x040024B2 RID: 9394
		Null,
		// Token: 0x040024B3 RID: 9395
		FadingIn,
		// Token: 0x040024B4 RID: 9396
		FadingOut,
		// Token: 0x040024B5 RID: 9397
		Ducking
	}

	// Token: 0x02000FB6 RID: 4022
	[CompilerGenerated]
	private sealed class <DelayedSyncTo>c__Iterator0 : IEnumerator, IDisposable, IEnumerator<object>
	{
		// Token: 0x060074DA RID: 29914 RVA: 0x000FE2A8 File Offset: 0x000FC6A8
		[DebuggerHidden]
		public <DelayedSyncTo>c__Iterator0()
		{
		}

		// Token: 0x060074DB RID: 29915 RVA: 0x000FE2B0 File Offset: 0x000FC6B0
		public bool MoveNext()
		{
			uint num = (uint)this.$PC;
			this.$PC = -1;
			switch (num)
			{
			case 0U:
				this.$current = new WaitForEndOfFrame();
				if (!this.$disposing)
				{
					this.$PC = 1;
				}
				return true;
			case 1U:
				this.audioSource.time = other.time;
				this.audioSource.Play();
				base.FadeTo(fadeTime, toVolume);
				this.$PC = -1;
				break;
			}
			return false;
		}

		// Token: 0x17001139 RID: 4409
		// (get) Token: 0x060074DC RID: 29916 RVA: 0x000FE349 File Offset: 0x000FC749
		object IEnumerator<object>.Current
		{
			[DebuggerHidden]
			get
			{
				return this.$current;
			}
		}

		// Token: 0x1700113A RID: 4410
		// (get) Token: 0x060074DD RID: 29917 RVA: 0x000FE351 File Offset: 0x000FC751
		object IEnumerator.Current
		{
			[DebuggerHidden]
			get
			{
				return this.$current;
			}
		}

		// Token: 0x060074DE RID: 29918 RVA: 0x000FE359 File Offset: 0x000FC759
		[DebuggerHidden]
		public void Dispose()
		{
			this.$disposing = true;
			this.$PC = -1;
		}

		// Token: 0x060074DF RID: 29919 RVA: 0x000FE369 File Offset: 0x000FC769
		[DebuggerHidden]
		public void Reset()
		{
			throw new NotSupportedException();
		}

		// Token: 0x040068E3 RID: 26851
		internal SoundEmitter other;

		// Token: 0x040068E4 RID: 26852
		internal float fadeTime;

		// Token: 0x040068E5 RID: 26853
		internal float toVolume;

		// Token: 0x040068E6 RID: 26854
		internal SoundEmitter $this;

		// Token: 0x040068E7 RID: 26855
		internal object $current;

		// Token: 0x040068E8 RID: 26856
		internal bool $disposing;

		// Token: 0x040068E9 RID: 26857
		internal int $PC;
	}

	// Token: 0x02000FB7 RID: 4023
	[CompilerGenerated]
	private sealed class <FadeSoundChannelTo>c__Iterator1 : IEnumerator, IDisposable, IEnumerator<object>
	{
		// Token: 0x060074E0 RID: 29920 RVA: 0x000FE370 File Offset: 0x000FC770
		[DebuggerHidden]
		public <FadeSoundChannelTo>c__Iterator1()
		{
		}

		// Token: 0x060074E1 RID: 29921 RVA: 0x000FE378 File Offset: 0x000FC778
		public bool MoveNext()
		{
			uint num = (uint)this.$PC;
			this.$PC = -1;
			float end;
			switch (num)
			{
			case 0U:
				start = this.audioSource.volume;
				end = toVolume;
				startTime = Time.realtimeSinceStartup;
				elapsedTime = 0f;
				break;
			case 1U:
				break;
			default:
				return false;
			}
			if (elapsedTime < fadeTime)
			{
				elapsedTime = Time.realtimeSinceStartup - startTime;
				t = elapsedTime / fadeTime;
				this.audioSource.volume = Mathf.Lerp(start, end, t);
				this.$current = 0;
				if (!this.$disposing)
				{
					this.$PC = 1;
				}
				return true;
			}
			this.state = SoundEmitter.FadeState.Null;
			this.$PC = -1;
			return false;
		}

		// Token: 0x1700113B RID: 4411
		// (get) Token: 0x060074E2 RID: 29922 RVA: 0x000FE476 File Offset: 0x000FC876
		object IEnumerator<object>.Current
		{
			[DebuggerHidden]
			get
			{
				return this.$current;
			}
		}

		// Token: 0x1700113C RID: 4412
		// (get) Token: 0x060074E3 RID: 29923 RVA: 0x000FE47E File Offset: 0x000FC87E
		object IEnumerator.Current
		{
			[DebuggerHidden]
			get
			{
				return this.$current;
			}
		}

		// Token: 0x060074E4 RID: 29924 RVA: 0x000FE486 File Offset: 0x000FC886
		[DebuggerHidden]
		public void Dispose()
		{
			this.$disposing = true;
			this.$PC = -1;
		}

		// Token: 0x060074E5 RID: 29925 RVA: 0x000FE496 File Offset: 0x000FC896
		[DebuggerHidden]
		public void Reset()
		{
			throw new NotSupportedException();
		}

		// Token: 0x040068EA RID: 26858
		internal float <start>__0;

		// Token: 0x040068EB RID: 26859
		internal float toVolume;

		// Token: 0x040068EC RID: 26860
		internal float <end>__0;

		// Token: 0x040068ED RID: 26861
		internal float <startTime>__0;

		// Token: 0x040068EE RID: 26862
		internal float <elapsedTime>__0;

		// Token: 0x040068EF RID: 26863
		internal float fadeTime;

		// Token: 0x040068F0 RID: 26864
		internal float <t>__1;

		// Token: 0x040068F1 RID: 26865
		internal SoundEmitter $this;

		// Token: 0x040068F2 RID: 26866
		internal object $current;

		// Token: 0x040068F3 RID: 26867
		internal bool $disposing;

		// Token: 0x040068F4 RID: 26868
		internal int $PC;
	}

	// Token: 0x02000FB8 RID: 4024
	[CompilerGenerated]
	private sealed class <FadeSoundChannel>c__Iterator2 : IEnumerator, IDisposable, IEnumerator<object>
	{
		// Token: 0x060074E6 RID: 29926 RVA: 0x000FE49D File Offset: 0x000FC89D
		[DebuggerHidden]
		public <FadeSoundChannel>c__Iterator2()
		{
		}

		// Token: 0x060074E7 RID: 29927 RVA: 0x000FE4A8 File Offset: 0x000FC8A8
		public bool MoveNext()
		{
			uint num = (uint)this.$PC;
			this.$PC = -1;
			switch (num)
			{
			case 0U:
				if (delaySecs > 0f)
				{
					this.$current = new WaitForSeconds(delaySecs);
					if (!this.$disposing)
					{
						this.$PC = 1;
					}
					return true;
				}
				break;
			case 1U:
				break;
			case 2U:
				if (restartPlay && Time.time > 0f)
				{
					this.audioSource.Play();
					restartPlay = false;
				}
				if (!this.audioSource.isPlaying)
				{
					goto IL_1CC;
				}
				goto IL_1BB;
			default:
				return false;
			}
			start = ((fadeType != Fade.In) ? defaultVolume : 0f);
			end = ((fadeType != Fade.In) ? 0f : defaultVolume);
			restartPlay = false;
			if (fadeType == Fade.In)
			{
				if (Time.time == 0f)
				{
					restartPlay = true;
				}
				this.audioSource.volume = 0f;
				this.audioSource.Play();
			}
			startTime = Time.realtimeSinceStartup;
			elapsedTime = 0f;
			IL_1BB:
			if (elapsedTime < fadeTime)
			{
				elapsedTime = Time.realtimeSinceStartup - startTime;
				t = elapsedTime / fadeTime;
				this.audioSource.volume = Mathf.Lerp(start, end, t);
				this.$current = 0;
				if (!this.$disposing)
				{
					this.$PC = 2;
				}
				return true;
			}
			IL_1CC:
			if (fadeType == Fade.Out)
			{
				base.Stop();
			}
			this.state = SoundEmitter.FadeState.Null;
			this.$PC = -1;
			return false;
		}

		// Token: 0x1700113D RID: 4413
		// (get) Token: 0x060074E8 RID: 29928 RVA: 0x000FE6AE File Offset: 0x000FCAAE
		object IEnumerator<object>.Current
		{
			[DebuggerHidden]
			get
			{
				return this.$current;
			}
		}

		// Token: 0x1700113E RID: 4414
		// (get) Token: 0x060074E9 RID: 29929 RVA: 0x000FE6B6 File Offset: 0x000FCAB6
		object IEnumerator.Current
		{
			[DebuggerHidden]
			get
			{
				return this.$current;
			}
		}

		// Token: 0x060074EA RID: 29930 RVA: 0x000FE6BE File Offset: 0x000FCABE
		[DebuggerHidden]
		public void Dispose()
		{
			this.$disposing = true;
			this.$PC = -1;
		}

		// Token: 0x060074EB RID: 29931 RVA: 0x000FE6CE File Offset: 0x000FCACE
		[DebuggerHidden]
		public void Reset()
		{
			throw new NotSupportedException();
		}

		// Token: 0x040068F5 RID: 26869
		internal float delaySecs;

		// Token: 0x040068F6 RID: 26870
		internal Fade fadeType;

		// Token: 0x040068F7 RID: 26871
		internal float defaultVolume;

		// Token: 0x040068F8 RID: 26872
		internal float <start>__0;

		// Token: 0x040068F9 RID: 26873
		internal float <end>__0;

		// Token: 0x040068FA RID: 26874
		internal bool <restartPlay>__0;

		// Token: 0x040068FB RID: 26875
		internal float <startTime>__0;

		// Token: 0x040068FC RID: 26876
		internal float <elapsedTime>__0;

		// Token: 0x040068FD RID: 26877
		internal float fadeTime;

		// Token: 0x040068FE RID: 26878
		internal float <t>__1;

		// Token: 0x040068FF RID: 26879
		internal SoundEmitter $this;

		// Token: 0x04006900 RID: 26880
		internal object $current;

		// Token: 0x04006901 RID: 26881
		internal bool $disposing;

		// Token: 0x04006902 RID: 26882
		internal int $PC;
	}
}
