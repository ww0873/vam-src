using System;
using System.Collections.Generic;
using SimpleJSON;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

// Token: 0x02000B5F RID: 2911
public class AnimationStep : CubicBezierPoint, TriggerHandler
{
	// Token: 0x06005138 RID: 20792 RVA: 0x001D4BE7 File Offset: 0x001D2FE7
	public AnimationStep()
	{
	}

	// Token: 0x06005139 RID: 20793 RVA: 0x001D4C1C File Offset: 0x001D301C
	public override JSONClass GetJSON(bool includePhysical = true, bool includeAppearance = true, bool forceStore = false)
	{
		JSONClass json = base.GetJSON(includePhysical, includeAppearance, forceStore);
		if ((includePhysical || forceStore) && this.trigger != null)
		{
			this.needsStore = true;
			json["trigger"] = this.trigger.GetJSON(base.subScenePrefix);
		}
		return json;
	}

	// Token: 0x0600513A RID: 20794 RVA: 0x001D4C70 File Offset: 0x001D3070
	public override void LateRestoreFromJSON(JSONClass jc, bool restorePhysical = true, bool restoreAppearance = true, bool setMissingToDefault = true)
	{
		base.LateRestoreFromJSON(jc, restorePhysical, restoreAppearance, setMissingToDefault);
		if (!base.physicalLocked && restorePhysical && !base.IsCustomPhysicalParamLocked("trigger"))
		{
			if (jc["trigger"] != null)
			{
				JSONClass asObject = jc["trigger"].AsObject;
				if (asObject != null)
				{
					this.trigger.RestoreFromJSON(asObject, base.subScenePrefix, base.mergeRestore);
				}
			}
			else if (setMissingToDefault)
			{
				this.trigger.RestoreFromJSON(new JSONClass());
			}
		}
	}

	// Token: 0x0600513B RID: 20795 RVA: 0x001D4D10 File Offset: 0x001D3110
	public override void Validate()
	{
		base.Validate();
		if (this.trigger != null)
		{
			this.trigger.Validate();
		}
	}

	// Token: 0x0600513C RID: 20796 RVA: 0x001D4D2E File Offset: 0x001D312E
	protected void OnAtomRename(string oldid, string newid)
	{
		if (this.trigger != null)
		{
			this.trigger.SyncAtomNames();
		}
	}

	// Token: 0x0600513D RID: 20797 RVA: 0x001D4D46 File Offset: 0x001D3146
	public virtual void RemoveTrigger(Trigger trigger)
	{
	}

	// Token: 0x0600513E RID: 20798 RVA: 0x001D4D48 File Offset: 0x001D3148
	public virtual void DuplicateTrigger(Trigger trigger)
	{
	}

	// Token: 0x0600513F RID: 20799 RVA: 0x001D4D4C File Offset: 0x001D314C
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

	// Token: 0x06005140 RID: 20800 RVA: 0x001D4D88 File Offset: 0x001D3188
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

	// Token: 0x06005141 RID: 20801 RVA: 0x001D4DC4 File Offset: 0x001D31C4
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

	// Token: 0x06005142 RID: 20802 RVA: 0x001D4E00 File Offset: 0x001D3200
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

	// Token: 0x06005143 RID: 20803 RVA: 0x001D4E3C File Offset: 0x001D323C
	public void RemoveTriggerActionUI(RectTransform rt)
	{
		if (rt != null)
		{
			UnityEngine.Object.Destroy(rt.gameObject);
		}
	}

	// Token: 0x17000BCF RID: 3023
	// (get) Token: 0x06005144 RID: 20804 RVA: 0x001D4E55 File Offset: 0x001D3255
	// (set) Token: 0x06005145 RID: 20805 RVA: 0x001D4E60 File Offset: 0x001D3260
	public int stepNumber
	{
		get
		{
			return this._stepNumber;
		}
		set
		{
			if (this._stepNumber != value)
			{
				this._stepNumber = value;
				if (this.stepNameText != null)
				{
					string str = string.Empty;
					if (this.animationParent != null)
					{
						str = this.animationParent.uid;
					}
					this.stepNameText.text = str + " Step " + this._stepNumber.ToString();
				}
			}
		}
	}

	// Token: 0x17000BD0 RID: 3024
	// (get) Token: 0x06005146 RID: 20806 RVA: 0x001D4EDB File Offset: 0x001D32DB
	// (set) Token: 0x06005147 RID: 20807 RVA: 0x001D4EE3 File Offset: 0x001D32E3
	public bool active
	{
		get
		{
			return this._active;
		}
		set
		{
			if (this._active != value)
			{
				this._active = value;
				if (this.trigger != null)
				{
					this.trigger.active = this._active;
				}
			}
		}
	}

