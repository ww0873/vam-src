using System;
using UnityEngine;

// Token: 0x02000D58 RID: 3416
public class ConfigurableJointScaler : ScaleChangeReceiver
{
	// Token: 0x06006908 RID: 26888 RVA: 0x00274855 File Offset: 0x00272C55
	public ConfigurableJointScaler()
	{
	}

	// Token: 0x06006909 RID: 26889 RVA: 0x00274868 File Offset: 0x00272C68
	public override void ScaleChanged(float scale)
	{
		base.ScaleChanged(scale);
		this.scalePow = Mathf.Pow(1.7f, scale - 1f);
		this.InitJoint();
		this.SyncJoint();
	}

	// Token: 0x0600690A RID: 26890 RVA: 0x00274894 File Offset: 0x00272C94
	protected void SyncJoint()
	{
		if (this.joint != null)
		{
			if (this.joint.rotationDriveMode == RotationDriveMode.Slerp)
			{
				JointDrive slerpDrive = this.joint.slerpDrive;
				slerpDrive.positionSpring = this.startingDriveSpring * this.scalePow;
				slerpDrive.positionDamper = this.startingDriveDamper * this.scalePow;
				slerpDrive.maximumForce = this.startingMaxForce * this.scalePow;
				this.joint.slerpDrive = slerpDrive;
			}
			else
			{
				JointDrive angularXDrive = this.joint.angularXDrive;
				angularXDrive.positionSpring = this.startingDriveSpring * this.scalePow;
				angularXDrive.positionDamper = this.startingDriveDamper * this.scalePow;
				angularXDrive.maximumForce = this.startingMaxForce * this.scalePow;
				this.joint.angularXDrive = angularXDrive;
				JointDrive angularYZDrive = this.joint.angularYZDrive;
				angularYZDrive.positionSpring = this.startingDriveSpring * this.scalePow;
				angularYZDrive.positionDamper = this.startingDriveDamper * this.scalePow;
				angularYZDrive.maximumForce = this.startingMaxForce * this.scalePow;
				this.joint.angularYZDrive = angularYZDrive;
			}
		}
	}

	// Token: 0x0600690B RID: 26891 RVA: 0x002749C4 File Offset: 0x00272DC4
	protected void InitJoint()
	{
		if (!this._wasInit)
		{
			this._wasInit = true;
			this.joint = base.GetComponent<ConfigurableJoint>();
			if (this.joint != null)
			{
				if (this.joint.rotationDriveMode == RotationDriveMode.Slerp)
				{
					JointDrive slerpDrive = this.joint.slerpDrive;
					this.startingDriveSpring = slerpDrive.positionSpring / this.scalePow;
					this.startingDriveDamper = slerpDrive.positionDamper / this.scalePow;
					this.startingMaxForce = slerpDrive.maximumForce / this.scalePow;
				}
				else
				{
					JointDrive angularXDrive = this.joint.angularXDrive;
					this.startingDriveSpring = angularXDrive.positionSpring / this.scalePow;
					this.startingDriveDamper = angularXDrive.positionDamper / this.scalePow;
					this.startingMaxForce = angularXDrive.maximumForce / this.scalePow;
					JointDrive angularYZDrive = this.joint.angularYZDrive;
					this.startingDriveSpring = angularYZDrive.positionSpring / this.scalePow;
					this.startingDriveDamper = angularYZDrive.positionDamper / this.scalePow;
					this.startingMaxForce = angularYZDrive.maximumForce / this.scalePow;
				}
			}
		}
	}

	// Token: 0x0600690C RID: 26892 RVA: 0x00274AEE File Offset: 0x00272EEE
	private void Awake()
	{
		this.InitJoint();
	}

	// Token: 0x040059CE RID: 22990
	protected float scalePow = 1f;

	// Token: 0x040059CF RID: 22991
	private ConfigurableJoint joint;

	// Token: 0x040059D0 RID: 22992
	private float startingDriveSpring;

	// Token: 0x040059D1 RID: 22993
	private float startingDriveDamper;

	// Token: 0x040059D2 RID: 22994
	private float startingMaxForce;

	// Token: 0x040059D3 RID: 22995
	private float startingDriveSpringAlt;

	// Token: 0x040059D4 RID: 22996
	private float startingDriveDamperAlt;

	// Token: 0x040059D5 RID: 22997
	private float startingMaxForceAlt;

	// Token: 0x040059D6 RID: 22998
	protected bool _wasInit;
}
