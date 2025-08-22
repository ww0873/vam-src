using System;
using System.Collections.Generic;
using SimpleJSON;
using UnityEngine;

// Token: 0x02000D87 RID: 3463
public class RhythmForceProducerV2 : ForceProducerV2
{
	// Token: 0x06006AB8 RID: 27320 RVA: 0x00282FD0 File Offset: 0x002813D0
	public RhythmForceProducerV2()
	{
	}

	// Token: 0x06006AB9 RID: 27321 RVA: 0x00283062 File Offset: 0x00281462
	public override string[] GetCustomParamNames()
	{
		return this.customParamNamesOverride;
	}

	// Token: 0x06006ABA RID: 27322 RVA: 0x0028306C File Offset: 0x0028146C
	public override JSONClass GetJSON(bool includePhysical = true, bool includeAppearance = true, bool forceStore = false)
	{
		JSONClass json = base.GetJSON(includePhysical, includeAppearance, forceStore);
		if ((includePhysical || forceStore) && this._rhythmController != null && this._rhythmController.containingAtom != null)
		{
			string text = base.AtomUidToStoreAtomUid(this._rhythmController.containingAtom.uid);
			if (text != null)
			{
				this.needsStore = true;
				json["rhythmController"] = text + ":" + this._rhythmController.name;
			}
			else
			{
				SuperController.LogError(string.Concat(new object[]
				{
					"Warning: RhythmForceProducer in atom ",
					this.containingAtom,
					" uses rhythm controller in atom ",
					this._rhythmController.containingAtom.uid,
					" that is not in subscene and cannot be saved"
				}));
			}
		}
		return json;
	}

	// Token: 0x06006ABB RID: 27323 RVA: 0x0028314C File Offset: 0x0028154C
	public override void RestoreFromJSON(JSONClass jc, bool restorePhysical = true, bool restoreAppearance = true, JSONArray presetAtoms = null, bool setMissingToDefault = true)
	{
		base.RestoreFromJSON(jc, restorePhysical, restoreAppearance, presetAtoms, setMissingToDefault);
		if (!base.physicalLocked && restorePhysical && !base.IsCustomPhysicalParamLocked("rhythmController"))
		{
			if (jc["rhythmController"] != null)
			{
				string rhythmController = base.StoredAtomUidToAtomUid(jc["rhythmController"]);
				this.SetRhythmController(rhythmController);
			}
			else if (setMissingToDefault)
			{
				this.SetRhythmController(string.Empty);
			}
		}
	}

	// Token: 0x06006ABC RID: 27324 RVA: 0x002831D4 File Offset: 0x002815D4
	protected override void OnAtomUIDRename(string fromid, string toid)
	{
		base.OnAtomUIDRename(fromid, toid);
		if (this.rhythmController != null && this.rhythmControllerAtomSelectionPopup != null)
		{
			this.rhythmControllerAtomSelectionPopup.currentValueNoCallback = this.rhythmController.containingAtom.uid;
		}
	}

	// Token: 0x06006ABD RID: 27325 RVA: 0x00283228 File Offset: 0x00281628
	protected virtual void SetRhythmControllerAtomNames()
	{
		if (this.rhythmControllerAtomSelectionPopup != null && SuperController.singleton != null)
		{
			List<string> atomUIDsWithRhythmControllers = SuperController.singleton.GetAtomUIDsWithRhythmControllers();
			if (atomUIDsWithRhythmControllers == null)
			{
				this.rhythmControllerAtomSelectionPopup.numPopupValues = 1;
				this.rhythmControllerAtomSelectionPopup.setPopupValue(0, "None");
			}
			else
			{
				this.rhythmControllerAtomSelectionPopup.numPopupValues = atomUIDsWithRhythmControllers.Count + 1;
				this.rhythmControllerAtomSelectionPopup.setPopupValue(0, "None");
				for (int i = 0; i < atomUIDsWithRhythmControllers.Count; i++)
				{
					this.rhythmControllerAtomSelectionPopup.setPopupValue(i + 1, atomUIDsWithRhythmControllers[i]);
				}
			}
		}
	}

