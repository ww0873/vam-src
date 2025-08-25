using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using SimpleJSON;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

// Token: 0x02000B69 RID: 2921
public class MotionAnimationMaster : JSONStorableTriggerHandler, AnimationTimelineTriggerHandler
{
	// Token: 0x06005192 RID: 20882 RVA: 0x001D7274 File Offset: 0x001D5674
	public MotionAnimationMaster()
	{
	}

	// Token: 0x06005193 RID: 20883 RVA: 0x001D72E5 File Offset: 0x001D56E5
	public override string[] GetCustomParamNames()
	{
		return this.customParamNames;
	}

	// Token: 0x06005194 RID: 20884 RVA: 0x001D72F0 File Offset: 0x001D56F0
	public override JSONClass GetJSON(bool includePhysical = true, bool includeAppearance = true, bool forceStore = false)
	{
		JSONClass json = base.GetJSON(includePhysical, includeAppearance, forceStore);
		if (includePhysical || forceStore)
		{
			if (this._recordedLength > 0f)
			{
				this.needsStore = true;
				json["recordedLength"].AsFloat = this._recordedLength;
			}
			if (this.triggers != null)
			{
				this.needsStore = true;
				JSONArray jsonarray = new JSONArray();
				json["triggers"] = jsonarray;
				foreach (AnimationTimelineTrigger animationTimelineTrigger in this.triggers)
				{
					jsonarray.Add(animationTimelineTrigger.GetJSON(base.subScenePrefix));
				}
			}
			if (this._audioSourceControl != null && this._audioSourceControl.containingAtom != null)
			{
				string text = base.AtomUidToStoreAtomUid(this._audioSourceControl.containingAtom.uid);
				if (text != null)
				{
					this.needsStore = true;
					json["audioSourceControl"] = text + ":" + this._audioSourceControl.name;
				}
				else
				{
					SuperController.LogError(string.Concat(new object[]
					{
						"Warning: AudioSourceControl in atom ",
						this.containingAtom,
						" uses audio source control in atom ",
						this._audioSourceControl.containingAtom.uid,
						" that is not in subscene and cannot be saved"
					}));
				}
			}
		}
		return json;
	}

	// Token: 0x06005195 RID: 20885 RVA: 0x001D747C File Offset: 0x001D587C
	public override void PreRestore(bool restorePhysical, bool restoreAppearance)
	{
		if (restorePhysical && !base.physicalLocked)
		{
			if (!base.mergeRestore && !base.IsCustomPhysicalParamLocked("triggers"))
			{
				this.ClearTriggers();
			}
			if (!base.IsCustomPhysicalParamLocked("recordedLength"))
			{
				this.StopPlayback();
				this._playbackCounter = 0f;
			}
		}
	}

	// Token: 0x06005196 RID: 20886 RVA: 0x001D74DC File Offset: 0x001D58DC
	public override void RestoreFromJSON(JSONClass jc, bool restorePhysical = true, bool restoreAppearance = true, JSONArray presetAtoms = null, bool setMissingToDefault = true)
	{
		base.RestoreFromJSON(jc, restorePhysical, restoreAppearance, presetAtoms, setMissingToDefault);
		if (!base.physicalLocked && restorePhysical)
		{
			if (!base.IsCustomPhysicalParamLocked("recordedLength"))
			{
				if (jc["recordedLength"] != null)
				{
					this.recordedLength = jc["recordedLength"].AsFloat;
					if (this._recordedLength > 0f)
					{
						this.InternalSetPlaybackCounter(this._startTimestep, false, false);
						if (this._autoPlay && (!this.isSceneMasterController || SuperController.singleton == null || SuperController.singleton.gameMode == SuperController.GameMode.Play))
						{
							this.StartPlayback();
						}
					}
				}
				else if (setMissingToDefault)
				{
					this.recordedLength = 0f;
				}
			}
			if (!base.IsCustomPhysicalParamLocked("audioSourceControl"))
			{
				if (jc["audioSourceControl"] != null)
				{
					string audioSourceControl = base.StoredAtomUidToAtomUid(jc["audioSourceControl"]);
					this.SetAudioSourceControl(audioSourceControl);
				}
				else if (setMissingToDefault)
				{
					this.SetAudioSourceControl(string.Empty);
				}
			}
		}
	}

	// Token: 0x06005197 RID: 20887 RVA: 0x001D7610 File Offset: 0x001D5A10
	public override void LateRestoreFromJSON(JSONClass jc, bool restorePhysical = true, bool restoreAppearance = true, bool setMissingToDefault = true)
	{
		base.LateRestoreFromJSON(jc, restorePhysical, restoreAppearance, setMissingToDefault);
		if (!base.physicalLocked && restorePhysical && !base.IsCustomPhysicalParamLocked("triggers"))
		{
			if (jc["triggers"] != null)
			{
				if (!base.mergeRestore)
				{
					this.ClearTriggers();
				}
				JSONArray asArray = jc["triggers"].AsArray;
				if (asArray != null)
				{
					IEnumerator enumerator = asArray.GetEnumerator();
					try
					{
						while (enumerator.MoveNext())
						{
							object obj = enumerator.Current;
							JSONNode jsonnode = (JSONNode)obj;
							JSONClass asObject = jsonnode.AsObject;
							if (asObject != null)
							{
								AnimationTimelineTrigger animationTimelineTrigger = this.AddTriggerInternal(-1);
								animationTimelineTrigger.RestoreFromJSON(asObject, base.subScenePrefix, base.mergeRestore);
							}
						}
					}
					finally
					{
						IDisposable disposable;
						if ((disposable = (enumerator as IDisposable)) != null)
						{
							disposable.Dispose();
						}
					}
				}
			}
			else if (setMissingToDefault && !base.mergeRestore)
			{
				this.ClearTriggers();
			}
		}
	}

	// Token: 0x06005198 RID: 20888 RVA: 0x001D7728 File Offset: 0x001D5B28
	public override void Validate()
	{
		base.Validate();
		foreach (Trigger trigger in this.triggers)
		{
			trigger.Validate();
		}
	}

	// Token: 0x06005199 RID: 20889 RVA: 0x001D778C File Offset: 0x001D5B8C
	public void RegisterAnimationControl(MotionAnimationControl mac)
	{
		if (this.controllers.Add(mac))
		{
			if (mac.animationMaster != null)
			{
				mac.animationMaster.DeregisterAnimationControl(mac);
			}
			mac.animationMaster = this;
		}
	}

	// Token: 0x0600519A RID: 20890 RVA: 0x001D77C3 File Offset: 0x001D5BC3
	public void DeregisterAnimationControl(MotionAnimationControl mac)
	{
		if (this.controllers.Remove(mac))
		{
			mac.animationMaster = null;
		}
	}

	// Token: 0x0600519B RID: 20891 RVA: 0x001D77DD File Offset: 0x001D5BDD
	protected void SyncLinkToAudioSourceControl(bool b)
	{
		this._linkToAudioSourceControl = b;
	}

	// Token: 0x17000BDA RID: 3034
	// (get) Token: 0x0600519C RID: 20892 RVA: 0x001D77E6 File Offset: 0x001D5BE6
	// (set) Token: 0x0600519D RID: 20893 RVA: 0x001D77EE File Offset: 0x001D5BEE
	public bool linkToAudioSourceControl
	{
		get
		{
			return this._linkToAudioSourceControl;
		}
		set
		{
			if (this.linkToAudioSourceControlJSON != null)
			{
				this.linkToAudioSourceControlJSON.val = value;
			}
			else
			{
				this.SyncLinkToAudioSourceControl(value);
			}
		}
	}

	// Token: 0x0600519E RID: 20894 RVA: 0x001D7813 File Offset: 0x001D5C13
	protected void SyncAudioSourceTimeOffset(float f)
	{
		this._audioSourceTimeOffset = f;
	}

	// Token: 0x17000BDB RID: 3035
	// (get) Token: 0x0600519F RID: 20895 RVA: 0x001D781C File Offset: 0x001D5C1C
	// (set) Token: 0x060051A0 RID: 20896 RVA: 0x001D7824 File Offset: 0x001D5C24
	public float audioSourceTimeOffset
	{
		get
		{
			return this._audioSourceTimeOffset;
		}
		set
		{
			if (this.audioSourceTimeOffsetJSON != null)
			{
				this.audioSourceTimeOffsetJSON.val = value;
			}
			else
			{
				this.SyncAudioSourceTimeOffset(value);
			}
		}
	}

	// Token: 0x060051A1 RID: 20897 RVA: 0x001D784C File Offset: 0x001D5C4C
	protected virtual void SetAudioSourceControlAtomNames()
	{
		if (this.audioSourceControlAtomSelectionPopup != null && SuperController.singleton != null)
		{
			List<string> atomUIDsWithAudioSourceControls = SuperController.singleton.GetAtomUIDsWithAudioSourceControls();
			if (atomUIDsWithAudioSourceControls == null)
			{
				this.audioSourceControlAtomSelectionPopup.numPopupValues = 1;
				this.audioSourceControlAtomSelectionPopup.setPopupValue(0, "None");
			}
			else
			{
				this.audioSourceControlAtomSelectionPopup.numPopupValues = atomUIDsWithAudioSourceControls.Count + 1;
				this.audioSourceControlAtomSelectionPopup.setPopupValue(0, "None");
				for (int i = 0; i < atomUIDsWithAudioSourceControls.Count; i++)
				{
					this.audioSourceControlAtomSelectionPopup.setPopupValue(i + 1, atomUIDsWithAudioSourceControls[i]);
				}
			}
		}
	}

	// Token: 0x060051A2 RID: 20898 RVA: 0x001D7900 File Offset: 0x001D5D00
	protected virtual void onAudioSourceControlNamesChanged(List<string> rcNames)
	{
		if (this.audioSourceControlSelectionPopup != null)
		{
			if (rcNames == null)
			{
				this.audioSourceControlSelectionPopup.numPopupValues = 1;
				this.audioSourceControlSelectionPopup.setPopupValue(0, "None");
			}
			else
			{
				this.audioSourceControlSelectionPopup.numPopupValues = rcNames.Count + 1;
				this.audioSourceControlSelectionPopup.setPopupValue(0, "None");
				for (int i = 0; i < rcNames.Count; i++)
				{
					this.audioSourceControlSelectionPopup.setPopupValue(i + 1, rcNames[i]);
				}
			}
		}
	}

	// Token: 0x060051A3 RID: 20899 RVA: 0x001D7998 File Offset: 0x001D5D98
	public virtual void SetAudioSourceControlAtom(string atomUID)
	{
		if (SuperController.singleton != null)
		{
			Atom atomByUid = SuperController.singleton.GetAtomByUid(atomUID);
			if (atomByUid != null)
			{
				this.audioSourceControlAtomUID = atomUID;
				List<string> audioSourceControlNamesInAtom = SuperController.singleton.GetAudioSourceControlNamesInAtom(this.audioSourceControlAtomUID);
				this.onAudioSourceControlNamesChanged(audioSourceControlNamesInAtom);
				if (this.audioSourceControlSelectionPopup != null)
				{
					this.audioSourceControlSelectionPopup.currentValue = "None";
				}
			}
			else
			{
				this.onAudioSourceControlNamesChanged(null);
			}
		}
	}

