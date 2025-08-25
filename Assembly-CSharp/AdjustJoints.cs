using System;
using UnityEngine;

// Token: 0x02000D42 RID: 3394
public class AdjustJoints : ScaleChangeReceiverJSONStorable
{
	// Token: 0x060067DD RID: 26589 RVA: 0x0026FBF0 File Offset: 0x0026DFF0
	public AdjustJoints()
	{
	}

	// Token: 0x060067DE RID: 26590 RVA: 0x0026FC58 File Offset: 0x0026E058
	protected void SyncMass(float f)
	{
		this._mass = f;
		if (this.joint1 != null)
		{
			Rigidbody component = this.joint1.GetComponent<Rigidbody>();
			if (component != null && component.mass != this._mass)
			{
				component.mass = this._mass;
				component.WakeUp();
			}
		}
		if (this.joint2 != null)
		{
			Rigidbody component2 = this.joint2.GetComponent<Rigidbody>();
			if (component2 != null && component2.mass != this._mass)
			{
				component2.mass = this._mass;
				component2.WakeUp();
			}
		}
	}

	// Token: 0x17000F36 RID: 3894
	// (get) Token: 0x060067DF RID: 26591 RVA: 0x0026FD04 File Offset: 0x0026E104
	// (set) Token: 0x060067E0 RID: 26592 RVA: 0x0026FD0C File Offset: 0x0026E10C
	public float mass
	{
		get
		{
			return this._mass;
		}
		set
		{
			if (this.massJSON != null)
			{
				this.massJSON.val = value;
			}
			else if (this._mass != value)
			{
				this.SyncMass(value);
			}
		}
	}

	// Token: 0x060067E1 RID: 26593 RVA: 0x0026FD40 File Offset: 0x0026E140
	protected void SyncCenterOfGravity(float f)
	{
		this._centerOfGravityPercent = f;
		if (this.useSetCenterOfGravity)
		{
			this.currentCenterOfGravity = Vector3.Lerp(this.lowCenterOfGravity, this.highCenterOfGravity, this._centerOfGravityPercent);
			if (this.joint1 != null)
			{
				Rigidbody component = this.joint1.GetComponent<Rigidbody>();
				if (component != null && component.centerOfMass != this.currentCenterOfGravity)
				{
					component.centerOfMass = this.currentCenterOfGravity;
					component.WakeUp();
				}
			}
			if (this.joint2 != null)
			{
				Rigidbody component2 = this.joint2.GetComponent<Rigidbody>();
				if (this.useJoint1COGForJoint2)
				{
					this.currentCenterOfGravityJoint2 = this.currentCenterOfGravity;
				}
				else
				{
					this.currentCenterOfGravityJoint2 = Vector3.Lerp(this.lowCenterOfGravityJoint2, this.highCenterOfGravityJoint2, this._centerOfGravityPercent);
				}
				if (component2 != null && component2.centerOfMass != this.currentCenterOfGravityJoint2)
				{
					component2.centerOfMass = this.currentCenterOfGravityJoint2;
					component2.WakeUp();
				}
			}
		}
	}

	// Token: 0x17000F37 RID: 3895
	// (get) Token: 0x060067E2 RID: 26594 RVA: 0x0026FE57 File Offset: 0x0026E257
	// (set) Token: 0x060067E3 RID: 26595 RVA: 0x0026FE5F File Offset: 0x0026E25F
	public float centerOfGravityPercent
	{
		get
		{
			return this._centerOfGravityPercent;
		}
		set
		{
			if (this.centerOfGravityPercentJSON != null)
			{
				this.centerOfGravityPercentJSON.val = value;
			}
			else if (this._centerOfGravityPercent != value)
			{
				this.SyncCenterOfGravity(value);
			}
		}
	}

	// Token: 0x060067E4 RID: 26596 RVA: 0x0026FE90 File Offset: 0x0026E290
	public override void ScaleChanged(float scale)
	{
		base.ScaleChanged(scale);
		this.scalePow = Mathf.Pow(1.7f, this._scale - 1f);
		if (base.gameObject.activeInHierarchy)
		{
			this.SyncJoint();
		}
	}

	// Token: 0x060067E5 RID: 26597 RVA: 0x0026FECC File Offset: 0x0026E2CC
	protected void SyncJoint()
	{
		float num = this.scalePow;
		float num2 = this.scalePow;
		float num3 = num;
		float num4 = this._spring * num;
		float num5 = this._damper * num2;
		if (this.joint1 != null)
		{
			JointDrive jointDrive = this.joint1.slerpDrive;
			if (this._springDamperMultiplierOn)
			{
				jointDrive.positionSpring = num4 * this._springDamperMultiplier;
				jointDrive.positionDamper = num5 * this._springDamperMultiplier;
			}
			else
			{
				jointDrive.positionSpring = num4;
				jointDrive.positionDamper = num5;
			}
			jointDrive.maximumForce = this._defaultJoint1SlerpMaxForce * num3;
			this.joint1.slerpDrive = jointDrive;
			jointDrive = this.joint1.angularXDrive;
			jointDrive.positionSpring = num4;
			jointDrive.positionDamper = num5;
			jointDrive.maximumForce = this._defaultJoint1XMaxForce * num3;
			this.joint1.angularXDrive = jointDrive;
			jointDrive = this.joint1.angularYZDrive;
			jointDrive.positionSpring = num4;
			jointDrive.positionDamper = num5;
			jointDrive.maximumForce = this._defaultJoint1YZMaxForce * num3;
			this.joint1.angularYZDrive = jointDrive;
			SoftJointLimitSpring softJointLimitSpring = this.joint1.angularXLimitSpring;
			softJointLimitSpring.spring = num4 * this._limitSpringMultiplier;
			softJointLimitSpring.damper = num5 * this._limitDamperMultiplier;
			this.joint1.angularXLimitSpring = softJointLimitSpring;
			softJointLimitSpring = this.joint1.angularYZLimitSpring;
			softJointLimitSpring.spring = num4 * this._limitSpringMultiplier;
			softJointLimitSpring.damper = num5 * this._limitDamperMultiplier;
			this.joint1.angularYZLimitSpring = softJointLimitSpring;
			Rigidbody component = this.joint1.GetComponent<Rigidbody>();
			if (component != null)
			{
				component.WakeUp();
			}
		}
		if (this.joint2 != null)
		{
			JointDrive jointDrive2 = this.joint2.slerpDrive;
			if (this._springDamperMultiplierOn)
			{
				jointDrive2.positionSpring = num4 * this._springDamperMultiplier;
				jointDrive2.positionDamper = num5 * this._springDamperMultiplier;
			}
			else
			{
				jointDrive2.positionSpring = num4;
				jointDrive2.positionDamper = num5;
			}
			jointDrive2.maximumForce = this._defaultJoint2SlerpMaxForce * num3;
			this.joint2.slerpDrive = jointDrive2;
			jointDrive2 = this.joint2.angularXDrive;
			jointDrive2.positionSpring = num4;
			jointDrive2.positionDamper = num5;
			jointDrive2.maximumForce = this._defaultJoint2XMaxForce * num3;
			this.joint2.angularXDrive = jointDrive2;
			jointDrive2 = this.joint2.angularYZDrive;
			jointDrive2.positionSpring = num4;
			jointDrive2.positionDamper = num5;
			jointDrive2.maximumForce = this._defaultJoint2YZMaxForce * num3;
			this.joint2.angularYZDrive = jointDrive2;
			SoftJointLimitSpring softJointLimitSpring2 = this.joint2.angularXLimitSpring;
			softJointLimitSpring2.spring = num4 * this._limitSpringMultiplier;
			softJointLimitSpring2.damper = num5 * this._limitDamperMultiplier;
			this.joint2.angularXLimitSpring = softJointLimitSpring2;
			softJointLimitSpring2 = this.joint2.angularYZLimitSpring;
			softJointLimitSpring2.spring = num4 * this._limitSpringMultiplier;
			softJointLimitSpring2.damper = num5 * this._limitDamperMultiplier;
			this.joint2.angularYZLimitSpring = softJointLimitSpring2;
			Rigidbody component2 = this.joint2.GetComponent<Rigidbody>();
			if (component2 != null)
			{
				component2.WakeUp();
			}
		}
	}