	// Token: 0x06006ABE RID: 27326 RVA: 0x002832DC File Offset: 0x002816DC
	protected virtual void onRhythmControllerNamesChanged(List<string> rcNames)
	{
		if (this.rhythmControllerSelectionPopup != null)
		{
			if (rcNames == null)
			{
				this.rhythmControllerSelectionPopup.numPopupValues = 1;
				this.rhythmControllerSelectionPopup.setPopupValue(0, "None");
			}
			else
			{
				this.rhythmControllerSelectionPopup.numPopupValues = rcNames.Count + 1;
				this.rhythmControllerSelectionPopup.setPopupValue(0, "None");
				for (int i = 0; i < rcNames.Count; i++)
				{
					this.rhythmControllerSelectionPopup.setPopupValue(i + 1, rcNames[i]);
				}
			}
		}
	}

	// Token: 0x06006ABF RID: 27327 RVA: 0x00283374 File Offset: 0x00281774
	public virtual void SetRhythmControllerAtom(string atomUID)
	{
		if (SuperController.singleton != null)
		{
			Atom atomByUid = SuperController.singleton.GetAtomByUid(atomUID);
			if (atomByUid != null)
			{
				this.rhythmControllerAtomUID = atomUID;
				List<string> rhythmControllerNamesInAtom = SuperController.singleton.GetRhythmControllerNamesInAtom(this.rhythmControllerAtomUID);
				this.onRhythmControllerNamesChanged(rhythmControllerNamesInAtom);
				if (this.rhythmControllerSelectionPopup != null)
				{
					this.rhythmControllerSelectionPopup.currentValue = "None";
				}
			}
			else
			{
				this.onRhythmControllerNamesChanged(null);
			}
		}
	}

	// Token: 0x06006AC0 RID: 27328 RVA: 0x002833F5 File Offset: 0x002817F5
	public virtual void SetRhythmControllerObject(string objectName)
	{
		if (this.rhythmControllerAtomUID != null && SuperController.singleton != null)
		{
			this.rhythmController = SuperController.singleton.RhythmControllerrNameToRhythmController(this.rhythmControllerAtomUID + ":" + objectName);
		}
	}

	// Token: 0x06006AC1 RID: 27329 RVA: 0x00283434 File Offset: 0x00281834
	public virtual void SetRhythmController(string controllerName)
	{
		if (SuperController.singleton != null)
		{
			RhythmController rhythmController = SuperController.singleton.RhythmControllerrNameToRhythmController(controllerName);
			if (rhythmController != null)
			{
				if (this.rhythmControllerAtomSelectionPopup != null && rhythmController.containingAtom != null)
				{
					this.rhythmControllerAtomSelectionPopup.currentValue = rhythmController.containingAtom.uid;
				}
				if (this.rhythmControllerSelectionPopup != null)
				{
					this.rhythmControllerSelectionPopup.currentValue = rhythmController.name;
				}
			}
			else
			{
				if (this.rhythmControllerAtomSelectionPopup != null)
				{
					this.rhythmControllerAtomSelectionPopup.currentValue = "None";
				}
				if (this.rhythmControllerSelectionPopup != null)
				{
					this.rhythmControllerSelectionPopup.currentValue = "None";
				}
			}
			this.rhythmController = rhythmController;
		}
	}

	// Token: 0x17000FAF RID: 4015
	// (get) Token: 0x06006AC2 RID: 27330 RVA: 0x00283511 File Offset: 0x00281911
	// (set) Token: 0x06006AC3 RID: 27331 RVA: 0x00283519 File Offset: 0x00281919
	public virtual RhythmController rhythmController
	{
		get
		{
			return this._rhythmController;
		}
		set
		{
			this._rhythmController = value;
		}
	}

	// Token: 0x06006AC4 RID: 27332 RVA: 0x00283522 File Offset: 0x00281922
	protected void SyncAlternateBeatRatio(float f)
	{
		this._alternateBeatRatio = f;
	}

	// Token: 0x17000FB0 RID: 4016
	// (get) Token: 0x06006AC5 RID: 27333 RVA: 0x0028352B File Offset: 0x0028192B
	// (set) Token: 0x06006AC6 RID: 27334 RVA: 0x00283533 File Offset: 0x00281933
	public float alternateBeatRatio
	{
		get
		{
			return this._alternateBeatRatio;
		}
		set
		{
			if (this.alternateBeatRatioJSON != null)
			{
				this.alternateBeatRatioJSON.val = value;
			}
			else if (this._alternateBeatRatio != value)
			{
				this.SyncAlternateBeatRatio(value);
			}
		}
	}

	// Token: 0x06006AC7 RID: 27335 RVA: 0x00283564 File Offset: 0x00281964
	protected void SyncThreshold(float f)
	{
		this._threshold = f;
	}

