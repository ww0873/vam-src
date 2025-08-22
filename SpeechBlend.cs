using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;
using SpeechBlendEngine;
using UnityEngine;

// Token: 0x02000445 RID: 1093
public class SpeechBlend : MonoBehaviour
{
	// Token: 0x06001B3A RID: 6970 RVA: 0x00097C0C File Offset: 0x0009600C
	public SpeechBlend()
	{
	}

	// Token: 0x17000302 RID: 770
	// (get) Token: 0x06001B3B RID: 6971 RVA: 0x00097D4F File Offset: 0x0009614F
	protected bool useUnitySpectrumMethod
	{
		get
		{
			return this.useUnitySpectrum || this.liveMode || !this.hasClip;
		}
	}

	// Token: 0x06001B3C RID: 6972 RVA: 0x00097D74 File Offset: 0x00096174
	private float GetLookaheadTime()
	{
		float result = 0f;
		if (this.useLookahead)
		{
			int num = Mathf.FloorToInt(this.timeBetweenSampling / Time.fixedDeltaTime) + 1;
			float num2 = (float)(num + 2) * Time.fixedDeltaTime;
			result = num2 + this.lookaheadAdjust;
		}
		return result;
	}

	// Token: 0x06001B3D RID: 6973 RVA: 0x00097DBC File Offset: 0x000961BC
	public void InitMorphs()
	{
		this.ClearMorphs();
		if (this._morphBank != null)
		{
			this._morphBank.Init();
			SpeechUtil.VisemeBlendshapeNames visemeBlendshapeNames = this.faceBlendshapeNamesArray[this.setChoice];
			if (visemeBlendshapeNames != null)
			{
				if (this.visemeMorphs == null || this.visemeMorphs.Length != visemeBlendshapeNames.visemeNames.Length)
				{
					this.visemeMorphs = new DAZMorph[visemeBlendshapeNames.visemeNames.Length];
				}
				if (this.VisemeRawValues == null || this.VisemeRawValues.Length != visemeBlendshapeNames.visemeNames.Length)
				{
					this.VisemeRawValues = new float[visemeBlendshapeNames.visemeNames.Length];
				}
				if (this.VisemeValues == null || this.VisemeValues.Length != visemeBlendshapeNames.visemeNames.Length)
				{
					this.VisemeValues = new float[visemeBlendshapeNames.visemeNames.Length];
				}
				for (int i = 0; i < visemeBlendshapeNames.visemeNames.Length; i++)
				{
					if (string.IsNullOrEmpty(visemeBlendshapeNames.visemeNames[i]))
					{
						this.visemeMorphs[i] = null;
						UnityEngine.Debug.LogError("Morph not set for viseme " + visemeBlendshapeNames.template.visemeNames[i]);
					}
					else
					{
						if (this._useBuiltInMorphs)
						{
							this.visemeMorphs[i] = this._morphBank.GetBuiltInMorphByUid(visemeBlendshapeNames.visemeNames[i]);
						}
						else
						{
							this.visemeMorphs[i] = this._morphBank.GetMorphByUid(visemeBlendshapeNames.visemeNames[i]);
						}
						if (this.visemeMorphs[i] == null)
						{
							UnityEngine.Debug.LogError("Could not find morph " + visemeBlendshapeNames.visemeNames[i] + " for viseme " + visemeBlendshapeNames.template.visemeNames[i]);
						}
					}
				}
				if (string.IsNullOrEmpty(visemeBlendshapeNames.mouthOpenName))
				{
					UnityEngine.Debug.LogError("Morph not set for mouth open");
				}
				else
				{
					if (this._useBuiltInMorphs)
					{
						this.mouthOpenMorph = this._morphBank.GetBuiltInMorphByUid(visemeBlendshapeNames.mouthOpenName);
					}
					else
					{
						this.mouthOpenMorph = this._morphBank.GetMorphByUid(visemeBlendshapeNames.mouthOpenName);
					}
					if (this.mouthOpenMorph == null)
					{
						UnityEngine.Debug.LogError("Could not find mouth open morph " + visemeBlendshapeNames.mouthOpenName);
					}
				}
			}
			else
			{
				this.visemeMorphs = null;
			}
		}
	}

	// Token: 0x06001B3E RID: 6974 RVA: 0x00097FF4 File Offset: 0x000963F4
	protected void ClearMorphs()
	{
		if (this.visemeMorphs != null)
		{
			for (int i = 0; i < this.visemeMorphs.Length; i++)
			{
				if (this.visemeMorphs[i] != null)
				{
					this.visemeMorphs[i].morphValue = 0f;
				}
			}
			this.visemeMorphs = null;
		}
		if (this.mouthOpenMorph != null)
		{
			this.mouthOpenMorph.morphValue = 0f;
			this.mouthOpenMorph = null;
		}
	}

	// Token: 0x17000303 RID: 771
	// (get) Token: 0x06001B3F RID: 6975 RVA: 0x0009806D File Offset: 0x0009646D
	public DAZMorph[] VisemeMorphs
	{
		get
		{
			return this.visemeMorphs;
		}
	}

	// Token: 0x17000304 RID: 772
	// (get) Token: 0x06001B40 RID: 6976 RVA: 0x00098075 File Offset: 0x00096475
	// (set) Token: 0x06001B41 RID: 6977 RVA: 0x0009807D File Offset: 0x0009647D
	public float[] VisemeRawValues
	{
		[CompilerGenerated]
		get
		{
			return this.<VisemeRawValues>k__BackingField;
		}
		[CompilerGenerated]
		protected set
		{
			this.<VisemeRawValues>k__BackingField = value;
		}
	}

