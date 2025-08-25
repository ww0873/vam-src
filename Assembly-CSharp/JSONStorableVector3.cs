using System;
using SimpleJSON;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

// Token: 0x02000CF1 RID: 3313
public class JSONStorableVector3 : JSONStorableParam
{
	// Token: 0x06006491 RID: 25745 RVA: 0x00261B0C File Offset: 0x0025FF0C
	public JSONStorableVector3(string paramName, Vector3 startingValue, Vector3 minValue, Vector3 maxValue, bool constrain = true, bool interactable = true)
	{
		this.type = JSONStorable.Type.Vector3;
		this.name = paramName;
		this.min = minValue;
		this.max = maxValue;
		this.defaultVal = startingValue;
		this.val = startingValue;
		this.setCallbackFunction = null;
		this.setJSONCallbackFunction = null;
		this.constrained = constrain;
		this._interactable = interactable;
	}

	// Token: 0x06006492 RID: 25746 RVA: 0x00261B68 File Offset: 0x0025FF68
	public JSONStorableVector3(string paramName, Vector3 startingValue, JSONStorableVector3.SetVector3Callback callback, Vector3 minValue, Vector3 maxValue, bool constrain = true, bool interactable = true)
	{
		this.type = JSONStorable.Type.Vector3;
		this.name = paramName;
		this.min = minValue;
		this.max = maxValue;
		this.defaultVal = startingValue;
		this.val = startingValue;
		this.setCallbackFunction = callback;
		this.setJSONCallbackFunction = null;
		this.constrained = constrain;
		this._interactable = interactable;
	}

	// Token: 0x06006493 RID: 25747 RVA: 0x00261BC8 File Offset: 0x0025FFC8
	public JSONStorableVector3(string paramName, Vector3 startingValue, JSONStorableVector3.SetJSONVector3Callback callback, Vector3 minValue, Vector3 maxValue, bool constrain = true, bool interactable = true)
	{
		this.type = JSONStorable.Type.Vector3;
		this.name = paramName;
		this.min = minValue;
		this.max = maxValue;
		this.defaultVal = startingValue;
		this.val = startingValue;
		this.setCallbackFunction = null;
		this.setJSONCallbackFunction = callback;
		this.constrained = constrain;
		this._interactable = interactable;
	}

	// Token: 0x06006494 RID: 25748 RVA: 0x00261C28 File Offset: 0x00260028
	public override bool StoreJSON(JSONClass jc, bool includePhysical = true, bool includeAppearance = true, bool forceStore = false)
	{
		bool flag = this.NeedsStore(jc, includePhysical, includeAppearance) && (forceStore || this._val != this._defaultVal);
		if (flag)
		{
			jc[this.name][120].AsFloat = this._val.x;
			jc[this.name][121].AsFloat = this._val.y;
			jc[this.name][122].AsFloat = this._val.z;
		}
		return flag;
	}

	// Token: 0x06006495 RID: 25749 RVA: 0x00261CD4 File Offset: 0x002600D4
	public override void RestoreFromJSON(JSONClass jc, bool restorePhysical = true, bool restoreAppearance = true, bool setMissingToDefault = true)
	{
		bool flag = this.NeedsRestore(jc, restorePhysical, restoreAppearance);
		if (flag)
		{
			if (jc[this.name] != null)
			{
				Vector3 val;
				val.x = 0f;
				val.y = 0f;
				val.z = 0f;
				if (jc[this.name][120] != null)
				{
					val.x = jc[this.name][120].AsFloat;
				}
				if (jc[this.name][121] != null)
				{
					val.y = jc[this.name][121].AsFloat;
				}
				if (jc[this.name][122] != null)
				{
					val.z = jc[this.name][122].AsFloat;
				}
				this.val = val;
			}
			else if (setMissingToDefault)
			{
				this.val = this._defaultVal;
			}
		}
	}

	// Token: 0x06006496 RID: 25750 RVA: 0x00261E04 File Offset: 0x00260204
	public override void LateRestoreFromJSON(JSONClass jc, bool restorePhysical = true, bool restoreAppearance = true, bool setMissingToDefault = true)
	{
		bool flag = this.NeedsLateRestore(jc, restorePhysical, restoreAppearance);
		if (flag)
		{
			if (jc[this.name] != null)
			{
				Vector3 val;
				val.x = 0f;
				val.y = 0f;
				val.z = 0f;
				if (jc[this.name][120] != null)
				{
					val.x = jc[this.name][120].AsFloat;
				}
				if (jc[this.name][121] != null)
				{
					val.y = jc[this.name][121].AsFloat;
				}
				if (jc[this.name][122] != null)
				{
					val.z = jc[this.name][122].AsFloat;
				}
				this.val = val;
			}
			else if (setMissingToDefault)
			{
				this.val = this._defaultVal;
			}
		}
	}