	// Token: 0x17000FB1 RID: 4017
	// (get) Token: 0x06006AC8 RID: 27336 RVA: 0x0028356D File Offset: 0x0028196D
	// (set) Token: 0x06006AC9 RID: 27337 RVA: 0x00283575 File Offset: 0x00281975
	public float threshold
	{
		get
		{
			return this._threshold;
		}
		set
		{
			if (this.thresholdJSON != null)
			{
				this.thresholdJSON.val = value;
			}
			else if (this._threshold != value)
			{
				this.SyncThreshold(value);
			}
		}
	}

	// Token: 0x06006ACA RID: 27338 RVA: 0x002835A6 File Offset: 0x002819A6
	protected void SyncBurstLength(float f)
	{
		this._burstLength = f;
	}

	// Token: 0x17000FB2 RID: 4018
	// (get) Token: 0x06006ACB RID: 27339 RVA: 0x002835AF File Offset: 0x002819AF
	// (set) Token: 0x06006ACC RID: 27340 RVA: 0x002835B7 File Offset: 0x002819B7
	public float burstLength
	{
		get
		{
			return this._burstLength;
		}
		set
		{
			if (this.burstLengthJSON != null)
			{
				this.burstLengthJSON.val = value;
			}
			else if (this._burstLength != value)
			{
				this.SyncBurstLength(value);
			}
		}
	}

	// Token: 0x06006ACD RID: 27341 RVA: 0x002835E8 File Offset: 0x002819E8
	protected void SyncMinSpacing(float f)
	{
		this._minSpacing = f;
	}

	// Token: 0x17000FB3 RID: 4019
	// (get) Token: 0x06006ACE RID: 27342 RVA: 0x002835F1 File Offset: 0x002819F1
	// (set) Token: 0x06006ACF RID: 27343 RVA: 0x002835F9 File Offset: 0x002819F9
	public float minSpacing
	{
		get
		{
			return this._minSpacing;
		}
		set
		{
			if (this.minSpacingJSON != null)
			{
				this.minSpacingJSON.val = value;
			}
			else if (this._minSpacing != value)
			{
				this.SyncMinSpacing(value);
			}
		}
	}

	// Token: 0x06006AD0 RID: 27344 RVA: 0x0028362A File Offset: 0x00281A2A
	protected void SyncRandomFactor(float f)
	{
		this._randomFactor = f;
	}

	// Token: 0x17000FB4 RID: 4020
	// (get) Token: 0x06006AD1 RID: 27345 RVA: 0x00283633 File Offset: 0x00281A33
	// (set) Token: 0x06006AD2 RID: 27346 RVA: 0x0028363B File Offset: 0x00281A3B
	public float randomFactor
	{
		get
		{
			return this._randomFactor;
		}
		set
		{
			if (this.randomFactorJSON != null)
			{
				this.randomFactorJSON.val = value;
			}
			else if (this._randomFactor != value)
			{
				this.SyncRandomFactor(value);
			}
		}
	}

	// Token: 0x06006AD3 RID: 27347 RVA: 0x0028366C File Offset: 0x00281A6C
	public void SetRangeSelect(string range)
	{
		try
		{
			this.rangeSelect = (RhythmForceProducerV2.RangeSelect)Enum.Parse(typeof(RhythmForceProducerV2.RangeSelect), range, true);
		}
		catch (ArgumentException)
		{
		}
	}

