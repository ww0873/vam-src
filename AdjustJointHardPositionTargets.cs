using System;
using UnityEngine;

// Token: 0x02000D3E RID: 3390
public class AdjustJointHardPositionTargets : MonoBehaviour
{
	// Token: 0x060067A8 RID: 26536 RVA: 0x0026F4FE File Offset: 0x0026D8FE
	public AdjustJointHardPositionTargets()
	{
	}

	// Token: 0x17000F1F RID: 3871
	// (get) Token: 0x060067A9 RID: 26537 RVA: 0x0026F50D File Offset: 0x0026D90D
	// (set) Token: 0x060067AA RID: 26538 RVA: 0x0026F515 File Offset: 0x0026D915
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

	// Token: 0x060067AB RID: 26539 RVA: 0x0026F530 File Offset: 0x0026D930
	public void ResyncTargets()
	{
		this.jphls = base.GetComponentsInChildren<JointPositionHardLimit>(true);
		this.Adjust();
	}

	// Token: 0x17000F20 RID: 3872
	// (get) Token: 0x060067AC RID: 26540 RVA: 0x0026F545 File Offset: 0x0026D945
	public JointPositionHardLimit[] controlledJointPositionHardLimits
	{
		get
		{
			if (this.jphls == null)
			{
				this.jphls = base.GetComponentsInChildren<JointPositionHardLimit>(true);
			}
			return this.jphls;
		}
	}

	// Token: 0x060067AD RID: 26541 RVA: 0x0026F568 File Offset: 0x0026D968
	protected virtual void Adjust()
	{
		if (this.on)
		{
			if (this.jphls == null)
			{
				this.jphls = base.GetComponentsInChildren<JointPositionHardLimit>(true);
			}
			if (this.jphls != null)
			{
				foreach (JointPositionHardLimit jointPositionHardLimit in this.jphls)
				{
					jointPositionHardLimit.percent = this.percent;
				}
			}
		}
	}

	// Token: 0x040058B6 RID: 22710
	public bool on = true;

	// Token: 0x040058B7 RID: 22711
	[SerializeField]
	protected float _percent;

	// Token: 0x040058B8 RID: 22712
	protected JointPositionHardLimit[] jphls;
}
