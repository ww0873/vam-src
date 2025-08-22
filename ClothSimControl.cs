using System;
using GPUTools.Cloth.Scripts;
using GPUTools.Skinner.Scripts.Providers;
using MeshVR;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

// Token: 0x02000D53 RID: 3411
public class ClothSimControl : PhysicsSimulatorJSONStorable
{
	// Token: 0x060068CC RID: 26828 RVA: 0x002734F7 File Offset: 0x002718F7
	public ClothSimControl()
	{
	}

	// Token: 0x060068CD RID: 26829 RVA: 0x002734FF File Offset: 0x002718FF
	public override void ScaleChanged(float scale)
	{
		base.ScaleChanged(scale);
		if (this.clothSettings != null)
		{
			this.clothSettings.WorldScale = scale;
		}
		this.Reset();
	}

	// Token: 0x060068CE RID: 26830 RVA: 0x0027352B File Offset: 0x0027192B
	public override void PostRestore()
	{
		base.PostRestore();
		this.Reset();
	}

	// Token: 0x060068CF RID: 26831 RVA: 0x00273539 File Offset: 0x00271939
	protected void SyncSimEnabled(bool b)
	{
		if (this.clothSettings != null)
		{
			this.clothSettings.enabled = b;
			if (b)
			{
				this.Reset();
			}
		}
	}

	// Token: 0x060068D0 RID: 26832 RVA: 0x00273564 File Offset: 0x00271964
	public void SetSimEnabled(bool b)
	{
		if (this.simEnabledJSON != null)
		{
			this.simEnabledJSON.val = b;
		}
	}

	// Token: 0x060068D1 RID: 26833 RVA: 0x0027357D File Offset: 0x0027197D
	protected void SyncIntegrateEnabled(bool b)
	{
		if (this.clothSettings != null)
		{
			this.clothSettings.IntegrateEnabled = b;
		}
	}

	// Token: 0x060068D2 RID: 26834 RVA: 0x0027359C File Offset: 0x0027199C
	public void Reset()
	{
		if (this.clothSettings != null)
		{
			this.clothSettings.Reset();
		}
	}

	// Token: 0x060068D3 RID: 26835 RVA: 0x002735BC File Offset: 0x002719BC
	protected override void SyncCollisionEnabled()
	{
		bool flag = true;
		if (this.collisionEnabledJSON != null)
		{
			flag = this.collisionEnabledJSON.val;
		}
		if (this.clothSettings != null)
		{
			this.clothSettings.CollisionEnabled = (this._collisionEnabled && flag);
		}
	}

	// Token: 0x060068D4 RID: 26836 RVA: 0x0027360D File Offset: 0x00271A0D
	protected void SyncCollisionEnabled(bool b)
	{
		this.SyncCollisionEnabled();
	}

	// Token: 0x060068D5 RID: 26837 RVA: 0x00273615 File Offset: 0x00271A15
	public void SyncCollisionRadius(float f)
	{
		if (this.clothSettings != null)
		{
			this.clothSettings.ParticleRadius = f;
			this.Reset();
		}
	}

	// Token: 0x060068D6 RID: 26838 RVA: 0x0027363A File Offset: 0x00271A3A
	public void SyncDrag(float f)
	{
		if (this.clothSettings != null)
		{
			this.clothSettings.Drag = f;
		}
	}

	// Token: 0x060068D7 RID: 26839 RVA: 0x00273659 File Offset: 0x00271A59
	public void SyncWeight(float f)
	{
		if (this.clothSettings != null)
		{
			this.clothSettings.Weight = f;
		}
	}

	// Token: 0x060068D8 RID: 26840 RVA: 0x00273678 File Offset: 0x00271A78
	public void SyncDistanceScale(float f)
	{
		if (this.clothSettings != null)
		{
			this.clothSettings.DistanceScale = f;
		}
	}

	// Token: 0x060068D9 RID: 26841 RVA: 0x00273697 File Offset: 0x00271A97
	public void SyncStiffness(float f)
	{
		if (this.clothSettings != null)
		{
			this.clothSettings.Stiffness = f;
		}
	}

	// Token: 0x060068DA RID: 26842 RVA: 0x002736B6 File Offset: 0x00271AB6
	public void SyncCompressionResistance(float f)
	{
		if (this.clothSettings != null)
		{
			this.clothSettings.CompressionResistance = f;
		}
	}