	// Token: 0x17000305 RID: 773
	// (get) Token: 0x06001B42 RID: 6978 RVA: 0x00098086 File Offset: 0x00096486
	// (set) Token: 0x06001B43 RID: 6979 RVA: 0x0009808E File Offset: 0x0009648E
	public float[] VisemeValues
	{
		[CompilerGenerated]
		get
		{
			return this.<VisemeValues>k__BackingField;
		}
		[CompilerGenerated]
		protected set
		{
			this.<VisemeValues>k__BackingField = value;
		}
	}

	// Token: 0x17000306 RID: 774
	// (get) Token: 0x06001B44 RID: 6980 RVA: 0x00098097 File Offset: 0x00096497
	public DAZMorph MouthOpenMorph
	{
		get
		{
			return this.mouthOpenMorph;
		}
	}

	// Token: 0x17000307 RID: 775
	// (get) Token: 0x06001B45 RID: 6981 RVA: 0x0009809F File Offset: 0x0009649F
	// (set) Token: 0x06001B46 RID: 6982 RVA: 0x000980A7 File Offset: 0x000964A7
	public float MouthOpenValue
	{
		[CompilerGenerated]
		get
		{
			return this.<MouthOpenValue>k__BackingField;
		}
		[CompilerGenerated]
		protected set
		{
			this.<MouthOpenValue>k__BackingField = value;
		}
	}

	// Token: 0x17000308 RID: 776
	// (get) Token: 0x06001B47 RID: 6983 RVA: 0x000980B0 File Offset: 0x000964B0
	// (set) Token: 0x06001B48 RID: 6984 RVA: 0x000980B8 File Offset: 0x000964B8
	public bool useMorphBank
	{
		get
		{
			return this._useMorphBank;
		}
		set
		{
			if (this._useMorphBank != value)
			{
				this._useMorphBank = value;
				if (this._useMorphBank)
				{
					this.InitMorphs();
				}
				else
				{
					this.ClearMorphs();
				}
			}
		}
	}

	// Token: 0x17000309 RID: 777
	// (get) Token: 0x06001B49 RID: 6985 RVA: 0x000980E9 File Offset: 0x000964E9
	// (set) Token: 0x06001B4A RID: 6986 RVA: 0x000980F1 File Offset: 0x000964F1
	public bool useBuiltInMorphs
	{
		get
		{
			return this._useBuiltInMorphs;
		}
		set
		{
			if (this._useBuiltInMorphs != value)
			{
				this._useBuiltInMorphs = value;
				this.InitMorphs();
			}
		}
	}

	// Token: 0x1700030A RID: 778
	// (get) Token: 0x06001B4B RID: 6987 RVA: 0x0009810C File Offset: 0x0009650C
	// (set) Token: 0x06001B4C RID: 6988 RVA: 0x00098114 File Offset: 0x00096514
	public DAZMorphBank morphBank
	{
		get
		{
			return this._morphBank;
		}
		set
		{
			if (this._morphBank != value)
			{
				this._morphBank = value;
				this.InitMorphs();
			}
		}
	}

	// Token: 0x1700030B RID: 779
	// (get) Token: 0x06001B4D RID: 6989 RVA: 0x00098134 File Offset: 0x00096534
	// (set) Token: 0x06001B4E RID: 6990 RVA: 0x0009813C File Offset: 0x0009653C
	public int setChoice
	{
		get
		{
			return this._setChoice;
		}
		set
		{
			if (this._setChoice != value)
			{
				this._setChoice = value;
				this.InitMorphs();
			}
		}
	}

	// Token: 0x06001B4F RID: 6991 RVA: 0x00098158 File Offset: 0x00096558
	protected void MTTask(object info)
	{
		SpeechBlend.SpeechBlendTaskInfo speechBlendTaskInfo = (SpeechBlend.SpeechBlendTaskInfo)info;
		while (this._threadsRunning)
		{
			speechBlendTaskInfo.resetEvent.WaitOne(-1, true);
			if (speechBlendTaskInfo.kill)
			{
				break;
			}
			try
			{
				if (!this.useUnitySpectrumMethod && this.soundData != null)
				{
					this.current_volume = this.extractFeatures.GetVolume(this.soundData);
					this.current_volume *= this.volumeMultiplier;
					if (this.volumeThreshold > 0f && this.current_volume < this.volumeThreshold)
					{
						this.current_volume = 0f;
					}
					this.current_volume = Mathf.Clamp(this.current_volume, 0f, this.volumeClamp);
					this.spectrumData = this.extractFeatures.ConvertSoundDataToSpectrumData(true);
				}
				float num = 85f;
				float num2 = 450f;
				int num3 = Mathf.FloorToInt(num / this.clipFrequency * (float)this.spectrumData.Length);
				int num4 = Mathf.CeilToInt(num2 / this.clipFrequency * (float)this.spectrumData.Length);
				this.speech_volume = 0f;
				float num5 = 0f;
				int num6 = 0;
				int num7 = 0;
				for (int i = 0; i < this.spectrumData.Length; i++)
				{
					if (i >= num3 && i <= num4)
					{
						num6++;
						this.speech_volume += this.spectrumData[i];
					}
					else
					{
						num7++;
						num5 += this.spectrumData[i];
					}
				}
				this.speech_volume /= (float)num6;
				num5 /= (float)num7;
				float[] mfccs = this.extractFeatures.ExtractSample(this.spectrumData, this.extractor, this.transformer, this.modifier, ref this.cmem, this.f_low, this.f_high, this.accuracy);
				this.extractFeatures.Evaluate(mfccs, this.voiceType, this.accuracy);
			}
			catch (Exception arg)
			{
				UnityEngine.Debug.LogError("Exception in thread " + arg);
			}
			this.taskEverRun = true;
			speechBlendTaskInfo.working = false;
		}
	}

