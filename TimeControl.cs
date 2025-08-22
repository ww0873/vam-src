using System;
using System.Runtime.CompilerServices;
using SimpleJSON;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

// Token: 0x02000C7A RID: 3194
public class TimeControl : JSONStorable
{
	// Token: 0x06005F72 RID: 24434 RVA: 0x0024093C File Offset: 0x0023ED3C
	public TimeControl()
	{
	}

	// Token: 0x06005F73 RID: 24435 RVA: 0x002409A1 File Offset: 0x0023EDA1
	public override string[] GetCustomParamNames()
	{
		return this.customParamNames;
	}

	// Token: 0x06005F74 RID: 24436 RVA: 0x002409AC File Offset: 0x0023EDAC
	public override JSONClass GetJSON(bool includePhysical = true, bool includeAppearance = true, bool forceStore = false)
	{
		JSONClass json = base.GetJSON(includePhysical, includeAppearance, forceStore);
		if ((includePhysical || forceStore) && this.currentScaleSlider != null)
		{
			SliderControl component = this.currentScaleSlider.GetComponent<SliderControl>();
			if (component == null || component.defaultValue != this.currentScale || forceStore)
			{
				this.needsStore = true;
				json["currentScale"].AsFloat = this.currentScale;
			}
		}
		return json;
	}

	// Token: 0x06005F75 RID: 24437 RVA: 0x00240A30 File Offset: 0x0023EE30
	public override void RestoreFromJSON(JSONClass jc, bool restorePhysical = true, bool restoreAppearance = true, JSONArray presetAtoms = null, bool setMissingToDefault = true)
	{
		base.RestoreFromJSON(jc, restorePhysical, restoreAppearance, presetAtoms, setMissingToDefault);
		if (!base.physicalLocked && restorePhysical && !base.IsCustomPhysicalParamLocked("currentScale"))
		{
			if (jc["currentScale"] != null)
			{
				this.currentScale = jc["currentScale"].AsFloat;
			}
			else if (setMissingToDefault && this.currentScaleSlider != null)
			{
				SliderControl component = this.currentScaleSlider.GetComponent<SliderControl>();
				if (component != null)
				{
					this.currentScale = component.defaultValue;
				}
			}
		}
	}

	// Token: 0x17000DFC RID: 3580
	// (get) Token: 0x06005F76 RID: 24438 RVA: 0x00240AD8 File Offset: 0x0023EED8
	// (set) Token: 0x06005F77 RID: 24439 RVA: 0x00240AE0 File Offset: 0x0023EEE0
	public TimeControl.State currentState
	{
		get
		{
			return this._currentState;
		}
		set
		{
			this._currentState = value;
			switch (this._currentState)
			{
			case TimeControl.State.Pause:
				this._currentScale = this.pauseScale;
				break;
			case TimeControl.State.Slow:
				this._currentScale = this.slowScale;
				break;
			case TimeControl.State.Normal:
				this._currentScale = this.normalScale;
				break;
			case TimeControl.State.Fast:
				this._currentScale = this.fastScale;
				break;
			}
			this.SetScale();
		}
	}

	// Token: 0x17000DFD RID: 3581
	// (get) Token: 0x06005F78 RID: 24440 RVA: 0x00240B69 File Offset: 0x0023EF69
	// (set) Token: 0x06005F79 RID: 24441 RVA: 0x00240B74 File Offset: 0x0023EF74
	public float currentScale
	{
		get
		{
			return this._currentScale;
		}
		set
		{
			this._currentScale = value;
			if (this._currentScale > this.highScale)
			{
				this._currentScale = this.highScale;
			}
			else if (this._currentScale < this.lowScale)
			{
				this._currentScale = this.lowScale;
			}
			if (this.currentScaleSlider != null)
			{
				this.currentScaleSlider.value = this._currentScale;
			}
			if (this.currentScaleSliderAlt != null)
			{
				this.currentScaleSliderAlt.value = this._currentScale;
			}
			this._currentState = TimeControl.State.Custom;
			this.SetScale();
		}
	}

