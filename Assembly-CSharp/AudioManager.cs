using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Audio;

// Token: 0x0200075F RID: 1887
public class AudioManager : MonoBehaviour
{
	// Token: 0x06003080 RID: 12416 RVA: 0x000FC34C File Offset: 0x000FA74C
	public AudioManager()
	{
	}

	// Token: 0x170005CF RID: 1487
	// (get) Token: 0x06003081 RID: 12417 RVA: 0x000FC3CE File Offset: 0x000FA7CE
	public static bool enableSpatialization
	{
		get
		{
			return AudioManager.theAudioManager != null && AudioManager.theAudioManager.enableSpatializedAudio;
		}
	}

	// Token: 0x170005D0 RID: 1488
	// (get) Token: 0x06003082 RID: 12418 RVA: 0x000FC3F0 File Offset: 0x000FA7F0
	public static AudioManager Instance
	{
		get
		{
			return AudioManager.theAudioManager;
		}
	}

	// Token: 0x170005D1 RID: 1489
	// (get) Token: 0x06003083 RID: 12419 RVA: 0x000FC3F7 File Offset: 0x000FA7F7
	public static float NearFallOff
	{
		get
		{
			return AudioManager.theAudioManager.audioMinFallOffDistance;
		}
	}

	// Token: 0x170005D2 RID: 1490
	// (get) Token: 0x06003084 RID: 12420 RVA: 0x000FC403 File Offset: 0x000FA803
	public static float FarFallOff
	{
		get
		{
			return AudioManager.theAudioManager.audioMaxFallOffDistance;
		}
	}

	// Token: 0x170005D3 RID: 1491
	// (get) Token: 0x06003085 RID: 12421 RVA: 0x000FC40F File Offset: 0x000FA80F
	public static AudioMixerGroup EmitterGroup
	{
		get
		{
			return AudioManager.theAudioManager.defaultMixerGroup;
		}
	}

	// Token: 0x170005D4 RID: 1492
	// (get) Token: 0x06003086 RID: 12422 RVA: 0x000FC41B File Offset: 0x000FA81B
	public static AudioMixerGroup ReservedGroup
	{
		get
		{
			return AudioManager.theAudioManager.reservedMixerGroup;
		}
	}

	// Token: 0x170005D5 RID: 1493
	// (get) Token: 0x06003087 RID: 12423 RVA: 0x000FC427 File Offset: 0x000FA827
	public static AudioMixerGroup VoipGroup
	{
		get
		{
			return AudioManager.theAudioManager.voiceChatMixerGroup;
		}
	}

	// Token: 0x06003088 RID: 12424 RVA: 0x000FC433 File Offset: 0x000FA833
	private void Awake()
	{
		this.Init();
	}

	// Token: 0x06003089 RID: 12425 RVA: 0x000FC43B File Offset: 0x000FA83B
	private void OnDestroy()
	{
		if (AudioManager.theAudioManager == this && AudioManager.soundEmitterParent != null)
		{
			UnityEngine.Object.Destroy(AudioManager.soundEmitterParent);
		}
	}

	// Token: 0x0600308A RID: 12426 RVA: 0x000FC468 File Offset: 0x000FA868
	private void Init()
	{
		if (AudioManager.theAudioManager != null)
		{
			if (Application.isPlaying && AudioManager.theAudioManager != this)
			{
				base.enabled = false;
			}
			return;
		}
		AudioManager.theAudioManager = this;
		AudioManager.nullSound.name = "Default Sound";
		this.RebuildSoundFXCache();
		if (Application.isPlaying)
		{
			this.InitializeSoundSystem();
			if (this.makePersistent && base.transform.parent == null)
			{
				UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
			}
		}
	}

	// Token: 0x0600308B RID: 12427 RVA: 0x000FC4FE File Offset: 0x000FA8FE
	private void Update()
	{
		this.UpdateFreeEmitters();
	}

	// Token: 0x0600308C RID: 12428 RVA: 0x000FC508 File Offset: 0x000FA908
	private void RebuildSoundFXCache()
	{
		int num = 0;
		for (int i = 0; i < this.soundGroupings.Length; i++)
		{
			num += this.soundGroupings[i].soundList.Length;
		}
		this.soundFXCache = new Dictionary<string, SoundFX>(num + 1);
		this.soundFXCache.Add(AudioManager.nullSound.name, AudioManager.nullSound);
		for (int j = 0; j < this.soundGroupings.Length; j++)
		{
			for (int k = 0; k < this.soundGroupings[j].soundList.Length; k++)
			{
				if (this.soundFXCache.ContainsKey(this.soundGroupings[j].soundList[k].name))
				{
					Debug.LogError(string.Concat(new string[]
					{
						"ERROR: Duplicate Sound FX name in the audio manager: '",
						this.soundGroupings[j].name,
						"' > '",
						this.soundGroupings[j].soundList[k].name,
						"'"
					}));
				}
				else
				{
					this.soundGroupings[j].soundList[k].Group = this.soundGroupings[j];
					this.soundFXCache.Add(this.soundGroupings[j].soundList[k].name, this.soundGroupings[j].soundList[k]);
				}
			}
			this.soundGroupings[j].playingSoundCount = 0;
		}
	}

