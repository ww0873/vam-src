using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

// Token: 0x02000DEF RID: 3567
public class UIDynamicSlider : UIDynamic
{
	// Token: 0x06006E3F RID: 28223 RVA: 0x002961DF File Offset: 0x002945DF
	public UIDynamicSlider()
	{
	}

	// Token: 0x06006E40 RID: 28224 RVA: 0x002961FC File Offset: 0x002945FC
	public void Configure(string labelString, float min, float max, float defaultValue, bool clamp = true, string valFormat = "F2", bool showQuickButtons = true, bool showRangeAdjust = false)
	{
		this.label = labelString;
		if (this.slider != null)
		{
			this.slider.minValue = min;
			this.slider.maxValue = max;
			this.slider.value = defaultValue;
		}
		if (this.sliderControl != null)
		{
			this.sliderControl.defaultValue = defaultValue;
			this.sliderControl.clamp = clamp;
		}
		this.valueFormat = valFormat;
		this.quickButtonsEnabled = showQuickButtons;
		this.rangeAdjustEnabled = showRangeAdjust;
	}

	// Token: 0x17001020 RID: 4128
	// (get) Token: 0x06006E41 RID: 28225 RVA: 0x00296289 File Offset: 0x00294689
	// (set) Token: 0x06006E42 RID: 28226 RVA: 0x002962A9 File Offset: 0x002946A9
	public string label
	{
		get
		{
			if (this.labelText != null)
			{
				return this.labelText.text;
			}
			return null;
		}
		set
		{
			if (this.labelText != null)
			{
				this.labelText.text = value;
			}
		}
	}

	// Token: 0x17001021 RID: 4129
	// (get) Token: 0x06006E43 RID: 28227 RVA: 0x002962C8 File Offset: 0x002946C8
	// (set) Token: 0x06006E44 RID: 28228 RVA: 0x002962E8 File Offset: 0x002946E8
	public string valueFormat
	{
		get
		{
			if (this.sliderValueTextFromFloat != null)
			{
				return this.sliderValueTextFromFloat.floatFormat;
			}
			return null;
		}
		set
		{
			if (this.sliderValueTextFromFloat != null && this.sliderValueTextFromFloat.floatFormat != value)
			{
				this.sliderValueTextFromFloat.floatFormat = value;
				this.sliderValueTextFromFloat.SyncText();
				if (this._autoSetQuickButtons)
				{
					this.AutoSetButtons();
				}
			}
		}
	}

	// Token: 0x06006E45 RID: 28229 RVA: 0x00296344 File Offset: 0x00294744
	protected void SyncQuickButtonsGroup()
	{
		if (this.quickButtonsGroup != null)
		{
			this.quickButtonsGroup.gameObject.SetActive(this._quickButtonsEnabled);
		}
	}

	// Token: 0x17001022 RID: 4130
	// (get) Token: 0x06006E46 RID: 28230 RVA: 0x0029636D File Offset: 0x0029476D
	// (set) Token: 0x06006E47 RID: 28231 RVA: 0x00296375 File Offset: 0x00294775
	public bool quickButtonsEnabled
	{
		get
		{
			return this._quickButtonsEnabled;
		}
		set
		{
			if (this._quickButtonsEnabled != value)
			{
				this._quickButtonsEnabled = value;
				this.SyncQuickButtonsGroup();
			}
		}
	}

	// Token: 0x17001023 RID: 4131
	// (get) Token: 0x06006E48 RID: 28232 RVA: 0x00296390 File Offset: 0x00294790
	// (set) Token: 0x06006E49 RID: 28233 RVA: 0x00296398 File Offset: 0x00294798
	public bool autoSetQuickButtons
	{
		get
		{
			return this._autoSetQuickButtons;
		}
		set
		{
			if (this._autoSetQuickButtons != value)
			{
				this._autoSetQuickButtons = value;
				this.AutoSetButtons();
			}
		}
	}

	// Token: 0x06006E4A RID: 28234 RVA: 0x002963B4 File Offset: 0x002947B4
	protected void InitQuickButton(Button b)
	{
		if (b != null)
		{
			UIDynamicSlider.<InitQuickButton>c__AnonStorey0 <InitQuickButton>c__AnonStorey = new UIDynamicSlider.<InitQuickButton>c__AnonStorey0();
			<InitQuickButton>c__AnonStorey.$this = this;
			<InitQuickButton>c__AnonStorey.f = this.GetQuickButtonValue(b);
			if (this.valueFormat.StartsWith("P"))
			{
				<InitQuickButton>c__AnonStorey.f *= 0.01f;
			}
			b.onClick.RemoveAllListeners();
			b.onClick.AddListener(new UnityAction(<InitQuickButton>c__AnonStorey.<>m__0));
		}
	}

