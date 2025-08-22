using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using SpeechBlendEngine;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000B84 RID: 2948
public class SpeechBlendControl : JSONStorable
{
	// Token: 0x060052EB RID: 21227 RVA: 0x001DF42E File Offset: 0x001DD82E
	public SpeechBlendControl()
	{
	}

	// Token: 0x060052EC RID: 21228 RVA: 0x001DF436 File Offset: 0x001DD836
	protected void SyncEnabled(bool b)
	{
		if (this.speechBlend != null)
		{
			this.speechBlend.enabled = b;
		}
	}

	// Token: 0x17000C0E RID: 3086
	// (get) Token: 0x060052ED RID: 21229 RVA: 0x001DF455 File Offset: 0x001DD855
	public float volume
	{
		get
		{
			return this.volumeJSON.val;
		}
	}

	// Token: 0x17000C0F RID: 3087
	// (get) Token: 0x060052EE RID: 21230 RVA: 0x001DF462 File Offset: 0x001DD862
	public float maxVolume
	{
		get
		{
			return this.maxVolumeJSON.val;
		}
	}

	// Token: 0x060052EF RID: 21231 RVA: 0x001DF46F File Offset: 0x001DD86F
	protected void SyncVolumeMultiplier(float f)
	{
		if (this.speechBlend != null)
		{
			this.speechBlend.volumeMultiplier = f;
		}
	}

	// Token: 0x17000C10 RID: 3088
	// (get) Token: 0x060052F0 RID: 21232 RVA: 0x001DF48E File Offset: 0x001DD88E
	// (set) Token: 0x060052F1 RID: 21233 RVA: 0x001DF496 File Offset: 0x001DD896
	public JSONStorableFloat volumeMultiplierJSON
	{
		[CompilerGenerated]
		get
		{
			return this.<volumeMultiplierJSON>k__BackingField;
		}
		[CompilerGenerated]
		protected set
		{
			this.<volumeMultiplierJSON>k__BackingField = value;
		}
	}

	// Token: 0x060052F2 RID: 21234 RVA: 0x001DF49F File Offset: 0x001DD89F
	protected void SyncVolumeClamp(float f)
	{
		if (this.speechBlend != null)
		{
			this.speechBlend.volumeClamp = f;
		}
	}

	// Token: 0x17000C11 RID: 3089
	// (get) Token: 0x060052F3 RID: 21235 RVA: 0x001DF4BE File Offset: 0x001DD8BE
	// (set) Token: 0x060052F4 RID: 21236 RVA: 0x001DF4C6 File Offset: 0x001DD8C6
	public JSONStorableFloat volumeClampJSON
	{
		[CompilerGenerated]
		get
		{
			return this.<volumeClampJSON>k__BackingField;
		}
		[CompilerGenerated]
		protected set
		{
			this.<volumeClampJSON>k__BackingField = value;
		}
	}

	// Token: 0x060052F5 RID: 21237 RVA: 0x001DF4CF File Offset: 0x001DD8CF
	protected void SyncVolumeThreshold(float f)
	{
		if (this.speechBlend != null)
		{
			this.speechBlend.volumeThreshold = f;
		}
	}

	// Token: 0x17000C12 RID: 3090
	// (get) Token: 0x060052F6 RID: 21238 RVA: 0x001DF4EE File Offset: 0x001DD8EE
	// (set) Token: 0x060052F7 RID: 21239 RVA: 0x001DF4F6 File Offset: 0x001DD8F6
	public JSONStorableFloat volumeThresholdJSON
	{
		[CompilerGenerated]
		get
		{
			return this.<volumeThresholdJSON>k__BackingField;
		}
		[CompilerGenerated]
		protected set
		{
			this.<volumeThresholdJSON>k__BackingField = value;
		}
	}

	// Token: 0x060052F8 RID: 21240 RVA: 0x001DF4FF File Offset: 0x001DD8FF
	protected void SyncMouthOpenFactorAmount(float f)
	{
		if (this.speechBlend != null)
		{
			this.speechBlend.jawMovementAmount = f;
		}
	}

	// Token: 0x17000C13 RID: 3091
	// (get) Token: 0x060052F9 RID: 21241 RVA: 0x001DF51E File Offset: 0x001DD91E
	// (set) Token: 0x060052FA RID: 21242 RVA: 0x001DF526 File Offset: 0x001DD926
	public JSONStorableFloat mouthOpenFactorJSON
	{
		[CompilerGenerated]
		get
		{
			return this.<mouthOpenFactorJSON>k__BackingField;
		}
		[CompilerGenerated]
		protected set
		{
			this.<mouthOpenFactorJSON>k__BackingField = value;
		}
	}

	// Token: 0x060052FB RID: 21243 RVA: 0x001DF52F File Offset: 0x001DD92F
	protected void SyncMouthOpenChangeRate(float f)
	{
		if (this.speechBlend != null)
		{
			this.speechBlend.jawMovementSpeed = f;
		}
	}

	// Token: 0x17000C14 RID: 3092
	// (get) Token: 0x060052FC RID: 21244 RVA: 0x001DF54E File Offset: 0x001DD94E
	// (set) Token: 0x060052FD RID: 21245 RVA: 0x001DF556 File Offset: 0x001DD956
	public JSONStorableFloat mouthOpenChangeRateJSON
	{
		[CompilerGenerated]
		get
		{
			return this.<mouthOpenChangeRateJSON>k__BackingField;
		}
		[CompilerGenerated]
		protected set
		{
			this.<mouthOpenChangeRateJSON>k__BackingField = value;
		}
	}

	// Token: 0x060052FE RID: 21246 RVA: 0x001DF55F File Offset: 0x001DD95F
	protected void SyncVisemeDetectionFactor(float f)
	{
		if (this.speechBlend != null)
		{
			this.speechBlend.lipsBlendshapeMovementAmount = f;
		}
	}

	// Token: 0x17000C15 RID: 3093
	// (get) Token: 0x060052FF RID: 21247 RVA: 0x001DF57E File Offset: 0x001DD97E
	// (set) Token: 0x06005300 RID: 21248 RVA: 0x001DF586 File Offset: 0x001DD986
	public JSONStorableFloat visemeDetectionFactorJSON
	{
		[CompilerGenerated]
		get
		{
			return this.<visemeDetectionFactorJSON>k__BackingField;
		}
		[CompilerGenerated]
		protected set
		{
			this.<visemeDetectionFactorJSON>k__BackingField = value;
		}
	}

