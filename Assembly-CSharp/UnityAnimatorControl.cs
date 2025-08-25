using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using SimpleJSON;
using UnityEngine;

// Token: 0x02000B76 RID: 2934
public class UnityAnimatorControl : JSONStorable
{
	// Token: 0x0600523C RID: 21052 RVA: 0x001DABA5 File Offset: 0x001D8FA5
	public UnityAnimatorControl()
	{
	}

	// Token: 0x0600523D RID: 21053 RVA: 0x001DABE4 File Offset: 0x001D8FE4
	public override JSONClass GetJSON(bool includePhysical = true, bool includeAppearance = true, bool forceStore = false)
	{
		JSONClass json = base.GetJSON(includePhysical, includeAppearance, forceStore);
		if (includePhysical)
		{
			this.AnimatorReset();
			JSONArray jsonarray = new JSONArray();
			json["sequence"] = jsonarray;
			foreach (AnimationSequenceClip animationSequenceClip in this.animationSequence)
			{
				JSONClass jsonclass = new JSONClass();
				jsonclass["name"] = animationSequenceClip.Name;
				jsonclass["useCrossFade"].AsBool = animationSequenceClip.UseCrossFade;
				jsonclass["crossFadeTime"].AsFloat = animationSequenceClip.CrossFadeTime;
				jsonarray.Add(jsonclass);
			}
		}
		return json;
	}

	// Token: 0x0600523E RID: 21054 RVA: 0x001DACB8 File Offset: 0x001D90B8
	public override void RestoreFromJSON(JSONClass jc, bool restorePhysical = true, bool restoreAppearance = true, JSONArray presetAtoms = null, bool setMissingToDefault = true)
	{
		if (restorePhysical && !base.physicalLocked)
		{
			this.ClearSequence();
			if (base.mergeRestore)
			{
				this.triggerSmoothTransition = true;
			}
			else
			{
				this.AnimatorReset();
			}
		}
		base.RestoreFromJSON(jc, restorePhysical, restoreAppearance, presetAtoms, setMissingToDefault);
		if (restorePhysical && !base.physicalLocked)
		{
			JSONArray asArray = jc["sequence"].AsArray;
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
							string text = asObject["name"];
							bool asBool = asObject["useCrossFade"].AsBool;
							float asFloat = asObject["crossFadeTime"].AsFloat;
							if (text != null)
							{
								AnimationSequenceClip asc = new AnimationSequenceClip(text, asBool, asFloat);
								this.AddAnimationToSequence(asc);
							}
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
			if (!base.mergeRestore)
			{
				this.AnimatorReset();
			}
			this.AnimatorPlay();
		}
	}

	// Token: 0x0600523F RID: 21055 RVA: 0x001DAE00 File Offset: 0x001D9200
	protected IEnumerator DelayResetPhysics()
	{
		yield return null;
		this.containingAtom.ResetPhysics(true, false);
		yield break;
	}

	// Token: 0x06005240 RID: 21056 RVA: 0x001DAE1C File Offset: 0x001D921C
	protected void AnimatorReset()
	{
		if (this.animator != null && this.animatorEnabledJSON.val)
		{
			this.animator.Rebind();
			this.animator.transform.localPosition = Vector3.zero;
			this.animator.transform.localRotation = Quaternion.identity;
			this.SetCurrentNode(this.animationSequence.First, true, 0f);
			base.StartCoroutine(this.DelayResetPhysics());
		}
	}

	// Token: 0x06005241 RID: 21057 RVA: 0x001DAEA3 File Offset: 0x001D92A3
	protected void SyncAnimatorEnabled(bool b)
	{
		this._animatorEnabled = b;
		if (this.animator != null)
		{
			this.animator.gameObject.SetActive(b);
		}
	}

	// Token: 0x06005242 RID: 21058 RVA: 0x001DAED0 File Offset: 0x001D92D0
	protected void SyncAnimatorIsPlaying(bool b)
	{
		this._animatorIsPlaying = b;
		if (this.animatorPlayAction != null && this.animatorPlayAction.button != null)
		{
			this.animatorPlayAction.button.gameObject.SetActive(!this._animatorIsPlaying);
		}
		if (this.animatorPauseAction != null && this.animatorPauseAction.button != null)
		{
			this.animatorPauseAction.button.gameObject.SetActive(this._animatorIsPlaying);
		}
	}

	// Token: 0x06005243 RID: 21059 RVA: 0x001DAF5F File Offset: 0x001D935F
	protected void AnimatorPlay()
	{
		this.animatorIsPlayingJSON.val = true;
	}

