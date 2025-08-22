using System;
using UnityEngine;

// Token: 0x02000D43 RID: 3395
public class AdjustJointSpring : ScaleChangeReceiver
{
	// Token: 0x06006832 RID: 26674 RVA: 0x002718F8 File Offset: 0x0026FCF8
	public AdjustJointSpring()
	{
	}

	// Token: 0x06006833 RID: 26675 RVA: 0x00271920 File Offset: 0x0026FD20
	private void SetSpringVarsFromPercent()
	{
		this._currentSpring = (this.highSpring - this.lowSpring) * this._percent + this.lowSpring;
		this._currentXSpring = this._currentSpring;
		this._currentYZSpring = this._currentSpring * this._yzMultiplier;
	}

	// Token: 0x17000F4E RID: 3918
	// (get) Token: 0x06006834 RID: 26676 RVA: 0x0027196D File Offset: 0x0026FD6D
	// (set) Token: 0x06006835 RID: 26677 RVA: 0x00271975 File Offset: 0x0026FD75
	public float percent
	{
		get
		{
			return this._percent;
		}
		set
		{
			if (this._percent != value)
			{
				this._percent = value;
				this.SetSpringVarsFromPercent();
				this.Adjust();
			}
		}
	}

	// Token: 0x06006836 RID: 26678 RVA: 0x00271996 File Offset: 0x0026FD96
	public override void ScaleChanged(float scale)
	{
		base.ScaleChanged(scale);
		this.scalePow = Mathf.Pow(2f, this._scale - 1f);
		this.Adjust();
	}

	// Token: 0x17000F4F RID: 3919
	// (get) Token: 0x06006837 RID: 26679 RVA: 0x002719C1 File Offset: 0x0026FDC1
	// (set) Token: 0x06006838 RID: 26680 RVA: 0x002719C9 File Offset: 0x0026FDC9
	public float lowSpring
	{
		get
		{
			return this._lowSpring;
		}
		set
		{
			if (this._lowSpring != value)
			{
				this._lowSpring = value;
				this.SetSpringVarsFromPercent();
				this.Adjust();
			}
		}
	}

	// Token: 0x17000F50 RID: 3920
	// (get) Token: 0x06006839 RID: 26681 RVA: 0x002719EA File Offset: 0x0026FDEA
	// (set) Token: 0x0600683A RID: 26682 RVA: 0x002719F2 File Offset: 0x0026FDF2
	public float defaultPercent
	{
		get
		{
			return this._defaultPercent;
		}
		set
		{
			if (this._defaultPercent != value)
			{
				this._defaultPercent = value;
			}
		}
	}

	// Token: 0x17000F51 RID: 3921
	// (get) Token: 0x0600683B RID: 26683 RVA: 0x00271A07 File Offset: 0x0026FE07
	// (set) Token: 0x0600683C RID: 26684 RVA: 0x00271A0F File Offset: 0x0026FE0F
	public float highSpring
	{
		get
		{
			return this._highSpring;
		}
		set
		{
			if (this._highSpring != value)
			{
				this._highSpring = value;
				this.SetSpringVarsFromPercent();
				this.Adjust();
			}
		}
	}

	// Token: 0x17000F52 RID: 3922
	// (get) Token: 0x0600683D RID: 26685 RVA: 0x00271A30 File Offset: 0x0026FE30
	// (set) Token: 0x0600683E RID: 26686 RVA: 0x00271A38 File Offset: 0x0026FE38
	public float yzMultiplier
	{
		get
		{
			return this._yzMultiplier;
		}
		set
		{
			if (this._yzMultiplier != value)
			{
				this._yzMultiplier = value;
				this.SetSpringVarsFromPercent();
				this.Adjust();
			}
		}
	}

	// Token: 0x17000F53 RID: 3923
	// (get) Token: 0x0600683F RID: 26687 RVA: 0x00271A59 File Offset: 0x0026FE59
	// (set) Token: 0x06006840 RID: 26688 RVA: 0x00271A61 File Offset: 0x0026FE61
	public float currentXSpring
	{
		get
		{
			return this._currentXSpring;
		}
		set
		{
			if (this._currentXSpring != value)
			{
				this._currentXSpring = value;
				this.Adjust();
			}
		}
	}

	// Token: 0x17000F54 RID: 3924
	// (get) Token: 0x06006841 RID: 26689 RVA: 0x00271A7C File Offset: 0x0026FE7C
	// (set) Token: 0x06006842 RID: 26690 RVA: 0x00271A84 File Offset: 0x0026FE84
	public float currentYZSpring
	{
		get
		{
			return this._currentYZSpring;
		}
		set
		{
			if (this._currentYZSpring != value)
			{
				this._currentYZSpring = value;
				this.Adjust();
			}
		}
	}

