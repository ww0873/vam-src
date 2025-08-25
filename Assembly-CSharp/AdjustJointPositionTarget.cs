using System;
using UnityEngine;

// Token: 0x02000D40 RID: 3392
public class AdjustJointPositionTarget : MonoBehaviour
{
	// Token: 0x060067BD RID: 26557 RVA: 0x0026F86B File Offset: 0x0026DC6B
	public AdjustJointPositionTarget()
	{
	}

	// Token: 0x060067BE RID: 26558 RVA: 0x0026F87C File Offset: 0x0026DC7C
	private void SetTargetPositionFromPercent()
	{
		if (this._percent < 0f)
		{
			this._currentTargetPosition = Vector3.Lerp(this._zeroTargetPosition, this._lowTargetPosition, -this._percent);
		}
		else
		{
			this._currentTargetPosition = Vector3.Lerp(this._zeroTargetPosition, this._highTargetPosition, this._percent);
		}
	}

	// Token: 0x17000F28 RID: 3880
	// (get) Token: 0x060067BF RID: 26559 RVA: 0x0026F8D9 File Offset: 0x0026DCD9
	// (set) Token: 0x060067C0 RID: 26560 RVA: 0x0026F8E1 File Offset: 0x0026DCE1
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
				this.SetTargetPositionFromPercent();
				this.Adjust();
			}
		}
	}

	// Token: 0x17000F29 RID: 3881
	// (get) Token: 0x060067C1 RID: 26561 RVA: 0x0026F902 File Offset: 0x0026DD02
	// (set) Token: 0x060067C2 RID: 26562 RVA: 0x0026F90A File Offset: 0x0026DD0A
	public Vector3 lowTargetPosition
	{
		get
		{
			return this._lowTargetPosition;
		}
		set
		{
			if (this._lowTargetPosition != value)
			{
				this._lowTargetPosition = value;
				this.SetTargetPositionFromPercent();
				this.Adjust();
			}
		}
	}

	// Token: 0x17000F2A RID: 3882
	// (get) Token: 0x060067C3 RID: 26563 RVA: 0x0026F930 File Offset: 0x0026DD30
	// (set) Token: 0x060067C4 RID: 26564 RVA: 0x0026F938 File Offset: 0x0026DD38
	public Vector3 zeroTargetPosition
	{
		get
		{
			return this._zeroTargetPosition;
		}
		set
		{
			if (this._zeroTargetPosition != value)
			{
				this._zeroTargetPosition = value;
				this.SetTargetPositionFromPercent();
				this.Adjust();
			}
		}
	}

	// Token: 0x17000F2B RID: 3883
	// (get) Token: 0x060067C5 RID: 26565 RVA: 0x0026F95E File Offset: 0x0026DD5E
	// (set) Token: 0x060067C6 RID: 26566 RVA: 0x0026F966 File Offset: 0x0026DD66
	public Vector3 highTargetPosition
	{
		get
		{
			return this._highTargetPosition;
		}
		set
		{
			if (this._highTargetPosition != value)
			{
				this._highTargetPosition = value;
				this.SetTargetPositionFromPercent();
				this.Adjust();
			}
		}
	}

	// Token: 0x17000F2C RID: 3884
	// (get) Token: 0x060067C7 RID: 26567 RVA: 0x0026F98C File Offset: 0x0026DD8C
	// (set) Token: 0x060067C8 RID: 26568 RVA: 0x0026F994 File Offset: 0x0026DD94
	public Vector3 currentTargetPosition
	{
		get
		{
			return this._currentTargetPosition;
		}
		set
		{
			if (this._currentTargetPosition != value)
			{
				this._currentTargetPosition = value;
				this.Adjust();
			}
		}
	}

	// Token: 0x17000F2D RID: 3885
	// (get) Token: 0x060067C9 RID: 26569 RVA: 0x0026F9B4 File Offset: 0x0026DDB4
	// (set) Token: 0x060067CA RID: 26570 RVA: 0x0026F9C4 File Offset: 0x0026DDC4
	public float currentTargetPositionX
	{
		get
		{
			return this._currentTargetPosition.x;
		}
		set
		{
			if (this._currentTargetPosition.x != value)
			{
				Vector3 currentTargetPosition = this._currentTargetPosition;
				currentTargetPosition.x = value;
				this._currentTargetPosition = currentTargetPosition;
				this.Adjust();
			}
		}
	}

	// Token: 0x17000F2E RID: 3886
	// (get) Token: 0x060067CB RID: 26571 RVA: 0x0026F9FE File Offset: 0x0026DDFE
	// (set) Token: 0x060067CC RID: 26572 RVA: 0x0026FA0C File Offset: 0x0026DE0C
	public float currentTargetPositionNegativeX
	{
		get
		{
			return -this._currentTargetPosition.x;
		}
		set
		{
			if (this._currentTargetPosition.x != -value)
			{
				Vector3 currentTargetPosition = this._currentTargetPosition;
				currentTargetPosition.x = -value;
				this._currentTargetPosition = currentTargetPosition;
				this.Adjust();
			}
		}
	}

	// Token: 0x17000F2F RID: 3887
	// (get) Token: 0x060067CD RID: 26573 RVA: 0x0026FA48 File Offset: 0x0026DE48
	// (set) Token: 0x060067CE RID: 26574 RVA: 0x0026FA58 File Offset: 0x0026DE58
	public float currentTargetPositionY
	{
		get
		{
			return this._currentTargetPosition.y;
		}
		set
		{
			if (this._currentTargetPosition.y != value)
			{
				Vector3 currentTargetPosition = this._currentTargetPosition;
				currentTargetPosition.y = value;
				this._currentTargetPosition = currentTargetPosition;
				this.Adjust();
			}
		}
	}

	// Token: 0x17000F30 RID: 3888
	// (get) Token: 0x060067CF RID: 26575 RVA: 0x0026FA92 File Offset: 0x0026DE92
	// (set) Token: 0x060067D0 RID: 26576 RVA: 0x0026FAA0 File Offset: 0x0026DEA0
	public float currentTargetPositionNegativeY
	{
		get
		{
			return -this._currentTargetPosition.y;
		}
		set
		{
			if (this._currentTargetPosition.y != -value)
			{
				Vector3 currentTargetPosition = this._currentTargetPosition;
				currentTargetPosition.y = -value;
				this._currentTargetPosition = currentTargetPosition;
				this.Adjust();
			}
		}
	}

	// Token: 0x17000F31 RID: 3889
	// (get) Token: 0x060067D1 RID: 26577 RVA: 0x0026FADC File Offset: 0x0026DEDC
	// (set) Token: 0x060067D2 RID: 26578 RVA: 0x0026FAEC File Offset: 0x0026DEEC
	public float currentTargetPositionZ
	{
		get
		{
			return this._currentTargetPosition.z;
		}
		set
		{
			if (this._currentTargetPosition.z != value)
			{
				Vector3 currentTargetPosition = this._currentTargetPosition;
				currentTargetPosition.z = value;
				this._currentTargetPosition = currentTargetPosition;
				this.Adjust();
			}
		}
	}

	// Token: 0x17000F32 RID: 3890
	// (get) Token: 0x060067D3 RID: 26579 RVA: 0x0026FB26 File Offset: 0x0026DF26
	// (set) Token: 0x060067D4 RID: 26580 RVA: 0x0026FB34 File Offset: 0x0026DF34
	public float currentTargetPositionNegativeZ
	{
		get
		{
			return -this._currentTargetPosition.z;
		}
		set
		{
			if (this._currentTargetPosition.z != -value)
			{
				Vector3 currentTargetPosition = this._currentTargetPosition;
				currentTargetPosition.z = -value;
				this._currentTargetPosition = currentTargetPosition;
				this.Adjust();
			}
		}
	}

	// Token: 0x17000F33 RID: 3891
	// (get) Token: 0x060067D5 RID: 26581 RVA: 0x0026FB70 File Offset: 0x0026DF70
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

	// Token: 0x060067D6 RID: 26582 RVA: 0x0026FB98 File Offset: 0x0026DF98
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
				this.CJ.targetPosition = this._currentTargetPosition;
			}
		}
	}

	// Token: 0x040058C1 RID: 22721
	public bool on = true;

	// Token: 0x040058C2 RID: 22722
	[SerializeField]
	private float _percent;

	// Token: 0x040058C3 RID: 22723
	[SerializeField]
	private Vector3 _lowTargetPosition;

	// Token: 0x040058C4 RID: 22724
	[SerializeField]
	private Vector3 _zeroTargetPosition;

	// Token: 0x040058C5 RID: 22725
	[SerializeField]
	private Vector3 _highTargetPosition;

	// Token: 0x040058C6 RID: 22726
	[SerializeField]
	private Vector3 _currentTargetPosition;

	// Token: 0x040058C7 RID: 22727
	private ConfigurableJoint CJ;
}