	// Token: 0x06006E4B RID: 28235 RVA: 0x00296430 File Offset: 0x00294830
	protected void InitQuickButtons()
	{
		this.InitQuickButton(this.quickButtonM1);
		this.InitQuickButton(this.quickButtonM2);
		this.InitQuickButton(this.quickButtonM3);
		this.InitQuickButton(this.quickButtonM4);
		this.InitQuickButton(this.quickButtonP1);
		this.InitQuickButton(this.quickButtonP2);
		this.InitQuickButton(this.quickButtonP3);
		this.InitQuickButton(this.quickButtonP4);
	}

	// Token: 0x06006E4C RID: 28236 RVA: 0x002964A0 File Offset: 0x002948A0
	public void ConfigureQuickButton(Button b, float qv)
	{
		UIDynamicSlider.<ConfigureQuickButton>c__AnonStorey1 <ConfigureQuickButton>c__AnonStorey = new UIDynamicSlider.<ConfigureQuickButton>c__AnonStorey1();
		<ConfigureQuickButton>c__AnonStorey.qv = qv;
		<ConfigureQuickButton>c__AnonStorey.$this = this;
		if (b != null)
		{
			Text componentInChildren = b.GetComponentInChildren<Text>();
			if (componentInChildren != null)
			{
				if (<ConfigureQuickButton>c__AnonStorey.qv > 0f)
				{
					componentInChildren.text = "+" + <ConfigureQuickButton>c__AnonStorey.qv.ToString();
				}
				else
				{
					componentInChildren.text = <ConfigureQuickButton>c__AnonStorey.qv.ToString();
				}
			}
			if (Application.isPlaying)
			{
				b.onClick.RemoveAllListeners();
				b.onClick.AddListener(new UnityAction(<ConfigureQuickButton>c__AnonStorey.<>m__0));
			}
		}
	}

	// Token: 0x06006E4D RID: 28237 RVA: 0x0029655C File Offset: 0x0029495C
	public void ConfigureQuickButton(Button b, float qv, float sv)
	{
		UIDynamicSlider.<ConfigureQuickButton>c__AnonStorey2 <ConfigureQuickButton>c__AnonStorey = new UIDynamicSlider.<ConfigureQuickButton>c__AnonStorey2();
		<ConfigureQuickButton>c__AnonStorey.sv = sv;
		<ConfigureQuickButton>c__AnonStorey.$this = this;
		if (b != null)
		{
			Text componentInChildren = b.GetComponentInChildren<Text>();
			if (componentInChildren != null)
			{
				if (qv > 0f)
				{
					componentInChildren.text = "+" + qv.ToString();
				}
				else
				{
					componentInChildren.text = qv.ToString();
				}
			}
			if (Application.isPlaying)
			{
				b.onClick.RemoveAllListeners();
				b.onClick.AddListener(new UnityAction(<ConfigureQuickButton>c__AnonStorey.<>m__0));
			}
		}
	}

	// Token: 0x06006E4E RID: 28238 RVA: 0x00296608 File Offset: 0x00294A08
	public float GetQuickButtonValue(Button b)
	{
		float result = 0f;
		if (b != null)
		{
			Text componentInChildren = b.GetComponentInChildren<Text>();
			if (componentInChildren != null)
			{
				string text = componentInChildren.text;
				text.Replace("+", string.Empty);
				float.TryParse(componentInChildren.text, out result);
			}
		}
		return result;
	}

	// Token: 0x06006E4F RID: 28239 RVA: 0x00296664 File Offset: 0x00294A64
	public void ConfigureQuickButtons(float m1v, float m2v, float m3v, float m4v, float p1v, float p2v, float p3v, float p4v)
	{
		this.ConfigureQuickButton(this.quickButtonM1, m1v);
		this.ConfigureQuickButton(this.quickButtonM2, m2v);
		this.ConfigureQuickButton(this.quickButtonM3, m3v);
		this.ConfigureQuickButton(this.quickButtonM4, m4v);
		this.ConfigureQuickButton(this.quickButtonP1, p1v);
		this.ConfigureQuickButton(this.quickButtonP2, p2v);
		this.ConfigureQuickButton(this.quickButtonP3, p3v);
		this.ConfigureQuickButton(this.quickButtonP4, p4v);
	}