	// Token: 0x06006AD4 RID: 27348 RVA: 0x002836B0 File Offset: 0x00281AB0
	protected override void Init()
	{
		base.Init();
		if (this.rhythmController != null)
		{
			this.rhythmControllerAtomUID = this.rhythmController.containingAtom.uid;
		}
		string[] names = Enum.GetNames(typeof(RhythmForceProducerV2.RangeSelect));
		List<string> choicesList = new List<string>(names);
		this.rangeSelectJSON = new JSONStorableStringChooser("range", choicesList, this.rangeSelect.ToString(), "Pitch Range", new JSONStorableStringChooser.SetStringCallback(this.SetRangeSelect));
		this.rangeSelectJSON.storeType = JSONStorableParam.StoreType.Physical;
		base.RegisterStringChooser(this.rangeSelectJSON);
		this.alternateBeatRatioJSON = new JSONStorableFloat("alternateBeatRatio", this._alternateBeatRatio, new JSONStorableFloat.SetFloatCallback(this.SyncAlternateBeatRatio), 0f, 1f, true, true);
		this.alternateBeatRatioJSON.storeType = JSONStorableParam.StoreType.Physical;
		base.RegisterFloat(this.alternateBeatRatioJSON);
		this.burstLengthJSON = new JSONStorableFloat("burstLength", this._burstLength, new JSONStorableFloat.SetFloatCallback(this.SyncBurstLength), 0f, 10f, false, true);
		this.burstLengthJSON.storeType = JSONStorableParam.StoreType.Physical;
		base.RegisterFloat(this.burstLengthJSON);
		this.minSpacingJSON = new JSONStorableFloat("minSpacing", this._minSpacing, new JSONStorableFloat.SetFloatCallback(this.SyncMinSpacing), 0f, 10f, false, true);
		this.minSpacingJSON.storeType = JSONStorableParam.StoreType.Physical;
		base.RegisterFloat(this.minSpacingJSON);
		this.randomFactorJSON = new JSONStorableFloat("randomFactor", this._randomFactor, new JSONStorableFloat.SetFloatCallback(this.SyncRandomFactor), 0f, 1f, true, true);
		this.randomFactorJSON.storeType = JSONStorableParam.StoreType.Physical;
		base.RegisterFloat(this.randomFactorJSON);
		this.thresholdJSON = new JSONStorableFloat("threshold", this._threshold, new JSONStorableFloat.SetFloatCallback(this.SyncThreshold), 1f, 100f, true, true);
		this.thresholdJSON.storeType = JSONStorableParam.StoreType.Physical;
		base.RegisterFloat(this.thresholdJSON);
		if (this.rhythmLineMaterial)
		{
			this.rhythmLineDrawer = new LineDrawer(this.rhythmLineMaterial);
		}
		if (this.rawRhythmLineMaterial)
		{
			this.rawRhythmLineDrawer = new LineDrawer(this.rawRhythmLineMaterial);
		}
	}

	// Token: 0x06006AD5 RID: 27349 RVA: 0x002838EC File Offset: 0x00281CEC
	public override void InitUI()
	{
		base.InitUI();
		if (this.UITransform != null)
		{
			RhythmForceProducerV2UI componentInChildren = this.UITransform.GetComponentInChildren<RhythmForceProducerV2UI>(true);
			if (componentInChildren != null)
			{
				this.rangeSelectJSON.popup = componentInChildren.rangeSelectPopup;
				this.alternateBeatRatioJSON.slider = componentInChildren.alternateBeatRatioSlider;
				this.burstLengthJSON.slider = componentInChildren.burstLengthSlider;
				this.minSpacingJSON.slider = componentInChildren.minSpacingSlider;
				this.randomFactorJSON.slider = componentInChildren.randomFactorSlider;
				this.thresholdJSON.slider = componentInChildren.thresholdSlider;
				this.rhythmControllerAtomSelectionPopup = componentInChildren.rhythmControllerAtomSelectionPopup;
				if (this.rhythmControllerAtomSelectionPopup != null)
				{
					if (this.rhythmController != null)
					{
						if (this.rhythmController.containingAtom != null)
						{
							this.SetRhythmControllerAtom(this.rhythmController.containingAtom.uid);
							this.rhythmControllerAtomSelectionPopup.currentValue = this.rhythmController.containingAtom.uid;
						}
						else
						{
							this.rhythmControllerAtomSelectionPopup.currentValue = "None";
						}
					}
					else
					{
						this.rhythmControllerAtomSelectionPopup.currentValue = "None";
					}
					UIPopup uipopup = this.rhythmControllerAtomSelectionPopup;
					uipopup.onOpenPopupHandlers = (UIPopup.OnOpenPopup)Delegate.Combine(uipopup.onOpenPopupHandlers, new UIPopup.OnOpenPopup(this.SetRhythmControllerAtomNames));
					UIPopup uipopup2 = this.rhythmControllerAtomSelectionPopup;
					uipopup2.onValueChangeHandlers = (UIPopup.OnValueChange)Delegate.Combine(uipopup2.onValueChangeHandlers, new UIPopup.OnValueChange(this.SetRhythmControllerAtom));
				}
				this.rhythmControllerSelectionPopup = componentInChildren.rhythmControllerSelectionPopup;
				if (this.rhythmControllerSelectionPopup != null)
				{
					if (this.rhythmController != null)
					{
						this.rhythmControllerSelectionPopup.currentValueNoCallback = this.rhythmController.name;
					}
					else
					{
						this.onRhythmControllerNamesChanged(null);
						this.rhythmControllerSelectionPopup.currentValue = "None";
					}
					UIPopup uipopup3 = this.rhythmControllerSelectionPopup;
					uipopup3.onValueChangeHandlers = (UIPopup.OnValueChange)Delegate.Combine(uipopup3.onValueChangeHandlers, new UIPopup.OnValueChange(this.SetRhythmControllerObject));
				}
			}
		}
	}