	// Token: 0x0600308D RID: 12429 RVA: 0x000FC674 File Offset: 0x000FAA74
	public static SoundFX FindSoundFX(string name, bool rebuildCache = false)
	{
		if (string.IsNullOrEmpty(name))
		{
			return AudioManager.nullSound;
		}
		if (rebuildCache)
		{
			AudioManager.theAudioManager.RebuildSoundFXCache();
		}
		if (!AudioManager.theAudioManager.soundFXCache.ContainsKey(name))
		{
			return AudioManager.nullSound;
		}
		return AudioManager.theAudioManager.soundFXCache[name];
	}

	// Token: 0x0600308E RID: 12430 RVA: 0x000FC6D0 File Offset: 0x000FAAD0
	private static bool FindAudioManager()
	{
		GameObject gameObject = GameObject.Find("AudioManager");
		if (gameObject == null || gameObject.GetComponent<AudioManager>() == null)
		{
			if (!AudioManager.hideWarnings)
			{
				Debug.LogError("[ERROR] AudioManager object missing from hierarchy!");
				AudioManager.hideWarnings = true;
			}
			return false;
		}
		gameObject.GetComponent<AudioManager>().Init();
		return true;
	}

	// Token: 0x0600308F RID: 12431 RVA: 0x000FC72D File Offset: 0x000FAB2D
	public static GameObject GetGameObject()
	{
		if (AudioManager.theAudioManager == null && !AudioManager.FindAudioManager())
		{
			return null;
		}
		return AudioManager.theAudioManager.gameObject;
	}

	// Token: 0x06003090 RID: 12432 RVA: 0x000FC755 File Offset: 0x000FAB55
	public static string NameMinusGroup(string name)
	{
		if (name.IndexOf("/") > -1)
		{
			return name.Substring(name.IndexOf("/") + 1);
		}
		return name;
	}

	// Token: 0x06003091 RID: 12433 RVA: 0x000FC780 File Offset: 0x000FAB80
	public static string[] GetSoundFXNames(string currentValue, out int currentIdx)
	{
		currentIdx = 0;
		AudioManager.names.Clear();
		if (AudioManager.theAudioManager == null && !AudioManager.FindAudioManager())
		{
			return AudioManager.defaultSound;
		}
		AudioManager.names.Add(AudioManager.nullSound.name);
		for (int i = 0; i < AudioManager.theAudioManager.soundGroupings.Length; i++)
		{
			for (int j = 0; j < AudioManager.theAudioManager.soundGroupings[i].soundList.Length; j++)
			{
				if (string.Compare(currentValue, AudioManager.theAudioManager.soundGroupings[i].soundList[j].name, true) == 0)
				{
					currentIdx = AudioManager.names.Count;
				}
				AudioManager.names.Add(AudioManager.theAudioManager.soundGroupings[i].name + "/" + AudioManager.theAudioManager.soundGroupings[i].soundList[j].name);
			}
		}
		return AudioManager.names.ToArray();
	}

	// Token: 0x170005D6 RID: 1494
	// (get) Token: 0x06003092 RID: 12434 RVA: 0x000FC886 File Offset: 0x000FAC86
	public static bool SoundEnabled
	{
		get
		{
			return AudioManager.soundEnabled;
		}
	}

	// Token: 0x06003093 RID: 12435 RVA: 0x000FC890 File Offset: 0x000FAC90
	private void InitializeSoundSystem()
	{
		int num = 960;
		int num2 = 4;
		AudioSettings.GetDSPBufferSize(out num, out num2);
		if (Application.isPlaying)
		{
			Debug.Log("[AudioManager] Audio Sample Rate: " + AudioSettings.outputSampleRate);
			Debug.Log(string.Concat(new object[]
			{
				"[AudioManager] Audio Buffer Length: ",
				num,
				" Size: ",
				num2
			}));
		}
		AudioListener audioListener = UnityEngine.Object.FindObjectOfType<AudioListener>();
		if (audioListener == null)
		{
			Debug.LogError("[AudioManager] Missing AudioListener object!  Add one to the scene.");
		}
		else
		{
			AudioManager.staticListenerPosition = audioListener.transform;
		}
		this.soundEmitters = new SoundEmitter[this.maxSoundEmitters + 1];
		AudioManager.soundEmitterParent = GameObject.Find("__SoundEmitters__");
		if (AudioManager.soundEmitterParent != null)
		{
			UnityEngine.Object.Destroy(AudioManager.soundEmitterParent);
		}
		AudioManager.soundEmitterParent = new GameObject("__SoundEmitters__");
		for (int i = 0; i < this.maxSoundEmitters + 1; i++)
		{
			GameObject gameObject = new GameObject("SoundEmitter_" + i);
			gameObject.transform.parent = AudioManager.soundEmitterParent.transform;
			gameObject.transform.position = Vector3.zero;
			gameObject.hideFlags = HideFlags.DontSaveInEditor;
			this.soundEmitters[i] = gameObject.AddComponent<SoundEmitter>();
			this.soundEmitters[i].SetDefaultParent(AudioManager.soundEmitterParent.transform);
			this.soundEmitters[i].SetChannel(i);
			this.soundEmitters[i].Stop();
			this.soundEmitters[i].originalIdx = i;
		}
		this.ResetFreeEmitters();
		AudioManager.soundEmitterParent.hideFlags = HideFlags.DontSaveInEditor;
		this.audioMaxFallOffDistanceSqr = this.audioMaxFallOffDistance * this.audioMaxFallOffDistance;
	}