	// Token: 0x06006E50 RID: 28240 RVA: 0x002966E0 File Offset: 0x00294AE0
	public void AutoSetButtons()
	{
		string valueFormat = this.valueFormat;
		switch (valueFormat)
		{
		case "F0":
			this.ConfigureQuickButton(this.quickButtonM1, -1f);
			this.ConfigureQuickButton(this.quickButtonM2, -10f);
			this.ConfigureQuickButton(this.quickButtonM3, -100f);
			this.ConfigureQuickButton(this.quickButtonM4, -1000f);
			this.ConfigureQuickButton(this.quickButtonP1, 1f);
			this.ConfigureQuickButton(this.quickButtonP2, 10f);
			this.ConfigureQuickButton(this.quickButtonP3, 100f);
			this.ConfigureQuickButton(this.quickButtonP4, 1000f);
			break;
		case "F1":
			this.ConfigureQuickButton(this.quickButtonM1, -0.1f);
			this.ConfigureQuickButton(this.quickButtonM2, -1f);
			this.ConfigureQuickButton(this.quickButtonM3, -10f);
			this.ConfigureQuickButton(this.quickButtonM4, -100f);
			this.ConfigureQuickButton(this.quickButtonP1, 0.1f);
			this.ConfigureQuickButton(this.quickButtonP2, 1f);
			this.ConfigureQuickButton(this.quickButtonP3, 10f);
			this.ConfigureQuickButton(this.quickButtonP4, 100f);
			break;
		case "F2":
			this.ConfigureQuickButton(this.quickButtonM1, -0.01f);
			this.ConfigureQuickButton(this.quickButtonM2, -0.1f);
			this.ConfigureQuickButton(this.quickButtonM3, -1f);
			this.ConfigureQuickButton(this.quickButtonM4, -10f);
			this.ConfigureQuickButton(this.quickButtonP1, 0.01f);
			this.ConfigureQuickButton(this.quickButtonP2, 0.1f);
			this.ConfigureQuickButton(this.quickButtonP3, 1f);
			this.ConfigureQuickButton(this.quickButtonP4, 10f);
			break;
		case "F3":
			this.ConfigureQuickButton(this.quickButtonM1, -0.001f);
			this.ConfigureQuickButton(this.quickButtonM2, -0.01f);
			this.ConfigureQuickButton(this.quickButtonM3, -0.1f);
			this.ConfigureQuickButton(this.quickButtonM4, -1f);
			this.ConfigureQuickButton(this.quickButtonP1, 0.001f);
			this.ConfigureQuickButton(this.quickButtonP2, 0.01f);
			this.ConfigureQuickButton(this.quickButtonP3, 0.1f);
			this.ConfigureQuickButton(this.quickButtonP4, 1f);
			break;
		case "F4":
			this.ConfigureQuickButton(this.quickButtonM1, -0.0001f);
			this.ConfigureQuickButton(this.quickButtonM2, -0.001f);
			this.ConfigureQuickButton(this.quickButtonM3, -0.01f);
			this.ConfigureQuickButton(this.quickButtonM4, -0.1f);
			this.ConfigureQuickButton(this.quickButtonP1, 0.0001f);
			this.ConfigureQuickButton(this.quickButtonP2, 0.001f);
			this.ConfigureQuickButton(this.quickButtonP3, 0.01f);
			this.ConfigureQuickButton(this.quickButtonP4, 0.1f);
			break;
		case "F5":
			this.ConfigureQuickButton(this.quickButtonM1, -1E-05f);
			this.ConfigureQuickButton(this.quickButtonM2, -0.0001f);
			this.ConfigureQuickButton(this.quickButtonM3, -0.001f);
			this.ConfigureQuickButton(this.quickButtonM4, -0.01f);
			this.ConfigureQuickButton(this.quickButtonP1, 1E-05f);
			this.ConfigureQuickButton(this.quickButtonP2, 0.0001f);
			this.ConfigureQuickButton(this.quickButtonP3, 0.001f);
			this.ConfigureQuickButton(this.quickButtonP4, 0.01f);
			break;
		case "P0":
			this.ConfigureQuickButton(this.quickButtonM1, -1f, -0.01f);
			this.ConfigureQuickButton(this.quickButtonM2, -10f, -0.1f);
			this.ConfigureQuickButton(this.quickButtonM3, -100f, -1f);
			this.ConfigureQuickButton(this.quickButtonM4, -1000f, -10f);
			this.ConfigureQuickButton(this.quickButtonP1, 1f, 0.01f);
			this.ConfigureQuickButton(this.quickButtonP2, 10f, 0.1f);
			this.ConfigureQuickButton(this.quickButtonP3, 100f, 1f);
			this.ConfigureQuickButton(this.quickButtonP4, 1000f, 10f);
			break;
		case "P1":
			this.ConfigureQuickButton(this.quickButtonM1, -0.1f, -0.001f);
			this.ConfigureQuickButton(this.quickButtonM2, -1f, -0.01f);
			this.ConfigureQuickButton(this.quickButtonM3, -10f, -0.1f);
			this.ConfigureQuickButton(this.quickButtonM4, -100f, -1f);
			this.ConfigureQuickButton(this.quickButtonP1, 0.1f, 0.001f);
			this.ConfigureQuickButton(this.quickButtonP2, 1f, 0.01f);
			this.ConfigureQuickButton(this.quickButtonP3, 10f, 0.1f);
			this.ConfigureQuickButton(this.quickButtonP4, 100f, 1f);
			break;
		case "P2":
			this.ConfigureQuickButton(this.quickButtonM1, -0.01f, -0.0001f);
			this.ConfigureQuickButton(this.quickButtonM2, -0.1f, -0.001f);
			this.ConfigureQuickButton(this.quickButtonM3, -1f, -0.01f);
			this.ConfigureQuickButton(this.quickButtonM4, -10f, -0.1f);
			this.ConfigureQuickButton(this.quickButtonP1, 0.01f, 0.0001f);
			this.ConfigureQuickButton(this.quickButtonP2, 0.1f, 0.001f);
			this.ConfigureQuickButton(this.quickButtonP3, 1f, 0.01f);
			this.ConfigureQuickButton(this.quickButtonP4, 10f, 0.1f);
			break;
		}
	}

