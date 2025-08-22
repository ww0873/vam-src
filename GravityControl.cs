using System;
using System.Runtime.CompilerServices;
using SimpleJSON;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

// Token: 0x02000C17 RID: 3095
public class GravityControl : JSONStorable
{
	// Token: 0x060059E3 RID: 23011 RVA: 0x00210FD9 File Offset: 0x0020F3D9
	public GravityControl()
	{
	}

	// Token: 0x060059E4 RID: 23012 RVA: 0x00211010 File Offset: 0x0020F410
	public override string[] GetCustomParamNames()
	{
		return this.customParamNames;
	}

	// Token: 0x060059E5 RID: 23013 RVA: 0x00211018 File Offset: 0x0020F418
	public override JSONClass GetJSON(bool includePhysical = true, bool includeAppearance = true, bool forceStore = false)
	{
		JSONClass json = base.GetJSON(includePhysical, includeAppearance, forceStore);
		if (includePhysical || forceStore)
		{
			if (this.gravityXSlider != null)
			{
				SliderControl component = this.gravityXSlider.GetComponent<SliderControl>();
				if (component == null || component.defaultValue != this.gravityX || forceStore)
				{
					this.needsStore = true;
					json["gravityX"].AsFloat = this.gravityX;
				}
			}
			if (this.gravityYSlider != null)
			{
				SliderControl component2 = this.gravityYSlider.GetComponent<SliderControl>();
				if (component2 == null || component2.defaultValue != this.gravityY || forceStore)
				{
					this.needsStore = true;
					json["gravityY"].AsFloat = this.gravityY;
				}
			}
			if (this.gravityZSlider != null)
			{
				SliderControl component3 = this.gravityZSlider.GetComponent<SliderControl>();
				if (component3 == null || component3.defaultValue != this.gravityZ || forceStore)
				{
					this.needsStore = true;
					json["gravityZ"].AsFloat = this.gravityZ;
				}
			}
		}
		return json;
	}

	// Token: 0x060059E6 RID: 23014 RVA: 0x00211154 File Offset: 0x0020F554
	public override void RestoreFromJSON(JSONClass jc, bool restorePhysical = true, bool restoreAppearance = true, JSONArray presetAtoms = null, bool setMissingToDefault = true)
	{
		base.RestoreFromJSON(jc, restorePhysical, restoreAppearance, presetAtoms, setMissingToDefault);
		if (!base.physicalLocked && restorePhysical)
		{
			if (!base.IsCustomPhysicalParamLocked("gravityX"))
			{
				if (jc["gravityX"] != null)
				{
					this.gravityX = jc["gravityX"].AsFloat;
				}
				else if (setMissingToDefault && this.gravityXSlider != null)
				{
					SliderControl component = this.gravityXSlider.GetComponent<SliderControl>();
					if (component != null)
					{
						this.gravityX = component.defaultValue;
					}
				}
			}
			if (!base.IsCustomPhysicalParamLocked("gravityY"))
			{
				if (jc["gravityY"] != null)
				{
					this.gravityY = jc["gravityY"].AsFloat;
				}
				else if (setMissingToDefault && this.gravityYSlider != null)
				{
					SliderControl component2 = this.gravityYSlider.GetComponent<SliderControl>();
					if (component2 != null)
					{
						this.gravityY = component2.defaultValue;
					}
				}
			}
			if (!base.IsCustomPhysicalParamLocked("gravityZ"))
			{
				if (jc["gravityZ"] != null)
				{
					this.gravityZ = jc["gravityZ"].AsFloat;
				}
				else if (setMissingToDefault && this.gravityZSlider != null)
				{
					SliderControl component3 = this.gravityZSlider.GetComponent<SliderControl>();
					if (component3 != null)
					{
						this.gravityZ = component3.defaultValue;
					}
				}
			}
		}
	}

	// Token: 0x17000D41 RID: 3393
	// (get) Token: 0x060059E7 RID: 23015 RVA: 0x002112F6 File Offset: 0x0020F6F6
	// (set) Token: 0x060059E8 RID: 23016 RVA: 0x002112FE File Offset: 0x0020F6FE
	public float gravityX
	{
		get
		{
			return this._gravityX;
		}
		set
		{
			if (this._gravityX != value)
			{
				this._gravityX = value;
				if (this.gravityXSlider != null)
				{
					this.gravityXSlider.value = this._gravityX;
				}
				this.SetGravity();
			}
		}
	}

	// Token: 0x17000D42 RID: 3394
	// (get) Token: 0x060059E9 RID: 23017 RVA: 0x0021133B File Offset: 0x0020F73B
	// (set) Token: 0x060059EA RID: 23018 RVA: 0x00211343 File Offset: 0x0020F743
	public float gravityY
	{
		get
		{
			return this._gravityY;
		}
		set
		{
			if (this._gravityY != value)
			{
				this._gravityY = value;
				if (this.gravityYSlider != null)
				{
					this.gravityYSlider.value = this._gravityY;
				}
				this.SetGravity();
			}
		}
	}