	// Token: 0x06005301 RID: 21249 RVA: 0x001DF58F File Offset: 0x001DD98F
	protected void SyncVisemeThreshold(float f)
	{
		if (this.speechBlend != null)
		{
			this.speechBlend.visemeThreshold = f;
		}
	}

	// Token: 0x17000C16 RID: 3094
	// (get) Token: 0x06005302 RID: 21250 RVA: 0x001DF5AE File Offset: 0x001DD9AE
	// (set) Token: 0x06005303 RID: 21251 RVA: 0x001DF5B6 File Offset: 0x001DD9B6
	public JSONStorableFloat visemeThresholdJSON
	{
		[CompilerGenerated]
		get
		{
			return this.<visemeThresholdJSON>k__BackingField;
		}
		[CompilerGenerated]
		protected set
		{
			this.<visemeThresholdJSON>k__BackingField = value;
		}
	}

	// Token: 0x06005304 RID: 21252 RVA: 0x001DF5BF File Offset: 0x001DD9BF
	protected void SyncTimeBetweenSampling(float f)
	{
		if (this.speechBlend != null)
		{
			this.speechBlend.timeBetweenSampling = f;
		}
	}

	// Token: 0x17000C17 RID: 3095
	// (get) Token: 0x06005305 RID: 21253 RVA: 0x001DF5DE File Offset: 0x001DD9DE
	// (set) Token: 0x06005306 RID: 21254 RVA: 0x001DF5E6 File Offset: 0x001DD9E6
	public JSONStorableFloat timeBetweenSamplingJSON
	{
		[CompilerGenerated]
		get
		{
			return this.<timeBetweenSamplingJSON>k__BackingField;
		}
		[CompilerGenerated]
		protected set
		{
			this.<timeBetweenSamplingJSON>k__BackingField = value;
		}
	}

	// Token: 0x06005307 RID: 21255 RVA: 0x001DF5EF File Offset: 0x001DD9EF
	protected void SyncSampleTimeAdjust(float f)
	{
		if (this.speechBlend != null)
		{
			this.speechBlend.lookaheadAdjust = f;
		}
	}

	// Token: 0x17000C18 RID: 3096
	// (get) Token: 0x06005308 RID: 21256 RVA: 0x001DF60E File Offset: 0x001DDA0E
	// (set) Token: 0x06005309 RID: 21257 RVA: 0x001DF616 File Offset: 0x001DDA16
	public JSONStorableFloat sampleTimeAdjustJSON
	{
		[CompilerGenerated]
		get
		{
			return this.<sampleTimeAdjustJSON>k__BackingField;
		}
		[CompilerGenerated]
		protected set
		{
			this.<sampleTimeAdjustJSON>k__BackingField = value;
		}
	}

	// Token: 0x0600530A RID: 21258 RVA: 0x001DF61F File Offset: 0x001DDA1F
	protected void SyncVisemeMorphChangeRate(float f)
	{
		if (this.speechBlend != null)
		{
			this.speechBlend.lipsBlendshapeChangeSpeed = f;
		}
	}

	// Token: 0x17000C19 RID: 3097
	// (get) Token: 0x0600530B RID: 21259 RVA: 0x001DF63E File Offset: 0x001DDA3E
	// (set) Token: 0x0600530C RID: 21260 RVA: 0x001DF646 File Offset: 0x001DDA46
	public JSONStorableFloat visemeMorphChangeRateJSON
	{
		[CompilerGenerated]
		get
		{
			return this.<visemeMorphChangeRateJSON>k__BackingField;
		}
		[CompilerGenerated]
		protected set
		{
			this.<visemeMorphChangeRateJSON>k__BackingField = value;
		}
	}

	// Token: 0x0600530D RID: 21261 RVA: 0x001DF64F File Offset: 0x001DDA4F
	protected void SyncVisemeMorphClamp(float f)
	{
		if (this.speechBlend != null)
		{
			this.speechBlend.blendshapeCutoff = f;
		}
	}

	// Token: 0x17000C1A RID: 3098
	// (get) Token: 0x0600530E RID: 21262 RVA: 0x001DF66E File Offset: 0x001DDA6E
	// (set) Token: 0x0600530F RID: 21263 RVA: 0x001DF676 File Offset: 0x001DDA76
	public JSONStorableFloat visemeMorphClampJSON
	{
		[CompilerGenerated]
		get
		{
			return this.<visemeMorphClampJSON>k__BackingField;
		}
		[CompilerGenerated]
		protected set
		{
			this.<visemeMorphClampJSON>k__BackingField = value;
		}
	}

	// Token: 0x06005310 RID: 21264 RVA: 0x001DF680 File Offset: 0x001DDA80
	protected void SyncVoiceType(string s)
	{
		if (this.speechBlend != null)
		{
			try
			{
				VoiceProfile.VoiceType voiceType = (VoiceProfile.VoiceType)Enum.Parse(typeof(VoiceProfile.VoiceType), s);
				this.speechBlend.voiceType = voiceType;
			}
			catch (ArgumentException)
			{
				UnityEngine.Debug.LogError("Attempted to set voice type to " + s + " which is not a valid type");
			}
		}
	}

	// Token: 0x17000C1B RID: 3099
	// (get) Token: 0x06005311 RID: 21265 RVA: 0x001DF6F0 File Offset: 0x001DDAF0
	// (set) Token: 0x06005312 RID: 21266 RVA: 0x001DF6F8 File Offset: 0x001DDAF8
	public JSONStorableStringChooser voiceTypeJSON
	{
		[CompilerGenerated]
		get
		{
			return this.<voiceTypeJSON>k__BackingField;
		}
		[CompilerGenerated]
		protected set
		{
			this.<voiceTypeJSON>k__BackingField = value;
		}
	}

	// Token: 0x06005313 RID: 21267 RVA: 0x001DF704 File Offset: 0x001DDB04
	protected void SyncMorphSetParams()
	{
		bool isStorable = this._morphSetType == SpeechBlendControl.MorphSetType.Normal;
		bool isStorable2 = this._morphSetType == SpeechBlendControl.MorphSetType.Physics;
		bool isStorable3 = this._morphSetType == SpeechBlendControl.MorphSetType.Custom;
		for (int i = 0; i < this.normalVisemeWeightsJSON.Length; i++)
		{
			this.normalVisemeWeightsJSON[i].isStorable = isStorable;
		}
		for (int j = 0; j < this.physicsVisemeWeightsJSON.Length; j++)
		{
			this.physicsVisemeWeightsJSON[j].isStorable = isStorable2;
		}
		for (int k = 0; k < this.customVisemeWeightsJSON.Length; k++)
		{
			this.customVisemeWeightsJSON[k].isStorable = isStorable3;
		}
		for (int l = 0; l < this.customMorphUidsJSON.Length; l++)
		{
			this.customMorphUidsJSON[l].isStorable = isStorable3;
		}
		this.customMouthOpenMorphUidJSON.isStorable = isStorable3;
	}