	// Token: 0x06006497 RID: 25751 RVA: 0x00261F32 File Offset: 0x00260332
	public override void SetDefaultFromCurrent()
	{
		this.defaultVal = this.val;
	}

	// Token: 0x06006498 RID: 25752 RVA: 0x00261F40 File Offset: 0x00260340
	public override void SetValToDefault()
	{
		this.val = this.defaultVal;
	}

	// Token: 0x17000ED4 RID: 3796
	// (get) Token: 0x06006499 RID: 25753 RVA: 0x00261F4E File Offset: 0x0026034E
	// (set) Token: 0x0600649A RID: 25754 RVA: 0x00261F58 File Offset: 0x00260358
	public Vector3 defaultVal
	{
		get
		{
			return this._defaultVal;
		}
		set
		{
			if (this._defaultVal != value)
			{
				this._defaultVal = value;
				if (this.sliderX != null)
				{
					SliderControl component = this.sliderX.GetComponent<SliderControl>();
					if (component != null)
					{
						component.defaultValue = this._defaultVal.x;
					}
				}
				if (this.sliderXAlt != null)
				{
					SliderControl component2 = this.sliderXAlt.GetComponent<SliderControl>();
					if (component2 != null)
					{
						component2.defaultValue = this._defaultVal.x;
					}
				}
				if (this.sliderY != null)
				{
					SliderControl component3 = this.sliderY.GetComponent<SliderControl>();
					if (component3 != null)
					{
						component3.defaultValue = this._defaultVal.y;
					}
				}
				if (this.sliderYAlt != null)
				{
					SliderControl component4 = this.sliderYAlt.GetComponent<SliderControl>();
					if (component4 != null)
					{
						component4.defaultValue = this._defaultVal.y;
					}
				}
				if (this.sliderZ != null)
				{
					SliderControl component5 = this.sliderZ.GetComponent<SliderControl>();
					if (component5 != null)
					{
						component5.defaultValue = this._defaultVal.z;
					}
				}
				if (this.sliderZAlt != null)
				{
					SliderControl component6 = this.sliderZAlt.GetComponent<SliderControl>();
					if (component6 != null)
					{
						component6.defaultValue = this._defaultVal.z;
					}
				}
			}
		}
	}