	// Token: 0x17000F38 RID: 3896
	// (get) Token: 0x060067E6 RID: 26598 RVA: 0x002701FE File Offset: 0x0026E5FE
	// (set) Token: 0x060067E7 RID: 26599 RVA: 0x00270206 File Offset: 0x0026E606
	public float limitSpringMultiplier
	{
		get
		{
			return this._limitSpringMultiplier;
		}
		set
		{
			if (this._limitSpringMultiplier != value)
			{
				this._limitSpringMultiplier = value;
				this.SyncJoint();
			}
		}
	}

	// Token: 0x17000F39 RID: 3897
	// (get) Token: 0x060067E8 RID: 26600 RVA: 0x00270221 File Offset: 0x0026E621
	// (set) Token: 0x060067E9 RID: 26601 RVA: 0x00270229 File Offset: 0x0026E629
	public float limitDamperMultiplier
	{
		get
		{
			return this._limitDamperMultiplier;
		}
		set
		{
			if (this._limitDamperMultiplier != value)
			{
				this._limitDamperMultiplier = value;
				this.SyncJoint();
			}
		}
	}

	// Token: 0x060067EA RID: 26602 RVA: 0x00270244 File Offset: 0x0026E644
	protected void SyncSpring(float f)
	{
		this._spring = f;
		this.SyncJoint();
	}

	// Token: 0x17000F3A RID: 3898
	// (get) Token: 0x060067EB RID: 26603 RVA: 0x00270253 File Offset: 0x0026E653
	// (set) Token: 0x060067EC RID: 26604 RVA: 0x0027025B File Offset: 0x0026E65B
	public float spring
	{
		get
		{
			return this._spring;
		}
		set
		{
			if (this.springJSON != null)
			{
				this.springJSON.val = value;
			}
			else if (this._spring != value)
			{
				this.SyncSpring(value);
			}
		}
	}

	// Token: 0x060067ED RID: 26605 RVA: 0x0027028C File Offset: 0x0026E68C
	protected void SyncDamper(float f)
	{
		this._damper = f;
		this.SyncJoint();
	}

	// Token: 0x17000F3B RID: 3899
	// (get) Token: 0x060067EE RID: 26606 RVA: 0x0027029B File Offset: 0x0026E69B
	// (set) Token: 0x060067EF RID: 26607 RVA: 0x002702A3 File Offset: 0x0026E6A3
	public float damper
	{
		get
		{
			return this._damper;
		}
		set
		{
			if (this.damperJSON != null)
			{
				this.damperJSON.val = value;
			}
			else if (this._damper != value)
			{
				this.SyncDamper(value);
			}
		}
	}

	// Token: 0x17000F3C RID: 3900
	// (get) Token: 0x060067F0 RID: 26608 RVA: 0x002702D4 File Offset: 0x0026E6D4
	// (set) Token: 0x060067F1 RID: 26609 RVA: 0x002702DC File Offset: 0x0026E6DC
	public float springDamperMultiplier
	{
		get
		{
			return this._springDamperMultiplier;
		}
		set
		{
			if (this._springDamperMultiplier != value)
			{
				this._springDamperMultiplier = value;
				this.SyncJoint();
			}
		}
	}

	// Token: 0x17000F3D RID: 3901
	// (get) Token: 0x060067F2 RID: 26610 RVA: 0x002702F7 File Offset: 0x0026E6F7
	// (set) Token: 0x060067F3 RID: 26611 RVA: 0x002702FF File Offset: 0x0026E6FF
	public bool springDamperMultiplierOn
	{
		get
		{
			return this._springDamperMultiplierOn;
		}
		set
		{
			if (this._springDamperMultiplierOn != value)
			{
				this._springDamperMultiplierOn = value;
				this.SyncJoint();
			}
		}
	}

	// Token: 0x060067F4 RID: 26612 RVA: 0x0027031C File Offset: 0x0026E71C
	protected void SyncJointPositionXDrive()
	{
		if (this.joint1 != null)
		{
			JointDrive xDrive = this.joint1.xDrive;
			xDrive.positionSpring = this._positionSpringX;
			xDrive.positionDamper = this._positionDamperX;
			this.joint1.xDrive = xDrive;
			Rigidbody component = this.joint1.GetComponent<Rigidbody>();
			if (component != null)
			{
				component.WakeUp();
			}
		}
		if (this.joint2 != null)
		{
			JointDrive xDrive2 = this.joint2.xDrive;
			xDrive2.positionSpring = this._positionSpringX;
			xDrive2.positionDamper = this._positionDamperX;
			this.joint2.xDrive = xDrive2;
			Rigidbody component2 = this.joint2.GetComponent<Rigidbody>();
			if (component2 != null)
			{
				component2.WakeUp();
			}
		}
	}

	// Token: 0x060067F5 RID: 26613 RVA: 0x002703EB File Offset: 0x0026E7EB
	protected void SyncPositionSpringX(float f)
	{
		this._positionSpringX = f;
		this.SyncJointPositionXDrive();
	}

	// Token: 0x17000F3E RID: 3902
	// (get) Token: 0x060067F6 RID: 26614 RVA: 0x002703FA File Offset: 0x0026E7FA
	// (set) Token: 0x060067F7 RID: 26615 RVA: 0x00270402 File Offset: 0x0026E802
	public float positionSpringX
	{
		get
		{
			return this._positionSpringX;
		}
		set
		{
			if (this.positionSpringXJSON != null)
			{
				this.positionSpringXJSON.val = value;
			}
			else if (this._positionSpringX != value)
			{
				this.SyncPositionSpringX(value);
			}
		}
	}

	// Token: 0x060067F8 RID: 26616 RVA: 0x00270433 File Offset: 0x0026E833
	protected void SyncPositionDamperX(float f)
	{
		this._positionDamperX = f;
		this.SyncJointPositionXDrive();
	}

	// Token: 0x17000F3F RID: 3903
	// (get) Token: 0x060067F9 RID: 26617 RVA: 0x00270442 File Offset: 0x0026E842
	// (set) Token: 0x060067FA RID: 26618 RVA: 0x0027044A File Offset: 0x0026E84A
	public float positionDamperX
	{
		get
		{
			return this._positionDamperX;
		}
		set
		{
			if (this.positionSpringXJSON != null)
			{
				this.positionSpringXJSON.val = value;
			}
			else if (this._positionDamperX != value)
			{
				this.SyncPositionDamperX(value);
			}
		}
	}