	// Token: 0x06005314 RID: 21268 RVA: 0x001DF7E8 File Offset: 0x001DDBE8
	protected void SyncMorphSet(string s)
	{
		if (this.speechBlend != null)
		{
			try
			{
				SpeechBlendControl.MorphSetType morphSetType = (SpeechBlendControl.MorphSetType)Enum.Parse(typeof(SpeechBlendControl.MorphSetType), s);
				this.speechBlend.useBuiltInMorphs = (this._morphSetType != SpeechBlendControl.MorphSetType.Custom);
				this.speechBlend.setChoice = (int)morphSetType;
				this._morphSetType = morphSetType;
				this.SyncMorphSetParams();
				this.SyncVisemeUIs();
			}
			catch (ArgumentException)
			{
				UnityEngine.Debug.LogError("Attempted to set morph set to " + s + " which is not a valid type");
			}
		}
	}

	// Token: 0x06005315 RID: 21269 RVA: 0x001DF884 File Offset: 0x001DDC84
	public void OpenAdvancedPanel()
	{
		if (this.advancedPanel != null)
		{
			this.advancedPanel.SetActive(true);
		}
	}

	// Token: 0x06005316 RID: 21270 RVA: 0x001DF8A4 File Offset: 0x001DDCA4
	public void ResetAllAdvancedSettings()
	{
		for (int i = 0; i < this.normalVisemeWeightsJSON.Length; i++)
		{
			this.normalVisemeWeightsJSON[i].SetValToDefault();
		}
		for (int j = 0; j < this.physicsVisemeWeightsJSON.Length; j++)
		{
			this.physicsVisemeWeightsJSON[j].SetValToDefault();
		}
		for (int k = 0; k < this.customVisemeWeightsJSON.Length; k++)
		{
			this.customVisemeWeightsJSON[k].SetValToDefault();
		}
		for (int l = 0; l < this.customMorphUidsJSON.Length; l++)
		{
			this.customMorphUidsJSON[l].SetValToDefault();
		}
		this.customMouthOpenMorphUidJSON.SetValToDefault();
	}

	// Token: 0x06005317 RID: 21271 RVA: 0x001DF954 File Offset: 0x001DDD54
	protected void SyncVisemeUIs()
	{
		if (this.visemeUIs != null)
		{
			if (this.visemeUIs.Length == this.normalVisemeWeightsJSON.Length)
			{
				if (this.currentVisemeNamesJSON != null)
				{
					for (int i = 0; i < this.currentVisemeNamesJSON.Length; i++)
					{
						this.currentVisemeNamesJSON[i].text = null;
					}
				}
				if (this.currentMorphUidsJSON != null)
				{
					for (int j = 0; j < this.currentMorphUidsJSON.Length; j++)
					{
						this.currentMorphUidsJSON[j].inputField = null;
					}
				}
				if (this.currentMouthOpenMorphUidJSON != null)
				{
					this.currentMouthOpenMorphUidJSON.inputField = null;
				}
				if (this.currentWeightsJSON != null)
				{
					for (int k = 0; k < this.currentWeightsJSON.Length; k++)
					{
						this.currentWeightsJSON[k].slider = null;
					}
				}
				SpeechBlendControl.MorphSetType morphSetType = this._morphSetType;
				if (morphSetType != SpeechBlendControl.MorphSetType.Normal)
				{
					if (morphSetType != SpeechBlendControl.MorphSetType.Physics)
					{
						if (morphSetType == SpeechBlendControl.MorphSetType.Custom)
						{
							this.currentVisemeNamesJSON = this.customVisemeNamesJSON;
							this.currentMorphUidsJSON = this.customMorphUidsJSON;
							this.currentMouthOpenMorphUidJSON = this.customMouthOpenMorphUidJSON;
							this.currentWeightsJSON = this.customVisemeWeightsJSON;
						}
					}
					else
					{
						this.currentVisemeNamesJSON = this.physicsVisemeNamesJSON;
						this.currentMorphUidsJSON = this.physicsMorphUidsJSON;
						this.currentMouthOpenMorphUidJSON = this.physicsMouthOpenMorphUidJSON;
						this.currentWeightsJSON = this.physicsVisemeWeightsJSON;
					}
				}
				else
				{
					this.currentVisemeNamesJSON = this.normalVisemeNamesJSON;
					this.currentMorphUidsJSON = this.normalMorphUidsJSON;
					this.currentMouthOpenMorphUidJSON = this.normalMouthOpenMorphUidJSON;
					this.currentWeightsJSON = this.normalVisemeWeightsJSON;
				}
				for (int l = 0; l < this.speechBlend.VisemeMorphs.Length; l++)
				{
					this.visemeFoundJSON[l].val = (this.speechBlend.VisemeMorphs[l] != null);
				}
				this.mouthOpenVisemeFoundJSON.val = (this.speechBlend.MouthOpenMorph != null);
				for (int m = 0; m < this.currentWeightsJSON.Length; m++)
				{
					this.currentVisemeNamesJSON[m].text = this.visemeUIs[m].visemeNameText;
					this.currentMorphUidsJSON[m].inputField = this.visemeUIs[m].visemeMorphUidInputField;
					this.currentWeightsJSON[m].slider = this.visemeUIs[m].visemeWeightSlider;
				}
				for (int n = 0; n < this.pasteCustomMorphUidAction.Length; n++)
				{
					this.pasteCustomMorphUidAction[n].button = this.visemeUIs[n].pasteMorphUidButton;
					if (this.pasteCustomMorphUidAction[n].button != null)
					{
						this.pasteCustomMorphUidAction[n].button.gameObject.SetActive(this._morphSetType == SpeechBlendControl.MorphSetType.Custom);
					}
				}
				this.currentMouthOpenMorphUidJSON.inputField = this.mouthOpenMorphUidInputField;
				if (this.pasteCustomMouthOpenMorphUidAction.button != null)
				{
					this.pasteCustomMouthOpenMorphUidAction.button.gameObject.SetActive(this._morphSetType == SpeechBlendControl.MorphSetType.Custom);
				}
				if (!this.visemeValuesJSONUIConnected)
				{
					if (this.visemeValuesJSON != null)
					{
						for (int num = 0; num < this.visemeValuesJSON.Length; num++)
						{
							this.visemeValuesJSON[num].slider = this.visemeUIs[num].visemeValueSlider;
						}
					}
					if (this.visemeRawValuesJSON != null)
					{
						for (int num2 = 0; num2 < this.visemeRawValuesJSON.Length; num2++)
						{
							this.visemeRawValuesJSON[num2].slider = this.visemeUIs[num2].visemeRawValueSlider;
						}
					}
					if (this.visemeFoundJSON != null)
					{
						for (int num3 = 0; num3 < this.visemeFoundJSON.Length; num3++)
						{
							this.visemeFoundJSON[num3].indicator = this.visemeUIs[num3].visemeFoundIndicator;
							this.visemeFoundJSON[num3].negativeIndicator = this.visemeUIs[num3].visemeFoundNegativeIndicator;
						}
					}
					this.visemeValuesJSONUIConnected = true;
				}
			}
			else
			{
				UnityEngine.Debug.LogError(string.Concat(new object[]
				{
					"Length of visemeUIs ",
					this.visemeUIs.Length,
					" does not match length of weights ",
					this.normalVisemeWeightsJSON.Length
				}));
			}
		}
	}

