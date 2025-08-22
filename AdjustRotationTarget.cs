using System;
using UnityEngine;

// Token: 0x02000D50 RID: 3408
public class AdjustRotationTarget : MonoBehaviour
{
	// Token: 0x060068A0 RID: 26784 RVA: 0x00272B85 File Offset: 0x00270F85
	public AdjustRotationTarget()
	{
	}

	// Token: 0x060068A1 RID: 26785 RVA: 0x00272B94 File Offset: 0x00270F94
	protected void SetTargetRotationFromPercent()
	{
		if (this._xPercent < 0f)
		{
			this._currentTargetRotation.x = Mathf.Lerp(this._zeroTargetRotation.x, this._lowTargetRotation.x, -this._xPercent);
		}
		else
		{
			this._currentTargetRotation.x = Mathf.Lerp(this._zeroTargetRotation.x, this._highTargetRotation.x, this._xPercent);
		}
		if (this._yPercent < 0f)
		{
			this._currentTargetRotation.y = Mathf.Lerp(this._zeroTargetRotation.y, this._lowTargetRotation.y, -this._yPercent);
		}
		else
		{
			this._currentTargetRotation.y = Mathf.Lerp(this._zeroTargetRotation.y, this._highTargetRotation.y, this._yPercent);
		}
		if (this._zPercent < 0f)
		{
			this._currentTargetRotation.z = Mathf.Lerp(this._zeroTargetRotation.z, this._lowTargetRotation.z, -this._zPercent);
		}
		else
		{
			this._currentTargetRotation.z = Mathf.Lerp(this._zeroTargetRotation.z, this._highTargetRotation.z, this._zPercent);
		}
	}

	// Token: 0x17000F70 RID: 3952
	// (get) Token: 0x060068A2 RID: 26786 RVA: 0x00272CEB File Offset: 0x002710EB
	// (set) Token: 0x060068A3 RID: 26787 RVA: 0x00272CF3 File Offset: 0x002710F3
	public float xPercent
	{
		get
		{
			return this._xPercent;
		}
		set
		{
			if (this._xPercent != value)
			{
				this._xPercent = value;
				this.SetTargetRotationFromPercent();
				this.Adjust();
			}
		}
	}

	// Token: 0x17000F71 RID: 3953
	// (get) Token: 0x060068A4 RID: 26788 RVA: 0x00272D14 File Offset: 0x00271114
	// (set) Token: 0x060068A5 RID: 26789 RVA: 0x00272D1C File Offset: 0x0027111C
	public float yPercent
	{
		get
		{
			return this._yPercent;
		}
		set
		{
			if (this._yPercent != value)
			{
				this._yPercent = value;
				this.SetTargetRotationFromPercent();
				this.Adjust();
			}
		}
	}

	// Token: 0x17000F72 RID: 3954
	// (get) Token: 0x060068A6 RID: 26790 RVA: 0x00272D3D File Offset: 0x0027113D
	// (set) Token: 0x060068A7 RID: 26791 RVA: 0x00272D45 File Offset: 0x00271145
	public float zPercent
	{
		get
		{
			return this._zPercent;
		}
		set
		{
			if (this._zPercent != value)
			{
				this._zPercent = value;
				this.SetTargetRotationFromPercent();
				this.Adjust();
			}
		}
	}

