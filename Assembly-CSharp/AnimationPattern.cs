using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using SimpleJSON;
using UnityEngine;
using UnityEngine.Events;

// Token: 0x02000B52 RID: 2898
public class AnimationPattern : CubicBezierCurve, AnimationTimelineTriggerHandler, TriggerHandler
{
	// Token: 0x060050BA RID: 20666 RVA: 0x001D1F5C File Offset: 0x001D035C
	public AnimationPattern()
	{
	}

	// Token: 0x060050BB RID: 20667 RVA: 0x001D1FAC File Offset: 0x001D03AC
	public override string[] GetCustomParamNames()
	{
		return this.customParamNamesOverride;
	}

	// Token: 0x060050BC RID: 20668 RVA: 0x001D1FB4 File Offset: 0x001D03B4
	public override JSONClass GetJSON(bool includePhysical = true, bool includeAppearance = true, bool forceStore = false)
	{
		JSONClass json = base.GetJSON(includePhysical, includeAppearance, forceStore);
		if (includePhysical || forceStore)
		{
			this.ResetAnimation();
			if (this.steps != null)
			{
				this.needsStore = true;
				JSONArray jsonarray = new JSONArray();
				for (int i = 0; i < this.steps.Length; i++)
				{
					jsonarray[i] = base.AtomUidToStoreAtomUid(this.steps[i].containingAtom.uid);
					SuperController.singleton.SaveAddDependency(this.steps[i].containingAtom);
				}
				json["steps"] = jsonarray;
			}
			if (this.triggers != null)
			{
				this.needsStore = true;
				JSONArray jsonarray2 = new JSONArray();
				json["triggers"] = jsonarray2;
				foreach (AnimationTimelineTrigger animationTimelineTrigger in this.triggers)
				{
					jsonarray2.Add(animationTimelineTrigger.GetJSON(base.subScenePrefix));
				}
			}
		}
		return json;
	}

	// Token: 0x060050BD RID: 20669 RVA: 0x001D20D8 File Offset: 0x001D04D8
	public override void PreRestore(bool restorePhysical, bool restoreAppearance)
	{
		if (restorePhysical && !base.physicalLocked)
		{
			if (!base.IsCustomPhysicalParamLocked("steps") && (this.containingAtom == null || !this.containingAtom.isSubSceneRestore))
			{
				this.DestroyAllSteps();
			}
			this.isPlaying = false;
		}
	}

	// Token: 0x060050BE RID: 20670 RVA: 0x001D2134 File Offset: 0x001D0534
	public override void RestoreFromJSON(JSONClass jc, bool restorePhysical = true, bool restoreAppearance = true, JSONArray presetAtoms = null, bool setMissingToDefault = true)
	{
		this.isRestoring = true;
		base.RestoreFromJSON(jc, restorePhysical, restoreAppearance, presetAtoms, setMissingToDefault);
		if (!base.physicalLocked && restorePhysical)
		{
			if (presetAtoms != null)
			{
				this._presetAnimationStepUIDMap = new Dictionary<string, string>();
				for (int i = 0; i < presetAtoms.Count; i++)
				{
					JSONClass asObject = presetAtoms[i].AsObject;
					string a = asObject["type"];
					if (a == "AnimationStep")
					{
						string key = asObject["id"];
						if (this.animationStepPrefab != null)
						{
							Transform transform = SuperController.singleton.AddAtom(this.animationStepPrefab, null, false, false, false, true);
							Atom component = transform.GetComponent<Atom>();
							component.RestoreTransform(asObject, true);
							if (this.containingAtom != null && component != null)
							{
								component.parentAtom = this.containingAtom;
							}
							else
							{
								transform.SetParent(base.transform, true);
							}
							string uid = component.uid;
							this._presetAnimationStepUIDMap.Add(key, uid);
							component.Restore(asObject, restorePhysical, restoreAppearance, true, null, false, false, true, false);
							component.LateRestore(asObject, restorePhysical, restoreAppearance, true, false, true, false);
						}
					}
				}
			}
			if (!base.IsCustomPhysicalParamLocked("steps"))
			{
				if (jc["steps"] != null)
				{
					JSONArray asArray = jc["steps"].AsArray;
					List<AnimationStep> list = new List<AnimationStep>();
					for (int j = 0; j < asArray.Count; j++)
					{
						string text;
						if (presetAtoms != null)
						{
							if (!this._presetAnimationStepUIDMap.TryGetValue(asArray[j], out text))
							{
								text = asArray[j];
							}
						}
						else
						{
							string text2 = base.StoredAtomUidToAtomUid(asArray[j]);
							text = text2;
						}
						Atom atomByUid = SuperController.singleton.GetAtomByUid(text);
						if (atomByUid != null)
						{
							if (atomByUid.animationSteps != null)
							{
								AnimationStep animationStep = atomByUid.animationSteps[0];
								animationStep.animationParent = this;
								list.Add(animationStep);
							}
							else
							{
								Debug.LogError("Atom " + text + " does not contain an AnimationStep component");
							}
						}
						else
						{
							SuperController.LogError(string.Concat(new string[]
							{
								"Atom ",
								text,
								" referenced by animation pattern ",
								this.uid,
								" does not exist"
							}));
						}
					}
					this.steps = list.ToArray();
				}
				else if (setMissingToDefault)
				{
					this.steps = new AnimationStep[0];
				}
			}
			this.isPlaying = this.autoPlayJSON.val;
		}
		this.isRestoring = false;
	}