	// Token: 0x06005244 RID: 21060 RVA: 0x001DAF6D File Offset: 0x001D936D
	protected void AnimatorPause()
	{
		this.animatorIsPlayingJSON.val = false;
	}

	// Token: 0x06005245 RID: 21061 RVA: 0x001DAF7C File Offset: 0x001D937C
	protected void SyncAnimatorSpeed(float f)
	{
		this._animatorSpeed = f;
		if (this.animator != null)
		{
			if (this.currentAnimationNameJSON.val != "None")
			{
				this.animator.speed = this._animatorSpeed;
			}
			else
			{
				this.animator.speed = 0f;
			}
		}
	}

	// Token: 0x06005246 RID: 21062 RVA: 0x001DAFE1 File Offset: 0x001D93E1
	protected void SyncAnimationSelection(string choice)
	{
		this._animationSelection = choice;
	}

	// Token: 0x06005247 RID: 21063 RVA: 0x001DAFEA File Offset: 0x001D93EA
	protected void SyncUseCrossFade(bool b)
	{
		this._useCrossFade = b;
	}

	// Token: 0x06005248 RID: 21064 RVA: 0x001DAFF3 File Offset: 0x001D93F3
	protected void SyncCrossFadeTime(float f)
	{
		this._crossFadeTime = f;
	}

	// Token: 0x06005249 RID: 21065 RVA: 0x001DAFFC File Offset: 0x001D93FC
	protected void SyncCurrentAnimationName(string s)
	{
		this._currentAnimationName = s;
	}

	// Token: 0x0600524A RID: 21066 RVA: 0x001DB008 File Offset: 0x001D9408
	protected void SetCurrentAnimation(AnimationSequenceClip animation, bool forceNoCrossFade = false, float fixedTimeOffset = 0f)
	{
		if (animation != null)
		{
			this.currentAnimationNameJSON.val = animation.Name;
			if (this.animator != null)
			{
				this.animator.speed = this._animatorSpeed;
				if (this.triggerSmoothTransition)
				{
					this.animator.CrossFadeInFixedTime(animation.Name, 0.5f, -1, fixedTimeOffset, 0f);
					this.triggerSmoothTransition = false;
				}
				else if (forceNoCrossFade)
				{
					this.animator.Play(animation.Name);
				}
				else if (animation.UseCrossFade)
				{
					this.animator.CrossFadeInFixedTime(animation.Name, animation.CrossFadeTime, -1, fixedTimeOffset, 0f);
				}
				else
				{
					this.animator.CrossFadeInFixedTime(animation.Name, 0f, -1, fixedTimeOffset, 0f);
				}
			}
		}
		else
		{
			this.currentAnimationNameJSON.val = "None";
			if (this.animator != null)
			{
				this.animator.speed = 0f;
			}
		}
	}

	// Token: 0x17000BF1 RID: 3057
	// (get) Token: 0x0600524B RID: 21067 RVA: 0x001DB11F File Offset: 0x001D951F
	protected LinkedListNode<AnimationSequenceClip> PreviousNode
	{
		get
		{
			return this._previousNode;
		}
	}

	// Token: 0x17000BF2 RID: 3058
	// (get) Token: 0x0600524C RID: 21068 RVA: 0x001DB127 File Offset: 0x001D9527
	protected LinkedListNode<AnimationSequenceClip> CurrentNode
	{
		get
		{
			return this._currentNode;
		}
	}

	// Token: 0x0600524D RID: 21069 RVA: 0x001DB130 File Offset: 0x001D9530
	protected void SetCurrentNode(LinkedListNode<AnimationSequenceClip> node, bool forceNoCrossFade = false, float fixedTimeOffset = 0f)
	{
		if (this._currentNode != null)
		{
			this._currentNode.Value.IsPlaying = false;
			this._previousNode = this._currentNode;
		}
		else
		{
			this._previousNode = null;
		}
		this._currentNode = node;
		if (this._currentNode != null)
		{
			this._currentNode.Value.IsPlaying = true;
			this.SetCurrentAnimation(this._currentNode.Value, forceNoCrossFade, fixedTimeOffset);
		}
		else
		{
			this.SetCurrentAnimation(null, forceNoCrossFade, fixedTimeOffset);
		}
	}

	// Token: 0x0600524E RID: 21070 RVA: 0x001DB1B5 File Offset: 0x001D95B5
	protected void SyncLoopSequence(bool b)
	{
		this._loopSequence = b;
	}

	// Token: 0x0600524F RID: 21071 RVA: 0x001DB1BE File Offset: 0x001D95BE
	protected void RestartAnimationSequence()
	{
		this.SetCurrentNode(this.animationSequence.First, false, 0f);
	}