	// Token: 0x06003094 RID: 12436 RVA: 0x000FCA4C File Offset: 0x000FAE4C
	private void UpdateFreeEmitters()
	{
		if (this.verboseLogging)
		{
			if (Input.GetKeyDown(KeyCode.A))
			{
				AudioManager.forceShowEmitterCount = !AudioManager.forceShowEmitterCount;
			}
			if (AudioManager.forceShowEmitterCount)
			{
				AudioManager.showPlayingEmitterCount = true;
			}
		}
		int num = 0;
		int num2 = 0;
		int num3 = 0;
		int num4 = 0;
		int num5 = 0;
		int num6 = 0;
		int i = 0;
		while (i < this.playingEmitters.size)
		{
			if (this.playingEmitters[i] == null)
			{
				Debug.LogError("[AudioManager] ERROR: playingEmitters list had a null emitter! Something nuked a sound emitter!!!");
				this.playingEmitters.RemoveAtFast(i);
				return;
			}
			if (!this.playingEmitters[i].IsPlaying())
			{
				if (this.verboseLogging && this.nextFreeEmitters.Contains(this.playingEmitters[i]))
				{
					Debug.LogError("[AudioManager] ERROR: playing sound emitter already in the free emitters list!");
				}
				this.playingEmitters[i].Stop();
				this.nextFreeEmitters.Add(this.playingEmitters[i]);
				this.playingEmitters.RemoveAtFast(i);
			}
			else
			{
				if (this.verboseLogging && AudioManager.showPlayingEmitterCount)
				{
					num++;
					SoundPriority priority = this.playingEmitters[i].priority;
					switch (priority + 2)
					{
					case SoundPriority.Default:
						num2++;
						break;
					case SoundPriority.High:
						num3++;
						break;
					case SoundPriority.VeryHigh:
						num4++;
						break;
					case (SoundPriority)3:
						num5++;
						break;
					case (SoundPriority)4:
						num6++;
						break;
					}
				}
				i++;
			}
		}
		if (this.verboseLogging && AudioManager.showPlayingEmitterCount)
		{
			Debug.LogWarning(string.Format("[AudioManager] Playing sounds: Total {0} | VeryLow {1} | Low {2} | Default {3} | High {4} | VeryHigh {5} | Free {6}", new object[]
			{
				this.Fmt(num),
				this.Fmt(num2),
				this.Fmt(num3),
				this.Fmt(num4),
				this.Fmt(num5),
				this.Fmt(num6),
				this.FmtFree(this.nextFreeEmitters.Count)
			}));
			AudioManager.showPlayingEmitterCount = false;
		}
	}

	// Token: 0x06003095 RID: 12437 RVA: 0x000FCC74 File Offset: 0x000FB074
	private string Fmt(int count)
	{
		float num = (float)count / (float)AudioManager.theAudioManager.maxSoundEmitters;
		if (num < 0.5f)
		{
			return "<color=green>" + count.ToString() + "</color>";
		}
		if ((double)num < 0.7)
		{
			return "<color=yellow>" + count.ToString() + "</color>";
		}
		return "<color=red>" + count.ToString() + "</color>";
	}

	// Token: 0x06003096 RID: 12438 RVA: 0x000FCD04 File Offset: 0x000FB104
	private string FmtFree(int count)
	{
		float num = (float)count / (float)AudioManager.theAudioManager.maxSoundEmitters;
		if (num < 0.2f)
		{
			return "<color=red>" + count.ToString() + "</color>";
		}
		if ((double)num < 0.3)
		{
			return "<color=yellow>" + count.ToString() + "</color>";
		}
		return "<color=green>" + count.ToString() + "</color>";
	}

	// Token: 0x06003097 RID: 12439 RVA: 0x000FCD94 File Offset: 0x000FB194
	private void OnPreSceneLoad()
	{
		Debug.Log("[AudioManager] OnPreSceneLoad cleanup");
		for (int i = 0; i < this.soundEmitters.Length; i++)
		{
			this.soundEmitters[i].Stop();
			this.soundEmitters[i].ResetParent(AudioManager.soundEmitterParent.transform);
		}
		this.ResetFreeEmitters();
	}

	// Token: 0x06003098 RID: 12440 RVA: 0x000FCDF0 File Offset: 0x000FB1F0
	private void ResetFreeEmitters()
	{
		this.nextFreeEmitters.Clear();
		this.playingEmitters.Clear();
		for (int i = 1; i < this.soundEmitters.Length; i++)
		{
			this.nextFreeEmitters.Add(this.soundEmitters[i]);
		}
	}

	// Token: 0x06003099 RID: 12441 RVA: 0x000FCE3F File Offset: 0x000FB23F
	public static void FadeOutSoundChannel(int channel, float delaySecs, float fadeTime)
	{
		AudioManager.theAudioManager.soundEmitters[channel].FadeOutDelayed(delaySecs, fadeTime);
	}