	// Token: 0x060067FB RID: 26619 RVA: 0x0027047C File Offset: 0x0026E87C
	protected void SyncJointPositionYDrive()
	{
		if (this.joint1 != null)
		{
			JointDrive yDrive = this.joint1.yDrive;
			yDrive.positionSpring = this._positionSpringY;
			yDrive.positionDamper = this._positionDamperY;
			this.joint1.yDrive = yDrive;
			Rigidbody component = this.joint1.GetComponent<Rigidbody>();
			if (component != null)
			{
				component.WakeUp();
			}
		}
		if (this.joint2 != null)
		{
			JointDrive yDrive2 = this.joint2.yDrive;
			yDrive2.positionSpring = this._positionSpringY;
			yDrive2.positionDamper = this._positionDamperY;
			this.joint2.yDrive = yDrive2;
			Rigidbody component2 = this.joint2.GetComponent<Rigidbody>();
			if (component2 != null)
			{
				component2.WakeUp();
			}
		}
	}

	// Token: 0x060067FC RID: 26620 RVA: 0x0027054B File Offset: 0x0026E94B
	protected void SyncPositionSpringY(float f)
	{
		this._positionSpringY = f;
		this.SyncJointPositionYDrive();
	}

	// Token: 0x17000F40 RID: 3904
	// (get) Token: 0x060067FD RID: 26621 RVA: 0x0027055A File Offset: 0x0026E95A
	// (set) Token: 0x060067FE RID: 26622 RVA: 0x00270562 File Offset: 0x0026E962
	public float positionSpringY
	{
		get
		{
			return this._positionSpringY;
		}
		set
		{
			if (this.positionSpringYJSON != null)
			{
				this.positionSpringYJSON.val = value;
			}
			else if (this._positionSpringY != value)
			{
				this.SyncPositionSpringY(value);
			}
		}
	}

	// Token: 0x060067FF RID: 26623 RVA: 0x00270593 File Offset: 0x0026E993
	protected void SyncPositionDamperY(float f)
	{
		this._positionDamperY = f;
		this.SyncJointPositionYDrive();
	}

	// Token: 0x17000F41 RID: 3905
	// (get) Token: 0x06006800 RID: 26624 RVA: 0x002705A2 File Offset: 0x0026E9A2
	// (set) Token: 0x06006801 RID: 26625 RVA: 0x002705AA File Offset: 0x0026E9AA
	public float positionDamperY
	{
		get
		{
			return this._positionDamperY;
		}
		set
		{
			if (this.positionDamperYJSON != null)
			{
				this.positionDamperYJSON.val = value;
			}
			else if (this._positionDamperY != value)
			{
				this.SyncPositionDamperY(value);
			}
		}
	}

	// Token: 0x06006802 RID: 26626 RVA: 0x002705DC File Offset: 0x0026E9DC
	protected void SyncJointPositionZDrive()
	{
		if (this.joint1 != null)
		{
			JointDrive zDrive = this.joint1.zDrive;
			zDrive.positionSpring = this._positionSpringZ;
			zDrive.positionDamper = this._positionDamperZ;
			this.joint1.zDrive = zDrive;
			Rigidbody component = this.joint1.GetComponent<Rigidbody>();
			if (component != null)
			{
				component.WakeUp();
			}
		}
		if (this.joint2 != null)
		{
			JointDrive zDrive2 = this.joint2.zDrive;
			zDrive2.positionSpring = this._positionSpringZ;
			zDrive2.positionDamper = this._positionDamperZ;
			this.joint2.zDrive = zDrive2;
			Rigidbody component2 = this.joint2.GetComponent<Rigidbody>();
			if (component2 != null)
			{
				component2.WakeUp();
			}
		}
	}

	// Token: 0x06006803 RID: 26627 RVA: 0x002706AB File Offset: 0x0026EAAB
	protected void SyncPositionSpringZ(float f)
	{
		this._positionSpringZ = f;
		this.SyncJointPositionZDrive();
	}

	// Token: 0x17000F42 RID: 3906
	// (get) Token: 0x06006804 RID: 26628 RVA: 0x002706BA File Offset: 0x0026EABA
	// (set) Token: 0x06006805 RID: 26629 RVA: 0x002706C2 File Offset: 0x0026EAC2
	public float positionSpringZ
	{
		get
		{
			return this._positionSpringZ;
		}
		set
		{
			if (this.positionSpringZJSON != null)
			{
				this.positionSpringZJSON.val = value;
			}
			else if (this._positionSpringZ != value)
			{
				this.SyncPositionSpringZ(value);
			}
		}
	}

	// Token: 0x06006806 RID: 26630 RVA: 0x002706F3 File Offset: 0x0026EAF3
	protected void SyncPositionDamperZ(float f)
	{
		this._positionDamperZ = f;
		this.SyncJointPositionZDrive();
	}

	// Token: 0x17000F43 RID: 3907
	// (get) Token: 0x06006807 RID: 26631 RVA: 0x00270702 File Offset: 0x0026EB02
	// (set) Token: 0x06006808 RID: 26632 RVA: 0x0027070A File Offset: 0x0026EB0A
	public float positionDamperZ
	{
		get
		{
			return this._positionDamperZ;
		}
		set
		{
			if (this.positionDamperZJSON != null)
			{
				this.positionDamperZJSON.val = value;
			}
			else if (this._positionDamperZ != value)
			{
				this.SyncPositionDamperZ(value);
			}
		}
	}

	// Token: 0x06006809 RID: 26633 RVA: 0x0027073C File Offset: 0x0026EB3C
	protected void SetTargetRotation()
	{
		if (this.joint1 != null)
		{
			DAZBone component = this.joint1.GetComponent<DAZBone>();
			if (component != null)
			{
				Vector3 baseJointRotation = this.smoothedJoint1TargetRotation;
				baseJointRotation.x = -baseJointRotation.x;
				component.baseJointRotation = baseJointRotation;
			}
			else
			{
				Quaternion targetRotation = Quaternion.Euler(this.smoothedJoint1TargetRotation);
				this.joint1.targetRotation = targetRotation;
			}
		}
		if (this.joint2 != null)
		{
			DAZBone component2 = this.joint2.GetComponent<DAZBone>();
			if (component2 != null)
			{
				Vector3 baseJointRotation2 = this.smoothedJoint2TargetRotation;
				baseJointRotation2.x = -baseJointRotation2.x;
				component2.baseJointRotation = baseJointRotation2;
			}
			else
			{
				Quaternion targetRotation2 = Quaternion.Euler(this.smoothedJoint2TargetRotation);
				this.joint2.targetRotation = targetRotation2;
			}
		}
	}

	// Token: 0x0600680A RID: 26634 RVA: 0x00270814 File Offset: 0x0026EC14
	protected void SmoothTargetRotation()
	{
		if (this.joint1 != null)
		{
			Vector3 b = (1f - 0.5f * this.smoothTargetRotationDamper) * this.smoothedJoint1TargetRotationVelocity + 0.5f * this.smoothTargetRotationSpring * (this.setJoint1TargetRotation - this.smoothedJoint1TargetRotation);
			this.smoothedJoint1TargetRotationVelocity = b;
			this.smoothedJoint1TargetRotation += b;
		}
		if (this.joint2 != null)
		{
			Vector3 b2 = (1f - 0.5f * this.smoothTargetRotationDamper) * this.smoothedJoint2TargetRotationVelocity + 0.5f * this.smoothTargetRotationSpring * (this.setJoint2TargetRotation - this.smoothedJoint2TargetRotation);
			this.smoothedJoint2TargetRotationVelocity = b2;
			this.smoothedJoint2TargetRotation += b2;
		}
		this.SetTargetRotation();
	}