	// Token: 0x0600649B RID: 25755 RVA: 0x002620E0 File Offset: 0x002604E0
	protected void InternalSetVal(Vector3 v, bool doCallback = true)
	{
		Vector3 vector = v;
		if (this._constrained)
		{
			vector.x = Mathf.Clamp(vector.x, this._min.x, this._max.x);
			vector.y = Mathf.Clamp(vector.y, this._min.x, this._max.x);
			vector.z = Mathf.Clamp(vector.z, this._min.x, this._max.x);
		}
		if (this._val != vector)
		{
			this._val = vector;
			bool flag = false;
			Vector3 min = this.min;
			if (this.min.x > this._val.x)
			{
				min.x = this._val.x;
				flag = true;
				if (this._sliderX != null && min.x > this._sliderX.minValue)
				{
					min.x = this._sliderX.minValue;
				}
				if (this._sliderXAlt != null && min.x > this._sliderXAlt.minValue)
				{
					min.x = this._sliderXAlt.minValue;
				}
			}
			if (this.min.y > this._val.y)
			{
				min.y = this._val.y;
				flag = true;
				if (this._sliderY != null && min.y > this._sliderY.minValue)
				{
					min.y = this._sliderY.minValue;
				}
				if (this._sliderYAlt != null && min.y > this._sliderYAlt.minValue)
				{
					min.y = this._sliderYAlt.minValue;
				}
			}
			if (this.min.z > this._val.z)
			{
				min.z = this._val.z;
				flag = true;
				if (this._sliderZ != null && min.z > this._sliderZ.minValue)
				{
					min.z = this._sliderZ.minValue;
				}
				if (this._sliderZAlt != null && min.z > this._sliderZAlt.minValue)
				{
					min.z = this._sliderZAlt.minValue;
				}
			}
			if (flag)
			{
				this.min = min;
			}
			bool flag2 = false;
			Vector3 max = this.max;
			if (this.max.x < this._val.x)
			{
				max.x = this._val.x;
				flag2 = true;
				if (this._sliderX != null && max.x < this._sliderX.maxValue)
				{
					max.x = this._sliderX.maxValue;
				}
				if (this._sliderXAlt != null && max.x < this._sliderXAlt.maxValue)
				{
					max.x = this._sliderXAlt.maxValue;
				}
			}
			if (this.max.y < this._val.y)
			{
				max.y = this._val.y;
				flag2 = true;
				if (this._sliderY != null && max.y < this._sliderY.maxValue)
				{
					max.y = this._sliderY.maxValue;
				}
				if (this._sliderYAlt != null && max.y < this._sliderYAlt.maxValue)
				{
					max.y = this._sliderYAlt.maxValue;
				}
			}
			if (this.max.z < this._val.z)
			{
				max.z = this._val.z;
				flag2 = true;
				if (this._sliderZ != null && max.z < this._sliderZ.maxValue)
				{
					max.z = this._sliderZ.maxValue;
				}
				if (this._sliderZAlt != null && max.z < this._sliderZAlt.maxValue)
				{
					max.z = this._sliderZAlt.maxValue;
				}
			}
			if (flag2)
			{
				this.max = max;
			}
			if (this._sliderX != null)
			{
				this._sliderX.value = this._val.x;
			}
			if (this._sliderXAlt != null)
			{
				this._sliderXAlt.value = this._val.x;
			}
			if (this._sliderY != null)
			{
				this._sliderY.value = this._val.y;
			}
			if (this._sliderYAlt != null)
			{
				this._sliderYAlt.value = this._val.y;
			}
			if (this._sliderZ != null)
			{
				this._sliderZ.value = this._val.z;
			}
			if (this._sliderZAlt != null)
			{
				this._sliderZAlt.value = this._val.z;
			}
			if (doCallback)
			{
				if (this.setCallbackFunction != null)
				{
					this.setCallbackFunction(this._val);
				}
				if (this.setJSONCallbackFunction != null)
				{
					this.setJSONCallbackFunction(this);
				}
			}
		}
	}

	// Token: 0x0600649C RID: 25756 RVA: 0x002626CC File Offset: 0x00260ACC
	public void SetVal(Vector3 value)
	{
		this.val = value;
	}

	// Token: 0x0600649D RID: 25757 RVA: 0x002626D8 File Offset: 0x00260AD8
	public void SetXVal(float f)
	{
		Vector3 val = this._val;
		val.x = f;
		this.val = val;
	}

	// Token: 0x0600649E RID: 25758 RVA: 0x002626FC File Offset: 0x00260AFC
	public void SetYVal(float f)
	{
		Vector3 val = this._val;
		val.y = f;
		this.val = val;
	}

	// Token: 0x0600649F RID: 25759 RVA: 0x00262720 File Offset: 0x00260B20
	public void SetZVal(float f)
	{
		Vector3 val = this._val;
		val.z = f;
		this.val = val;
	}

	// Token: 0x17000ED5 RID: 3797
	// (get) Token: 0x060064A0 RID: 25760 RVA: 0x00262743 File Offset: 0x00260B43
	// (set) Token: 0x060064A1 RID: 25761 RVA: 0x0026274B File Offset: 0x00260B4B
	public virtual Vector3 val
	{
		get
		{
			return this._val;
		}
		set
		{
			this.InternalSetVal(value, true);
		}
	}

	// Token: 0x17000ED6 RID: 3798
	// (get) Token: 0x060064A2 RID: 25762 RVA: 0x00262755 File Offset: 0x00260B55
	// (set) Token: 0x060064A3 RID: 25763 RVA: 0x0026275D File Offset: 0x00260B5D
	public virtual Vector3 valNoCallback
	{
		get
		{
			return this._val;
		}
		set
		{
			this.InternalSetVal(value, false);
		}
	}

	// Token: 0x17000ED7 RID: 3799
	// (get) Token: 0x060064A4 RID: 25764 RVA: 0x00262767 File Offset: 0x00260B67
	// (set) Token: 0x060064A5 RID: 25765 RVA: 0x0026276F File Offset: 0x00260B6F
	public bool constrained
	{
		get
		{
			return this._constrained;
		}
		set
		{
			if (this._constrained != value)
			{
				this._constrained = value;
			}
		}
	}