	// Token: 0x06001B50 RID: 6992 RVA: 0x000983A8 File Offset: 0x000967A8
	protected void StopThreads()
	{
		this._threadsRunning = false;
		if (this.speechBlendTask != null)
		{
			this.speechBlendTask.kill = true;
			this.speechBlendTask.resetEvent.Set();
			while (this.speechBlendTask.thread.IsAlive)
			{
			}
			this.taskEverRun = false;
			this.speechBlendTask = null;
		}
	}

	// Token: 0x06001B51 RID: 6993 RVA: 0x00098410 File Offset: 0x00096810
	protected void StartThreads()
	{
		if (!this._threadsRunning)
		{
			this._threadsRunning = true;
			this.speechBlendTask = new SpeechBlend.SpeechBlendTaskInfo();
			this.speechBlendTask.name = "SpeechBlendTask";
			this.speechBlendTask.resetEvent = new AutoResetEvent(false);
			this.speechBlendTask.thread = new Thread(new ParameterizedThreadStart(this.MTTask));
			this.speechBlendTask.thread.Priority = System.Threading.ThreadPriority.Normal;
			this.speechBlendTask.thread.Start(this.speechBlendTask);
		}
	}

	// Token: 0x06001B52 RID: 6994 RVA: 0x0009849E File Offset: 0x0009689E
	private void OnEnable()
	{
		this.StartThreads();
		this.InitMorphs();
	}

	// Token: 0x06001B53 RID: 6995 RVA: 0x000984AC File Offset: 0x000968AC
	private void OnDisable()
	{
		this.StopThreads();
		this.ClearMorphs();
	}

	// Token: 0x06001B54 RID: 6996 RVA: 0x000984BC File Offset: 0x000968BC
	private void Start()
	{
		this.normalize_mult = 1f / (float)this.sample_count;
		int nvis;
		if (this._useMorphBank)
		{
			nvis = this.faceBlendshapeNamesArray[this._setChoice].template.Nvis;
		}
		else
		{
			nvis = this.faceBlendshapes.template.Nvis;
		}
		this.bs_setpoint = new float[nvis];
		this.bs_setpoint_last = new float[nvis];
		this.bs_setpoint_for_sort = new SpeechBlend.ValueAndIndex[nvis];
		if (this.jawJoint != null)
		{
			this.trans_mouthOpen_setpoint = this.jawJoint.localRotation;
		}
		this.trans_mouthOpen_rest = this.trans_mouthOpen_setpoint;
		this.bs_mouthOpen_setpoint = 0f;
		this.accuracy_last = this.accuracy;
		this.fres = ExtractFeatures.CalculateFres();
		this.UpdateExtractor();
		if (this._useMorphBank)
		{
			if (this.jawJoint == null && !this.faceBlendshapeNamesArray[this._setChoice].AnyAssigned())
			{
				MonoBehaviour.print("Warning (SpeechBlend): Neither jaw joint or face blendshapes have been assigned");
				this.lipsyncActive = false;
			}
			if (this.trackingMode.Equals(SpeechUtil.Mode.jawAndVisemes) && this.faceBlendshapeNamesArray[this._setChoice].JawOnly())
			{
				MonoBehaviour.print("Warning (SpeechBlend): No viseme blendshapes detected, jaw-only mode enabled.");
				this.trackingMode = SpeechUtil.Mode.jawOnly;
			}
			else if (this.trackingMode.Equals(SpeechUtil.Mode.jawAndVisemes))
			{
				this.blendshapeInfluenceActive = new bool[nvis];
			}
		}
		else
		{
			if (this.jawJoint == null && !this.faceBlendshapes.AnyAssigned())
			{
				MonoBehaviour.print("Warning (SpeechBlend): Neither jaw joint or face blendshapes have been assigned");
				this.lipsyncActive = false;
			}
			if (this.trackingMode.Equals(SpeechUtil.Mode.jawAndVisemes) && this.faceBlendshapes.JawOnly())
			{
				MonoBehaviour.print("Warning (SpeechBlend): No viseme blendshapes detected, jaw-only mode enabled.");
				this.trackingMode = SpeechUtil.Mode.jawOnly;
			}
			else if (this.trackingMode.Equals(SpeechUtil.Mode.jawAndVisemes))
			{
				this.blendshapeInfluenceActive = new bool[nvis];
			}
		}
	}

