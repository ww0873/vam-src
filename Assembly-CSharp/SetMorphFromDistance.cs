using System;
using UnityEngine;

// Token: 0x02000D36 RID: 3382
[ExecuteInEditMode]
public class SetMorphFromDistance : MonoBehaviour
{
	// Token: 0x0600676E RID: 26478 RVA: 0x0026E56A File Offset: 0x0026C96A
	public SetMorphFromDistance()
	{
	}

	// Token: 0x0600676F RID: 26479 RVA: 0x0026E572 File Offset: 0x0026C972
	private void Start()
	{
		this.startLocalPosition = base.transform.localPosition;
	}

	// Token: 0x06006770 RID: 26480 RVA: 0x0026E588 File Offset: 0x0026C988
	private void setWeight()
	{
		if (this.skin)
		{
			this.currentDistance = Vector3.Magnitude(base.transform.localPosition - this.startLocalPosition);
			float num = (this.currentDistance - this.distanceLow) / (this.distanceHigh - this.distanceLow);
			int blendShapeIndex = this.skin.sharedMesh.GetBlendShapeIndex(this.morphName);
			if (blendShapeIndex != -1)
			{
				this.currentWeight = this.morphLow + (this.morphHigh - this.morphLow) * num;
				this.currentWeight = Mathf.Clamp(this.currentWeight, this.morphLow, this.morphHigh);
				this.skin.SetBlendShapeWeight(blendShapeIndex, this.currentWeight);
			}
		}
	}

	// Token: 0x06006771 RID: 26481 RVA: 0x0026E64B File Offset: 0x0026CA4B
	private void Update()
	{
		if (this.resetStart)
		{
			this.resetStart = false;
			this.Start();
		}
		this.setWeight();
	}

	// Token: 0x04005869 RID: 22633
	public SkinnedMeshRenderer skin;

	// Token: 0x0400586A RID: 22634
	public bool resetStart;

	// Token: 0x0400586B RID: 22635
	public string morphName;

	// Token: 0x0400586C RID: 22636
	public float distanceLow;

	// Token: 0x0400586D RID: 22637
	public float distanceHigh;

	// Token: 0x0400586E RID: 22638
	public float morphLow;

	// Token: 0x0400586F RID: 22639
	public float morphHigh;

	// Token: 0x04005870 RID: 22640
	public float currentDistance;

	// Token: 0x04005871 RID: 22641
	public float currentWeight;

	// Token: 0x04005872 RID: 22642
	private Vector3 startLocalPosition;
}
