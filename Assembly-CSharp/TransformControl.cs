using System;
using UnityEngine;

// Token: 0x02000BC9 RID: 3017
public class TransformControl : JSONStorable
{
	// Token: 0x060055C6 RID: 21958 RVA: 0x001F5DD8 File Offset: 0x001F41D8
	public TransformControl()
	{
	}

	// Token: 0x060055C7 RID: 21959 RVA: 0x001F5DE0 File Offset: 0x001F41E0
	protected void SyncXPosition(float f)
	{
		Vector3 localPosition = base.transform.localPosition;
		localPosition.x = f;
		base.transform.localPosition = localPosition;
	}

	// Token: 0x060055C8 RID: 21960 RVA: 0x001F5E10 File Offset: 0x001F4210
	protected void SyncYPosition(float f)
	{
		Vector3 localPosition = base.transform.localPosition;
		localPosition.y = f;
		base.transform.localPosition = localPosition;
	}

	// Token: 0x060055C9 RID: 21961 RVA: 0x001F5E40 File Offset: 0x001F4240
	protected void SyncZPosition(float f)
	{
		Vector3 localPosition = base.transform.localPosition;
		localPosition.z = f;
		base.transform.localPosition = localPosition;
	}

	// Token: 0x060055CA RID: 21962 RVA: 0x001F5E70 File Offset: 0x001F4270
	protected void SyncXRotation(float f)
	{
		Vector3 localEulerAngles = base.transform.localEulerAngles;
		localEulerAngles.x = f;
		base.transform.localEulerAngles = localEulerAngles;
	}

	// Token: 0x060055CB RID: 21963 RVA: 0x001F5EA0 File Offset: 0x001F42A0
	protected void SyncYRotation(float f)
	{
		Vector3 localEulerAngles = base.transform.localEulerAngles;
		localEulerAngles.y = f;
		base.transform.localEulerAngles = localEulerAngles;
	}

	// Token: 0x060055CC RID: 21964 RVA: 0x001F5ED0 File Offset: 0x001F42D0
	protected void SyncZRotation(float f)
	{
		Vector3 localEulerAngles = base.transform.localEulerAngles;
		localEulerAngles.z = f;
		base.transform.localEulerAngles = localEulerAngles;
	}

	// Token: 0x060055CD RID: 21965 RVA: 0x001F5F00 File Offset: 0x001F4300
	protected void SyncScale(float f)
	{
		Vector3 localScale;
		localScale.x = this.scaleJSON.val * this.xScaleJSON.val;
		localScale.y = this.scaleJSON.val * this.yScaleJSON.val;
		localScale.z = this.scaleJSON.val * this.zScaleJSON.val;
		base.transform.localScale = localScale;
		if (this.scaleChangeReceiverContainer != null)
		{
			ScaleChangeReceiver[] componentsInChildren = this.scaleChangeReceiverContainer.GetComponentsInChildren<ScaleChangeReceiver>();
			foreach (ScaleChangeReceiver scaleChangeReceiver in componentsInChildren)
			{
				scaleChangeReceiver.ScaleChanged(scaleChangeReceiver.scale);
			}
			ScaleChangeReceiverJSONStorable[] componentsInChildren2 = this.scaleChangeReceiverContainer.GetComponentsInChildren<ScaleChangeReceiverJSONStorable>();
			foreach (ScaleChangeReceiverJSONStorable scaleChangeReceiverJSONStorable in componentsInChildren2)
			{
				scaleChangeReceiverJSONStorable.ScaleChanged(scaleChangeReceiverJSONStorable.scale);
			}
		}
	}