	// Token: 0x06005250 RID: 21072 RVA: 0x001DB1D8 File Offset: 0x001D95D8
	protected void SyncSequenceUIOrder()
	{
		int num = 0;
		foreach (AnimationSequenceClip animationSequenceClip in this.animationSequence)
		{
			animationSequenceClip.UI.SetSiblingIndex(num);
			num++;
		}
	}

	// Token: 0x06005251 RID: 21073 RVA: 0x001DB240 File Offset: 0x001D9640
	protected void AddAnimationToSequence(AnimationSequenceClip asc)
	{
		UnityAnimatorControl.<AddAnimationToSequence>c__AnonStorey1 <AddAnimationToSequence>c__AnonStorey = new UnityAnimatorControl.<AddAnimationToSequence>c__AnonStorey1();
		<AddAnimationToSequence>c__AnonStorey.asc = asc;
		<AddAnimationToSequence>c__AnonStorey.$this = this;
		if (this.animationSequenceClipPrefab != null && this.animationSequenceContainer != null)
		{
			UnityAnimatorControl.<AddAnimationToSequence>c__AnonStorey2 <AddAnimationToSequence>c__AnonStorey2 = new UnityAnimatorControl.<AddAnimationToSequence>c__AnonStorey2();
			<AddAnimationToSequence>c__AnonStorey2.<>f__ref$1 = <AddAnimationToSequence>c__AnonStorey;
			<AddAnimationToSequence>c__AnonStorey2.rt = (RectTransform)UnityEngine.Object.Instantiate<Transform>(this.animationSequenceClipPrefab);
			<AddAnimationToSequence>c__AnonStorey.asc.UI = <AddAnimationToSequence>c__AnonStorey2.rt;
			<AddAnimationToSequence>c__AnonStorey.asc.removeCallback = new AnimationSequenceClip.RemoveCallback(<AddAnimationToSequence>c__AnonStorey2.<>m__0);
			<AddAnimationToSequence>c__AnonStorey2.rt.SetParent(this.animationSequenceContainer, false);
		}
		else
		{
			<AddAnimationToSequence>c__AnonStorey.asc.removeCallback = new AnimationSequenceClip.RemoveCallback(<AddAnimationToSequence>c__AnonStorey.<>m__0);
		}
		<AddAnimationToSequence>c__AnonStorey.newNode = this.animationSequence.AddLast(<AddAnimationToSequence>c__AnonStorey.asc);
		<AddAnimationToSequence>c__AnonStorey.asc.moveBackwardCallback = new AnimationSequenceClip.MoveBackwardCallback(<AddAnimationToSequence>c__AnonStorey.<>m__1);
		<AddAnimationToSequence>c__AnonStorey.asc.moveForwardCallback = new AnimationSequenceClip.MoveForwardCallback(<AddAnimationToSequence>c__AnonStorey.<>m__2);
	}

	// Token: 0x06005252 RID: 21074 RVA: 0x001DB344 File Offset: 0x001D9744
	protected void AddAnimationToSequence(string choice)
	{
		AnimationSequenceClip asc = new AnimationSequenceClip(choice, this._useCrossFade, this._crossFadeTime);
		this.AddAnimationToSequence(asc);
	}

	// Token: 0x06005253 RID: 21075 RVA: 0x001DB36B File Offset: 0x001D976B
	protected void ClearAndAddAnimationToSequence(string choice)
	{
		this.ClearSequence();
		this.SetCurrentNode(this.animationSequence.First, false, 0f);
		this.AddAnimationToSequence(choice);
	}

	// Token: 0x06005254 RID: 21076 RVA: 0x001DB394 File Offset: 0x001D9794
	protected void ClearSequence()
	{
		List<AnimationSequenceClip> list = new List<AnimationSequenceClip>();
		foreach (AnimationSequenceClip item in this.animationSequence)
		{
			list.Add(item);
		}
		foreach (AnimationSequenceClip animationSequenceClip in list)
		{
			animationSequenceClip.Remove();
		}
		this.animationSequence.Clear();
		this.SetCurrentNode(null, false, 0f);
	}

	// Token: 0x06005255 RID: 21077 RVA: 0x001DB454 File Offset: 0x001D9854
	protected void ClearSequenceButFinishCurrentClip()
	{
		List<AnimationSequenceClip> list = new List<AnimationSequenceClip>();
		foreach (AnimationSequenceClip item in this.animationSequence)
		{
			list.Add(item);
		}
		foreach (AnimationSequenceClip animationSequenceClip in list)
		{
			animationSequenceClip.Remove();
		}
		this.animationSequence.Clear();
	}