	// Token: 0x17000BD1 RID: 3025
	// (get) Token: 0x06005148 RID: 20808 RVA: 0x001D4F14 File Offset: 0x001D3314
	// (set) Token: 0x06005149 RID: 20809 RVA: 0x001D4F1C File Offset: 0x001D331C
	public float stepRatio
	{
		get
		{
			return this._stepRatio;
		}
		set
		{
			if (this._stepRatio != value)
			{
				this._stepRatio = value;
				if (this.trigger != null)
				{
					this.trigger.transitionInterpValue = this._stepRatio;
				}
			}
		}
	}

	// Token: 0x0600514A RID: 20810 RVA: 0x001D4F4D File Offset: 0x001D334D
	protected void SyncTransitionToTime(float f)
	{
		this._transitionToTime = f;
		if (this.animationParent != null)
		{
			this.animationParent.RecalculateTimeSteps();
		}
	}

	// Token: 0x17000BD2 RID: 3026
	// (get) Token: 0x0600514B RID: 20811 RVA: 0x001D4F72 File Offset: 0x001D3372
	// (set) Token: 0x0600514C RID: 20812 RVA: 0x001D4F7A File Offset: 0x001D337A
	public float transitionToTime
	{
		get
		{
			return this._transitionToTime;
		}
		set
		{
			if (this.transitionToTimeJSON != null)
			{
				this.transitionToTimeJSON.val = value;
			}
			else if (this._transitionToTime != value)
			{
				this.SyncTransitionToTime(value);
			}
		}
	}