	// Token: 0x0600680B RID: 26635 RVA: 0x00270908 File Offset: 0x0026ED08
	public void SyncTargetRotation()
	{
		if (this._useDAZMorphForRotation)
		{
			SetDAZMorph component = base.GetComponent<SetDAZMorph>();
			if (component != null && component.enabled)
			{
				component.morphRawValue = (this.targetRotationX + this.additionalJoint1RotationX) * this.DAZMorphAngleMultiplier;
			}
		}
		else
		{
			if (this.joint1 != null)
			{
				this.setJoint1TargetRotation.x = this.targetRotationX + this.additionalJoint1RotationX;
				this.setJoint1TargetRotation.y = this.targetRotationY + this.additionalJoint1RotationY;
				this.setJoint1TargetRotation.z = this.targetRotationZ + this.additionalJoint1RotationZ;
			}
			if (this.joint2 != null)
			{
				if (this.invertJoint2RotationX)
				{
					this.setJoint2TargetRotation.x = -this.targetRotationX + this.additionalJoint2RotationX;
				}
				else
				{
					this.setJoint2TargetRotation.x = this.targetRotationX + this.additionalJoint2RotationX;
				}
				if (this.invertJoint2RotationY)
				{
					this.setJoint2TargetRotation.y = -this.targetRotationY + this.additionalJoint2RotationY;
				}
				else
				{
					this.setJoint2TargetRotation.y = this.targetRotationY + this.additionalJoint2RotationY;
				}
				if (this.invertJoint2RotationZ)
				{
					this.setJoint2TargetRotation.z = -this.targetRotationZ + this.additionalJoint2RotationZ;
				}
				else
				{
					this.setJoint2TargetRotation.z = this.targetRotationZ + this.additionalJoint2RotationZ;
				}
			}
			if (!this.useSmoothChanges)
			{
				this.smoothedJoint1TargetRotation = this.setJoint1TargetRotation;
				this.smoothedJoint2TargetRotation = this.setJoint2TargetRotation;
				this.SetTargetRotation();
			}
		}
	}

	// Token: 0x0600680C RID: 26636 RVA: 0x00270AB3 File Offset: 0x0026EEB3
	protected void SyncSmoothTargetRotationSpring(float f)
	{
		this._smoothTargetRotationSpring = f;
	}

	// Token: 0x17000F44 RID: 3908
	// (get) Token: 0x0600680D RID: 26637 RVA: 0x00270ABC File Offset: 0x0026EEBC
	// (set) Token: 0x0600680E RID: 26638 RVA: 0x00270AC4 File Offset: 0x0026EEC4
	public float smoothTargetRotationSpring
	{
		get
		{
			return this._smoothTargetRotationSpring;
		}
		set
		{
			if (this.smoothTargetRotationSpringJSON != null)
			{
				this.smoothTargetRotationSpringJSON.val = value;
			}
			else if (this._smoothTargetRotationSpring != value)
			{
				this.SyncSmoothTargetRotationSpring(value);
			}
		}
	}

	// Token: 0x0600680F RID: 26639 RVA: 0x00270AF5 File Offset: 0x0026EEF5
	protected void SyncSmoothTargetRotationDamper(float f)
	{
		this._smoothTargetRotationDamper = f;
	}

	// Token: 0x17000F45 RID: 3909
	// (get) Token: 0x06006810 RID: 26640 RVA: 0x00270AFE File Offset: 0x0026EEFE
	// (set) Token: 0x06006811 RID: 26641 RVA: 0x00270B06 File Offset: 0x0026EF06
	public float smoothTargetRotationDamper
	{
		get
		{
			return this._smoothTargetRotationDamper;
		}
		set
		{
			if (this.smoothTargetRotationDamperJSON != null)
			{
				this.smoothTargetRotationDamperJSON.val = value;
			}
			else if (this._smoothTargetRotationDamper != value)
			{
				this.SyncSmoothTargetRotationDamper(value);
			}
		}
	}

	// Token: 0x17000F46 RID: 3910
	// (get) Token: 0x06006812 RID: 26642 RVA: 0x00270B37 File Offset: 0x0026EF37
	// (set) Token: 0x06006813 RID: 26643 RVA: 0x00270B3F File Offset: 0x0026EF3F
	public bool useDAZMorphForRotation
	{
		get
		{
			return this._useDAZMorphForRotation;
		}
		set
		{
			if (this._useDAZMorphForRotation != value)
			{
				this._useDAZMorphForRotation = value;
				this.SyncTargetRotation();
			}
		}
	}

	// Token: 0x06006814 RID: 26644 RVA: 0x00270B5A File Offset: 0x0026EF5A
	protected void SyncTargetRotationX(float f)
	{
		this._targetRotationX = f;
		this.SyncTargetRotation();
	}

	// Token: 0x17000F47 RID: 3911
	// (get) Token: 0x06006815 RID: 26645 RVA: 0x00270B69 File Offset: 0x0026EF69
	// (set) Token: 0x06006816 RID: 26646 RVA: 0x00270B71 File Offset: 0x0026EF71
	public float targetRotationX
	{
		get
		{
			return this._targetRotationX;
		}
		set
		{
			if (this.targetRotationXJSON != null)
			{
				this.targetRotationXJSON.val = value;
			}
			else if (this._targetRotationX != value)
			{
				this.SyncTargetRotationX(value);
			}
		}
	}

	// Token: 0x06006817 RID: 26647 RVA: 0x00270BA2 File Offset: 0x0026EFA2
	protected void SyncTargetRotationY(float f)
	{
		this._targetRotationY = f;
		this.SyncTargetRotation();
	}

	// Token: 0x17000F48 RID: 3912
	// (get) Token: 0x06006818 RID: 26648 RVA: 0x00270BB1 File Offset: 0x0026EFB1
	// (set) Token: 0x06006819 RID: 26649 RVA: 0x00270BB9 File Offset: 0x0026EFB9
	public float targetRotationY
	{
		get
		{
			return this._targetRotationY;
		}
		set
		{
			if (this.targetRotationYJSON != null)
			{
				this.targetRotationYJSON.val = value;
			}
			else if (this._targetRotationY != value)
			{
				this.SyncTargetRotationY(value);
			}
		}
	}

	// Token: 0x0600681A RID: 26650 RVA: 0x00270BEA File Offset: 0x0026EFEA
	protected void SyncTargetRotationZ(float f)
	{
		this._targetRotationZ = f;
		this.SyncTargetRotation();
	}

	// Token: 0x17000F49 RID: 3913
	// (get) Token: 0x0600681B RID: 26651 RVA: 0x00270BF9 File Offset: 0x0026EFF9
	// (set) Token: 0x0600681C RID: 26652 RVA: 0x00270C01 File Offset: 0x0026F001
	public float targetRotationZ
	{
		get
		{
			return this._targetRotationZ;
		}
		set
		{
			if (this.targetRotationZJSON != null)
			{
				this.targetRotationZJSON.val = value;
			}
			else if (this._targetRotationZ != value)
			{
				this.SyncTargetRotationZ(value);
			}
		}
	}

	// Token: 0x0600681D RID: 26653 RVA: 0x00270C32 File Offset: 0x0026F032
	protected void SyncDriveXRotationFromAudioSource(bool b)
	{
		this._driveXRotationFromAudioSource = b;
	}

