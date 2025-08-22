using System;
using UnityEngine;

// Token: 0x02000D38 RID: 3384
[ExecuteInEditMode]
public class SetMorphFromHighestDistance : MonoBehaviour
{
	// Token: 0x06006776 RID: 26486 RVA: 0x0026E804 File Offset: 0x0026CC04
	public SetMorphFromHighestDistance()
	{
	}

	// Token: 0x06006777 RID: 26487 RVA: 0x0026E80C File Offset: 0x0026CC0C
	private void Start()
	{
	}

	// Token: 0x06006778 RID: 26488 RVA: 0x0026E810 File Offset: 0x0026CC10
	private void setWeight()
	{
		if (this.t1 && this.t2 && this.refTransform && this.skin)
		{
			this.currentDistance1 = Vector3.Magnitude(this.t1.position - this.refTransform.position);
			this.currentDistance2 = Vector3.Magnitude(this.t2.position - this.refTransform.position);
			float num;
			if (this.currentDistance1 > this.currentDistance2)
			{
				num = this.currentDistance1;
			}
			else
			{
				num = this.currentDistance2;
			}
			float num2 = (num - this.distanceLow) / (this.distanceHigh - this.distanceLow);
			int blendShapeIndex = this.skin.sharedMesh.GetBlendShapeIndex(this.morphName);
			if (blendShapeIndex != -1)
			{
				this.currentWeight = this.morphLow + (this.morphHigh - this.morphLow) * num2;
				if (this.morphHigh > this.morphLow)
				{
					this.currentWeight = Mathf.Clamp(this.currentWeight, this.morphLow, this.morphHigh);
				}
				else
				{
					this.currentWeight = Mathf.Clamp(this.currentWeight, this.morphHigh, this.morphLow);
				}
				this.skin.SetBlendShapeWeight(blendShapeIndex, this.currentWeight);
			}
		}
	}

	// Token: 0x06006779 RID: 26489 RVA: 0x0026E980 File Offset: 0x0026CD80
	private void Update()
	{
		this.setWeight();
	}

	// Token: 0x04005881 RID: 22657
	public SkinnedMeshRenderer skin;

	// Token: 0x04005882 RID: 22658
	public string morphName;

	// Token: 0x04005883 RID: 22659
	public Transform t1;

	// Token: 0x04005884 RID: 22660
	public Transform t2;

	// Token: 0x04005885 RID: 22661
	public Transform refTransform;

	// Token: 0x04005886 RID: 22662
	public float distanceLow;

	// Token: 0x04005887 RID: 22663
	public float distanceHigh;

	// Token: 0x04005888 RID: 22664
	public float morphLow;

	// Token: 0x04005889 RID: 22665
	public float morphHigh;

	// Token: 0x0400588A RID: 22666
	public float currentDistance1;

	// Token: 0x0400588B RID: 22667
	public float currentDistance2;

	// Token: 0x0400588C RID: 22668
	public float currentWeight;
}
