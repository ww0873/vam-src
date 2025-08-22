using System;
using SimpleJSON;
using UnityEngine;

// Token: 0x02000CDC RID: 3292
public class JSONStorableColor : JSONStorableParam
{
	// Token: 0x06006385 RID: 25477 RVA: 0x0025E940 File Offset: 0x0025CD40
	public JSONStorableColor(string paramName, HSVColor startingColor)
	{
		this.type = JSONStorable.Type.Color;
		this.name = paramName;
		this._defaultVal = default(HSVColor);
		this._defaultVal.H = startingColor.H;
		this._defaultVal.S = startingColor.S;
		this._defaultVal.V = startingColor.V;
		this._val = default(HSVColor);
		this._val.H = startingColor.H;
		this._val.S = startingColor.S;
		this._val.V = startingColor.V;
		this.setCallbackFunction = null;
		this.setJSONCallbackFunction = null;
	}

	// Token: 0x06006386 RID: 25478 RVA: 0x0025E9FC File Offset: 0x0025CDFC
	public JSONStorableColor(string paramName, HSVColor startingColor, JSONStorableColor.SetHSVColorCallback callback)
	{
		this.type = JSONStorable.Type.Color;
		this.name = paramName;
		this._defaultVal = default(HSVColor);
		this._defaultVal.H = startingColor.H;
		this._defaultVal.S = startingColor.S;
		this._defaultVal.V = startingColor.V;
		this._val = default(HSVColor);
		this._val.H = startingColor.H;
		this._val.S = startingColor.S;
		this._val.V = startingColor.V;
		this.setCallbackFunction = callback;
		this.setJSONCallbackFunction = null;
	}

	// Token: 0x06006387 RID: 25479 RVA: 0x0025EAB8 File Offset: 0x0025CEB8
	public JSONStorableColor(string paramName, HSVColor startingColor, JSONStorableColor.SetJSONColorCallback callback)
	{
		this.type = JSONStorable.Type.Color;
		this.name = paramName;
		this._defaultVal = default(HSVColor);
		this._defaultVal.H = startingColor.H;
		this._defaultVal.S = startingColor.S;
		this._defaultVal.V = startingColor.V;
		this._val = default(HSVColor);
		this._val.H = startingColor.H;
		this._val.S = startingColor.S;
		this._val.V = startingColor.V;
		this.setCallbackFunction = null;
		this.setJSONCallbackFunction = callback;
	}

	// Token: 0x06006388 RID: 25480 RVA: 0x0025EB74 File Offset: 0x0025CF74
	public override bool StoreJSON(JSONClass jc, bool includePhysical = true, bool includeAppearance = true, bool forceStore = false)
	{
		bool flag = this.NeedsStore(jc, includePhysical, includeAppearance) && (forceStore || this._val.H != this._defaultVal.H || this._val.S != this._defaultVal.S || this._val.V != this._defaultVal.V);
		if (flag)
		{
			jc[this.name]["h"].AsFloat = this._val.H;
			jc[this.name]["s"].AsFloat = this._val.S;
			jc[this.name]["v"].AsFloat = this._val.V;
		}
		return flag;
	}

	// Token: 0x06006389 RID: 25481 RVA: 0x0025EC6C File Offset: 0x0025D06C
	protected void Restore(JSONClass jc, bool setMissingToDefault)
	{
		float h = this._val.H;
		float s = this._val.S;
		float v = this._val.V;
		if (jc[this.name] != null)
		{
			if (jc[this.name]["h"] != null)
			{
				h = jc[this.name]["h"].AsFloat;
			}
			else if (setMissingToDefault)
			{
				h = this._defaultVal.H;
			}
			if (jc[this.name]["s"] != null)
			{
				s = jc[this.name]["s"].AsFloat;
			}
			else if (setMissingToDefault)
			{
				s = this._defaultVal.S;
			}
			if (jc[this.name]["v"] != null)
			{
				v = jc[this.name]["v"].AsFloat;
			}
			else if (setMissingToDefault)
			{
				v = this._defaultVal.V;
			}
		}
		else if (setMissingToDefault)
		{
			h = this._defaultVal.H;
			s = this._defaultVal.S;
			v = this._defaultVal.V;
		}
		this.InternalSetVal(h, s, v, true);
	}

	// Token: 0x0600638A RID: 25482 RVA: 0x0025EDEC File Offset: 0x0025D1EC
	public override void RestoreFromJSON(JSONClass jc, bool restorePhysical = true, bool restoreAppearance = true, bool setMissingToDefault = true)
	{
		bool flag = this.NeedsRestore(jc, restorePhysical, restoreAppearance);
		if (flag)
		{
			this.Restore(jc, setMissingToDefault);
		}
	}

