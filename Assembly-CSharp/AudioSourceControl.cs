using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using SimpleJSON;
using SpeechBlendEngine;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000B79 RID: 2937
public class AudioSourceControl : VariableTrigger
{
	// Token: 0x0600526E RID: 21102 RVA: 0x001DD130 File Offset: 0x001DB530
	public AudioSourceControl()
	{
	}

	// Token: 0x0600526F RID: 21103 RVA: 0x001DD1AD File Offset: 0x001DB5AD
	public override void RestoreFromJSON(JSONClass jc, bool restorePhysical = true, bool restoreAppearance = true, JSONArray presetAtoms = null, bool setMissingToDefault = true)
	{
		if (restoreAppearance && restorePhysical)
		{
			this.ClearPlayingClip();
		}
		base.RestoreFromJSON(jc, restorePhysical, restoreAppearance, presetAtoms, setMissingToDefault);
	}

	// Token: 0x06005270 RID: 21104 RVA: 0x001DD1CE File Offset: 0x001DB5CE
	protected void SyncLoop(bool b)
	{
		this._loop = b;
		if (this.audioSource != null)
		{
			this.audioSource.loop = this._loop;
		}
	}

	// Token: 0x17000BF4 RID: 3060
	// (get) Token: 0x06005271 RID: 21105 RVA: 0x001DD1F9 File Offset: 0x001DB5F9
	// (set) Token: 0x06005272 RID: 21106 RVA: 0x001DD201 File Offset: 0x001DB601
	public bool loop
	{
		get
		{
			return this._loop;
		}
		set
		{
			if (this.loopJSON != null)
			{
				this.loopJSON.val = value;
			}
			else if (this._loop != value)
			{
				this.SyncLoop(value);
			}
		}
	}

	// Token: 0x06005273 RID: 21107 RVA: 0x001DD232 File Offset: 0x001DB632
	protected void SyncVolume(float f)
	{
		this._volume = f;
		if (this.audioSource != null)
		{
			this.audioSource.volume = this._volume;
		}
	}

	// Token: 0x17000BF5 RID: 3061
	// (get) Token: 0x06005274 RID: 21108 RVA: 0x001DD25D File Offset: 0x001DB65D
	// (set) Token: 0x06005275 RID: 21109 RVA: 0x001DD265 File Offset: 0x001DB665
	public float volume
	{
		get
		{
			return this._volume;
		}
		set
		{
			if (this.volumeJSON != null)
			{
				this.volumeJSON.val = value;
			}
			else if (this._volume != value)
			{
				this.SyncVolume(value);
			}
		}
	}

	// Token: 0x06005276 RID: 21110 RVA: 0x001DD296 File Offset: 0x001DB696
	protected void SyncPitch(float f)
	{
		this._pitch = f;
		if (this.audioSource != null)
		{
			this.audioSource.pitch = this._pitch;
		}
	}

	// Token: 0x17000BF6 RID: 3062
	// (get) Token: 0x06005277 RID: 21111 RVA: 0x001DD2C1 File Offset: 0x001DB6C1
	// (set) Token: 0x06005278 RID: 21112 RVA: 0x001DD2C9 File Offset: 0x001DB6C9
	public float pitch
	{
		get
		{
			return this._pitch;
		}
		set
		{
			if (this.pitchJSON != null)
			{
				this.pitchJSON.val = value;
			}
			else if (this._pitch != value)
			{
				this.SyncPitch(value);
			}
		}
	}

	// Token: 0x06005279 RID: 21113 RVA: 0x001DD2FA File Offset: 0x001DB6FA
	protected void SyncStereoPan(float f)
	{
		this._stereoPan = f;
		if (this.audioSource != null)
		{
			this.audioSource.panStereo = this._stereoPan;
		}
	}

	// Token: 0x17000BF7 RID: 3063
	// (get) Token: 0x0600527A RID: 21114 RVA: 0x001DD325 File Offset: 0x001DB725
	// (set) Token: 0x0600527B RID: 21115 RVA: 0x001DD32D File Offset: 0x001DB72D
	public float stereoPan
	{
		get
		{
			return this._stereoPan;
		}
		set
		{
			if (this.stereoPanJSON != null)
			{
				this.stereoPanJSON.val = value;
			}
			else if (this._stereoPan != value)
			{
				this.SyncStereoPan(value);
			}
		}
	}

	// Token: 0x0600527C RID: 21116 RVA: 0x001DD35E File Offset: 0x001DB75E
	protected void SyncStereoSpread(float f)
	{
		this._stereoSpread = f;
		if (this.audioSource != null)
		{
			this.audioSource.spread = this._stereoSpread;
		}
	}

	// Token: 0x17000BF8 RID: 3064
	// (get) Token: 0x0600527D RID: 21117 RVA: 0x001DD389 File Offset: 0x001DB789
	// (set) Token: 0x0600527E RID: 21118 RVA: 0x001DD391 File Offset: 0x001DB791
	public float stereoSpread
	{
		get
		{
			return this._stereoSpread;
		}
		set
		{
			if (this.stereoSpreadJSON != null)
			{
				this.stereoSpreadJSON.val = value;
			}
			else if (this._stereoSpread != value)
			{
				this.SyncStereoSpread(value);
			}
		}
	}

	// Token: 0x0600527F RID: 21119 RVA: 0x001DD3C4 File Offset: 0x001DB7C4
	protected void SyncMinDistance(float f)
	{
		this._minDistance = f;
		if (this._maxDistance < this._minDistance + 0.1f)
		{
			this.maxDistance = this._minDistance + 0.1f;
		}
		if (this.audioSource != null)
		{
			this.audioSource.minDistance = this._minDistance;
		}
	}