	// Token: 0x06001B55 RID: 6997 RVA: 0x000986E8 File Offset: 0x00096AE8
	private void FixedUpdate()
	{
		bool flag = this.voiceAudioSource != null && this.voiceAudioSource.isPlaying;
		if (flag && this.lipsyncActive)
		{
			this.bs_volume_scaling = 10f * Mathf.Exp(2f * (this.lipsBlendshapeMovementAmount - 0.5f));
			this.jaw_volume_scaling = 10f * this.jawMovementAmount;
			this.updateFrame++;
			this.timeSinceLastSample += Time.fixedDeltaTime;
			if (this.timeSinceLastSample >= this.timeBetweenSampling)
			{
				this.lastFixedUpdateTime = Time.fixedTime;
				if (this.taskEverRun)
				{
					while (this.speechBlendTask.working)
					{
						Thread.Sleep(0);
					}
				}
				this.hasClip = (this.voiceAudioSource.clip != null);
				if (this.useUnitySpectrumMethod || this.trackingMode != SpeechUtil.Mode.jawAndVisemes)
				{
					this.current_volume = 0f;
					if (this.audioTrace == null)
					{
						this.audioTrace = new float[this.sample_count];
					}
					this.voiceAudioSource.GetOutputData(this.audioTrace, 0);
					for (int i = 0; i < this.sample_count; i++)
					{
						this.current_volume += Mathf.Abs(this.audioTrace[i]);
					}
					this.current_volume *= this.normalize_mult;
					if (this.volumeEqualization)
					{
						if (SuperController.singleton != null)
						{
							this.current_volume = ExtractFeatures.EqualizeDistance(this.current_volume, this.voiceAudioSource, SuperController.singleton.CurrentAudioListener);
						}
						else
						{
							this.current_volume = ExtractFeatures.EqualizeDistance(this.current_volume, this.voiceAudioSource, this.activeListener);
						}
					}
					this.current_volume *= this.volumeMultiplier;
					if (this.volumeThreshold > 0f && this.current_volume < this.volumeThreshold)
					{
						this.current_volume = 0f;
					}
					this.current_volume = Mathf.Clamp(this.current_volume, 0f, this.volumeClamp);
				}
				if (this.current_volume > this.recent_max_volume)
				{
					this.recent_max_volume = this.current_volume;
					this.timeSinceLastMaxVolume = 0f;
				}
				this.timeSinceLastMaxVolume += this.timeSinceLastSample;
				if (this.timeSinceLastMaxVolume > this.timeToResetMaxVolume)
				{
					this.recent_max_volume = this.current_volume;
					this.timeSinceLastMaxVolume = 0f;
				}
				if (this._useMorphBank)
				{
					this.bs_mouthOpen_setpoint = this.current_volume * this.jaw_volume_scaling * 0.1f * (1f / this.jaw_CSF);
				}
				else
				{
					this.bs_mouthOpen_setpoint = 100f * this.current_volume * this.jaw_volume_scaling * 0.1f * (1f / this.jaw_CSF);
				}
				if (this.jawJoint != null)
				{
					this.trans_mouthOpen_setpoint = Quaternion.Euler(this.jawJointOffset + this.trans_mouthOpen_rest.eulerAngles * (1f - this.jawMovementAmount * 3f) + (this.trans_mouthOpen_rest.eulerAngles + this.jawOpenDirection * this.current_volume * this.jaw_volume_scaling) * this.jawMovementAmount * 3f);
				}
				if (this.trackingMode == SpeechUtil.Mode.jawAndVisemes)
				{
					this.f_low = Mathf.RoundToInt((float)ExtractFeatures.getlf(this.accuracy) / this.fres);
					this.f_high = Mathf.RoundToInt((float)ExtractFeatures.gethf(this.accuracy) / this.fres);
					if (this.accuracy_last != this.accuracy)
					{
						this.UpdateExtractor();
					}
					this.accuracy_last = this.accuracy;
					if (this.hasClip)
					{
						this.clipFrequency = (float)this.voiceAudioSource.clip.frequency;
					}
					if (!this.taskEverRun)
					{
						this.spectrumData = this.extractFeatures.GetUnitySpectrumData(this.voiceAudioSource, this.useHamming);
						float[] mfccs = this.extractFeatures.ExtractSample(this.spectrumData, this.extractor, this.transformer, this.modifier, ref this.cmem, this.f_low, this.f_high, this.accuracy);
						this.extractFeatures.Evaluate(mfccs, this.voiceType, this.accuracy);
					}
					else if (this.useUnitySpectrumMethod)
					{
						this.spectrumData = this.extractFeatures.GetUnitySpectrumData(this.voiceAudioSource, this.useHamming);
					}
					if (!this.useUnitySpectrumMethod)
					{
						this.soundData = this.extractFeatures.GetUnitySoundData(this.voiceAudioSource, this.GetLookaheadTime());
					}
					if (this.influences == null || this.influences.Length != ExtractFeatures.no_visemes)
					{
						this.influences = new float[ExtractFeatures.no_visemes];
					}
					else
					{
						for (int j = 0; j < this.influences.Length; j++)
						{
							this.influences[j] = 0f;
						}
					}
					for (int k = 0; k < this.extractFeatures.featureOutput.size; k++)
					{
						for (int l = 0; l < ExtractFeatures.no_visemes; l++)
						{
							this.influences[l] += VoiceProfile.Influence(this.voiceType, this.extractFeatures.featureOutput.reg[k], l, this.accuracy) * this.extractFeatures.featureOutput.w[k];
						}
					}
					float[] array = VoiceProfile.InfluenceTemplateTransform(this.influences, this.shapeTemplate);
					int nvis;
					if (this._useMorphBank)
					{
						nvis = this.faceBlendshapeNamesArray[this.setChoice].template.Nvis;
					}
					else
					{
						nvis = this.faceBlendshapes.template.Nvis;
					}
					if (this.blendshapeInfluenceActive == null || this.blendshapeInfluenceActive.Length != nvis)
					{
						this.blendshapeInfluenceActive = new bool[nvis];
					}
					SpeechUtil.VisemeWeight visemeWeight;
					if (this._useMorphBank)
					{
						visemeWeight = this.visemeWeightTuningArray[this.setChoice];
					}
					else
					{
						visemeWeight = this.visemeWeightTuning;
					}
					for (int m = 0; m < nvis; m++)
					{
						float byIndex = visemeWeight.GetByIndex(m);
						array[m] *= byIndex;
						if ((double)byIndex < 0.01)
						{
							this.blendshapeInfluenceActive[m] = false;
						}
						else
						{
							this.blendshapeInfluenceActive[m] = true;
						}
					}
					if (this._useMorphBank)
					{
						for (int n = 0; n < nvis; n++)
						{
							this.bs_setpoint_last[n] = this.bs_setpoint[n];
							this.bs_setpoint[n] = array[n] * this.current_volume * this.bs_volume_scaling;
							if (this.bs_setpoint[n] < this.visemeThreshold)
							{
								this.bs_setpoint[n] = 0f;
							}
							SpeechBlend.ValueAndIndex valueAndIndex;
							valueAndIndex.value = this.bs_setpoint[n];
							valueAndIndex.index = n;
							this.bs_setpoint_for_sort[n] = valueAndIndex;
						}
					}
					else
					{
						for (int num = 0; num < nvis; num++)
						{
							this.bs_setpoint_last[num] = this.bs_setpoint[num];
							this.bs_setpoint[num] = array[num] * 100f * this.current_volume * this.bs_volume_scaling;
							if (this.bs_setpoint[num] < this.visemeThreshold)
							{
								this.bs_setpoint[num] = 0f;
							}
							SpeechBlend.ValueAndIndex valueAndIndex2;
							valueAndIndex2.value = this.bs_setpoint[num];
							valueAndIndex2.index = num;
							this.bs_setpoint_for_sort[num] = valueAndIndex2;
						}
					}
					SpeechBlend.ValueAndIndex[] array2 = this.bs_setpoint_for_sort;
					if (SpeechBlend.<>f__am$cache0 == null)
					{
						SpeechBlend.<>f__am$cache0 = new Comparison<SpeechBlend.ValueAndIndex>(SpeechBlend.<FixedUpdate>m__0);
					}
					Array.Sort<SpeechBlend.ValueAndIndex>(array2, SpeechBlend.<>f__am$cache0);
					for (int num2 = 0; num2 < this.bs_setpoint_for_sort.Length; num2++)
					{
						if (num2 >= this.maxSimultaneousVisemes)
						{
							this.bs_setpoint[this.bs_setpoint_for_sort[num2].index] = 0f;
						}
					}
					this.bs_CSF = VoiceProfile.Influence(this.voiceType, this.extractFeatures.featureOutput.reg[0], ExtractFeatures.no_visemes, this.accuracy);
					this.jaw_CSF = VoiceProfile.Influence(this.voiceType, this.extractFeatures.featureOutput.reg[0], ExtractFeatures.no_visemes, this.accuracy);
					this.bs_mouthOpen_setpoint /= VoiceProfile.Influence(this.voiceType, this.extractFeatures.featureOutput.reg[0], ExtractFeatures.no_visemes, this.accuracy);
					this.speechBlendTask.working = true;
					this.speechBlendTask.resetEvent.Set();
				}
				this.timeSinceLastSample = 0f;
				this.updateFrame = 0;
			}
			else if (this.trackingMode == SpeechUtil.Mode.jawAndVisemes)
			{
				this.hasClip = (this.voiceAudioSource.clip != null);
				if (this.hasClip)
				{
					this.clipFrequency = (float)this.voiceAudioSource.clip.frequency;
				}
				if (!this.taskEverRun)
				{
					this.spectrumData = this.extractFeatures.GetUnitySpectrumData(this.voiceAudioSource, this.useHamming);
					float[] mfccs2 = this.extractFeatures.ExtractSample(this.spectrumData, this.extractor, this.transformer, this.modifier, ref this.cmem, this.f_low, this.f_high, this.accuracy);
					this.extractFeatures.Evaluate(mfccs2, this.voiceType, this.accuracy);
				}
				else
				{
					while (this.speechBlendTask.working)
					{
						Thread.Sleep(0);
					}
					if (this.useUnitySpectrumMethod)
					{
						this.spectrumData = this.extractFeatures.GetUnitySpectrumData(this.voiceAudioSource, this.useHamming);
					}
					else
					{
						this.soundData = this.extractFeatures.GetUnitySoundData(this.voiceAudioSource, this.GetLookaheadTime());
					}
				}
				this.speechBlendTask.working = true;
				this.speechBlendTask.resetEvent.Set();
			}
		}
		else if (this.wasPlaying)
		{
			int nvis2;
			if (this._useMorphBank)
			{
				nvis2 = this.faceBlendshapeNamesArray[this.setChoice].template.Nvis;
			}
			else
			{
				nvis2 = this.faceBlendshapes.template.Nvis;
			}
			for (int num3 = 0; num3 < nvis2; num3++)
			{
				this.bs_setpoint_last[num3] = 0f;
				this.bs_setpoint[num3] = 0f;
			}
			this.bs_mouthOpen_setpoint = 0f;
			this.current_volume = 0f;
			this.recent_max_volume = 0f;
			this.timeSinceLastMaxVolume = 0f;
		}
		this.wasPlaying = flag;
	}

