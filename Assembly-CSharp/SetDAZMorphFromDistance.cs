using System;
using UnityEngine;

// Token: 0x02000B34 RID: 2868
public class SetDAZMorphFromDistance : SetDAZMorph
{
	// Token: 0x06004E6F RID: 20079 RVA: 0x001B9D60 File Offset: 0x001B8160
	public SetDAZMorphFromDistance()
	{
	}

	// Token: 0x06004E70 RID: 20080 RVA: 0x001B9D88 File Offset: 0x001B8188
	public void DoUpdate()
	{
		if (this.isOn && this.transform1 != null && this.transform2 != null && this.morph1 != null)
		{
			float f = (this.transform1.position - this.transform2.position).magnitude / base.transform.lossyScale.x;
			if (float.IsNaN(f))
			{
				Debug.LogError("Detected NaN value during distance calculation for SetDAZMorphFromDistance " + base.name);
			}
			else
			{
				this.currentDistance = f;
				float num = Mathf.Clamp(this.currentDistance, this.distanceLow, this.distanceHigh) - this.distanceLow;
				float t = num / (this.distanceHigh - this.distanceLow);
				float num2 = Mathf.Lerp(this.morph1Low, this.morph1High, t);
				if (this.lerpOverTime)
				{
					this.currentMorph1Value = Mathf.Lerp(this.currentMorph1Value, num2, this.lerpRate * Time.deltaTime);
				}
				else
				{
					this.currentMorph1Value = num2;
				}
				this.morph1.SetValueThreadSafe(this.currentMorph1Value);
			}
		}
	}

	// Token: 0x06004E71 RID: 20081 RVA: 0x001B9EBF File Offset: 0x001B82BF
	protected void SyncMorphJSON()
	{
		if (this.morph1 != null)
		{
			this.morph1.SyncJSON();
		}
	}

	// Token: 0x06004E72 RID: 20082 RVA: 0x001B9ED7 File Offset: 0x001B82D7
	private void Update()
	{
		if (this.updateEnabled)
		{
			this.DoUpdate();
			this.SyncMorphJSON();
		}
		else
		{
			this.SyncMorphJSON();
		}
	}

	// Token: 0x04003E2B RID: 15915
	public bool updateEnabled = true;

	// Token: 0x04003E2C RID: 15916
	public Transform transform1;

	// Token: 0x04003E2D RID: 15917
	public Transform transform2;

	// Token: 0x04003E2E RID: 15918
	public float distanceLow;

	// Token: 0x04003E2F RID: 15919
	public float distanceHigh = 0.1f;

	// Token: 0x04003E30 RID: 15920
	public float currentDistance;

	// Token: 0x04003E31 RID: 15921
	public bool lerpOverTime;

	// Token: 0x04003E32 RID: 15922
	public float lerpRate = 10f;
}