	// Token: 0x17000BF9 RID: 3065
	// (get) Token: 0x06005280 RID: 21120 RVA: 0x001DD423 File Offset: 0x001DB823
	// (set) Token: 0x06005281 RID: 21121 RVA: 0x001DD42B File Offset: 0x001DB82B
	public float minDistance
	{
		get
		{
			return this._minDistance;
		}
		set
		{
			if (this.minDistanceJSON != null)
			{
				this.minDistanceJSON.val = value;
			}
			else if (this._minDistance != value)
			{
				this.SyncMinDistance(value);
			}
		}
	}

	// Token: 0x06005282 RID: 21122 RVA: 0x001DD45C File Offset: 0x001DB85C
	protected void SyncMaxDistance(float f)
	{
		this._maxDistance = f;
		if (this._maxDistance < this._minDistance + 0.1f)
		{
			this.minDistance = this._maxDistance - 0.1f;
		}
		if (this.audioSource != null)
		{
			this.audioSource.maxDistance = this._maxDistance;
		}
	}

	// Token: 0x17000BFA RID: 3066
	// (get) Token: 0x06005283 RID: 21123 RVA: 0x001DD4BB File Offset: 0x001DB8BB
	// (set) Token: 0x06005284 RID: 21124 RVA: 0x001DD4C3 File Offset: 0x001DB8C3
	public float maxDistance
	{
		get
		{
			return this._maxDistance;
		}
		set
		{
			if (this.maxDistanceJSON != null)
			{
				this.maxDistanceJSON.val = value;
			}
			else if (this._maxDistance != value)
			{
				this.SyncMaxDistance(value);
			}
		}
	}

	// Token: 0x06005285 RID: 21125 RVA: 0x001DD4F4 File Offset: 0x001DB8F4
	protected void SyncSpatialBlend(float f)
	{
		this._spatialBlend = f;
		if (this.audioSource != null)
		{
			this.audioSource.spatialBlend = this._spatialBlend;
		}
	}

	// Token: 0x17000BFB RID: 3067
	// (get) Token: 0x06005286 RID: 21126 RVA: 0x001DD51F File Offset: 0x001DB91F
	// (set) Token: 0x06005287 RID: 21127 RVA: 0x001DD527 File Offset: 0x001DB927
	public float spatialBlend
	{
		get
		{
			return this._spatialBlend;
		}
		set
		{
			if (this.spatialBlendJSON != null)
			{
				this.spatialBlendJSON.val = value;
			}
			else if (this._spatialBlend != value)
			{
				this.SyncSpatialBlend(value);
			}
		}
	}

	// Token: 0x06005288 RID: 21128 RVA: 0x001DD558 File Offset: 0x001DB958
	protected void SyncSpatialize(bool b)
	{
		this._spatialize = b;
		if (this.audioSource != null)
		{
			this.audioSource.spatialize = this._spatialize;
		}
	}

	// Token: 0x17000BFC RID: 3068
	// (get) Token: 0x06005289 RID: 21129 RVA: 0x001DD583 File Offset: 0x001DB983
	// (set) Token: 0x0600528A RID: 21130 RVA: 0x001DD58B File Offset: 0x001DB98B
	public bool spatialize
	{
		get
		{
			return this._spatialize;
		}
		set
		{
			if (this.spatializeJSON != null)
			{
				this.spatializeJSON.val = value;
			}
			else if (this._spatialize != value)
			{
				this.SyncSpatialize(value);
			}
		}
	}

	// Token: 0x0600528B RID: 21131 RVA: 0x001DD5BC File Offset: 0x001DB9BC
	protected void SyncRolloffToUI()
	{
		if (this.minDistanceJSON != null && this.minDistanceJSON.slider != null && this.minDistanceJSON.slider.transform.parent != null)
		{
			this.minDistanceJSON.slider.transform.parent.gameObject.SetActive(this._audioRolloffMode != AudioRolloffMode.Custom);
		}
		if (this.maxDistanceJSON != null && this.maxDistanceJSON.slider != null && this.maxDistanceJSON.slider.transform.parent != null)
		{
			this.maxDistanceJSON.slider.transform.parent.gameObject.SetActive(this._audioRolloffMode != AudioRolloffMode.Logarithmic);
		}
	}

	// Token: 0x0600528C RID: 21132 RVA: 0x001DD6A4 File Offset: 0x001DBAA4
	protected void SyncAudioRolloffMode(string s)
	{
		try
		{
			AudioRolloffMode audioRolloffMode = (AudioRolloffMode)Enum.Parse(typeof(AudioRolloffMode), s);
			this._audioRolloffMode = audioRolloffMode;
			if (this.audioSource != null)
			{
				this.audioSource.rolloffMode = this.audioRolloffMode;
			}
			this.SyncRolloffToUI();
		}
		catch (ArgumentException)
		{
			UnityEngine.Debug.LogError("Attempted to set audio rolloff mode to " + s + " which is not a valid type");
		}
	}

	// Token: 0x17000BFD RID: 3069
	// (get) Token: 0x0600528D RID: 21133 RVA: 0x001DD728 File Offset: 0x001DBB28
	// (set) Token: 0x0600528E RID: 21134 RVA: 0x001DD730 File Offset: 0x001DBB30
	public AudioRolloffMode audioRolloffMode
	{
		get
		{
			return this._audioRolloffMode;
		}
		set
		{
			if (this.audioRolloffModeJSON != null)
			{
				this.audioRolloffModeJSON.val = value.ToString();
			}
			else if (this._audioRolloffMode != value)
			{
				this.SyncAudioRolloffMode(value.ToString());
			}
		}
	}

	// Token: 0x0600528F RID: 21135 RVA: 0x001DD784 File Offset: 0x001DBB84
	protected void SyncDelayBetweenQueuedClips(float f)
	{
		this._delayBetweenQueuedClips = f;
	}