	// Token: 0x0600309A RID: 12442 RVA: 0x000FCE54 File Offset: 0x000FB254
	public static bool StopSound(int idx, bool fadeOut = true, bool stopReserved = false)
	{
		if (!stopReserved && idx == 0)
		{
			return false;
		}
		if (!fadeOut)
		{
			AudioManager.theAudioManager.soundEmitters[idx].Stop();
		}
		else
		{
			AudioManager.theAudioManager.soundEmitters[idx].FadeOut(AudioManager.theAudioManager.soundFxFadeSecs);
		}
		return true;
	}

	// Token: 0x0600309B RID: 12443 RVA: 0x000FCEA7 File Offset: 0x000FB2A7
	public static void FadeInSound(int idx, float fadeTime, float volume)
	{
		AudioManager.theAudioManager.soundEmitters[idx].FadeIn(fadeTime, volume);
	}

	// Token: 0x0600309C RID: 12444 RVA: 0x000FCEBC File Offset: 0x000FB2BC
	public static void FadeInSound(int idx, float fadeTime)
	{
		AudioManager.theAudioManager.soundEmitters[idx].FadeIn(fadeTime);
	}

	// Token: 0x0600309D RID: 12445 RVA: 0x000FCED0 File Offset: 0x000FB2D0
	public static void FadeOutSound(int idx, float fadeTime)
	{
		AudioManager.theAudioManager.soundEmitters[idx].FadeOut(fadeTime);
	}

	// Token: 0x0600309E RID: 12446 RVA: 0x000FCEE4 File Offset: 0x000FB2E4
	public static void StopAllSounds(bool fadeOut, bool stopReserved = false)
	{
		for (int i = 0; i < AudioManager.theAudioManager.soundEmitters.Length; i++)
		{
			AudioManager.StopSound(i, fadeOut, stopReserved);
		}
	}

	// Token: 0x0600309F RID: 12447 RVA: 0x000FCF18 File Offset: 0x000FB318
	public void MuteAllSounds(bool mute, bool muteReserved = false)
	{
		for (int i = 0; i < this.soundEmitters.Length; i++)
		{
			if (muteReserved || i != 0)
			{
				this.soundEmitters[i].audioSource.mute = true;
			}
		}
	}

	// Token: 0x060030A0 RID: 12448 RVA: 0x000FCF64 File Offset: 0x000FB364
	public void UnMuteAllSounds(bool unmute, bool unmuteReserved = false)
	{
		for (int i = 0; i < this.soundEmitters.Length; i++)
		{
			if (unmuteReserved || i != 0)
			{
				if (this.soundEmitters[i].audioSource.isPlaying)
				{
					this.soundEmitters[i].audioSource.mute = false;
				}
			}
		}
	}

	// Token: 0x060030A1 RID: 12449 RVA: 0x000FCFC5 File Offset: 0x000FB3C5
	public static float GetEmitterEndTime(int idx)
	{
		return AudioManager.theAudioManager.soundEmitters[idx].endPlayTime;
	}

	// Token: 0x060030A2 RID: 12450 RVA: 0x000FCFD8 File Offset: 0x000FB3D8
	public static float SetEmitterTime(int idx, float time)
	{
		AudioManager.theAudioManager.soundEmitters[idx].time = time;
		return time;
	}

	// Token: 0x060030A3 RID: 12451 RVA: 0x000FCFFA File Offset: 0x000FB3FA
	public static int PlaySound(AudioClip clip, float volume, EmitterChannel src = EmitterChannel.Any, float delay = 0f, float pitchVariance = 1f, bool loop = false)
	{
		if (!AudioManager.SoundEnabled)
		{
			return -1;
		}
		return AudioManager.PlaySoundAt((!(AudioManager.staticListenerPosition != null)) ? Vector3.zero : AudioManager.staticListenerPosition.position, clip, volume, src, delay, pitchVariance, loop);
	}

	// Token: 0x060030A4 RID: 12452 RVA: 0x000FD03C File Offset: 0x000FB43C
	private static int FindFreeEmitter(EmitterChannel src, SoundPriority priority)
	{
		AudioManager.<FindFreeEmitter>c__AnonStorey0 <FindFreeEmitter>c__AnonStorey = new AudioManager.<FindFreeEmitter>c__AnonStorey0();
		<FindFreeEmitter>c__AnonStorey.priority = priority;
		SoundEmitter soundEmitter = AudioManager.theAudioManager.soundEmitters[0];
		if (src == EmitterChannel.Any)
		{
			if (AudioManager.theAudioManager.nextFreeEmitters.size > 0)
			{
				soundEmitter = AudioManager.theAudioManager.nextFreeEmitters[0];
				AudioManager.theAudioManager.nextFreeEmitters.RemoveAtFast(0);
			}
			else
			{
				if (<FindFreeEmitter>c__AnonStorey.priority == SoundPriority.VeryLow)
				{
					return -1;
				}
				soundEmitter = AudioManager.theAudioManager.playingEmitters.Find(new Predicate<SoundEmitter>(<FindFreeEmitter>c__AnonStorey.<>m__0));
				if (soundEmitter == null)
				{
					if (<FindFreeEmitter>c__AnonStorey.priority < SoundPriority.Default)
					{
						if (AudioManager.theAudioManager.verboseLogging)
						{
							Debug.LogWarning("[AudioManager] skipping sound " + <FindFreeEmitter>c__AnonStorey.priority);
						}
						return -1;
					}
					FastList<SoundEmitter> fastList = AudioManager.theAudioManager.playingEmitters;
					if (AudioManager.<>f__am$cache0 == null)
					{
						AudioManager.<>f__am$cache0 = new Predicate<SoundEmitter>(AudioManager.<FindFreeEmitter>m__0);
					}
					soundEmitter = fastList.Find(AudioManager.<>f__am$cache0);
				}
				if (soundEmitter != null)
				{
					if (AudioManager.theAudioManager.verboseLogging)
					{
						Debug.LogWarning(string.Concat(new object[]
						{
							"[AudioManager] cannabalizing ",
							soundEmitter.originalIdx,
							" Time: ",
							Time.time
						}));
					}
					soundEmitter.Stop();
					AudioManager.theAudioManager.playingEmitters.RemoveFast(soundEmitter);
				}
			}
		}
		if (soundEmitter == null)
		{
			Debug.LogError("[AudioManager] ERROR - absolutely couldn't find a free emitter! Priority = " + <FindFreeEmitter>c__AnonStorey.priority + " TOO MANY PlaySound* calls!");
			AudioManager.showPlayingEmitterCount = true;
			return -1;
		}
		return soundEmitter.originalIdx;
	}

