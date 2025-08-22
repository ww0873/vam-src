using System;
using UnityEngine;

// Token: 0x02000D4E RID: 3406
public class AdjustJointXDrive : JSONStorable
{
	// Token: 0x06006897 RID: 26775 RVA: 0x00272974 File Offset: 0x00270D74
	public AdjustJointXDrive()
	{
	}

	// Token: 0x06006898 RID: 26776 RVA: 0x0027297C File Offset: 0x00270D7C
	protected void SyncDriveAngle(float f)
	{
		if (this.joint != null)
		{
			Vector3 euler;
			euler.x = f;
			euler.y = 0f;
			euler.z = 0f;
			Quaternion targetRotation = Quaternion.Euler(euler);
			this.joint.targetRotation = targetRotation;
		}
	}

	// Token: 0x06006899 RID: 26777 RVA: 0x002729CD File Offset: 0x00270DCD
	protected void SyncAutoDriveSpeed(float f)
	{
	}

	// Token: 0x0600689A RID: 26778 RVA: 0x002729D0 File Offset: 0x00270DD0
	public override void InitUI()
	{
		if (this.UITransform != null)
		{
			AdjustJointXDriveUI componentInChildren = this.UITransform.GetComponentInChildren<AdjustJointXDriveUI>(true);
			if (componentInChildren != null)
			{
				this.driveAngleJSON.slider = componentInChildren.driveAngleSlider;
				this.autoDriveSpeedJSON.slider = componentInChildren.autoDriveSpeedSlider;
			}
		}
	}

	// Token: 0x0600689B RID: 26779 RVA: 0x00272A2C File Offset: 0x00270E2C
	public override void InitUIAlt()
	{
		if (this.UITransformAlt != null)
		{
			AdjustJointXDriveUI componentInChildren = this.UITransformAlt.GetComponentInChildren<AdjustJointXDriveUI>(true);
			if (componentInChildren != null)
			{
				this.driveAngleJSON.sliderAlt = componentInChildren.driveAngleSlider;
				this.autoDriveSpeedJSON.sliderAlt = componentInChildren.autoDriveSpeedSlider;
			}
		}
	}

	// Token: 0x0600689C RID: 26780 RVA: 0x00272A88 File Offset: 0x00270E88
	protected void Init()
	{
		this.driveAngleJSON = new JSONStorableAngle("driveAngle", 0f, new JSONStorableFloat.SetFloatCallback(this.SyncDriveAngle));
		base.RegisterFloat(this.driveAngleJSON);
		this.autoDriveSpeedJSON = new JSONStorableFloat("autoDriveSpeed", 0f, new JSONStorableFloat.SetFloatCallback(this.SyncAutoDriveSpeed), -10f, 10f, false, true);
		base.RegisterFloat(this.autoDriveSpeedJSON);
	}

	// Token: 0x0600689D RID: 26781 RVA: 0x00272AFB File Offset: 0x00270EFB
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

	// Token: 0x0600689E RID: 26782 RVA: 0x00272B20 File Offset: 0x00270F20
	private void Update()
	{
		if (this.autoDriveSpeedJSON != null && this.autoDriveSpeedJSON.val != 0f)
		{
			float val = this.driveAngleJSON.val + this.autoDriveSpeedJSON.val * Time.deltaTime * 90f;
			this.driveAngleJSON.val = val;
		}
	}

	// Token: 0x0400596C RID: 22892
	public ConfigurableJoint joint;

	// Token: 0x0400596D RID: 22893
	public JSONStorableAngle driveAngleJSON;

	// Token: 0x0400596E RID: 22894
	public JSONStorableFloat autoDriveSpeedJSON;
}