	// Token: 0x06005318 RID: 21272 RVA: 0x001DFDB8 File Offset: 0x001DE1B8
	protected override void InitUI(Transform t, bool isAlt)
	{
		base.InitUI(t, isAlt);
		if (t != null)
		{
			SpeechBlendControlUI componentInChildren = t.GetComponentInChildren<SpeechBlendControlUI>(true);
			if (componentInChildren != null)
			{
				this.enabledJSON.RegisterToggle(componentInChildren.enabledToggle, isAlt);
				this.volumeJSON.RegisterSlider(componentInChildren.volumeSlider, isAlt);
				this.maxVolumeJSON.RegisterSlider(componentInChildren.maxVolumeSlider, isAlt);
				this.volumeMultiplierJSON.RegisterSlider(componentInChildren.volumeMultiplierSlider, isAlt);
				this.volumeClampJSON.RegisterSlider(componentInChildren.volumeClampSlider, isAlt);
				this.volumeThresholdJSON.RegisterSlider(componentInChildren.volumeThresholdSlider, isAlt);
				this.mouthOpenFactorJSON.RegisterSlider(componentInChildren.mouthOpenFactorSlider, isAlt);
				this.mouthOpenChangeRateJSON.RegisterSlider(componentInChildren.mouthOpenChangeRateSlider, isAlt);
				this.visemeDetectionFactorJSON.RegisterSlider(componentInChildren.visemeDetectionFactorSlider, isAlt);
				this.visemeThresholdJSON.RegisterSlider(componentInChildren.visemeThresholdSlider, isAlt);
				this.timeBetweenSamplingJSON.RegisterSlider(componentInChildren.timeBetweenSamplingSlider, isAlt);
				this.sampleTimeAdjustJSON.RegisterSlider(componentInChildren.sampleTimeAdjustSlider, isAlt);
				this.visemeMorphChangeRateJSON.RegisterSlider(componentInChildren.visemeMorphChangeRateSlider, isAlt);
				this.visemeMorphClampJSON.RegisterSlider(componentInChildren.visemeMorphClampSlider, isAlt);
				this.voiceTypeJSON.RegisterPopup(componentInChildren.voiceTypePopup, isAlt);
				this.morphSetJSON.RegisterPopup(componentInChildren.morphSetPopup, isAlt);
				if (!isAlt)
				{
					this.advancedPanel = componentInChildren.advancedPanel;
					this.mouthOpenMorphUidInputField = componentInChildren.mouthOpenMorphUidInputField;
				}
				this.mouthOpenVisemeFoundJSON.RegisterIndicator(componentInChildren.mouthOpenVisemeFoundIndicator, isAlt);
				this.mouthOpenVisemeFoundJSON.RegisterNegativeIndicator(componentInChildren.mouthOpenVisemeFoundNegativeIndicator, isAlt);
				this.mouthOpenVisemeValueJSON.RegisterSlider(componentInChildren.mouthOpenVisemeValueSlider, isAlt);
				this.openAdvancedPanelAction.RegisterButton(componentInChildren.openAdvancedPanelButton, isAlt);
				if (this.advancedPanel != null)
				{
					this.visemeUIs = this.advancedPanel.GetComponentsInChildren<SpeechBlendVisemeUI>(true);
					this.SyncVisemeUIs();
				}
				this.pasteCustomMouthOpenMorphUidAction.RegisterButton(componentInChildren.pasteMouthOpenMorphUidButton, isAlt);
				this.resetAllAdvancedSettingsAction.RegisterButton(componentInChildren.resetAllAdvancedSettingsButton, isAlt);
			}
		}
	}