	// Token: 0x060068DB RID: 26843 RVA: 0x002736D5 File Offset: 0x00271AD5
	public void SyncFriction(float f)
	{
		if (this.clothSettings != null)
		{
			this.clothSettings.Friction = f;
		}
	}

	// Token: 0x060068DC RID: 26844 RVA: 0x002736F4 File Offset: 0x00271AF4
	public void SyncStaticMultiplier(float f)
	{
		if (this.clothSettings != null)
		{
			this.clothSettings.StaticMultiplier = f;
		}
	}

	// Token: 0x060068DD RID: 26845 RVA: 0x00273713 File Offset: 0x00271B13
	public void SyncCollisionPower(float f)
	{
		if (this.clothSettings != null)
		{
			this.clothSettings.CollisionPower = f;
		}
	}

	// Token: 0x060068DE RID: 26846 RVA: 0x00273732 File Offset: 0x00271B32
	public void SyncGravityMultiplier(float f)
	{
		if (this.clothSettings != null)
		{
			this.clothSettings.GravityMultiplier = f;
		}
	}

	// Token: 0x060068DF RID: 26847 RVA: 0x00273751 File Offset: 0x00271B51
	public void SyncIterations(float f)
	{
		if (this.clothSettings != null)
		{
			this.clothSettings.Iterations = Mathf.FloorToInt(f);
		}
	}

	// Token: 0x060068E0 RID: 26848 RVA: 0x00273775 File Offset: 0x00271B75
	public void SyncAllowDetatch(bool b)
	{
		if (this.clothSettings != null)
		{
			this.clothSettings.BreakEnabled = b;
		}
	}

	// Token: 0x060068E1 RID: 26849 RVA: 0x00273794 File Offset: 0x00271B94
	public void AllowDetach()
	{
		if (this.allowDetachJSON != null)
		{
			this.allowDetachJSON.val = true;
		}
	}

	// Token: 0x060068E2 RID: 26850 RVA: 0x002737AD File Offset: 0x00271BAD
	public void SyncDetachThreshold(float f)
	{
		if (this.clothSettings != null)
		{
			this.clothSettings.BreakThreshold = f;
		}
	}

	// Token: 0x060068E3 RID: 26851 RVA: 0x002737CC File Offset: 0x00271BCC
	public void SyncJointStrength(float f)
	{
		if (this.clothSettings != null)
		{
			this.clothSettings.JointStrength = f;
		}
	}

	// Token: 0x060068E4 RID: 26852 RVA: 0x002737EB File Offset: 0x00271BEB
	protected void SyncForce()
	{
		if (this.forceJSON != null)
		{
			this.SyncForce(this.forceJSON.val);
		}
	}

	// Token: 0x060068E5 RID: 26853 RVA: 0x00273809 File Offset: 0x00271C09
	protected void SyncForce(Vector3 v)
	{
		if (this.clothSettings != null && this.clothSettings.Runtime != null)
		{
			this.clothSettings.Runtime.Wind = v + WindControl.globalWind;
		}
	}

