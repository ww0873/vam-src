using System;

// Token: 0x02000D46 RID: 3398
public class AdjustJointSpringsControl : JSONStorable
{
	// Token: 0x06006857 RID: 26711 RVA: 0x00271E9E File Offset: 0x0027029E
	public AdjustJointSpringsControl()
	{
	}

	// Token: 0x06006858 RID: 26712 RVA: 0x00271EA6 File Offset: 0x002702A6
	public void SyncSpringStrength(float f)
	{
		if (this.jointSprings != null)
		{
			this.jointSprings.percent = f;
		}
	}

	// Token: 0x06006859 RID: 26713 RVA: 0x00271EC8 File Offset: 0x002702C8
	public override void InitUI()
	{
		if (this.UITransform != null)
		{
			AdjustJointSpringsControlUI componentInChildren = this.UITransform.GetComponentInChildren<AdjustJointSpringsControlUI>(true);
			if (componentInChildren != null)
			{
				this.springStrengthJSON.slider = componentInChildren.springStrengthSlider;
			}
		}
	}

	// Token: 0x0600685A RID: 26714 RVA: 0x00271F10 File Offset: 0x00270310
	public override void InitUIAlt()
	{
		if (this.UITransformAlt != null)
		{
			AdjustJointSpringsControlUI componentInChildren = this.UITransformAlt.GetComponentInChildren<AdjustJointSpringsControlUI>(true);
			if (componentInChildren != null)
			{
				this.springStrengthJSON.sliderAlt = componentInChildren.springStrengthSlider;
			}
		}
	}

	// Token: 0x0600685B RID: 26715 RVA: 0x00271F58 File Offset: 0x00270358
	protected void Init()
	{
		this.springStrengthJSON = new JSONStorableFloat("springStrength", 0.1f, new JSONStorableFloat.SetFloatCallback(this.SyncSpringStrength), 0f, 1f, true, true);
		base.RegisterFloat(this.springStrengthJSON);
	}

	// Token: 0x0600685C RID: 26716 RVA: 0x00271F93 File Offset: 0x00270393
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

	// Token: 0x04005936 RID: 22838
	public AdjustJointSprings jointSprings;

	// Token: 0x04005937 RID: 22839
	public JSONStorableFloat springStrengthJSON;
}
