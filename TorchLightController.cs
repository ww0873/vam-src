using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000D12 RID: 3346
public class TorchLightController : JSONStorable
{
	// Token: 0x06006636 RID: 26166 RVA: 0x0026A132 File Offset: 0x00268532
	public TorchLightController()
	{
	}

	// Token: 0x06006637 RID: 26167 RVA: 0x0026A148 File Offset: 0x00268548
	protected void SyncIntensity()
	{
		Light component = this.torchLight.GetComponent<Light>();
		if (component != null)
		{
			component.intensity = this.intensity;
		}
		ParticleSystem component2 = this.mainFlame.GetComponent<ParticleSystem>();
		component2.emission.rateOverTime = this.intensity * 20f;
		component2 = this.baseFlame.GetComponent<ParticleSystem>();
		component2.emission.rateOverTime = this.intensity * 15f;
		component2 = this.etincelles.GetComponent<ParticleSystem>();
		component2.emission.rateOverTime = this.intensity * 7f;
		component2 = this.fumee.GetComponent<ParticleSystem>();
		component2.emission.rateOverTime = this.intensity * 12f;
		if (this.flicker != null)
		{
			this.flicker.intensity = this.intensity;
		}
		if (this.flickeringLight != null)
		{
			this.flickeringLight.intensityOrigin = this.intensity;
		}
	}

	// Token: 0x06006638 RID: 26168 RVA: 0x0026A269 File Offset: 0x00268669
	protected void SyncIntensity(float f)
	{
		this._intensity = f;
		this.SyncIntensity();
	}

	// Token: 0x17000F0D RID: 3853
	// (get) Token: 0x06006639 RID: 26169 RVA: 0x0026A278 File Offset: 0x00268678
	// (set) Token: 0x0600663A RID: 26170 RVA: 0x0026A280 File Offset: 0x00268680
	public float intensity
	{
		get
		{
			return this._intensity;
		}
		set
		{
			if (this.intensityJSON != null)
			{
				this.intensityJSON.val = value;
			}
			else if (this._intensity != value)
			{
				this.SyncIntensity(value);
			}
		}
	}

	// Token: 0x0600663B RID: 26171 RVA: 0x0026A2B4 File Offset: 0x002686B4
	protected void Init()
	{
		this.intensityJSON = new JSONStorableFloat("intensity", this._intensity, new JSONStorableFloat.SetFloatCallback(this.SyncIntensity), 0f, 5f, true, true);
		base.RegisterFloat(this.intensityJSON);
		this.SyncIntensity();
	}

	// Token: 0x0600663C RID: 26172 RVA: 0x0026A304 File Offset: 0x00268704
	public override void InitUI()
	{
		if (this.UITransform != null)
		{
			TorchLightControllerUI componentInChildren = this.UITransform.GetComponentInChildren<TorchLightControllerUI>(true);
			if (componentInChildren != null)
			{
				this.intensityJSON.slider = componentInChildren.intensitySlider;
			}
		}
	}

	// Token: 0x0600663D RID: 26173 RVA: 0x0026A34C File Offset: 0x0026874C
	public override void InitUIAlt()
	{
		if (this.UITransformAlt != null)
		{
			TorchLightControllerUI componentInChildren = this.UITransformAlt.GetComponentInChildren<TorchLightControllerUI>(true);
			if (componentInChildren != null)
			{
				this.intensityJSON.sliderAlt = componentInChildren.intensitySlider;
			}
		}
	}

	// Token: 0x0600663E RID: 26174 RVA: 0x0026A394 File Offset: 0x00268794
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

	// Token: 0x040055C8 RID: 21960
	public GameObject torchLight;

	// Token: 0x040055C9 RID: 21961
	public GameObject mainFlame;

	// Token: 0x040055CA RID: 21962
	public GameObject baseFlame;

	// Token: 0x040055CB RID: 21963
	public GameObject etincelles;

	// Token: 0x040055CC RID: 21964
	public GameObject fumee;

	// Token: 0x040055CD RID: 21965
	public Slider intensitySlider;

	// Token: 0x040055CE RID: 21966
	public Slider intensitySliderAlt;

	// Token: 0x040055CF RID: 21967
	public flickering flicker;

	// Token: 0x040055D0 RID: 21968
	public FlickeringLight flickeringLight;

	// Token: 0x040055D1 RID: 21969
	protected JSONStorableFloat intensityJSON;

	// Token: 0x040055D2 RID: 21970
	[SerializeField]
	protected float _intensity = 1f;
}
