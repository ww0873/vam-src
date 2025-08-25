using System;
using System.Runtime.CompilerServices;
using SimpleJSON;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

// Token: 0x02000ADA RID: 2778
public class DAZMaleAnatomyControl : JSONStorable
{
	// Token: 0x060049EF RID: 18927 RVA: 0x0017D2DA File Offset: 0x0017B6DA
	public DAZMaleAnatomyControl()
	{
	}

	// Token: 0x060049F0 RID: 18928 RVA: 0x0017D2E4 File Offset: 0x0017B6E4
	public override JSONClass GetJSON(bool includePhysical = true, bool includeAppearance = true, bool forceStore = false)
	{
		JSONClass json = base.GetJSON(includePhysical, includeAppearance, forceStore);
		if (includePhysical || forceStore)
		{
			if (this.stiffnessSlider != null)
			{
				SliderControl component = this.stiffnessSlider.GetComponent<SliderControl>();
				if (component == null || component.defaultValue != this.stiffness || forceStore)
				{
					this.needsStore = true;
					json["stiffness"].AsFloat = this.stiffness;
				}
			}
			if (this.upDownAngleSlider != null)
			{
				SliderControl component2 = this.upDownAngleSlider.GetComponent<SliderControl>();
				if (component2 == null || component2.defaultValue != this.upDownAngle || forceStore)
				{
					this.needsStore = true;
					json["upDownAngle"].AsFloat = this.upDownAngle;
				}
			}
		}
		return json;
	}

	// Token: 0x060049F1 RID: 18929 RVA: 0x0017D3C4 File Offset: 0x0017B7C4
	public override void RestoreFromJSON(JSONClass jc, bool restorePhysical = true, bool restoreAppearance = true, JSONArray presetAtoms = null, bool setMissingToDefault = true)
	{
		base.RestoreFromJSON(jc, restorePhysical, restoreAppearance, presetAtoms, setMissingToDefault);
		if (restorePhysical)
		{
			if (jc["stiffness"] != null)
			{
				this.stiffness = jc["stiffness"].AsFloat;
			}
			else if (setMissingToDefault && this.stiffnessSlider != null)
			{
				SliderControl component = this.stiffnessSlider.GetComponent<SliderControl>();
				if (component != null)
				{
					this.stiffness = component.defaultValue;
				}
			}
			if (jc["upDownAngle"] != null)
			{
				this.upDownAngle = jc["upDownAngle"].AsFloat;
			}
			else if (setMissingToDefault && this.upDownAngleSlider != null)
			{
				SliderControl component2 = this.upDownAngleSlider.GetComponent<SliderControl>();
				if (component2 != null)
				{
					this.upDownAngle = component2.defaultValue;
				}
			}
		}
	}

	// Token: 0x17000A5B RID: 2651
	// (get) Token: 0x060049F2 RID: 18930 RVA: 0x0017D4BE File Offset: 0x0017B8BE
	// (set) Token: 0x060049F3 RID: 18931 RVA: 0x0017D4C8 File Offset: 0x0017B8C8
	public float stiffness
	{
		get
		{
			return this._stiffness;
		}
		set
		{
			if (this._stiffness != value)
			{
				this._stiffness = value;
				if (this.stiffnessSlider != null)
				{
					this.stiffnessSlider.value = value;
				}
				if (this.stiffnessSprings != null)
				{
					this.stiffnessSprings.percent = value;
				}
			}
		}
	}

	// Token: 0x17000A5C RID: 2652
	// (get) Token: 0x060049F4 RID: 18932 RVA: 0x0017D522 File Offset: 0x0017B922
	// (set) Token: 0x060049F5 RID: 18933 RVA: 0x0017D52C File Offset: 0x0017B92C
	public float upDownAngle
	{
		get
		{
			return this._upDownAngle;
		}
		set
		{
			if (this._upDownAngle != value)
			{
				this._upDownAngle = value;
				if (this.upDownAngleSlider != null)
				{
					this.upDownAngleSlider.value = value;
				}
				if (this.upDownAngleTarget != null)
				{
					this.upDownAngleTarget.currentTargetRotationX = value;
				}
			}
		}
	}

	// Token: 0x060049F6 RID: 18934 RVA: 0x0017D588 File Offset: 0x0017B988
	public override void InitUI()
	{
		if (this.stiffnessSlider != null)
		{
			this.stiffnessSlider.value = this._stiffness;
			this.stiffnessSlider.onValueChanged.AddListener(new UnityAction<float>(this.<InitUI>m__0));
			SliderControl component = this.stiffnessSlider.GetComponent<SliderControl>();
			if (component != null)
			{
				component.defaultValue = this._stiffness;
			}
		}
		if (this.upDownAngleSlider != null)
		{
			this.upDownAngleSlider.value = this._upDownAngle;
			this.upDownAngleSlider.onValueChanged.AddListener(new UnityAction<float>(this.<InitUI>m__1));
			SliderControl component2 = this.upDownAngleSlider.GetComponent<SliderControl>();
			if (component2 != null)
			{
				component2.defaultValue = this._upDownAngle;
			}
		}
	}

	// Token: 0x060049F7 RID: 18935 RVA: 0x0017D659 File Offset: 0x0017BA59
	private void Start()
	{
		this.InitUI();
	}

	// Token: 0x060049F8 RID: 18936 RVA: 0x0017D661 File Offset: 0x0017BA61
	[CompilerGenerated]
	private void <InitUI>m__0(float A_1)
	{
		this.stiffness = this.stiffnessSlider.value;
	}

	// Token: 0x060049F9 RID: 18937 RVA: 0x0017D674 File Offset: 0x0017BA74
	[CompilerGenerated]
	private void <InitUI>m__1(float A_1)
	{
		this.upDownAngle = this.upDownAngleSlider.value;
	}

	// Token: 0x04003889 RID: 14473
	public AdjustJointSprings stiffnessSprings;

	// Token: 0x0400388A RID: 14474
	public Slider stiffnessSlider;

	// Token: 0x0400388B RID: 14475
	[SerializeField]
	private float _stiffness;

	// Token: 0x0400388C RID: 14476
	public AdjustJointTarget upDownAngleTarget;

	// Token: 0x0400388D RID: 14477
	public Slider upDownAngleSlider;

	// Token: 0x0400388E RID: 14478
	[SerializeField]
	private float _upDownAngle;
}
