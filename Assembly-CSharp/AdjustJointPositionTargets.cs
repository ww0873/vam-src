using System;
using UnityEngine;

// Token: 0x02000D41 RID: 3393
public class AdjustJointPositionTargets : MonoBehaviour
{
	// Token: 0x060067D7 RID: 26583 RVA: 0x00141069 File Offset: 0x0013F469
	public AdjustJointPositionTargets()
	{
	}

	// Token: 0x17000F34 RID: 3892
	// (get) Token: 0x060067D8 RID: 26584 RVA: 0x00141078 File Offset: 0x0013F478
	// (set) Token: 0x060067D9 RID: 26585 RVA: 0x00141080 File Offset: 0x0013F480
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

	// Token: 0x060067DA RID: 26586 RVA: 0x0014109B File Offset: 0x0013F49B
	public void ResyncTargets()
	{
		this.ajpts = base.GetComponentsInChildren<AdjustJointPositionTarget>(true);
		this.Adjust();
	}

	// Token: 0x17000F35 RID: 3893
	// (get) Token: 0x060067DB RID: 26587 RVA: 0x001410B0 File Offset: 0x0013F4B0
	public AdjustJointPositionTarget[] controlledAdjustJointPositionTargets
	{
		get
		{
			if (this.ajpts == null)
			{
				this.ajpts = base.GetComponentsInChildren<AdjustJointPositionTarget>(true);
			}
			return this.ajpts;
		}
	}

	// Token: 0x060067DC RID: 26588 RVA: 0x001410D0 File Offset: 0x0013F4D0
	protected virtual void Adjust()
	{
		if (this.on)
		{
			if (this.ajpts == null)
			{
				this.ajpts = base.GetComponentsInChildren<AdjustJointPositionTarget>(true);
			}
			if (this.ajpts != null)
			{
				foreach (AdjustJointPositionTarget adjustJointPositionTarget in this.ajpts)
				{
					adjustJointPositionTarget.percent = this.percent;
				}
			}
		}
	}

	// Token: 0x040058C8 RID: 22728
	public bool on = true;

	// Token: 0x040058C9 RID: 22729
	[SerializeField]
	protected float _percent;

	// Token: 0x040058CA RID: 22730
	protected AdjustJointPositionTarget[] ajpts;
}