	// Token: 0x17000F73 RID: 3955
	// (get) Token: 0x060068A8 RID: 26792 RVA: 0x00272D66 File Offset: 0x00271166
	// (set) Token: 0x060068A9 RID: 26793 RVA: 0x00272D6E File Offset: 0x0027116E
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
				this._xPercent = value;
				this._yPercent = value;
				this._zPercent = value;
				this.SetTargetRotationFromPercent();
				this.Adjust();
			}
		}
	}

	// Token: 0x17000F74 RID: 3956
	// (get) Token: 0x060068AA RID: 26794 RVA: 0x00272DA4 File Offset: 0x002711A4
	// (set) Token: 0x060068AB RID: 26795 RVA: 0x00272DAC File Offset: 0x002711AC
	public Vector3 lowTargetRotation
	{
		get
		{
			return this._lowTargetRotation;
		}
		set
		{
			if (this._lowTargetRotation != value)
			{
				this._lowTargetRotation = value;
				this.SetTargetRotationFromPercent();
				this.Adjust();
			}
		}
	}

	// Token: 0x17000F75 RID: 3957
	// (get) Token: 0x060068AC RID: 26796 RVA: 0x00272DD2 File Offset: 0x002711D2
	// (set) Token: 0x060068AD RID: 26797 RVA: 0x00272DDA File Offset: 0x002711DA
	public Vector3 zeroTargetRotation
	{
		get
		{
			return this._zeroTargetRotation;
		}
		set
		{
			if (this._zeroTargetRotation != value)
			{
				this._zeroTargetRotation = value;
				this.SetTargetRotationFromPercent();
				this.Adjust();
			}
		}
	}

	// Token: 0x17000F76 RID: 3958
	// (get) Token: 0x060068AE RID: 26798 RVA: 0x00272E00 File Offset: 0x00271200
	// (set) Token: 0x060068AF RID: 26799 RVA: 0x00272E08 File Offset: 0x00271208
	public Vector3 highTargetRotation
	{
		get
		{
			return this._highTargetRotation;
		}
		set
		{
			if (this._highTargetRotation != value)
			{
				this._highTargetRotation = value;
				this.SetTargetRotationFromPercent();
				this.Adjust();
			}
		}
	}

	// Token: 0x17000F77 RID: 3959
	// (get) Token: 0x060068B0 RID: 26800 RVA: 0x00272E2E File Offset: 0x0027122E
	// (set) Token: 0x060068B1 RID: 26801 RVA: 0x00272E36 File Offset: 0x00271236
	public Vector3 currentTargetRotation
	{
		get
		{
			return this._currentTargetRotation;
		}
		set
		{
			if (this._currentTargetRotation != value)
			{
				this._currentTargetRotation = value;
				this.Adjust();
			}
		}
	}

	// Token: 0x17000F78 RID: 3960
	// (get) Token: 0x060068B2 RID: 26802 RVA: 0x00272E56 File Offset: 0x00271256
	// (set) Token: 0x060068B3 RID: 26803 RVA: 0x00272E64 File Offset: 0x00271264
	public float currentTargetRotationX
	{
		get
		{
			return this._currentTargetRotation.x;
		}
		set
		{
			if (this._currentTargetRotation.x != value)
			{
				Vector3 currentTargetRotation = this._currentTargetRotation;
				currentTargetRotation.x = value;
				this._currentTargetRotation = currentTargetRotation;
				this.Adjust();
			}
		}
	}

	// Token: 0x17000F79 RID: 3961
	// (get) Token: 0x060068B4 RID: 26804 RVA: 0x00272E9E File Offset: 0x0027129E
	// (set) Token: 0x060068B5 RID: 26805 RVA: 0x00272EAC File Offset: 0x002712AC
	public float currentTargetRotationNegativeX
	{
		get
		{
			return -this._currentTargetRotation.x;
		}
		set
		{
			if (this._currentTargetRotation.x != -value)
			{
				Vector3 currentTargetRotation = this._currentTargetRotation;
				currentTargetRotation.x = -value;
				this._currentTargetRotation = currentTargetRotation;
				this.Adjust();
			}
		}
	}

	// Token: 0x17000F7A RID: 3962
	// (get) Token: 0x060068B6 RID: 26806 RVA: 0x00272EE8 File Offset: 0x002712E8
	// (set) Token: 0x060068B7 RID: 26807 RVA: 0x00272EF8 File Offset: 0x002712F8
	public float currentTargetRotationY
	{
		get
		{
			return this._currentTargetRotation.y;
		}
		set
		{
			if (this._currentTargetRotation.y != value)
			{
				Vector3 currentTargetRotation = this._currentTargetRotation;
				currentTargetRotation.y = value;
				this._currentTargetRotation = currentTargetRotation;
				this.Adjust();
			}
		}
	}

	// Token: 0x17000F7B RID: 3963
	// (get) Token: 0x060068B8 RID: 26808 RVA: 0x00272F32 File Offset: 0x00271332
	// (set) Token: 0x060068B9 RID: 26809 RVA: 0x00272F40 File Offset: 0x00271340
	public float currentTargetRotationNegativeY
	{
		get
		{
			return -this._currentTargetRotation.y;
		}
		set
		{
			if (this._currentTargetRotation.y != -value)
			{
				Vector3 currentTargetRotation = this._currentTargetRotation;
				currentTargetRotation.y = -value;
				this._currentTargetRotation = currentTargetRotation;
				this.Adjust();
			}
		}
	}

	// Token: 0x17000F7C RID: 3964
	// (get) Token: 0x060068BA RID: 26810 RVA: 0x00272F7C File Offset: 0x0027137C
	// (set) Token: 0x060068BB RID: 26811 RVA: 0x00272F8C File Offset: 0x0027138C
	public float currentTargetRotationZ
	{
		get
		{
			return this._currentTargetRotation.z;
		}
		set
		{
			if (this._currentTargetRotation.z != value)
			{
				Vector3 currentTargetRotation = this._currentTargetRotation;
				currentTargetRotation.z = value;
				this._currentTargetRotation = currentTargetRotation;
				this.Adjust();
			}
		}
	}

	// Token: 0x17000F7D RID: 3965
	// (get) Token: 0x060068BC RID: 26812 RVA: 0x00272FC6 File Offset: 0x002713C6
	// (set) Token: 0x060068BD RID: 26813 RVA: 0x00272FD4 File Offset: 0x002713D4
	public float currentTargetRotationNegativeZ
	{
		get
		{
			return -this._currentTargetRotation.z;
		}
		set
		{
			if (this._currentTargetRotation.z != -value)
			{
				Vector3 currentTargetRotation = this._currentTargetRotation;
				currentTargetRotation.z = -value;
				this._currentTargetRotation = currentTargetRotation;
				this.Adjust();
			}
		}
	}

	// Token: 0x060068BE RID: 26814 RVA: 0x00273010 File Offset: 0x00271410
	public void Adjust()
	{
		if (this.on && Application.isPlaying)
		{
			Quaternion localRotation = Quaternion.Euler(this._currentTargetRotation);
			base.transform.localRotation = localRotation;
		}
	}

	// Token: 0x060068BF RID: 26815 RVA: 0x0027304A File Offset: 0x0027144A
	private void Start()
	{
		this.Adjust();
	}

	// Token: 0x04005971 RID: 22897
	public bool on = true;

	// Token: 0x04005972 RID: 22898
	[SerializeField]
	protected float _xPercent;

	// Token: 0x04005973 RID: 22899
	[SerializeField]
	protected float _yPercent;

	// Token: 0x04005974 RID: 22900
	[SerializeField]
	protected float _zPercent;

	// Token: 0x04005975 RID: 22901
	[SerializeField]
	protected float _percent;

	// Token: 0x04005976 RID: 22902
	[SerializeField]
	protected Vector3 _lowTargetRotation;

	// Token: 0x04005977 RID: 22903
	[SerializeField]
	protected Vector3 _zeroTargetRotation;

	// Token: 0x04005978 RID: 22904
	[SerializeField]
	protected Vector3 _highTargetRotation;

	// Token: 0x04005979 RID: 22905
	[SerializeField]
	protected Vector3 _currentTargetRotation;
}
