using System;
using SimpleJSON;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

// Token: 0x02000CDF RID: 3295
public class JSONStorableFloat : JSONStorableParam
{
	// Token: 0x060063A4 RID: 25508 RVA: 0x0025D6E0 File Offset: 0x0025BAE0
	public JSONStorableFloat(string paramName, float startingValue, float minValue, float maxValue, bool constrain = true, bool interactable = true)
	{
		this.type = JSONStorable.Type.Float;
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

	// Token: 0x060063A5 RID: 25509 RVA: 0x0025D73C File Offset: 0x0025BB3C
	public JSONStorableFloat(string paramName, float startingValue, JSONStorableFloat.SetFloatCallback callback, float minValue, float maxValue, bool constrain = true, bool interactable = true)
	{
		this.type = JSONStorable.Type.Float;
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

	// Token: 0x060063A6 RID: 25510 RVA: 0x0025D79C File Offset: 0x0025BB9C
	public JSONStorableFloat(string paramName, float startingValue, JSONStorableFloat.SetJSONFloatCallback callback, float minValue, float maxValue, bool constrain = true, bool interactable = true)
	{
		this.type = JSONStorable.Type.Float;
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

	// Token: 0x060063A7 RID: 25511 RVA: 0x0025D7FC File Offset: 0x0025BBFC
	public override bool StoreJSON(JSONClass jc, bool includePhysical = true, bool includeAppearance = true, bool forceStore = false)
	{
		bool flag = this.NeedsStore(jc, includePhysical, includeAppearance) && (forceStore || this._val != this._defaultVal);
		if (flag)
		{
			if (this.name.IndexOf(':') != -1)
			{
				string[] array = this.name.Split(new char[]
				{
					':'
				});
				jc[array[0]][array[1]].AsFloat = this._val;
			}
			else
			{
				jc[this.name].AsFloat = this._val;
			}
		}
		return flag;
	}

	// Token: 0x060063A8 RID: 25512 RVA: 0x0025D8A0 File Offset: 0x0025BCA0
	public override void RestoreFromJSON(JSONClass jc, bool restorePhysical = true, bool restoreAppearance = true, bool setMissingToDefault = true)
	{
		bool flag = this.NeedsRestore(jc, restorePhysical, restoreAppearance);
		if (flag)
		{
			if (this.name.IndexOf(':') != -1)
			{
				string[] array = this.name.Split(new char[]
				{
					':'
				});
				if (jc[array[0]][array[1]] != null)
				{
					this.val = jc[array[0]][array[1]].AsFloat;
				}
				else if (setMissingToDefault)
				{
					this.val = this._defaultVal;
				}
			}
			else if (jc[this.name] != null)
			{
				if (jc[this.name].Value == string.Empty)
				{
					this.val = this._defaultVal;
				}
				else
				{
					this.val = jc[this.name].AsFloat;
				}
			}
			else if (setMissingToDefault)
			{
				this.val = this._defaultVal;
			}
		}
	}

	// Token: 0x060063A9 RID: 25513 RVA: 0x0025D9B4 File Offset: 0x0025BDB4
	public override void LateRestoreFromJSON(JSONClass jc, bool restorePhysical = true, bool restoreAppearance = true, bool setMissingToDefault = true)
	{
		bool flag = this.NeedsLateRestore(jc, restorePhysical, restoreAppearance);
		if (flag)
		{
			if (this.name.IndexOf(':') != -1)
			{
				string[] array = this.name.Split(new char[]
				{
					':'
				});
				if (jc[array[0]][array[1]] != null)
				{
					this.val = jc[array[0]][array[1]].AsFloat;
				}
				else if (setMissingToDefault)
				{
					this.val = this._defaultVal;
				}
			}
			else if (jc[this.name] != null)
			{
				if (jc[this.name].Value == string.Empty)
				{
					this.val = this._defaultVal;
				}
				else
				{
					this.val = jc[this.name].AsFloat;
				}
			}
			else if (setMissingToDefault)
			{
				this.val = this._defaultVal;
			}
		}
	}

	// Token: 0x060063AA RID: 25514 RVA: 0x0025DAC7 File Offset: 0x0025BEC7
	public override void SetDefaultFromCurrent()
	{
		this.defaultVal = this.val;
	}

	// Token: 0x060063AB RID: 25515 RVA: 0x0025DAD5 File Offset: 0x0025BED5
	public override void SetValToDefault()
	{
		this.val = this.defaultVal;
	}

	// Token: 0x060063AC RID: 25516 RVA: 0x0025DAE4 File Offset: 0x0025BEE4
	public void SetInteractble(bool b)
	{
		this._interactable = b;
		if (this.slider != null)
		{
			this.slider.interactable = this._interactable;
		}
		if (this.sliderAlt != null)
		{
			this.sliderAlt.interactable = this._interactable;
		}
	}

	// Token: 0x17000EA1 RID: 3745
	// (get) Token: 0x060063AD RID: 25517 RVA: 0x0025DB3C File Offset: 0x0025BF3C
	// (set) Token: 0x060063AE RID: 25518 RVA: 0x0025DB44 File Offset: 0x0025BF44
	public float defaultVal
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
				if (this.slider != null)
				{
					SliderControl component = this.slider.GetComponent<SliderControl>();
					if (component != null)
					{
						component.defaultValue = this._defaultVal;
					}
				}
				if (this.sliderAlt != null)
				{
					SliderControl component2 = this.sliderAlt.GetComponent<SliderControl>();
					if (component2 != null)
					{
						component2.defaultValue = this._defaultVal;
					}
				}
			}
		}
	}