	// Token: 0x0600638B RID: 25483 RVA: 0x0025EE14 File Offset: 0x0025D214
	public override void LateRestoreFromJSON(JSONClass jc, bool restorePhysical = true, bool restoreAppearance = true, bool setMissingToDefault = true)
	{
		bool flag = this.NeedsLateRestore(jc, restorePhysical, restoreAppearance);
		if (flag)
		{
			this.Restore(jc, setMissingToDefault);
		}
	}

	// Token: 0x0600638C RID: 25484 RVA: 0x0025EE3A File Offset: 0x0025D23A
	public override void SetDefaultFromCurrent()
	{
		this.defaultVal = this.val;
	}

	// Token: 0x0600638D RID: 25485 RVA: 0x0025EE48 File Offset: 0x0025D248
	public override void SetValToDefault()
	{
		this.val = this.defaultVal;
	}

	// Token: 0x17000E9C RID: 3740
	// (get) Token: 0x0600638E RID: 25486 RVA: 0x0025EE56 File Offset: 0x0025D256
	// (set) Token: 0x0600638F RID: 25487 RVA: 0x0025EE60 File Offset: 0x0025D260
	public HSVColor defaultVal
	{
		get
		{
			return this._defaultVal;
		}
		set
		{
			HSVColor hsvcolor = value;
			if (this._defaultVal.H != hsvcolor.H || this._defaultVal.S != hsvcolor.S || this._defaultVal.V != hsvcolor.V)
			{
				this._defaultVal.H = hsvcolor.H;
				this._defaultVal.S = hsvcolor.S;
				this._defaultVal.V = hsvcolor.V;
				if (this._colorPicker != null)
				{
					this._colorPicker.defaultHue = this._defaultVal.H;
					this._colorPicker.defaultSaturation = this._defaultVal.S;
					this._colorPicker.defaultCvalue = this._defaultVal.V;
				}
				if (this._colorPickerAlt != null)
				{
					this._colorPickerAlt.defaultHue = this._defaultVal.H;
					this._colorPickerAlt.defaultSaturation = this._defaultVal.S;
					this._colorPickerAlt.defaultCvalue = this._defaultVal.V;
				}
			}
		}
	}

	// Token: 0x06006390 RID: 25488 RVA: 0x0025EF90 File Offset: 0x0025D390
	public void SetVal(float h, float s, float v)
	{
		this.InternalSetVal(h, s, v, true);
	}

	// Token: 0x06006391 RID: 25489 RVA: 0x0025EF9C File Offset: 0x0025D39C
	protected void InternalSetVal(float h, float s, float v, bool doCallback)
	{
		if (this._val.H != h || this._val.S != s || this._val.V != v)
		{
			this._val.H = h;
			this._val.S = s;
			this._val.V = v;
			if (this._colorPicker != null)
			{
				this._colorPicker.SetHSV(this._val, true);
			}
			if (this._colorPickerAlt != null)
			{
				this._colorPickerAlt.SetHSV(this._val, true);
			}
			if (doCallback)
			{
				if (this.setCallbackFunction != null)
				{
					this.setCallbackFunction(this._val.H, this._val.S, this._val.V);
				}
				if (this.setJSONCallbackFunction != null)
				{
					this.setJSONCallbackFunction(this);
				}
			}
		}
	}

	// Token: 0x17000E9D RID: 3741
	// (get) Token: 0x06006392 RID: 25490 RVA: 0x0025F09B File Offset: 0x0025D49B
	// (set) Token: 0x06006393 RID: 25491 RVA: 0x0025F0A3 File Offset: 0x0025D4A3
	public HSVColor val
	{
		get
		{
			return this._val;
		}
		set
		{
			this.InternalSetVal(value.H, value.S, value.V, true);
		}
	}

	// Token: 0x17000E9E RID: 3742
	// (get) Token: 0x06006394 RID: 25492 RVA: 0x0025F0C1 File Offset: 0x0025D4C1
	// (set) Token: 0x06006395 RID: 25493 RVA: 0x0025F0C9 File Offset: 0x0025D4C9
	public HSVColor valNoCallback
	{
		get
		{
			return this._val;
		}
		set
		{
			this.InternalSetVal(value.H, value.S, value.V, false);
		}
	}

	// Token: 0x06006396 RID: 25494 RVA: 0x0025F0E8 File Offset: 0x0025D4E8
	public void SetColor(Color c)
	{
		HSVColor hsvcolor = HSVColorPicker.RGBToHSV(c.r, c.g, c.b);
		this.InternalSetVal(hsvcolor.H, hsvcolor.S, hsvcolor.V, true);
	}