	// Token: 0x060068E6 RID: 26854 RVA: 0x00273848 File Offset: 0x00271C48
	protected void Init()
	{
		if (this.clothSettings == null)
		{
			this.clothSettings = base.GetComponent<ClothSettings>();
		}
		if (this.clothSettings != null)
		{
			if (this.clothSettings.MeshProvider.Type == ScalpMeshType.PreCalc)
			{
				this.skinWrap = (this.clothSettings.MeshProvider.PreCalcProvider as DAZSkinWrap);
			}
			this.resetAction = new JSONStorableAction("Reset", new JSONStorableAction.ActionCallback(this.Reset));
			base.RegisterAction(this.resetAction);
			this.simEnabledJSON = new JSONStorableBool("simEnabled", this.clothSettings.enabled, new JSONStorableBool.SetBoolCallback(this.SyncSimEnabled));
			base.RegisterBool(this.simEnabledJSON);
			this.integrateEnabledJSON = new JSONStorableBool("integrateEnabled", this.clothSettings.IntegrateEnabled, new JSONStorableBool.SetBoolCallback(this.SyncIntegrateEnabled));
			base.RegisterBool(this.integrateEnabledJSON);
			this.collisionEnabledJSON = new JSONStorableBool("collisionEnabled", this.clothSettings.CollisionEnabled, new JSONStorableBool.SetBoolCallback(this.SyncCollisionEnabled));
			base.RegisterBool(this.collisionEnabledJSON);
			this.collisionRadiusJSON = new JSONStorableFloat("collisionRadius", this.clothSettings.ParticleRadius, new JSONStorableFloat.SetFloatCallback(this.SyncCollisionRadius), 0.001f, 0.1f, true, true);
			base.RegisterFloat(this.collisionRadiusJSON);
			this.dragJSON = new JSONStorableFloat("drag", this.clothSettings.Drag, new JSONStorableFloat.SetFloatCallback(this.SyncDrag), 0f, 1f, true, true);
			base.RegisterFloat(this.dragJSON);
			this.weightJSON = new JSONStorableFloat("weight", this.clothSettings.Weight, new JSONStorableFloat.SetFloatCallback(this.SyncWeight), 0f, 2f, true, true);
			base.RegisterFloat(this.weightJSON);
			this.distanceScaleJSON = new JSONStorableFloat("distanceScale", this.clothSettings.DistanceScale, new JSONStorableFloat.SetFloatCallback(this.SyncDistanceScale), 0.1f, 2f, true, true);
			base.RegisterFloat(this.distanceScaleJSON);
			this.stiffnessJSON = new JSONStorableFloat("stiffness", this.clothSettings.Stiffness, new JSONStorableFloat.SetFloatCallback(this.SyncStiffness), 0f, 1f, true, true);
			base.RegisterFloat(this.stiffnessJSON);
			this.compressionResistanceJSON = new JSONStorableFloat("compressionResistance", this.clothSettings.CompressionResistance, new JSONStorableFloat.SetFloatCallback(this.SyncCompressionResistance), 0f, 1f, true, true);
			base.RegisterFloat(this.compressionResistanceJSON);
			this.frictionJSON = new JSONStorableFloat("friction", this.clothSettings.Friction, new JSONStorableFloat.SetFloatCallback(this.SyncFriction), 0f, 1f, true, true);
			base.RegisterFloat(this.frictionJSON);
			this.staticMultiplierJSON = new JSONStorableFloat("staticMultiplier", this.clothSettings.StaticMultiplier, new JSONStorableFloat.SetFloatCallback(this.SyncStaticMultiplier), 0f, 10f, true, true);
			base.RegisterFloat(this.staticMultiplierJSON);
			this.collisionPowerJSON = new JSONStorableFloat("collisionPower", this.clothSettings.CollisionPower, new JSONStorableFloat.SetFloatCallback(this.SyncCollisionPower), 0f, 1f, true, true);
			base.RegisterFloat(this.collisionPowerJSON);
			this.gravityMultiplierJSON = new JSONStorableFloat("gravityMultiplier", this.clothSettings.GravityMultiplier, new JSONStorableFloat.SetFloatCallback(this.SyncGravityMultiplier), -4f, 4f, true, true);
			base.RegisterFloat(this.gravityMultiplierJSON);
			this.iterationsJSON = new JSONStorableFloat("iterations", (float)this.clothSettings.Iterations, new JSONStorableFloat.SetFloatCallback(this.SyncIterations), 1f, 7f, true, true);
			base.RegisterFloat(this.iterationsJSON);
			this.allowDetachJSON = new JSONStorableBool("allowDetach", this.clothSettings.BreakEnabled, new JSONStorableBool.SetBoolCallback(this.SyncAllowDetatch));
			base.RegisterBool(this.allowDetachJSON);
			this.detachThresholdJSON = new JSONStorableFloat("detachThreshold", this.clothSettings.BreakThreshold, new JSONStorableFloat.SetFloatCallback(this.SyncDetachThreshold), 0f, 0.05f, false, true);
			base.RegisterFloat(this.detachThresholdJSON);
			this.jointStrengthJSON = new JSONStorableFloat("jointStrength", this.clothSettings.JointStrength, new JSONStorableFloat.SetFloatCallback(this.SyncJointStrength), 0f, 1f, true, true);
			base.RegisterFloat(this.jointStrengthJSON);
			this.forceJSON = new JSONStorableVector3("force", Vector3.zero, new Vector3(-50f, -50f, -50f), new Vector3(50f, 50f, 50f), false, true);
			base.RegisterVector3(this.forceJSON);
		}
	}