	// Token: 0x06005256 RID: 21078 RVA: 0x001DB508 File Offset: 0x001D9908
	protected void NextClipInSequence()
	{
		if (this.CurrentNode != null && this.CurrentNode.Next != null)
		{
			this.SetCurrentNode(this.CurrentNode.Next, false, 0f);
		}
	}

	// Token: 0x06005257 RID: 21079 RVA: 0x001DB53C File Offset: 0x001D993C
	protected void PreviousClipInSequence()
	{
		if (this.CurrentNode != null && this.CurrentNode.Previous != null)
		{
			this.SetCurrentNode(this.CurrentNode.Previous, false, 0f);
		}
	}

	// Token: 0x06005258 RID: 21080 RVA: 0x001DB570 File Offset: 0x001D9970
	protected void SyncAnimationRotationSpeed(float f)
	{
		this._animationRotationSpeed = f;
	}

	// Token: 0x06005259 RID: 21081 RVA: 0x001DB579 File Offset: 0x001D9979
	protected void SyncAnimationRotationDegressForAction(float f)
	{
		this._animationRotationDegreesForAction = f;
	}

	// Token: 0x0600525A RID: 21082 RVA: 0x001DB582 File Offset: 0x001D9982
	protected void AnimationRotate()
	{
		if (this.animator != null)
		{
			this.animator.transform.Rotate(0f, this._animationRotationDegreesForAction, 0f);
		}
	}

	// Token: 0x0600525B RID: 21083 RVA: 0x001DB5B5 File Offset: 0x001D99B5
	public void RotateAnimation(float degrees)
	{
		if (this.animator != null)
		{
			this.animator.transform.Rotate(0f, degrees, 0f);
		}
	}