	// Token: 0x06005319 RID: 21273 RVA: 0x001DFFC0 File Offset: 0x001DE3C0
	protected void Init()
	{
		if (this.speechBlend != null)
		{
			this.enabledJSON = new JSONStorableBool("enabled", this.speechBlend.enabled, new JSONStorableBool.SetBoolCallback(this.SyncEnabled));
			this.enabledJSON.storeType = JSONStorableParam.StoreType.Physical;
			base.RegisterBool(this.enabledJSON);
			this.volumeJSON = new JSONStorableFloat("volume", 0f, 0f, 1f, true, false);
			this.maxVolumeJSON = new JSONStorableFloat("maxVolume", 0f, 0f, 1f, true, false);
			this.volumeMultiplierJSON = new JSONStorableFloat("volumeMultiplier", this.speechBlend.volumeMultiplier, new JSONStorableFloat.SetFloatCallback(this.SyncVolumeMultiplier), 0f, 10f, false, true);
			this.volumeMultiplierJSON.storeType = JSONStorableParam.StoreType.Physical;
			base.RegisterFloat(this.volumeMultiplierJSON);
			this.volumeClampJSON = new JSONStorableFloat("volumeClamp", this.speechBlend.volumeClamp, new JSONStorableFloat.SetFloatCallback(this.SyncVolumeClamp), 0f, 1f, true, true);
			this.volumeClampJSON.storeType = JSONStorableParam.StoreType.Physical;
			base.RegisterFloat(this.volumeClampJSON);
			this.volumeThresholdJSON = new JSONStorableFloat("volumeThreshold", this.speechBlend.volumeThreshold, new JSONStorableFloat.SetFloatCallback(this.SyncVolumeThreshold), 0f, 1f, true, true);
			this.volumeThresholdJSON.storeType = JSONStorableParam.StoreType.Physical;
			base.RegisterFloat(this.volumeThresholdJSON);
			this.mouthOpenFactorJSON = new JSONStorableFloat("mouthOpenFactor", this.speechBlend.jawMovementAmount, new JSONStorableFloat.SetFloatCallback(this.SyncMouthOpenFactorAmount), 0f, 1f, true, true);
			this.mouthOpenFactorJSON.storeType = JSONStorableParam.StoreType.Physical;
			base.RegisterFloat(this.mouthOpenFactorJSON);
			this.mouthOpenChangeRateJSON = new JSONStorableFloat("mouthOpenChangeRate", this.speechBlend.jawMovementSpeed, new JSONStorableFloat.SetFloatCallback(this.SyncMouthOpenChangeRate), 0f, 1f, true, true);
			this.mouthOpenChangeRateJSON.storeType = JSONStorableParam.StoreType.Physical;
			base.RegisterFloat(this.mouthOpenChangeRateJSON);
			this.visemeDetectionFactorJSON = new JSONStorableFloat("visemeDetectionFactor", this.speechBlend.lipsBlendshapeMovementAmount, new JSONStorableFloat.SetFloatCallback(this.SyncVisemeDetectionFactor), -0.5f, 1f, true, true);
			this.visemeDetectionFactorJSON.storeType = JSONStorableParam.StoreType.Physical;
			base.RegisterFloat(this.visemeDetectionFactorJSON);
			this.visemeThresholdJSON = new JSONStorableFloat("visemeThreshold", this.speechBlend.visemeThreshold, new JSONStorableFloat.SetFloatCallback(this.SyncVisemeThreshold), 0f, 1f, true, true);
			this.visemeThresholdJSON.storeType = JSONStorableParam.StoreType.Physical;
			base.RegisterFloat(this.visemeThresholdJSON);
			this.timeBetweenSamplingJSON = new JSONStorableFloat("timeBetweenSampling", this.speechBlend.timeBetweenSampling, new JSONStorableFloat.SetFloatCallback(this.SyncTimeBetweenSampling), 0f, 0.15f, true, true);
			this.timeBetweenSamplingJSON.storeType = JSONStorableParam.StoreType.Physical;
			base.RegisterFloat(this.timeBetweenSamplingJSON);
			this.sampleTimeAdjustJSON = new JSONStorableFloat("sampleTimeAdjust", this.speechBlend.lookaheadAdjust, new JSONStorableFloat.SetFloatCallback(this.SyncSampleTimeAdjust), -0.2f, 0.2f, true, true);
			this.sampleTimeAdjustJSON.storeType = JSONStorableParam.StoreType.Physical;
			base.RegisterFloat(this.sampleTimeAdjustJSON);
			this.visemeMorphChangeRateJSON = new JSONStorableFloat("visemeMorphChangeRate", this.speechBlend.lipsBlendshapeChangeSpeed, new JSONStorableFloat.SetFloatCallback(this.SyncVisemeMorphChangeRate), 0f, 1f, true, true);
			this.visemeMorphChangeRateJSON.storeType = JSONStorableParam.StoreType.Physical;
			base.RegisterFloat(this.visemeMorphChangeRateJSON);
			this.visemeMorphClampJSON = new JSONStorableFloat("visemeMorphClamp", this.speechBlend.blendshapeCutoff, new JSONStorableFloat.SetFloatCallback(this.SyncVisemeMorphClamp), 0f, 1f, true, true);
			this.visemeMorphClampJSON.storeType = JSONStorableParam.StoreType.Physical;
			base.RegisterFloat(this.visemeMorphClampJSON);
			List<string> choicesList = new List<string>(Enum.GetNames(typeof(VoiceProfile.VoiceType)));
			this.voiceTypeJSON = new JSONStorableStringChooser("voiceType", choicesList, this.speechBlend.voiceType.ToString(), "Voice Type", new JSONStorableStringChooser.SetStringCallback(this.SyncVoiceType));
			this.voiceTypeJSON.storeType = JSONStorableParam.StoreType.Physical;
			base.RegisterStringChooser(this.voiceTypeJSON);
			List<string> choicesList2 = new List<string>(Enum.GetNames(typeof(SpeechBlendControl.MorphSetType)));
			this.morphSetJSON = new JSONStorableStringChooser("morphSet", choicesList2, ((SpeechBlendControl.MorphSetType)this.speechBlend.setChoice).ToString(), "Morph Set", new JSONStorableStringChooser.SetStringCallback(this.SyncMorphSet));
			this.morphSetJSON.storeType = JSONStorableParam.StoreType.Physical;
			base.RegisterStringChooser(this.morphSetJSON);
			this.openAdvancedPanelAction = new JSONStorableAction("Open Advanced", new JSONStorableAction.ActionCallback(this.OpenAdvancedPanel));
			base.RegisterAction(this.openAdvancedPanelAction);
			if (this.speechBlend.faceBlendshapeNamesArray.Length == 3)
			{
				SpeechUtil.VisemeBlendshapeNames visemeBlendshapeNames = this.speechBlend.faceBlendshapeNamesArray[0];
				SpeechUtil.VisemeWeight visemeWeight = this.speechBlend.visemeWeightTuningArray[0];
				this.normalVisemeNamesJSON = new JSONStorableString[visemeBlendshapeNames.template.Nvis];
				this.normalMorphUidsJSON = new JSONStorableString[visemeBlendshapeNames.template.Nvis];
				this.normalVisemeWeightsJSON = new JSONStorableFloat[visemeBlendshapeNames.template.Nvis];
				for (int i = 0; i < this.normalVisemeNamesJSON.Length; i++)
				{
					SpeechBlendControl.<Init>c__AnonStorey0 <Init>c__AnonStorey = new SpeechBlendControl.<Init>c__AnonStorey0();
					<Init>c__AnonStorey.$this = this;
					string text = visemeBlendshapeNames.template.visemeNames[i];
					this.normalVisemeNamesJSON[i] = new JSONStorableString("normalVisemeName" + i, text);
					this.normalMorphUidsJSON[i] = new JSONStorableString("normalMorphUid" + text, visemeBlendshapeNames.visemeNames[i]);
					this.normalMorphUidsJSON[i].interactable = false;
					<Init>c__AnonStorey.index = i;
					this.normalVisemeWeightsJSON[i] = new JSONStorableFloat("normalVisemeWeight" + text, visemeWeight.weights[i], new JSONStorableFloat.SetFloatCallback(<Init>c__AnonStorey.<>m__0), 0f, 2f, true, true);
					this.normalVisemeWeightsJSON[i].storeType = JSONStorableParam.StoreType.Physical;
					base.RegisterFloat(this.normalVisemeWeightsJSON[i]);
				}
				this.normalMouthOpenMorphUidJSON = new JSONStorableString("normalMouthOpenMorphUid", visemeBlendshapeNames.mouthOpenName);
				this.normalMouthOpenMorphUidJSON.interactable = false;
				visemeBlendshapeNames = this.speechBlend.faceBlendshapeNamesArray[1];
				visemeWeight = this.speechBlend.visemeWeightTuningArray[1];
				this.physicsVisemeNamesJSON = new JSONStorableString[visemeBlendshapeNames.template.Nvis];
				this.physicsMorphUidsJSON = new JSONStorableString[visemeBlendshapeNames.template.Nvis];
				this.physicsVisemeWeightsJSON = new JSONStorableFloat[visemeBlendshapeNames.template.Nvis];
				for (int j = 0; j < this.physicsVisemeNamesJSON.Length; j++)
				{
					SpeechBlendControl.<Init>c__AnonStorey1 <Init>c__AnonStorey2 = new SpeechBlendControl.<Init>c__AnonStorey1();
					<Init>c__AnonStorey2.$this = this;
					string text2 = visemeBlendshapeNames.template.visemeNames[j];
					this.physicsVisemeNamesJSON[j] = new JSONStorableString("physicsVisemeName" + j, text2);
					this.physicsMorphUidsJSON[j] = new JSONStorableString("physicsMorphUid" + text2, visemeBlendshapeNames.visemeNames[j]);
					this.physicsMorphUidsJSON[j].interactable = false;
					<Init>c__AnonStorey2.index = j;
					this.physicsVisemeWeightsJSON[j] = new JSONStorableFloat("physicsVisemeWeight" + text2, visemeWeight.weights[j], new JSONStorableFloat.SetFloatCallback(<Init>c__AnonStorey2.<>m__0), 0f, 2f, true, true);
					this.physicsVisemeWeightsJSON[j].storeType = JSONStorableParam.StoreType.Physical;
					base.RegisterFloat(this.physicsVisemeWeightsJSON[j]);
				}
				this.physicsMouthOpenMorphUidJSON = new JSONStorableString("physicsMouthOpenMorphUid", visemeBlendshapeNames.mouthOpenName);
				this.physicsMouthOpenMorphUidJSON.interactable = false;
				visemeBlendshapeNames = this.speechBlend.faceBlendshapeNamesArray[2];
				visemeWeight = this.speechBlend.visemeWeightTuningArray[2];
				this.customVisemeNamesJSON = new JSONStorableString[visemeBlendshapeNames.template.Nvis];
				this.customMorphUidsJSON = new JSONStorableString[visemeBlendshapeNames.template.Nvis];
				this.pasteCustomMorphUidAction = new JSONStorableAction[visemeBlendshapeNames.template.Nvis];
				this.customVisemeWeightsJSON = new JSONStorableFloat[visemeBlendshapeNames.template.Nvis];
				for (int k = 0; k < this.customVisemeNamesJSON.Length; k++)
				{
					SpeechBlendControl.<Init>c__AnonStorey2 <Init>c__AnonStorey3 = new SpeechBlendControl.<Init>c__AnonStorey2();
					<Init>c__AnonStorey3.$this = this;
					<Init>c__AnonStorey3.index = k;
					string text3 = visemeBlendshapeNames.template.visemeNames[k];
					this.customVisemeNamesJSON[k] = new JSONStorableString("customVisemeName" + k, text3);
					this.customMorphUidsJSON[k] = new JSONStorableString("customMorphUid" + text3, visemeBlendshapeNames.visemeNames[k], new JSONStorableString.SetStringCallback(<Init>c__AnonStorey3.<>m__0));
					base.RegisterString(this.customMorphUidsJSON[k]);
					this.pasteCustomMorphUidAction[k] = new JSONStorableAction("pasteCustomMorphUid" + text3, new JSONStorableAction.ActionCallback(<Init>c__AnonStorey3.<>m__1));
					base.RegisterAction(this.pasteCustomMorphUidAction[k]);
					this.customVisemeWeightsJSON[k] = new JSONStorableFloat("customVisemeWeight" + text3, visemeWeight.weights[k], new JSONStorableFloat.SetFloatCallback(<Init>c__AnonStorey3.<>m__2), 0f, 2f, true, true);
					this.customVisemeWeightsJSON[k].storeType = JSONStorableParam.StoreType.Physical;
					base.RegisterFloat(this.customVisemeWeightsJSON[k]);
				}
				this.customMouthOpenMorphUidJSON = new JSONStorableString("customMouthOpenMorphUid", visemeBlendshapeNames.mouthOpenName);
				this.customMouthOpenMorphUidJSON.storeType = JSONStorableParam.StoreType.Physical;
				base.RegisterString(this.customMouthOpenMorphUidJSON);
				this.pasteCustomMouthOpenMorphUidAction = new JSONStorableAction("pasteCustomMouthMorphUid", new JSONStorableAction.ActionCallback(this.<Init>m__0));
				base.RegisterAction(this.pasteCustomMouthOpenMorphUidAction);
				this.visemeFoundJSON = new JSONStorableBool[visemeBlendshapeNames.template.Nvis];
				this.visemeValuesJSON = new JSONStorableFloat[visemeBlendshapeNames.template.Nvis];
				this.visemeRawValuesJSON = new JSONStorableFloat[visemeBlendshapeNames.template.Nvis];
				for (int l = 0; l < this.visemeValuesJSON.Length; l++)
				{
					string str = visemeBlendshapeNames.template.visemeNames[l];
					this.visemeFoundJSON[l] = new JSONStorableBool("visemeFound" + str, false);
					this.visemeFoundJSON[l].isStorable = false;
					this.visemeFoundJSON[l].isRestorable = false;
					base.RegisterBool(this.visemeFoundJSON[l]);
					this.visemeRawValuesJSON[l] = new JSONStorableFloat("visemeRawValue" + str, 0f, 0f, 1f, true, false);
					this.visemeRawValuesJSON[l].isStorable = false;
					this.visemeRawValuesJSON[l].isRestorable = false;
					base.RegisterFloat(this.visemeRawValuesJSON[l]);
					this.visemeValuesJSON[l] = new JSONStorableFloat("visemeValue" + str, 0f, 0f, 1f, true, false);
					this.visemeValuesJSON[l].isStorable = false;
					this.visemeValuesJSON[l].isRestorable = false;
					base.RegisterFloat(this.visemeValuesJSON[l]);
				}
				this.mouthOpenVisemeFoundJSON = new JSONStorableBool("mouthOpenVisemeFound", false);
				this.mouthOpenVisemeFoundJSON.isStorable = false;
				this.mouthOpenVisemeFoundJSON.isRestorable = false;
				base.RegisterBool(this.mouthOpenVisemeFoundJSON);
				this.mouthOpenVisemeValueJSON = new JSONStorableFloat("mouthOpenVisemeValue", 0f, 0f, 1f, true, false);
				this.mouthOpenVisemeValueJSON.isStorable = false;
				this.mouthOpenVisemeValueJSON.isRestorable = false;
				base.RegisterFloat(this.mouthOpenVisemeValueJSON);
				this.SyncMorphSetParams();
				this.resetAllAdvancedSettingsAction = new JSONStorableAction("ResetAllAdvancedSettings", new JSONStorableAction.ActionCallback(this.ResetAllAdvancedSettings));
				base.RegisterAction(this.resetAllAdvancedSettingsAction);
			}
			else
			{
				UnityEngine.Debug.LogError("Speech blend is not using exactly 3 sets");
			}
			if (this.audioSourceControl != null)
			{
				AudioSourceControl audioSourceControl = this.audioSourceControl;
				audioSourceControl.onMicStartHandlers = (AudioSourceControl.OnMicStart)Delegate.Combine(audioSourceControl.onMicStartHandlers, new AudioSourceControl.OnMicStart(this.<Init>m__1));
				AudioSourceControl audioSourceControl2 = this.audioSourceControl;
				audioSourceControl2.onMicStopHandlers = (AudioSourceControl.OnMicStop)Delegate.Combine(audioSourceControl2.onMicStopHandlers, new AudioSourceControl.OnMicStop(this.<Init>m__2));
			}
		}
	}

