using System;
using UnityEngine;

// Token: 0x02000B38 RID: 2872
public class SetDAZMorphRandom : SetDAZMorph
{
	// Token: 0x06004E7C RID: 20092 RVA: 0x001BA0FF File Offset: 0x001B84FF
	public SetDAZMorphRandom()
	{
	}

	// Token: 0x06004E7D RID: 20093 RVA: 0x001BA134 File Offset: 0x001B8534
	private void Update()
	{
		if (this.countdown < 0f)
		{
			this.countdown = UnityEngine.Random.Range(this.frequency - this.frequencyRandomness, this.frequency + this.frequencyRandomness);
			this.targetValue = UnityEngine.Random.Range(0f, 1f);
		}
		else
		{
			this.countdown -= Time.deltaTime;
		}
		if (this.currentValue < this.targetValue)
		{
			this.currentValue = Mathf.Lerp(this.currentValue, this.targetValue, Time.deltaTime * this.lerpUpFactor);
		}
		else
		{
			this.currentValue = Mathf.Lerp(this.currentValue, this.targetValue, Time.deltaTime * this.lerpDownFactor);
		}
		if (this.morph1 != null)
		{
			this.currentMorph1Value = Mathf.Lerp(this.morph1Low, this.morph1High, this.currentValue);
			this.morph1.morphValue = this.currentMorph1Value;
		}
	}

	// Token: 0x04003E40 RID: 15936
	public float frequency = 2f;

	// Token: 0x04003E41 RID: 15937
	public float frequencyRandomness = 1f;

	// Token: 0x04003E42 RID: 15938
	public float lerpUpFactor = 1f;

	// Token: 0x04003E43 RID: 15939
	public float lerpDownFactor = 1f;

	// Token: 0x04003E44 RID: 15940
	public float currentValue;

	// Token: 0x04003E45 RID: 15941
	public float targetValue;

	// Token: 0x04003E46 RID: 15942
	private float countdown;
}