	// Token: 0x0600525C RID: 21084 RVA: 0x001DB5E4 File Offset: 0x001D99E4
	protected void Init()
	{
		this.animationSequence = new LinkedList<AnimationSequenceClip>();
		this.animatorResetAction = new JSONStorableAction("AnimatorReset", new JSONStorableAction.ActionCallback(this.AnimatorReset));
		base.RegisterAction(this.animatorResetAction);
		this.animatorEnabledJSON = new JSONStorableBool("animatorEnabled", this._animatorEnabled, new JSONStorableBool.SetBoolCallback(this.SyncAnimatorEnabled));
		this.animatorEnabledJSON.storeType = JSONStorableParam.StoreType.Physical;
		base.RegisterBool(this.animatorEnabledJSON);
		this.animatorIsPlayingJSON = new JSONStorableBool("animatorIsPlaying", true, new JSONStorableBool.SetBoolCallback(this.SyncAnimatorIsPlaying));
		this.animatorIsPlayingJSON.storeType = JSONStorableParam.StoreType.Physical;
		base.RegisterBool(this.animatorIsPlayingJSON);
		this.animatorPlayAction = new JSONStorableAction("AnimatorPlay", new JSONStorableAction.ActionCallback(this.AnimatorPlay));
		base.RegisterAction(this.animatorPlayAction);
		this.animatorPauseAction = new JSONStorableAction("AnimatorPause", new JSONStorableAction.ActionCallback(this.AnimatorPause));
		base.RegisterAction(this.animatorPauseAction);
		this.animatorSpeedJSON = new JSONStorableFloat("AnimatorSpeed", this._animatorSpeed, new JSONStorableFloat.SetFloatCallback(this.SyncAnimatorSpeed), 0f, 5f, true, true);
		this.animatorSpeedJSON.storeType = JSONStorableParam.StoreType.Physical;
		base.RegisterFloat(this.animatorSpeedJSON);
		List<string> choicesList = new List<string>(this.animationChoices);
		this._animationSelection = this.startingAnimationChoice;
		this.animationSelectionJSON = new JSONStorableStringChooser("animationSelection", choicesList, this.startingAnimationChoice, "Animation Selection", new JSONStorableStringChooser.SetStringCallback(this.SyncAnimationSelection));
		this.animationSelectionJSON.storeType = JSONStorableParam.StoreType.Physical;
		base.RegisterStringChooser(this.animationSelectionJSON);
		this.useCrossFadeJSON = new JSONStorableBool("useCrossFade", this._useCrossFade, new JSONStorableBool.SetBoolCallback(this.SyncUseCrossFade));
		this.useCrossFadeJSON.storeType = JSONStorableParam.StoreType.Physical;
		base.RegisterBool(this.useCrossFadeJSON);
		this.crossFadeTimeJSON = new JSONStorableFloat("crossFadeTime", this._crossFadeTime, new JSONStorableFloat.SetFloatCallback(this.SyncCrossFadeTime), 0f, 5f, true, true);
		this.crossFadeTimeJSON.storeType = JSONStorableParam.StoreType.Physical;
		base.RegisterFloat(this.crossFadeTimeJSON);
		this.currentAnimationNameJSON = new JSONStorableString("currentAnimationName", this._currentAnimationName, new JSONStorableString.SetStringCallback(this.SyncCurrentAnimationName));
		this.loopSequenceJSON = new JSONStorableBool("loopSequence", this._loopSequence, new JSONStorableBool.SetBoolCallback(this.SyncLoopSequence));
		this.loopSequenceJSON.storeType = JSONStorableParam.StoreType.Physical;
		base.RegisterBool(this.loopSequenceJSON);
		this.restartAnimationSequenceAction = new JSONStorableAction("RestartAnimationSequence", new JSONStorableAction.ActionCallback(this.RestartAnimationSequence));
		base.RegisterAction(this.restartAnimationSequenceAction);
		this.addAnimationToSequenceAction = new JSONStorableActionStringChooser("AddAnimationToSequence", new JSONStorableActionStringChooser.StringChoiceActionCallback(this.AddAnimationToSequence), this.animationSelectionJSON);
		base.RegisterStringChooserAction(this.addAnimationToSequenceAction);
		this.clearAndAddAnimationToSequenceAction = new JSONStorableActionStringChooser("ClearAndAddAnimationToSequence", new JSONStorableActionStringChooser.StringChoiceActionCallback(this.ClearAndAddAnimationToSequence), this.animationSelectionJSON);
		base.RegisterStringChooserAction(this.clearAndAddAnimationToSequenceAction);
		this.clearSequenceAction = new JSONStorableAction("ClearSequence", new JSONStorableAction.ActionCallback(this.ClearSequence));
		base.RegisterAction(this.clearSequenceAction);
		this.clearSequenceButFinishCurrentClipAction = new JSONStorableAction("ClearSequenceButFinishCurrentClip", new JSONStorableAction.ActionCallback(this.ClearSequenceButFinishCurrentClip));
		base.RegisterAction(this.clearSequenceButFinishCurrentClipAction);
		this.nextClipInSequenceAction = new JSONStorableAction("NextClipInSequence", new JSONStorableAction.ActionCallback(this.NextClipInSequence));
		base.RegisterAction(this.nextClipInSequenceAction);
		this.previousClipInSequenceAction = new JSONStorableAction("PreviousClipInSequence", new JSONStorableAction.ActionCallback(this.PreviousClipInSequence));
		base.RegisterAction(this.previousClipInSequenceAction);
		this.animationRotationSpeedJSON = new JSONStorableFloat("animationRotationSpeed", this._animationRotationSpeed, new JSONStorableFloat.SetFloatCallback(this.SyncAnimationRotationSpeed), -100f, 100f, false, true);
		this.animationRotationSpeedJSON.storeType = JSONStorableParam.StoreType.Physical;
		base.RegisterFloat(this.animationRotationSpeedJSON);
		this.animationRotationDegressForAction = new JSONStorableFloat("animationRotationDegreesForAction", this._animationRotationDegreesForAction, new JSONStorableFloat.SetFloatCallback(this.SyncAnimationRotationDegressForAction), -90f, 90f, false, true);
		this.animationRotationDegressForAction.storeType = JSONStorableParam.StoreType.Physical;
		base.RegisterFloat(this.animationRotationDegressForAction);
		this.animationRotateAction = new JSONStorableAction("AnimationRotate", new JSONStorableAction.ActionCallback(this.AnimationRotate));
		base.RegisterAction(this.animationRotateAction);
	}