	// Token: 0x06006AD6 RID: 27350 RVA: 0x00283B08 File Offset: 0x00281F08
	public override void InitUIAlt()
	{
		base.InitUIAlt();
		if (this.UITransformAlt != null)
		{
			RhythmForceProducerV2UI componentInChildren = this.UITransformAlt.GetComponentInChildren<RhythmForceProducerV2UI>(true);
			if (componentInChildren != null)
			{
				this.rangeSelectJSON.popupAlt = componentInChildren.rangeSelectPopup;
				this.alternateBeatRatioJSON.sliderAlt = componentInChildren.alternateBeatRatioSlider;
				this.burstLengthJSON.sliderAlt = componentInChildren.burstLengthSlider;
				this.minSpacingJSON.sliderAlt = componentInChildren.minSpacingSlider;
				this.randomFactorJSON.sliderAlt = componentInChildren.randomFactorSlider;
				this.thresholdJSON.sliderAlt = componentInChildren.thresholdSlider;
			}
		}
	}

	// Token: 0x06006AD7 RID: 27351 RVA: 0x00283BAB File Offset: 0x00281FAB
	protected override void Start()
	{
		base.Start();
		this.oneOverMaxOnset = 1f / this.maxOnset;
	}

	// Token: 0x06006AD8 RID: 27352 RVA: 0x00283BC8 File Offset: 0x00281FC8
	protected override void Update()
	{
		base.Update();
		this.forceTimer -= Time.deltaTime;
		if (this.forceTimer > 0f)
		{
			this.SetTargetForcePercent(this.flip * this.onset * this.oneOverMaxOnset);
			if (this.onset > this.peakOnset)
			{
				this.peakOnset = this.onset;
			}
		}
		else
		{
			this.SetTargetForcePercent(0f);
		}
		if (this.rhythmController != null && this.rhythmController.rhythmTool != null && this.rhythmController.low != null)
		{
			float num;
			if (!this.rhythmController.IsPlaying)
			{
				num = 0f;
			}
			else if (this.rangeSelect == RhythmForceProducerV2.RangeSelect.Low)
			{
				if (this.rhythmController.rhythmTool.CurrentFrame >= this.rhythmController.low.Length)
				{
					Debug.LogError(string.Concat(new object[]
					{
						"Rhythm frame ",
						this.rhythmController.rhythmTool.CurrentFrame,
						" is greater than analysis length ",
						this.rhythmController.low.Length
					}));
					num = 0f;
				}
				else
				{
					num = this.rhythmController.low[this.rhythmController.rhythmTool.CurrentFrame].onset * this.onsetMult;
				}
			}
			else if (this.rangeSelect == RhythmForceProducerV2.RangeSelect.Mid)
			{
				num = this.rhythmController.mid[this.rhythmController.rhythmTool.CurrentFrame].onset * this.onsetMult;
			}
			else
			{
				num = this.rhythmController.high[this.rhythmController.rhythmTool.CurrentFrame].onset * this.onsetMult;
			}
			if (num > this.maxOnset)
			{
				num = this.maxOnset;
			}
			if (num > this.minThreshold)
			{
				this.rawOnset = num;
				this.rawRhythmLineLength = this.rawOnset * this.linesScale * this._forceFactor * this.oneOverMaxOnset;
			}
			if (this.timerOn)
			{
				this.timer -= Time.deltaTime;
				if (this.timer < 0f)
				{
					this.timerOn = false;
				}
			}
			else
			{
				this.onset = num;
				if (this.onset > this.threshold)
				{
					if (this.flip > 0f)
					{
						this.flip = -this._alternateBeatRatio;
					}
					else
					{
						this.flip = 1f;
					}
					this.timerOn = true;
					this.timer = this._minSpacing;
					this.forceTimer = this._burstLength;
					if (UnityEngine.Random.value < this._randomFactor)
					{
						this.timer += this._minSpacing;
						this.forceTimer += this._burstLength;
					}
					this.rhythmLineLength = this.rawRhythmLineLength * this.flip;
				}
			}
			this.rawRhythmLineLength = Mathf.Lerp(this.rawRhythmLineLength, 0f, Time.deltaTime * 5f);
			if (this.on && this.receiver != null && this.drawLines)
			{
				Vector3 a = this.AxisToVector(this.forceAxis);
				Vector3 a2 = this.AxisToUpVector(this.forceAxis);
				Vector3 vector = base.transform.position + a2 * (this.lineOffset + this.lineSpacing * 10f);
				if (this.rhythmLineDrawer != null)
				{
					this.rhythmLineDrawer.SetLinePoints(vector, vector + a * this.rhythmLineLength);
					this.rhythmLineDrawer.Draw(base.gameObject.layer);
				}
				vector += a2 * this.lineSpacing;
				if (this.rawRhythmLineDrawer != null)
				{
					this.rawRhythmLineDrawer.SetLinePoints(vector, vector + a * -this.rawRhythmLineLength);
					this.rawRhythmLineDrawer.Draw(base.gameObject.layer);
				}
			}
		}
	}

