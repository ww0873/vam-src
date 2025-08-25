using System;
using UnityEngine;

// Token: 0x02000D24 RID: 3364
public class ParticleSystemControl : JSONStorable
{
	// Token: 0x06006732 RID: 26418 RVA: 0x0026D349 File Offset: 0x0026B749
	public ParticleSystemControl()
	{
	}

	// Token: 0x06006733 RID: 26419 RVA: 0x0026D384 File Offset: 0x0026B784
	protected void SyncEmission()
	{
		if (this.controlEmission)
		{
			foreach (ParticleSystem particleSystem in this.systems1)
			{
				ParticleSystem.EmissionModule emission = particleSystem.emission;
				if (this._emissionEnabled)
				{
					emission.rateOverTime = Mathf.Lerp(this.systems1EmissionRateLow, this.systems1EmissionRateHigh, this._emissionRate);
				}
				else
				{
					emission.rateOverTime = 0f;
				}
			}
			foreach (ParticleSystem particleSystem2 in this.systems2)
			{
				ParticleSystem.EmissionModule emission2 = particleSystem2.emission;
				if (this._emissionEnabled)
				{
					emission2.rateOverTime = Mathf.Lerp(this.systems2EmissionRateLow, this.systems2EmissionRateHigh, this._emissionRate);
				}
				else
				{
					emission2.rateOverTime = 0f;
				}
			}
			if (this.audioSourceControlledByRate != null)
			{
				if (this._emissionEnabled)
				{
					this.audioSourceControlledByRate.volume = Mathf.Lerp(this.audioSourceVolumeLow, this.audioSourceVolumeHigh, this._emissionRate);
				}
				else
				{
					this.audioSourceControlledByRate.volume = 0f;
				}
			}
		}
	}

	// Token: 0x06006734 RID: 26420 RVA: 0x0026D4CE File Offset: 0x0026B8CE
	protected void SyncEmissionEnabled(bool b)
	{
		this._emissionEnabled = b;
		this.SyncEmission();
	}

	// Token: 0x06006735 RID: 26421 RVA: 0x0026D4DD File Offset: 0x0026B8DD
	protected void SyncEmissionRate(float f)
	{
		this._emissionRate = f;
		this.SyncEmission();
	}

	// Token: 0x06006736 RID: 26422 RVA: 0x0026D4EC File Offset: 0x0026B8EC
	protected void SyncSystem1MaterialAlpha(float f)
	{
		foreach (ParticleSystem particleSystem in this.systems1)
		{
			ParticleSystemRenderer component = particleSystem.GetComponent<ParticleSystemRenderer>();
			if (component != null)
			{
				Material material = component.material;
				Color color = material.GetColor("_TintColor");
				color.a = f;
				material.SetColor("_TintColor", color);
			}
		}
	}

	// Token: 0x06006737 RID: 26423 RVA: 0x0026D558 File Offset: 0x0026B958
	protected void SyncSystem2MaterialAlpha(float f)
	{
		foreach (ParticleSystem particleSystem in this.systems2)
		{
			ParticleSystemRenderer component = particleSystem.GetComponent<ParticleSystemRenderer>();
			if (component != null)
			{
				Material material = component.material;
				Color color = material.GetColor("_TintColor");
				color.a = f;
				material.SetColor("_TintColor", color);
			}
		}
	}