	// Token: 0x06001B56 RID: 6998 RVA: 0x00099224 File Offset: 0x00097624
	private void LateUpdate()
	{
		if (this._useMorphBank)
		{
			if (!this.faceBlendshapeNamesArray[this.setChoice].MouthOpenBlendshapeAssigned() && this.jawJoint != null)
			{
				float t = 2.5f * Mathf.Exp(3.658f * this.jawMovementSpeed);
				this.jawJoint.transform.localRotation = Quaternion.Lerp(this.jawJoint.transform.localRotation, this.trans_mouthOpen_setpoint, t);
			}
		}
		else if (!this.faceBlendshapes.MouthOpenBlendshapeAssigned() && this.jawJoint != null)
		{
			float t2 = 2.5f * Mathf.Exp(3.658f * this.jawMovementSpeed);
			this.jawJoint.transform.localRotation = Quaternion.Lerp(this.jawJoint.transform.localRotation, this.trans_mouthOpen_setpoint, t2);
		}
		this.UpdateBlendshapes();
	}

	// Token: 0x06001B57 RID: 6999 RVA: 0x00099318 File Offset: 0x00097718
	private void UpdateBlendshapes()
	{
		float num = Time.deltaTime * 15f;
		float t = 1f;
		if (this.useInterpolation && !this.liveMode)
		{
			int num2 = Mathf.FloorToInt(this.timeBetweenSampling / Time.fixedDeltaTime) + 1;
			float num3 = (float)num2 * Time.fixedDeltaTime;
			float num4 = Time.time - this.lastFixedUpdateTime;
			t = Mathf.Clamp01(this.interpolationCurve.Evaluate(num4 / num3));
		}
		if (this._useMorphBank)
		{
			if (this.trackingMode == SpeechUtil.Mode.jawAndVisemes && this.visemeMorphs != null && this.blendshapeInfluenceActive != null)
			{
				for (int i = 0; i < this.visemeMorphs.Length; i++)
				{
					if (this.visemeMorphs[i] != null)
					{
						if (i < this.blendshapeInfluenceActive.Length && this.blendshapeInfluenceActive[i])
						{
							float num5 = this.visemeMorphs[i].morphValue / this.blendshapeMultiplier;
							float num6;
							if (this.useInterpolation && !this.liveMode)
							{
								num6 = Mathf.Lerp(this.bs_setpoint_last[i], this.bs_setpoint[i], t);
							}
							else
							{
								num6 = this.bs_setpoint[i];
							}
							this.VisemeRawValues[i] = num6;
							if (this.useClampChangeMethod)
							{
								float value = num6 - num5;
								float num7 = this.lipsBlendshapeChangeSpeed * num * this.bs_CSF;
								float num8 = Mathf.Clamp(value, -num7, num7);
								this.VisemeValues[i] = Mathf.Clamp((num5 + num8) * this.blendshapeMultiplier, 0f, this.blendshapeCutoff);
							}
							else
							{
								this.VisemeValues[i] = Mathf.Clamp(Mathf.Lerp(num5, num6 * this.blendshapeMultiplier, this.lipsBlendshapeChangeSpeed * num * this.blendshapeMultiplier), 0f, this.blendshapeCutoff);
							}
							this.visemeMorphs[i].morphValue = this.VisemeValues[i];
						}
						else
						{
							this.VisemeRawValues[i] = 0f;
							this.VisemeValues[i] = 0f;
							this.visemeMorphs[i].morphValue = 0f;
						}
					}
				}
			}
			if (this.mouthOpenMorph != null)
			{
				float morphValue = this.mouthOpenMorph.morphValue;
				float t2 = this.jawMovementSpeed * this.jaw_CSF * num * 2f;
				float value2 = Mathf.Lerp(morphValue, this.bs_mouthOpen_setpoint, t2);
				this.MouthOpenValue = Mathf.Clamp(value2, 0f, 1f);
				this.mouthOpenMorph.morphValue = this.MouthOpenValue;
			}
		}
		else
		{
			if (this.trackingMode == SpeechUtil.Mode.jawAndVisemes)
			{
				for (int j = 0; j < this.faceBlendshapes.template.Nvis; j++)
				{
					if (this.faceBlendshapes.BlendshapeAssigned(j) & this.blendshapeInfluenceActive[j])
					{
						float blendShapeWeight = this.headMesh.GetBlendShapeWeight(this.faceBlendshapes.GetByIndex(j));
						float num9 = this.bs_setpoint[j];
						float value3 = num9 - blendShapeWeight;
						float num10 = this.lipsBlendshapeChangeSpeed * num * 20f * this.bs_CSF;
						float num11 = Mathf.Clamp(value3, -num10, num10);
						float value4 = Mathf.Clamp(blendShapeWeight + num11, 0f, 100f);
						this.headMesh.SetBlendShapeWeight(this.faceBlendshapes.GetByIndex(j), value4);
					}
				}
			}
			if (this.faceBlendshapes.MouthOpenBlendshapeAssigned())
			{
				float blendShapeWeight2 = this.headMesh.GetBlendShapeWeight(this.faceBlendshapes.mouthOpenIndex);
				float t3 = this.jawMovementSpeed * this.jaw_CSF * num * 2f;
				this.headMesh.SetBlendShapeWeight(this.faceBlendshapes.mouthOpenIndex, Mathf.Lerp(blendShapeWeight2, this.bs_mouthOpen_setpoint, t3));
			}
		}
	}

