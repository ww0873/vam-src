using System;
using UnityEngine;

// Token: 0x02000D3D RID: 3389
public class AdjustJointDamper : MonoBehaviour
{
	// Token: 0x06006793 RID: 26515 RVA: 0x0026F210 File Offset: 0x0026D610
	public AdjustJointDamper()
	{
	}

	// Token: 0x06006794 RID: 26516 RVA: 0x0026F22C File Offset: 0x0026D62C
	private void SetDamperVarsFromPercent()
	{
		this._currentDamper = (this.highDamper - this.lowDamper) * this._percent + this.lowDamper;
		this._currentXDamper = this._currentDamper;
		this._currentYZDamper = this._currentDamper * this._yzMultiplier;
	}

	// Token: 0x17000F16 RID: 3862
	// (get) Token: 0x06006795 RID: 26517 RVA: 0x0026F279 File Offset: 0x0026D679
	// (set) Token: 0x06006796 RID: 26518 RVA: 0x0026F281 File Offset: 0x0026D681
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
				this.SetDamperVarsFromPercent();
				this.Adjust();
			}
		}
	}

	// Token: 0x17000F17 RID: 3863
	// (get) Token: 0x06006797 RID: 26519 RVA: 0x0026F2A2 File Offset: 0x0026D6A2
	// (set) Token: 0x06006798 RID: 26520 RVA: 0x0026F2AA File Offset: 0x0026D6AA
	public float lowDamper
	{
		get
		{
			return this._lowDamper;
		}
		set
		{
			if (this._lowDamper != value)
			{
				this._lowDamper = value;
				this.SetDamperVarsFromPercent();
				this.Adjust();
			}
		}
	}

	// Token: 0x17000F18 RID: 3864
	// (get) Token: 0x06006799 RID: 26521 RVA: 0x0026F2CB File Offset: 0x0026D6CB
	// (set) Token: 0x0600679A RID: 26522 RVA: 0x0026F2D3 File Offset: 0x0026D6D3
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

	// Token: 0x17000F19 RID: 3865
	// (get) Token: 0x0600679B RID: 26523 RVA: 0x0026F2E8 File Offset: 0x0026D6E8
	// (set) Token: 0x0600679C RID: 26524 RVA: 0x0026F2F0 File Offset: 0x0026D6F0
	public float highDamper
	{
		get
		{
			return this._highDamper;
		}
		set
		{
			if (this._highDamper != value)
			{
				this._highDamper = value;
				this.SetDamperVarsFromPercent();
				this.Adjust();
			}
		}
	}

	// Token: 0x17000F1A RID: 3866
	// (get) Token: 0x0600679D RID: 26525 RVA: 0x0026F311 File Offset: 0x0026D711
	// (set) Token: 0x0600679E RID: 26526 RVA: 0x0026F319 File Offset: 0x0026D719
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
				this.SetDamperVarsFromPercent();
				this.Adjust();
			}
		}
	}

	// Token: 0x17000F1B RID: 3867
	// (get) Token: 0x0600679F RID: 26527 RVA: 0x0026F33A File Offset: 0x0026D73A
	// (set) Token: 0x060067A0 RID: 26528 RVA: 0x0026F342 File Offset: 0x0026D742
	public float currentDamper
	{
		get
		{
			return this._currentDamper;
		}
		set
		{
			if (this._currentDamper != value)
			{
				this._currentDamper = value;
				this._currentXDamper = value;
				this._currentYZDamper = value * this._yzMultiplier;
				this.Adjust();
			}
		}
	}

	// Token: 0x17000F1C RID: 3868
	// (get) Token: 0x060067A1 RID: 26529 RVA: 0x0026F372 File Offset: 0x0026D772
	// (set) Token: 0x060067A2 RID: 26530 RVA: 0x0026F37A File Offset: 0x0026D77A
	public float currentXDamper
	{
		get
		{
			return this._currentXDamper;
		}
		set
		{
			if (this._currentXDamper != value)
			{
				this._currentXDamper = value;
				this.Adjust();
			}
		}
	}

	// Token: 0x17000F1D RID: 3869
	// (get) Token: 0x060067A3 RID: 26531 RVA: 0x0026F395 File Offset: 0x0026D795
	// (set) Token: 0x060067A4 RID: 26532 RVA: 0x0026F39D File Offset: 0x0026D79D
	public float currentYZDamper
	{
		get
		{
			return this._currentYZDamper;
		}
		set
		{
			if (this._currentYZDamper != value)
			{
				this._currentYZDamper = value;
				this.Adjust();
			}
		}
	}

	// Token: 0x060067A5 RID: 26533 RVA: 0x0026F3B8 File Offset: 0x0026D7B8
	public void SetDefaultPercent()
	{
		this.percent = this._defaultPercent;
	}

	// Token: 0x17000F1E RID: 3870
	// (get) Token: 0x060067A6 RID: 26534 RVA: 0x0026F3C6 File Offset: 0x0026D7C6
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

	// Token: 0x060067A7 RID: 26535 RVA: 0x0026F3EC File Offset: 0x0026D7EC
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
				if (this.CJ.slerpDrive.positionDamper != this._currentDamper)
				{
					JointDrive slerpDrive = this.CJ.slerpDrive;
					slerpDrive.positionDamper = this._currentDamper;
					this.CJ.slerpDrive = slerpDrive;
				}
				if (this.CJ.angularXDrive.positionDamper != this._currentXDamper)
				{
					JointDrive angularXDrive = this.CJ.angularXDrive;
					angularXDrive.positionDamper = this._currentXDamper;
					this.CJ.angularXDrive = angularXDrive;
				}
				if (this.CJ.angularYZDrive.positionDamper != this._currentYZDamper)
				{
					JointDrive angularYZDrive = this.CJ.angularYZDrive;
					angularYZDrive.positionDamper = this._currentYZDamper;
					this.CJ.angularYZDrive = angularYZDrive;
				}
			}
		}
	}

	// Token: 0x040058AC RID: 22700
	public bool on = true;

	// Token: 0x040058AD RID: 22701
	[SerializeField]
	private float _percent;

	// Token: 0x040058AE RID: 22702
	[SerializeField]
	private float _lowDamper;

	// Token: 0x040058AF RID: 22703
	[SerializeField]
	private float _defaultPercent;

	// Token: 0x040058B0 RID: 22704
	[SerializeField]
	private float _highDamper;

	// Token: 0x040058B1 RID: 22705
	[SerializeField]
	private float _yzMultiplier = 1f;

	// Token: 0x040058B2 RID: 22706
	[SerializeField]
	private float _currentDamper;

	// Token: 0x040058B3 RID: 22707
	[SerializeField]
	private float _currentXDamper;

	// Token: 0x040058B4 RID: 22708
	[SerializeField]
	private float _currentYZDamper;

	// Token: 0x040058B5 RID: 22709
	private ConfigurableJoint CJ;
}