	// Token: 0x17000F4A RID: 3914
	// (get) Token: 0x0600681E RID: 26654 RVA: 0x00270C3B File Offset: 0x0026F03B
	// (set) Token: 0x0600681F RID: 26655 RVA: 0x00270C43 File Offset: 0x0026F043
	public bool driveXRotationFromAudioSource
	{
		get
		{
			return this._driveXRotationFromAudioSource;
		}
		set
		{
			if (this.driveXRotationFromAudioSourceJSON != null)
			{
				this.driveXRotationFromAudioSourceJSON.val = value;
			}
			else if (this._driveXRotationFromAudioSource != value)
			{
				this.SyncDriveXRotationFromAudioSource(value);
			}
		}
	}

	// Token: 0x06006820 RID: 26656 RVA: 0x00270C74 File Offset: 0x0026F074
	protected void SyncDriveXRotationFromAudioSourceMultiplier(float f)
	{
		this._driveXRotationFromAudioSourceMultiplier = f;
	}

	// Token: 0x17000F4B RID: 3915
	// (get) Token: 0x06006821 RID: 26657 RVA: 0x00270C7D File Offset: 0x0026F07D
	// (set) Token: 0x06006822 RID: 26658 RVA: 0x00270C85 File Offset: 0x0026F085
	public float driveXRotationFromAudioSourceMultiplier
	{
		get
		{
			return this._driveXRotationFromAudioSourceMultiplier;
		}
		set
		{
			if (this.driveXRotationFromAudioSourceMultiplierJSON != null)
			{
				this.driveXRotationFromAudioSourceMultiplierJSON.val = value;
			}
			else if (this._driveXRotationFromAudioSourceMultiplier != value)
			{
				this.SyncDriveXRotationFromAudioSourceMultiplier(value);
			}
		}
	}

	// Token: 0x06006823 RID: 26659 RVA: 0x00270CB6 File Offset: 0x0026F0B6
	protected void SyncDriveXRotationFromAudioSourceAdditionalAngle(float f)
	{
		this._driveXRotationFromAudioSourceAdditionalAngle = f;
	}

	// Token: 0x17000F4C RID: 3916
	// (get) Token: 0x06006824 RID: 26660 RVA: 0x00270CBF File Offset: 0x0026F0BF
	// (set) Token: 0x06006825 RID: 26661 RVA: 0x00270CC7 File Offset: 0x0026F0C7
	public float driveXRotationFromAudioSourceAdditionalAngle
	{
		get
		{
			return this._driveXRotationFromAudioSourceAdditionalAngle;
		}
		set
		{
			if (this.driveXRotationFromAudioSourceAdditionalAngleJSON != null)
			{
				this.driveXRotationFromAudioSourceAdditionalAngleJSON.val = value;
			}
			else if (this._driveXRotationFromAudioSourceAdditionalAngle != value)
			{
				this.SyncDriveXRotationFromAudioSourceAdditionalAngle(value);
			}
		}
	}

	// Token: 0x06006826 RID: 26662 RVA: 0x00270CF8 File Offset: 0x0026F0F8
	protected void SyncDriveXRotationFromAudioSourceMaxAngle(float f)
	{
		this._driveXRotationFromAudioSourceMaxAngle = f;
	}

	// Token: 0x17000F4D RID: 3917
	// (get) Token: 0x06006827 RID: 26663 RVA: 0x00270D01 File Offset: 0x0026F101
	// (set) Token: 0x06006828 RID: 26664 RVA: 0x00270D09 File Offset: 0x0026F109
	public float driveXRotationFromAudioSourceMaxAngle
	{
		get
		{
			return this._driveXRotationFromAudioSourceMaxAngle;
		}
		set
		{
			if (this.driveXRotationFromAudioSourceMaxAngleJSON != null)
			{
				this.driveXRotationFromAudioSourceMaxAngleJSON.val = value;
			}
			else if (this._driveXRotationFromAudioSourceMaxAngle != value)
			{
				this.SyncDriveXRotationFromAudioSourceMaxAngle(value);
			}
		}
	}

	// Token: 0x06006829 RID: 26665 RVA: 0x00270D3C File Offset: 0x0026F13C
	public override void InitUI()
	{
		if (this.UITransform != null && base.isActiveAndEnabled)
		{
			AdjustJointsUI componentInChildren = this.UITransform.GetComponentInChildren<AdjustJointsUI>(true);
			if (componentInChildren != null)
			{
				this.massJSON.slider = componentInChildren.massSlider;
				this.centerOfGravityPercentJSON.slider = componentInChildren.centerOfGravityPercentSlider;
				this.springJSON.slider = componentInChildren.springSlider;
				this.damperJSON.slider = componentInChildren.damperSlider;
				this.positionSpringXJSON.slider = componentInChildren.positionSpringXSlider;
				this.positionSpringYJSON.slider = componentInChildren.positionSpringYSlider;
				this.positionSpringZJSON.slider = componentInChildren.positionSpringZSlider;
				this.positionDamperXJSON.slider = componentInChildren.positionDamperXSlider;
				this.positionDamperYJSON.slider = componentInChildren.positionDamperYSlider;
				this.positionDamperZJSON.slider = componentInChildren.positionDamperZSlider;
				if (this.smoothTargetRotationSpringJSON != null)
				{
					this.smoothTargetRotationSpringJSON.slider = componentInChildren.smoothTargetRotationSpringSlider;
				}
				if (this.smoothTargetRotationDamperJSON != null)
				{
					this.smoothTargetRotationDamperJSON.slider = componentInChildren.smoothTargetRotationDamperSlider;
				}
				this.targetRotationXJSON.slider = componentInChildren.targetRotationXSlider;
				this.targetRotationYJSON.slider = componentInChildren.targetRotationYSlider;
				this.targetRotationZJSON.slider = componentInChildren.targetRotationZSlider;
				if (this.audioSourceControl != null)
				{
					this.driveXRotationFromAudioSourceJSON.toggle = componentInChildren.driveXRotationFromAudioSourceToggle;
					this.driveXRotationFromAudioSourceMultiplierJSON.slider = componentInChildren.driveXRotationFromAudioSourceMultiplierSlider;
					this.driveXRotationFromAudioSourceAdditionalAngleJSON.slider = componentInChildren.driveXRotationFromAudioSourceAdditionalAngleSlider;
					this.driveXRotationFromAudioSourceMaxAngleJSON.slider = componentInChildren.driveXRotationFromAudioSourceMaxAngleSlider;
				}
			}
		}
	}