	// Token: 0x06001B58 RID: 7000 RVA: 0x000996E4 File Offset: 0x00097AE4
	public void UpdateExtractor()
	{
		this.extractFeatures = new ExtractFeatures();
		this.extractor = ExtractFeatures.BuildExtractor(this.fres, ExtractFeatures.getlf(this.accuracy), ExtractFeatures.gethf(this.accuracy), this.accuracy);
		this.cmem = new float[ExtractFeatures.getC(this.accuracy) + 1, 2];
		this.modifier = ExtractFeatures.CreateCC_lifter(this.accuracy);
		this.transformer = ExtractFeatures.GenerateTransformer(this.accuracy);
		this.f_low = Mathf.RoundToInt((float)ExtractFeatures.getlf(this.accuracy) / this.fres);
		this.f_high = Mathf.RoundToInt((float)ExtractFeatures.gethf(this.accuracy) / this.fres);
	}

	// Token: 0x06001B59 RID: 7001 RVA: 0x000997A0 File Offset: 0x00097BA0
	[CompilerGenerated]
	private static int <FixedUpdate>m__0(SpeechBlend.ValueAndIndex a, SpeechBlend.ValueAndIndex b)
	{
		return b.value.CompareTo(a.value);
	}

	// Token: 0x040016E6 RID: 5862
	public AudioSource voiceAudioSource;

	// Token: 0x040016E7 RID: 5863
	public SkinnedMeshRenderer headMesh;

	// Token: 0x040016E8 RID: 5864
	[HideInInspector]
	public bool showBlendShapeMenu;