	// Token: 0x17000D43 RID: 3395
	// (get) Token: 0x060059EB RID: 23019 RVA: 0x00211380 File Offset: 0x0020F780
	// (set) Token: 0x060059EC RID: 23020 RVA: 0x00211388 File Offset: 0x0020F788
	public float gravityZ
	{
		get
		{
			return this._gravityZ;
		}
		set
		{
			if (this._gravityZ != value)
			{
				this._gravityZ = value;
				if (this.gravityZSlider != null)
				{
					this.gravityZSlider.value = this._gravityZ;
				}
				this.SetGravity();
			}
		}
	}

	// Token: 0x17000D44 RID: 3396
	// (get) Token: 0x060059ED RID: 23021 RVA: 0x002113C5 File Offset: 0x0020F7C5
	// (set) Token: 0x060059EE RID: 23022 RVA: 0x002113D0 File Offset: 0x0020F7D0
	public Vector3 gravity
	{
		get
		{
			return this._setGravity;
		}
		set
		{
			if (this._gravityX != value.x || this._gravityY != value.y || this._gravityZ != value.z)
			{
				this._gravityX = value.x;
				this._gravityY = value.y;
				this._gravityZ = value.z;
				this.SetGravity();
			}
		}
	}

	// Token: 0x060059EF RID: 23023 RVA: 0x00211440 File Offset: 0x0020F840
	private void SetGravity()
	{
		this._setGravity.x = this._gravityX;
		this._setGravity.y = this._gravityY;
		this._setGravity.z = this._gravityZ;
		Physics.gravity = this._setGravity;
	}

	// Token: 0x060059F0 RID: 23024 RVA: 0x00211480 File Offset: 0x0020F880
	public override void InitUI()
	{
		if (this.gravityXSlider != null)
		{
			this.gravityXSlider.value = this._gravityX;
			this.gravityXSlider.onValueChanged.AddListener(new UnityAction<float>(this.<InitUI>m__0));
			SliderControl component = this.gravityXSlider.GetComponent<SliderControl>();
			if (component != null)
			{
				component.defaultValue = this._gravityX;
			}
		}
		if (this.gravityYSlider != null)
		{
			this.gravityYSlider.value = this._gravityY;
			this.gravityYSlider.onValueChanged.AddListener(new UnityAction<float>(this.<InitUI>m__1));
			SliderControl component2 = this.gravityYSlider.GetComponent<SliderControl>();
			if (component2 != null)
			{
				component2.defaultValue = this._gravityY;
			}
		}
		if (this.gravityZSlider != null)
		{
			this.gravityZSlider.value = this._gravityZ;
			this.gravityZSlider.onValueChanged.AddListener(new UnityAction<float>(this.<InitUI>m__2));
			SliderControl component3 = this.gravityZSlider.GetComponent<SliderControl>();
			if (component3 != null)
			{
				component3.defaultValue = this._gravityZ;
			}
		}
	}

	// Token: 0x060059F1 RID: 23025 RVA: 0x002115B3 File Offset: 0x0020F9B3
	private void Start()
	{
		this.InitUI();
		this.SetGravity();
	}

	// Token: 0x060059F2 RID: 23026 RVA: 0x002115C1 File Offset: 0x0020F9C1
	[CompilerGenerated]
	private void <InitUI>m__0(float A_1)
	{
		this.gravityX = this.gravityXSlider.value;
	}

	// Token: 0x060059F3 RID: 23027 RVA: 0x002115D4 File Offset: 0x0020F9D4
	[CompilerGenerated]
	private void <InitUI>m__1(float A_1)
	{
		this.gravityY = this.gravityYSlider.value;
	}

	// Token: 0x060059F4 RID: 23028 RVA: 0x002115E7 File Offset: 0x0020F9E7
	[CompilerGenerated]
	private void <InitUI>m__2(float A_1)
	{
		this.gravityZ = this.gravityZSlider.value;
	}

	// Token: 0x04004A2A RID: 18986
	protected string[] customParamNames = new string[]
	{
		"gravityX",
		"gravityY",
		"gravityZ"
	};

	// Token: 0x04004A2B RID: 18987
	public Slider gravityXSlider;

	// Token: 0x04004A2C RID: 18988
	[SerializeField]
	private float _gravityX;

	// Token: 0x04004A2D RID: 18989
	public Slider gravityYSlider;

	// Token: 0x04004A2E RID: 18990
	[SerializeField]
	private float _gravityY = -9.81f;

	// Token: 0x04004A2F RID: 18991
	public Slider gravityZSlider;

	// Token: 0x04004A30 RID: 18992
	[SerializeField]
	private float _gravityZ;

	// Token: 0x04004A31 RID: 18993
	private Vector3 _setGravity;
}
