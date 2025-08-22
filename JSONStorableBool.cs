using System;
using SimpleJSON;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

// Token: 0x02000CD9 RID: 3289
public class JSONStorableBool : JSONStorableParam
{
	// Token: 0x0600635E RID: 25438 RVA: 0x0025E252 File Offset: 0x0025C652
	public JSONStorableBool(string paramName, bool startingValue)
	{
		this.type = JSONStorable.Type.Bool;
		this.name = paramName;
		this.defaultVal = startingValue;
		this.val = startingValue;
		this.setCallbackFunction = null;
		this.setJSONCallbackFunction = null;
	}

	// Token: 0x0600635F RID: 25439 RVA: 0x0025E284 File Offset: 0x0025C684
	public JSONStorableBool(string paramName, bool startingValue, JSONStorableBool.SetBoolCallback callback)
	{
		this.type = JSONStorable.Type.Bool;
		this.name = paramName;
		this.defaultVal = startingValue;
		this.val = startingValue;
		this.setCallbackFunction = callback;
		this.setJSONCallbackFunction = null;
	}

	// Token: 0x06006360 RID: 25440 RVA: 0x0025E2B6 File Offset: 0x0025C6B6
	public JSONStorableBool(string paramName, bool startingValue, JSONStorableBool.SetJSONBoolCallback callback)
	{
		this.type = JSONStorable.Type.Bool;
		this.name = paramName;
		this.defaultVal = startingValue;
		this.val = startingValue;
		this.setCallbackFunction = null;
		this.setJSONCallbackFunction = callback;
	}

	// Token: 0x06006361 RID: 25441 RVA: 0x0025E2E8 File Offset: 0x0025C6E8
	public override bool StoreJSON(JSONClass jc, bool includePhysical = true, bool includeAppearance = true, bool forceStore = false)
	{
		bool flag = this.NeedsStore(jc, includePhysical, includeAppearance) && (forceStore || this._val != this._defaultVal);
		if (flag)
		{
			jc[this.name].AsBool = this._val;
		}
		return flag;
	}

	// Token: 0x06006362 RID: 25442 RVA: 0x0025E340 File Offset: 0x0025C740
	public override void RestoreFromJSON(JSONClass jc, bool restorePhysical = true, bool restoreAppearance = true, bool setMissingToDefault = true)
	{
		bool flag = this.NeedsRestore(jc, restorePhysical, restoreAppearance);
		if (flag)
		{
			if (jc[this.name] != null)
			{
				if (jc[this.name].Value == string.Empty)
				{
					this.val = this._defaultVal;
				}
				else
				{
					this.val = jc[this.name].AsBool;
				}
			}
			else if (this.altName != null)
			{
				if (jc[this.altName] != null)
				{
					this.val = jc[this.altName].AsBool;
				}
				else if (setMissingToDefault)
				{
					this.val = this._defaultVal;
				}
			}
			else if (setMissingToDefault)
			{
				this.val = this._defaultVal;
			}
		}
	}

	// Token: 0x06006363 RID: 25443 RVA: 0x0025E42C File Offset: 0x0025C82C
	public override void LateRestoreFromJSON(JSONClass jc, bool restorePhysical = true, bool restoreAppearance = true, bool setMissingToDefault = true)
	{
		bool flag = this.NeedsLateRestore(jc, restorePhysical, restoreAppearance);
		if (flag)
		{
			if (jc[this.name] != null)
			{
				if (jc[this.name].Value == string.Empty)
				{
					this.val = this._defaultVal;
				}
				else
				{
					this.val = jc[this.name].AsBool;
				}
			}
			else if (this.altName != null)
			{
				if (jc[this.altName] != null)
				{
					this.val = jc[this.altName].AsBool;
				}
				else if (setMissingToDefault)
				{
					this.val = this._defaultVal;
				}
			}
			else if (setMissingToDefault)
			{
				this.val = this._defaultVal;
			}
		}
	}

	// Token: 0x06006364 RID: 25444 RVA: 0x0025E516 File Offset: 0x0025C916
	public override void SetDefaultFromCurrent()
	{
		this.defaultVal = this.val;
	}

	// Token: 0x06006365 RID: 25445 RVA: 0x0025E524 File Offset: 0x0025C924
	public override void SetValToDefault()
	{
		this.val = this.defaultVal;
	}

