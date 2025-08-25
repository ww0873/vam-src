using System;
using UnityEngine;

// Token: 0x02000D4C RID: 3404
public class AdjustJointTargets : MonoBehaviour
{
	// Token: 0x06006889 RID: 26761 RVA: 0x002725C1 File Offset: 0x002709C1
	public AdjustJointTargets()
	{
	}

	// Token: 0x17000F6A RID: 3946
	// (get) Token: 0x0600688A RID: 26762 RVA: 0x002725DB File Offset: 0x002709DB
	// (set) Token: 0x0600688B RID: 26763 RVA: 0x002725E3 File Offset: 0x002709E3
	public bool on
	{
		get
		{
			return this._on;
		}
		set
		{
			if (this._on != value)
			{
				this._on = value;
				this.ResyncTargets();
			}
		}
	}

	// Token: 0x17000F6B RID: 3947
	// (get) Token: 0x0600688C RID: 26764 RVA: 0x002725FE File Offset: 0x002709FE
	// (set) Token: 0x0600688D RID: 26765 RVA: 0x00272608 File Offset: 0x00270A08
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
				if (this.on)
				{
					if (this.ajts == null)
					{
						this.ajts = base.GetComponentsInChildren<AdjustJointTarget>(true);
					}
					if (this.ajts != null)
					{
						foreach (AdjustJointTarget adjustJointTarget in this.ajts)
						{
							adjustJointTarget.percent = this._percent;
						}
					}
				}
			}
		}
	}

	// Token: 0x17000F6C RID: 3948
	// (get) Token: 0x0600688E RID: 26766 RVA: 0x00272696 File Offset: 0x00270A96
	// (set) Token: 0x0600688F RID: 26767 RVA: 0x002726A0 File Offset: 0x00270AA0
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
				if (this.on)
				{
					if (this.ajts == null)
					{
						this.ajts = base.GetComponentsInChildren<AdjustJointTarget>(true);
					}
					if (this.ajts != null)
					{
						foreach (AdjustJointTarget adjustJointTarget in this.ajts)
						{
							adjustJointTarget.xPercent = this._xPercent;
						}
					}
				}
			}
		}
	}

	// Token: 0x17000F6D RID: 3949
	// (get) Token: 0x06006890 RID: 26768 RVA: 0x00272719 File Offset: 0x00270B19
	// (set) Token: 0x06006891 RID: 26769 RVA: 0x00272724 File Offset: 0x00270B24
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
				if (this.on)
				{
					if (this.ajts == null)
					{
						this.ajts = base.GetComponentsInChildren<AdjustJointTarget>(true);
					}
					if (this.ajts != null)
					{
						foreach (AdjustJointTarget adjustJointTarget in this.ajts)
						{
							adjustJointTarget.yPercent = this._yPercent;
						}
					}
				}
			}
		}
	}

	// Token: 0x17000F6E RID: 3950
	// (get) Token: 0x06006892 RID: 26770 RVA: 0x0027279D File Offset: 0x00270B9D
	// (set) Token: 0x06006893 RID: 26771 RVA: 0x002727A8 File Offset: 0x00270BA8
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
				if (this.on)
				{
					if (this.ajts == null)
					{
						this.ajts = base.GetComponentsInChildren<AdjustJointTarget>(true);
					}
					if (this.ajts != null)
					{
						foreach (AdjustJointTarget adjustJointTarget in this.ajts)
						{
							adjustJointTarget.zPercent = this._zPercent;
						}
					}
				}
			}
		}
	}

	// Token: 0x06006894 RID: 26772 RVA: 0x00272824 File Offset: 0x00270C24
	public void ResyncTargets()
	{
		this.ajts = base.GetComponentsInChildren<AdjustJointTarget>(true);
		if (this.ajts != null && this.on)
		{
			foreach (AdjustJointTarget adjustJointTarget in this.ajts)
			{
				adjustJointTarget.percent = this._percent;
				adjustJointTarget.xPercent = this._xPercent;
				adjustJointTarget.yPercent = this._yPercent;
				adjustJointTarget.zPercent = this._zPercent;
			}
		}
	}

	// Token: 0x17000F6F RID: 3951
	// (get) Token: 0x06006895 RID: 26773 RVA: 0x002728A3 File Offset: 0x00270CA3
	public AdjustJointTarget[] controlledAdjustJointTargets
	{
		get
		{
			if (this.ajts == null)
			{
				this.ajts = base.GetComponentsInChildren<AdjustJointTarget>(true);
			}
			return this.ajts;
		}
	}

	// Token: 0x06006896 RID: 26774 RVA: 0x002728C4 File Offset: 0x00270CC4
	private void Update()
	{
		if (this.driveFromMotionTrigger != AdjustJointTargets.MotionTriggerSide.None && SuperController.singleton != null)
		{
			AdjustJointTargets.MotionTriggerSide motionTriggerSide = this.driveFromMotionTrigger;
			if (motionTriggerSide != AdjustJointTargets.MotionTriggerSide.Both)
			{
				if (motionTriggerSide != AdjustJointTargets.MotionTriggerSide.Left)
				{
					if (motionTriggerSide == AdjustJointTargets.MotionTriggerSide.Right)
					{
						this.percent = SuperController.singleton.GetRightGrabVal() * this.motionTriggerScale;
					}
				}
				else
				{
					this.percent = SuperController.singleton.GetLeftGrabVal() * this.motionTriggerScale;
				}
			}
			else
			{
				float leftGrabVal = SuperController.singleton.GetLeftGrabVal();
				float rightGrabVal = SuperController.singleton.GetRightGrabVal();
				this.percent = Mathf.Max(leftGrabVal, rightGrabVal) * this.motionTriggerScale;
			}
		}
	}

	// Token: 0x0400595F RID: 22879
	[SerializeField]
	protected bool _on = true;

	// Token: 0x04005960 RID: 22880
	public AdjustJointTargets.MotionTriggerSide driveFromMotionTrigger;

	// Token: 0x04005961 RID: 22881
	public float motionTriggerScale = 1f;

	// Token: 0x04005962 RID: 22882
	[SerializeField]
	private float _percent;

	// Token: 0x04005963 RID: 22883
	[SerializeField]
	private float _xPercent;

	// Token: 0x04005964 RID: 22884
	[SerializeField]
	private float _yPercent;

	// Token: 0x04005965 RID: 22885
	[SerializeField]
	private float _zPercent;

	// Token: 0x04005966 RID: 22886
	private AdjustJointTarget[] ajts;

	// Token: 0x02000D4D RID: 3405
	public enum MotionTriggerSide
	{
		// Token: 0x04005968 RID: 22888
		None,
		// Token: 0x04005969 RID: 22889
		Left,
		// Token: 0x0400596A RID: 22890
		Right,
		// Token: 0x0400596B RID: 22891
		Both
	}
}