	// Token: 0x060063AF RID: 25519 RVA: 0x0025DBD0 File Offset: 0x0025BFD0
	protected void InternalSetVal(float f, bool doCallback = true)
	{
		float num = f;
		if (this._constrained)
		{
			num = Mathf.Clamp(num, this._min, this._max);
		}
		if (this._val != num)
		{
			this._val = num;
			if (this.min > this._val)
			{
				float num2 = this._val;
				if (this._slider != null && num2 > this._slider.minValue)
				{
					num2 = this._slider.minValue;
				}
				if (this._sliderAlt != null && num2 > this._sliderAlt.minValue)
				{
					num2 = this._sliderAlt.minValue;
				}
				this.min = num2;
			}
			if (this.max < this._val)
			{
				float num3 = this._val;
				if (this._slider != null && num3 < this._slider.maxValue)
				{
					num3 = this._slider.maxValue;
				}
				if (this._sliderAlt != null && num3 < this._sliderAlt.maxValue)
				{
					num3 = this._sliderAlt.maxValue;
				}
				this.max = num3;
			}
			if (this._slider != null)
			{
				this._slider.value = this._val;
			}
			if (this._sliderAlt != null)
			{
				this._sliderAlt.value = this._val;
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

	// Token: 0x060063B0 RID: 25520 RVA: 0x0025DD83 File Offset: 0x0025C183
	public void SetVal(float value)
	{
		this.val = value;
	}

	// Token: 0x17000EA2 RID: 3746
	// (get) Token: 0x060063B1 RID: 25521 RVA: 0x0025DD8C File Offset: 0x0025C18C
	// (set) Token: 0x060063B2 RID: 25522 RVA: 0x0025DD94 File Offset: 0x0025C194
	public virtual float val
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

	// Token: 0x17000EA3 RID: 3747
	// (get) Token: 0x060063B3 RID: 25523 RVA: 0x0025DD9E File Offset: 0x0025C19E
	// (set) Token: 0x060063B4 RID: 25524 RVA: 0x0025DDA6 File Offset: 0x0025C1A6
	public virtual float valNoCallback
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

	// Token: 0x17000EA4 RID: 3748
	// (get) Token: 0x060063B5 RID: 25525 RVA: 0x0025DDB0 File Offset: 0x0025C1B0
	// (set) Token: 0x060063B6 RID: 25526 RVA: 0x0025DDB8 File Offset: 0x0025C1B8
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
				if (this._slider != null)
				{
					SliderControl component = this._slider.GetComponent<SliderControl>();
					if (component != null)
					{
						component.clamp = this._constrained;
					}
				}
				if (this._sliderAlt != null)
				{
					SliderControl component2 = this._sliderAlt.GetComponent<SliderControl>();
					if (component2 != null)
					{
						component2.clamp = this._constrained;
					}
				}
			}
		}
	}

