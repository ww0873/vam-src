using System;
using UnityEngine;

// Token: 0x02000D3F RID: 3391
public class AdjustJointPositionDrive : MonoBehaviour
{
	// Token: 0x060067AE RID: 26542 RVA: 0x0026F5CE File Offset: 0x0026D9CE
	public AdjustJointPositionDrive()
	{
	}

	// Token: 0x17000F21 RID: 3873
	// (get) Token: 0x060067AF RID: 26543 RVA: 0x0026F5DD File Offset: 0x0026D9DD
	// (set) Token: 0x060067B0 RID: 26544 RVA: 0x0026F5E5 File Offset: 0x0026D9E5
	public float XDriveSpring
	{
		get
		{
			return this._XDriveSpring;
		}
		set
		{
			if (this._XDriveSpring != value)
			{
				this._XDriveSpring = value;
				this.Adjust();
			}
		}
	}

	// Token: 0x17000F22 RID: 3874
	// (get) Token: 0x060067B1 RID: 26545 RVA: 0x0026F600 File Offset: 0x0026DA00
	// (set) Token: 0x060067B2 RID: 26546 RVA: 0x0026F608 File Offset: 0x0026DA08
	public float XDriveDamper
	{
		get
		{
			return this._XDriveDamper;
		}
		set
		{
			if (this._XDriveDamper != value)
			{
				this._XDriveDamper = value;
				this.Adjust();
			}
		}
	}

	// Token: 0x17000F23 RID: 3875
	// (get) Token: 0x060067B3 RID: 26547 RVA: 0x0026F623 File Offset: 0x0026DA23
	// (set) Token: 0x060067B4 RID: 26548 RVA: 0x0026F62B File Offset: 0x0026DA2B
	public float YDriveSpring
	{
		get
		{
			return this._YDriveSpring;
		}
		set
		{
			if (this._YDriveSpring != value)
			{
				this._YDriveSpring = value;
				this.Adjust();
			}
		}
	}

	// Token: 0x17000F24 RID: 3876
	// (get) Token: 0x060067B5 RID: 26549 RVA: 0x0026F646 File Offset: 0x0026DA46
	// (set) Token: 0x060067B6 RID: 26550 RVA: 0x0026F64E File Offset: 0x0026DA4E
	public float YDriveDamper
	{
		get
		{
			return this._YDriveDamper;
		}
		set
		{
			if (this._YDriveDamper != value)
			{
				this._YDriveDamper = value;
				this.Adjust();
			}
		}
	}

	// Token: 0x17000F25 RID: 3877
	// (get) Token: 0x060067B7 RID: 26551 RVA: 0x0026F669 File Offset: 0x0026DA69
	// (set) Token: 0x060067B8 RID: 26552 RVA: 0x0026F671 File Offset: 0x0026DA71
	public float ZDriveSpring
	{
		get
		{
			return this._ZDriveSpring;
		}
		set
		{
			if (this._ZDriveSpring != value)
			{
				this._ZDriveSpring = value;
				this.Adjust();
			}
		}
	}

	// Token: 0x17000F26 RID: 3878
	// (get) Token: 0x060067B9 RID: 26553 RVA: 0x0026F68C File Offset: 0x0026DA8C
	// (set) Token: 0x060067BA RID: 26554 RVA: 0x0026F694 File Offset: 0x0026DA94
	public float ZDriveDamper
	{
		get
		{
			return this._ZDriveDamper;
		}
		set
		{
			if (this._ZDriveDamper != value)
			{
				this._ZDriveDamper = value;
				this.Adjust();
			}
		}
	}

	// Token: 0x17000F27 RID: 3879
	// (get) Token: 0x060067BB RID: 26555 RVA: 0x0026F6AF File Offset: 0x0026DAAF
	public ConfigurableJoint controlledJoint
	{
		get
		{
			if (this.CJ == null)
			{
				this.CJ = base.GetComponent<ConfigurableJoint>();
			}
			return this.CJ;
		}
	}

	// Token: 0x060067BC RID: 26556 RVA: 0x0026F6D4 File Offset: 0x0026DAD4
	private void Adjust()
	{
		if (this.on)
		{
			if (this.CJ == null)
			{
				this.CJ = base.GetComponent<ConfigurableJoint>();
			}
			if (this.CJ != null)
			{
				if (this.CJ.xDrive.positionSpring != this._XDriveSpring || this.CJ.xDrive.positionDamper != this._XDriveDamper)
				{
					JointDrive xDrive = this.CJ.xDrive;
					xDrive.positionSpring = this._XDriveSpring;
					xDrive.positionDamper = this._XDriveDamper;
					this.CJ.xDrive = xDrive;
				}
				if (this.CJ.yDrive.positionSpring != this._YDriveSpring || this.CJ.yDrive.positionDamper != this._YDriveDamper)
				{
					JointDrive yDrive = this.CJ.yDrive;
					yDrive.positionSpring = this._YDriveSpring;
					yDrive.positionDamper = this._YDriveDamper;
					this.CJ.yDrive = yDrive;
				}
				if (this.CJ.zDrive.positionSpring != this._ZDriveSpring || this.CJ.zDrive.positionDamper != this._ZDriveDamper)
				{
					JointDrive zDrive = this.CJ.zDrive;
					zDrive.positionSpring = this._ZDriveSpring;
					zDrive.positionDamper = this._ZDriveDamper;
					this.CJ.zDrive = zDrive;
				}
			}
		}
	}

	// Token: 0x040058B9 RID: 22713
	public bool on = true;

	// Token: 0x040058BA RID: 22714
	[SerializeField]
	private float _XDriveSpring;

	// Token: 0x040058BB RID: 22715
	[SerializeField]
	private float _XDriveDamper;

	// Token: 0x040058BC RID: 22716
	[SerializeField]
	private float _YDriveSpring;

	// Token: 0x040058BD RID: 22717
	[SerializeField]
	private float _YDriveDamper;

	// Token: 0x040058BE RID: 22718
	[SerializeField]
	private float _ZDriveSpring;

	// Token: 0x040058BF RID: 22719
	[SerializeField]
	private float _ZDriveDamper;

	// Token: 0x040058C0 RID: 22720
	private ConfigurableJoint CJ;
}