	// Token: 0x17000ED8 RID: 3800
	// (get) Token: 0x060064A6 RID: 25766 RVA: 0x00262784 File Offset: 0x00260B84
	// (set) Token: 0x060064A7 RID: 25767 RVA: 0x0026278C File Offset: 0x00260B8C
	public Vector3 min
	{
		get
		{
			return this._min;
		}
		set
		{
			if (this._min != value)
			{
				this._min = value;
				if (this._sliderX != null)
				{
					this._sliderX.minValue = this._min.x;
				}
				if (this._sliderXAlt != null)
				{
					this._sliderXAlt.minValue = this._min.x;
				}
				if (this._sliderY != null)
				{
					this._sliderY.minValue = this._min.y;
				}
				if (this._sliderYAlt != null)
				{
					this._sliderYAlt.minValue = this._min.y;
				}
				if (this._sliderZ != null)
				{
					this._sliderZ.minValue = this._min.z;
				}
				if (this._sliderZAlt != null)
				{
					this._sliderZAlt.minValue = this._min.z;
				}
			}
		}
	}

	// Token: 0x17000ED9 RID: 3801
	// (get) Token: 0x060064A8 RID: 25768 RVA: 0x0026289B File Offset: 0x00260C9B
	// (set) Token: 0x060064A9 RID: 25769 RVA: 0x002628A4 File Offset: 0x00260CA4
	public Vector3 max
	{
		get
		{
			return this._max;
		}
		set
		{
			if (this._max != value)
			{
				this._max = value;
				if (this._sliderX != null)
				{
					this._sliderX.maxValue = this._max.x;
				}
				if (this._sliderXAlt != null)
				{
					this._sliderXAlt.maxValue = this._max.x;
				}
				if (this._sliderY != null)
				{
					this._sliderY.maxValue = this._max.y;
				}
				if (this._sliderYAlt != null)
				{
					this._sliderYAlt.maxValue = this._max.y;
				}
				if (this._sliderZ != null)
				{
					this._sliderZ.maxValue = this._max.z;
				}
				if (this._sliderZAlt != null)
				{
					this._sliderZAlt.maxValue = this._max.z;
				}
			}
		}
	}

	// Token: 0x060064AA RID: 25770 RVA: 0x002629B3 File Offset: 0x00260DB3
	public void RegisterSliderX(Slider s, bool isAlt = false)
	{
		if (isAlt)
		{
			this.sliderXAlt = s;
		}
		else
		{
			this.sliderX = s;
		}
	}

	// Token: 0x17000EDA RID: 3802
	// (get) Token: 0x060064AB RID: 25771 RVA: 0x002629CE File Offset: 0x00260DCE
	// (set) Token: 0x060064AC RID: 25772 RVA: 0x002629D8 File Offset: 0x00260DD8
	public Slider sliderX
	{
		get
		{
			return this._sliderX;
		}
		set
		{
			if (this._sliderX != value)
			{
				if (this._sliderX != null)
				{
					this._sliderX.interactable = true;
					this._sliderX.onValueChanged.RemoveListener(new UnityAction<float>(this.SetXVal));
				}
				this._sliderX = value;
				this._sliderXControl = null;
				if (this._sliderX != null)
				{
					this._sliderX.interactable = this._interactable;
					this._sliderXControl = this._sliderX.GetComponent<SliderControl>();
					if (this._sliderXControl != null)
					{
						this._sliderXControl.defaultValue = this._defaultVal.x;
					}
					this._sliderX.minValue = this._min.x;
					this._sliderX.maxValue = this._max.x;
					this._sliderX.value = this._val.x;
					this._sliderX.onValueChanged.AddListener(new UnityAction<float>(this.SetXVal));
				}
			}
		}
	}