	// Token: 0x060051A4 RID: 20900 RVA: 0x001D7A19 File Offset: 0x001D5E19
	public virtual void SetAudioSourceControlObject(string objectName)
	{
		if (this.audioSourceControlAtomUID != null && SuperController.singleton != null)
		{
			this.audioSourceControl = SuperController.singleton.AudioSourceControlrNameToAudioSourceControl(this.audioSourceControlAtomUID + ":" + objectName);
		}
	}

	// Token: 0x060051A5 RID: 20901 RVA: 0x001D7A58 File Offset: 0x001D5E58
	public virtual void SetAudioSourceControl(string controllerName)
	{
		if (SuperController.singleton != null)
		{
			AudioSourceControl audioSourceControl = SuperController.singleton.AudioSourceControlrNameToAudioSourceControl(controllerName);
			if (audioSourceControl != null)
			{
				if (this.audioSourceControlAtomSelectionPopup != null && audioSourceControl.containingAtom != null)
				{
					this.audioSourceControlAtomSelectionPopup.currentValue = audioSourceControl.containingAtom.uid;
				}
				if (this.audioSourceControlSelectionPopup != null)
				{
					this.audioSourceControlSelectionPopup.currentValue = audioSourceControl.name;
				}
			}
			else
			{
				if (this.audioSourceControlAtomSelectionPopup != null)
				{
					this.audioSourceControlAtomSelectionPopup.currentValue = "None";
				}
				if (this.audioSourceControlSelectionPopup != null)
				{
					this.audioSourceControlSelectionPopup.currentValue = "None";
				}
			}
			this.audioSourceControl = audioSourceControl;
		}
	}

	// Token: 0x17000BDC RID: 3036
	// (get) Token: 0x060051A6 RID: 20902 RVA: 0x001D7B35 File Offset: 0x001D5F35
	// (set) Token: 0x060051A7 RID: 20903 RVA: 0x001D7B3D File Offset: 0x001D5F3D
	public AudioSourceControl audioSourceControl
	{
		get
		{
			return this._audioSourceControl;
		}
		set
		{
			this._audioSourceControl = value;
		}
	}

	// Token: 0x060051A8 RID: 20904 RVA: 0x001D7B48 File Offset: 0x001D5F48
	protected void SetAudioSourceTime(float f)
	{
		float num = f - this._audioSourceTimeOffset;
		if (num < 0f)
		{
			this.audioSourceControl.Stop();
		}
		else if (num > this.audioSourceControl.audioSource.clip.length)
		{
			this.audioSourceControl.Stop();
		}
		else
		{
			this.audioSourceControl.audioSource.time = num;
		}
	}

	// Token: 0x060051A9 RID: 20905 RVA: 0x001D7BB5 File Offset: 0x001D5FB5
	public float GetTotalTime()
	{
		return this.totalTime;
	}

	// Token: 0x060051AA RID: 20906 RVA: 0x001D7BBD File Offset: 0x001D5FBD
	public float GetCurrentTimeCounter()
	{
		return this._playbackCounter;
	}

	// Token: 0x060051AB RID: 20907 RVA: 0x001D7BC8 File Offset: 0x001D5FC8
	protected void SyncPlaybackCounter(float f)
	{
		if (!this._isRecording)
		{
			if (this._linkToAudioSourceControl && this.audioSourceControl != null && this.audioSourceControl.audioSource.clip != null)
			{
				this.SetAudioSourceTime(f);
			}
			this.InternalSetPlaybackCounter(f, true, false);
		}
	}

	// Token: 0x17000BDD RID: 3037
	// (get) Token: 0x060051AC RID: 20908 RVA: 0x001D7C27 File Offset: 0x001D6027
	// (set) Token: 0x060051AD RID: 20909 RVA: 0x001D7C2F File Offset: 0x001D602F
	public float playbackCounter
	{
		get
		{
			return this._playbackCounter;
		}
		set
		{
			if (this.playbackCounterJSON != null)
			{
				this.playbackCounterJSON.val = value;
			}
			else
			{
				this.SyncPlaybackCounter(value);
			}
		}
	}

	// Token: 0x060051AE RID: 20910 RVA: 0x001D7C54 File Offset: 0x001D6054
	protected void SyncStartTimestep(float f)
	{
		this._startTimestep = f;
	}

	// Token: 0x060051AF RID: 20911 RVA: 0x001D7C5D File Offset: 0x001D605D
	protected void SyncStopTimestep(float f)
	{
		this._stopTimestep = f;
	}

	// Token: 0x060051B0 RID: 20912 RVA: 0x001D7C66 File Offset: 0x001D6066
	protected void SetLoopback()
	{
		this.PlaybackStep();
		this._isLoopingBack = true;
		this._loopbackCounter = 0f;
	}

	// Token: 0x060051B1 RID: 20913 RVA: 0x001D7C80 File Offset: 0x001D6080
	public void ResetAnimation()
	{
		if (SuperController.singleton != null)
		{
			SuperController.singleton.ResetSimulation(5, "MotionAnimation ResetAnimation", true);
		}
		if (this._linkToAudioSourceControl && this.audioSourceControl != null && this.audioSourceControl.audioSource.clip != null)
		{
			this.SetAudioSourceTime(this._startTimestep);
		}
		this._isLoopingBack = false;
		float startTimestep = this._startTimestep;
		this._lastPlaybackCounter = startTimestep;
		this._playbackCounter = startTimestep;
		this._isPlaying = false;
		this.SyncActive();
		this.playbackCounterJSON.val = startTimestep;
		this.PlaybackStep();
		List<AnimationTimelineTrigger> list = new List<AnimationTimelineTrigger>(this.triggers);
		List<AnimationTimelineTrigger> list2 = list;
		if (MotionAnimationMaster.<>f__am$cache0 == null)
		{
			MotionAnimationMaster.<>f__am$cache0 = new Comparison<AnimationTimelineTrigger>(MotionAnimationMaster.<ResetAnimation>m__0);
		}
		list2.Sort(MotionAnimationMaster.<>f__am$cache0);
		foreach (AnimationTimelineTrigger animationTimelineTrigger in list)
		{
			animationTimelineTrigger.Reset();
		}
	}

	// Token: 0x060051B2 RID: 20914 RVA: 0x001D7DA4 File Offset: 0x001D61A4
	protected void InternalSetPlaybackCounter(float val, bool manualSet = false, bool forceAlign = false)
	{
		this._isLoopingBack = false;
		if (this._playbackCounter != val)
		{
			bool flag = false;
			if (manualSet && this._playbackCounter > val)
			{
				flag = true;
			}
			this._lastPlaybackCounter = this._playbackCounter;
			this._playbackCounter = val;
			if (this._isRecording)
			{
				if (this._playbackCounter > this._recordedLength)
				{
					if (this._autoRecordStop && !this._ignoreAutoRecordStop)
					{
						this._playbackCounter = this._recordedLength;
						this.StopRecordMode();
					}
					else
					{
						this.recordedLength = this._playbackCounter;
					}
				}
				this.playbackCounterJSON.valNoCallback = val;
			}
			else if (this._playbackCounter > this._stopTimestep)
			{
				if (!manualSet)
				{
					if (this._loop)
					{
						this.SetLoopback();
					}
					else
					{
						this.StopPlayback();
					}
				}
				this._playbackCounter = this._stopTimestep;
				this.playbackCounterJSON.valNoCallback = this._playbackCounter;
			}
			else if (this._playbackCounter > this.playbackCounterJSON.max)
			{
				if (this._loop)
				{
					this.SetLoopback();
				}
				else
				{
					this.StopPlayback();
				}
				this._playbackCounter = this.playbackCounterJSON.max;
				this.playbackCounterJSON.valNoCallback = this._playbackCounter;
			}
			else
			{
				this.playbackCounterJSON.valNoCallback = val;
			}
			if (!this._isLoopingBack)
			{
				if (forceAlign)
				{
					this.PlaybackStepForceAlign();
				}
				else
				{
					this.PlaybackStep();
				}
			}
			if (flag)
			{
				foreach (AnimationTimelineTrigger animationTimelineTrigger in this.reverseTriggers)
				{
					animationTimelineTrigger.Update(flag, this._lastPlaybackCounter);
				}
			}
			else
			{
				foreach (AnimationTimelineTrigger animationTimelineTrigger2 in this.triggers)
				{
					animationTimelineTrigger2.Update(flag, this._lastPlaybackCounter);
				}
			}
		}
	}

	// Token: 0x060051B3 RID: 20915 RVA: 0x001D7FE4 File Offset: 0x001D63E4
	protected void SyncLoopbackTime(float f)
	{
		this._loopbackTime = f;
	}

	// Token: 0x17000BDE RID: 3038
	// (get) Token: 0x060051B4 RID: 20916 RVA: 0x001D7FED File Offset: 0x001D63ED
	// (set) Token: 0x060051B5 RID: 20917 RVA: 0x001D7FF8 File Offset: 0x001D63F8
	public float loopbackTime
	{
		get
		{
			return this._loopbackTime;
		}
		set
		{
			if (this.loopbackTimeJSON != null)
			{
				this.loopbackTimeJSON.val = value;
			}
			else if (this._loopbackTime != value)
			{
				this._loopbackTime = value;
				if (this.loopbackTimeSlider != null)
				{
					this.loopbackTimeSlider.value = value;
				}
			}
		}
	}

	// Token: 0x060051B6 RID: 20918 RVA: 0x001D8051 File Offset: 0x001D6451
	protected void SyncAutoPlay(bool b)
	{
		this._autoPlay = b;
	}

	// Token: 0x17000BDF RID: 3039
	// (get) Token: 0x060051B7 RID: 20919 RVA: 0x001D805A File Offset: 0x001D645A
	// (set) Token: 0x060051B8 RID: 20920 RVA: 0x001D8062 File Offset: 0x001D6462
	public bool autoPlay
	{
		get
		{
			return this._autoPlay;
		}
		set
		{
			if (this.autoPlayJSON != null)
			{
				this.autoPlayJSON.val = value;
			}
			else if (this._autoPlay != value)
			{
				this.SyncAutoPlay(value);
			}
		}
	}

	// Token: 0x060051B9 RID: 20921 RVA: 0x001D8093 File Offset: 0x001D6493
	protected void SyncLoop(bool b)
	{
		this._loop = b;
	}