	// Token: 0x0600682A RID: 26666 RVA: 0x00270EE8 File Offset: 0x0026F2E8
	public override void InitUIAlt()
	{
		if (this.UITransformAlt != null && base.isActiveAndEnabled)
		{
			AdjustJointsUI componentInChildren = this.UITransformAlt.GetComponentInChildren<AdjustJointsUI>(true);
			if (componentInChildren != null)
			{
				this.massJSON.sliderAlt = componentInChildren.massSlider;
				this.centerOfGravityPercentJSON.sliderAlt = componentInChildren.centerOfGravityPercentSlider;
				this.springJSON.sliderAlt = componentInChildren.springSlider;
				this.damperJSON.sliderAlt = componentInChildren.damperSlider;
				this.positionSpringXJSON.sliderAlt = componentInChildren.positionSpringXSlider;
				this.positionSpringYJSON.sliderAlt = componentInChildren.positionSpringYSlider;
				this.positionSpringZJSON.sliderAlt = componentInChildren.positionSpringZSlider;
				this.positionDamperXJSON.sliderAlt = componentInChildren.positionDamperXSlider;
				this.positionDamperYJSON.sliderAlt = componentInChildren.positionDamperYSlider;
				this.positionDamperZJSON.sliderAlt = componentInChildren.positionDamperZSlider;
				if (this.smoothTargetRotationSpringJSON != null)
				{
					this.smoothTargetRotationSpringJSON.sliderAlt = componentInChildren.smoothTargetRotationSpringSlider;
				}
				if (this.smoothTargetRotationDamperJSON != null)
				{
					this.smoothTargetRotationDamperJSON.sliderAlt = componentInChildren.smoothTargetRotationDamperSlider;
				}
				this.targetRotationXJSON.sliderAlt = componentInChildren.targetRotationXSlider;
				this.targetRotationYJSON.sliderAlt = componentInChildren.targetRotationYSlider;
				this.targetRotationZJSON.sliderAlt = componentInChildren.targetRotationZSlider;
				if (this.audioSourceControl != null)
				{
					this.driveXRotationFromAudioSourceJSON.toggleAlt = componentInChildren.driveXRotationFromAudioSourceToggle;
					this.driveXRotationFromAudioSourceMultiplierJSON.sliderAlt = componentInChildren.driveXRotationFromAudioSourceMultiplierSlider;
					this.driveXRotationFromAudioSourceAdditionalAngleJSON.sliderAlt = componentInChildren.driveXRotationFromAudioSourceAdditionalAngleSlider;
					this.driveXRotationFromAudioSourceMaxAngleJSON.sliderAlt = componentInChildren.driveXRotationFromAudioSourceMaxAngleSlider;
				}
			}
		}
	}

	// Token: 0x0600682B RID: 26667 RVA: 0x00271094 File Offset: 0x0026F494
	protected void DeregisterUI()
	{
		this.massJSON.slider = null;
		this.centerOfGravityPercentJSON.slider = null;
		this.springJSON.slider = null;
		this.damperJSON.slider = null;
		this.positionSpringXJSON.slider = null;
		this.positionSpringYJSON.slider = null;
		this.positionSpringZJSON.slider = null;
		this.positionDamperXJSON.slider = null;
		this.positionDamperYJSON.slider = null;
		this.positionDamperZJSON.slider = null;
		if (this.smoothTargetRotationSpringJSON != null)
		{
			this.smoothTargetRotationSpringJSON.slider = null;
		}
		if (this.smoothTargetRotationDamperJSON != null)
		{
			this.smoothTargetRotationDamperJSON.slider = null;
		}
		this.targetRotationXJSON.slider = null;
		this.targetRotationYJSON.slider = null;
		this.targetRotationZJSON.slider = null;
		if (this.audioSourceControl != null)
		{
			this.driveXRotationFromAudioSourceJSON.toggle = null;
			this.driveXRotationFromAudioSourceMultiplierJSON.slider = null;
			this.driveXRotationFromAudioSourceAdditionalAngleJSON.slider = null;
			this.driveXRotationFromAudioSourceMaxAngleJSON.slider = null;
		}
	}

	// Token: 0x0600682C RID: 26668 RVA: 0x002711AC File Offset: 0x0026F5AC
	protected void SyncAll()
	{
		this.SyncMass(this._mass);
		this.SyncCenterOfGravity(this._centerOfGravityPercent);
		this.SyncJoint();
		this.SyncTargetRotation();
		this.SyncJointPositionXDrive();
		this.SyncJointPositionYDrive();
		this.SyncJointPositionZDrive();
	}