	// Token: 0x17000EDB RID: 3803
	// (get) Token: 0x060064AD RID: 25773 RVA: 0x00262AF5 File Offset: 0x00260EF5
	// (set) Token: 0x060064AE RID: 25774 RVA: 0x00262B00 File Offset: 0x00260F00
	public Slider sliderXAlt
	{
		get
		{
			return this._sliderXAlt;
		}
		set
		{
			if (this._sliderXAlt != value)
			{
				if (this._sliderXAlt != null)
				{
					this._sliderXAlt.interactable = true;
					this._sliderXAlt.onValueChanged.RemoveListener(new UnityAction<float>(this.SetXVal));
				}
				this._sliderXAlt = value;
				this._sliderXControlAlt = null;
				if (this._sliderXAlt != null)
				{
					this._sliderXAlt.interactable = this._interactable;
					this._sliderXControlAlt = this._sliderXAlt.GetComponent<SliderControl>();
					if (this._sliderXControlAlt != null)
					{
						this._sliderXControlAlt.defaultValue = this._defaultVal.x;
					}
					this._sliderXAlt.minValue = this._min.x;
					this._sliderXAlt.maxValue = this._max.x;
					this._sliderXAlt.value = this._val.x;
					this._sliderXAlt.onValueChanged.AddListener(new UnityAction<float>(this.SetXVal));
				}
			}
		}
	}

	// Token: 0x060064AF RID: 25775 RVA: 0x00262C1D File Offset: 0x0026101D
	public void RegisterSliderY(Slider s, bool isAlt = false)
	{
		if (isAlt)
		{
			this.sliderYAlt = s;
		}
		else
		{
			this.sliderY = s;
		}
	}

	// Token: 0x17000EDC RID: 3804
	// (get) Token: 0x060064B0 RID: 25776 RVA: 0x00262C38 File Offset: 0x00261038
	// (set) Token: 0x060064B1 RID: 25777 RVA: 0x00262C40 File Offset: 0x00261040
	public Slider sliderY
	{
		get
		{
			return this._sliderY;
		}
		set
		{
			if (this._sliderY != value)
			{
				if (this._sliderY != null)
				{
					this._sliderY.interactable = true;
					this._sliderY.onValueChanged.RemoveListener(new UnityAction<float>(this.SetXVal));
				}
				this._sliderY = value;
				this._sliderYControl = null;
				if (this._sliderY != null)
				{
					this._sliderY.interactable = this._interactable;
					this._sliderYControl = this._sliderY.GetComponent<SliderControl>();
					if (this._sliderYControl != null)
					{
						this._sliderYControl.defaultValue = this._defaultVal.y;
					}
					this._sliderY.minValue = this._min.y;
					this._sliderY.maxValue = this._max.y;
					this._sliderY.value = this._val.y;
					this._sliderY.onValueChanged.AddListener(new UnityAction<float>(this.SetXVal));
				}
			}
		}
	}

	// Token: 0x17000EDD RID: 3805
	// (get) Token: 0x060064B2 RID: 25778 RVA: 0x00262D5D File Offset: 0x0026115D
	// (set) Token: 0x060064B3 RID: 25779 RVA: 0x00262D68 File Offset: 0x00261168
	public Slider sliderYAlt
	{
		get
		{
			return this._sliderYAlt;
		}
		set
		{
			if (this._sliderYAlt != value)
			{
				if (this._sliderYAlt != null)
				{
					this._sliderYAlt.interactable = true;
					this._sliderYAlt.onValueChanged.RemoveListener(new UnityAction<float>(this.SetXVal));
				}
				this._sliderYAlt = value;
				this._sliderYControlAlt = null;
				if (this._sliderYAlt != null)
				{
					this._sliderYAlt.interactable = this._interactable;
					this._sliderYControlAlt = this._sliderYAlt.GetComponent<SliderControl>();
					if (this._sliderYControlAlt != null)
					{
						this._sliderYControlAlt.defaultValue = this._defaultVal.y;
					}
					this._sliderYAlt.minValue = this._min.y;
					this._sliderYAlt.maxValue = this._max.y;
					this._sliderYAlt.value = this._val.y;
					this._sliderYAlt.onValueChanged.AddListener(new UnityAction<float>(this.SetXVal));
				}
			}
		}
	}

	// Token: 0x060064B4 RID: 25780 RVA: 0x00262E85 File Offset: 0x00261285
	public void RegisterSliderZ(Slider s, bool isAlt = false)
	{
		if (isAlt)
		{
			this.sliderZAlt = s;
		}
		else
		{
			this.sliderZ = s;
		}
	}