	// Token: 0x060030A5 RID: 12453 RVA: 0x000FD1E8 File Offset: 0x000FB5E8
	public static int PlaySound(SoundFX soundFX, EmitterChannel src = EmitterChannel.Any, float delay = 0f)
	{
		if (!AudioManager.SoundEnabled)
		{
			return -1;
		}
		return AudioManager.PlaySoundAt((!(AudioManager.staticListenerPosition != null)) ? Vector3.zero : AudioManager.staticListenerPosition.position, soundFX, src, delay, 1f, 1f);
	}

	// Token: 0x060030A6 RID: 12454 RVA: 0x000FD238 File Offset: 0x000FB638
	public static int PlaySoundAt(Vector3 position, SoundFX soundFX, EmitterChannel src = EmitterChannel.Any, float delay = 0f, float volumeOverride = 1f, float pitchMultiplier = 1f)
	{
		if (!AudioManager.SoundEnabled)
		{
			return -1;
		}
		AudioClip clip = soundFX.GetClip();
		if (clip == null)
		{
			return -1;
		}
		if (AudioManager.staticListenerPosition != null)
		{
			float sqrMagnitude = (AudioManager.staticListenerPosition.position - position).sqrMagnitude;
			if (sqrMagnitude > AudioManager.theAudioManager.audioMaxFallOffDistanceSqr)
			{
				return -1;
			}
			if (sqrMagnitude > soundFX.MaxFalloffDistSquared)
			{
				return -1;
			}
		}
		if (soundFX.ReachedGroupPlayLimit())
		{
			if (AudioManager.theAudioManager.verboseLogging)
			{
				Debug.Log("[AudioManager] PlaySoundAt() with " + soundFX.name + " skipped due to group play limit");
			}
			return -1;
		}
		int num = AudioManager.FindFreeEmitter(src, soundFX.priority);
		if (num == -1)
		{
			return -1;
		}
		SoundEmitter soundEmitter = AudioManager.theAudioManager.soundEmitters[num];
		soundEmitter.ResetParent(AudioManager.soundEmitterParent.transform);
		soundEmitter.gameObject.SetActive(true);
		AudioSource audioSource = soundEmitter.audioSource;
		ONSPAudioSource osp = soundEmitter.osp;
		audioSource.enabled = true;
		audioSource.volume = Mathf.Clamp01(Mathf.Clamp01(AudioManager.theAudioManager.volumeSoundFX * soundFX.volume) * volumeOverride * soundFX.GroupVolumeOverride);
		audioSource.pitch = soundFX.GetPitch() * pitchMultiplier;
		audioSource.time = 0f;
		audioSource.spatialBlend = 1f;
		audioSource.rolloffMode = soundFX.falloffCurve;
		if (soundFX.falloffCurve == AudioRolloffMode.Custom)
		{
			audioSource.SetCustomCurve(AudioSourceCurveType.CustomRolloff, soundFX.volumeFalloffCurve);
		}
		audioSource.SetCustomCurve(AudioSourceCurveType.ReverbZoneMix, soundFX.reverbZoneMix);
		audioSource.dopplerLevel = 0f;
		audioSource.clip = clip;
		audioSource.spread = soundFX.spread;
		audioSource.loop = soundFX.looping;
		audioSource.mute = false;
		audioSource.minDistance = soundFX.falloffDistance.x;
		audioSource.maxDistance = soundFX.falloffDistance.y;
		audioSource.outputAudioMixerGroup = soundFX.GetMixerGroup(AudioManager.EmitterGroup);
		soundEmitter.endPlayTime = Time.time + clip.length + delay;
		soundEmitter.defaultVolume = audioSource.volume;
		soundEmitter.priority = soundFX.priority;
		soundEmitter.onFinished = null;
		soundEmitter.SetPlayingSoundGroup(soundFX.Group);
		if (src == EmitterChannel.Any)
		{
			AudioManager.theAudioManager.playingEmitters.AddUnique(soundEmitter);
		}
		if (osp != null)
		{
			osp.EnableSpatialization = soundFX.ospProps.enableSpatialization;
			osp.EnableRfl = (AudioManager.theAudioManager.enableSpatializedFastOverride || soundFX.ospProps.useFastOverride);
			osp.Gain = soundFX.ospProps.gain;
			osp.UseInvSqr = soundFX.ospProps.enableInvSquare;
			osp.Near = soundFX.ospProps.invSquareFalloff.x;
			osp.Far = soundFX.ospProps.invSquareFalloff.y;
			audioSource.spatialBlend = ((!soundFX.ospProps.enableSpatialization) ? 0.8f : 1f);
			osp.SetParameters(ref audioSource);
		}
		audioSource.transform.position = position;
		if (AudioManager.theAudioManager.verboseLogging)
		{
			Debug.Log(string.Concat(new object[]
			{
				"[AudioManager] PlaySoundAt() channel = ",
				num,
				" soundFX = ",
				soundFX.name,
				" volume = ",
				soundEmitter.volume,
				" Delay = ",
				delay,
				" time = ",
				Time.time,
				"\n"
			}));
		}
		if (delay > 0f)
		{
			audioSource.PlayDelayed(delay);
		}
		else
		{
			audioSource.Play();
		}
		return num;
	}

