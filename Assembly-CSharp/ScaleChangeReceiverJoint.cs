using System;
using UnityEngine;

// Token: 0x02000D8C RID: 3468
public class ScaleChangeReceiverJoint : ScaleChangeReceiver
{
	// Token: 0x06006AFD RID: 27389 RVA: 0x00284509 File Offset: 0x00282909
	public ScaleChangeReceiverJoint()
	{
	}

	// Token: 0x06006AFE RID: 27390 RVA: 0x0028451C File Offset: 0x0028291C
	public override void ScaleChanged(float scale)
	{
		base.ScaleChanged(scale);
		this.scalePow = Mathf.Pow(1.7f, this._scale - 1f);
		this.Adjust();
	}

	// Token: 0x06006AFF RID: 27391 RVA: 0x00284548 File Offset: 0x00282948
	protected void Adjust()
	{
		if (this.CJ == null)
		{
			this.CJ = base.GetComponent<ConfigurableJoint>();
		}
		if (this.CJ != null)
		{
			float num = this.scalePow;
			float num2 = this.scalePow;
			float num3 = num;
			JointDrive jointDrive = this.CJ.slerpDrive;
			if (!this._defaultsSet)
			{
				this._defaultSpring = jointDrive.positionSpring;
				this._defaultDamper = jointDrive.positionDamper;
				this._defaultMaxForce = jointDrive.maximumForce;
			}
			jointDrive.positionSpring = this._defaultSpring * num;
			jointDrive.positionDamper = this._defaultDamper * num2;
			jointDrive.maximumForce = this._defaultMaxForce * num3;
			this.CJ.slerpDrive = jointDrive;
			jointDrive = this.CJ.angularXDrive;
			if (!this._defaultsSet)
			{
				this._defaultXSpring = jointDrive.positionSpring;
				this._defaultXDamper = jointDrive.positionDamper;
				this._defaultXMaxForce = jointDrive.maximumForce;
			}
			jointDrive.positionSpring = this._defaultXSpring * num;
			jointDrive.positionDamper = this._defaultXDamper * num2;
			jointDrive.maximumForce = this._defaultXMaxForce * num3;
			this.CJ.angularXDrive = jointDrive;
			jointDrive = this.CJ.angularYZDrive;
			if (!this._defaultsSet)
			{
				this._defaultYZSpring = jointDrive.positionSpring;
				this._defaultYZDamper = jointDrive.positionDamper;
				this._defaultYZMaxForce = jointDrive.maximumForce;
			}
			jointDrive.positionSpring = this._defaultYZSpring * num;
			jointDrive.positionDamper = this._defaultYZDamper * num2;
			jointDrive.maximumForce = this._defaultYZMaxForce * num3;
			this.CJ.angularYZDrive = jointDrive;
			SoftJointLimit linearLimit = this.CJ.linearLimit;
			if (!this._defaultsSet)
			{
				this._defaultLinearLimit = linearLimit.limit;
			}
			linearLimit.limit = this._defaultLinearLimit * this._scale;
			this.CJ.linearLimit = linearLimit;
			Vector3 targetPosition = this.CJ.targetPosition;
			if (!this._defaultsSet)
			{
				this._defaultTargetPosition = targetPosition;
			}
			this.CJ.targetPosition = this._defaultTargetPosition * this._scale;
			this._defaultsSet = true;
			Rigidbody component = base.GetComponent<Rigidbody>();
			component.WakeUp();
		}
	}

	// Token: 0x04005CD3 RID: 23763
	protected ConfigurableJoint CJ;

	// Token: 0x04005CD4 RID: 23764
	protected float scalePow = 1f;

	// Token: 0x04005CD5 RID: 23765
	protected bool _defaultsSet;

	// Token: 0x04005CD6 RID: 23766
	protected float _defaultSpring;

	// Token: 0x04005CD7 RID: 23767
	protected float _defaultDamper;

	// Token: 0x04005CD8 RID: 23768
	protected float _defaultMaxForce;

	// Token: 0x04005CD9 RID: 23769
	protected float _defaultXSpring;

	// Token: 0x04005CDA RID: 23770
	protected float _defaultXDamper;

	// Token: 0x04005CDB RID: 23771
	protected float _defaultXMaxForce;

	// Token: 0x04005CDC RID: 23772
	protected float _defaultYZSpring;

	// Token: 0x04005CDD RID: 23773
	protected float _defaultYZDamper;

	// Token: 0x04005CDE RID: 23774
	protected float _defaultYZMaxForce;

	// Token: 0x04005CDF RID: 23775
	protected float _defaultLinearLimit;

	// Token: 0x04005CE0 RID: 23776
	protected Vector3 _defaultTargetPosition;
}