	// Token: 0x0600525D RID: 21085 RVA: 0x001DBA38 File Offset: 0x001D9E38
	protected override void InitUI(Transform t, bool isAlt)
	{
		if (t != null)
		{
			UnityAnimatorControlUI componentInChildren = t.GetComponentInChildren<UnityAnimatorControlUI>(true);
			if (componentInChildren != null)
			{
				if (!isAlt)
				{
					this.animationSequenceContainer = componentInChildren.sequenceContainer;
				}
				this.animatorResetAction.RegisterButton(componentInChildren.animatorResetButton, isAlt);
				this.animatorEnabledJSON.RegisterToggle(componentInChildren.animatorEnabledToggle, isAlt);
				this.animatorIsPlayingJSON.RegisterIndicator(componentInChildren.animatorIsPlayingIndicator, isAlt);
				this.animatorPlayAction.RegisterButton(componentInChildren.animatorPlayButton, isAlt);
				this.animatorPauseAction.RegisterButton(componentInChildren.animatorPauseButton, isAlt);
				this.animatorSpeedJSON.RegisterSlider(componentInChildren.animatorSpeedSlider, isAlt);
				this.animationSelectionJSON.RegisterPopup(componentInChildren.animationSelectionPopup, isAlt);
				this.useCrossFadeJSON.RegisterToggle(componentInChildren.useCrossFadeToggle, isAlt);
				this.crossFadeTimeJSON.RegisterSlider(componentInChildren.crossFadeTimeSlider, isAlt);
				this.currentAnimationNameJSON.RegisterText(componentInChildren.currentAnimationNameText, isAlt);
				this.loopSequenceJSON.RegisterToggle(componentInChildren.loopSequenceToggle, isAlt);
				this.restartAnimationSequenceAction.RegisterButton(componentInChildren.restartAnimationSequenceButton, isAlt);
				this.addAnimationToSequenceAction.RegisterButton(componentInChildren.addAnimationToSequenceButton, isAlt);
				this.clearAndAddAnimationToSequenceAction.RegisterButton(componentInChildren.clearAndAddAnimationToSequenceButton, isAlt);
				this.clearSequenceAction.RegisterButton(componentInChildren.clearSequenceButton, isAlt);
				this.nextClipInSequenceAction.RegisterButton(componentInChildren.nextClipInSequenceButton, isAlt);
				this.previousClipInSequenceAction.RegisterButton(componentInChildren.previousClipInSequenceButton, isAlt);
				this.animationRotationSpeedJSON.RegisterSlider(componentInChildren.animationRotationSpeedSlider, isAlt);
				this.animationRotationDegressForAction.RegisterSlider(componentInChildren.animationRotationDegressForActionSlider, isAlt);
				this.animationRotateAction.RegisterButton(componentInChildren.animationRotateButton, isAlt);
				this.SyncAnimatorIsPlaying(this._animatorIsPlaying);
			}
		}
	}

	// Token: 0x0600525E RID: 21086 RVA: 0x001DBBEB File Offset: 0x001D9FEB
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

	// Token: 0x0600525F RID: 21087 RVA: 0x001DBC10 File Offset: 0x001DA010
	protected void FixedUpdate()
	{
		if (this.animator != null)
		{
			this.animator.enabled = (this._animatorIsPlaying && (SuperController.singleton == null || !SuperController.singleton.freezeAnimation));
			if (this.animator.enabled && this._animatorEnabled)
			{
				AnimatorStateInfo currentAnimatorStateInfo = this.animator.GetCurrentAnimatorStateInfo(0);
				bool loop = currentAnimatorStateInfo.loop;
				float normalizedTime = currentAnimatorStateInfo.normalizedTime;
				float num = Mathf.Floor(normalizedTime);
				float num2 = Mathf.Floor(normalizedTime);
				float playProgress = normalizedTime - num;
				AnimatorStateInfo nextAnimatorStateInfo = this.animator.GetNextAnimatorStateInfo(0);
				bool flag = false;
				if (this.CurrentNode != null)
				{
					if (nextAnimatorStateInfo.IsName(this.CurrentNode.Value.Name))
					{
						flag = true;
						float normalizedTime2 = nextAnimatorStateInfo.normalizedTime;
						float playProgress2 = normalizedTime2 - Mathf.Floor(normalizedTime2);
						this.CurrentNode.Value.PlayProgress = playProgress2;
						if (this.PreviousNode != null)
						{
							this.PreviousNode.Value.PlayProgress = playProgress;
						}
					}
					else if (loop)
					{
						this.CurrentNode.Value.PlayProgress = playProgress;
					}
					else if (normalizedTime >= 1f)
					{
						this.lastAnimatorStateLoopCount = 0f;
						this.CurrentNode.Value.PlayProgress = 1f;
					}
					else
					{
						this.CurrentNode.Value.PlayProgress = playProgress;
					}
				}
				if (this.CurrentNode != null)
				{
					if (num2 > this.lastAnimatorStateLoopCount && !flag)
					{
						if (this.CurrentNode.List != null)
						{
							if (this.CurrentNode.Next != null)
							{
								this.SetCurrentNode(this.CurrentNode.Next, false, 0f);
							}
							else if (this._loopSequence)
							{
								this.SetCurrentNode(this.animationSequence.First, false, 0f);
							}
						}
						else
						{
							this.SetCurrentNode(this.animationSequence.First, false, 0f);
						}
					}
				}
				else
				{
					this.SetCurrentNode(this.animationSequence.First, false, 0f);
				}
				this.lastAnimatorStateNormalizedTime = normalizedTime;
				this.lastAnimatorStateLoopCount = num2;
				if (this._animationRotationSpeed != 0f)
				{
					this.animator.transform.Rotate(0f, this._animationRotationSpeed * Time.fixedDeltaTime, 0f);
				}
			}
		}
	}

