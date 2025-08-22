using System;
using UnityEngine;

// Token: 0x02000D59 RID: 3417
public class CycleForceProducerV2 : ForceProducerV2
{
	// Token: 0x0600690D RID: 26893 RVA: 0x00276508 File Offset: 0x00274908
	public CycleForceProducerV2()
	{
	}

	// Token: 0x0600690E RID: 26894 RVA: 0x00276538 File Offset: 0x00274938
	protected override void SyncForceFactor(float f)
	{
		base.SyncForceFactor(f);
		this.maxForce = f;
	}

	// Token: 0x0600690F RID: 26895 RVA: 0x00276548 File Offset: 0x00274948
	protected override void SyncTorqueFactor(float f)
	{
		base.SyncTorqueFactor(f);
		this.maxTorque = f;
	}

	// Token: 0x06006910 RID: 26896 RVA: 0x00276558 File Offset: 0x00274958
	protected void SyncPeriod(float f)
	{
		this._period = f;
	}

	// Token: 0x17000F83 RID: 3971
	// (get) Token: 0x06006911 RID: 26897 RVA: 0x00276561 File Offset: 0x00274961
	// (set) Token: 0x06006912 RID: 26898 RVA: 0x00276569 File Offset: 0x00274969
	public float period
	{
		get
		{
			return this._period;
		}
		set
		{
			if (this.periodJSON != null)
			{
				this.periodJSON.val = value;
			}
			else if (this._period != value)
			{
				this.SyncPeriod(value);
			}
		}
	}

	// Token: 0x06006913 RID: 26899 RVA: 0x0027659A File Offset: 0x0027499A
	protected void SyncPeriodRatio(float f)
	{
		this._periodRatio = f;
	}

	// Token: 0x17000F84 RID: 3972
	// (get) Token: 0x06006914 RID: 26900 RVA: 0x002765A3 File Offset: 0x002749A3
	// (set) Token: 0x06006915 RID: 26901 RVA: 0x002765AB File Offset: 0x002749AB
	public float periodRatio
	{
		get
		{
			return this._periodRatio;
		}
		set
		{
			if (this.periodRatioJSON != null)
			{
				this.periodRatioJSON.val = value;
			}
			else if (this._periodRatio != value)
			{
				this.SyncPeriodRatio(value);
			}
		}
	}

	// Token: 0x06006916 RID: 26902 RVA: 0x002765DC File Offset: 0x002749DC
	protected void SyncForceDuration(float f)
	{
		this._forceDuration = f;
	}

	// Token: 0x17000F85 RID: 3973
	// (get) Token: 0x06006917 RID: 26903 RVA: 0x002765E5 File Offset: 0x002749E5
	// (set) Token: 0x06006918 RID: 26904 RVA: 0x002765ED File Offset: 0x002749ED
	public float forceDuration
	{
		get
		{
			return this._forceDuration;
		}
		set
		{
			if (this.forceDurationJSON != null)
			{
				this.forceDurationJSON.val = value;
			}
			else if (this._forceDuration != value)
			{
				this.SyncForceDuration(value);
			}
		}
	}

	// Token: 0x06006919 RID: 26905 RVA: 0x0027661E File Offset: 0x00274A1E
	protected void SyncApplyForceOnReturn(bool b)
	{
		this._applyForceOnReturn = b;
	}

	// Token: 0x17000F86 RID: 3974
	// (get) Token: 0x0600691A RID: 26906 RVA: 0x00276627 File Offset: 0x00274A27
	// (set) Token: 0x0600691B RID: 26907 RVA: 0x0027662F File Offset: 0x00274A2F
	public bool applyForceOnReturn
	{
		get
		{
			return this._applyForceOnReturn;
		}
		set
		{
			if (this.applyForceOnReturnJSON != null)
			{
				this.applyForceOnReturnJSON.val = value;
			}
			else if (this._applyForceOnReturn != value)
			{
				this.SyncApplyForceOnReturn(value);
			}
		}
	}

