using System;
using UnityEngine;

// Token: 0x02000D45 RID: 3397
public class AdjustJointSprings : MonoBehaviour
{
	// Token: 0x0600684E RID: 26702 RVA: 0x00271DAC File Offset: 0x002701AC
	public AdjustJointSprings()
	{
	}

	// Token: 0x17000F57 RID: 3927
	// (get) Token: 0x0600684F RID: 26703 RVA: 0x00271DB4 File Offset: 0x002701B4
	// (set) Token: 0x06006850 RID: 26704 RVA: 0x00271DBC File Offset: 0x002701BC
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
				this.Adjust();
			}
		}
	}

	// Token: 0x17000F58 RID: 3928
	// (get) Token: 0x06006851 RID: 26705 RVA: 0x00271DD7 File Offset: 0x002701D7
	// (set) Token: 0x06006852 RID: 26706 RVA: 0x00271DDF File Offset: 0x002701DF
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

	// Token: 0x06006853 RID: 26707 RVA: 0x00271DF4 File Offset: 0x002701F4
	public void SetDefaultPercent()
	{
		this.percent = this._defaultPercent;
	}

	// Token: 0x06006854 RID: 26708 RVA: 0x00271E02 File Offset: 0x00270202
	public void ResyncSprings()
	{
		this.ajss = base.GetComponentsInChildren<AdjustJointSpring>(true);
		this.Adjust();
	}

	// Token: 0x17000F59 RID: 3929
	// (get) Token: 0x06006855 RID: 26709 RVA: 0x00271E17 File Offset: 0x00270217
	public AdjustJointSpring[] controlledAdjustJointSprings
	{
		get
		{
			if (this.ajss == null)
			{
				this.ajss = base.GetComponentsInChildren<AdjustJointSpring>(true);
			}
			return this.ajss;
		}
	}

	// Token: 0x06006856 RID: 26710 RVA: 0x00271E38 File Offset: 0x00270238
	private void Adjust()
	{
		if (this.on)
		{
			if (this.ajss == null)
			{
				this.ajss = base.GetComponentsInChildren<AdjustJointSpring>(true);
			}
			if (this.ajss != null)
			{
				foreach (AdjustJointSpring adjustJointSpring in this.ajss)
				{
					adjustJointSpring.percent = this.percent;
				}
			}
		}
	}

	// Token: 0x04005932 RID: 22834
	public bool on;

	// Token: 0x04005933 RID: 22835
	[SerializeField]
	private float _percent;

	// Token: 0x04005934 RID: 22836
	[SerializeField]
	private float _defaultPercent;

	// Token: 0x04005935 RID: 22837
	private AdjustJointSpring[] ajss;
}