	// Token: 0x06006397 RID: 25495 RVA: 0x0025F12C File Offset: 0x0025D52C
	public void RegisterColorPicker(HSVColorPicker cp, bool isAlt = false)
	{
		if (isAlt)
		{
			this.colorPickerAlt = cp;
		}
		else
		{
			this.colorPicker = cp;
		}
	}

	// Token: 0x17000E9F RID: 3743
	// (get) Token: 0x06006398 RID: 25496 RVA: 0x0025F147 File Offset: 0x0025D547
	// (set) Token: 0x06006399 RID: 25497 RVA: 0x0025F150 File Offset: 0x0025D550
	public HSVColorPicker colorPicker
	{
		get
		{
			return this._colorPicker;
		}
		set
		{
			if (this._colorPicker != value)
			{
				if (this._colorPicker != null)
				{
					HSVColorPicker colorPicker = this._colorPicker;
					colorPicker.onHSVColorChangedHandlers = (HSVColorPicker.OnHSVColorChanged)Delegate.Remove(colorPicker.onHSVColorChangedHandlers, new HSVColorPicker.OnHSVColorChanged(this.SetVal));
				}
				this._colorPicker = value;
				if (this._colorPicker != null)
				{
					this._colorPicker.hue = this._val.H;
					this._colorPicker.saturation = this._val.S;
					this._colorPicker.cvalue = this._val.V;
					this._colorPicker.defaultHue = this._defaultVal.H;
					this._colorPicker.defaultSaturation = this._defaultVal.S;
					this._colorPicker.defaultCvalue = this._defaultVal.V;
					HSVColorPicker colorPicker2 = this._colorPicker;
					colorPicker2.onHSVColorChangedHandlers = (HSVColorPicker.OnHSVColorChanged)Delegate.Combine(colorPicker2.onHSVColorChangedHandlers, new HSVColorPicker.OnHSVColorChanged(this.SetVal));
				}
			}
		}
	}

	// Token: 0x17000EA0 RID: 3744
	// (get) Token: 0x0600639A RID: 25498 RVA: 0x0025F269 File Offset: 0x0025D669
	// (set) Token: 0x0600639B RID: 25499 RVA: 0x0025F274 File Offset: 0x0025D674
	public HSVColorPicker colorPickerAlt
	{
		get
		{
			return this._colorPickerAlt;
		}
		set
		{
			if (this._colorPickerAlt != value)
			{
				if (this._colorPickerAlt != null)
				{
					HSVColorPicker colorPickerAlt = this._colorPickerAlt;
					colorPickerAlt.onHSVColorChangedHandlers = (HSVColorPicker.OnHSVColorChanged)Delegate.Remove(colorPickerAlt.onHSVColorChangedHandlers, new HSVColorPicker.OnHSVColorChanged(this.SetVal));
				}
				this._colorPickerAlt = value;
				if (this._colorPickerAlt != null)
				{
					this._colorPickerAlt.hue = this._val.H;
					this._colorPickerAlt.saturation = this._val.S;
					this._colorPickerAlt.cvalue = this._val.V;
					this._colorPickerAlt.defaultHue = this._defaultVal.H;
					this._colorPickerAlt.defaultSaturation = this._defaultVal.S;
					this._colorPickerAlt.defaultCvalue = this._defaultVal.V;
					HSVColorPicker colorPickerAlt2 = this._colorPickerAlt;
					colorPickerAlt2.onHSVColorChangedHandlers = (HSVColorPicker.OnHSVColorChanged)Delegate.Combine(colorPickerAlt2.onHSVColorChangedHandlers, new HSVColorPicker.OnHSVColorChanged(this.SetVal));
				}
			}
		}
	}

	// Token: 0x040053E2 RID: 21474
	protected HSVColor _defaultVal;

	// Token: 0x040053E3 RID: 21475
	protected HSVColor _val;

	// Token: 0x040053E4 RID: 21476
	public JSONStorableColor.SetHSVColorCallback setCallbackFunction;

	// Token: 0x040053E5 RID: 21477
	public JSONStorableColor.SetJSONColorCallback setJSONCallbackFunction;

	// Token: 0x040053E6 RID: 21478
	protected HSVColorPicker _colorPicker;

	// Token: 0x040053E7 RID: 21479
	protected HSVColorPicker _colorPickerAlt;

	// Token: 0x02000CDD RID: 3293
	// (Invoke) Token: 0x0600639D RID: 25501
	public delegate void SetHSVColorCallback(float h, float s, float v);

	// Token: 0x02000CDE RID: 3294
	// (Invoke) Token: 0x060063A1 RID: 25505
	public delegate void SetJSONColorCallback(JSONStorableColor jc);
}