	// Token: 0x040041FF RID: 16895
	public Animator animator;

	// Token: 0x04004200 RID: 16896
	[HideInInspector]
	public string[] animationChoices;

	// Token: 0x04004201 RID: 16897
	public string startingAnimationChoice;

	// Token: 0x04004202 RID: 16898
	protected JSONStorableAction animatorResetAction;

	// Token: 0x04004203 RID: 16899
	protected bool _animatorEnabled;

	// Token: 0x04004204 RID: 16900
	protected JSONStorableBool animatorEnabledJSON;

	// Token: 0x04004205 RID: 16901
	protected bool _animatorIsPlaying = true;

	// Token: 0x04004206 RID: 16902
	protected JSONStorableBool animatorIsPlayingJSON;

	// Token: 0x04004207 RID: 16903
	protected JSONStorableAction animatorPlayAction;

	// Token: 0x04004208 RID: 16904
	protected JSONStorableAction animatorPauseAction;

	// Token: 0x04004209 RID: 16905
	protected float _animatorSpeed = 1f;

	// Token: 0x0400420A RID: 16906
	protected JSONStorableFloat animatorSpeedJSON;

	// Token: 0x0400420B RID: 16907
	protected string _animationSelection;

	// Token: 0x0400420C RID: 16908
	protected JSONStorableStringChooser animationSelectionJSON;

	// Token: 0x0400420D RID: 16909
	protected bool _useCrossFade = true;

	// Token: 0x0400420E RID: 16910
	protected JSONStorableBool useCrossFadeJSON;

	// Token: 0x0400420F RID: 16911
	protected float _crossFadeTime = 0.5f;

	// Token: 0x04004210 RID: 16912
	protected JSONStorableFloat crossFadeTimeJSON;

	// Token: 0x04004211 RID: 16913
	protected Transform animationSequenceContainer;

	// Token: 0x04004212 RID: 16914
	public Transform animationSequenceClipPrefab;

	// Token: 0x04004213 RID: 16915
	protected string _currentAnimationName = "None";

	// Token: 0x04004214 RID: 16916
	protected JSONStorableString currentAnimationNameJSON;

	// Token: 0x04004215 RID: 16917
	protected bool triggerSmoothTransition;

	// Token: 0x04004216 RID: 16918
	protected int animationSequencePosition = -1;

	// Token: 0x04004217 RID: 16919
	protected LinkedListNode<AnimationSequenceClip> _previousNode;

	// Token: 0x04004218 RID: 16920
	protected LinkedListNode<AnimationSequenceClip> _currentNode;

	// Token: 0x04004219 RID: 16921
	protected LinkedList<AnimationSequenceClip> animationSequence;

	// Token: 0x0400421A RID: 16922
	protected bool _loopSequence;

	// Token: 0x0400421B RID: 16923
	protected JSONStorableBool loopSequenceJSON;

	// Token: 0x0400421C RID: 16924
	protected JSONStorableAction restartAnimationSequenceAction;

	// Token: 0x0400421D RID: 16925
	protected JSONStorableActionStringChooser addAnimationToSequenceAction;

	// Token: 0x0400421E RID: 16926
	protected JSONStorableActionStringChooser clearAndAddAnimationToSequenceAction;

	// Token: 0x0400421F RID: 16927
	protected JSONStorableAction clearSequenceAction;

	// Token: 0x04004220 RID: 16928
	protected JSONStorableAction clearSequenceButFinishCurrentClipAction;

	// Token: 0x04004221 RID: 16929
	protected JSONStorableAction nextClipInSequenceAction;

	// Token: 0x04004222 RID: 16930
	protected JSONStorableAction previousClipInSequenceAction;

	// Token: 0x04004223 RID: 16931
	protected float _animationRotationSpeed;

	// Token: 0x04004224 RID: 16932
	protected JSONStorableFloat animationRotationSpeedJSON;

	// Token: 0x04004225 RID: 16933
	protected float _animationRotationDegreesForAction;

	// Token: 0x04004226 RID: 16934
	protected JSONStorableFloat animationRotationDegressForAction;

	// Token: 0x04004227 RID: 16935
	protected JSONStorableAction animationRotateAction;

	// Token: 0x04004228 RID: 16936
	protected float lastAnimatorStateNormalizedTime;

	// Token: 0x04004229 RID: 16937
	protected float lastAnimatorStateLoopCount;

	// Token: 0x02000FD8 RID: 4056
	[CompilerGenerated]
	private sealed class <DelayResetPhysics>c__Iterator0 : IEnumerator, IDisposable, IEnumerator<object>
	{
		// Token: 0x06007572 RID: 30066 RVA: 0x001DBE90 File Offset: 0x001DA290
		[DebuggerHidden]
		public <DelayResetPhysics>c__Iterator0()
		{
		}