	// Token: 0x060030A7 RID: 12455 RVA: 0x000FD618 File Offset: 0x000FBA18
	public static int PlayRandomSoundAt(Vector3 position, AudioClip[] clips, float volume, EmitterChannel src = EmitterChannel.Any, float delay = 0f, float pitch = 1f, bool loop = false)
	{
		if (clips == null || clips.Length == 0)
		{
			return -1;
		}
		int num = UnityEngine.Random.Range(0, clips.Length);
		return AudioManager.PlaySoundAt(position, clips[num], volume, src, delay, pitch, loop);
	}

	// Token: 0x060030A8 RID: 12456 RVA: 0x000FD650 File Offset: 0x000FBA50
	public static int PlaySoundAt(Vector3 position, AudioClip clip, float volume = 1f, EmitterChannel src = EmitterChannel.Any, float delay = 0f, float pitch = 1f, bool loop = false)
	{
		if (!AudioManager.SoundEnabled)
		{
			return -1;
		}
		if (clip == null)
		{
			return -1;
		}
		if (AudioManager.staticListenerPosition != null && (AudioManager.staticListenerPosition.position - position).sqrMagnitude > AudioManager.theAudioManager.audioMaxFallOffDistanceSqr)
		{
			return -1;
		}
		int num = AudioManager.FindFreeEmitter(src, SoundPriority.Default);
		if (num == -1)
		{
			return -1;
		}
		SoundEmitter soundEmitter = AudioManager.theAudioManager.soundEmitters[num];
		soundEmitter.ResetParent(AudioManager.soundEmitterParent.transform);
		soundEmitter.gameObject.SetActive(true);
		AudioSource audioSource = soundEmitter.audioSource;
		ONSPAudioSource osp = soundEmitter.osp;
		audioSource.enabled = true;
		audioSource.volume = Mathf.Clamp01(AudioManager.theAudioManager.volumeSoundFX * volume);
		audioSource.pitch = pitch;
		audioSource.spatialBlend = 0.8f;
		audioSource.rolloffMode = AudioRolloffMode.Linear;
		audioSource.SetCustomCurve(AudioSourceCurveType.ReverbZoneMix, AudioManager.defaultReverbZoneMix);
		audioSource.dopplerLevel = 0f;
		audioSource.clip = clip;
		audioSource.spread = 0f;
		audioSource.loop = loop;
		audioSource.mute = false;
		audioSource.minDistance = AudioManager.theAudioManager.audioMinFallOffDistance;
		audioSource.maxDistance = AudioManager.theAudioManager.audioMaxFallOffDistance;
		audioSource.outputAudioMixerGroup = AudioManager.EmitterGroup;
		soundEmitter.endPlayTime = Time.time + clip.length + delay;
		soundEmitter.defaultVolume = audioSource.volume;
		soundEmitter.priority = SoundPriority.Default;
		soundEmitter.onFinished = null;
		soundEmitter.SetPlayingSoundGroup(null);
		if (src == EmitterChannel.Any)
		{
			AudioManager.theAudioManager.playingEmitters.AddUnique(soundEmitter);
		}
		if (osp != null)
		{
			osp.EnableSpatialization = false;
		}
		audioSource.transform.position = position;
		if (AudioManager.theAudioManager.verboseLogging)
		{
			Debug.Log(string.Concat(new object[]
			{
				"[AudioManager] PlaySoundAt() channel = ",
				num,
				" clip = ",
				clip.name,
				" volume = ",
				soundEmitter.volume,
				" Delay = ",
				delay,
				" time = ",
				Time.time,
				"\n"
			}));
		}
		if (delay > 0f)
		{
			audioSource.PlayDelayed(delay);
		}
		else
		{
			audioSource.Play();
		}
		return num;
	}

	// Token: 0x060030A9 RID: 12457 RVA: 0x000FD8AB File Offset: 0x000FBCAB
	public static void SetOnFinished(int emitterIdx, Action onFinished)
	{
		if (emitterIdx >= 0 && emitterIdx < AudioManager.theAudioManager.maxSoundEmitters)
		{
			AudioManager.theAudioManager.soundEmitters[emitterIdx].SetOnFinished(onFinished);
		}
	}