	// Token: 0x17000BE0 RID: 3040
	// (get) Token: 0x060051BA RID: 20922 RVA: 0x001D809C File Offset: 0x001D649C
	// (set) Token: 0x060051BB RID: 20923 RVA: 0x001D80A4 File Offset: 0x001D64A4
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
				this._loop = value;
				if (this.loopToggle != null)
				{
					this.loopToggle.isOn = value;
				}
			}
		}
	}

	// Token: 0x060051BC RID: 20924 RVA: 0x001D80FD File Offset: 0x001D64FD
	protected void SyncAutoRecordStop(bool b)
	{
		this._autoRecordStop = b;
	}

	// Token: 0x17000BE1 RID: 3041
	// (get) Token: 0x060051BD RID: 20925 RVA: 0x001D8106 File Offset: 0x001D6506
	// (set) Token: 0x060051BE RID: 20926 RVA: 0x001D8110 File Offset: 0x001D6510
	public bool autoRecordStop
	{
		get
		{
			return this._autoRecordStop;
		}
		set
		{
			if (this.autoRecordStopJSON != null)
			{
				this.autoRecordStopJSON.val = value;
			}
			else if (this._autoRecordStop != value)
			{
				this._autoRecordStop = value;
				if (this.autoRecordStopToggle != null)
				{
					this.autoRecordStopToggle.isOn = value;
				}
			}
		}
	}

	// Token: 0x060051BF RID: 20927 RVA: 0x001D8169 File Offset: 0x001D6569
	protected void SyncPlaybackSpeed(float f)
	{
		this._playbackSpeed = f;
	}

	// Token: 0x17000BE2 RID: 3042
	// (get) Token: 0x060051C0 RID: 20928 RVA: 0x001D8172 File Offset: 0x001D6572
	// (set) Token: 0x060051C1 RID: 20929 RVA: 0x001D817C File Offset: 0x001D657C
	public float playbackSpeed
	{
		get
		{
			return this._playbackSpeed;
		}
		set
		{
			if (this.playbackSpeedJSON != null)
			{
				this.playbackSpeedJSON.val = value;
			}
			else if (this._playbackSpeed != value)
			{
				this.SyncPlaybackSpeed(value);
				if (this.playbackSpeedSlider != null)
				{
					this.playbackSpeedSlider.value = value;
				}
			}
		}
	}

	// Token: 0x060051C2 RID: 20930 RVA: 0x001D81D8 File Offset: 0x001D65D8
	protected void SyncShowRecordPaths(bool b)
	{
		this._showRecordPaths = b;
		if (this.controllers != null)
		{
			foreach (MotionAnimationControl motionAnimationControl in this.controllers)
			{
				motionAnimationControl.drawPathOpt = this._showRecordPaths;
			}
		}
	}

	// Token: 0x17000BE3 RID: 3043
	// (get) Token: 0x060051C3 RID: 20931 RVA: 0x001D824C File Offset: 0x001D664C
	// (set) Token: 0x060051C4 RID: 20932 RVA: 0x001D8254 File Offset: 0x001D6654
	public bool showRecordPaths
	{
		get
		{
			return this._showRecordPaths;
		}
		set
		{
			if (this.showRecordPathsJSON != null)
			{
				this.showRecordPathsJSON.val = value;
			}
			else if (this._showRecordPaths != value)
			{
				if (this.showRecordPathsToggle != null)
				{
					this.showRecordPathsToggle.isOn = value;
				}
				this.SyncShowRecordPaths(value);
			}
		}
	}

	// Token: 0x060051C5 RID: 20933 RVA: 0x001D82AD File Offset: 0x001D66AD
	protected void SyncShowStartMarkers(bool b)
	{
		this._showStartMarkers = b;
	}

	// Token: 0x17000BE4 RID: 3044
	// (get) Token: 0x060051C6 RID: 20934 RVA: 0x001D82B6 File Offset: 0x001D66B6
	// (set) Token: 0x060051C7 RID: 20935 RVA: 0x001D82C0 File Offset: 0x001D66C0
	public bool showStartMarkers
	{
		get
		{
			return this._showStartMarkers;
		}
		set
		{
			if (this.showStartMarkersJSON != null)
			{
				this.showStartMarkersJSON.val = value;
			}
			else if (this._showStartMarkers != value)
			{
				this._showStartMarkers = value;
				if (this.showStartMarkersToggle != null)
				{
					this.showStartMarkersToggle.isOn = value;
				}
			}
		}
	}

	// Token: 0x060051C8 RID: 20936 RVA: 0x001D8319 File Offset: 0x001D6719
	public IEnumerable<AnimationTimelineTrigger> GetTriggers()
	{
		return this.triggers;
	}

	// Token: 0x060051C9 RID: 20937 RVA: 0x001D8324 File Offset: 0x001D6724
	public void ClearTriggers()
	{
		List<Trigger> list = new List<Trigger>();
		foreach (Trigger item in this.triggers)
		{
			list.Add(item);
		}
		foreach (Trigger trigger in list)
		{
			trigger.Remove();
		}
	}

	// Token: 0x060051CA RID: 20938 RVA: 0x001D83CC File Offset: 0x001D67CC
	protected void CreateTriggerUI(AnimationTimelineTrigger att, int index)
	{
		if (this.triggerContentManager != null)
		{
			if (this.triggerPrefab != null)
			{
				RectTransform rectTransform = UnityEngine.Object.Instantiate<RectTransform>(this.triggerPrefab);
				this.triggerContentManager.AddItem(rectTransform, index, false);
				att.triggerPanel = rectTransform;
			}
			else
			{
				UnityEngine.Debug.LogError("Attempted to make TriggerUI when prefab was not set");
			}
		}
	}

	// Token: 0x060051CB RID: 20939 RVA: 0x001D842B File Offset: 0x001D682B
	protected void OpenAdvancedPanel()
	{
		if (this.advancedPanel != null)
		{
			this.advancedPanel.SetActive(true);
		}
		this.advancedPanelOpen = true;
	}

	// Token: 0x060051CC RID: 20940 RVA: 0x001D8451 File Offset: 0x001D6851
	protected void CloseAdvancedPanel()
	{
		if (this.advancedPanel != null)
		{
			this.advancedPanel.SetActive(false);
		}
		this.advancedPanelOpen = false;
	}

	// Token: 0x060051CD RID: 20941 RVA: 0x001D8477 File Offset: 0x001D6877
	protected void ReOpenAdvancedPanelIfWasOpen()
	{
		if (this.advancedPanel != null && this.advancedPanelOpen)
		{
			this.advancedPanel.SetActive(true);
		}
	}

	// Token: 0x060051CE RID: 20942 RVA: 0x001D84A1 File Offset: 0x001D68A1
	protected void TempCloseAdvancedPanelIfOpen()
	{
		if (this.advancedPanel != null && this.advancedPanelOpen)
		{
			this.advancedPanel.SetActive(false);
		}
	}

	// Token: 0x060051CF RID: 20943 RVA: 0x001D84CC File Offset: 0x001D68CC
	protected AnimationTimelineTrigger AddTriggerInternal(int index = -1)
	{
		AnimationTimelineTrigger animationTimelineTrigger = new AnimationTimelineTrigger();
		animationTimelineTrigger.timeLineHandler = this;
		animationTimelineTrigger.handler = this;
		animationTimelineTrigger.doActionsInReverse = false;
		AnimationTimelineTrigger animationTimelineTrigger2 = animationTimelineTrigger;
		animationTimelineTrigger2.onSelectedHandlers = (AnimationTimelineTrigger.OnSelected)Delegate.Combine(animationTimelineTrigger2.onSelectedHandlers, new AnimationTimelineTrigger.OnSelected(this.TriggerSelectionChange));
		AnimationTimelineTrigger animationTimelineTrigger3 = animationTimelineTrigger;
		animationTimelineTrigger3.onOpenTriggerActionsPanel = (Trigger.OnOpenTriggerActionsPanel)Delegate.Combine(animationTimelineTrigger3.onOpenTriggerActionsPanel, new Trigger.OnOpenTriggerActionsPanel(this.TempCloseAdvancedPanelIfOpen));
		AnimationTimelineTrigger animationTimelineTrigger4 = animationTimelineTrigger;
		animationTimelineTrigger4.onCloseTriggerActionsPanel = (Trigger.OnCloseTriggerActionsPanel)Delegate.Combine(animationTimelineTrigger4.onCloseTriggerActionsPanel, new Trigger.OnCloseTriggerActionsPanel(this.ReOpenAdvancedPanelIfWasOpen));
		if (index == -1)
		{
			this.triggers.Add(animationTimelineTrigger);
		}
		else
		{
			this.triggers.Insert(index, animationTimelineTrigger);
		}
		this.reverseTriggers = new List<AnimationTimelineTrigger>(this.triggers);
		this.reverseTriggers.Reverse();
		this.CreateTriggerUI(animationTimelineTrigger, index);
		animationTimelineTrigger.InitTriggerUI();
		animationTimelineTrigger.triggerActionsParent = this.triggerActionsParent;
		return animationTimelineTrigger;
	}

	// Token: 0x060051D0 RID: 20944 RVA: 0x001D85B6 File Offset: 0x001D69B6
	public void AddTrigger()
	{
		this.AddTriggerInternal(-1);
	}

	// Token: 0x060051D1 RID: 20945 RVA: 0x001D85C0 File Offset: 0x001D69C0
	public AnimationTimelineTrigger AddAndReturnTrigger()
	{
		return this.AddTriggerInternal(-1);
	}

	// Token: 0x060051D2 RID: 20946 RVA: 0x001D85CC File Offset: 0x001D69CC
	public override void RemoveTrigger(Trigger trigger)
	{
		AnimationTimelineTrigger animationTimelineTrigger = trigger as AnimationTimelineTrigger;
		if (this.triggers.Remove(animationTimelineTrigger))
		{
			AnimationTimelineTrigger animationTimelineTrigger2 = animationTimelineTrigger;
			animationTimelineTrigger2.onSelectedHandlers = (AnimationTimelineTrigger.OnSelected)Delegate.Remove(animationTimelineTrigger2.onSelectedHandlers, new AnimationTimelineTrigger.OnSelected(this.TriggerSelectionChange));
			AnimationTimelineTrigger animationTimelineTrigger3 = animationTimelineTrigger;
			animationTimelineTrigger3.onOpenTriggerActionsPanel = (Trigger.OnOpenTriggerActionsPanel)Delegate.Remove(animationTimelineTrigger3.onOpenTriggerActionsPanel, new Trigger.OnOpenTriggerActionsPanel(this.TempCloseAdvancedPanelIfOpen));
			AnimationTimelineTrigger animationTimelineTrigger4 = animationTimelineTrigger;
			animationTimelineTrigger4.onCloseTriggerActionsPanel = (Trigger.OnCloseTriggerActionsPanel)Delegate.Remove(animationTimelineTrigger4.onCloseTriggerActionsPanel, new Trigger.OnCloseTriggerActionsPanel(this.ReOpenAdvancedPanelIfWasOpen));
			this.reverseTriggers.Remove(animationTimelineTrigger);
			this._selectedTriggers.Remove(animationTimelineTrigger);
			if (trigger.triggerActionsPanel != null)
			{
				UnityEngine.Object.Destroy(trigger.triggerActionsPanel.gameObject);
			}
			if (trigger.triggerPanel != null)
			{
				if (this.triggerContentManager != null)
				{
					RectTransform component = trigger.triggerPanel.GetComponent<RectTransform>();
					if (component != null)
					{
						this.triggerContentManager.RemoveItem(component);
					}
				}
				UnityEngine.Object.Destroy(trigger.triggerPanel.gameObject);
			}
		}
		else
		{
			UnityEngine.Debug.Log("Could not remove trigger " + trigger.displayName);
		}
	}

	// Token: 0x060051D3 RID: 20947 RVA: 0x001D8704 File Offset: 0x001D6B04
	public override void DuplicateTrigger(Trigger trigger)
	{
		AnimationTimelineTrigger animationTimelineTrigger = trigger as AnimationTimelineTrigger;
		if (animationTimelineTrigger != null)
		{
			int num = this.triggers.IndexOf(animationTimelineTrigger);
			if (num != -1)
			{
				JSONClass json = animationTimelineTrigger.GetJSON();
				AnimationTimelineTrigger animationTimelineTrigger2 = this.AddTriggerInternal(num + 1);
				animationTimelineTrigger2.RestoreFromJSON(json);
			}
		}
	}

	// Token: 0x17000BE5 RID: 3045
	// (get) Token: 0x060051D4 RID: 20948 RVA: 0x001D874A File Offset: 0x001D6B4A
	public IEnumerable<AnimationTimelineTrigger> selectedTriggers
	{
		get
		{
			return this._selectedTriggers;
		}
	}

	// Token: 0x060051D5 RID: 20949 RVA: 0x001D8752 File Offset: 0x001D6B52
	public void TriggerSelectionChange(AnimationTimelineTrigger att)
	{
		if (att.selected)
		{
			this._selectedTriggers.Add(att);
		}
		else
		{
			this._selectedTriggers.Remove(att);
		}
	}

	// Token: 0x17000BE6 RID: 3046
	// (get) Token: 0x060051D6 RID: 20950 RVA: 0x001D877E File Offset: 0x001D6B7E
	// (set) Token: 0x060051D7 RID: 20951 RVA: 0x001D878B File Offset: 0x001D6B8B
	public float triggerSelectFromTime
	{
		get
		{
			return this.triggerSelectFromTimeJSON.val;
		}
		set
		{
			this.triggerSelectFromTimeJSON.val = value;
		}
	}

	// Token: 0x17000BE7 RID: 3047
	// (get) Token: 0x060051D8 RID: 20952 RVA: 0x001D8799 File Offset: 0x001D6B99
	// (set) Token: 0x060051D9 RID: 20953 RVA: 0x001D87A6 File Offset: 0x001D6BA6
	public float triggerSelectToTime
	{
		get
		{
			return this.triggerSelectToTimeJSON.val;
		}
		set
		{
			this.triggerSelectToTimeJSON.val = value;
		}
	}

	// Token: 0x060051DA RID: 20954 RVA: 0x001D87B4 File Offset: 0x001D6BB4
	protected void SelectTriggersInTimeRange()
	{
		foreach (AnimationTimelineTrigger animationTimelineTrigger in this.triggers)
		{
			if (animationTimelineTrigger.triggerStartTime >= this.triggerSelectFromTimeJSON.val && animationTimelineTrigger.triggerStartTime <= this.triggerSelectToTimeJSON.val)
			{
				animationTimelineTrigger.selected = true;
			}
			else
			{
				animationTimelineTrigger.selected = false;
			}
		}
	}

	// Token: 0x060051DB RID: 20955 RVA: 0x001D8848 File Offset: 0x001D6C48
	public void ClearSelectedTriggers()
	{
		foreach (AnimationTimelineTrigger animationTimelineTrigger in this.triggers)
		{
			animationTimelineTrigger.selected = false;
		}
	}

	// Token: 0x060051DC RID: 20956 RVA: 0x001D88A4 File Offset: 0x001D6CA4
	public void AdjustTimeOfSelectedTriggers()
	{
		float val = this.triggerTimeAdjustmentJSON.val;
		foreach (AnimationTimelineTrigger animationTimelineTrigger in this._selectedTriggers)
		{
			if (val > 0f)
			{
				animationTimelineTrigger.triggerEndTime += val;
				animationTimelineTrigger.triggerStartTime += val;
			}
			else
			{
				animationTimelineTrigger.triggerStartTime += val;
				animationTimelineTrigger.triggerEndTime += val;
			}
			if (animationTimelineTrigger.triggerEndTime > this._recordedLength)
			{
				this.recordedLength = animationTimelineTrigger.triggerEndTime;
			}
		}
		this.SortTriggersByStartTime();
	}

	// Token: 0x060051DD RID: 20957 RVA: 0x001D8970 File Offset: 0x001D6D70
	public void SortTriggersByStartTime()
	{
		List<AnimationTimelineTrigger> list = this.triggers;
		if (MotionAnimationMaster.<>f__am$cache1 == null)
		{
			MotionAnimationMaster.<>f__am$cache1 = new Comparison<AnimationTimelineTrigger>(MotionAnimationMaster.<SortTriggersByStartTime>m__1);
		}
		list.Sort(MotionAnimationMaster.<>f__am$cache1);
		this.reverseTriggers = new List<AnimationTimelineTrigger>(this.triggers);
		this.reverseTriggers.Reverse();
		if (this.triggerContentManager != null)
		{
			this.triggerContentManager.RemoveAllItems();
			int num = 0;
			foreach (AnimationTimelineTrigger animationTimelineTrigger in this.triggers)
			{
				if (animationTimelineTrigger.triggerPanel != null)
				{
					RectTransform component = animationTimelineTrigger.triggerPanel.GetComponent<RectTransform>();
					if (component != null)
					{
						this.triggerContentManager.AddItem(component, num, true);
					}
				}
				num++;
			}
			this.triggerContentManager.RelayoutPanel();
		}
	}

	// Token: 0x17000BE8 RID: 3048
	// (get) Token: 0x060051DE RID: 20958 RVA: 0x001D8A70 File Offset: 0x001D6E70
	// (set) Token: 0x060051DF RID: 20959 RVA: 0x001D8A7D File Offset: 0x001D6E7D
	public float triggerPasteToTime
	{
		get
		{
			return this.triggerPasteToTimeJSON.val;
		}
		set
		{
			this.triggerPasteToTimeJSON.val = value;
		}
	}

	// Token: 0x060051E0 RID: 20960 RVA: 0x001D8A8C File Offset: 0x001D6E8C
	public void CopySelectedTriggersAndPasteToTime()
	{
		AnimationTimelineTrigger animationTimelineTrigger = null;
		float num = float.MaxValue;
		foreach (AnimationTimelineTrigger animationTimelineTrigger2 in this._selectedTriggers)
		{
			if (animationTimelineTrigger2.triggerStartTime < num)
			{
				num = animationTimelineTrigger2.triggerStartTime;
				animationTimelineTrigger = animationTimelineTrigger2;
			}
		}
		if (animationTimelineTrigger != null)
		{
			float num2 = this.triggerPasteToTimeJSON.val - animationTimelineTrigger.triggerStartTime;
			foreach (AnimationTimelineTrigger animationTimelineTrigger3 in this.selectedTriggers)
			{
				JSONClass json = animationTimelineTrigger3.GetJSON();
				AnimationTimelineTrigger animationTimelineTrigger4 = this.AddTriggerInternal(-1);
				animationTimelineTrigger4.RestoreFromJSON(json);
				if (num2 > 0f)
				{
					animationTimelineTrigger4.triggerEndTime += num2;
					animationTimelineTrigger4.triggerStartTime += num2;
				}
				else
				{
					animationTimelineTrigger4.triggerStartTime += num2;
					animationTimelineTrigger4.triggerEndTime += num2;
				}
				if (animationTimelineTrigger4.triggerEndTime > this._recordedLength)
				{
					this.recordedLength = animationTimelineTrigger4.triggerEndTime;
				}
			}
			this.SortTriggersByStartTime();
		}
	}

	// Token: 0x17000BE9 RID: 3049
	// (get) Token: 0x060051E1 RID: 20961 RVA: 0x001D8BF0 File Offset: 0x001D6FF0
	public bool isRecording
	{
		get
		{
			return this._isRecording;
		}
	}

	// Token: 0x060051E2 RID: 20962 RVA: 0x001D8BF8 File Offset: 0x001D6FF8
	protected void SyncActive()
	{
		if (this.activeWhilePlaying != null)
		{
			this.activeWhilePlaying.SetActive(this._isPlaying);
		}
		if (this.activeWhileStopped != null)
		{
			this.activeWhileStopped.SetActive(!this._isPlaying);
		}
	}

	// Token: 0x17000BEA RID: 3050
	// (get) Token: 0x060051E3 RID: 20963 RVA: 0x001D8C4C File Offset: 0x001D704C
	// (set) Token: 0x060051E4 RID: 20964 RVA: 0x001D8C54 File Offset: 0x001D7054
	protected float recordedLength
	{
		get
		{
			return this._recordedLength;
		}
		set
		{
			this._recordedLength = value;
			if (this._recordedLength < 0f)
			{
				this._recordedLength = 0f;
			}
			this.playbackCounterJSON.max = this._recordedLength;
			this.startTimestepJSON.max = this._recordedLength;
			this.stopTimestepJSON.max = this._recordedLength;
			this.stopTimestepJSON.val = this._recordedLength;
			this.triggerSelectFromTimeJSON.max = this._recordedLength;
			this.triggerSelectToTimeJSON.max = this._recordedLength;
			this.triggerPasteToTimeJSON.max = this._recordedLength;
			foreach (AnimationTimelineTrigger animationTimelineTrigger in this.triggers)
			{
				animationTimelineTrigger.ResyncMaxStartAndEndTimes();
			}
		}
	}

	// Token: 0x17000BEB RID: 3051
	// (get) Token: 0x060051E5 RID: 20965 RVA: 0x001D8D48 File Offset: 0x001D7148
	public float totalTime
	{
		get
		{
			return this._recordedLength;
		}
	}

	// Token: 0x060051E6 RID: 20966 RVA: 0x001D8D50 File Offset: 0x001D7150
	public void ClearAllAnimation()
	{
		this.StopPlayback();
		foreach (MotionAnimationControl motionAnimationControl in this.controllers)
		{
			motionAnimationControl.ClearAnimation();
		}
		this.recordedLength = 0f;
		this._playbackCounter = 0f;
	}

	// Token: 0x060051E7 RID: 20967 RVA: 0x001D8DC8 File Offset: 0x001D71C8
	public void SelectControllersArmedForRecord()
	{
		if (SuperController.singleton != null)
		{
			SuperController.singleton.SelectModeArmedForRecord(this.controllers);
		}
	}

	// Token: 0x060051E8 RID: 20968 RVA: 0x001D8DEA File Offset: 0x001D71EA
	public void ArmAllControlledControllersForRecord()
	{
		if (SuperController.singleton != null)
		{
			SuperController.singleton.ArmAllControlledControllersForRecord(this.controllers);
		}
	}

	// Token: 0x060051E9 RID: 20969 RVA: 0x001D8E0C File Offset: 0x001D720C
	public void StartRecordMode()
	{
		if (SuperController.singleton != null)
		{
			SuperController.singleton.SelectModeAnimationRecord(this);
		}
	}

	// Token: 0x060051EA RID: 20970 RVA: 0x001D8E2C File Offset: 0x001D722C
	public void StartRecord()
	{
		this.SyncShowRecordPaths(this._showRecordPaths);
		this._isRecording = true;
		this._isPlaying = true;
		this.SyncActive();
		this._lastRecordTime = -1f;
		if (this._recordedLength == 0f)
		{
			this._ignoreAutoRecordStop = true;
		}
		else
		{
			this._ignoreAutoRecordStop = false;
		}
		TimeControl.singleton.currentScale = 1f;
		if (SuperController.singleton != null)
		{
			SuperController.singleton.SetFreezeAnimation(false);
		}
		int recordCounter = Mathf.FloorToInt(this._playbackCounter);
		foreach (MotionAnimationControl motionAnimationControl in this.controllers)
		{
			motionAnimationControl.PrepareRecord(recordCounter);
		}
		if (this._showStartMarkers)
		{
			foreach (MotionAnimationControl motionAnimationControl2 in this.controllers)
			{
				if (motionAnimationControl2.armedForRecord && motionAnimationControl2.controller != null)
				{
					motionAnimationControl2.controller.TakeSnapshot();
					motionAnimationControl2.controller.drawSnapshot = true;
				}
			}
		}
	}

	// Token: 0x060051EB RID: 20971 RVA: 0x001D8F90 File Offset: 0x001D7390
	public void StopRecord()
	{
		if (this._isRecording)
		{
			this.RecordStep(true);
			this._isRecording = false;
			foreach (MotionAnimationControl motionAnimationControl in this.controllers)
			{
				if (motionAnimationControl.controller != null)
				{
					motionAnimationControl.controller.drawSnapshot = false;
				}
				motionAnimationControl.FinalizeRecord();
				motionAnimationControl.armedForRecord = false;
			}
			if (this._autoPlay)
			{
				this.StopLoopback();
				this.StartPlayback();
			}
			else
			{
				this._isPlaying = false;
				this.SyncActive();
			}
		}
	}

	// Token: 0x060051EC RID: 20972 RVA: 0x001D9054 File Offset: 0x001D7454
	public void StopRecordMode()
	{
		if (SuperController.singleton != null)
		{
			SuperController.singleton.StopRecording();
		}
		this.StopRecord();
	}

	// Token: 0x060051ED RID: 20973 RVA: 0x001D9076 File Offset: 0x001D7476
	protected void SetIsPlayingFromAudioSource()
	{
		this._isPlaying = true;
		this.SyncActive();
	}

	// Token: 0x060051EE RID: 20974 RVA: 0x001D9085 File Offset: 0x001D7485
	public void StartPlayback()
	{
		if (this._linkToAudioSourceControl && this.audioSourceControl != null)
		{
			this.audioSourceControl.UnPause();
		}
		this._isPlaying = true;
		this.SyncActive();
	}

	// Token: 0x060051EF RID: 20975 RVA: 0x001D90BC File Offset: 0x001D74BC
	public void StopPlayback()
	{
		if (this._linkToAudioSourceControl && this.audioSourceControl != null)
		{
			this.audioSourceControl.Pause();
		}
		if (this._isLoopingBack)
		{
			this.StopLoopback();
		}
		if (this._isRecording)
		{
			this.StopRecordMode();
		}
		this._isPlaying = false;
		this.SyncActive();
	}

	// Token: 0x060051F0 RID: 20976 RVA: 0x001D9120 File Offset: 0x001D7520
	public void TrimAnimation()
	{
		float val = this.startTimestepJSON.val;
		float val2 = this.stopTimestepJSON.val;
		foreach (MotionAnimationControl motionAnimationControl in this.controllers)
		{
			motionAnimationControl.TrimClip(val, val2);
		}
		this.recordedLength = val2 - val;
		this.startTimestepJSON.val = 0f;
	}

	// Token: 0x060051F1 RID: 20977 RVA: 0x001D91B0 File Offset: 0x001D75B0
	protected void SetToDesiredLength()
	{
		this.recordedLength = this._desiredLength;
		float val = this.startTimestepJSON.val;
		float val2 = this.stopTimestepJSON.val;
		foreach (MotionAnimationControl motionAnimationControl in this.controllers)
		{
			motionAnimationControl.TrimClip(val, val2);
		}
	}

	// Token: 0x060051F2 RID: 20978 RVA: 0x001D9234 File Offset: 0x001D7634
	protected void SyncDesiredLength(float f)
	{
		this._desiredLength = f;
	}

	// Token: 0x060051F3 RID: 20979 RVA: 0x001D9240 File Offset: 0x001D7640
	public void CopyFromSceneMaster()
	{
		if (!this.isSceneMasterController)
		{
			MotionAnimationMaster motionAnimationMaster = SuperController.singleton.motionAnimationMaster;
			JSONClass json = motionAnimationMaster.GetJSON(true, true, true);
			this.RestoreFromJSON(json, true, true, null, true);
			this.LateRestoreFromJSON(json, true, true, true);
		}
	}

	// Token: 0x060051F4 RID: 20980 RVA: 0x001D9284 File Offset: 0x001D7684
	public void CopyToSceneMaster()
	{
		if (!this.isSceneMasterController)
		{
			MotionAnimationMaster motionAnimationMaster = SuperController.singleton.motionAnimationMaster;
			JSONClass json = this.GetJSON(true, true, true);
			motionAnimationMaster.RestoreFromJSON(json, true, true, null, true);
			motionAnimationMaster.LateRestoreFromJSON(json, true, true, true);
		}
	}

	// Token: 0x060051F5 RID: 20981 RVA: 0x001D92C8 File Offset: 0x001D76C8
	protected void StopLoopback()
	{
		if (this._linkToAudioSourceControl && this.audioSourceControl != null && this.audioSourceControl.audioSource.clip != null)
		{
			this.SetAudioSourceTime(this.startTimestepJSON.val);
		}
		this.InternalSetPlaybackCounter(this.startTimestepJSON.val, false, false);
	}

	// Token: 0x060051F6 RID: 20982 RVA: 0x001D9330 File Offset: 0x001D7730
	protected void RecordStep(bool forceRecord = false)
	{
		if (this._playbackCounter - this._lastRecordTime > this.recordInterval)
		{
			this._lastRecordTime = this._playbackCounter;
			foreach (MotionAnimationControl motionAnimationControl in this.controllers)
			{
				motionAnimationControl.RecordStep(this._playbackCounter, forceRecord);
			}
		}
	}

	// Token: 0x060051F7 RID: 20983 RVA: 0x001D93B8 File Offset: 0x001D77B8
	protected IEnumerator SeekToBeginningCo()
	{
		for (int i = 0; i < 10; i++)
		{
			this.PlaybackStepForceAlign();
			yield return null;
		}
		yield break;
	}

	// Token: 0x060051F8 RID: 20984 RVA: 0x001D93D4 File Offset: 0x001D77D4
	public void SeekToBeginning()
	{
		if (SuperController.singleton != null)
		{
			SuperController.singleton.ResetSimulation(15, "MotionAnimation SeekToBeginning", true);
		}
		if (this._linkToAudioSourceControl && this.audioSourceControl != null && this.audioSourceControl.audioSource.clip != null)
		{
			this.SetAudioSourceTime(this.startTimestepJSON.val);
		}
		this.InternalSetPlaybackCounter(this.startTimestepJSON.val, true, true);
		base.StartCoroutine(this.SeekToBeginningCo());
	}

	// Token: 0x060051F9 RID: 20985 RVA: 0x001D946C File Offset: 0x001D786C
	protected void PlaybackStep()
	{
		foreach (MotionAnimationControl motionAnimationControl in this.controllers)
		{
			if (!this._isRecording || !motionAnimationControl.armedForRecord)
			{
				motionAnimationControl.PlaybackStep(this._playbackCounter);
			}
		}
	}

	// Token: 0x060051FA RID: 20986 RVA: 0x001D94E4 File Offset: 0x001D78E4
	protected void PlaybackStepForceAlign()
	{
		foreach (MotionAnimationControl motionAnimationControl in this.controllers)
		{
			if (!this._isRecording || !motionAnimationControl.armedForRecord)
			{
				motionAnimationControl.PlaybackStepForceAlign(this._playbackCounter);
			}
		}
	}

	// Token: 0x060051FB RID: 20987 RVA: 0x001D955C File Offset: 0x001D795C
	protected void LoopbackStep()
	{
		float val = this.startTimestepJSON.val;
		foreach (MotionAnimationControl motionAnimationControl in this.controllers)
		{
			motionAnimationControl.LoopbackStep(this._loopbackCounter / this._loopbackTime, val);
		}
	}

	// Token: 0x060051FC RID: 20988 RVA: 0x001D95D4 File Offset: 0x001D79D4
	protected void OnAtomRename(string fromuid, string touid)
	{
		foreach (AnimationTimelineTrigger animationTimelineTrigger in this.triggers)
		{
			animationTimelineTrigger.SyncAtomNames();
		}
		if (this.audioSourceControl != null && this.audioSourceControlAtomSelectionPopup != null)
		{
			this.audioSourceControlAtomSelectionPopup.currentValueNoCallback = this.audioSourceControl.containingAtom.uid;
		}
	}

	// Token: 0x060051FD RID: 20989 RVA: 0x001D966C File Offset: 0x001D7A6C
	protected void Init()
	{
		this.linkToAudioSourceControlJSON = new JSONStorableBool("linkToAudioSourceControl", this._linkToAudioSourceControl, new JSONStorableBool.SetBoolCallback(this.SyncLinkToAudioSourceControl));
		this.linkToAudioSourceControlJSON.storeType = JSONStorableParam.StoreType.Physical;
		base.RegisterBool(this.linkToAudioSourceControlJSON);
		this.audioSourceTimeOffsetJSON = new JSONStorableFloat("audioSourceTimeOffset", this._audioSourceTimeOffset, new JSONStorableFloat.SetFloatCallback(this.SyncAudioSourceTimeOffset), -10f, 10f, false, true);
		this.audioSourceTimeOffsetJSON.storeType = JSONStorableParam.StoreType.Physical;
		base.RegisterFloat(this.audioSourceTimeOffsetJSON);
		this.playbackCounterJSON = new JSONStorableFloat("playbackCounter", this._playbackCounter, new JSONStorableFloat.SetFloatCallback(this.SyncPlaybackCounter), 0f, 0f, true, true);
		this.playbackCounterJSON.storeType = JSONStorableParam.StoreType.Physical;
		base.RegisterFloat(this.playbackCounterJSON);
		this.startTimestepJSON = new JSONStorableFloat("startTimestep", this._startTimestep, new JSONStorableFloat.SetFloatCallback(this.SyncStartTimestep), 0f, 0f, true, true);
		this.startTimestepJSON.storeType = JSONStorableParam.StoreType.Physical;
		base.RegisterFloat(this.startTimestepJSON);
		this.stopTimestepJSON = new JSONStorableFloat("stopTimestep", this._stopTimestep, new JSONStorableFloat.SetFloatCallback(this.SyncStopTimestep), 0f, 0f, true, true);
		this.stopTimestepJSON.storeType = JSONStorableParam.StoreType.Physical;
		base.RegisterFloat(this.stopTimestepJSON);
		this.loopbackTimeJSON = new JSONStorableFloat("loopbackTime", this._loopbackTime, new JSONStorableFloat.SetFloatCallback(this.SyncLoopbackTime), 0f, 10f, true, true);
		this.loopbackTimeJSON.storeType = JSONStorableParam.StoreType.Physical;
		base.RegisterFloat(this.loopbackTimeJSON);
		this.autoPlayJSON = new JSONStorableBool("autoPlay", this._autoPlay, new JSONStorableBool.SetBoolCallback(this.SyncAutoPlay));
		this.autoPlayJSON.storeType = JSONStorableParam.StoreType.Physical;
		base.RegisterBool(this.autoPlayJSON);
		this.loopJSON = new JSONStorableBool("loop", this._loop, new JSONStorableBool.SetBoolCallback(this.SyncLoop));
		this.loopJSON.storeType = JSONStorableParam.StoreType.Physical;
		base.RegisterBool(this.loopJSON);
		this.playbackSpeedJSON = new JSONStorableFloat("playbackSpeed", this._playbackSpeed, new JSONStorableFloat.SetFloatCallback(this.SyncPlaybackSpeed), 0f, 10f, true, true);
		this.playbackSpeedJSON.storeType = JSONStorableParam.StoreType.Physical;
		base.RegisterFloat(this.playbackSpeedJSON);
		this.desiredLengthJSON = new JSONStorableFloat("desiredLength", this._desiredLength, new JSONStorableFloat.SetFloatCallback(this.SyncDesiredLength), 0f, 100f, false, true);
		this.desiredLengthJSON.storeType = JSONStorableParam.StoreType.Physical;
		base.RegisterFloat(this.desiredLengthJSON);
		this.StartPlaybackAction = new JSONStorableAction("StartPlayback", new JSONStorableAction.ActionCallback(this.StartPlayback));
		base.RegisterAction(this.StartPlaybackAction);
		this.StopPlaybackAction = new JSONStorableAction("StopPlayback", new JSONStorableAction.ActionCallback(this.StopPlayback));
		base.RegisterAction(this.StopPlaybackAction);
		this.SelectControllersArmedForRecordAction = new JSONStorableAction("SelectControllersArmedForRecord", new JSONStorableAction.ActionCallback(this.SelectControllersArmedForRecord));
		base.RegisterAction(this.SelectControllersArmedForRecordAction);
		this.ArmAllControlledControllersForRecordAction = new JSONStorableAction("ArmAllControlledControllesForRecord", new JSONStorableAction.ActionCallback(this.ArmAllControlledControllersForRecord));
		base.RegisterAction(this.ArmAllControlledControllersForRecordAction);
		this.StartRecordModeAction = new JSONStorableAction("StartRecordMode", new JSONStorableAction.ActionCallback(this.StartRecordMode));
		base.RegisterAction(this.StartRecordModeAction);
		this.StartRecordAction = new JSONStorableAction("StartRecord", new JSONStorableAction.ActionCallback(this.StartRecord));
		base.RegisterAction(this.StartRecordAction);
		this.StopRecordAction = new JSONStorableAction("StopRecord", new JSONStorableAction.ActionCallback(this.StopRecord));
		base.RegisterAction(this.StopRecordAction);
		this.StopRecordModeAction = new JSONStorableAction("StopRecordMode", new JSONStorableAction.ActionCallback(this.StopRecordMode));
		base.RegisterAction(this.StopRecordModeAction);
		this.ClearAllAnimationAction = new JSONStorableAction("ClearAllAnimation", new JSONStorableAction.ActionCallback(this.ClearAllAnimation));
		base.RegisterAction(this.ClearAllAnimationAction);
		this.TrimAnimationAction = new JSONStorableAction("TrimAnimation", new JSONStorableAction.ActionCallback(this.TrimAnimation));
		base.RegisterAction(this.TrimAnimationAction);
		this.SetToDesiredLengthAction = new JSONStorableAction("SetToDesiredLength", new JSONStorableAction.ActionCallback(this.SetToDesiredLength));
		base.RegisterAction(this.SetToDesiredLengthAction);
		this.SeekToBeginningAction = new JSONStorableAction("SeekToBeginning", new JSONStorableAction.ActionCallback(this.SeekToBeginning));
		base.RegisterAction(this.SeekToBeginningAction);
		this.ResetAnimationAction = new JSONStorableAction("ResetAnimation", new JSONStorableAction.ActionCallback(this.ResetAnimation));
		base.RegisterAction(this.ResetAnimationAction);
		if (!this.isSceneMasterController)
		{
			this.CopyFromSceneMasterAction = new JSONStorableAction("CopyFromSceneMaster", new JSONStorableAction.ActionCallback(this.CopyFromSceneMaster));
			base.RegisterAction(this.CopyFromSceneMasterAction);
			this.CopyToSceneMasterAction = new JSONStorableAction("CopyToSceneMaster", new JSONStorableAction.ActionCallback(this.CopyToSceneMaster));
			base.RegisterAction(this.CopyToSceneMasterAction);
		}
		this.SelectTriggersInTimeRangeAction = new JSONStorableAction("SelectTriggersInTimeRange", new JSONStorableAction.ActionCallback(this.SelectTriggersInTimeRange));
		base.RegisterAction(this.SelectTriggersInTimeRangeAction);
		this.ClearSelectedTriggersAction = new JSONStorableAction("ClearSelectedTriggers", new JSONStorableAction.ActionCallback(this.ClearSelectedTriggers));
		base.RegisterAction(this.ClearSelectedTriggersAction);
		this.AdjustTimeOfSelectedTriggersAction = new JSONStorableAction("AdjustTimeOfSelectedTriggers", new JSONStorableAction.ActionCallback(this.AdjustTimeOfSelectedTriggers));
		base.RegisterAction(this.AdjustTimeOfSelectedTriggersAction);
		this.SortTriggersByStartTimeAction = new JSONStorableAction("SortTriggersByStartTime", new JSONStorableAction.ActionCallback(this.SortTriggersByStartTime));
		base.RegisterAction(this.SortTriggersByStartTimeAction);
		this.CopySelectedTriggersAndPasteToTimeAction = new JSONStorableAction("CopySelectedTriggersAndPasteToTime", new JSONStorableAction.ActionCallback(this.CopySelectedTriggersAndPasteToTime));
		base.RegisterAction(this.CopySelectedTriggersAndPasteToTimeAction);
		this.triggerSelectFromTimeJSON = new JSONStorableFloat("triggerSelectFromTime", 0f, 0f, 0f, true, true);
		this.triggerSelectFromTimeJSON.isStorable = false;
		this.triggerSelectFromTimeJSON.isRestorable = false;
		this.triggerSelectToTimeJSON = new JSONStorableFloat("triggerSelectToTime", 0f, 0f, 0f, true, true);
		this.triggerSelectToTimeJSON.isStorable = false;
		this.triggerSelectToTimeJSON.isRestorable = false;
		this.triggerTimeAdjustmentJSON = new JSONStorableFloat("triggerTimeAdjustment", 0f, -100f, 100f, false, true);
		this.triggerTimeAdjustmentJSON.isStorable = false;
		this.triggerTimeAdjustmentJSON.isRestorable = false;
		this.triggerPasteToTimeJSON = new JSONStorableFloat("triggerPasteToTime", 0f, 0f, 0f, true, true);
		this.triggerPasteToTimeJSON.isStorable = false;
		this.triggerPasteToTimeJSON.isRestorable = false;
		this.autoRecordStopJSON = new JSONStorableBool("autoRecordStop", this._autoRecordStop, new JSONStorableBool.SetBoolCallback(this.SyncAutoRecordStop));
		this.autoRecordStopJSON.isStorable = false;
		this.autoRecordStopJSON.isRestorable = false;
		base.RegisterBool(this.autoRecordStopJSON);
		this.showRecordPathsJSON = new JSONStorableBool("showRecordPath", this._showRecordPaths, new JSONStorableBool.SetBoolCallback(this.SyncShowRecordPaths));
		this.showRecordPathsJSON.isStorable = false;
		this.showRecordPathsJSON.isRestorable = false;
		base.RegisterBool(this.showRecordPathsJSON);
		this.showStartMarkersJSON = new JSONStorableBool("showStartMarkers", this._showStartMarkers, new JSONStorableBool.SetBoolCallback(this.SyncShowStartMarkers));
		this.showStartMarkersJSON.isStorable = false;
		this.showStartMarkersJSON.isRestorable = false;
		base.RegisterBool(this.showStartMarkersJSON);
		this.SyncShowRecordPaths(this._showRecordPaths);
		this.controllers = new HashSet<MotionAnimationControl>();
		this.triggers = new List<AnimationTimelineTrigger>();
		this.reverseTriggers = new List<AnimationTimelineTrigger>();
		this._selectedTriggers = new HashSet<AnimationTimelineTrigger>();
		if (SuperController.singleton != null)
		{
			SuperController singleton = SuperController.singleton;
			singleton.onAtomUIDRenameHandlers = (SuperController.OnAtomUIDRename)Delegate.Combine(singleton.onAtomUIDRenameHandlers, new SuperController.OnAtomUIDRename(this.OnAtomRename));
		}
	}

	// Token: 0x060051FE RID: 20990 RVA: 0x001D9E60 File Offset: 0x001D8260
	protected override void InitUI(Transform t, bool isAlt)
	{
		if (t != null)
		{
			MotionAnimationMasterUI componentInChildren = t.GetComponentInChildren<MotionAnimationMasterUI>(true);
			if (componentInChildren != null)
			{
				this.linkToAudioSourceControlJSON.RegisterToggle(componentInChildren.linkToAudioSourceControlToggle, isAlt);
				this.audioSourceTimeOffsetJSON.RegisterSlider(componentInChildren.audioSourceTimeOffsetSlider, isAlt);
				this.playbackCounterJSON.RegisterSlider(componentInChildren.playbackCounterSlider, isAlt);
				this.startTimestepJSON.RegisterSlider(componentInChildren.startTimestepSlider, isAlt);
				this.stopTimestepJSON.RegisterSlider(componentInChildren.stopTimestepSlider, isAlt);
				this.loopbackTimeJSON.RegisterSlider(componentInChildren.loopbackTimeSlider, isAlt);
				this.playbackSpeedJSON.RegisterSlider(componentInChildren.playbackSpeedSlider, isAlt);
				this.autoPlayJSON.RegisterToggle(componentInChildren.autoPlayToggle, isAlt);
				this.loopJSON.RegisterToggle(componentInChildren.loopToggle, isAlt);
				this.autoRecordStopJSON.RegisterToggle(componentInChildren.autoRecordStopToggle, isAlt);
				this.showRecordPathsJSON.RegisterToggle(componentInChildren.showRecordPathsToggle, isAlt);
				this.showStartMarkersJSON.RegisterToggle(componentInChildren.showStartMarkersToggle, isAlt);
				this.ClearAllAnimationAction.RegisterButton(componentInChildren.clearAllAnimationButton, isAlt);
				this.SelectControllersArmedForRecordAction.RegisterButton(componentInChildren.selectControllersArmedForRecordButton, isAlt);
				this.ArmAllControlledControllersForRecordAction.RegisterButton(componentInChildren.armAllControlledControllersForRecordButton, isAlt);
				this.StartRecordModeAction.RegisterButton(componentInChildren.startRecordModeButton, isAlt);
				this.StartRecordAction.RegisterButton(componentInChildren.startRecordButton, isAlt);
				this.StopRecordAction.RegisterButton(componentInChildren.stopRecordButton, isAlt);
				this.StopRecordModeAction.RegisterButton(componentInChildren.stopRecordModeButton, isAlt);
				this.StartPlaybackAction.RegisterButton(componentInChildren.startPlaybackButton, isAlt);
				this.StopPlaybackAction.RegisterButton(componentInChildren.stopPlaybackButton, isAlt);
				this.TrimAnimationAction.RegisterButton(componentInChildren.trimAnimationButton, isAlt);
				this.desiredLengthJSON.RegisterSlider(componentInChildren.desiredLengthSlider, isAlt);
				this.SetToDesiredLengthAction.RegisterButton(componentInChildren.setToDesiredLengthButton, isAlt);
				this.SeekToBeginningAction.RegisterButton(componentInChildren.seekToBeginningButton, isAlt);
				this.ResetAnimationAction.RegisterButton(componentInChildren.resetAnimationButton, isAlt);
				this.SelectTriggersInTimeRangeAction.RegisterButton(componentInChildren.selectTriggersInTimeRangeButton, isAlt);
				this.ClearSelectedTriggersAction.RegisterButton(componentInChildren.clearSelectedTriggersButton, isAlt);
				this.AdjustTimeOfSelectedTriggersAction.RegisterButton(componentInChildren.adjustTimeOfSelectedTriggersButton, isAlt);
				this.SortTriggersByStartTimeAction.RegisterButton(componentInChildren.sortTriggersByStartTimeButton, isAlt);
				this.CopySelectedTriggersAndPasteToTimeAction.RegisterButton(componentInChildren.copySelectedTriggersAndPasteToTimeButton, isAlt);
				this.triggerSelectFromTimeJSON.RegisterSlider(componentInChildren.triggerSelectFromTimeSlider, isAlt);
				this.triggerSelectToTimeJSON.RegisterSlider(componentInChildren.triggerSelectToTimeSlider, isAlt);
				this.triggerTimeAdjustmentJSON.RegisterSlider(componentInChildren.triggerTimeAdjustmentSlider, isAlt);
				this.triggerPasteToTimeJSON.RegisterSlider(componentInChildren.triggerPasteToTimeSlider, isAlt);
				if (!this.isSceneMasterController)
				{
					this.CopyFromSceneMasterAction.RegisterButton(componentInChildren.copyFromSceneMasterButton, isAlt);
					this.CopyToSceneMasterAction.RegisterButton(componentInChildren.copyToSceneMasterButton, isAlt);
				}
				else
				{
					if (componentInChildren.copyFromSceneMasterButton != null)
					{
						componentInChildren.copyFromSceneMasterButton.gameObject.SetActive(false);
					}
					if (componentInChildren.copyToSceneMasterButton != null)
					{
						componentInChildren.copyToSceneMasterButton.gameObject.SetActive(false);
					}
				}
				if (!isAlt)
				{
					this.advancedPanel = componentInChildren.advancedPanel;
					if (componentInChildren.openAdvancedPanelButton != null)
					{
						componentInChildren.openAdvancedPanelButton.onClick.AddListener(new UnityAction(this.OpenAdvancedPanel));
					}
					if (componentInChildren.closeAdvancedPanelButton != null)
					{
						componentInChildren.closeAdvancedPanelButton.onClick.AddListener(new UnityAction(this.CloseAdvancedPanel));
					}
					this.audioSourceControlAtomSelectionPopup = componentInChildren.audioSourceControlAtomSelectionPopup;
					if (this.audioSourceControlAtomSelectionPopup != null)
					{
						UIPopup uipopup = this.audioSourceControlAtomSelectionPopup;
						uipopup.onOpenPopupHandlers = (UIPopup.OnOpenPopup)Delegate.Combine(uipopup.onOpenPopupHandlers, new UIPopup.OnOpenPopup(this.SetAudioSourceControlAtomNames));
						UIPopup uipopup2 = this.audioSourceControlAtomSelectionPopup;
						uipopup2.onValueChangeHandlers = (UIPopup.OnValueChange)Delegate.Combine(uipopup2.onValueChangeHandlers, new UIPopup.OnValueChange(this.SetAudioSourceControlAtom));
					}
					this.audioSourceControlSelectionPopup = componentInChildren.audioSourceControlSelectionPopup;
					if (this.audioSourceControlSelectionPopup != null)
					{
						UIPopup uipopup3 = this.audioSourceControlSelectionPopup;
						uipopup3.onValueChangeHandlers = (UIPopup.OnValueChange)Delegate.Combine(uipopup3.onValueChangeHandlers, new UIPopup.OnValueChange(this.SetAudioSourceControlObject));
					}
					this.triggerContentManager = componentInChildren.triggerContentManager;
					this.triggerActionsParent = componentInChildren.triggerActionsParent;
					for (int i = 0; i < this.triggers.Count; i++)
					{
						AnimationTimelineTrigger animationTimelineTrigger = this.triggers[i];
						this.CreateTriggerUI(animationTimelineTrigger, i);
						animationTimelineTrigger.InitTriggerUI();
						animationTimelineTrigger.triggerActionsParent = this.triggerActionsParent;
					}
					this.playbackCounterSlider = componentInChildren.playbackCounterSlider;
					this.startTimestepSlider = componentInChildren.startTimestepSlider;
					this.stopTimestepSlider = componentInChildren.stopTimestepSlider;
					this.loopbackTimeSlider = componentInChildren.loopbackTimeSlider;
					this.playbackSpeedSlider = componentInChildren.playbackSpeedSlider;
					this.loopToggle = componentInChildren.loopToggle;
					this.autoRecordStopToggle = componentInChildren.autoRecordStopToggle;
					this.showRecordPathsToggle = componentInChildren.showRecordPathsToggle;
					this.showStartMarkersToggle = componentInChildren.showStartMarkersToggle;
					if (componentInChildren.addTriggerButton != null)
					{
						this.addTriggerButton = componentInChildren.addTriggerButton;
						componentInChildren.addTriggerButton.onClick.AddListener(new UnityAction(this.AddTrigger));
					}
					if (componentInChildren.clearAllTriggersButton != null)
					{
						this.clearAllTriggersButton = componentInChildren.clearAllTriggersButton;
						componentInChildren.clearAllTriggersButton.onClick.AddListener(new UnityAction(this.ClearTriggers));
					}
				}
			}
		}
	}

	// Token: 0x060051FF RID: 20991 RVA: 0x001DA3D2 File Offset: 0x001D87D2
	protected void OnDestroy()
	{
		if (SuperController.singleton != null)
		{
			SuperController singleton = SuperController.singleton;
			singleton.onAtomUIDRenameHandlers = (SuperController.OnAtomUIDRename)Delegate.Remove(singleton.onAtomUIDRenameHandlers, new SuperController.OnAtomUIDRename(this.OnAtomRename));
		}
	}

	// Token: 0x06005200 RID: 20992 RVA: 0x001DA40A File Offset: 0x001D880A
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

	// Token: 0x06005201 RID: 20993 RVA: 0x001DA430 File Offset: 0x001D8830
	private void Update()
	{
		if (!this.freeze && (SuperController.singleton == null || !SuperController.singleton.freezeAnimation))
		{
			if (this._isRecording)
			{
				this.RecordStep(false);
			}
			if (this._linkToAudioSourceControl && this.audioSourceControl != null && this.audioSourceControl.audioSource.clip != null && this.audioSourceControl.audioSource.isPlaying)
			{
				if (this._recordedLength < this.audioSourceControl.audioSource.clip.length + this._audioSourceTimeOffset)
				{
					this.recordedLength = this.audioSourceControl.audioSource.clip.length + this._audioSourceTimeOffset;
				}
				this.SetIsPlayingFromAudioSource();
				this.InternalSetPlaybackCounter(this.audioSourceControl.audioSource.time + this._audioSourceTimeOffset, true, false);
			}
			else if (this._isLoopingBack)
			{
				this._loopbackCounter += Time.deltaTime * this._playbackSpeed;
				if (this._loopbackCounter > this._loopbackTime)
				{
					this.StopLoopback();
				}
				else
				{
					this.LoopbackStep();
				}
			}
			else if (this._isPlaying)
			{
				this.InternalSetPlaybackCounter(this.playbackCounter + Time.deltaTime * this._playbackSpeed, false, false);
			}
		}
		foreach (AnimationTimelineTrigger animationTimelineTrigger in this.triggers)
		{
			animationTimelineTrigger.Update();
		}
	}

	// Token: 0x06005202 RID: 20994 RVA: 0x001DA5F8 File Offset: 0x001D89F8
	[CompilerGenerated]
	private static int <ResetAnimation>m__0(AnimationTimelineTrigger t1, AnimationTimelineTrigger t2)
	{
		return t2.triggerStartTime.CompareTo(t1.triggerStartTime);
	}

	// Token: 0x06005203 RID: 20995 RVA: 0x001DA61C File Offset: 0x001D8A1C
	[CompilerGenerated]
	private static int <SortTriggersByStartTime>m__1(AnimationTimelineTrigger a, AnimationTimelineTrigger b)
	{
		return a.triggerStartTime.CompareTo(b.triggerStartTime);
	}

	// Token: 0x04004141 RID: 16705
	protected string[] customParamNames = new string[]
	{
		"recordedLength",
		"triggers",
		"audioSourceControl"
	};

	// Token: 0x04004142 RID: 16706
	public bool isSceneMasterController;

	// Token: 0x04004143 RID: 16707
	protected HashSet<MotionAnimationControl> controllers;

	// Token: 0x04004144 RID: 16708
	[SerializeField]
	protected bool _linkToAudioSourceControl;

	// Token: 0x04004145 RID: 16709
	protected JSONStorableBool linkToAudioSourceControlJSON;

	// Token: 0x04004146 RID: 16710
	protected float _audioSourceTimeOffset;

	// Token: 0x04004147 RID: 16711
	protected JSONStorableFloat audioSourceTimeOffsetJSON;

	// Token: 0x04004148 RID: 16712
	public UIPopup audioSourceControlAtomSelectionPopup;

	// Token: 0x04004149 RID: 16713
	public UIPopup audioSourceControlSelectionPopup;

	// Token: 0x0400414A RID: 16714
	protected string audioSourceControlAtomUID;

	// Token: 0x0400414B RID: 16715
	[SerializeField]
	protected AudioSourceControl _audioSourceControl;

	// Token: 0x0400414C RID: 16716
	public Slider playbackCounterSlider;

	// Token: 0x0400414D RID: 16717
	protected float _playbackCounter;

	// Token: 0x0400414E RID: 16718
	protected float _lastPlaybackCounter;

	// Token: 0x0400414F RID: 16719
	protected JSONStorableFloat playbackCounterJSON;

	// Token: 0x04004150 RID: 16720
	public Slider startTimestepSlider;

	// Token: 0x04004151 RID: 16721
	protected float _startTimestep;

	// Token: 0x04004152 RID: 16722
	protected JSONStorableFloat startTimestepJSON;

	// Token: 0x04004153 RID: 16723
	public Slider stopTimestepSlider;

	// Token: 0x04004154 RID: 16724
	protected float _stopTimestep;

	// Token: 0x04004155 RID: 16725
	protected JSONStorableFloat stopTimestepJSON;

	// Token: 0x04004156 RID: 16726
	protected JSONStorableAction ResetAnimationAction;

	// Token: 0x04004157 RID: 16727
	protected bool _disableTriggers;

	// Token: 0x04004158 RID: 16728
	public bool freeze;

	// Token: 0x04004159 RID: 16729
	protected JSONStorableFloat loopbackTimeJSON;

	// Token: 0x0400415A RID: 16730
	protected float _loopbackCounter;

	// Token: 0x0400415B RID: 16731
	public Slider loopbackTimeSlider;

	// Token: 0x0400415C RID: 16732
	[SerializeField]
	protected float _loopbackTime = 1f;

	// Token: 0x0400415D RID: 16733
	protected JSONStorableBool autoPlayJSON;

	// Token: 0x0400415E RID: 16734
	protected bool _autoPlay = true;

	// Token: 0x0400415F RID: 16735
	protected JSONStorableBool loopJSON;

	// Token: 0x04004160 RID: 16736
	public Toggle loopToggle;

	// Token: 0x04004161 RID: 16737
	[SerializeField]
	protected bool _loop;

	// Token: 0x04004162 RID: 16738
	protected JSONStorableBool autoRecordStopJSON;

	// Token: 0x04004163 RID: 16739
	public Toggle autoRecordStopToggle;

	// Token: 0x04004164 RID: 16740
	protected bool _ignoreAutoRecordStop;

	// Token: 0x04004165 RID: 16741
	[SerializeField]
	protected bool _autoRecordStop = true;

	// Token: 0x04004166 RID: 16742
	protected JSONStorableFloat playbackSpeedJSON;

	// Token: 0x04004167 RID: 16743
	public Slider playbackSpeedSlider;

	// Token: 0x04004168 RID: 16744
	[SerializeField]
	protected float _playbackSpeed = 1f;

	// Token: 0x04004169 RID: 16745
	protected JSONStorableBool showRecordPathsJSON;

	// Token: 0x0400416A RID: 16746
	public Toggle showRecordPathsToggle;

	// Token: 0x0400416B RID: 16747
	[SerializeField]
	protected bool _showRecordPaths;

	// Token: 0x0400416C RID: 16748
	protected JSONStorableBool showStartMarkersJSON;

	// Token: 0x0400416D RID: 16749
	public Toggle showStartMarkersToggle;

	// Token: 0x0400416E RID: 16750
	[SerializeField]
	protected bool _showStartMarkers;

	// Token: 0x0400416F RID: 16751
	protected List<AnimationTimelineTrigger> triggers;

	// Token: 0x04004170 RID: 16752
	protected List<AnimationTimelineTrigger> reverseTriggers;

	// Token: 0x04004171 RID: 16753
	public RectTransform triggerActionsParent;

	// Token: 0x04004172 RID: 16754
	public RectTransform triggerPrefab;

	// Token: 0x04004173 RID: 16755
	public ScrollRectContentManager triggerContentManager;

	// Token: 0x04004174 RID: 16756
	public Button clearAllTriggersButton;

	// Token: 0x04004175 RID: 16757
	protected GameObject advancedPanel;

	// Token: 0x04004176 RID: 16758
	protected bool advancedPanelOpen;

	// Token: 0x04004177 RID: 16759
	public Button addTriggerButton;

	// Token: 0x04004178 RID: 16760
	protected HashSet<AnimationTimelineTrigger> _selectedTriggers;

	// Token: 0x04004179 RID: 16761
	protected JSONStorableFloat triggerSelectFromTimeJSON;

	// Token: 0x0400417A RID: 16762
	protected JSONStorableFloat triggerSelectToTimeJSON;

	// Token: 0x0400417B RID: 16763
	protected JSONStorableAction SelectTriggersInTimeRangeAction;

	// Token: 0x0400417C RID: 16764
	protected JSONStorableAction ClearSelectedTriggersAction;

	// Token: 0x0400417D RID: 16765
	protected JSONStorableFloat triggerTimeAdjustmentJSON;

	// Token: 0x0400417E RID: 16766
	protected JSONStorableAction AdjustTimeOfSelectedTriggersAction;

	// Token: 0x0400417F RID: 16767
	protected JSONStorableAction SortTriggersByStartTimeAction;

	// Token: 0x04004180 RID: 16768
	protected JSONStorableFloat triggerPasteToTimeJSON;

	// Token: 0x04004181 RID: 16769
	protected JSONStorableAction CopySelectedTriggersAndPasteToTimeAction;

	// Token: 0x04004182 RID: 16770
	protected float _lastRecordTime;

	// Token: 0x04004183 RID: 16771
	public float recordInterval = 0.02f;

	// Token: 0x04004184 RID: 16772
	protected bool _isRecording;

	// Token: 0x04004185 RID: 16773
	public GameObject activeWhilePlaying;

	// Token: 0x04004186 RID: 16774
	public GameObject activeWhileStopped;

	// Token: 0x04004187 RID: 16775
	protected bool _isPlaying;

	// Token: 0x04004188 RID: 16776
	protected bool _isLoopingBack;

	// Token: 0x04004189 RID: 16777
	protected float _recordedLength;

	// Token: 0x0400418A RID: 16778
	protected JSONStorableAction ClearAllAnimationAction;

	// Token: 0x0400418B RID: 16779
	protected JSONStorableAction SelectControllersArmedForRecordAction;

	// Token: 0x0400418C RID: 16780
	protected JSONStorableAction ArmAllControlledControllersForRecordAction;

	// Token: 0x0400418D RID: 16781
	protected JSONStorableAction StartRecordModeAction;

	// Token: 0x0400418E RID: 16782
	protected JSONStorableAction StartRecordAction;

	// Token: 0x0400418F RID: 16783
	protected JSONStorableAction StopRecordAction;

	// Token: 0x04004190 RID: 16784
	protected JSONStorableAction StopRecordModeAction;

	// Token: 0x04004191 RID: 16785
	protected JSONStorableAction StartPlaybackAction;

	// Token: 0x04004192 RID: 16786
	protected JSONStorableAction StopPlaybackAction;

	// Token: 0x04004193 RID: 16787
	protected JSONStorableAction TrimAnimationAction;

	// Token: 0x04004194 RID: 16788
	protected JSONStorableAction SetToDesiredLengthAction;

	// Token: 0x04004195 RID: 16789
	protected float _desiredLength = 60f;

	// Token: 0x04004196 RID: 16790
	protected JSONStorableFloat desiredLengthJSON;

	// Token: 0x04004197 RID: 16791
	protected JSONStorableAction CopyFromSceneMasterAction;

	// Token: 0x04004198 RID: 16792
	protected JSONStorableAction CopyToSceneMasterAction;

	// Token: 0x04004199 RID: 16793
	protected JSONStorableAction SeekToBeginningAction;

	// Token: 0x0400419A RID: 16794
	[CompilerGenerated]
	private static Comparison<AnimationTimelineTrigger> <>f__am$cache0;

	// Token: 0x0400419B RID: 16795
	[CompilerGenerated]
	private static Comparison<AnimationTimelineTrigger> <>f__am$cache1;

	// Token: 0x02000FD7 RID: 4055
	[CompilerGenerated]
	private sealed class <SeekToBeginningCo>c__Iterator0 : IEnumerator, IDisposable, IEnumerator<object>
	{
		// Token: 0x0600756C RID: 30060 RVA: 0x001DA63D File Offset: 0x001D8A3D
		[DebuggerHidden]
		public <SeekToBeginningCo>c__Iterator0()
		{
		}

		// Token: 0x0600756D RID: 30061 RVA: 0x001DA648 File Offset: 0x001D8A48
		public bool MoveNext()
		{
			uint num = (uint)this.$PC;
			this.$PC = -1;
			switch (num)
			{
			case 0U:
				i = 0;
				break;
			case 1U:
				i++;
				break;
			default:
				return false;
			}
			if (i < 10)
			{
				base.PlaybackStepForceAlign();
				this.$current = null;
				if (!this.$disposing)
				{
					this.$PC = 1;
				}
				return true;
			}
			this.$PC = -1;
			return false;
		}

		// Token: 0x1700115F RID: 4447
		// (get) Token: 0x0600756E RID: 30062 RVA: 0x001DA6CD File Offset: 0x001D8ACD
		object IEnumerator<object>.Current
		{
			[DebuggerHidden]
			get
			{
				return this.$current;
			}
		}

		// Token: 0x17001160 RID: 4448
		// (get) Token: 0x0600756F RID: 30063 RVA: 0x001DA6D5 File Offset: 0x001D8AD5
		object IEnumerator.Current
		{
			[DebuggerHidden]
			get
			{
				return this.$current;
			}
		}

		// Token: 0x06007570 RID: 30064 RVA: 0x001DA6DD File Offset: 0x001D8ADD
		[DebuggerHidden]
		public void Dispose()
		{
			this.$disposing = true;
			this.$PC = -1;
		}

		// Token: 0x06007571 RID: 30065 RVA: 0x001DA6ED File Offset: 0x001D8AED
		[DebuggerHidden]
		public void Reset()
		{
			throw new NotSupportedException();
		}

		// Token: 0x0400698A RID: 27018
		internal int <i>__1;

		// Token: 0x0400698B RID: 27019
		internal MotionAnimationMaster $this;

		// Token: 0x0400698C RID: 27020
		internal object $current;

		// Token: 0x0400698D RID: 27021
		internal bool $disposing;

		// Token: 0x0400698E RID: 27022
		internal int $PC;
	}
}