	// Token: 0x06005F7A RID: 24442 RVA: 0x00240C18 File Offset: 0x0023F018
	private void SetScale()
	{
		if (this.compensateFixedTimestep)
		{
			float num = this._currentScale / Time.timeScale;
			Time.fixedDeltaTime *= num;
		}
		Time.timeScale = this._currentScale;
	}

	// Token: 0x17000DFE RID: 3582
	// (get) Token: 0x06005F7B RID: 24443 RVA: 0x00240C54 File Offset: 0x0023F054
	// (set) Token: 0x06005F7C RID: 24444 RVA: 0x00240C5F File Offset: 0x0023F05F
	public bool SetSlow
	{
		get
		{
			return this._currentState == TimeControl.State.Slow;
		}
		set
		{
			if (value)
			{
				this.currentState = TimeControl.State.Slow;
			}
			else
			{
				this.currentState = TimeControl.State.Normal;
			}
		}
	}

	// Token: 0x06005F7D RID: 24445 RVA: 0x00240C7C File Offset: 0x0023F07C
	public override void InitUI()
	{
		if (this.currentScaleSlider != null)
		{
			this.currentScaleSlider.value = this._currentScale;
			this.currentScaleSlider.onValueChanged.AddListener(new UnityAction<float>(this.<InitUI>m__0));
			SliderControl component = this.currentScaleSlider.GetComponent<SliderControl>();
			if (component != null)
			{
				component.defaultValue = this._currentScale;
			}
		}
		if (this.currentScaleSliderAlt != null)
		{
			this.currentScaleSliderAlt.value = this._currentScale;
			this.currentScaleSliderAlt.onValueChanged.AddListener(new UnityAction<float>(this.<InitUI>m__1));
			SliderControl component2 = this.currentScaleSliderAlt.GetComponent<SliderControl>();
			if (component2 != null)
			{
				component2.defaultValue = this._currentScale;
			}
		}
	}

	// Token: 0x06005F7E RID: 24446 RVA: 0x00240D4D File Offset: 0x0023F14D
	protected override void Awake()
	{
		if (!this.awakecalled)
		{
			base.Awake();
			TimeControl.singleton = this;
			this.InitUI();
		}
	}

	// Token: 0x06005F7F RID: 24447 RVA: 0x00240D6C File Offset: 0x0023F16C
	[CompilerGenerated]
	private void <InitUI>m__0(float A_1)
	{
		this.currentScale = this.currentScaleSlider.value;
	}

	// Token: 0x06005F80 RID: 24448 RVA: 0x00240D7F File Offset: 0x0023F17F
	[CompilerGenerated]
	private void <InitUI>m__1(float A_1)
	{
		this.currentScale = this.currentScaleSliderAlt.value;
	}

	// Token: 0x04004F2F RID: 20271
	protected string[] customParamNames = new string[]
	{
		"currentScale"
	};

	// Token: 0x04004F30 RID: 20272
	public static TimeControl singleton;

	// Token: 0x04004F31 RID: 20273
	[SerializeField]
	private TimeControl.State _currentState = TimeControl.State.Normal;

	// Token: 0x04004F32 RID: 20274
	public bool compensateFixedTimestep;

	// Token: 0x04004F33 RID: 20275
	public float pauseScale;

	// Token: 0x04004F34 RID: 20276
	public float slowScale = 0.2f;

	// Token: 0x04004F35 RID: 20277
	public float normalScale = 1f;

	// Token: 0x04004F36 RID: 20278
	public float fastScale = 2f;

	// Token: 0x04004F37 RID: 20279
	public float lowScale;

	// Token: 0x04004F38 RID: 20280
	public float highScale = 2f;

	// Token: 0x04004F39 RID: 20281
	public Slider currentScaleSlider;

	// Token: 0x04004F3A RID: 20282
	public Slider currentScaleSliderAlt;

	// Token: 0x04004F3B RID: 20283
	[SerializeField]
	private float _currentScale = 1f;

	// Token: 0x02000C7B RID: 3195
	public enum State
	{
		// Token: 0x04004F3D RID: 20285
		Pause,
		// Token: 0x04004F3E RID: 20286
		Slow,
		// Token: 0x04004F3F RID: 20287
		Normal,
		// Token: 0x04004F40 RID: 20288
		Fast,
		// Token: 0x04004F41 RID: 20289
		Custom
	}
}