	// Token: 0x0600514D RID: 20813 RVA: 0x001D4FAC File Offset: 0x001D33AC
	protected void SyncCurveType()
	{
		AnimationStep.CurveType curveType = this._curveType;
		if (curveType != AnimationStep.CurveType.Linear)
		{
			if (curveType == AnimationStep.CurveType.EaseInOut)
			{
				this.curve = AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);
			}
		}
		else
		{
			this.curve = AnimationCurve.Linear(0f, 0f, 1f, 1f);
		}
	}

	// Token: 0x0600514E RID: 20814 RVA: 0x001D501C File Offset: 0x001D341C
	protected void SetCurveType(string type)
	{
		try
		{
			AnimationStep.CurveType curveType = (AnimationStep.CurveType)Enum.Parse(typeof(AnimationStep.CurveType), type);
			this._curveType = curveType;
			this.SyncCurveType();
		}
		catch (ArgumentException)
		{
			Debug.LogError("Attempted to set curve type to " + type + " which is not a valid curve type");
		}
	}

	// Token: 0x17000BD3 RID: 3027
	// (get) Token: 0x0600514F RID: 20815 RVA: 0x001D507C File Offset: 0x001D347C
	// (set) Token: 0x06005150 RID: 20816 RVA: 0x001D5084 File Offset: 0x001D3484
	public AnimationStep.CurveType curveType
	{
		get
		{
			return this._curveType;
		}
		set
		{
			if (this.curveTypeJSON != null)
			{
				this.curveTypeJSON.val = value.ToString();
			}
			else if (this._curveType != value)
			{
				this._curveType = value;
				this.SyncCurveType();
			}
		}
	}

	// Token: 0x06005151 RID: 20817 RVA: 0x001D50D2 File Offset: 0x001D34D2
	public void CreateStepBefore()
	{
		if (this.animationParent != null)
		{
			this.animationParent.CreateStepBeforeStep(this);
		}
	}

	// Token: 0x06005152 RID: 20818 RVA: 0x001D50F2 File Offset: 0x001D34F2
	public void CreateStepAfter()
	{
		if (this.animationParent != null)
		{
			this.animationParent.CreateStepAfterStep(this);
		}
	}

	// Token: 0x06005153 RID: 20819 RVA: 0x001D5114 File Offset: 0x001D3514
	public void DestroyStep()
	{
		if (this.animationParent != null)
		{
			this.DeregisterUI();
			this.animationParent.DestroyStep(this);
		}
		else if (this.containingAtom != null)
		{
			this.DeregisterUI();
			SuperController.singleton.RemoveAtom(this.containingAtom);
		}
	}

	// Token: 0x06005154 RID: 20820 RVA: 0x001D5170 File Offset: 0x001D3570
	public void AlignPositionToRoot()
	{
		if (this.animationParent != null && this.point != null)
		{
			if (this.fcv3 != null)
			{
				this.fcv3.MoveControl(this.animationParent.transform.position);
			}
			else
			{
				this.point.position = this.animationParent.transform.position;
			}
		}
	}

	// Token: 0x06005155 RID: 20821 RVA: 0x001D51EC File Offset: 0x001D35EC
	public void AlignRotationToRoot()
	{
		if (this.animationParent != null && this.point != null)
		{
			if (this.fcv3 != null)
			{
				this.fcv3.RotateControl(this.animationParent.transform.eulerAngles);
			}
			else
			{
				this.point.rotation = this.animationParent.transform.rotation;
			}
		}
	}

	// Token: 0x06005156 RID: 20822 RVA: 0x001D5268 File Offset: 0x001D3668
	public void AlignPositionToReceiver()
	{
		if (this.animationParent != null && this.animationParent.animatedTransform != null)
		{
			MoveProducer component = this.animationParent.animatedTransform.GetComponent<MoveProducer>();
			Transform transform;
			if (component != null && component.receiver != null)
			{
				transform = component.receiver.transform;
			}
			else
			{
				transform = this.animationParent.animatedTransform;
			}
			if (this.fcv3 != null)
			{
				this.fcv3.MoveControl(transform.position);
			}
			else
			{
				this.point.position = transform.position;
			}
		}
	}

	// Token: 0x06005157 RID: 20823 RVA: 0x001D5320 File Offset: 0x001D3720
	public void AlignRotationToReceiver()
	{
		if (this.animationParent != null && this.animationParent.animatedTransform != null)
		{
			MoveProducer component = this.animationParent.animatedTransform.GetComponent<MoveProducer>();
			Transform transform;
			if (component != null && component.receiver != null)
			{
				transform = component.receiver.transform;
			}
			else
			{
				transform = this.animationParent.animatedTransform;
			}
			if (this.fcv3 != null)
			{
				this.fcv3.RotateControl(transform.eulerAngles);
			}
			else
			{
				this.point.rotation = transform.rotation;
			}
		}
	}

	// Token: 0x06005158 RID: 20824 RVA: 0x001D53D8 File Offset: 0x001D37D8
	public override void InitUI()
	{
		if (this.UITransform != null)
		{
			AnimationStepUI componentInChildren = this.UITransform.GetComponentInChildren<AnimationStepUI>(true);
			if (componentInChildren != null)
			{
				this.transitionToTimeJSON.slider = componentInChildren.transitionToTimeSlider;
				this.curveTypeJSON.popup = componentInChildren.curveTypePopup;
			}
			if (componentInChildren.createStepBeforeButton != null)
			{
				componentInChildren.createStepBeforeButton.onClick.AddListener(new UnityAction(this.CreateStepBefore));
			}
			if (componentInChildren.createStepAfterButton != null)
			{
				componentInChildren.createStepAfterButton.onClick.AddListener(new UnityAction(this.CreateStepAfter));
			}
			if (componentInChildren.alignPositionToRootButton != null)
			{
				componentInChildren.alignPositionToRootButton.onClick.AddListener(new UnityAction(this.AlignPositionToRoot));
			}
			if (componentInChildren.alignRotationToRootButton != null)
			{
				componentInChildren.alignRotationToRootButton.onClick.AddListener(new UnityAction(this.AlignRotationToRoot));
			}
			if (componentInChildren.alignPositionToReceiverButton != null)
			{
				componentInChildren.alignPositionToReceiverButton.onClick.AddListener(new UnityAction(this.AlignPositionToReceiver));
			}
			if (componentInChildren.alignRotationToReceiverButton != null)
			{
				componentInChildren.alignRotationToReceiverButton.onClick.AddListener(new UnityAction(this.AlignRotationToReceiver));
			}
			if (componentInChildren.removeStepButton != null)
			{
				componentInChildren.removeStepButton.onClick.AddListener(new UnityAction(this.DestroyStep));
			}
			AnimationStepTriggerUI componentInChildren2 = this.UITransform.GetComponentInChildren<AnimationStepTriggerUI>();
			if (componentInChildren2 != null)
			{
				this.trigger.triggerActionsParent = componentInChildren2.transform;
				this.trigger.triggerPanel = componentInChildren2.transform;
				this.trigger.triggerActionsPanel = componentInChildren2.transform;
				this.trigger.InitTriggerUI();
				this.trigger.InitTriggerActionsUI();
			}
		}
	}

	// Token: 0x06005159 RID: 20825 RVA: 0x001D55D0 File Offset: 0x001D39D0
	public override void InitUIAlt()
	{
		if (this.UITransformAlt != null)
		{
			AnimationStepUI componentInChildren = this.UITransformAlt.GetComponentInChildren<AnimationStepUI>(true);
			if (componentInChildren != null)
			{
				this.transitionToTimeJSON.sliderAlt = componentInChildren.transitionToTimeSlider;
				this.curveTypeJSON.popupAlt = componentInChildren.curveTypePopup;
			}
			if (componentInChildren.createStepBeforeButton != null)
			{
				componentInChildren.createStepBeforeButton.onClick.AddListener(new UnityAction(this.CreateStepBefore));
			}
			if (componentInChildren.createStepAfterButton != null)
			{
				componentInChildren.createStepAfterButton.onClick.AddListener(new UnityAction(this.CreateStepAfter));
			}
			if (componentInChildren.alignPositionToRootButton != null)
			{
				componentInChildren.alignPositionToRootButton.onClick.AddListener(new UnityAction(this.AlignPositionToRoot));
			}
			if (componentInChildren.alignRotationToRootButton != null)
			{
				componentInChildren.alignRotationToRootButton.onClick.AddListener(new UnityAction(this.AlignRotationToRoot));
			}
			if (componentInChildren.alignPositionToReceiverButton != null)
			{
				componentInChildren.alignPositionToReceiverButton.onClick.AddListener(new UnityAction(this.AlignPositionToReceiver));
			}
			if (componentInChildren.alignRotationToReceiverButton != null)
			{
				componentInChildren.alignRotationToReceiverButton.onClick.AddListener(new UnityAction(this.AlignRotationToReceiver));
			}
			if (componentInChildren.removeStepButton != null)
			{
				componentInChildren.removeStepButton.onClick.AddListener(new UnityAction(this.DestroyStep));
			}
		}
	}

	// Token: 0x0600515A RID: 20826 RVA: 0x001D5764 File Offset: 0x001D3B64
	protected void DeregisterUI()
	{
		this.transitionToTimeJSON.slider = null;
		this.transitionToTimeJSON.sliderAlt = null;
		this.curveTypeJSON.popup = null;
		this.curveTypeJSON.popupAlt = null;
		if (this.UITransform != null)
		{
			AnimationStepUI componentInChildren = this.UITransform.GetComponentInChildren<AnimationStepUI>();
			if (componentInChildren != null)
			{
				if (componentInChildren.createStepBeforeButton)
				{
					componentInChildren.createStepBeforeButton.onClick.RemoveAllListeners();
				}
				if (componentInChildren.createStepAfterButton)
				{
					componentInChildren.createStepAfterButton.onClick.RemoveAllListeners();
				}
				if (componentInChildren.alignPositionToRootButton)
				{
					componentInChildren.alignPositionToRootButton.onClick.RemoveAllListeners();
				}
				if (componentInChildren.alignRotationToRootButton)
				{
					componentInChildren.alignRotationToRootButton.onClick.RemoveAllListeners();
				}
				if (componentInChildren.alignPositionToReceiverButton != null)
				{
					componentInChildren.alignPositionToReceiverButton.onClick.RemoveAllListeners();
				}
				if (componentInChildren.alignRotationToReceiverButton != null)
				{
					componentInChildren.alignRotationToReceiverButton.onClick.RemoveAllListeners();
				}
				if (componentInChildren.removeStepButton)
				{
					componentInChildren.removeStepButton.onClick.RemoveAllListeners();
				}
			}
		}
		if (this.UITransformAlt != null)
		{
			AnimationStepUI componentInChildren2 = this.UITransformAlt.GetComponentInChildren<AnimationStepUI>();
			if (componentInChildren2 != null)
			{
				if (componentInChildren2.createStepBeforeButton)
				{
					componentInChildren2.createStepBeforeButton.onClick.RemoveAllListeners();
				}
				if (componentInChildren2.createStepAfterButton)
				{
					componentInChildren2.createStepAfterButton.onClick.RemoveAllListeners();
				}
				if (componentInChildren2.alignPositionToRootButton)
				{
					componentInChildren2.alignPositionToRootButton.onClick.RemoveAllListeners();
				}
				if (componentInChildren2.alignRotationToRootButton)
				{
					componentInChildren2.alignRotationToRootButton.onClick.RemoveAllListeners();
				}
				if (componentInChildren2.alignPositionToReceiverButton != null)
				{
					componentInChildren2.alignPositionToReceiverButton.onClick.RemoveAllListeners();
				}
				if (componentInChildren2.alignRotationToReceiverButton != null)
				{
					componentInChildren2.alignRotationToReceiverButton.onClick.RemoveAllListeners();
				}
				if (componentInChildren2.removeStepButton)
				{
					componentInChildren2.removeStepButton.onClick.RemoveAllListeners();
				}
			}
		}
	}

	// Token: 0x0600515B RID: 20827 RVA: 0x001D59B8 File Offset: 0x001D3DB8
	protected void Init()
	{
		if (this.point != null)
		{
			this.fcv3 = this.point.GetComponent<FreeControllerV3>();
		}
		this.trigger = new Trigger();
		this.trigger.handler = this;
		this.transitionToTimeJSON = new JSONStorableFloat("transitionToTime", this._transitionToTime, new JSONStorableFloat.SetFloatCallback(this.SyncTransitionToTime), 0f, 10f, false, true);
		this.transitionToTimeJSON.storeType = JSONStorableParam.StoreType.Physical;
		base.RegisterFloat(this.transitionToTimeJSON);
		string[] names = Enum.GetNames(typeof(AnimationStep.CurveType));
		List<string> choicesList = new List<string>(names);
		this.curveTypeJSON = new JSONStorableStringChooser("curveType", choicesList, this._curveType.ToString(), "Curve Type", new JSONStorableStringChooser.SetStringCallback(this.SetCurveType));
		this.curveTypeJSON.storeType = JSONStorableParam.StoreType.Physical;
		base.RegisterStringChooser(this.curveTypeJSON);
		if (SuperController.singleton)
		{
			SuperController singleton = SuperController.singleton;
			singleton.onAtomUIDRenameHandlers = (SuperController.OnAtomUIDRename)Delegate.Combine(singleton.onAtomUIDRenameHandlers, new SuperController.OnAtomUIDRename(this.OnAtomRename));
		}
	}

	// Token: 0x0600515C RID: 20828 RVA: 0x001D5ADB File Offset: 0x001D3EDB
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

	// Token: 0x0600515D RID: 20829 RVA: 0x001D5B00 File Offset: 0x001D3F00
	private void OnDestroy()
	{
		if (SuperController.singleton)
		{
			SuperController singleton = SuperController.singleton;
			singleton.onAtomUIDRenameHandlers = (SuperController.OnAtomUIDRename)Delegate.Remove(singleton.onAtomUIDRenameHandlers, new SuperController.OnAtomUIDRename(this.OnAtomRename));
		}
		if (this.animationParent != null)
		{
			this.animationParent.RemoveStep(this);
		}
	}

	// Token: 0x04004101 RID: 16641
	public Trigger trigger;

	// Token: 0x04004102 RID: 16642
	public RectTransform triggerActionsPrefab;

	// Token: 0x04004103 RID: 16643
	public RectTransform triggerActionMiniPrefab;

	// Token: 0x04004104 RID: 16644
	public RectTransform triggerActionDiscretePrefab;

	// Token: 0x04004105 RID: 16645
	public RectTransform triggerActionTransitionPrefab;

	// Token: 0x04004106 RID: 16646
	public AnimationPattern animationParent;

	// Token: 0x04004107 RID: 16647
	[SerializeField]
	protected int _stepNumber;

	// Token: 0x04004108 RID: 16648
	public Text stepNameText;

	// Token: 0x04004109 RID: 16649
	protected bool _active;

	// Token: 0x0400410A RID: 16650
	protected float _stepRatio;

	// Token: 0x0400410B RID: 16651
	public float timeStep;

	// Token: 0x0400410C RID: 16652
	protected JSONStorableFloat transitionToTimeJSON;

	// Token: 0x0400410D RID: 16653
	protected float _startingTransitionTime;

	// Token: 0x0400410E RID: 16654
	[SerializeField]
	protected float _transitionToTime = 1f;

	// Token: 0x0400410F RID: 16655
	protected JSONStorableStringChooser curveTypeJSON;

	// Token: 0x04004110 RID: 16656
	[SerializeField]
	protected AnimationStep.CurveType _curveType;

	// Token: 0x04004111 RID: 16657
	public AnimationCurve curve = AnimationCurve.Linear(0f, 0f, 1f, 1f);

	// Token: 0x04004112 RID: 16658
	protected FreeControllerV3 fcv3;

	// Token: 0x02000B60 RID: 2912
	public enum CurveType
	{
		// Token: 0x04004114 RID: 16660
		Linear,
		// Token: 0x04004115 RID: 16661
		EaseInOut
	}
}
