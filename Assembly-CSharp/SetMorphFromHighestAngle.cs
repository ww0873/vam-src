using System;
using UnityEngine;

// Token: 0x02000D37 RID: 3383
[ExecuteInEditMode]
public class SetMorphFromHighestAngle : MonoBehaviour
{
	// Token: 0x06006772 RID: 26482 RVA: 0x0026E66B File Offset: 0x0026CA6B
	public SetMorphFromHighestAngle()
	{
	}

	// Token: 0x06006773 RID: 26483 RVA: 0x0026E673 File Offset: 0x0026CA73
	private void Start()
	{
		this.originalT1Rotation = this.t1.localRotation;
		this.originalT2Rotation = this.t2.localRotation;
	}

	// Token: 0x06006774 RID: 26484 RVA: 0x0026E698 File Offset: 0x0026CA98
	private void setWeight()
	{
		if (this.t1 && this.t2 && this.skin)
		{
			this.currentAngle1 = Quaternion.Angle(this.originalT1Rotation, this.t1.localRotation);
			this.currentAngle2 = Quaternion.Angle(this.originalT2Rotation, this.t2.localRotation);
			float num;
			if (this.currentAngle1 > this.currentAngle2)
			{
				num = this.currentAngle1;
			}
			else
			{
				num = this.currentAngle2;
			}
			float num2 = (num - this.angleLow) / (this.angleHigh - this.angleLow);
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

	// Token: 0x06006775 RID: 26485 RVA: 0x0026E7E4 File Offset: 0x0026CBE4
	private void Update()
	{
		if (this.resetStart)
		{
			this.resetStart = false;
			this.Start();
		}
		this.setWeight();
	}

	// Token: 0x04005873 RID: 22643
	public SkinnedMeshRenderer skin;

	// Token: 0x04005874 RID: 22644
	public bool resetStart;

	// Token: 0x04005875 RID: 22645
	public string morphName;

	// Token: 0x04005876 RID: 22646
	public Transform t1;

	// Token: 0x04005877 RID: 22647
	public Transform t2;

	// Token: 0x04005878 RID: 22648
	public float angleLow;

	// Token: 0x04005879 RID: 22649
	public float angleHigh;

	// Token: 0x0400587A RID: 22650
	public float morphLow;

	// Token: 0x0400587B RID: 22651
	public float morphHigh;

	// Token: 0x0400587C RID: 22652
	public float currentAngle1;

	// Token: 0x0400587D RID: 22653
	public float currentAngle2;

	// Token: 0x0400587E RID: 22654
	public float currentWeight;

	// Token: 0x0400587F RID: 22655
	private Quaternion originalT1Rotation;

	// Token: 0x04005880 RID: 22656
	private Quaternion originalT2Rotation;
}
