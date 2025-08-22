using System;
using UnityEngine;

// Token: 0x02000D4A RID: 3402
public class AdjustJointTargetAndCOG : MonoBehaviour
{
	// Token: 0x06006880 RID: 26752 RVA: 0x002724CE File Offset: 0x002708CE
	public AdjustJointTargetAndCOG()
	{
	}

	// Token: 0x17000F69 RID: 3945
	// (get) Token: 0x06006881 RID: 26753 RVA: 0x002724DD File Offset: 0x002708DD
	// (set) Token: 0x06006882 RID: 26754 RVA: 0x002724E5 File Offset: 0x002708E5
	public float percent
	{
		get
		{
			return this._percent;
		}
		set
		{
			this._percent = value;
		}
	}

	// Token: 0x06006883 RID: 26755 RVA: 0x002724EE File Offset: 0x002708EE
	public void SetPercentFromUISlider()
	{
	}

	// Token: 0x06006884 RID: 26756 RVA: 0x002724F0 File Offset: 0x002708F0
	private void Start()
	{
		this.CJ = base.GetComponent<ConfigurableJoint>();
		this.RB = base.GetComponent<Rigidbody>();
	}

	// Token: 0x06006885 RID: 26757 RVA: 0x0027250C File Offset: 0x0027090C
	private void Update()
	{
		if (this.on)
		{
			this.currentCOG = this.RB.centerOfMass;
			if (this.CJ != null)
			{
				Quaternion targetRotation = Quaternion.Lerp(this.lowTargetRotation, this.highTargetRotation, this._percent);
				this.CJ.targetRotation = targetRotation;
			}
			if (this.RB != null)
			{
				Vector3 vector = Vector3.Lerp(this.lowCOG, this.highCOG, this._percent);
				if (this.RB.centerOfMass != vector)
				{
					this.RB.centerOfMass = vector;
				}
			}
		}
	}

	// Token: 0x04005956 RID: 22870
	public bool on = true;

	// Token: 0x04005957 RID: 22871
	[SerializeField]
	private float _percent;

	// Token: 0x04005958 RID: 22872
	public Vector3 currentCOG;

	// Token: 0x04005959 RID: 22873
	public Vector3 lowCOG;

	// Token: 0x0400595A RID: 22874
	public Vector3 highCOG;

	// Token: 0x0400595B RID: 22875
	public Quaternion lowTargetRotation;

	// Token: 0x0400595C RID: 22876
	public Quaternion highTargetRotation;

	// Token: 0x0400595D RID: 22877
	private ConfigurableJoint CJ;

	// Token: 0x0400595E RID: 22878
	private Rigidbody RB;
}