	// Token: 0x17000BFE RID: 3070
	// (get) Token: 0x06005290 RID: 21136 RVA: 0x001DD78D File Offset: 0x001DBB8D
	// (set) Token: 0x06005291 RID: 21137 RVA: 0x001DD795 File Offset: 0x001DBB95
	public float delayBetweenQueuedClips
	{
		get
		{
			return this._delayBetweenQueuedClips;
		}
		set
		{
			if (this.delayBetweenQueuedClipsJSON != null)
			{
				this.delayBetweenQueuedClipsJSON.val = value;
			}
			else if (this._delayBetweenQueuedClips != value)
			{
				this.SyncDelayBetweenQueuedClips(value);
			}
		}
	}

	// Token: 0x06005292 RID: 21138 RVA: 0x001DD7C6 File Offset: 0x001DBBC6
	protected void SyncVolumeTriggerQuickness(float f)
	{
		this._volumeTriggerQuickness = f;
	}

	// Token: 0x17000BFF RID: 3071
	// (get) Token: 0x06005293 RID: 21139 RVA: 0x001DD7CF File Offset: 0x001DBBCF
	// (set) Token: 0x06005294 RID: 21140 RVA: 0x001DD7D7 File Offset: 0x001DBBD7
	public float volumeTriggerQuickness
	{
		get
		{
			return this._volumeTriggerQuickness;
		}
		set
		{
			if (this.volumeTriggerQuicknessJSON != null)
			{
				this.volumeTriggerQuicknessJSON.val = value;
			}
			else if (this._volumeTriggerQuickness != value)
			{
				this.SyncVolumeTriggerQuickness(value);
			}
		}
	}

	// Token: 0x06005295 RID: 21141 RVA: 0x001DD808 File Offset: 0x001DBC08
	protected void SyncVolumeTriggerMultiplier(float f)
	{
		this._volumeTriggerMultiplier = f;
	}

	// Token: 0x17000C00 RID: 3072
	// (get) Token: 0x06005296 RID: 21142 RVA: 0x001DD811 File Offset: 0x001DBC11
	// (set) Token: 0x06005297 RID: 21143 RVA: 0x001DD819 File Offset: 0x001DBC19
	public float volumeTriggerMultiplier
	{
		get
		{
			return this._volumeTriggerMultiplier;
		}
		set
		{
			if (this.volumeTriggerMultiplierJSON != null)
			{
				this.volumeTriggerMultiplierJSON.val = value;
			}
			else if (this._volumeTriggerMultiplier != value)
			{
				this.SyncVolumeTriggerMultiplier(value);
			}
		}
	}

	// Token: 0x06005298 RID: 21144 RVA: 0x001DD84A File Offset: 0x001DBC4A
	protected void SyncEqualizeVolume(bool b)
	{
		this._equalizeVolume = b;
	}

	// Token: 0x17000C01 RID: 3073
	// (get) Token: 0x06005299 RID: 21145 RVA: 0x001DD853 File Offset: 0x001DBC53
	// (set) Token: 0x0600529A RID: 21146 RVA: 0x001DD85B File Offset: 0x001DBC5B
	public bool equalizeVolume
	{
		get
		{
			return this._equalizeVolume;
		}
		set
		{
			if (this.equalizeVolumeJSON != null)
			{
				this.equalizeVolumeJSON.val = value;
			}
			else if (this._equalizeVolume != value)
			{
				this.SyncEqualizeVolume(value);
			}
		}
	}

	// Token: 0x17000C02 RID: 3074
	// (get) Token: 0x0600529B RID: 21147 RVA: 0x001DD88C File Offset: 0x001DBC8C
	public NamedAudioClip playingClip
	{
		get
		{
			return this._playingClip;
		}
	}

	// Token: 0x17000C03 RID: 3075
	// (get) Token: 0x0600529C RID: 21148 RVA: 0x001DD894 File Offset: 0x001DBC94
	// (set) Token: 0x0600529D RID: 21149 RVA: 0x001DD89C File Offset: 0x001DBC9C
	public bool MicActive
	{
		[CompilerGenerated]
		get
		{
			return this.<MicActive>k__BackingField;
		}
		[CompilerGenerated]
		protected set
		{
			this.<MicActive>k__BackingField = value;
		}
	}

	// Token: 0x0600529E RID: 21150 RVA: 0x001DD8A8 File Offset: 0x001DBCA8
	protected virtual void StartMicrophoneInput()
	{
		this.EndMicrophoneInput();
		if (this.audioSource != null && Microphone.devices != null && Microphone.devices.Length > 0)
		{
			this.micDevice = Microphone.devices[0];
			int num;
			int num2;
			Microphone.GetDeviceCaps(this.micDevice, out num, out num2);
			int num3 = 48000;
			if (num3 > num2)
			{
				num3 = num2;
			}
			else if (num3 < num)
			{
				num3 = num;
			}
			UnityEngine.Debug.Log(string.Concat(new object[]
			{
				"Using mic ",
				this.micDevice,
				" min freq: ",
				num,
				" max freq: ",
				num2,
				" set freq: ",
				num3
			}));
			AudioClip audioClip = Microphone.Start(this.micDevice, true, 10, num3);
			if (audioClip != null)
			{
				while (Microphone.GetPosition(this.micDevice) <= 0)
				{
				}
				this.PlayClip(new NamedAudioClip
				{
					sourceClip = audioClip,
					displayName = "Microphone: " + this.micDevice
				}, true);
				this.MicActive = true;
				if (this.onMicStartHandlers != null)
				{
					this.onMicStartHandlers();
				}
			}
			else
			{
				UnityEngine.Debug.LogError("Failed to get audio clip from microphone");
			}
		}
	}

	// Token: 0x0600529F RID: 21151 RVA: 0x001DDA00 File Offset: 0x001DBE00
	protected virtual void EndMicrophoneInput()
	{
		if (this.micDevice != null)
		{
			this.MicActive = false;
			Microphone.End(this.micDevice);
			this.micDevice = null;
			this.ClearPlayingClip();
			if (this.onMicStopHandlers != null)
			{
				this.onMicStopHandlers();
			}
		}
	}