	// Token: 0x0600691C RID: 26908 RVA: 0x00276660 File Offset: 0x00274A60
	protected override void Init()
	{
		base.Init();
		this.SyncForceFactor(this._forceFactor);
		this.SyncTorqueFactor(this._torqueFactor);
		this.periodJSON = new JSONStorableFloat("period", this._period, new JSONStorableFloat.SetFloatCallback(this.SyncPeriod), 0f, 10f, false, true);
		this.periodJSON.storeType = JSONStorableParam.StoreType.Physical;
		base.RegisterFloat(this.periodJSON);
		this.periodRatioJSON = new JSONStorableFloat("periodRatio", this._periodRatio, new JSONStorableFloat.SetFloatCallback(this.SyncPeriodRatio), 0f, 1f, true, true);
		this.periodRatioJSON.storeType = JSONStorableParam.StoreType.Physical;
		base.RegisterFloat(this.periodRatioJSON);
		this.forceDurationJSON = new JSONStorableFloat("forceDuration", this._forceDuration, new JSONStorableFloat.SetFloatCallback(this.SyncForceDuration), 0f, 1f, true, true);
		this.forceDurationJSON.storeType = JSONStorableParam.StoreType.Physical;
		base.RegisterFloat(this.forceDurationJSON);
		this.applyForceOnReturnJSON = new JSONStorableBool("applyForceOnReturn", this._applyForceOnReturn, new JSONStorableBool.SetBoolCallback(this.SyncApplyForceOnReturn));
		this.applyForceOnReturnJSON.storeType = JSONStorableParam.StoreType.Physical;
		base.RegisterBool(this.applyForceOnReturnJSON);
	}

	// Token: 0x0600691D RID: 26909 RVA: 0x00276798 File Offset: 0x00274B98
	public override void InitUI()
	{
		base.InitUI();
		if (this.UITransform != null)
		{
			CycleForceProducerV2UI componentInChildren = this.UITransform.GetComponentInChildren<CycleForceProducerV2UI>(true);
			if (componentInChildren != null)
			{
				this.applyForceOnReturnJSON.toggle = componentInChildren.applyForceOnReturnToggle;
				this.forceDurationJSON.slider = componentInChildren.forceDurationSlider;
				this.periodRatioJSON.slider = componentInChildren.periodRatioSlider;
				this.periodJSON.slider = componentInChildren.periodSlider;
			}
		}
	}

	// Token: 0x0600691E RID: 26910 RVA: 0x0027681C File Offset: 0x00274C1C
	public override void InitUIAlt()
	{
		base.InitUIAlt();
		if (this.UITransformAlt != null)
		{
			CycleForceProducerV2UI componentInChildren = this.UITransformAlt.GetComponentInChildren<CycleForceProducerV2UI>(true);
			if (componentInChildren != null)
			{
				this.applyForceOnReturnJSON.toggleAlt = componentInChildren.applyForceOnReturnToggle;
				this.forceDurationJSON.sliderAlt = componentInChildren.forceDurationSlider;
				this.periodRatioJSON.sliderAlt = componentInChildren.periodRatioSlider;
				this.periodJSON.sliderAlt = componentInChildren.periodSlider;
			}
		}
	}

	// Token: 0x0600691F RID: 26911 RVA: 0x0027689D File Offset: 0x00274C9D
	protected override void Start()
	{
		base.Start();
		this.flip = 1f;
	}

	// Token: 0x06006920 RID: 26912 RVA: 0x002768B0 File Offset: 0x00274CB0
	protected override void Update()
	{
		base.Update();
		this.timer -= Time.deltaTime;
		this.forceTimer -= Time.deltaTime;
		if (this.timer < 0f)
		{
			if ((this.flip > 0f && this._periodRatio != 1f) || this._periodRatio == 0f)
			{
				if (this._applyForceOnReturn)
				{
					this.flip = -1f;
				}
				else
				{
					this.flip = 0f;
				}
				this.timer = this._period * (1f - this._periodRatio);
				this.forceTimer = this._forceDuration * this._period;
			}
			else
			{
				this.flip = 1f;
				this.timer = this._period * this.periodRatio;
				this.forceTimer = this._forceDuration * this._period;
			}
			this.SetTargetForcePercent(this.flip);
		}
		else if (this.forceTimer < 0f)
		{
			this.SetTargetForcePercent(0f);
		}
	}

	// Token: 0x040059D7 RID: 22999
	protected JSONStorableFloat periodJSON;

	// Token: 0x040059D8 RID: 23000
	[SerializeField]
	protected float _period = 1f;

	// Token: 0x040059D9 RID: 23001
	protected JSONStorableFloat periodRatioJSON;

	// Token: 0x040059DA RID: 23002
	[SerializeField]
	protected float _periodRatio = 0.5f;

	// Token: 0x040059DB RID: 23003
	protected JSONStorableFloat forceDurationJSON;

	// Token: 0x040059DC RID: 23004
	[SerializeField]
	protected float _forceDuration = 1f;

	// Token: 0x040059DD RID: 23005
	protected JSONStorableBool applyForceOnReturnJSON;

	// Token: 0x040059DE RID: 23006
	[SerializeField]
	protected bool _applyForceOnReturn = true;

	// Token: 0x040059DF RID: 23007
	protected float timer;

	// Token: 0x040059E0 RID: 23008
	protected float forceTimer;

	// Token: 0x040059E1 RID: 23009
	protected float flip;
}