	// Token: 0x060030AA RID: 12458 RVA: 0x000FD8D6 File Offset: 0x000FBCD6
	public static void SetOnFinished(int emitterIdx, Action<object> onFinished, object obj)
	{
		if (emitterIdx >= 0 && emitterIdx < AudioManager.theAudioManager.maxSoundEmitters)
		{
			AudioManager.theAudioManager.soundEmitters[emitterIdx].SetOnFinished(onFinished, obj);
		}
	}

	// Token: 0x060030AB RID: 12459 RVA: 0x000FD904 File Offset: 0x000FBD04
	public static void AttachSoundToParent(int idx, Transform parent)
	{
		if (AudioManager.theAudioManager.verboseLogging)
		{
			string text = parent.name;
			if (parent.parent != null)
			{
				text = parent.parent.name + "/" + text;
			}
			Debug.Log(string.Concat(new object[]
			{
				"[AudioManager] ATTACHING INDEX ",
				idx,
				" to ",
				text
			}));
		}
		AudioManager.theAudioManager.soundEmitters[idx].ParentTo(parent);
	}

	// Token: 0x060030AC RID: 12460 RVA: 0x000FD98E File Offset: 0x000FBD8E
	public static void DetachSoundFromParent(int idx)
	{
		if (AudioManager.theAudioManager.verboseLogging)
		{
			Debug.Log("[AudioManager] DETACHING INDEX " + idx);
		}
		AudioManager.theAudioManager.soundEmitters[idx].DetachFromParent();
	}

	// Token: 0x060030AD RID: 12461 RVA: 0x000FD9C8 File Offset: 0x000FBDC8
	public static void DetachSoundsFromParent(SoundEmitter[] emitters, bool stopSounds = true)
	{
		if (emitters == null)
		{
			return;
		}
		foreach (SoundEmitter soundEmitter in emitters)
		{
			if (soundEmitter.defaultParent != null)
			{
				if (stopSounds)
				{
					soundEmitter.Stop();
				}
				soundEmitter.DetachFromParent();
				soundEmitter.gameObject.SetActive(true);
			}
			else if (stopSounds)
			{
				soundEmitter.Stop();
			}
		}
	}

	// Token: 0x060030AE RID: 12462 RVA: 0x000FDA36 File Offset: 0x000FBE36
	public static void SetEmitterMixerGroup(int idx, AudioMixerGroup mixerGroup)
	{
		if (AudioManager.theAudioManager != null && idx > -1)
		{
			AudioManager.theAudioManager.soundEmitters[idx].SetAudioMixer(mixerGroup);
		}
	}

	// Token: 0x060030AF RID: 12463 RVA: 0x000FDA61 File Offset: 0x000FBE61
	public static MixerSnapshot GetActiveSnapshot()
	{
		return (!(AudioManager.theAudioManager != null)) ? null : AudioManager.theAudioManager.currentSnapshot;
	}

	// Token: 0x060030B0 RID: 12464 RVA: 0x000FDA84 File Offset: 0x000FBE84
	public static void SetCurrentSnapshot(MixerSnapshot mixerSnapshot)
	{
		if (AudioManager.theAudioManager != null)
		{
			if (mixerSnapshot != null && mixerSnapshot.snapshot != null)
			{
				mixerSnapshot.snapshot.TransitionTo(mixerSnapshot.transitionTime);
			}
			else
			{
				mixerSnapshot = null;
			}
			AudioManager.theAudioManager.currentSnapshot = mixerSnapshot;
		}
	}

	// Token: 0x060030B1 RID: 12465 RVA: 0x000FDADC File Offset: 0x000FBEDC
	public static void BlendWithCurrentSnapshot(MixerSnapshot blendSnapshot, float weight, float blendTime = 0f)
	{
		if (AudioManager.theAudioManager != null)
		{
			if (AudioManager.theAudioManager.audioMixer == null)
			{
				Debug.LogWarning("[AudioManager] can't call BlendWithCurrentSnapshot if the audio mixer is not set!");
				return;
			}
			if (blendTime == 0f)
			{
				blendTime = Time.deltaTime;
			}
			if (AudioManager.theAudioManager.currentSnapshot != null && AudioManager.theAudioManager.currentSnapshot.snapshot != null && blendSnapshot != null && blendSnapshot.snapshot != null)
			{
				weight = Mathf.Clamp01(weight);
				if (weight == 0f)
				{
					AudioManager.theAudioManager.currentSnapshot.snapshot.TransitionTo(blendTime);
				}
				else
				{
					AudioMixerSnapshot[] snapshots = new AudioMixerSnapshot[]
					{
						AudioManager.theAudioManager.currentSnapshot.snapshot,
						blendSnapshot.snapshot
					};
					float[] weights = new float[]
					{
						1f - weight,
						weight
					};
					AudioManager.theAudioManager.audioMixer.TransitionToSnapshots(snapshots, weights, blendTime);
				}
			}
		}
	}

	// Token: 0x060030B2 RID: 12466 RVA: 0x000FDBE4 File Offset: 0x000FBFE4
	// Note: this type is marked as 'beforefieldinit'.
	static AudioManager()
	{
	}