	// Token: 0x060052A0 RID: 21152 RVA: 0x001DDA50 File Offset: 0x001DBE50
	public void PlayNext(NamedAudioClip nac)
	{
		if (this.audioSource != null && nac.clipToPlay != null)
		{
			if (this.audioSource.isPlaying || this.isPaused || this.wasFrozen)
			{
				this.queue.AddFirst(nac);
			}
			else
			{
				this.EndMicrophoneInput();
				this.PlayClip(nac, false);
			}
		}
	}

	// Token: 0x060052A1 RID: 21153 RVA: 0x001DDAC5 File Offset: 0x001DBEC5
	public void PlayNextClearQueue(NamedAudioClip nac)
	{
		this.queue.Clear();
		this.QueueClip(nac);
	}

	// Token: 0x060052A2 RID: 21154 RVA: 0x001DDADC File Offset: 0x001DBEDC
	public void QueueClip(NamedAudioClip nac)
	{
		if (this.audioSource != null && nac.clipToPlay != null)
		{
			if (this.audioSource.isPlaying || this.isPaused || this.wasFrozen)
			{
				this.queue.AddLast(nac);
			}
			else
			{
				this.EndMicrophoneInput();
				this.PlayClip(nac, false);
			}
		}
	}

	// Token: 0x060052A3 RID: 21155 RVA: 0x001DDB51 File Offset: 0x001DBF51
	public void PlayNow(NamedAudioClip nac)
	{
		this.EndMicrophoneInput();
		this.PlayClip(nac, false);
	}

	// Token: 0x060052A4 RID: 21156 RVA: 0x001DDB64 File Offset: 0x001DBF64
	public void PlayIfClear(NamedAudioClip nac)
	{
		if (this.audioSource != null && !this.audioSource.isPlaying && !this.isPaused && !this.wasFrozen)
		{
			this.EndMicrophoneInput();
			this.PlayClip(nac, false);
		}
	}

	// Token: 0x060052A5 RID: 21157 RVA: 0x001DDBB6 File Offset: 0x001DBFB6
	public void PlayNowLoop(NamedAudioClip nac)
	{
		this.queue.Clear();
		this.EndMicrophoneInput();
		this.PlayClip(nac, true);
	}

	// Token: 0x060052A6 RID: 21158 RVA: 0x001DDBD1 File Offset: 0x001DBFD1
	public void PlayNowClearQueue(NamedAudioClip nac)
	{
		this.queue.Clear();
		this.EndMicrophoneInput();
		this.PlayClip(nac, false);
	}

	// Token: 0x060052A7 RID: 21159 RVA: 0x001DDBEC File Offset: 0x001DBFEC
	protected virtual void ClearPlayingClip()
	{
		this.EndMicrophoneInput();
		if (this.audioSource != null)
		{
			this.audioSource.clip = null;
		}
		this._playingClip = null;
		if (this.playingClipNameText != null)
		{
			this.playingClipNameText.text = string.Empty;
		}
		if (this.playingClipNameTextAlt != null)
		{
			this.playingClipNameTextAlt.text = string.Empty;
		}
	}