	// Token: 0x0600531A RID: 21274 RVA: 0x001E0C08 File Offset: 0x001DF008
	protected void Update()
	{
		if (this.speechBlend != null && this.volumeJSON != null)
		{
			this.volumeJSON.val = this.speechBlend.current_volume;
			this.maxVolumeJSON.val = this.speechBlend.recent_max_volume;
			if (this.speechBlend.VisemeValues != null)
			{
				for (int i = 0; i < this.visemeValuesJSON.Length; i++)
				{
					this.visemeValuesJSON[i].val = this.speechBlend.VisemeValues[i];
					this.visemeRawValuesJSON[i].val = this.speechBlend.VisemeRawValues[i];
				}
			}
			this.mouthOpenVisemeValueJSON.val = this.speechBlend.MouthOpenValue;
		}
	}

	// Token: 0x0600531B RID: 21275 RVA: 0x001E0CD0 File Offset: 0x001DF0D0
	protected override void Awake()
	{
		if (!this.awakecalled)
		{
			base.Awake();
			this.Init();
			this.InitUI();
			this.InitUIAlt();
		}
	}

	// Token: 0x0600531C RID: 21276 RVA: 0x001E0CF5 File Offset: 0x001DF0F5
	[CompilerGenerated]
	private void <Init>m__0()
	{
		this.customMouthOpenMorphUidJSON.val = DAZMorph.uidCopy;
	}