	// Token: 0x17000F55 RID: 3925
	// (get) Token: 0x06006843 RID: 26691 RVA: 0x00271A9F File Offset: 0x0026FE9F
	// (set) Token: 0x06006844 RID: 26692 RVA: 0x00271AA7 File Offset: 0x0026FEA7
	public float currentSpring
	{
		get
		{
			return this._currentSpring;
		}
		set
		{
			if (this._currentSpring != value)
			{
				this._currentSpring = value;
				this._currentXSpring = value;
				this._currentYZSpring = value * this._yzMultiplier;
				this.Adjust();
			}
		}
	}

	// Token: 0x06006845 RID: 26693 RVA: 0x00271AD7 File Offset: 0x0026FED7
	public void SetDefaultPercent()
	{
		this.percent = this._defaultPercent;
	}

	// Token: 0x17000F56 RID: 3926
	// (get) Token: 0x06006846 RID: 26694 RVA: 0x00271AE5 File Offset: 0x0026FEE5
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

	// Token: 0x06006847 RID: 26695 RVA: 0x00271B0C File Offset: 0x0026FF0C
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
				float num = this.scalePow;
				float num2 = this.scalePow;
				float num3 = num;
				JointDrive jointDrive = this.CJ.slerpDrive;
				if (!this._defaultsSet)
				{
					this._defaultDamper = jointDrive.positionDamper;
					this._defaultMaxForce = jointDrive.maximumForce;
				}
				jointDrive.positionSpring = this._currentSpring * num;
				jointDrive.positionDamper = this._defaultDamper * num2;
				jointDrive.maximumForce = this._defaultMaxForce * num3;
				this.CJ.slerpDrive = jointDrive;
				jointDrive = this.CJ.angularXDrive;
				if (!this._defaultsSet)
				{
					this._defaultXDamper = jointDrive.positionDamper;
					this._defaultXMaxForce = jointDrive.maximumForce;
				}
				jointDrive.positionSpring = this._currentXSpring * num;
				jointDrive.positionDamper = this._defaultXDamper * num2;
				jointDrive.maximumForce = this._defaultXMaxForce * num3;
				this.CJ.angularXDrive = jointDrive;
				jointDrive = this.CJ.angularYZDrive;
				if (!this._defaultsSet)
				{
					this._defaultYZDamper = jointDrive.positionDamper;
					this._defaultYZMaxForce = jointDrive.maximumForce;
				}
				jointDrive.positionSpring = this._currentYZSpring * num;
				jointDrive.positionDamper = this._defaultYZDamper * num2;
				jointDrive.maximumForce = this._defaultYZMaxForce * num3;
				this.CJ.angularYZDrive = jointDrive;
				this._defaultsSet = true;
				Rigidbody component = base.GetComponent<Rigidbody>();
				component.WakeUp();
			}
		}
	}

	// Token: 0x0400591A RID: 22810
	public bool on = true;

	// Token: 0x0400591B RID: 22811
	[SerializeField]
	private float _percent;

	// Token: 0x0400591C RID: 22812
	protected float scalePow = 1f;

	// Token: 0x0400591D RID: 22813
	[SerializeField]
	private float _lowSpring;

	// Token: 0x0400591E RID: 22814
	[SerializeField]
	private float _defaultPercent;

	// Token: 0x0400591F RID: 22815
	[SerializeField]
	private float _highSpring;

	// Token: 0x04005920 RID: 22816
	[SerializeField]
	private float _yzMultiplier = 1f;

	// Token: 0x04005921 RID: 22817
	[SerializeField]
	private float _currentXSpring;

	// Token: 0x04005922 RID: 22818
	[SerializeField]
	private float _currentYZSpring;

	// Token: 0x04005923 RID: 22819
	[SerializeField]
	private float _currentSpring;

	// Token: 0x04005924 RID: 22820
	private ConfigurableJoint CJ;

	// Token: 0x04005925 RID: 22821
	private bool _defaultsSet;

	// Token: 0x04005926 RID: 22822
	private float _defaultDamper;

	// Token: 0x04005927 RID: 22823
	private float _defaultMaxForce;

	// Token: 0x04005928 RID: 22824
	private float _defaultXDamper;

	// Token: 0x04005929 RID: 22825
	private float _defaultXMaxForce;

	// Token: 0x0400592A RID: 22826
	private float _defaultYZDamper;

	// Token: 0x0400592B RID: 22827
	private float _defaultYZMaxForce;
}