	// Token: 0x17000EDE RID: 3806
	// (get) Token: 0x060064B5 RID: 25781 RVA: 0x00262EA0 File Offset: 0x002612A0
	// (set) Token: 0x060064B6 RID: 25782 RVA: 0x00262EA8 File Offset: 0x002612A8
	public Slider sliderZ
	{
		get
		{
			return this._sliderZ;
		}
		set
		{
			if (this._sliderZ != value)
			{
				if (this._sliderZ != null)
				{
					this._sliderZ.interactable = true;
					this._sliderZ.onValueChanged.RemoveListener(new UnityAction<float>(this.SetXVal));
				}
				this._sliderZ = value;
				this._sliderZControl = null;
				if (this._sliderZ != null)
				{
					this._sliderZ.interactable = this._interactable;
					this._sliderZControl = this._sliderZ.GetComponent<SliderControl>();
					if (this._sliderZControl != null)
					{
						this._sliderZControl.defaultValue = this._defaultVal.z;
					}
					this._sliderZ.minValue = this._min.z;
					this._sliderZ.maxValue = this._max.z;
					this._sliderZ.value = this._val.z;
					this._sliderZ.onValueChanged.AddListener(new UnityAction<float>(this.SetXVal));
				}
			}
		}
	}

	// Token: 0x17000EDF RID: 3807
	// (get) Token: 0x060064B7 RID: 25783 RVA: 0x00262FC5 File Offset: 0x002613C5
	// (set) Token: 0x060064B8 RID: 25784 RVA: 0x00262FD0 File Offset: 0x002613D0
	public Slider sliderZAlt
	{
		get
		{
			return this._sliderZAlt;
		}
		set
		{
			if (this._sliderZAlt != value)
			{
				if (this._sliderZAlt != null)
				{
					this._sliderZAlt.interactable = true;
					this._sliderZAlt.onValueChanged.RemoveListener(new UnityAction<float>(this.SetXVal));
				}
				this._sliderZAlt = value;
				this._sliderZControlAlt = null;
				if (this._sliderZAlt != null)
				{
					this._sliderZAlt.interactable = this._interactable;
					this._sliderZControlAlt = this._sliderZAlt.GetComponent<SliderControl>();
					if (this._sliderZControlAlt != null)
					{
						this._sliderZControlAlt.defaultValue = this._defaultVal.z;
					}
					this._sliderZAlt.minValue = this._min.z;
					this._sliderZAlt.maxValue = this._max.z;
					this._sliderZAlt.value = this._val.z;
					this._sliderZAlt.onValueChanged.AddListener(new UnityAction<float>(this.SetXVal));
				}
			}
		}
	}

	// Token: 0x04005449 RID: 21577
	protected bool _interactable;

	// Token: 0x0400544A RID: 21578
	protected Vector3 _defaultVal;

	// Token: 0x0400544B RID: 21579
	protected Vector3 _val;

	// Token: 0x0400544C RID: 21580
	protected bool _constrained;

	// Token: 0x0400544D RID: 21581
	protected Vector3 _min;

	// Token: 0x0400544E RID: 21582
	protected Vector3 _max;

	// Token: 0x0400544F RID: 21583
	public JSONStorableVector3.SetVector3Callback setCallbackFunction;

	// Token: 0x04005450 RID: 21584
	public JSONStorableVector3.SetJSONVector3Callback setJSONCallbackFunction;

	// Token: 0x04005451 RID: 21585
	protected SliderControl _sliderXControl;

	// Token: 0x04005452 RID: 21586
	protected Slider _sliderX;

	// Token: 0x04005453 RID: 21587
	protected SliderControl _sliderXControlAlt;

	// Token: 0x04005454 RID: 21588
	protected Slider _sliderXAlt;

	// Token: 0x04005455 RID: 21589
	protected SliderControl _sliderYControl;

	// Token: 0x04005456 RID: 21590
	protected Slider _sliderY;

	// Token: 0x04005457 RID: 21591
	protected SliderControl _sliderYControlAlt;

	// Token: 0x04005458 RID: 21592
	protected Slider _sliderYAlt;

	// Token: 0x04005459 RID: 21593
	protected SliderControl _sliderZControl;

	// Token: 0x0400545A RID: 21594
	protected Slider _sliderZ;

	// Token: 0x0400545B RID: 21595
	protected SliderControl _sliderZControlAlt;

	// Token: 0x0400545C RID: 21596
	protected Slider _sliderZAlt;

	// Token: 0x02000CF2 RID: 3314
	// (Invoke) Token: 0x060064BA RID: 25786
	public delegate void SetJSONVector3Callback(JSONStorableVector3 jf);

	// Token: 0x02000CF3 RID: 3315
	// (Invoke) Token: 0x060064BE RID: 25790
	public delegate void SetVector3Callback(Vector3 v);
}