	// Token: 0x040016E9 RID: 5865
	[HideInInspector]
	public SpeechUtil.VisemeBlendshapeIndexes faceBlendshapes;

	// Token: 0x040016EA RID: 5866
	[HideInInspector]
	public SpeechUtil.VisemeWeight visemeWeightTuning;

	// Token: 0x040016EB RID: 5867
	[Header("Settings")]
	[Space(10f)]
	[Tooltip("Toggle lipsyncing")]
	public bool lipsyncActive = true;

	// Token: 0x040016EC RID: 5868
	[Tooltip("Select whether visemes are used")]
	public SpeechUtil.Mode trackingMode;

	// Token: 0x040016ED RID: 5869
	[Tooltip("Volume multiplier")]
	[Range(0f, 10f)]
	public float volumeMultiplier = 1f;

	// Token: 0x040016EE RID: 5870
	[Tooltip("Minimum volume before lip sync activates")]
	[Range(0f, 1f)]
	public float volumeThreshold;

	// Token: 0x040016EF RID: 5871
	[Tooltip("Clamp maximum volume to prevent shape overshoot")]
	[Range(0f, 1f)]
	public float volumeClamp = 1f;

	// Token: 0x040016F0 RID: 5872
	[Tooltip("Amplitude of jaw movement")]
	[Range(0f, 1f)]
	public float jawMovementAmount = 0.5f;

	// Token: 0x040016F1 RID: 5873
	[Tooltip("Jaw motion speed")]
	[Range(0f, 1f)]
	public float jawMovementSpeed = 0.5f;

	// Token: 0x040016F2 RID: 5874
	[Tooltip("Amplitude of lip movement")]
	[Range(0f, 1f)]
	public float lipsBlendshapeMovementAmount = 0.5f;

	// Token: 0x040016F3 RID: 5875
	[Tooltip("Lip viseme movement speed")]
	[Range(0f, 1f)]
	public float lipsBlendshapeChangeSpeed = 0.5f;

	// Token: 0x040016F4 RID: 5876
	[Range(0f, 1f)]
	public float visemeThreshold;

	// Token: 0x040016F5 RID: 5877
	[Range(0.5f, 2f)]
	public float blendshapeMultiplier = 1f;

	// Token: 0x040016F6 RID: 5878
	[Range(0f, 1f)]
	public float blendshapeCutoff = 1f;

	// Token: 0x040016F7 RID: 5879
	[Range(1f, 5f)]
	public int maxSimultaneousVisemes = 5;

	// Token: 0x040016F8 RID: 5880
	public bool liveMode;

	// Token: 0x040016F9 RID: 5881
	public bool useInterpolation = true;

	// Token: 0x040016FA RID: 5882
	protected bool hasClip;

	// Token: 0x040016FB RID: 5883
	public bool useUnitySpectrum = true;

	// Token: 0x040016FC RID: 5884
	public bool useHamming = true;

	// Token: 0x040016FD RID: 5885
	public bool useLookahead = true;

	// Token: 0x040016FE RID: 5886
	[Range(-0.2f, 0.2f)]
	public float lookaheadAdjust;

	// Token: 0x040016FF RID: 5887
	public AnimationCurve interpolationCurve;

	// Token: 0x04001700 RID: 5888
	[Tooltip("Number of calculations to use.")]
	public SpeechUtil.Accuracy accuracy = SpeechUtil.Accuracy.Medium;

	// Token: 0x04001701 RID: 5889
	[Range(0f, 1f)]
	public float timeBetweenSampling = 0.1f;

	// Token: 0x04001702 RID: 5890
	[Tooltip("Ignore distance between AudioSource and AudioListener when accounting for volume.")]
	public bool volumeEqualization;

	// Token: 0x04001703 RID: 5891
	[Tooltip("Voice type of character")]
	public VoiceProfile.VoiceType voiceType = VoiceProfile.VoiceType.female;

	// Token: 0x04001704 RID: 5892
	[Tooltip("Jaw joint for when not using a mouth open blendshape")]
	public Transform jawJoint;

	// Token: 0x04001705 RID: 5893
	[Tooltip("Direction adjust for jaw opening")]
	public Vector3 jawOpenDirection = new Vector3(1f, 0f, 0f);

	// Token: 0x04001706 RID: 5894
	[Tooltip("Angular offset for jaw joint opening")]
	public Vector3 jawJointOffset;

	// Token: 0x04001707 RID: 5895
	[Tooltip("Blendshape template for visemes shapes. (default: DAZ)")]
	public VoiceProfile.VisemeBlendshapeTemplate shapeTemplate;

	// Token: 0x04001708 RID: 5896
	public AudioListener activeListener;

	// Token: 0x04001709 RID: 5897
	protected DAZMorph[] visemeMorphs;

	// Token: 0x0400170A RID: 5898
	[CompilerGenerated]
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private float[] <VisemeRawValues>k__BackingField;

	// Token: 0x0400170B RID: 5899
	[CompilerGenerated]
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private float[] <VisemeValues>k__BackingField;

	// Token: 0x0400170C RID: 5900
	protected DAZMorph mouthOpenMorph;

	// Token: 0x0400170D RID: 5901
	[CompilerGenerated]
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private float <MouthOpenValue>k__BackingField;

	// Token: 0x0400170E RID: 5902
	[SerializeField]
	protected bool _useMorphBank;

	// Token: 0x0400170F RID: 5903
	public bool useClampChangeMethod = true;

	// Token: 0x04001710 RID: 5904
	[SerializeField]
	protected bool _useBuiltInMorphs;

	// Token: 0x04001711 RID: 5905
	[SerializeField]
	[HideInInspector]
	protected DAZMorphBank _morphBank;