	// Token: 0x17000E93 RID: 3731
	// (get) Token: 0x06006366 RID: 25446 RVA: 0x0025E532 File Offset: 0x0025C932
	// (set) Token: 0x06006367 RID: 25447 RVA: 0x0025E53A File Offset: 0x0025C93A
	public bool defaultVal
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
			}
		}
	}

	// Token: 0x06006368 RID: 25448 RVA: 0x0025E550 File Offset: 0x0025C950
	protected void InternalSetVal(bool b, bool doCallback = true)
	{
		if (this._val != b)
		{
			this._val = b;
			if (this._toggle != null)
			{
				this._toggle.isOn = this._val;
			}
			if (this._toggleAlt != null)
			{
				this._toggleAlt.isOn = this._val;
			}
			if (this.indicator != null)
			{
				this.indicator.SetActive(this._val);
			}
			if (this.indicatorAlt != null)
			{
				this.indicatorAlt.SetActive(this._val);
			}
			if (this.negativeIndicator != null)
			{
				this.negativeIndicator.SetActive(!this._val);
			}
			if (this.negativeIndicatorAlt != null)
			{
				this.negativeIndicatorAlt.SetActive(!this._val);
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

	// Token: 0x06006369 RID: 25449 RVA: 0x0025E67B File Offset: 0x0025CA7B
	public void SetVal(bool value)
	{
		this.val = value;
	}

	// Token: 0x17000E94 RID: 3732
	// (get) Token: 0x0600636A RID: 25450 RVA: 0x0025E684 File Offset: 0x0025CA84
	// (set) Token: 0x0600636B RID: 25451 RVA: 0x0025E68C File Offset: 0x0025CA8C
	public bool val
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

	// Token: 0x17000E95 RID: 3733
	// (get) Token: 0x0600636C RID: 25452 RVA: 0x0025E696 File Offset: 0x0025CA96
	// (set) Token: 0x0600636D RID: 25453 RVA: 0x0025E69E File Offset: 0x0025CA9E
	public bool valNoCallback
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

	// Token: 0x0600636E RID: 25454 RVA: 0x0025E6A8 File Offset: 0x0025CAA8
	public void RegisterToggle(Toggle t, bool isAlt = false)
	{
		if (isAlt)
		{
			this.toggleAlt = t;
		}
		else
		{
			this.toggle = t;
		}
	}

	// Token: 0x17000E96 RID: 3734
	// (get) Token: 0x0600636F RID: 25455 RVA: 0x0025E6C3 File Offset: 0x0025CAC3
	// (set) Token: 0x06006370 RID: 25456 RVA: 0x0025E6CC File Offset: 0x0025CACC
	public Toggle toggle
	{
		get
		{
			return this._toggle;
		}
		set
		{
			if (this._toggle != value)
			{
				if (this._toggle != null)
				{
					this._toggle.onValueChanged.RemoveListener(new UnityAction<bool>(this.SetVal));
				}
				this._toggle = value;
				if (this._toggle != null)
				{
					this._toggle.isOn = this._val;
					this._toggle.onValueChanged.AddListener(new UnityAction<bool>(this.SetVal));
				}
			}
		}
	}

	// Token: 0x17000E97 RID: 3735
	// (get) Token: 0x06006371 RID: 25457 RVA: 0x0025E75C File Offset: 0x0025CB5C
	// (set) Token: 0x06006372 RID: 25458 RVA: 0x0025E764 File Offset: 0x0025CB64
	public Toggle toggleAlt
	{
		get
		{
			return this._toggleAlt;
		}
		set
		{
			if (this._toggleAlt != value)
			{
				if (this._toggleAlt != null)
				{
					this._toggleAlt.onValueChanged.RemoveListener(new UnityAction<bool>(this.SetVal));
				}
				this._toggleAlt = value;
				if (this._toggleAlt != null)
				{
					this._toggleAlt.isOn = this._val;
					this._toggleAlt.onValueChanged.AddListener(new UnityAction<bool>(this.SetVal));
				}
			}
		}
	}

	// Token: 0x06006373 RID: 25459 RVA: 0x0025E7F4 File Offset: 0x0025CBF4
	public void RegisterIndicator(GameObject go, bool isAlt = false)
	{
		if (isAlt)
		{
			this.indicatorAlt = go;
		}
		else
		{
			this.indicator = go;
		}
	}

	// Token: 0x17000E98 RID: 3736
	// (get) Token: 0x06006374 RID: 25460 RVA: 0x0025E80F File Offset: 0x0025CC0F
	// (set) Token: 0x06006375 RID: 25461 RVA: 0x0025E817 File Offset: 0x0025CC17
	public GameObject indicator
	{
		get
		{
			return this._indicator;
		}
		set
		{
			if (this._indicator != value)
			{
				this._indicator = value;
				if (this._indicator != null)
				{
					this._indicator.SetActive(this._val);
				}
			}
		}
	}

	// Token: 0x17000E99 RID: 3737
	// (get) Token: 0x06006376 RID: 25462 RVA: 0x0025E853 File Offset: 0x0025CC53
	// (set) Token: 0x06006377 RID: 25463 RVA: 0x0025E85B File Offset: 0x0025CC5B
	public GameObject indicatorAlt
	{
		get
		{
			return this._indicatorAlt;
		}
		set
		{
			if (this._indicatorAlt != value)
			{
				this._indicatorAlt = value;
				if (this._indicatorAlt != null)
				{
					this._indicatorAlt.SetActive(this._val);
				}
			}
		}
	}

	// Token: 0x06006378 RID: 25464 RVA: 0x0025E897 File Offset: 0x0025CC97
	public void RegisterNegativeIndicator(GameObject go, bool isAlt = false)
	{
		if (isAlt)
		{
			this.negativeIndicatorAlt = go;
		}
		else
		{
			this.negativeIndicator = go;
		}
	}

	// Token: 0x17000E9A RID: 3738
	// (get) Token: 0x06006379 RID: 25465 RVA: 0x0025E8B2 File Offset: 0x0025CCB2
	// (set) Token: 0x0600637A RID: 25466 RVA: 0x0025E8BA File Offset: 0x0025CCBA
	public GameObject negativeIndicator
	{
		get
		{
			return this._negativeIndicator;
		}
		set
		{
			if (this._negativeIndicator != value)
			{
				this._negativeIndicator = value;
				if (this._negativeIndicator != null)
				{
					this._negativeIndicator.SetActive(!this._val);
				}
			}
		}
	}

	// Token: 0x17000E9B RID: 3739
	// (get) Token: 0x0600637B RID: 25467 RVA: 0x0025E8F9 File Offset: 0x0025CCF9
	// (set) Token: 0x0600637C RID: 25468 RVA: 0x0025E901 File Offset: 0x0025CD01
	public GameObject negativeIndicatorAlt
	{
		get
		{
			return this._negativeIndicatorAlt;
		}
		set
		{
			if (this._negativeIndicatorAlt != value)
			{
				this._negativeIndicatorAlt = value;
				if (this._negativeIndicatorAlt != null)
				{
					this._negativeIndicatorAlt.SetActive(!this._val);
				}
			}
		}
	}

	// Token: 0x040053D8 RID: 21464
	protected bool _defaultVal;

	// Token: 0x040053D9 RID: 21465
	protected bool _val;

	// Token: 0x040053DA RID: 21466
	public JSONStorableBool.SetBoolCallback setCallbackFunction;

	// Token: 0x040053DB RID: 21467
	public JSONStorableBool.SetJSONBoolCallback setJSONCallbackFunction;

	// Token: 0x040053DC RID: 21468
	protected Toggle _toggle;

	// Token: 0x040053DD RID: 21469
	protected Toggle _toggleAlt;

	// Token: 0x040053DE RID: 21470
	protected GameObject _indicator;

	// Token: 0x040053DF RID: 21471
	protected GameObject _indicatorAlt;

	// Token: 0x040053E0 RID: 21472
	protected GameObject _negativeIndicator;

	// Token: 0x040053E1 RID: 21473
	protected GameObject _negativeIndicatorAlt;

	// Token: 0x02000CDA RID: 3290
	// (Invoke) Token: 0x0600637E RID: 25470
	public delegate void SetBoolCallback(bool val);

	// Token: 0x02000CDB RID: 3291
	// (Invoke) Token: 0x06006382 RID: 25474
	public delegate void SetJSONBoolCallback(JSONStorableBool jb);
}