	// Token: 0x0600682D RID: 26669 RVA: 0x002711E4 File Offset: 0x0026F5E4
	protected void Init()
	{
		this.massJSON = new JSONStorableFloat("mass", this._mass, new JSONStorableFloat.SetFloatCallback(this.SyncMass), 0.1f, 2f, true, true);
		base.RegisterFloat(this.massJSON);
		this.centerOfGravityPercentJSON = new JSONStorableFloat("centerOfGravityPercent", this._centerOfGravityPercent, new JSONStorableFloat.SetFloatCallback(this.SyncCenterOfGravity), 0f, 1f, true, true);
		base.RegisterFloat(this.centerOfGravityPercentJSON);
		this.springJSON = new JSONStorableFloat("spring", this._spring, new JSONStorableFloat.SetFloatCallback(this.SyncSpring), 0f, 100f, true, true);
		base.RegisterFloat(this.springJSON);
		this.damperJSON = new JSONStorableFloat("damper", this._damper, new JSONStorableFloat.SetFloatCallback(this.SyncDamper), 0f, 5f, true, true);
		base.RegisterFloat(this.damperJSON);
		this.positionSpringXJSON = new JSONStorableFloat("positionSpringX", this._positionSpringX, new JSONStorableFloat.SetFloatCallback(this.SyncPositionSpringX), 0f, 1000f, true, true);
		base.RegisterFloat(this.positionSpringXJSON);
		this.positionSpringYJSON = new JSONStorableFloat("positionSpringY", this._positionSpringY, new JSONStorableFloat.SetFloatCallback(this.SyncPositionSpringY), 0f, 1000f, true, true);
		base.RegisterFloat(this.positionSpringYJSON);
		this.positionSpringZJSON = new JSONStorableFloat("positionSpringZ", this._positionSpringZ, new JSONStorableFloat.SetFloatCallback(this.SyncPositionSpringZ), 0f, 1000f, true, true);
		base.RegisterFloat(this.positionSpringZJSON);
		this.positionDamperXJSON = new JSONStorableFloat("positionDamperX", this._positionDamperX, new JSONStorableFloat.SetFloatCallback(this.SyncPositionDamperX), 0f, 1000f, true, true);
		base.RegisterFloat(this.positionDamperXJSON);
		this.positionDamperYJSON = new JSONStorableFloat("positionDamperY", this._positionDamperY, new JSONStorableFloat.SetFloatCallback(this.SyncPositionDamperY), 0f, 1000f, true, true);
		base.RegisterFloat(this.positionDamperYJSON);
		this.positionDamperZJSON = new JSONStorableFloat("positionDamperZ", this._positionDamperZ, new JSONStorableFloat.SetFloatCallback(this.SyncPositionDamperZ), 0f, 1000f, true, true);
		base.RegisterFloat(this.positionDamperZJSON);
		if (this.useSmoothChanges)
		{
			this.smoothTargetRotationSpringJSON = new JSONStorableFloat("smoothTargetRotationSpring", this._smoothTargetRotationSpring, new JSONStorableFloat.SetFloatCallback(this.SyncSmoothTargetRotationSpring), 0.1f, 1f, false, true);
			base.RegisterFloat(this.smoothTargetRotationSpringJSON);
			this.smoothTargetRotationDamperJSON = new JSONStorableFloat("smoothTargetRotationDamper", this._smoothTargetRotationDamper, new JSONStorableFloat.SetFloatCallback(this.SyncSmoothTargetRotationDamper), 0.1f, 1f, false, true);
			base.RegisterFloat(this.smoothTargetRotationDamperJSON);
		}
		this.targetRotationXJSON = new JSONStorableFloat("targetRotationX", this._targetRotationX, new JSONStorableFloat.SetFloatCallback(this.SyncTargetRotationX), -20f, 20f, true, true);
		base.RegisterFloat(this.targetRotationXJSON);
		this.targetRotationYJSON = new JSONStorableFloat("targetRotationY", this._targetRotationY, new JSONStorableFloat.SetFloatCallback(this.SyncTargetRotationY), -20f, 20f, true, true);
		base.RegisterFloat(this.targetRotationYJSON);
		this.targetRotationZJSON = new JSONStorableFloat("targetRotationZ", this._targetRotationZ, new JSONStorableFloat.SetFloatCallback(this.SyncTargetRotationZ), -20f, 20f, true, true);
		base.RegisterFloat(this.targetRotationZJSON);
		if (this.audioSourceControl != null)
		{
			this.driveXRotationFromAudioSourceJSON = new JSONStorableBool("driveXRotationFromAudioSource", this._driveXRotationFromAudioSource, new JSONStorableBool.SetBoolCallback(this.SyncDriveXRotationFromAudioSource));
			base.RegisterBool(this.driveXRotationFromAudioSourceJSON);
			this.driveXRotationFromAudioSourceMultiplierJSON = new JSONStorableFloat("driveXRotationFromAudioSourceMultiplier", this._driveXRotationFromAudioSourceMultiplier, new JSONStorableFloat.SetFloatCallback(this.SyncDriveXRotationFromAudioSourceMultiplier), 0f, 1000f, false, true);
			base.RegisterFloat(this.driveXRotationFromAudioSourceMultiplierJSON);
			this.driveXRotationFromAudioSourceAdditionalAngleJSON = new JSONStorableFloat("driveXRotationFromAudioSourceAdditionalAngle", this._driveXRotationFromAudioSourceAdditionalAngle, new JSONStorableFloat.SetFloatCallback(this.SyncDriveXRotationFromAudioSourceAdditionalAngle), -35f, 0f, true, true);
			base.RegisterFloat(this.driveXRotationFromAudioSourceAdditionalAngleJSON);
			this.driveXRotationFromAudioSourceMaxAngleJSON = new JSONStorableFloat("driveXRotationFromAudioSourceMaxAngle", this._driveXRotationFromAudioSourceMaxAngle, new JSONStorableFloat.SetFloatCallback(this.SyncDriveXRotationFromAudioSourceMaxAngle), -35f, 0f, true, true);
			base.RegisterFloat(this.driveXRotationFromAudioSourceMaxAngleJSON);
		}
		if (!this.isAppearance)
		{
			this.massJSON.storeType = JSONStorableParam.StoreType.Physical;
			this.centerOfGravityPercentJSON.storeType = JSONStorableParam.StoreType.Physical;
			this.springJSON.storeType = JSONStorableParam.StoreType.Physical;
			this.damperJSON.storeType = JSONStorableParam.StoreType.Physical;
			this.positionSpringXJSON.storeType = JSONStorableParam.StoreType.Physical;
			this.positionSpringYJSON.storeType = JSONStorableParam.StoreType.Physical;
			this.positionSpringZJSON.storeType = JSONStorableParam.StoreType.Physical;
			this.positionDamperXJSON.storeType = JSONStorableParam.StoreType.Physical;
			this.positionDamperYJSON.storeType = JSONStorableParam.StoreType.Physical;
			this.positionDamperZJSON.storeType = JSONStorableParam.StoreType.Physical;
			if (this.useSmoothChanges)
			{
				this.smoothTargetRotationSpringJSON.storeType = JSONStorableParam.StoreType.Physical;
				this.smoothTargetRotationDamperJSON.storeType = JSONStorableParam.StoreType.Physical;
			}
			this.targetRotationXJSON.storeType = JSONStorableParam.StoreType.Physical;
			this.targetRotationYJSON.storeType = JSONStorableParam.StoreType.Physical;
			this.targetRotationZJSON.storeType = JSONStorableParam.StoreType.Physical;
			if (this.audioSourceControl != null)
			{
				this.driveXRotationFromAudioSourceJSON.storeType = JSONStorableParam.StoreType.Physical;
				this.driveXRotationFromAudioSourceMultiplierJSON.storeType = JSONStorableParam.StoreType.Physical;
				this.driveXRotationFromAudioSourceAdditionalAngleJSON.storeType = JSONStorableParam.StoreType.Physical;
				this.driveXRotationFromAudioSourceMaxAngleJSON.storeType = JSONStorableParam.StoreType.Physical;
			}
		}
		if (this.joint1 != null)
		{
			this._defaultJoint1SlerpMaxForce = this.joint1.slerpDrive.maximumForce;
			this._defaultJoint1XMaxForce = this.joint1.angularXDrive.maximumForce;
			this._defaultJoint1YZMaxForce = this.joint1.angularYZDrive.maximumForce;
		}
		if (this.joint2 != null)
		{
			this._defaultJoint2SlerpMaxForce = this.joint2.slerpDrive.maximumForce;
			this._defaultJoint2XMaxForce = this.joint2.angularXDrive.maximumForce;
			this._defaultJoint2YZMaxForce = this.joint2.angularYZDrive.maximumForce;
		}
	}

	// Token: 0x0600682E RID: 26670 RVA: 0x00271814 File Offset: 0x0026FC14
	private void Update()
	{
		if (this.driveXRotationFromAudioSource && this.audioSourceControl != null)
		{
			if (this._driveXRotationFromAudioSourceMaxAngle < 0f)
			{
				this.targetRotationX = Mathf.Clamp(this.audioSourceControl.smoothedClipLoudness * -this.driveXRotationFromAudioSourceMultiplier + this._driveXRotationFromAudioSourceAdditionalAngle, this._driveXRotationFromAudioSourceMaxAngle, 0f);
			}
			else
			{
				this.targetRotationX = Mathf.Clamp(this.audioSourceControl.smoothedClipLoudness * -this.driveXRotationFromAudioSourceMultiplier + this._driveXRotationFromAudioSourceAdditionalAngle, 0f, this._driveXRotationFromAudioSourceMaxAngle);
			}
		}
		if (this.useSmoothChanges)
		{
			this.SmoothTargetRotation();
		}
	}

	// Token: 0x0600682F RID: 26671 RVA: 0x002718C3 File Offset: 0x0026FCC3
	private void OnEnable()
	{
		this.InitUI();
		this.InitUIAlt();
		this.SyncAll();
	}

	// Token: 0x06006830 RID: 26672 RVA: 0x002718D7 File Offset: 0x0026FCD7
	private void OnDisable()
	{
		this.DeregisterUI();
	}

	// Token: 0x06006831 RID: 26673 RVA: 0x002718DF File Offset: 0x0026FCDF
	protected override void Awake()
	{
		if (!this.awakecalled)
		{
			base.Awake();
			this.Init();
		}
	}

	// Token: 0x040058CB RID: 22731
	public bool isAppearance;

	// Token: 0x040058CC RID: 22732
	public ConfigurableJoint joint1;

	// Token: 0x040058CD RID: 22733
	public ConfigurableJoint joint2;

	// Token: 0x040058CE RID: 22734
	protected JSONStorableFloat massJSON;

	// Token: 0x040058CF RID: 22735
	[SerializeField]
	protected float _mass;

	// Token: 0x040058D0 RID: 22736
	public bool useSetCenterOfGravity;

	// Token: 0x040058D1 RID: 22737
	public Vector3 lowCenterOfGravity;

	// Token: 0x040058D2 RID: 22738
	public Vector3 highCenterOfGravity;

	// Token: 0x040058D3 RID: 22739
	public bool useJoint1COGForJoint2 = true;

	// Token: 0x040058D4 RID: 22740
	public Vector3 lowCenterOfGravityJoint2;