	// Token: 0x04001712 RID: 5906
	[SerializeField]
	[HideInInspector]
	protected int _setChoice;

	// Token: 0x04001713 RID: 5907
	public int numberOfBlendShapeSets = 1;

	// Token: 0x04001714 RID: 5908
	[HideInInspector]
	public SpeechUtil.VisemeBlendshapeNames[] faceBlendshapeNamesArray;

	// Token: 0x04001715 RID: 5909
	[HideInInspector]
	public SpeechUtil.VisemeWeight[] visemeWeightTuningArray;

	// Token: 0x04001716 RID: 5910
	private float bs_volume_scaling = 20f;

	// Token: 0x04001717 RID: 5911
	private float jaw_volume_scaling = 20f;

	// Token: 0x04001718 RID: 5912
	private int f_low;

	// Token: 0x04001719 RID: 5913
	private int f_high;

	// Token: 0x0400171A RID: 5914
	private float fres;

	// Token: 0x0400171B RID: 5915
	private ExtractFeatures extractFeatures;

	// Token: 0x0400171C RID: 5916
	private float[,] extractor;

	// Token: 0x0400171D RID: 5917
	private float[,] transformer;

	// Token: 0x0400171E RID: 5918
	private float[] modifier;

	// Token: 0x0400171F RID: 5919
	private float[] bs_setpoint;

	// Token: 0x04001720 RID: 5920
	private float[] bs_setpoint_last;

	// Token: 0x04001721 RID: 5921
	private SpeechBlend.ValueAndIndex[] bs_setpoint_for_sort;

	// Token: 0x04001722 RID: 5922
	private float[,] cmem;

	// Token: 0x04001723 RID: 5923
	private float bs_mouthOpen_setpoint;

	// Token: 0x04001724 RID: 5924
	private Quaternion trans_mouthOpen_setpoint;

	// Token: 0x04001725 RID: 5925
	private Quaternion trans_mouthOpen_rest;

	// Token: 0x04001726 RID: 5926
	public int sample_count = 256;

	// Token: 0x04001727 RID: 5927
	private float calculated_volume;

	// Token: 0x04001728 RID: 5928
	public float current_volume;

	// Token: 0x04001729 RID: 5929
	public float speech_volume;

	// Token: 0x0400172A RID: 5930
	public float recent_max_volume;

	// Token: 0x0400172B RID: 5931
	private float normalize_mult = 1f;

	// Token: 0x0400172C RID: 5932
	[HideInInspector]
	public VoiceProfile.VisemeBlendshapeTemplate template_saved;

	// Token: 0x0400172D RID: 5933
	[HideInInspector]
	public SpeechUtil.VisemeBlendshapeIndexes faceBlendshapes_saved = new SpeechUtil.VisemeBlendshapeIndexes(VoiceProfile.G2_template);

	// Token: 0x0400172E RID: 5934
	[HideInInspector]
	public SpeechUtil.VisemeWeight visemeWeightTuning_saved = new SpeechUtil.VisemeWeight(VoiceProfile.G2_template);

	// Token: 0x0400172F RID: 5935
	private float jaw_CSF = 1f;

	// Token: 0x04001730 RID: 5936
	private float bs_CSF = 1f;

	// Token: 0x04001731 RID: 5937
	private int updateFrame;

	// Token: 0x04001732 RID: 5938
	private SpeechUtil.Accuracy accuracy_last;

	// Token: 0x04001733 RID: 5939
	private bool[] blendshapeInfluenceActive;

	// Token: 0x04001734 RID: 5940
	protected bool taskEverRun;

	// Token: 0x04001735 RID: 5941
	protected SpeechBlend.SpeechBlendTaskInfo speechBlendTask;

	// Token: 0x04001736 RID: 5942
	protected bool _threadsRunning;

	// Token: 0x04001737 RID: 5943
	private float[] audioTrace;

	// Token: 0x04001738 RID: 5944
	private float[] influences;

	// Token: 0x04001739 RID: 5945
	private float clipFrequency;

	// Token: 0x0400173A RID: 5946
	private float[] soundData;

	// Token: 0x0400173B RID: 5947
	private float[] spectrumData;

	// Token: 0x0400173C RID: 5948
	private float[] rawData;

	// Token: 0x0400173D RID: 5949
	private float timeToResetMaxVolume = 1f;

	// Token: 0x0400173E RID: 5950
	private float timeSinceLastMaxVolume;

	// Token: 0x0400173F RID: 5951
	private bool wasPlaying;

	// Token: 0x04001740 RID: 5952
	private float timeSinceLastSample;

	// Token: 0x04001741 RID: 5953
	private float lastFixedUpdateTime;

	// Token: 0x04001742 RID: 5954
	[CompilerGenerated]
	private static Comparison<SpeechBlend.ValueAndIndex> <>f__am$cache0;

	// Token: 0x02000446 RID: 1094
	protected class SpeechBlendTaskInfo
	{
		// Token: 0x06001B5A RID: 7002 RVA: 0x000997B5 File Offset: 0x00097BB5
		public SpeechBlendTaskInfo()
		{
		}

		// Token: 0x04001743 RID: 5955
		public string name;

		// Token: 0x04001744 RID: 5956
		public AutoResetEvent resetEvent;

		// Token: 0x04001745 RID: 5957
		public Thread thread;

		// Token: 0x04001746 RID: 5958
		public volatile bool working;

		// Token: 0x04001747 RID: 5959
		public volatile bool kill;
	}

	// Token: 0x02000447 RID: 1095
	private struct ValueAndIndex
	{
		// Token: 0x04001748 RID: 5960
		public int index;

		// Token: 0x04001749 RID: 5961
		public float value;
	}
}