	// Token: 0x06006E51 RID: 28241 RVA: 0x00296D38 File Offset: 0x00295138
	protected void SyncRangeAdjustGroup()
	{
		if (this.rangeAdjustGroup != null)
		{
			this.rangeAdjustGroup.gameObject.SetActive(this._rangeAdjustEnabled);
			if (this.slider != null)
			{
				RectTransform component = this.slider.GetComponent<RectTransform>();
				Vector2 offsetMax = component.offsetMax;
				if (this._rangeAdjustEnabled)
				{
					offsetMax.x = -90f;
				}
				else
				{
					offsetMax.x = -10f;
				}
				component.offsetMax = offsetMax;
			}
		}
	}

	// Token: 0x17001024 RID: 4132
	// (get) Token: 0x06006E52 RID: 28242 RVA: 0x00296DBF File Offset: 0x002951BF
	// (set) Token: 0x06006E53 RID: 28243 RVA: 0x00296DC7 File Offset: 0x002951C7
	public bool rangeAdjustEnabled
	{
		get
		{
			return this._rangeAdjustEnabled;
		}
		set
		{
			if (this._rangeAdjustEnabled != value)
			{
				this._rangeAdjustEnabled = value;
				this.SyncRangeAdjustGroup();
			}
		}
	}

	// Token: 0x17001025 RID: 4133
	// (get) Token: 0x06006E54 RID: 28244 RVA: 0x00296DE2 File Offset: 0x002951E2
	// (set) Token: 0x06006E55 RID: 28245 RVA: 0x00296E06 File Offset: 0x00295206
	public float defaultValue
	{
		get
		{
			if (this.sliderControl != null)
			{
				return this.sliderControl.defaultValue;
			}
			return 0f;
		}
		set
		{
			if (this.sliderControl != null)
			{
				this.sliderControl.defaultValue = value;
			}
		}
	}

	// Token: 0x06006E56 RID: 28246 RVA: 0x00296E25 File Offset: 0x00295225
	protected void SyncDefaultButton()
	{
		if (this.defaultButton != null)
		{
			this.defaultButton.gameObject.SetActive(this._defaultButtonEnabled);
		}
	}

	// Token: 0x17001026 RID: 4134
	// (get) Token: 0x06006E57 RID: 28247 RVA: 0x00296E4E File Offset: 0x0029524E
	// (set) Token: 0x06006E58 RID: 28248 RVA: 0x00296E56 File Offset: 0x00295256
	public bool defaultButtonEnabled
	{
		get
		{
			return this._defaultButtonEnabled;
		}
		set
		{
			if (this._defaultButtonEnabled != value)
			{
				this._defaultButtonEnabled = value;
				this.SyncDefaultButton();
			}
		}
	}

