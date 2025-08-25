using System;
using UnityEngine;

// Token: 0x02000D49 RID: 3401
public class AdjustJointTarget : MonoBehaviour
{
	// Token: 0x0600685F RID: 26719 RVA: 0x00271FC8 File Offset: 0x002703C8
	public AdjustJointTarget()
	{
	}

	// Token: 0x06006860 RID: 26720 RVA: 0x00271FD8 File Offset: 0x002703D8
	private void SetTargetRotationFromPercent()
	{
		if (this._xPercent < 0f)
		{
			this._currentTargetRotation.x = Mathf.LerpUnclamped(this._zeroTargetRotation.x, this._lowTargetRotation.x, -this._xPercent);
		}
		else
		{
			this._currentTargetRotation.x = Mathf.LerpUnclamped(this._zeroTargetRotation.x, this._highTargetRotation.x, this._xPercent);
		}
		if (this._yPercent < 0f)
		{
			this._currentTargetRotation.y = Mathf.LerpUnclamped(this._zeroTargetRotation.y, this._lowTargetRotation.y, -this._yPercent);
		}
		else
		{
			this._currentTargetRotation.y = Mathf.LerpUnclamped(this._zeroTargetRotation.y, this._highTargetRotation.y, this._yPercent);
		}
		if (this._zPercent < 0f)
		{
			this._currentTargetRotation.z = Mathf.LerpUnclamped(this._zeroTargetRotation.z, this._lowTargetRotation.z, -this._zPercent);
		}
		else
		{
			this._currentTargetRotation.z = Mathf.LerpUnclamped(this._zeroTargetRotation.z, this._highTargetRotation.z, this._zPercent);
		}
	}

	// Token: 0x06006861 RID: 26721 RVA: 0x0027212F File Offset: 0x0027052F
	public void Sync()
	{
		this.SetTargetRotationFromPercent();
		this.Adjust();
	}