	// Token: 0x06006738 RID: 26424 RVA: 0x0026D5C4 File Offset: 0x0026B9C4
	protected void Init()
	{
		if (this.mat1 != null)
		{
			this.system1MaterialAlphaJSON = new JSONStorableFloat("system1MaterialAlpha", this.mat1.GetColor("_TintColor").a, new JSONStorableFloat.SetFloatCallback(this.SyncSystem1MaterialAlpha), 0f, 1f, true, true);
			base.RegisterFloat(this.system1MaterialAlphaJSON);
		}
		if (this.mat2 != null)
		{
			this.system2MaterialAlphaJSON = new JSONStorableFloat("system2MaterialAlpha", this.mat2.GetColor("_TintColor").a, new JSONStorableFloat.SetFloatCallback(this.SyncSystem2MaterialAlpha), 0f, 1f, true, true);
			base.RegisterFloat(this.system2MaterialAlphaJSON);
		}
		if (this.controlEmission)
		{
			this.emissionEnabledJSON = new JSONStorableBool("emissionEnabled", this._emissionEnabled, new JSONStorableBool.SetBoolCallback(this.SyncEmissionEnabled));
			base.RegisterBool(this.emissionEnabledJSON);
			this.emissionRateJSON = new JSONStorableFloat("emissionRate", this._emissionRate, new JSONStorableFloat.SetFloatCallback(this.SyncEmissionRate), 0f, 1f, true, true);
			base.RegisterFloat(this.emissionRateJSON);
			this.SyncEmission();
		}
	}

	// Token: 0x06006739 RID: 26425 RVA: 0x0026D704 File Offset: 0x0026BB04
	protected override void InitUI(Transform t, bool isAlt)
	{
		if (t != null)
		{
			ParticleSystemControlUI componentInChildren = t.GetComponentInChildren<ParticleSystemControlUI>();
			if (componentInChildren != null)
			{
				if (this.system1MaterialAlphaJSON != null)
				{
					this.system1MaterialAlphaJSON.RegisterSlider(componentInChildren.system1MaterialAlphaSlider, isAlt);
				}
				if (this.system2MaterialAlphaJSON != null)
				{
					this.system2MaterialAlphaJSON.RegisterSlider(componentInChildren.system2MaterialAlphaSlider, isAlt);
				}
				if (this.controlEmission)
				{
					this.emissionEnabledJSON.RegisterToggle(componentInChildren.emissionEnabledToggle, isAlt);
					this.emissionRateJSON.RegisterSlider(componentInChildren.emissionRateSlider, isAlt);
				}
			}
		}
	}

	// Token: 0x0600673A RID: 26426 RVA: 0x0026D799 File Offset: 0x0026BB99
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

	// Token: 0x0400582A RID: 22570
	public ParticleSystem[] systems1;

	// Token: 0x0400582B RID: 22571
	public ParticleSystem[] systems2;

	// Token: 0x0400582C RID: 22572
	public Material mat1;

	// Token: 0x0400582D RID: 22573
	public Material mat2;

	// Token: 0x0400582E RID: 22574
	public bool controlEmission;

	// Token: 0x0400582F RID: 22575
	public float systems1EmissionRateLow;

	// Token: 0x04005830 RID: 22576
	public float systems1EmissionRateHigh = 20f;

	// Token: 0x04005831 RID: 22577
	public float systems2EmissionRateLow;

	// Token: 0x04005832 RID: 22578
	public float systems2EmissionRateHigh = 20f;

	// Token: 0x04005833 RID: 22579
	public float audioSourceVolumeLow;

	// Token: 0x04005834 RID: 22580
	public float audioSourceVolumeHigh = 1f;

	// Token: 0x04005835 RID: 22581
	public AudioSource audioSourceControlledByRate;

	// Token: 0x04005836 RID: 22582
	[SerializeField]
	protected bool _emissionEnabled = true;

	// Token: 0x04005837 RID: 22583
	protected JSONStorableBool emissionEnabledJSON;

	// Token: 0x04005838 RID: 22584
	[SerializeField]
	protected float _emissionRate = 1f;

	// Token: 0x04005839 RID: 22585
	protected JSONStorableFloat emissionRateJSON;

	// Token: 0x0400583A RID: 22586
	protected JSONStorableFloat system1MaterialAlphaJSON;

	// Token: 0x0400583B RID: 22587
	protected JSONStorableFloat system2MaterialAlphaJSON;
}