		// Token: 0x06007573 RID: 30067 RVA: 0x001DBE98 File Offset: 0x001DA298
		public bool MoveNext()
		{
			uint num = (uint)this.$PC;
			this.$PC = -1;
			switch (num)
			{
			case 0U:
				this.$current = null;
				if (!this.$disposing)
				{
					this.$PC = 1;
				}
				return true;
			case 1U:
				this.containingAtom.ResetPhysics(true, false);
				this.$PC = -1;
				break;
			}
			return false;
		}

		// Token: 0x17001161 RID: 4449
		// (get) Token: 0x06007574 RID: 30068 RVA: 0x001DBEFD File Offset: 0x001DA2FD
		object IEnumerator<object>.Current
		{
			[DebuggerHidden]
			get
			{
				return this.$current;
			}
		}

		// Token: 0x17001162 RID: 4450
		// (get) Token: 0x06007575 RID: 30069 RVA: 0x001DBF05 File Offset: 0x001DA305
		object IEnumerator.Current
		{
			[DebuggerHidden]
			get
			{
				return this.$current;
			}
		}

		// Token: 0x06007576 RID: 30070 RVA: 0x001DBF0D File Offset: 0x001DA30D
		[DebuggerHidden]
		public void Dispose()
		{
			this.$disposing = true;
			this.$PC = -1;
		}

		// Token: 0x06007577 RID: 30071 RVA: 0x001DBF1D File Offset: 0x001DA31D
		[DebuggerHidden]
		public void Reset()
		{
			throw new NotSupportedException();
		}

		// Token: 0x0400698F RID: 27023
		internal UnityAnimatorControl $this;

		// Token: 0x04006990 RID: 27024
		internal object $current;

		// Token: 0x04006991 RID: 27025
		internal bool $disposing;

		// Token: 0x04006992 RID: 27026
		internal int $PC;
	}

	// Token: 0x02000FD9 RID: 4057
	[CompilerGenerated]
	private sealed class <AddAnimationToSequence>c__AnonStorey1
	{
		// Token: 0x06007578 RID: 30072 RVA: 0x001DBF24 File Offset: 0x001DA324
		public <AddAnimationToSequence>c__AnonStorey1()
		{
		}

		// Token: 0x06007579 RID: 30073 RVA: 0x001DBF2C File Offset: 0x001DA32C
		internal void <>m__0()
		{
			this.$this.animationSequence.Remove(this.asc);
		}

		// Token: 0x0600757A RID: 30074 RVA: 0x001DBF48 File Offset: 0x001DA348
		internal void <>m__1()
		{
			LinkedListNode<AnimationSequenceClip> previous = this.newNode.Previous;
			if (previous != null)
			{
				this.$this.animationSequence.Remove(this.newNode);
				this.$this.animationSequence.AddBefore(previous, this.newNode);
				this.$this.SyncSequenceUIOrder();
			}
		}

		// Token: 0x0600757B RID: 30075 RVA: 0x001DBFA0 File Offset: 0x001DA3A0
		internal void <>m__2()
		{
			LinkedListNode<AnimationSequenceClip> next = this.newNode.Next;
			if (next != null)
			{
				this.$this.animationSequence.Remove(this.newNode);
				this.$this.animationSequence.AddAfter(next, this.newNode);
				this.$this.SyncSequenceUIOrder();
			}
		}

		// Token: 0x04006993 RID: 27027
		internal AnimationSequenceClip asc;

		// Token: 0x04006994 RID: 27028
		internal LinkedListNode<AnimationSequenceClip> newNode;

		// Token: 0x04006995 RID: 27029
		internal UnityAnimatorControl $this;
	}

	// Token: 0x02000FDA RID: 4058
	[CompilerGenerated]
	private sealed class <AddAnimationToSequence>c__AnonStorey2
	{
		// Token: 0x0600757C RID: 30076 RVA: 0x001DBFF7 File Offset: 0x001DA3F7
		public <AddAnimationToSequence>c__AnonStorey2()
		{
		}

		// Token: 0x0600757D RID: 30077 RVA: 0x001DBFFF File Offset: 0x001DA3FF
		internal void <>m__0()
		{
			this.<>f__ref$1.$this.animationSequence.Remove(this.<>f__ref$1.asc);
			UnityEngine.Object.Destroy(this.rt.gameObject);
		}

		// Token: 0x04006996 RID: 27030
		internal RectTransform rt;

		// Token: 0x04006997 RID: 27031
		internal UnityAnimatorControl.<AddAnimationToSequence>c__AnonStorey1 <>f__ref$1;
	}
}