	// Token: 0x0600531D RID: 21277 RVA: 0x001E0D07 File Offset: 0x001DF107
	[CompilerGenerated]
	private void <Init>m__1()
	{
		this.speechBlend.liveMode = true;
	}

	// Token: 0x0600531E RID: 21278 RVA: 0x001E0D15 File Offset: 0x001DF115
	[CompilerGenerated]
	private void <Init>m__2()
	{
		this.speechBlend.liveMode = false;
	}

	// Token: 0x040042B4 RID: 17076
	public SpeechBlend speechBlend;

	// Token: 0x040042B5 RID: 17077
	public AudioSourceControl audioSourceControl;

	// Token: 0x040042B6 RID: 17078
	protected JSONStorableBool enabledJSON;

	// Token: 0x040042B7 RID: 17079
	protected JSONStorableFloat volumeJSON;

	// Token: 0x040042B8 RID: 17080
	protected JSONStorableFloat maxVolumeJSON;

	// Token: 0x040042B9 RID: 17081
	[CompilerGenerated]
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private JSONStorableFloat <volumeMultiplierJSON>k__BackingField;

	// Token: 0x040042BA RID: 17082
	[CompilerGenerated]
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private JSONStorableFloat <volumeClampJSON>k__BackingField;

	// Token: 0x040042BB RID: 17083
	[CompilerGenerated]
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private JSONStorableFloat <volumeThresholdJSON>k__BackingField;

	// Token: 0x040042BC RID: 17084
	[CompilerGenerated]
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private JSONStorableFloat <mouthOpenFactorJSON>k__BackingField;

	// Token: 0x040042BD RID: 17085
	[CompilerGenerated]
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private JSONStorableFloat <mouthOpenChangeRateJSON>k__BackingField;

	// Token: 0x040042BE RID: 17086
	[CompilerGenerated]
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private JSONStorableFloat <visemeDetectionFactorJSON>k__BackingField;

	// Token: 0x040042BF RID: 17087
	[CompilerGenerated]
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private JSONStorableFloat <visemeThresholdJSON>k__BackingField;

	// Token: 0x040042C0 RID: 17088
	[CompilerGenerated]
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private JSONStorableFloat <timeBetweenSamplingJSON>k__BackingField;

	// Token: 0x040042C1 RID: 17089
	[CompilerGenerated]
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private JSONStorableFloat <sampleTimeAdjustJSON>k__BackingField;

	// Token: 0x040042C2 RID: 17090
	[CompilerGenerated]
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private JSONStorableFloat <visemeMorphChangeRateJSON>k__BackingField;

	// Token: 0x040042C3 RID: 17091
	[CompilerGenerated]
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private JSONStorableFloat <visemeMorphClampJSON>k__BackingField;

	// Token: 0x040042C4 RID: 17092
	[CompilerGenerated]
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private JSONStorableStringChooser <voiceTypeJSON>k__BackingField;