	// Token: 0x17000EA5 RID: 3749
	// (get) Token: 0x060063B7 RID: 25527 RVA: 0x0025DE42 File Offset: 0x0025C242
	// (set) Token: 0x060063B8 RID: 25528 RVA: 0x0025DE4C File Offset: 0x0025C24C
	public float min
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
				if (this._slider != null)
				{
					this._slider.minValue = this._min;
				}
				if (this._sliderAlt != null)
				{
					this._sliderAlt.minValue = this._min;
				}
			}
		}
	}

	// Token: 0x17000EA6 RID: 3750
	// (get) Token: 0x060063B9 RID: 25529 RVA: 0x0025DEB0 File Offset: 0x0025C2B0
	// (set) Token: 0x060063BA RID: 25530 RVA: 0x0025DEB8 File Offset: 0x0025C2B8
	public float max
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
				if (this._slider != null)
				{
					this._slider.maxValue = this._max;
				}
				if (this._sliderAlt != null)
				{
					this._sliderAlt.maxValue = this._max;
				}
			}
		}
	}

	// Token: 0x060063BB RID: 25531 RVA: 0x0025DF1C File Offset: 0x0025C31C
	public void RegisterSlider(Slider s, bool isAlt = false)
	{
		if (isAlt)
		{
			this.sliderAlt = s;
		}
		else
		{
			this.slider = s;
		}
	}

	// Token: 0x17000EA7 RID: 3751
	// (get) Token: 0x060063BC RID: 25532 RVA: 0x0025DF37 File Offset: 0x0025C337
	// (set) Token: 0x060063BD RID: 25533 RVA: 0x0025DF40 File Offset: 0x0025C340
	public Slider slider
	{
		get
		{
			return this._slider;
		}
		set
		{
			if (this._slider != value)
			{
				if (this._slider != null)
				{
					this._slider.interactable = true;
					this._slider.onValueChanged.RemoveListener(new UnityAction<float>(this.SetVal));
				}
				this._slider = value;
				this._sliderControl = null;
				if (this._slider != null)
				{
					this._slider.interactable = this._interactable;
					this._sliderControl = this._slider.GetComponent<SliderControl>();
					if (this._sliderControl != null)
					{
						this._sliderControl.defaultValue = this._defaultVal;
						this._sliderControl.clamp = this._constrained;
					}
					this._slider.minValue = this._min;
					this._slider.maxValue = this._max;
					this._slider.value = this._val;
					this._slider.onValueChanged.AddListener(new UnityAction<float>(this.SetVal));
				}
			}
		}
	}

	// Token: 0x17000EA8 RID: 3752
	// (get) Token: 0x060063BE RID: 25534 RVA: 0x0025E05A File Offset: 0x0025C45A
	// (set) Token: 0x060063BF RID: 25535 RVA: 0x0025E064 File Offset: 0x0025C464
	public Slider sliderAlt
	{
		get
		{
			return this._sliderAlt;
		}
		set
		{
			if (this._sliderAlt != value)
			{
				if (this._sliderAlt != null)
				{
					this._sliderAlt.interactable = true;
					this._sliderAlt.onValueChanged.RemoveListener(new UnityAction<float>(this.SetVal));
				}
				this._sliderAlt = value;
				this._sliderControlAlt = null;
				if (this._sliderAlt != null)
				{
					this._sliderAlt.interactable = this._interactable;
					this._sliderControlAlt = this._sliderAlt.GetComponent<SliderControl>();
					if (this._sliderControlAlt != null)
					{
						this._sliderControlAlt.defaultValue = this._defaultVal;
						this._sliderControlAlt.clamp = this._constrained;
					}
					this._sliderAlt.minValue = this._min;
					this._sliderAlt.maxValue = this._max;
					this._sliderAlt.value = this._val;
					this._sliderAlt.onValueChanged.AddListener(new UnityAction<float>(this.SetVal));
				}
			}
		}
	}

	// Token: 0x040053E8 RID: 21480
	protected bool _interactable;

	// Token: 0x040053E9 RID: 21481
	protected float _defaultVal;

	// Token: 0x040053EA RID: 21482
	protected float _val;

	// Token: 0x040053EB RID: 21483
	protected bool _constrained;

	// Token: 0x040053EC RID: 21484
	protected float _min;

	// Token: 0x040053ED RID: 21485
	protected float _max;

	// Token: 0x040053EE RID: 21486
	public JSONStorableFloat.SetFloatCallback setCallbackFunction;

	// Token: 0x040053EF RID: 21487
	public JSONStorableFloat.SetJSONFloatCallback setJSONCallbackFunction;

	// Token: 0x040053F0 RID: 21488
	protected SliderControl _sliderControl;

	// Token: 0x040053F1 RID: 21489
	protected Slider _slider;

	// Token: 0x040053F2 RID: 21490
	protected SliderControl _sliderControlAlt;

	// Token: 0x040053F3 RID: 21491
	protected Slider _sliderAlt;

	// Token: 0x02000CE0 RID: 3296
	// (Invoke) Token: 0x060063C1 RID: 25537
	public delegate void SetJSONFloatCallback(JSONStorableFloat jf);

	// Token: 0x02000CE1 RID: 3297
	// (Invoke) Token: 0x060063C5 RID: 25541
	public delegate void SetFloatCallback(float val);
}