	// Token: 0x060068E7 RID: 26855 RVA: 0x00273D28 File Offset: 0x00272128
	protected override void InitUI(Transform t, bool isAlt)
	{
		if (t != null)
		{
			ClothSimControlUI componentInChildren = t.GetComponentInChildren<ClothSimControlUI>(true);
			if (componentInChildren != null)
			{
				if (isAlt)
				{
					this.resetButtonAlt = componentInChildren.resetButton;
					if (this.resetButtonAlt != null)
					{
						this.resetButtonAlt.onClick.AddListener(new UnityAction(this.Reset));
					}
				}
				else
				{
					this.resetButton = componentInChildren.resetButton;
					if (this.resetButton != null)
					{
						this.resetButton.onClick.AddListener(new UnityAction(this.Reset));
					}
				}
				this.simEnabledJSON.RegisterToggle(componentInChildren.simEnabledToggle, isAlt);
				this.integrateEnabledJSON.RegisterToggle(componentInChildren.integrateEnabledToggle, isAlt);
				this.collisionEnabledJSON.RegisterToggle(componentInChildren.collisionEnabledToggle, isAlt);
				this.collisionRadiusJSON.RegisterSlider(componentInChildren.collisionRadiusSlider, isAlt);
				this.dragJSON.RegisterSlider(componentInChildren.dragSlider, isAlt);
				this.weightJSON.RegisterSlider(componentInChildren.weightSlider, isAlt);
				this.distanceScaleJSON.RegisterSlider(componentInChildren.distanceScaleSlider, isAlt);
				this.stiffnessJSON.RegisterSlider(componentInChildren.stiffnessSlider, isAlt);
				this.compressionResistanceJSON.RegisterSlider(componentInChildren.compressionResistanceSlider, isAlt);
				this.frictionJSON.RegisterSlider(componentInChildren.frictionSlider, isAlt);
				this.staticMultiplierJSON.RegisterSlider(componentInChildren.staticMultiplierSlider, isAlt);
				this.collisionPowerJSON.RegisterSlider(componentInChildren.collisionPowerSlider, isAlt);
				this.gravityMultiplierJSON.RegisterSlider(componentInChildren.gravityMultiplierSlider, isAlt);
				this.iterationsJSON.RegisterSlider(componentInChildren.iterationsSlider, isAlt);
				this.allowDetachJSON.RegisterToggle(componentInChildren.allowDetachToggle, isAlt);
				this.detachThresholdJSON.RegisterSlider(componentInChildren.detachThresholdSlider, isAlt);
				this.jointStrengthJSON.RegisterSlider(componentInChildren.jointStrengthSlider, isAlt);
				this.forceJSON.RegisterSliderX(componentInChildren.forceXSlider, isAlt);
				this.forceJSON.RegisterSliderY(componentInChildren.forceYSlider, isAlt);
				this.forceJSON.RegisterSliderZ(componentInChildren.forceZSlider, isAlt);
			}
		}
	}

	// Token: 0x060068E8 RID: 26856 RVA: 0x00273F3C File Offset: 0x0027233C
	protected void DeregisterUI()
	{
		this.collisionRadiusJSON.slider = null;
		this.dragJSON.slider = null;
		this.weightJSON.slider = null;
		this.distanceScaleJSON.slider = null;
		this.stiffnessJSON.slider = null;
		this.compressionResistanceJSON.slider = null;
		this.frictionJSON.slider = null;
		this.staticMultiplierJSON.slider = null;
		this.collisionPowerJSON.slider = null;
		this.gravityMultiplierJSON.slider = null;
		this.iterationsJSON.slider = null;
		this.allowDetachJSON.toggle = null;
		this.detachThresholdJSON.slider = null;
		this.jointStrengthJSON.slider = null;
		if (this.resetButton != null)
		{
			this.resetButton.onClick.RemoveListener(new UnityAction(this.Reset));
		}
		this.forceJSON.sliderX = null;
		this.forceJSON.sliderY = null;
		this.forceJSON.sliderZ = null;
		this.collisionRadiusJSON.sliderAlt = null;
		this.dragJSON.sliderAlt = null;
		this.weightJSON.sliderAlt = null;
		this.distanceScaleJSON.sliderAlt = null;
		this.stiffnessJSON.sliderAlt = null;
		this.compressionResistanceJSON.sliderAlt = null;
		this.frictionJSON.sliderAlt = null;
		this.staticMultiplierJSON.sliderAlt = null;
		this.collisionPowerJSON.sliderAlt = null;
		this.gravityMultiplierJSON.sliderAlt = null;
		this.iterationsJSON.sliderAlt = null;
		this.allowDetachJSON.toggleAlt = null;
		this.detachThresholdJSON.sliderAlt = null;
		this.jointStrengthJSON.sliderAlt = null;
		if (this.resetButtonAlt != null)
		{
			this.resetButtonAlt.onClick.RemoveListener(new UnityAction(this.Reset));
		}
		this.forceJSON.sliderXAlt = null;
		this.forceJSON.sliderYAlt = null;
		this.forceJSON.sliderZAlt = null;
	}