	// Token: 0x17000F5A RID: 3930
	// (get) Token: 0x06006862 RID: 26722 RVA: 0x0027213D File Offset: 0x0027053D
	// (set) Token: 0x06006863 RID: 26723 RVA: 0x00272145 File Offset: 0x00270545
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
				this.Sync();
			}
		}
	}

	// Token: 0x17000F5B RID: 3931
	// (get) Token: 0x06006864 RID: 26724 RVA: 0x00272160 File Offset: 0x00270560
	// (set) Token: 0x06006865 RID: 26725 RVA: 0x00272168 File Offset: 0x00270568
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
				this.Sync();
			}
		}
	}

	// Token: 0x17000F5C RID: 3932
	// (get) Token: 0x06006866 RID: 26726 RVA: 0x00272183 File Offset: 0x00270583
	// (set) Token: 0x06006867 RID: 26727 RVA: 0x0027218B File Offset: 0x0027058B
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
				this.Sync();
			}
		}
	}

	// Token: 0x17000F5D RID: 3933
	// (get) Token: 0x06006868 RID: 26728 RVA: 0x002721A6 File Offset: 0x002705A6
	// (set) Token: 0x06006869 RID: 26729 RVA: 0x002721AE File Offset: 0x002705AE
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
				this._xPercent = this._percent;
				this._yPercent = this._percent;
				this._zPercent = this._percent;
				this.Sync();
			}
		}
	}

	// Token: 0x17000F5E RID: 3934
	// (get) Token: 0x0600686A RID: 26730 RVA: 0x002721ED File Offset: 0x002705ED
	// (set) Token: 0x0600686B RID: 26731 RVA: 0x002721F5 File Offset: 0x002705F5
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
				this.Sync();
			}
		}
	}

	// Token: 0x17000F5F RID: 3935
	// (get) Token: 0x0600686C RID: 26732 RVA: 0x00272215 File Offset: 0x00270615
	// (set) Token: 0x0600686D RID: 26733 RVA: 0x0027221D File Offset: 0x0027061D
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
				this.Sync();
			}
		}
	}

	// Token: 0x17000F60 RID: 3936
	// (get) Token: 0x0600686E RID: 26734 RVA: 0x0027223D File Offset: 0x0027063D
	// (set) Token: 0x0600686F RID: 26735 RVA: 0x00272245 File Offset: 0x00270645
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
				this.Sync();
			}
		}
	}

	// Token: 0x17000F61 RID: 3937
	// (get) Token: 0x06006870 RID: 26736 RVA: 0x00272265 File Offset: 0x00270665
	// (set) Token: 0x06006871 RID: 26737 RVA: 0x0027226D File Offset: 0x0027066D
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

	// Token: 0x17000F62 RID: 3938
	// (get) Token: 0x06006872 RID: 26738 RVA: 0x0027228D File Offset: 0x0027068D
	// (set) Token: 0x06006873 RID: 26739 RVA: 0x0027229C File Offset: 0x0027069C
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

	// Token: 0x17000F63 RID: 3939
	// (get) Token: 0x06006874 RID: 26740 RVA: 0x002722D6 File Offset: 0x002706D6
	// (set) Token: 0x06006875 RID: 26741 RVA: 0x002722E4 File Offset: 0x002706E4
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

	// Token: 0x17000F64 RID: 3940
	// (get) Token: 0x06006876 RID: 26742 RVA: 0x00272320 File Offset: 0x00270720
	// (set) Token: 0x06006877 RID: 26743 RVA: 0x00272330 File Offset: 0x00270730
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

	// Token: 0x17000F65 RID: 3941
	// (get) Token: 0x06006878 RID: 26744 RVA: 0x0027236A File Offset: 0x0027076A
	// (set) Token: 0x06006879 RID: 26745 RVA: 0x00272378 File Offset: 0x00270778
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

	// Token: 0x17000F66 RID: 3942
	// (get) Token: 0x0600687A RID: 26746 RVA: 0x002723B4 File Offset: 0x002707B4
	// (set) Token: 0x0600687B RID: 26747 RVA: 0x002723C4 File Offset: 0x002707C4
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

	// Token: 0x17000F67 RID: 3943
	// (get) Token: 0x0600687C RID: 26748 RVA: 0x002723FE File Offset: 0x002707FE
	// (set) Token: 0x0600687D RID: 26749 RVA: 0x0027240C File Offset: 0x0027080C
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

	// Token: 0x17000F68 RID: 3944
	// (get) Token: 0x0600687E RID: 26750 RVA: 0x00272448 File Offset: 0x00270848
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

	// Token: 0x0600687F RID: 26751 RVA: 0x00272470 File Offset: 0x00270870
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
				Quaternion targetRotation = Quaternion.Euler(this._currentTargetRotation);
				this.CJ.targetRotation = targetRotation;
			}
		}
	}

	// Token: 0x0400594C RID: 22860
	public bool on = true;

	// Token: 0x0400594D RID: 22861
	[SerializeField]
	private float _xPercent;

	// Token: 0x0400594E RID: 22862
	[SerializeField]
	private float _yPercent;

	// Token: 0x0400594F RID: 22863
	[SerializeField]
	private float _zPercent;

	// Token: 0x04005950 RID: 22864
	[SerializeField]
	private float _percent;

	// Token: 0x04005951 RID: 22865
	[SerializeField]
	private Vector3 _lowTargetRotation;

	// Token: 0x04005952 RID: 22866
	[SerializeField]
	private Vector3 _zeroTargetRotation;

	// Token: 0x04005953 RID: 22867
	[SerializeField]
	private Vector3 _highTargetRotation;

	// Token: 0x04005954 RID: 22868
	[SerializeField]
	private Vector3 _currentTargetRotation;

	// Token: 0x04005955 RID: 22869
	private ConfigurableJoint CJ;
}