	// Token: 0x060055CE RID: 21966 RVA: 0x001F5FFC File Offset: 0x001F43FC
	protected void Init()
	{
		this.xPositionJSON = new JSONStorableFloat("xPosition", 0f, new JSONStorableFloat.SetFloatCallback(this.SyncXPosition), -0.1f, 0.1f, false, true);
		base.RegisterFloat(this.xPositionJSON);
		this.yPositionJSON = new JSONStorableFloat("yPosition", 0f, new JSONStorableFloat.SetFloatCallback(this.SyncYPosition), -0.1f, 0.1f, false, true);
		base.RegisterFloat(this.yPositionJSON);
		this.zPositionJSON = new JSONStorableFloat("zPosition", 0f, new JSONStorableFloat.SetFloatCallback(this.SyncZPosition), -0.1f, 0.1f, false, true);
		base.RegisterFloat(this.zPositionJSON);
		this.xRotationJSON = new JSONStorableFloat("xRotation", 0f, new JSONStorableFloat.SetFloatCallback(this.SyncXRotation), -180f, 180f, true, true);
		base.RegisterFloat(this.xRotationJSON);
		this.yRotationJSON = new JSONStorableFloat("yRotation", 0f, new JSONStorableFloat.SetFloatCallback(this.SyncYRotation), -180f, 180f, true, true);
		base.RegisterFloat(this.yRotationJSON);
		this.zRotationJSON = new JSONStorableFloat("zRotation", 0f, new JSONStorableFloat.SetFloatCallback(this.SyncZRotation), -180f, 180f, true, true);
		base.RegisterFloat(this.zRotationJSON);
		this.scaleJSON = new JSONStorableFloat("scale", 1f, new JSONStorableFloat.SetFloatCallback(this.SyncScale), 0f, 2f, false, true);
		base.RegisterFloat(this.scaleJSON);
		this.xScaleJSON = new JSONStorableFloat("xScale", 1f, new JSONStorableFloat.SetFloatCallback(this.SyncScale), 0f, 2f, false, true);
		base.RegisterFloat(this.xScaleJSON);
		this.yScaleJSON = new JSONStorableFloat("yScale", 1f, new JSONStorableFloat.SetFloatCallback(this.SyncScale), 0f, 2f, false, true);
		base.RegisterFloat(this.yScaleJSON);
		this.zScaleJSON = new JSONStorableFloat("zScale", 1f, new JSONStorableFloat.SetFloatCallback(this.SyncScale), 0f, 2f, false, true);
		base.RegisterFloat(this.zScaleJSON);
	}

	// Token: 0x060055CF RID: 21967 RVA: 0x001F6244 File Offset: 0x001F4644
	public override void InitUI()
	{
		if (this.UITransform != null)
		{
			TransformControlUI componentInChildren = this.UITransform.GetComponentInChildren<TransformControlUI>(true);
			if (componentInChildren != null)
			{
				this.xPositionJSON.slider = componentInChildren.xPositionSlider;
				this.yPositionJSON.slider = componentInChildren.yPositionSlider;
				this.zPositionJSON.slider = componentInChildren.zPositionSlider;
				this.xRotationJSON.slider = componentInChildren.xRotationSlider;
				this.yRotationJSON.slider = componentInChildren.yRotationSlider;
				this.zRotationJSON.slider = componentInChildren.zRotationSlider;
				this.scaleJSON.slider = componentInChildren.scaleSlider;
				this.xScaleJSON.slider = componentInChildren.xScaleSlider;
				this.yScaleJSON.slider = componentInChildren.yScaleSlider;
				this.zScaleJSON.slider = componentInChildren.zScaleSlider;
			}
		}
	}

	// Token: 0x060055D0 RID: 21968 RVA: 0x001F6328 File Offset: 0x001F4728
	public override void InitUIAlt()
	{
		if (this.UITransformAlt != null)
		{
			TransformControlUI componentInChildren = this.UITransformAlt.GetComponentInChildren<TransformControlUI>(true);
			if (componentInChildren != null)
			{
				this.xPositionJSON.sliderAlt = componentInChildren.xPositionSlider;
				this.yPositionJSON.sliderAlt = componentInChildren.yPositionSlider;
				this.zPositionJSON.sliderAlt = componentInChildren.zPositionSlider;
				this.xRotationJSON.sliderAlt = componentInChildren.xRotationSlider;
				this.yRotationJSON.sliderAlt = componentInChildren.yRotationSlider;
				this.zRotationJSON.sliderAlt = componentInChildren.zRotationSlider;
				this.scaleJSON.sliderAlt = componentInChildren.scaleSlider;
				this.xScaleJSON.sliderAlt = componentInChildren.xScaleSlider;
				this.yScaleJSON.sliderAlt = componentInChildren.yScaleSlider;
				this.zScaleJSON.sliderAlt = componentInChildren.zScaleSlider;
			}
		}
	}

	// Token: 0x060055D1 RID: 21969 RVA: 0x001F6409 File Offset: 0x001F4809
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

	// Token: 0x040046E7 RID: 18151
	protected JSONStorableFloat xPositionJSON;

	// Token: 0x040046E8 RID: 18152
	protected JSONStorableFloat yPositionJSON;

	// Token: 0x040046E9 RID: 18153
	protected JSONStorableFloat zPositionJSON;

	// Token: 0x040046EA RID: 18154
	protected JSONStorableFloat xRotationJSON;

	// Token: 0x040046EB RID: 18155
	protected JSONStorableFloat yRotationJSON;

	// Token: 0x040046EC RID: 18156
	protected JSONStorableFloat zRotationJSON;

	// Token: 0x040046ED RID: 18157
	protected JSONStorableFloat scaleJSON;

	// Token: 0x040046EE RID: 18158
	protected JSONStorableFloat xScaleJSON;

	// Token: 0x040046EF RID: 18159
	protected JSONStorableFloat yScaleJSON;

	// Token: 0x040046F0 RID: 18160
	protected JSONStorableFloat zScaleJSON;

	// Token: 0x040046F1 RID: 18161
	public Transform scaleChangeReceiverContainer;
}