	// Token: 0x060052A8 RID: 21160 RVA: 0x001DDC68 File Offset: 0x001DC068
	protected virtual void PlayClip(NamedAudioClip nac, bool loopClip = false)
	{
		if (this.audioSource != null && nac.clipToPlay != null)
		{
			this.loop = loopClip;
			this._playingClip = nac;
			this.timeSinceClipFinished = 0f;
			this.audioSource.clip = nac.clipToPlay;
			this.audioSource.time = 0f;
			this.isPaused = false;
			this.audioSource.Play();
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

	// Token: 0x060052A9 RID: 21161 RVA: 0x001DDD31 File Offset: 0x001DC131
	public void Pause()
	{
		if (this.audioSource != null)
		{
			this.isPaused = true;
			this.audioSource.Pause();
		}
	}

	// Token: 0x060052AA RID: 21162 RVA: 0x001DDD56 File Offset: 0x001DC156
	public void StopLoop()
	{
		this.loop = false;
	}

	// Token: 0x060052AB RID: 21163 RVA: 0x001DDD5F File Offset: 0x001DC15F
	public void Stop()
	{
		if (this.audioSource != null)
		{
			this.isPaused = false;
			this.audioSource.Stop();
		}
	}

	// Token: 0x060052AC RID: 21164 RVA: 0x001DDD84 File Offset: 0x001DC184
	public void UnPause()
	{
		if (this.audioSource != null)
		{
			this.isPaused = false;
			this.audioSource.UnPause();
		}
	}

	// Token: 0x060052AD RID: 21165 RVA: 0x001DDDAC File Offset: 0x001DC1AC
	public void TogglePause()
	{
		if (this.audioSource != null)
		{
			if (this.isPaused)
			{
				this.isPaused = false;
				this.audioSource.UnPause();
			}
			else
			{
				this.isPaused = true;
				this.audioSource.Pause();
			}
		}
	}

	// Token: 0x060052AE RID: 21166 RVA: 0x001DDDFE File Offset: 0x001DC1FE
	public void StopAndClearQueue()
	{
		this.queue.Clear();
		this.Stop();
	}

	// Token: 0x060052AF RID: 21167 RVA: 0x001DDE11 File Offset: 0x001DC211
	public void ClearQueue()
	{
		this.queue.Clear();
	}

	// Token: 0x060052B0 RID: 21168 RVA: 0x001DDE1E File Offset: 0x001DC21E
	protected override void CreateFloatJSON()
	{
		this.floatJSON = new JSONStorableFloat("audioVolume", 0f, new JSONStorableFloat.SetFloatCallback(base.SyncFloat), 0f, 1f, true, false);
	}

	// Token: 0x060052B1 RID: 21169 RVA: 0x001DDE50 File Offset: 0x001DC250
	protected override void Init()
	{
		base.Init();
		this.queue = new LinkedList<NamedAudioClip>();
		this.clipSampleData = new float[this.sampleDataLength];
		if (this.audioSource != null)
		{
			this._loop = this.audioSource.loop;
			this._volume = this.audioSource.volume;
			this._pitch = this.audioSource.pitch;
			this._stereoPan = this.audioSource.panStereo;
			this._minDistance = this.audioSource.minDistance;
			this._maxDistance = this.audioSource.maxDistance;
			this._spatialBlend = this.audioSource.spatialBlend;
			this._stereoSpread = this.audioSource.spread;
			this._spatialize = this.audioSource.spatialize;
			this._audioRolloffMode = this.audioSource.rolloffMode;
			this.loopJSON = new JSONStorableBool("loop", this._loop, new JSONStorableBool.SetBoolCallback(this.SyncLoop));
			this.loopJSON.storeType = JSONStorableParam.StoreType.Full;
			base.RegisterBool(this.loopJSON);
			this.volumeJSON = new JSONStorableFloat("volume", this._volume, new JSONStorableFloat.SetFloatCallback(this.SyncVolume), 0f, 1f, true, true);
			this.volumeJSON.storeType = JSONStorableParam.StoreType.Full;
			base.RegisterFloat(this.volumeJSON);
			this.pitchJSON = new JSONStorableFloat("pitch", this._pitch, new JSONStorableFloat.SetFloatCallback(this.SyncPitch), -3f, 3f, true, true);
			this.pitchJSON.storeType = JSONStorableParam.StoreType.Full;
			base.RegisterFloat(this.pitchJSON);
			this.stereoPanJSON = new JSONStorableFloat("stereoPan", this._stereoPan, new JSONStorableFloat.SetFloatCallback(this.SyncStereoPan), -1f, 1f, true, true);
			this.stereoPanJSON.storeType = JSONStorableParam.StoreType.Full;
			base.RegisterFloat(this.stereoPanJSON);
			this.minDistanceJSON = new JSONStorableFloat("minDistance", this._minDistance, new JSONStorableFloat.SetFloatCallback(this.SyncMinDistance), 0f, 10f, true, true);
			this.minDistanceJSON.storeType = JSONStorableParam.StoreType.Full;
			base.RegisterFloat(this.minDistanceJSON);
			this.maxDistanceJSON = new JSONStorableFloat("maxDistance", this._maxDistance, new JSONStorableFloat.SetFloatCallback(this.SyncMaxDistance), 1f, 500f, true, true);
			this.maxDistanceJSON.storeType = JSONStorableParam.StoreType.Full;
			base.RegisterFloat(this.maxDistanceJSON);
			this.spatialBlendJSON = new JSONStorableFloat("spatialBlend", this._spatialBlend, new JSONStorableFloat.SetFloatCallback(this.SyncSpatialBlend), 0f, 1f, true, true);
			this.spatialBlendJSON.storeType = JSONStorableParam.StoreType.Full;
			base.RegisterFloat(this.spatialBlendJSON);
			this.stereoSpreadJSON = new JSONStorableFloat("stereoSpread", this._stereoSpread, new JSONStorableFloat.SetFloatCallback(this.SyncStereoSpread), 0f, 360f, true, true);
			this.stereoSpreadJSON.storeType = JSONStorableParam.StoreType.Full;
			base.RegisterFloat(this.stereoSpreadJSON);
			this.spatializeJSON = new JSONStorableBool("spatialize", this._spatialize, new JSONStorableBool.SetBoolCallback(this.SyncSpatialize));
			this.spatializeJSON.storeType = JSONStorableParam.StoreType.Full;
			base.RegisterBool(this.spatializeJSON);
			List<string> choicesList = new List<string>(Enum.GetNames(typeof(AudioRolloffMode)));
			this.audioRolloffModeJSON = new JSONStorableStringChooser("audioRolloffMode", choicesList, new List<string>
			{
				"Logarithmic",
				"Linear",
				"Natural"
			}, this._audioRolloffMode.ToString(), "Audio Rolloff Mode", new JSONStorableStringChooser.SetStringCallback(this.SyncAudioRolloffMode));
			this.audioRolloffModeJSON.storeType = JSONStorableParam.StoreType.Full;
			base.RegisterStringChooser(this.audioRolloffModeJSON);
			this.delayBetweenQueuedClipsJSON = new JSONStorableFloat("delayBetweenQueuedClips", this._delayBetweenQueuedClips, new JSONStorableFloat.SetFloatCallback(this.SyncDelayBetweenQueuedClips), 0f, 10f, true, true);
			this.delayBetweenQueuedClipsJSON.storeType = JSONStorableParam.StoreType.Full;
			base.RegisterFloat(this.delayBetweenQueuedClipsJSON);
			this.volumeTriggerQuicknessJSON = new JSONStorableFloat("volumeTriggerQuickness", this._volumeTriggerQuickness, new JSONStorableFloat.SetFloatCallback(this.SyncVolumeTriggerQuickness), 1f, 50f, true, true);
			this.volumeTriggerQuicknessJSON.storeType = JSONStorableParam.StoreType.Full;
			base.RegisterFloat(this.volumeTriggerQuicknessJSON);
			this.volumeTriggerMultiplierJSON = new JSONStorableFloat("volumeTriggerMultiplier", this._volumeTriggerMultiplier, new JSONStorableFloat.SetFloatCallback(this.SyncVolumeTriggerMultiplier), 0f, 100f, true, true);
			this.volumeTriggerMultiplierJSON.storeType = JSONStorableParam.StoreType.Full;
			base.RegisterFloat(this.volumeTriggerMultiplierJSON);
			this.equalizeVolumeJSON = new JSONStorableBool("equalizeVolume", this._equalizeVolume, new JSONStorableBool.SetBoolCallback(this.SyncEqualizeVolume));
			this.equalizeVolumeJSON.storeType = JSONStorableParam.StoreType.Full;
			base.RegisterBool(this.equalizeVolumeJSON);
			this.startMicrophoneInputAction = new JSONStorableAction("StartMicrophoneInput", new JSONStorableAction.ActionCallback(this.StartMicrophoneInput));
			this.endMicrophoneInputAction = new JSONStorableAction("EndMicrophoneInput", new JSONStorableAction.ActionCallback(this.EndMicrophoneInput));
			this.playIfClearJSONAction = new JSONStorableActionAudioClip("PlayIfClear", new JSONStorableActionAudioClip.AudioClipActionCallback(this.PlayIfClear));
			base.RegisterAudioClipAction(this.playIfClearJSONAction);
			this.playNowJSONAction = new JSONStorableActionAudioClip("PlayNow", new JSONStorableActionAudioClip.AudioClipActionCallback(this.PlayNow));
			base.RegisterAudioClipAction(this.playNowJSONAction);
			this.playNowLoopJSONAction = new JSONStorableActionAudioClip("PlayNowLoop", new JSONStorableActionAudioClip.AudioClipActionCallback(this.PlayNowLoop));
			base.RegisterAudioClipAction(this.playNowLoopJSONAction);
			this.playNowClearQueueJSONAction = new JSONStorableActionAudioClip("PlayNowClearQueue", new JSONStorableActionAudioClip.AudioClipActionCallback(this.PlayNowClearQueue));
			base.RegisterAudioClipAction(this.playNowClearQueueJSONAction);
			this.playNextJSONAction = new JSONStorableActionAudioClip("PlayNext", new JSONStorableActionAudioClip.AudioClipActionCallback(this.PlayNext));
			base.RegisterAudioClipAction(this.playNextJSONAction);
			this.playNextClearQueueJSONAction = new JSONStorableActionAudioClip("PlayNextClearQueue", new JSONStorableActionAudioClip.AudioClipActionCallback(this.PlayNextClearQueue));
			base.RegisterAudioClipAction(this.playNextClearQueueJSONAction);
			this.queueClipJSONAction = new JSONStorableActionAudioClip("Queue", new JSONStorableActionAudioClip.AudioClipActionCallback(this.QueueClip));
			base.RegisterAudioClipAction(this.queueClipJSONAction);
			this.pauseJSONAction = new JSONStorableAction("Pause", new JSONStorableAction.ActionCallback(this.Pause));
			base.RegisterAction(this.pauseJSONAction);
			this.unpauseJSONAction = new JSONStorableAction("UnPause", new JSONStorableAction.ActionCallback(this.UnPause));
			base.RegisterAction(this.unpauseJSONAction);
			this.togglePauseJSONAction = new JSONStorableAction("TogglePause", new JSONStorableAction.ActionCallback(this.TogglePause));
			base.RegisterAction(this.togglePauseJSONAction);
			this.stopLoopJSONAction = new JSONStorableAction("StopLoop", new JSONStorableAction.ActionCallback(this.StopLoop));
			base.RegisterAction(this.stopLoopJSONAction);
			this.stopJSONAction = new JSONStorableAction("Stop", new JSONStorableAction.ActionCallback(this.Stop));
			base.RegisterAction(this.stopJSONAction);
			this.stopAndClearQueueJSONAction = new JSONStorableAction("StopAndClearQueue", new JSONStorableAction.ActionCallback(this.StopAndClearQueue));
			base.RegisterAction(this.stopAndClearQueueJSONAction);
			this.clearQueueJSONAction = new JSONStorableAction("ClearQueue", new JSONStorableAction.ActionCallback(this.ClearQueue));
			base.RegisterAction(this.clearQueueJSONAction);
			this.recentMaxVolumeJSON = new JSONStorableFloat("recentMaxVolume", 0f, 0f, 1f, true, false);
		}
	}

	// Token: 0x060052B2 RID: 21170 RVA: 0x001DE5B8 File Offset: 0x001DC9B8
	public override void InitUI()
	{
		base.InitUI();
		if (this.UITransform != null)
		{
			AudioSourceControlUI componentInChildren = this.UITransform.GetComponentInChildren<AudioSourceControlUI>(true);
			if (componentInChildren != null)
			{
				this.playingClipNameText = componentInChildren.playingClipNameText;
				if (this._playingClip != null && this.playingClipNameText != null)
				{
					this.playingClipNameText.text = this._playingClip.displayName;
				}
				this.loopJSON.toggle = componentInChildren.loopToggle;
				this.volumeJSON.slider = componentInChildren.volumeSlider;
				this.pitchJSON.slider = componentInChildren.pitchSlider;
				this.stereoPanJSON.slider = componentInChildren.stereoPanSlider;
				this.minDistanceJSON.slider = componentInChildren.minDistanceSlider;
				this.maxDistanceJSON.slider = componentInChildren.maxDistanceSlider;
				this.spatialBlendJSON.slider = componentInChildren.spatialBlendSlider;
				this.stereoSpreadJSON.slider = componentInChildren.stereoSpreadSlider;
				this.spatializeJSON.toggle = componentInChildren.spatializeToggle;
				this.audioRolloffModeJSON.popup = componentInChildren.audioRolloffModePopup;
				this.delayBetweenQueuedClipsJSON.slider = componentInChildren.delayBetweenQueuedClipsSlider;
				this.volumeTriggerQuicknessJSON.slider = componentInChildren.volumeTriggerQuicknessSlider;
				this.volumeTriggerMultiplierJSON.slider = componentInChildren.volumeTriggerMultiplierSlider;
				this.equalizeVolumeJSON.toggle = componentInChildren.equalizeVolumeSlider;
				this.startMicrophoneInputAction.button = componentInChildren.startMicrophoneInputButton;
				this.endMicrophoneInputAction.button = componentInChildren.endMicrophoneInputButton;
				this.recentMaxVolumeJSON.slider = componentInChildren.recentMaxVolumeSlider;
				this.SyncRolloffToUI();
			}
		}
	}

	// Token: 0x060052B3 RID: 21171 RVA: 0x001DE75C File Offset: 0x001DCB5C
	public override void InitUIAlt()
	{
		base.InitUIAlt();
		if (this.UITransformAlt != null)
		{
			AudioSourceControlUI componentInChildren = this.UITransformAlt.GetComponentInChildren<AudioSourceControlUI>(true);
			if (componentInChildren != null)
			{
				this.playingClipNameTextAlt = componentInChildren.playingClipNameText;
				if (this._playingClip != null && this.playingClipNameTextAlt != null)
				{
					this.playingClipNameTextAlt.text = this._playingClip.displayName;
				}
				this.loopJSON.toggleAlt = componentInChildren.loopToggle;
				this.volumeJSON.sliderAlt = componentInChildren.volumeSlider;
				this.pitchJSON.sliderAlt = componentInChildren.pitchSlider;
				this.stereoPanJSON.sliderAlt = componentInChildren.stereoPanSlider;
				this.minDistanceJSON.sliderAlt = componentInChildren.minDistanceSlider;
				this.maxDistanceJSON.sliderAlt = componentInChildren.maxDistanceSlider;
				this.spatialBlendJSON.sliderAlt = componentInChildren.spatialBlendSlider;
				this.stereoSpreadJSON.sliderAlt = componentInChildren.stereoSpreadSlider;
				this.spatializeJSON.toggleAlt = componentInChildren.spatializeToggle;
				this.audioRolloffModeJSON.popupAlt = componentInChildren.audioRolloffModePopup;
				this.delayBetweenQueuedClipsJSON.sliderAlt = componentInChildren.delayBetweenQueuedClipsSlider;
				this.volumeTriggerQuicknessJSON.sliderAlt = componentInChildren.volumeTriggerQuicknessSlider;
				this.volumeTriggerMultiplierJSON.sliderAlt = componentInChildren.volumeTriggerMultiplierSlider;
				this.equalizeVolumeJSON.toggleAlt = componentInChildren.equalizeVolumeSlider;
				this.startMicrophoneInputAction.buttonAlt = componentInChildren.startMicrophoneInputButton;
				this.endMicrophoneInputAction.buttonAlt = componentInChildren.endMicrophoneInputButton;
				this.recentMaxVolumeJSON.sliderAlt = componentInChildren.recentMaxVolumeSlider;
			}
		}
	}

	// Token: 0x060052B4 RID: 21172 RVA: 0x001DE8F8 File Offset: 0x001DCCF8
	protected virtual void Update()
	{
		if (this.audioSource != null)
		{
			if (SuperController.singleton != null && SuperController.singleton.freezeAnimation)
			{
				if (this.audioSource.isPlaying)
				{
					this.wasFrozen = true;
					this.audioSource.Pause();
				}
			}
			else if (this.wasFrozen)
			{
				this.wasFrozen = false;
				this.audioSource.UnPause();
			}
			else
			{
				if (this.audioSource.isPlaying)
				{
					this.timeSinceClipFinished = 0f;
					this.audioSource.GetOutputData(this.clipSampleData, 0);
					this.clipLoudness = 0f;
					foreach (float f in this.clipSampleData)
					{
						this.clipLoudness += Mathf.Abs(f);
					}
					this.clipLoudness /= (float)this.sampleDataLength;
					if (this._equalizeVolume)
					{
						this.clipLoudness = ExtractFeatures.EqualizeDistance(this.clipLoudness, this.audioSource, SuperController.singleton.CurrentAudioListener);
					}
				}
				else if (this.isPaused)
				{
					this.timeSinceClipFinished = 0f;
					this.clipLoudness = 0f;
				}
				else
				{
					this.clipLoudness = 0f;
					this._playingClip = null;
					if (this.audioSource.clip != null)
					{
						this.audioSource.clip = null;
					}
					if (this.playingClipNameText != null)
					{
						this.playingClipNameText.text = string.Empty;
					}
					if (this.playingClipNameTextAlt != null)
					{
						this.playingClipNameTextAlt.text = string.Empty;
					}
					this.timeSinceClipFinished += Time.unscaledDeltaTime;
				}
				this.smoothedClipLoudness = Mathf.Lerp(this.floatJSON.val, Mathf.Clamp01(this.clipLoudness * this._volumeTriggerMultiplier), Time.deltaTime * this._volumeTriggerQuickness);
				this.floatJSON.val = this.smoothedClipLoudness;
				if (this.smoothedClipLoudness > this.recentMaxLoudness)
				{
					this.recentMaxLoudness = this.smoothedClipLoudness;
					this.timeSinceLastMaxLoudness = 0f;
				}
				this.timeSinceLastMaxLoudness += Time.deltaTime;
				if (this.timeSinceLastMaxLoudness > this.timeToResetMaxLoudness)
				{
					this.recentMaxLoudness = this.smoothedClipLoudness;
					this.timeSinceLastMaxLoudness = 0f;
				}
				this.recentMaxVolumeJSON.val = this.recentMaxLoudness;
			}
			if (this.queue != null && this.queue.Count > 0 && this.timeSinceClipFinished > this.delayBetweenQueuedClips)
			{
				NamedAudioClip value = this.queue.First.Value;
				this.queue.RemoveFirst();
				this.PlayClip(value, false);
			}
		}
	}

	// Token: 0x04004244 RID: 16964
	public AudioSource audioSource;

	// Token: 0x04004245 RID: 16965
	protected Text playingClipNameText;

	// Token: 0x04004246 RID: 16966
	protected Text playingClipNameTextAlt;

	// Token: 0x04004247 RID: 16967
	protected JSONStorableBool loopJSON;

	// Token: 0x04004248 RID: 16968
	protected bool _loop;

	// Token: 0x04004249 RID: 16969
	protected JSONStorableFloat volumeJSON;

	// Token: 0x0400424A RID: 16970
	protected float _volume = 1f;

	// Token: 0x0400424B RID: 16971
	protected JSONStorableFloat pitchJSON;

	// Token: 0x0400424C RID: 16972
	protected float _pitch = 1f;

	// Token: 0x0400424D RID: 16973
	protected JSONStorableFloat stereoPanJSON;

	// Token: 0x0400424E RID: 16974
	protected float _stereoPan;

	// Token: 0x0400424F RID: 16975
	protected JSONStorableFloat stereoSpreadJSON;

	// Token: 0x04004250 RID: 16976
	protected float _stereoSpread;

	// Token: 0x04004251 RID: 16977
	protected JSONStorableFloat minDistanceJSON;

	// Token: 0x04004252 RID: 16978
	protected float _minDistance = 0.5f;

	// Token: 0x04004253 RID: 16979
	protected JSONStorableFloat maxDistanceJSON;

	// Token: 0x04004254 RID: 16980
	protected float _maxDistance = 500f;

	// Token: 0x04004255 RID: 16981
	protected JSONStorableFloat spatialBlendJSON;

	// Token: 0x04004256 RID: 16982
	protected float _spatialBlend = 1f;

	// Token: 0x04004257 RID: 16983
	protected JSONStorableBool spatializeJSON;

	// Token: 0x04004258 RID: 16984
	protected bool _spatialize = true;

	// Token: 0x04004259 RID: 16985
	protected JSONStorableStringChooser audioRolloffModeJSON;

	// Token: 0x0400425A RID: 16986
	protected AudioRolloffMode _audioRolloffMode;

	// Token: 0x0400425B RID: 16987
	protected JSONStorableFloat delayBetweenQueuedClipsJSON;

	// Token: 0x0400425C RID: 16988
	protected float _delayBetweenQueuedClips;

	// Token: 0x0400425D RID: 16989
	protected JSONStorableFloat volumeTriggerQuicknessJSON;

	// Token: 0x0400425E RID: 16990
	protected float _volumeTriggerQuickness = 2.5f;

	// Token: 0x0400425F RID: 16991
	protected JSONStorableFloat volumeTriggerMultiplierJSON;

	// Token: 0x04004260 RID: 16992
	protected float _volumeTriggerMultiplier = 5f;

	// Token: 0x04004261 RID: 16993
	protected JSONStorableBool equalizeVolumeJSON;

	// Token: 0x04004262 RID: 16994
	protected bool _equalizeVolume;

	// Token: 0x04004263 RID: 16995
	protected float delayTimer;

	// Token: 0x04004264 RID: 16996
	protected LinkedList<NamedAudioClip> queue;

	// Token: 0x04004265 RID: 16997
	protected NamedAudioClip _playingClip;

	// Token: 0x04004266 RID: 16998
	protected JSONStorableAction startMicrophoneInputAction;

	// Token: 0x04004267 RID: 16999
	protected string micDevice;

	// Token: 0x04004268 RID: 17000
	public AudioSourceControl.OnMicStart onMicStartHandlers;

	// Token: 0x04004269 RID: 17001
	[CompilerGenerated]
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private bool <MicActive>k__BackingField;

	// Token: 0x0400426A RID: 17002
	public AudioSourceControl.OnMicStop onMicStopHandlers;

	// Token: 0x0400426B RID: 17003
	protected JSONStorableAction endMicrophoneInputAction;

	// Token: 0x0400426C RID: 17004
	protected JSONStorableActionAudioClip playNextJSONAction;

	// Token: 0x0400426D RID: 17005
	protected JSONStorableActionAudioClip playNextClearQueueJSONAction;

	// Token: 0x0400426E RID: 17006
	protected JSONStorableActionAudioClip queueClipJSONAction;

	// Token: 0x0400426F RID: 17007
	protected JSONStorableActionAudioClip playNowJSONAction;

	// Token: 0x04004270 RID: 17008
	protected JSONStorableActionAudioClip playIfClearJSONAction;

	// Token: 0x04004271 RID: 17009
	protected JSONStorableActionAudioClip playNowLoopJSONAction;

	// Token: 0x04004272 RID: 17010
	protected JSONStorableActionAudioClip playNowClearQueueJSONAction;

	// Token: 0x04004273 RID: 17011
	protected bool isPaused;

	// Token: 0x04004274 RID: 17012
	protected JSONStorableAction pauseJSONAction;

	// Token: 0x04004275 RID: 17013
	protected JSONStorableAction stopLoopJSONAction;

	// Token: 0x04004276 RID: 17014
	protected JSONStorableAction stopJSONAction;

	// Token: 0x04004277 RID: 17015
	protected JSONStorableAction unpauseJSONAction;

	// Token: 0x04004278 RID: 17016
	protected JSONStorableAction togglePauseJSONAction;

	// Token: 0x04004279 RID: 17017
	protected JSONStorableAction stopAndClearQueueJSONAction;

	// Token: 0x0400427A RID: 17018
	protected JSONStorableAction clearQueueJSONAction;

	// Token: 0x0400427B RID: 17019
	protected float timeSinceClipFinished;

	// Token: 0x0400427C RID: 17020
	protected JSONStorableFloat recentMaxVolumeJSON;

	// Token: 0x0400427D RID: 17021
	public int sampleDataLength = 128;

	// Token: 0x0400427E RID: 17022
	private bool wasFrozen;

	// Token: 0x0400427F RID: 17023
	private float timeToResetMaxLoudness = 1f;

	// Token: 0x04004280 RID: 17024
	private float timeSinceLastMaxLoudness;

	// Token: 0x04004281 RID: 17025
	public float recentMaxLoudness;

	// Token: 0x04004282 RID: 17026
	public float clipLoudness;

	// Token: 0x04004283 RID: 17027
	public float smoothedClipLoudness;

	// Token: 0x04004284 RID: 17028
	private float[] clipSampleData;

	// Token: 0x02000B7A RID: 2938
	// (Invoke) Token: 0x060052B6 RID: 21174
	public delegate void OnMicStart();

	// Token: 0x02000B7B RID: 2939
	// (Invoke) Token: 0x060052BA RID: 21178
	public delegate void OnMicStop();
}