	// Token: 0x060030B3 RID: 12467 RVA: 0x000FDC88 File Offset: 0x000FC088
	[CompilerGenerated]
	private static bool <FindFreeEmitter>m__0(SoundEmitter item)
	{
		return item != null && item.priority <= SoundPriority.Default;
	}

	// Token: 0x04002475 RID: 9333
	[Tooltip("Make the audio manager persistent across all scene loads")]
	public bool makePersistent = true;

	// Token: 0x04002476 RID: 9334
	[Tooltip("Enable the OSP audio plugin features")]
	public bool enableSpatializedAudio = true;

	// Token: 0x04002477 RID: 9335
	[Tooltip("Always play spatialized sounds with no reflections (Default)")]
	public bool enableSpatializedFastOverride;

	// Token: 0x04002478 RID: 9336
	[Tooltip("The audio mixer asset used for snapshot blends, etc.")]
	public AudioMixer audioMixer;

	// Token: 0x04002479 RID: 9337
	[Tooltip("The audio mixer group used for the pooled emitters")]
	public AudioMixerGroup defaultMixerGroup;

	// Token: 0x0400247A RID: 9338
	[Tooltip("The audio mixer group used for the reserved pool emitter")]
	public AudioMixerGroup reservedMixerGroup;

	// Token: 0x0400247B RID: 9339
	[Tooltip("The audio mixer group used for voice chat")]
	public AudioMixerGroup voiceChatMixerGroup;

	// Token: 0x0400247C RID: 9340
	[Tooltip("Log all PlaySound calls to the Unity console")]
	public bool verboseLogging;

	// Token: 0x0400247D RID: 9341
	[Tooltip("Maximum sound emitters")]
	public int maxSoundEmitters = 32;

	// Token: 0x0400247E RID: 9342
	[Tooltip("Default volume for all sounds modulated by individual sound FX volumes")]
	public float volumeSoundFX = 1f;

	// Token: 0x0400247F RID: 9343
	[Tooltip("Sound FX fade time")]
	public float soundFxFadeSecs = 1f;

	// Token: 0x04002480 RID: 9344
	public float audioMinFallOffDistance = 1f;

	// Token: 0x04002481 RID: 9345
	public float audioMaxFallOffDistance = 25f;

	// Token: 0x04002482 RID: 9346
	public SoundGroup[] soundGroupings = new SoundGroup[0];

	// Token: 0x04002483 RID: 9347
	private Dictionary<string, SoundFX> soundFXCache;

	// Token: 0x04002484 RID: 9348
	private static AudioManager theAudioManager = null;

	// Token: 0x04002485 RID: 9349
	private static FastList<string> names = new FastList<string>();

	// Token: 0x04002486 RID: 9350
	private static string[] defaultSound = new string[]
	{
		"Default Sound"
	};

	// Token: 0x04002487 RID: 9351
	private static SoundFX nullSound = new SoundFX();

	// Token: 0x04002488 RID: 9352
	private static bool hideWarnings = false;

	// Token: 0x04002489 RID: 9353
	private float audioMaxFallOffDistanceSqr = 625f;

	// Token: 0x0400248A RID: 9354
	private SoundEmitter[] soundEmitters;

	// Token: 0x0400248B RID: 9355
	private FastList<SoundEmitter> playingEmitters = new FastList<SoundEmitter>();

	// Token: 0x0400248C RID: 9356
	private FastList<SoundEmitter> nextFreeEmitters = new FastList<SoundEmitter>();

	// Token: 0x0400248D RID: 9357
	private MixerSnapshot currentSnapshot;

	// Token: 0x0400248E RID: 9358
	private static GameObject soundEmitterParent = null;

	// Token: 0x0400248F RID: 9359
	private static Transform staticListenerPosition = null;

	// Token: 0x04002490 RID: 9360
	private static bool showPlayingEmitterCount = false;

	// Token: 0x04002491 RID: 9361
	private static bool forceShowEmitterCount = false;

	// Token: 0x04002492 RID: 9362
	private static bool soundEnabled = true;

	// Token: 0x04002493 RID: 9363
	private static readonly AnimationCurve defaultReverbZoneMix = new AnimationCurve(new Keyframe[]
	{
		new Keyframe(0f, 1f),
		new Keyframe(1f, 1f)
	});

	// Token: 0x04002494 RID: 9364
	[CompilerGenerated]
	private static Predicate<SoundEmitter> <>f__am$cache0;

	// Token: 0x02000762 RID: 1890
	public enum Fade
	{
		// Token: 0x0400249C RID: 9372
		In,
		// Token: 0x0400249D RID: 9373
		Out
	}

	// Token: 0x02000FB5 RID: 4021
	[CompilerGenerated]
	private sealed class <FindFreeEmitter>c__AnonStorey0
	{
		// Token: 0x060074D8 RID: 29912 RVA: 0x000FDCA5 File Offset: 0x000FC0A5
		public <FindFreeEmitter>c__AnonStorey0()
		{
		}

		// Token: 0x060074D9 RID: 29913 RVA: 0x000FDCAD File Offset: 0x000FC0AD
		internal bool <>m__0(SoundEmitter item)
		{
			return item != null && item.priority < this.priority;
		}

		// Token: 0x040068E2 RID: 26850
		internal SoundPriority priority;
	}
}