	// Token: 0x06006E59 RID: 28249 RVA: 0x00296E71 File Offset: 0x00295271
	protected void Init()
	{
		this.InitQuickButtons();
		this.SyncQuickButtonsGroup();
		this.SyncRangeAdjustGroup();
		this.SyncDefaultButton();
	}

	// Token: 0x06006E5A RID: 28250 RVA: 0x00296E8B File Offset: 0x0029528B
	private void Awake()
	{
		this.Init();
	}

	// Token: 0x04005F6C RID: 24428
	public Text labelText;

	// Token: 0x04005F6D RID: 24429
	public Slider slider;

	// Token: 0x04005F6E RID: 24430
	public SliderControl sliderControl;

	// Token: 0x04005F6F RID: 24431
	public SetTextFromFloat sliderValueTextFromFloat;

	// Token: 0x04005F70 RID: 24432
	public RectTransform quickButtonsGroup;

	// Token: 0x04005F71 RID: 24433
	public Button quickButtonM1;

	// Token: 0x04005F72 RID: 24434
	public Button quickButtonM2;

	// Token: 0x04005F73 RID: 24435
	public Button quickButtonM3;

	// Token: 0x04005F74 RID: 24436
	public Button quickButtonM4;

	// Token: 0x04005F75 RID: 24437
	public Button quickButtonP1;

	// Token: 0x04005F76 RID: 24438
	public Button quickButtonP2;

	// Token: 0x04005F77 RID: 24439
	public Button quickButtonP3;

	// Token: 0x04005F78 RID: 24440
	public Button quickButtonP4;

	// Token: 0x04005F79 RID: 24441
	[HideInInspector]
	[SerializeField]
	protected bool _quickButtonsEnabled = true;

	// Token: 0x04005F7A RID: 24442
	[HideInInspector]
	[SerializeField]
	protected bool _autoSetQuickButtons = true;

	// Token: 0x04005F7B RID: 24443
	public RectTransform rangeAdjustGroup;

	// Token: 0x04005F7C RID: 24444
	[HideInInspector]
	[SerializeField]
	protected bool _rangeAdjustEnabled;

	// Token: 0x04005F7D RID: 24445
	public Button defaultButton;

	// Token: 0x04005F7E RID: 24446
	[HideInInspector]
	[SerializeField]
	protected bool _defaultButtonEnabled = true;

	// Token: 0x04005F7F RID: 24447
	[CompilerGenerated]
	private static Dictionary<string, int> <>f__switch$map3;

	// Token: 0x0200103C RID: 4156
	[CompilerGenerated]
	private sealed class <InitQuickButton>c__AnonStorey0
	{
		// Token: 0x06007791 RID: 30609 RVA: 0x00296E93 File Offset: 0x00295293
		public <InitQuickButton>c__AnonStorey0()
		{
		}

		// Token: 0x06007792 RID: 30610 RVA: 0x00296E9B File Offset: 0x0029529B
		internal void <>m__0()
		{
			this.$this.sliderControl.incrementSlider(this.f);
		}

		// Token: 0x04006B93 RID: 27539
		internal float f;

		// Token: 0x04006B94 RID: 27540
		internal UIDynamicSlider $this;
	}

	// Token: 0x0200103D RID: 4157
	[CompilerGenerated]
	private sealed class <ConfigureQuickButton>c__AnonStorey1
	{
		// Token: 0x06007793 RID: 30611 RVA: 0x00296EB3 File Offset: 0x002952B3
		public <ConfigureQuickButton>c__AnonStorey1()
		{
		}

		// Token: 0x06007794 RID: 30612 RVA: 0x00296EBB File Offset: 0x002952BB
		internal void <>m__0()
		{
			this.$this.sliderControl.incrementSlider(this.qv);
		}

		// Token: 0x04006B95 RID: 27541
		internal float qv;

		// Token: 0x04006B96 RID: 27542
		internal UIDynamicSlider $this;
	}

	// Token: 0x0200103E RID: 4158
	[CompilerGenerated]
	private sealed class <ConfigureQuickButton>c__AnonStorey2
	{
		// Token: 0x06007795 RID: 30613 RVA: 0x00296ED3 File Offset: 0x002952D3
		public <ConfigureQuickButton>c__AnonStorey2()
		{
		}

		// Token: 0x06007796 RID: 30614 RVA: 0x00296EDB File Offset: 0x002952DB
		internal void <>m__0()
		{
			this.$this.sliderControl.incrementSlider(this.sv);
		}

		// Token: 0x04006B97 RID: 27543
		internal float sv;

		// Token: 0x04006B98 RID: 27544
		internal UIDynamicSlider $this;
	}
}