	// Token: 0x040042C5 RID: 17093
	protected SpeechBlendControl.MorphSetType _morphSetType;

	// Token: 0x040042C6 RID: 17094
	public JSONStorableStringChooser morphSetJSON;

	// Token: 0x040042C7 RID: 17095
	protected GameObject advancedPanel;

	// Token: 0x040042C8 RID: 17096
	public JSONStorableAction openAdvancedPanelAction;

	// Token: 0x040042C9 RID: 17097
	protected InputField mouthOpenMorphUidInputField;

	// Token: 0x040042CA RID: 17098
	protected JSONStorableString[] normalVisemeNamesJSON;

	// Token: 0x040042CB RID: 17099
	protected JSONStorableString[] physicsVisemeNamesJSON;

	// Token: 0x040042CC RID: 17100
	public JSONStorableString[] customVisemeNamesJSON;

	// Token: 0x040042CD RID: 17101
	protected JSONStorableString[] normalMorphUidsJSON;

	// Token: 0x040042CE RID: 17102
	protected JSONStorableString[] physicsMorphUidsJSON;

	// Token: 0x040042CF RID: 17103
	public JSONStorableString[] customMorphUidsJSON;

	// Token: 0x040042D0 RID: 17104
	public JSONStorableAction[] pasteCustomMorphUidAction;

	// Token: 0x040042D1 RID: 17105
	protected JSONStorableString normalMouthOpenMorphUidJSON;

	// Token: 0x040042D2 RID: 17106
	protected JSONStorableString physicsMouthOpenMorphUidJSON;

	// Token: 0x040042D3 RID: 17107
	public JSONStorableString customMouthOpenMorphUidJSON;

	// Token: 0x040042D4 RID: 17108
	public JSONStorableAction pasteCustomMouthOpenMorphUidAction;

	// Token: 0x040042D5 RID: 17109
	public JSONStorableFloat[] normalVisemeWeightsJSON;

	// Token: 0x040042D6 RID: 17110
	public JSONStorableFloat[] physicsVisemeWeightsJSON;

	// Token: 0x040042D7 RID: 17111
	public JSONStorableFloat[] customVisemeWeightsJSON;

	// Token: 0x040042D8 RID: 17112
	public JSONStorableBool[] visemeFoundJSON;

	// Token: 0x040042D9 RID: 17113
	public JSONStorableFloat[] visemeRawValuesJSON;

	// Token: 0x040042DA RID: 17114
	public JSONStorableFloat[] visemeValuesJSON;

	// Token: 0x040042DB RID: 17115
	public JSONStorableBool mouthOpenVisemeFoundJSON;

	// Token: 0x040042DC RID: 17116
	public JSONStorableFloat mouthOpenVisemeValueJSON;

	// Token: 0x040042DD RID: 17117
	protected JSONStorableString[] currentVisemeNamesJSON;

	// Token: 0x040042DE RID: 17118
	protected JSONStorableString[] currentMorphUidsJSON;

	// Token: 0x040042DF RID: 17119
	protected JSONStorableString currentMouthOpenMorphUidJSON;

	// Token: 0x040042E0 RID: 17120
	protected JSONStorableFloat[] currentWeightsJSON;

	// Token: 0x040042E1 RID: 17121
	public JSONStorableAction resetAllAdvancedSettingsAction;

	// Token: 0x040042E2 RID: 17122
	protected bool visemeValuesJSONUIConnected;

	// Token: 0x040042E3 RID: 17123
	protected SpeechBlendVisemeUI[] visemeUIs;

	// Token: 0x02000B85 RID: 2949
	public enum MorphSetType
	{
		// Token: 0x040042E5 RID: 17125
		Normal,
		// Token: 0x040042E6 RID: 17126
		Physics,
		// Token: 0x040042E7 RID: 17127
		Custom
	}

	// Token: 0x02000FDB RID: 4059
	[CompilerGenerated]
	private sealed class <Init>c__AnonStorey0
	{
		// Token: 0x0600757E RID: 30078 RVA: 0x001E0D23 File Offset: 0x001DF123
		public <Init>c__AnonStorey0()
		{
		}

		// Token: 0x0600757F RID: 30079 RVA: 0x001E0D2B File Offset: 0x001DF12B
		internal void <>m__0(float f)
		{
			this.$this.speechBlend.visemeWeightTuningArray[0].weights[this.index] = f;
		}

		// Token: 0x04006998 RID: 27032
		internal int index;

		// Token: 0x04006999 RID: 27033
		internal SpeechBlendControl $this;
	}

	// Token: 0x02000FDC RID: 4060
	[CompilerGenerated]
	private sealed class <Init>c__AnonStorey1
	{
		// Token: 0x06007580 RID: 30080 RVA: 0x001E0D4C File Offset: 0x001DF14C
		public <Init>c__AnonStorey1()
		{
		}

		// Token: 0x06007581 RID: 30081 RVA: 0x001E0D54 File Offset: 0x001DF154
		internal void <>m__0(float f)
		{
			this.$this.speechBlend.visemeWeightTuningArray[1].weights[this.index] = f;
		}

		// Token: 0x0400699A RID: 27034
		internal int index;

		// Token: 0x0400699B RID: 27035
		internal SpeechBlendControl $this;
	}

	// Token: 0x02000FDD RID: 4061
	[CompilerGenerated]
	private sealed class <Init>c__AnonStorey2
	{
		// Token: 0x06007582 RID: 30082 RVA: 0x001E0D75 File Offset: 0x001DF175
		public <Init>c__AnonStorey2()
		{
		}

		// Token: 0x06007583 RID: 30083 RVA: 0x001E0D7D File Offset: 0x001DF17D
		internal void <>m__0(string s)
		{
			this.$this.speechBlend.faceBlendshapeNamesArray[2].visemeNames[this.index] = s;
			this.$this.speechBlend.InitMorphs();
		}

		// Token: 0x06007584 RID: 30084 RVA: 0x001E0DAE File Offset: 0x001DF1AE
		internal void <>m__1()
		{
			this.$this.customMorphUidsJSON[this.index].val = DAZMorph.uidCopy;
		}

		// Token: 0x06007585 RID: 30085 RVA: 0x001E0DCC File Offset: 0x001DF1CC
		internal void <>m__2(float f)
		{
			this.$this.speechBlend.visemeWeightTuningArray[2].weights[this.index] = f;
		}

		// Token: 0x0400699C RID: 27036
		internal int index;

		// Token: 0x0400699D RID: 27037
		internal SpeechBlendControl $this;
	}
}