	// Token: 0x060050BF RID: 20671 RVA: 0x001D2410 File Offset: 0x001D0810
	public override void LateRestoreFromJSON(JSONClass jc, bool restorePhysical = true, bool restoreAppearance = true, bool setMissingToDefault = true)
	{
		this.isRestoring = true;
		base.LateRestoreFromJSON(jc, restorePhysical, restoreAppearance, setMissingToDefault);
		if (!base.physicalLocked && restorePhysical)
		{
			if (!base.IsCustomPhysicalParamLocked("triggers"))
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
				else if (setMissingToDefault)
				{
					this.ClearTriggers();
				}
			}
			this.ResetAnimation();
		}
		this.isRestoring = false;
	}

	// Token: 0x060050C0 RID: 20672 RVA: 0x001D2534 File Offset: 0x001D0934
	public override void Validate()
	{
		base.Validate();
		foreach (Trigger trigger in this.triggers)
		{
			trigger.Validate();
		}
	}

	// Token: 0x060050C1 RID: 20673 RVA: 0x001D2598 File Offset: 0x001D0998
	public override void Remove()
	{
		this.DestroyAllSteps();
	}

	// Token: 0x060050C2 RID: 20674 RVA: 0x001D25A0 File Offset: 0x001D09A0
	protected void SyncOn(bool b)
	{
		if (this.animatedTransform != null)
		{
			MoveProducer component = this.animatedTransform.GetComponent<MoveProducer>();
			if (component != null)
			{
				component.on = b;
			}
		}
		this.SetCurrentPositionAndRotation();
	}

	// Token: 0x060050C3 RID: 20675 RVA: 0x001D25E3 File Offset: 0x001D09E3
	protected void SyncPause(bool b)
	{
		if (b)
		{
			this._lastTimeStep = this.currentTimeJSON.val;
		}
	}

	// Token: 0x060050C4 RID: 20676 RVA: 0x001D25FC File Offset: 0x001D09FC
	protected void SyncAutoPlay(bool b)
	{
	}

	// Token: 0x060050C5 RID: 20677 RVA: 0x001D25FE File Offset: 0x001D09FE
	protected override void SyncLoop(bool val)
	{
		base.SyncLoop(val);
		this.RecalculateTimeSteps();
	}

	// Token: 0x060050C6 RID: 20678 RVA: 0x001D260D File Offset: 0x001D0A0D
	protected void SyncLoopOnce(bool b)
	{
		if (!this.loopOnceJSON.val && this.autoPlayJSON.val)
		{
			this.isPlaying = true;
		}
	}

	// Token: 0x060050C7 RID: 20679 RVA: 0x001D2636 File Offset: 0x001D0A36
	protected void SyncSpeed(float f)
	{
	}

	// Token: 0x060050C8 RID: 20680 RVA: 0x001D2638 File Offset: 0x001D0A38
	protected void SyncHideCurveUnlessSelected(bool b)
	{
		this._hideCurveUnlessSelected = b;
	}

	// Token: 0x17000BC4 RID: 3012
	// (get) Token: 0x060050C9 RID: 20681 RVA: 0x001D2641 File Offset: 0x001D0A41
	// (set) Token: 0x060050CA RID: 20682 RVA: 0x001D2660 File Offset: 0x001D0A60
	public bool hideCurveUnlessSelected
	{
		get
		{
			if (this.hideCurveUnlessSelectedJSON != null)
			{
				return this.hideCurveUnlessSelectedJSON.val;
			}
			return this._hideCurveUnlessSelected;
		}
		set
		{
			if (this.hideCurveUnlessSelectedJSON != null)
			{
				this.hideCurveUnlessSelectedJSON.val = value;
			}
			else if (this._hideCurveUnlessSelected != value)
			{
				this.SyncHideCurveUnlessSelected(value);
			}
		}
	}

	// Token: 0x060050CB RID: 20683 RVA: 0x001D2694 File Offset: 0x001D0A94
	public void HideAllSteps()
	{
		foreach (AnimationStep animationStep in this._steps)
		{
			animationStep.containingAtom.hiddenNoCallback = true;
		}
		SuperController.singleton.SyncHiddenAtoms();
	}

	// Token: 0x060050CC RID: 20684 RVA: 0x001D26D8 File Offset: 0x001D0AD8
	public void UnhideAllSteps()
	{
		foreach (AnimationStep animationStep in this._steps)
		{
			animationStep.containingAtom.hiddenNoCallback = false;
		}
		SuperController.singleton.SyncHiddenAtoms();
	}

	// Token: 0x060050CD RID: 20685 RVA: 0x001D271C File Offset: 0x001D0B1C
	public void ParentAllSteps()
	{
		if (this.containingAtom != null)
		{
			foreach (AnimationStep animationStep in this._steps)
			{
				if (animationStep.containingAtom != null)
				{
					animationStep.containingAtom.SelectAtomParent(this.containingAtom);
				}
			}
		}
	}

	// Token: 0x060050CE RID: 20686 RVA: 0x001D277C File Offset: 0x001D0B7C
	public void UnparentAllSteps()
	{
		foreach (AnimationStep animationStep in this._steps)
		{
			if (animationStep.containingAtom != null)
			{
				animationStep.containingAtom.SelectAtomParent(null);
			}
		}
	}

	// Token: 0x17000BC5 RID: 3013
	// (get) Token: 0x060050CF RID: 20687 RVA: 0x001D27C5 File Offset: 0x001D0BC5
	public string uid
	{
		get
		{
			if (this.containingAtom != null)
			{
				return this.containingAtom.uid;
			}
			return null;
		}
	}

	// Token: 0x17000BC6 RID: 3014
	// (get) Token: 0x060050D0 RID: 20688 RVA: 0x001D27E5 File Offset: 0x001D0BE5
	// (set) Token: 0x060050D1 RID: 20689 RVA: 0x001D27ED File Offset: 0x001D0BED
	public AnimationStep[] steps
	{
		get
		{
			return this._steps;
		}
		set
		{
			this._steps = value;
			base.points = value;
			this.SyncStepPositionsNames();
			this.ResetAnimation();
		}
	}

	// Token: 0x060050D2 RID: 20690 RVA: 0x001D280C File Offset: 0x001D0C0C
	public void SyncStepNames()
	{
		if (this._autoSyncStepNames && !this.isRestoring)
		{
			SuperController.singleton.PauseSyncAtomLists();
			foreach (AnimationStep animationStep in this._steps)
			{
				string b = this.containingAtom.uid + "Step" + animationStep.stepNumber;
				if (animationStep.containingAtom.uid != b)
				{
					animationStep.containingAtom.SetUID(SuperController.singleton.GetTempUID());
				}
			}
			foreach (AnimationStep animationStep2 in this._steps)
			{
				string text = this.containingAtom.uid + "Step" + animationStep2.stepNumber;
				if (animationStep2.containingAtom.uid != text)
				{
					SuperController.singleton.ReleaseTempUID(animationStep2.containingAtom.uid);
					animationStep2.containingAtom.SetUID(text);
				}
			}
			SuperController.singleton.ResumeSyncAtomLists();
		}
	}

	// Token: 0x060050D3 RID: 20691 RVA: 0x001D2935 File Offset: 0x001D0D35
	protected void SyncAutoSyncStepNames(bool b)
	{
		this._autoSyncStepNames = b;
		this.SyncStepNames();
	}

	// Token: 0x060050D4 RID: 20692 RVA: 0x001D2944 File Offset: 0x001D0D44
	protected void DrawRootLine()
	{
		if (this.rootLineDrawer != null && this._draw && this._steps.Length > 0)
		{
			this.rootLineDrawer.SetLinePoints(base.transform.position, this._steps[0].transform.position);
			this.rootLineDrawer.Draw(base.gameObject.layer);
		}
	}

	// Token: 0x060050D5 RID: 20693 RVA: 0x001D29B4 File Offset: 0x001D0DB4
	public AnimationStep CreateStepAtPosition(int position)
	{
		if (this.animationStepPrefab != null)
		{
			Transform transform = SuperController.singleton.AddAtom(this.animationStepPrefab, null, false, false, false, true);
			AnimationStep componentInChildren = transform.GetComponentInChildren<AnimationStep>();
			Atom component = transform.GetComponent<Atom>();
			if (this.containingAtom != null && component != null)
			{
				component.parentAtom = this.containingAtom;
			}
			else
			{
				transform.SetParent(base.transform, true);
			}
			transform.position = base.transform.position;
			transform.rotation = base.transform.rotation;
			if (this._steps.Length >= 2)
			{
				if (position == 0)
				{
					if (this._loop)
					{
						componentInChildren.point.position = base.GetPositionFromPoint(this._steps.Length - 1, 0.5f);
						componentInChildren.point.rotation = base.GetRotationFromPoint(this._steps.Length - 1, 0.5f);
					}
					else
					{
						Vector3 b;
						if (this._steps.Length > 1)
						{
							b = this._steps[0].point.position - this._steps[1].point.position;
						}
						else
						{
							b.x = 0f;
							b.y = 0f;
							b.z = 0f;
						}
						componentInChildren.point.position = this._steps[0].point.position + b;
						componentInChildren.point.rotation = this._steps[0].point.rotation;
					}
				}
				else if (position >= this._steps.Length)
				{
					if (this._loop)
					{
						componentInChildren.point.position = base.GetPositionFromPoint(this._steps.Length - 1, 0.5f);
						componentInChildren.point.rotation = base.GetRotationFromPoint(this._steps.Length - 1, 0.5f);
					}
					else
					{
						Vector3 b2;
						if (this._steps.Length > 1)
						{
							b2 = this._steps[this._steps.Length - 1].point.position - this._steps[this._steps.Length - 2].point.position;
						}
						else
						{
							b2.x = 0f;
							b2.y = 0f;
							b2.z = 0f;
						}
						componentInChildren.point.position = this._steps[this._steps.Length - 1].point.position + b2;
						componentInChildren.point.rotation = this._steps[this._steps.Length - 1].point.rotation;
					}
				}
				else
				{
					componentInChildren.point.position = base.GetPositionFromPoint(position, 0.5f);
					componentInChildren.point.rotation = base.GetRotationFromPoint(position, 0.5f);
				}
			}
			else if (this._steps.Length == 1)
			{
				Vector3 b3;
				b3.x = 0.1f;
				b3.y = 0f;
				b3.z = 0f;
				componentInChildren.point.position = transform.position + b3;
				componentInChildren.point.rotation = transform.rotation;
			}
			else
			{
				Vector3 b4;
				b4.x = 0.1f;
				b4.y = 0f;
				b4.z = 0f;
				componentInChildren.point.position = transform.position - b4;
				componentInChildren.point.rotation = transform.rotation;
			}
			componentInChildren.animationParent = this;
			this.AddStepAtPosition(componentInChildren, position);
			return componentInChildren;
		}
		return null;
	}

	// Token: 0x060050D6 RID: 20694 RVA: 0x001D2D7C File Offset: 0x001D117C
	public AnimationStep CreateStepBeforeStep(AnimationStep step)
	{
		int num = 0;
		foreach (AnimationStep x in this._steps)
		{
			if (x == step)
			{
				break;
			}
			num++;
		}
		return this.CreateStepAtPosition(num);
	}

	// Token: 0x060050D7 RID: 20695 RVA: 0x001D2DC8 File Offset: 0x001D11C8
	public AnimationStep CreateStepAfterStep(AnimationStep step)
	{
		int num = 0;
		foreach (AnimationStep x in this._steps)
		{
			if (x == step)
			{
				num++;
				break;
			}
			num++;
		}
		return this.CreateStepAtPosition(num);
	}

	// Token: 0x060050D8 RID: 20696 RVA: 0x001D2E16 File Offset: 0x001D1216
	public void CreateStepAtEnd()
	{
		this.CreateStepAtPosition(this._steps.Length);
	}

	// Token: 0x060050D9 RID: 20697 RVA: 0x001D2E28 File Offset: 0x001D1228
	public void DestroyAllSteps()
	{
		bool autoSyncStepNames = this._autoSyncStepNames;
		this._autoSyncStepNames = false;
		foreach (AnimationStep step in this._steps)
		{
			this.DestroyStep(step);
		}
		this.steps = new AnimationStep[0];
		this._autoSyncStepNames = autoSyncStepNames;
	}

	// Token: 0x060050DA RID: 20698 RVA: 0x001D2E7C File Offset: 0x001D127C
	public void DestroyStep(AnimationStep step)
	{
		this.RemoveStep(step);
		if (Application.isPlaying)
		{
			if (step.containingAtom != null)
			{
				SuperController.singleton.RemoveAtom(step.containingAtom);
			}
			else
			{
				UnityEngine.Object.DestroyImmediate(step.gameObject);
			}
		}
		else
		{
			UnityEngine.Object.DestroyImmediate(step.gameObject);
		}
	}

	// Token: 0x060050DB RID: 20699 RVA: 0x001D2EDC File Offset: 0x001D12DC
	public void AddStepAtPosition(AnimationStep step, int position)
	{
		List<AnimationStep> list = new List<AnimationStep>();
		int num = 0;
		bool flag = false;
		bool flag2 = true;
		foreach (AnimationStep item in this._steps)
		{
			flag2 = false;
			if (num == position)
			{
				flag = true;
				list.Add(step);
			}
			list.Add(item);
			num++;
		}
		if (!flag)
		{
			list.Add(step);
			if (flag2 && this.animatedTransform != null)
			{
				this.animatedTransform.position = step.point.position;
				this.animatedTransform.rotation = step.point.rotation;
			}
		}
		this.steps = list.ToArray();
	}

	// Token: 0x060050DC RID: 20700 RVA: 0x001D2F98 File Offset: 0x001D1398
	public void SyncStepPositionsNames()
	{
		int num = 1;
		foreach (AnimationStep animationStep in this._steps)
		{
			animationStep.stepNumber = num;
			num++;
		}
		this.RecalculateTimeSteps();
		this.SyncStepNames();
	}

	// Token: 0x060050DD RID: 20701 RVA: 0x001D2FDD File Offset: 0x001D13DD
	public void AddStepAtEnd(AnimationStep step)
	{
		this.AddStepAtPosition(step, this._steps.Length);
	}

	// Token: 0x060050DE RID: 20702 RVA: 0x001D2FF0 File Offset: 0x001D13F0
	public void RemoveStep(AnimationStep step)
	{
		if (this._currentStep == step)
		{
			this._currentStep = null;
			this.activeStep = this._currentStep;
		}
		List<AnimationStep> list = new List<AnimationStep>();
		foreach (AnimationStep animationStep in this._steps)
		{
			if (animationStep != step)
			{
				list.Add(animationStep);
			}
		}
		if (base.gameObject != null)
		{
			this.steps = list.ToArray();
		}
	}

	// Token: 0x060050DF RID: 20703 RVA: 0x001D3076 File Offset: 0x001D1476
	public void Play()
	{
		this.pauseJSON.val = false;
		this.isPlaying = true;
	}

	// Token: 0x060050E0 RID: 20704 RVA: 0x001D308C File Offset: 0x001D148C
	public void ResetAnimation()
	{
		this._disableTriggers = true;
		this.currentTimeJSON.val = 0f;
		if (this._currentStep == null)
		{
			this.SeekToTimeStep(0f);
		}
		this._lastTimeStep = -0.1f;
		this._disableTriggers = false;
		this.isPlaying = this.autoPlayJSON.val;
		if (this.steps.Length > 0 && this.animatedTransform != null && this._steps[0] != null && this._steps[0].point != null)
		{
			this.animatedTransform.position = this._steps[0].point.position;
			this.animatedTransform.rotation = this._steps[0].point.rotation;
		}
		List<AnimationTimelineTrigger> list = new List<AnimationTimelineTrigger>(this.triggers);
		List<AnimationTimelineTrigger> list2 = list;
		if (AnimationPattern.<>f__am$cache0 == null)
		{
			AnimationPattern.<>f__am$cache0 = new Comparison<AnimationTimelineTrigger>(AnimationPattern.<ResetAnimation>m__0);
		}
		list2.Sort(AnimationPattern.<>f__am$cache0);
		foreach (AnimationTimelineTrigger animationTimelineTrigger in list)
		{
			animationTimelineTrigger.Reset();
		}
	}

	// Token: 0x060050E1 RID: 20705 RVA: 0x001D31EC File Offset: 0x001D15EC
	public void ResetAndPlay()
	{
		this.ResetAnimation();
		this.Play();
	}

	// Token: 0x060050E2 RID: 20706 RVA: 0x001D31FA File Offset: 0x001D15FA
	public void SmoothResetAnimation()
	{
	}

	// Token: 0x060050E3 RID: 20707 RVA: 0x001D31FC File Offset: 0x001D15FC
	public void Pause()
	{
		this.pauseJSON.val = true;
	}

	// Token: 0x060050E4 RID: 20708 RVA: 0x001D320A File Offset: 0x001D160A
	public void UnPause()
	{
		this.pauseJSON.val = false;
	}

	// Token: 0x060050E5 RID: 20709 RVA: 0x001D3218 File Offset: 0x001D1618
	public void TogglePause()
	{
		this.pauseJSON.val = !this.pauseJSON.val;
	}

	// Token: 0x17000BC7 RID: 3015
	// (set) Token: 0x060050E6 RID: 20710 RVA: 0x001D3234 File Offset: 0x001D1634
	protected AnimationStep activeStep
	{
		set
		{
			if (this._activeStep != value)
			{
				if (this._activeStep != null)
				{
					this._activeStep.active = false;
				}
				this._activeStep = value;
				if (this._activeStep != null)
				{
					this._activeStep.active = true;
				}
			}
		}
	}

	// Token: 0x17000BC8 RID: 3016
	// (get) Token: 0x060050E7 RID: 20711 RVA: 0x001D3293 File Offset: 0x001D1693
	// (set) Token: 0x060050E8 RID: 20712 RVA: 0x001D329B File Offset: 0x001D169B
	protected float currentStepToNextStepRatio
	{
		get
		{
			return this._currentStepToNextStepRatio;
		}
		set
		{
			if (this._currentStepToNextStepRatio != value)
			{
				this._currentStepToNextStepRatio = value;
				if (this._currentStep != null)
				{
					this._currentStep.stepRatio = value;
				}
			}
		}
	}

	// Token: 0x060050E9 RID: 20713 RVA: 0x001D32CD File Offset: 0x001D16CD
	public float GetCurrentTimeCounter()
	{
		if (this.currentTimeJSON != null)
		{
			return this.currentTimeJSON.val;
		}
		return 0f;
	}

	// Token: 0x060050EA RID: 20714 RVA: 0x001D32EB File Offset: 0x001D16EB
	public float GetTotalTime()
	{
		if (this.currentTimeJSON != null)
		{
			return this.currentTimeJSON.max;
		}
		return 0f;
	}

	// Token: 0x060050EB RID: 20715 RVA: 0x001D330C File Offset: 0x001D170C
	public void RecalculateTimeSteps()
	{
		float num = 0f;
		for (int i = 0; i < this._steps.Length; i++)
		{
			if (i != 0)
			{
				num += this._steps[i].transitionToTime;
			}
			this._steps[i].timeStep = num;
		}
		if (this._loop && this._steps.Length > 0)
		{
			num += this._steps[0].transitionToTime;
		}
		if (this.currentTimeJSON != null)
		{
			this.currentTimeJSON.max = num;
		}
		foreach (AnimationTimelineTrigger animationTimelineTrigger in this.triggers)
		{
			animationTimelineTrigger.ResyncMaxStartAndEndTimes();
		}
	}

	// Token: 0x060050EC RID: 20716 RVA: 0x001D33EC File Offset: 0x001D17EC
	protected void SeekToTimeStep(float timeStep)
	{
		if ((this._currentStep == null || timeStep == 0f) && this._steps.Length > 0)
		{
			this._currentStep = this._steps[0];
			if (this._steps.Length > 1)
			{
				this._nextStep = this._steps[1];
			}
			else
			{
				this._nextStep = null;
			}
		}
		if (this._currentStep != null)
		{
			int num = this._currentStep.stepNumber - 1;
			bool flag = false;
			while (!flag)
			{
				flag = true;
				if (timeStep < this._currentStep.timeStep)
				{
					if (num > 0)
					{
						this._nextStep = this._currentStep;
						num--;
						this._currentStep = this._steps[num];
						flag = false;
					}
				}
				else if (this._nextStep != null && timeStep >= this._nextStep.timeStep)
				{
					this._currentStep = this._nextStep;
					num++;
					if (num < this._steps.Length)
					{
						this._nextStep = this._steps[num];
						flag = false;
					}
					else
					{
						this._nextStep = null;
					}
				}
			}
			if (this._nextStep == null)
			{
				if (this._loop)
				{
					float transitionToTime = this.steps[0].transitionToTime;
					float num2 = timeStep - this._currentStep.timeStep;
					this.currentStepToNextStepRatio = Mathf.Clamp01(num2 / transitionToTime);
				}
				else
				{
					this.currentStepToNextStepRatio = 0f;
				}
			}
			else
			{
				float num3 = this._nextStep.timeStep - this._currentStep.timeStep;
				float num4 = timeStep - this._currentStep.timeStep;
				this.currentStepToNextStepRatio = Mathf.Clamp01(num4 / num3);
			}
		}
		this.activeStep = this._currentStep;
		float lastTimeStep = this._lastTimeStep;
		this._lastTimeStep = timeStep;
		bool disableTriggers = this._disableTriggers;
		this._disableTriggers = false;
		bool autoCounter = this._autoCounter;
		this._autoCounter = false;
		this.SetCurrentPositionAndRotation();
		if (!disableTriggers)
		{
			bool flag2 = (autoCounter && this._autoReverse) || (!autoCounter && lastTimeStep > timeStep);
			if (flag2)
			{
				foreach (AnimationTimelineTrigger animationTimelineTrigger in this.reverseTriggers)
				{
					animationTimelineTrigger.Update(flag2, lastTimeStep);
				}
			}
			else
			{
				foreach (AnimationTimelineTrigger animationTimelineTrigger2 in this.triggers)
				{
					animationTimelineTrigger2.Update(flag2, lastTimeStep);
				}
			}
		}
	}

	// Token: 0x060050ED RID: 20717 RVA: 0x001D36D4 File Offset: 0x001D1AD4
	protected void SetCurrentPositionAndRotation()
	{
		if (this.onJSON.val && this._currentStep != null)
		{
			if (this._nextStep == null)
			{
				if (this._loop)
				{
					float t = this._currentStep.curve.Evaluate(this._currentStepToNextStepRatio);
					this.animatedTransform.position = base.GetPositionFromPoint(this._currentStep.stepNumber - 1, t);
					this.animatedTransform.rotation = base.GetRotationFromPoint(this._currentStep.stepNumber - 1, t);
				}
				else
				{
					this.animatedTransform.position = this._currentStep.point.position;
					this.animatedTransform.rotation = this._currentStep.point.rotation;
				}
			}
			else
			{
				float t2 = this._currentStep.curve.Evaluate(this._currentStepToNextStepRatio);
				this.animatedTransform.position = base.GetPositionFromPoint(this._currentStep.stepNumber - 1, t2);
				this.animatedTransform.rotation = base.GetRotationFromPoint(this._currentStep.stepNumber - 1, t2);
			}
		}
	}

	// Token: 0x060050EE RID: 20718 RVA: 0x001D3808 File Offset: 0x001D1C08
	protected void IncrementPlaybackCounter(float increment)
	{
		if (this.isPlaying && this.onJSON.val && !this.pauseJSON.val && (!SuperController.singleton || !SuperController.singleton.freezeAnimation))
		{
			this._autoCounter = true;
			this._autoReverse = (this.speedJSON.val < 0f);
			float num = this.currentTimeJSON.val + increment * this.speedJSON.val;
			if (num <= 0f)
			{
				if (this._loop)
				{
					if (this.loopOnceJSON.val)
					{
						this.currentTimeJSON.val = this.currentTimeJSON.max;
						this.isPlaying = false;
					}
					else
					{
						this.currentTimeJSON.val = this.currentTimeJSON.max + num;
					}
				}
				else
				{
					this.currentTimeJSON.val = this.currentTimeJSON.min;
					this.isPlaying = false;
				}
			}
			else if (num >= this.currentTimeJSON.max)
			{
				if (this._loop)
				{
					if (this.loopOnceJSON.val)
					{
						this.currentTimeJSON.val = this.currentTimeJSON.min;
						this.isPlaying = false;
					}
					else
					{
						this.currentTimeJSON.val = num - this.currentTimeJSON.val;
					}
				}
				else
				{
					this.currentTimeJSON.val = this.currentTimeJSON.max;
					this.isPlaying = false;
				}
			}
			else
			{
				this.currentTimeJSON.val = num;
			}
		}
	}

	// Token: 0x060050EF RID: 20719 RVA: 0x001D39B8 File Offset: 0x001D1DB8
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

	// Token: 0x060050F0 RID: 20720 RVA: 0x001D3A60 File Offset: 0x001D1E60
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
				Debug.LogError("Attempted to make TriggerUI when prefab was not set");
			}
		}
	}

	// Token: 0x060050F1 RID: 20721 RVA: 0x001D3AC0 File Offset: 0x001D1EC0
	protected AnimationTimelineTrigger AddTriggerInternal(int index = -1)
	{
		AnimationTimelineTrigger animationTimelineTrigger = new AnimationTimelineTrigger();
		animationTimelineTrigger.timeLineHandler = this;
		animationTimelineTrigger.handler = this;
		animationTimelineTrigger.doActionsInReverse = true;
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

	// Token: 0x060050F2 RID: 20722 RVA: 0x001D3B44 File Offset: 0x001D1F44
	public void AddTrigger()
	{
		this.AddTriggerInternal(-1);
	}

	// Token: 0x060050F3 RID: 20723 RVA: 0x001D3B50 File Offset: 0x001D1F50
	public RectTransform CreateTriggerActionsUI()
	{
		RectTransform result = null;
		if (this.triggerActionsPrefab != null)
		{
			result = UnityEngine.Object.Instantiate<RectTransform>(this.triggerActionsPrefab);
		}
		else
		{
			Debug.LogError("Attempted to make TriggerActionsUI when prefab was not set");
		}
		return result;
	}

	// Token: 0x060050F4 RID: 20724 RVA: 0x001D3B8C File Offset: 0x001D1F8C
	public RectTransform CreateTriggerActionMiniUI()
	{
		RectTransform result = null;
		if (this.triggerActionMiniPrefab != null)
		{
			result = UnityEngine.Object.Instantiate<RectTransform>(this.triggerActionMiniPrefab);
		}
		else
		{
			Debug.LogError("Attempted to make TriggerActionMiniUI when prefab was not set");
		}
		return result;
	}

	// Token: 0x060050F5 RID: 20725 RVA: 0x001D3BC8 File Offset: 0x001D1FC8
	public RectTransform CreateTriggerActionDiscreteUI()
	{
		RectTransform result = null;
		if (this.triggerActionDiscretePrefab != null)
		{
			result = UnityEngine.Object.Instantiate<RectTransform>(this.triggerActionDiscretePrefab);
		}
		else
		{
			Debug.LogError("Attempted to make TriggerActionDiscreteUI when prefab was not set");
		}
		return result;
	}

	// Token: 0x060050F6 RID: 20726 RVA: 0x001D3C04 File Offset: 0x001D2004
	public RectTransform CreateTriggerActionTransitionUI()
	{
		RectTransform result = null;
		if (this.triggerActionTransitionPrefab != null)
		{
			result = UnityEngine.Object.Instantiate<RectTransform>(this.triggerActionTransitionPrefab);
		}
		else
		{
			Debug.LogError("Attempted to make TriggerActionTransitionUI when prefab was not set");
		}
		return result;
	}

	// Token: 0x060050F7 RID: 20727 RVA: 0x001D3C40 File Offset: 0x001D2040
	public void RemoveTriggerActionUI(RectTransform rt)
	{
		if (rt != null)
		{
			UnityEngine.Object.Destroy(rt.gameObject);
		}
	}

	// Token: 0x060050F8 RID: 20728 RVA: 0x001D3C5C File Offset: 0x001D205C
	public void RemoveTrigger(Trigger trigger)
	{
		if (this.triggers.Remove(trigger as AnimationTimelineTrigger))
		{
			this.reverseTriggers.Remove(trigger as AnimationTimelineTrigger);
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
			Debug.LogError("Could not remove trigger " + trigger.displayName);
		}
	}

	// Token: 0x060050F9 RID: 20729 RVA: 0x001D3D24 File Offset: 0x001D2124
	public void DuplicateTrigger(Trigger trigger)
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

	// Token: 0x060050FA RID: 20730 RVA: 0x001D3D6C File Offset: 0x001D216C
	protected void OnAtomRename(string fromuid, string touid)
	{
		foreach (AnimationTimelineTrigger animationTimelineTrigger in this.triggers)
		{
			animationTimelineTrigger.SyncAtomNames();
		}
		if (touid == this.uid)
		{
			this.SyncStepNames();
		}
	}

	// Token: 0x060050FB RID: 20731 RVA: 0x001D3DE0 File Offset: 0x001D21E0
	protected override void Init()
	{
		base.Init();
		this.triggers = new List<AnimationTimelineTrigger>();
		this.reverseTriggers = new List<AnimationTimelineTrigger>();
		this.isPlaying = true;
		if (this.rootLineDrawerMaterial != null)
		{
			this.rootLineDrawer = new LineDrawer(this.rootLineDrawerMaterial);
		}
		this.onJSON = new JSONStorableBool("on", true, new JSONStorableBool.SetBoolCallback(this.SyncOn));
		this.onJSON.storeType = JSONStorableParam.StoreType.Physical;
		base.RegisterBool(this.onJSON);
		this.pauseJSON = new JSONStorableBool("pause", false, new JSONStorableBool.SetBoolCallback(this.SyncPause));
		this.pauseJSON.storeType = JSONStorableParam.StoreType.Physical;
		base.RegisterBool(this.pauseJSON);
		this.autoPlayJSON = new JSONStorableBool("autoPlay", true, new JSONStorableBool.SetBoolCallback(this.SyncAutoPlay));
		this.autoPlayJSON.storeType = JSONStorableParam.StoreType.Physical;
		base.RegisterBool(this.autoPlayJSON);
		this.loopOnceJSON = new JSONStorableBool("loopOnce", false, new JSONStorableBool.SetBoolCallback(this.SyncLoopOnce));
		this.loopOnceJSON.storeType = JSONStorableParam.StoreType.Physical;
		base.RegisterBool(this.loopOnceJSON);
		this.speedJSON = new JSONStorableFloat("speed", 1f, new JSONStorableFloat.SetFloatCallback(this.SyncSpeed), -10f, 10f, false, true);
		this.speedJSON.storeType = JSONStorableParam.StoreType.Physical;
		base.RegisterFloat(this.speedJSON);
		this.currentTimeJSON = new JSONStorableFloat("currentTime", 0f, new JSONStorableFloat.SetFloatCallback(this.SeekToTimeStep), 0f, 0f, true, true);
		this.currentTimeJSON.storeType = JSONStorableParam.StoreType.Physical;
		base.RegisterFloat(this.currentTimeJSON);
		this.autoSyncStepNamesJSON = new JSONStorableBool("autoSyncStepNames", this._autoSyncStepNames, new JSONStorableBool.SetBoolCallback(this.SyncAutoSyncStepNames));
		this.autoSyncStepNamesJSON.storeType = JSONStorableParam.StoreType.Physical;
		base.RegisterBool(this.autoSyncStepNamesJSON);
		this.hideCurveUnlessSelectedJSON = new JSONStorableBool("hideCurveUnlessSelected", this._hideCurveUnlessSelected, new JSONStorableBool.SetBoolCallback(this.SyncHideCurveUnlessSelected));
		this.hideCurveUnlessSelectedJSON.storeType = JSONStorableParam.StoreType.Physical;
		base.RegisterBool(this.hideCurveUnlessSelectedJSON);
		this.playJSONAction = new JSONStorableAction("Play", new JSONStorableAction.ActionCallback(this.Play));
		base.RegisterAction(this.playJSONAction);
		this.resetAnimationJSONAction = new JSONStorableAction("ResetAnimation", new JSONStorableAction.ActionCallback(this.ResetAnimation));
		base.RegisterAction(this.resetAnimationJSONAction);
		this.resetAndPlayJSONAction = new JSONStorableAction("ResetAndPlay", new JSONStorableAction.ActionCallback(this.ResetAndPlay));
		base.RegisterAction(this.resetAndPlayJSONAction);
		this.pauseJSONAction = new JSONStorableAction("Pause", new JSONStorableAction.ActionCallback(this.Pause));
		base.RegisterAction(this.pauseJSONAction);
		this.unPauseJSONAction = new JSONStorableAction("UnPause", new JSONStorableAction.ActionCallback(this.UnPause));
		base.RegisterAction(this.unPauseJSONAction);
		this.togglePauseJSONAction = new JSONStorableAction("TogglePause", new JSONStorableAction.ActionCallback(this.TogglePause));
		base.RegisterAction(this.togglePauseJSONAction);
		this.hideAllStepsJSONAction = new JSONStorableAction("HideAllSteps", new JSONStorableAction.ActionCallback(this.HideAllSteps));
		base.RegisterAction(this.hideAllStepsJSONAction);
		this.unhideAllStepsJSONAction = new JSONStorableAction("UnhideAllSteps", new JSONStorableAction.ActionCallback(this.UnhideAllSteps));
		base.RegisterAction(this.unhideAllStepsJSONAction);
		this.parentAllStepsJSONAction = new JSONStorableAction("ParentAllSteps", new JSONStorableAction.ActionCallback(this.ParentAllSteps));
		base.RegisterAction(this.parentAllStepsJSONAction);
		this.unparentAllStepsJSONAction = new JSONStorableAction("UnparentAllSteps", new JSONStorableAction.ActionCallback(this.UnparentAllSteps));
		base.RegisterAction(this.unparentAllStepsJSONAction);
		if (SuperController.singleton != null)
		{
			SuperController singleton = SuperController.singleton;
			singleton.onAtomUIDRenameHandlers = (SuperController.OnAtomUIDRename)Delegate.Combine(singleton.onAtomUIDRenameHandlers, new SuperController.OnAtomUIDRename(this.OnAtomRename));
		}
	}

	// Token: 0x060050FC RID: 20732 RVA: 0x001D41CC File Offset: 0x001D25CC
	public override void InitUI()
	{
		if (this.UITransform != null)
		{
			AnimationPatternUI componentInChildren = this.UITransform.GetComponentInChildren<AnimationPatternUI>(true);
			if (componentInChildren != null)
			{
				this.onJSON.toggle = componentInChildren.onToggle;
				this.autoPlayJSON.toggle = componentInChildren.autoPlayToggle;
				this.pauseJSON.toggle = componentInChildren.pauseToggle;
				this.loopJSON.toggle = componentInChildren.loopToggle;
				this.loopOnceJSON.toggle = componentInChildren.loopOnceToggle;
				this.curveTypeJSON.popup = componentInChildren.curveTypeSelector;
				this.speedJSON.slider = componentInChildren.speedSlider;
				this.autoSyncStepNamesJSON.toggle = componentInChildren.autoSyncStepNamesToggle;
				this.hideCurveUnlessSelectedJSON.toggle = componentInChildren.hideCurveUnlessSelectedToggle;
				this.currentTimeJSON.slider = componentInChildren.currentTimeSlider;
				this.triggerContentManager = componentInChildren.triggerContentManager;
				this.triggerActionsParent = componentInChildren.triggerActionsParent;
				for (int i = 0; i < this.triggers.Count; i++)
				{
					AnimationTimelineTrigger animationTimelineTrigger = this.triggers[i];
					this.CreateTriggerUI(animationTimelineTrigger, i);
					animationTimelineTrigger.InitTriggerUI();
					animationTimelineTrigger.triggerActionsParent = this.triggerActionsParent;
				}
				if (componentInChildren.createStepAtEndButton != null)
				{
					componentInChildren.createStepAtEndButton.onClick.AddListener(new UnityAction(this.CreateStepAtEnd));
				}
				this.resetAnimationJSONAction.button = componentInChildren.resetAnimationButton;
				this.playJSONAction.button = componentInChildren.playButton;
				if (componentInChildren.addTriggerButton != null)
				{
					componentInChildren.addTriggerButton.onClick.AddListener(new UnityAction(this.AddTrigger));
				}
				if (componentInChildren.clearAllTriggersButton != null)
				{
					componentInChildren.clearAllTriggersButton.onClick.AddListener(new UnityAction(this.ClearTriggers));
				}
				this.hideAllStepsJSONAction.button = componentInChildren.hideAllStepsButton;
				this.unhideAllStepsJSONAction.button = componentInChildren.unhideAllStepsButton;
				this.parentAllStepsJSONAction.button = componentInChildren.parentAllStepsButton;
				this.unparentAllStepsJSONAction.button = componentInChildren.unparentAllStepsButton;
			}
		}
	}

	// Token: 0x060050FD RID: 20733 RVA: 0x001D43F8 File Offset: 0x001D27F8
	public override void InitUIAlt()
	{
		if (this.UITransformAlt != null)
		{
			AnimationPatternUI componentInChildren = this.UITransformAlt.GetComponentInChildren<AnimationPatternUI>(true);
			if (componentInChildren != null)
			{
				this.onJSON.toggleAlt = componentInChildren.onToggle;
				this.autoPlayJSON.toggleAlt = componentInChildren.autoPlayToggle;
				this.pauseJSON.toggleAlt = componentInChildren.pauseToggle;
				this.loopJSON.toggleAlt = componentInChildren.loopToggle;
				this.loopOnceJSON.toggleAlt = componentInChildren.loopOnceToggle;
				this.curveTypeJSON.popupAlt = componentInChildren.curveTypeSelector;
				this.speedJSON.sliderAlt = componentInChildren.speedSlider;
				this.autoSyncStepNamesJSON.toggleAlt = componentInChildren.autoSyncStepNamesToggle;
				this.hideCurveUnlessSelectedJSON.toggleAlt = componentInChildren.hideCurveUnlessSelectedToggle;
				this.currentTimeJSON.sliderAlt = componentInChildren.currentTimeSlider;
				if (componentInChildren.createStepAtEndButton != null)
				{
					componentInChildren.createStepAtEndButton.onClick.AddListener(new UnityAction(this.CreateStepAtEnd));
				}
				this.resetAnimationJSONAction.button = componentInChildren.resetAnimationButton;
				this.playJSONAction.button = componentInChildren.playButton;
				if (componentInChildren.addTriggerButton != null)
				{
					componentInChildren.addTriggerButton.onClick.AddListener(new UnityAction(this.AddTrigger));
				}
				if (componentInChildren.clearAllTriggersButton != null)
				{
					componentInChildren.clearAllTriggersButton.onClick.AddListener(new UnityAction(this.ClearTriggers));
				}
				this.hideAllStepsJSONAction.buttonAlt = componentInChildren.hideAllStepsButton;
				this.unhideAllStepsJSONAction.buttonAlt = componentInChildren.unhideAllStepsButton;
				this.parentAllStepsJSONAction.buttonAlt = componentInChildren.parentAllStepsButton;
				this.unparentAllStepsJSONAction.buttonAlt = componentInChildren.unparentAllStepsButton;
			}
		}
	}

	// Token: 0x060050FE RID: 20734 RVA: 0x001D45C8 File Offset: 0x001D29C8
	protected void FixedUpdate()
	{
		if (Application.isPlaying)
		{
			float fixedTime = Time.fixedTime;
			if (this.lastTime == 0f)
			{
				this.lastTime = fixedTime;
			}
			float increment = fixedTime - this.lastTime;
			this.lastTime = fixedTime;
			this.IncrementPlaybackCounter(increment);
		}
	}

	// Token: 0x060050FF RID: 20735 RVA: 0x001D4614 File Offset: 0x001D2A14
	protected override void OnDestroy()
	{
		base.OnDestroy();
		this._autoSyncStepNames = false;
		if (SuperController.singleton != null)
		{
			SuperController singleton = SuperController.singleton;
			singleton.onAtomUIDRenameHandlers = (SuperController.OnAtomUIDRename)Delegate.Remove(singleton.onAtomUIDRenameHandlers, new SuperController.OnAtomUIDRename(this.OnAtomRename));
		}
	}

	// Token: 0x06005100 RID: 20736 RVA: 0x001D4664 File Offset: 0x001D2A64
	protected new void Update()
	{
		base.Update();
		if (Application.isPlaying)
		{
			float time = Time.time;
			if (this.lastTime == 0f)
			{
				this.lastTime = time;
			}
			float increment = time - this.lastTime;
			this.lastTime = time;
			this.IncrementPlaybackCounter(increment);
			foreach (AnimationTimelineTrigger animationTimelineTrigger in this.triggers)
			{
				animationTimelineTrigger.Update();
			}
			this.SetCurrentPositionAndRotation();
			this.DrawRootLine();
		}
	}

	// Token: 0x06005101 RID: 20737 RVA: 0x001D4710 File Offset: 0x001D2B10
	[CompilerGenerated]
	private static int <ResetAnimation>m__0(AnimationTimelineTrigger t1, AnimationTimelineTrigger t2)
	{
		return t2.triggerStartTime.CompareTo(t1.triggerStartTime);
	}

	// Token: 0x0400409E RID: 16542
	protected string[] customParamNamesOverride = new string[]
	{
		"curveType",
		"steps",
		"triggers"
	};

	// Token: 0x0400409F RID: 16543
	protected Dictionary<string, string> _presetAnimationStepUIDMap;

	// Token: 0x040040A0 RID: 16544
	protected bool isRestoring;

	// Token: 0x040040A1 RID: 16545
	public Transform animatedTransform;

	// Token: 0x040040A2 RID: 16546
	protected JSONStorableBool onJSON;

	// Token: 0x040040A3 RID: 16547
	protected JSONStorableBool pauseJSON;

	// Token: 0x040040A4 RID: 16548
	protected JSONStorableBool autoPlayJSON;

	// Token: 0x040040A5 RID: 16549
	protected JSONStorableBool loopOnceJSON;

	// Token: 0x040040A6 RID: 16550
	protected JSONStorableFloat speedJSON;

	// Token: 0x040040A7 RID: 16551
	protected bool _hideCurveUnlessSelected;

	// Token: 0x040040A8 RID: 16552
	protected JSONStorableBool hideCurveUnlessSelectedJSON;

	// Token: 0x040040A9 RID: 16553
	protected JSONStorableAction hideAllStepsJSONAction;

	// Token: 0x040040AA RID: 16554
	protected JSONStorableAction unhideAllStepsJSONAction;

	// Token: 0x040040AB RID: 16555
	protected JSONStorableAction parentAllStepsJSONAction;

	// Token: 0x040040AC RID: 16556
	protected JSONStorableAction unparentAllStepsJSONAction;

	// Token: 0x040040AD RID: 16557
	[SerializeField]
	protected AnimationStep[] _steps;

	// Token: 0x040040AE RID: 16558
	public Atom animationStepPrefab;

	// Token: 0x040040AF RID: 16559
	protected bool _autoSyncStepNames = true;

	// Token: 0x040040B0 RID: 16560
	public JSONStorableBool autoSyncStepNamesJSON;

	// Token: 0x040040B1 RID: 16561
	public LineDrawer rootLineDrawer;

	// Token: 0x040040B2 RID: 16562
	public Material rootLineDrawerMaterial;

	// Token: 0x040040B3 RID: 16563
	protected JSONStorableAction playJSONAction;

	// Token: 0x040040B4 RID: 16564
	protected JSONStorableAction resetAnimationJSONAction;

	// Token: 0x040040B5 RID: 16565
	protected JSONStorableAction resetAndPlayJSONAction;

	// Token: 0x040040B6 RID: 16566
	protected JSONStorableAction pauseJSONAction;

	// Token: 0x040040B7 RID: 16567
	protected JSONStorableAction unPauseJSONAction;

	// Token: 0x040040B8 RID: 16568
	protected JSONStorableAction togglePauseJSONAction;

	// Token: 0x040040B9 RID: 16569
	protected JSONStorableFloat currentTimeJSON;

	// Token: 0x040040BA RID: 16570
	protected AnimationStep _currentStep;

	// Token: 0x040040BB RID: 16571
	protected AnimationStep _activeStep;

	// Token: 0x040040BC RID: 16572
	protected AnimationStep _nextStep;

	// Token: 0x040040BD RID: 16573
	protected float _currentStepToNextStepRatio;

	// Token: 0x040040BE RID: 16574
	protected bool isPlaying = true;

	// Token: 0x040040BF RID: 16575
	protected float _lastTimeStep = -0.1f;

	// Token: 0x040040C0 RID: 16576
	protected bool _disableTriggers;

	// Token: 0x040040C1 RID: 16577
	protected bool _autoCounter;

	// Token: 0x040040C2 RID: 16578
	protected bool _autoReverse;

	// Token: 0x040040C3 RID: 16579
	protected List<AnimationTimelineTrigger> triggers;

	// Token: 0x040040C4 RID: 16580
	protected List<AnimationTimelineTrigger> reverseTriggers;

	// Token: 0x040040C5 RID: 16581
	public RectTransform triggerPrefab;

	// Token: 0x040040C6 RID: 16582
	public RectTransform triggerActionsPrefab;

	// Token: 0x040040C7 RID: 16583
	public RectTransform triggerActionMiniPrefab;

	// Token: 0x040040C8 RID: 16584
	public RectTransform triggerActionDiscretePrefab;

	// Token: 0x040040C9 RID: 16585
	public RectTransform triggerActionTransitionPrefab;

	// Token: 0x040040CA RID: 16586
	public RectTransform triggerActionsParent;

	// Token: 0x040040CB RID: 16587
	public ScrollRectContentManager triggerContentManager;

	// Token: 0x040040CC RID: 16588
	protected float lastTime;

	// Token: 0x040040CD RID: 16589
	[CompilerGenerated]
	private static Comparison<AnimationTimelineTrigger> <>f__am$cache0;
}
