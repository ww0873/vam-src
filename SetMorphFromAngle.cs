using System;
using UnityEngine;

// Token: 0x02000D35 RID: 3381
[ExecuteInEditMode]
public class SetMorphFromAngle : MonoBehaviour
{
	// Token: 0x0600676A RID: 26474 RVA: 0x0026E440 File Offset: 0x0026C840
	public SetMorphFromAngle()
	{
	}

	// Token: 0x0600676B RID: 26475 RVA: 0x0026E448 File Offset: 0x0026C848
	private void Start()
	{
	}

	// Token: 0x0600676C RID: 26476 RVA: 0x0026E44C File Offset: 0x0026C84C
	private void setWeight()
	{
		if (this.t1 && this.t2 && this.skin)
		{
			this.currentAngle = Quaternion.Angle(this.t1.rotation, this.t2.rotation);
			float num = (this.currentAngle - this.angleLow) / (this.angleHigh - this.angleLow);
			int blendShapeIndex = this.skin.sharedMesh.GetBlendShapeIndex(this.morphName);
			if (blendShapeIndex != -1)
			{
				this.currentWeight = this.morphLow + (this.morphHigh - this.morphLow) * num;
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

	// Token: 0x0600676D RID: 26477 RVA: 0x0026E562 File Offset: 0x0026C962
	private void Update()
	{
		this.setWeight();
	}

	// Token: 0x0400585F RID: 22623
	public SkinnedMeshRenderer skin;

	// Token: 0x04005860 RID: 22624
	public string morphName;

	// Token: 0x04005861 RID: 22625
	public Transform t1;

	// Token: 0x04005862 RID: 22626
	public Transform t2;

	// Token: 0x04005863 RID: 22627
	public float angleLow;

	// Token: 0x04005864 RID: 22628
	public float angleHigh;

	// Token: 0x04005865 RID: 22629
	public float morphLow;

	// Token: 0x04005866 RID: 22630
	public float morphHigh;

	// Token: 0x04005867 RID: 22631
	public float currentAngle;

	// Token: 0x04005868 RID: 22632
	public float currentWeight;
}