	// Token: 0x040058D5 RID: 22741
	public Vector3 highCenterOfGravityJoint2;

	// Token: 0x040058D6 RID: 22742
	protected Vector3 currentCenterOfGravity;

	// Token: 0x040058D7 RID: 22743
	protected Vector3 currentCenterOfGravityJoint2;

	// Token: 0x040058D8 RID: 22744
	protected JSONStorableFloat centerOfGravityPercentJSON;

	// Token: 0x040058D9 RID: 22745
	[SerializeField]
	protected float _centerOfGravityPercent;

	// Token: 0x040058DA RID: 22746
	protected float scalePow = 1f;

	// Token: 0x040058DB RID: 22747
	[SerializeField]
	protected float _limitSpringMultiplier;

	// Token: 0x040058DC RID: 22748
	[SerializeField]
	protected float _limitDamperMultiplier;

	// Token: 0x040058DD RID: 22749
	protected JSONStorableFloat springJSON;

	// Token: 0x040058DE RID: 22750
	[SerializeField]
	protected float _spring;

	// Token: 0x040058DF RID: 22751
	protected JSONStorableFloat damperJSON;

	// Token: 0x040058E0 RID: 22752
	[SerializeField]
	protected float _damper;

	// Token: 0x040058E1 RID: 22753
	[SerializeField]
	protected float _springDamperMultiplier = 3f;

	// Token: 0x040058E2 RID: 22754
	[SerializeField]
	protected bool _springDamperMultiplierOn;

	// Token: 0x040058E3 RID: 22755
	protected float _defaultJoint1SlerpMaxForce;

	// Token: 0x040058E4 RID: 22756
	protected float _defaultJoint1XMaxForce;

	// Token: 0x040058E5 RID: 22757
	protected float _defaultJoint1YZMaxForce;

	// Token: 0x040058E6 RID: 22758
	protected float _defaultJoint2SlerpMaxForce;

	// Token: 0x040058E7 RID: 22759
	protected float _defaultJoint2XMaxForce;

	// Token: 0x040058E8 RID: 22760
	protected float _defaultJoint2YZMaxForce;

	// Token: 0x040058E9 RID: 22761
	protected JSONStorableFloat positionSpringXJSON;

	// Token: 0x040058EA RID: 22762
	[SerializeField]
	protected float _positionSpringX;

	// Token: 0x040058EB RID: 22763
	protected JSONStorableFloat positionDamperXJSON;

	// Token: 0x040058EC RID: 22764
	[SerializeField]
	protected float _positionDamperX;

	// Token: 0x040058ED RID: 22765
	protected JSONStorableFloat positionSpringYJSON;

	// Token: 0x040058EE RID: 22766
	[SerializeField]
	protected float _positionSpringY;

	// Token: 0x040058EF RID: 22767
	protected JSONStorableFloat positionDamperYJSON;

	// Token: 0x040058F0 RID: 22768
	[SerializeField]
	protected float _positionDamperY;

	// Token: 0x040058F1 RID: 22769
	protected JSONStorableFloat positionSpringZJSON;

	// Token: 0x040058F2 RID: 22770
	[SerializeField]
	protected float _positionSpringZ;

	// Token: 0x040058F3 RID: 22771
	protected JSONStorableFloat positionDamperZJSON;

	// Token: 0x040058F4 RID: 22772
	[SerializeField]
	protected float _positionDamperZ;

	// Token: 0x040058F5 RID: 22773
	public bool useSmoothChanges;

	// Token: 0x040058F6 RID: 22774
	public Vector3 setJoint1TargetRotation;

	// Token: 0x040058F7 RID: 22775
	public Vector3 setJoint2TargetRotation;

	// Token: 0x040058F8 RID: 22776
	public Vector3 smoothedJoint1TargetRotation;

	// Token: 0x040058F9 RID: 22777
	public Vector3 smoothedJoint1TargetRotationVelocity;

	// Token: 0x040058FA RID: 22778
	public Vector3 smoothedJoint2TargetRotation;

	// Token: 0x040058FB RID: 22779
	public Vector3 smoothedJoint2TargetRotationVelocity;

	// Token: 0x040058FC RID: 22780
	protected JSONStorableFloat smoothTargetRotationSpringJSON;

	// Token: 0x040058FD RID: 22781
	[SerializeField]
	protected float _smoothTargetRotationSpring = 1f;

	// Token: 0x040058FE RID: 22782
	protected JSONStorableFloat smoothTargetRotationDamperJSON;

	// Token: 0x040058FF RID: 22783
	[SerializeField]
	protected float _smoothTargetRotationDamper = 1f;

	// Token: 0x04005900 RID: 22784
	[SerializeField]
	protected bool _useDAZMorphForRotation;

	// Token: 0x04005901 RID: 22785
	public float DAZMorphAngleMultiplier = 0.05f;

	// Token: 0x04005902 RID: 22786
	protected JSONStorableFloat targetRotationXJSON;

	// Token: 0x04005903 RID: 22787
	[SerializeField]
	protected float _targetRotationX;

	// Token: 0x04005904 RID: 22788
	public bool invertJoint2RotationX;

	// Token: 0x04005905 RID: 22789
	protected JSONStorableFloat targetRotationYJSON;

	// Token: 0x04005906 RID: 22790
	[SerializeField]
	protected float _targetRotationY;

	// Token: 0x04005907 RID: 22791
	public bool invertJoint2RotationY;

	// Token: 0x04005908 RID: 22792
	protected JSONStorableFloat targetRotationZJSON;

	// Token: 0x04005909 RID: 22793
	[SerializeField]
	protected float _targetRotationZ;

	// Token: 0x0400590A RID: 22794
	public bool invertJoint2RotationZ;

	// Token: 0x0400590B RID: 22795
	public float additionalJoint1RotationX;

	// Token: 0x0400590C RID: 22796
	public float additionalJoint1RotationY;

	// Token: 0x0400590D RID: 22797
	public float additionalJoint1RotationZ;

	// Token: 0x0400590E RID: 22798
	public float additionalJoint2RotationX;

	// Token: 0x0400590F RID: 22799
	public float additionalJoint2RotationY;

	// Token: 0x04005910 RID: 22800
	public float additionalJoint2RotationZ;

	// Token: 0x04005911 RID: 22801
	public AudioSourceControl audioSourceControl;

	// Token: 0x04005912 RID: 22802
	protected JSONStorableBool driveXRotationFromAudioSourceJSON;

	// Token: 0x04005913 RID: 22803
	[SerializeField]
	protected bool _driveXRotationFromAudioSource;

	// Token: 0x04005914 RID: 22804
	protected JSONStorableFloat driveXRotationFromAudioSourceMultiplierJSON;

	// Token: 0x04005915 RID: 22805
	[SerializeField]
	protected float _driveXRotationFromAudioSourceMultiplier = 4000f;

	// Token: 0x04005916 RID: 22806
	protected JSONStorableFloat driveXRotationFromAudioSourceAdditionalAngleJSON;

	// Token: 0x04005917 RID: 22807
	[SerializeField]
	protected float _driveXRotationFromAudioSourceAdditionalAngle;

	// Token: 0x04005918 RID: 22808
	protected JSONStorableFloat driveXRotationFromAudioSourceMaxAngleJSON;

	// Token: 0x04005919 RID: 22809
	[SerializeField]
	protected float _driveXRotationFromAudioSourceMaxAngle = 30f;
}