	// Token: 0x04005C99 RID: 23705
	protected string[] customParamNamesOverride = new string[]
	{
		"receiver",
		"rhythmController"
	};

	// Token: 0x04005C9A RID: 23706
	public UIPopup rhythmControllerAtomSelectionPopup;

	// Token: 0x04005C9B RID: 23707
	public UIPopup rhythmControllerSelectionPopup;

	// Token: 0x04005C9C RID: 23708
	protected string rhythmControllerAtomUID;

	// Token: 0x04005C9D RID: 23709
	[SerializeField]
	protected RhythmController _rhythmController;

	// Token: 0x04005C9E RID: 23710
	protected JSONStorableFloat alternateBeatRatioJSON;

	// Token: 0x04005C9F RID: 23711
	[SerializeField]
	protected float _alternateBeatRatio = 1f;

	// Token: 0x04005CA0 RID: 23712
	protected JSONStorableFloat thresholdJSON;

	// Token: 0x04005CA1 RID: 23713
	[SerializeField]
	protected float _threshold = 1f;

	// Token: 0x04005CA2 RID: 23714
	protected JSONStorableFloat burstLengthJSON;

	// Token: 0x04005CA3 RID: 23715
	[SerializeField]
	protected float _burstLength = 1f;

	// Token: 0x04005CA4 RID: 23716
	protected JSONStorableFloat minSpacingJSON;

	// Token: 0x04005CA5 RID: 23717
	[SerializeField]
	protected float _minSpacing = 0.4f;

	// Token: 0x04005CA6 RID: 23718
	protected JSONStorableFloat randomFactorJSON;

	// Token: 0x04005CA7 RID: 23719
	[SerializeField]
	protected float _randomFactor = 0.1f;

	// Token: 0x04005CA8 RID: 23720
	public RhythmForceProducerV2.RangeSelect rangeSelect;

	// Token: 0x04005CA9 RID: 23721
	protected JSONStorableStringChooser rangeSelectJSON;

	// Token: 0x04005CAA RID: 23722
	public Material rhythmLineMaterial;

	// Token: 0x04005CAB RID: 23723
	public Material rawRhythmLineMaterial;

	// Token: 0x04005CAC RID: 23724
	protected float minThreshold = 1f;

	// Token: 0x04005CAD RID: 23725
	protected float flip = 1f;

	// Token: 0x04005CAE RID: 23726
	protected bool timerOn;

	// Token: 0x04005CAF RID: 23727
	protected float timer;

	// Token: 0x04005CB0 RID: 23728
	protected float forceTimer;

	// Token: 0x04005CB1 RID: 23729
	protected float maxOnset = 60f;

	// Token: 0x04005CB2 RID: 23730
	public float peakOnset;

	// Token: 0x04005CB3 RID: 23731
	protected float oneOverMaxOnset;

	// Token: 0x04005CB4 RID: 23732
	protected float onsetMult = 2f;

	// Token: 0x04005CB5 RID: 23733
	protected float rawOnset;

	// Token: 0x04005CB6 RID: 23734
	protected float onset;

	// Token: 0x04005CB7 RID: 23735
	protected LineDrawer rhythmLineDrawer;

	// Token: 0x04005CB8 RID: 23736
	protected LineDrawer rawRhythmLineDrawer;

	// Token: 0x04005CB9 RID: 23737
	protected float rhythmLineLength;

	// Token: 0x04005CBA RID: 23738
	protected float rawRhythmLineLength;

	// Token: 0x02000D88 RID: 3464
	public enum RangeSelect
	{
		// Token: 0x04005CBC RID: 23740
		Low,
		// Token: 0x04005CBD RID: 23741
		Mid,
		// Token: 0x04005CBE RID: 23742
		High
	}
}