	// Token: 0x060068E9 RID: 26857 RVA: 0x0027413B File Offset: 0x0027253B
	public override void SetUI(Transform t)
	{
		if (this.UITransform != t)
		{
			this.UITransform = t;
			if (base.isActiveAndEnabled)
			{
				this.InitUI();
			}
		}
	}

	// Token: 0x060068EA RID: 26858 RVA: 0x00274166 File Offset: 0x00272566
	public override void SetUIAlt(Transform t)
	{
		if (this.UITransformAlt != t)
		{
			this.UITransformAlt = t;
			if (base.isActiveAndEnabled)
			{
				this.InitUIAlt();
			}
		}
	}

	// Token: 0x060068EB RID: 26859 RVA: 0x00274191 File Offset: 0x00272591
	protected override void Awake()
	{
		if (!this.awakecalled)
		{
			base.Awake();
			this.Init();
		}
	}

	// Token: 0x060068EC RID: 26860 RVA: 0x002741AC File Offset: 0x002725AC
	protected override void Update()
	{
		base.Update();
		if (this._resetSimulation && this.clothSettings != null && this.clothSettings.builder != null && this.clothSettings.builder.physics != null)
		{
			this.clothSettings.Reset();
		}
		this.SyncForce();
	}

	// Token: 0x060068ED RID: 26861 RVA: 0x00274217 File Offset: 0x00272617
	private void OnEnable()
	{
		this.InitUI();
		this.InitUIAlt();
	}

	// Token: 0x060068EE RID: 26862 RVA: 0x00274225 File Offset: 0x00272625
	private void OnDisable()
	{
		this.DeregisterUI();
	}

	// Token: 0x04005993 RID: 22931
	public ClothSettings clothSettings;

	// Token: 0x04005994 RID: 22932
	protected DAZSkinWrap skinWrap;

	// Token: 0x04005995 RID: 22933
	public JSONStorableBool simEnabledJSON;

	// Token: 0x04005996 RID: 22934
	public JSONStorableBool integrateEnabledJSON;

	// Token: 0x04005997 RID: 22935
	protected Button resetButton;

	// Token: 0x04005998 RID: 22936
	protected Button resetButtonAlt;

	// Token: 0x04005999 RID: 22937
	protected JSONStorableAction resetAction;

	// Token: 0x0400599A RID: 22938
	public JSONStorableBool collisionEnabledJSON;

	// Token: 0x0400599B RID: 22939
	protected JSONStorableFloat collisionRadiusJSON;

	// Token: 0x0400599C RID: 22940
	protected JSONStorableFloat dragJSON;

	// Token: 0x0400599D RID: 22941
	protected JSONStorableFloat weightJSON;

	// Token: 0x0400599E RID: 22942
	protected JSONStorableFloat distanceScaleJSON;

	// Token: 0x0400599F RID: 22943
	protected JSONStorableFloat stiffnessJSON;

	// Token: 0x040059A0 RID: 22944
	protected JSONStorableFloat compressionResistanceJSON;

	// Token: 0x040059A1 RID: 22945
	protected JSONStorableFloat frictionJSON;

	// Token: 0x040059A2 RID: 22946
	protected JSONStorableFloat staticMultiplierJSON;

	// Token: 0x040059A3 RID: 22947
	protected JSONStorableFloat collisionPowerJSON;

	// Token: 0x040059A4 RID: 22948
	protected JSONStorableFloat gravityMultiplierJSON;

	// Token: 0x040059A5 RID: 22949
	protected JSONStorableFloat iterationsJSON;

	// Token: 0x040059A6 RID: 22950
	protected JSONStorableBool allowDetachJSON;

	// Token: 0x040059A7 RID: 22951
	protected JSONStorableFloat detachThresholdJSON;

	// Token: 0x040059A8 RID: 22952
	protected JSONStorableFloat jointStrengthJSON;

	// Token: 0x040059A9 RID: 22953
	protected JSONStorableVector3 forceJSON;
}
