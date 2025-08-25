using System;
using UnityEngine;

// Token: 0x02000B35 RID: 2869
public class SetDAZMorphFromLocalDistance : SetDAZMorph
{
	// Token: 0x06004E73 RID: 20083 RVA: 0x001B9EFB File Offset: 0x001B82FB
	public SetDAZMorphFromLocalDistance()
	{
	}

	// Token: 0x17000B2C RID: 2860
	// (get) Token: 0x06004E74 RID: 20084 RVA: 0x001B9F27 File Offset: 0x001B8327
	// (set) Token: 0x06004E75 RID: 20085 RVA: 0x001B9F2F File Offset: 0x001B832F
	public float multiplier
	{
		get
		{
			return this._multiplier;
		}
		set
		{
			this._multiplier = value;
		}
	}

	// Token: 0x06004E76 RID: 20086 RVA: 0x001B9F38 File Offset: 0x001B8338
	public void DoUpdate()
	{
		if (this.isOn && this.movingTransform != null && this.morph1 != null)
		{
			float f;
			switch (this.distanceAxis)
			{
			case SetDAZMorphFromLocalDistance.axis.X:
				f = this.movingTransform.localPosition.x - this.startingLocalPosition.x;
				break;
			case SetDAZMorphFromLocalDistance.axis.Y:
				f = this.movingTransform.localPosition.y - this.startingLocalPosition.y;
				break;
			case SetDAZMorphFromLocalDistance.axis.Z:
				f = this.movingTransform.localPosition.z - this.startingLocalPosition.z;
				break;
			default:
				f = 0f;
				break;
			}
			if (float.IsNaN(f))
			{
				Debug.LogError("Detected NaN value during distance calculation for SetDAZMorphFromLocalDistance " + base.name);
			}
			else
			{
				this.currentDistance = f;
				float num = (this.currentDistance * this._multiplier - this.distanceLow) / (this.distanceHigh - this.distanceLow);
				float num2 = num;
				if (this.clampMorphValue)
				{
					num2 = Mathf.Clamp(num, 0f, 1f);
				}
				this.currentMorph1Value = this.morph1Low + (this.morph1High - this.morph1Low) * num2;
				this.morph1.SetValueThreadSafe(this.currentMorph1Value);
			}
		}
	}

	// Token: 0x06004E77 RID: 20087 RVA: 0x001BA0A6 File Offset: 0x001B84A6
	protected void SyncMorphJSON()
	{
		if (this.morph1 != null)
		{
			this.morph1.SyncJSON();
		}
	}

	// Token: 0x06004E78 RID: 20088 RVA: 0x001BA0BE File Offset: 0x001B84BE
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

	// Token: 0x06004E79 RID: 20089 RVA: 0x001BA0E2 File Offset: 0x001B84E2
	private void Start()
	{
		this.startingLocalPosition = this.movingTransform.localPosition;
	}

	// Token: 0x04003E33 RID: 15923
	public Transform movingTransform;

	// Token: 0x04003E34 RID: 15924
	public bool updateEnabled = true;

	// Token: 0x04003E35 RID: 15925
	public SetDAZMorphFromLocalDistance.axis distanceAxis;

	// Token: 0x04003E36 RID: 15926
	public float distanceLow;

	// Token: 0x04003E37 RID: 15927
	public float distanceHigh = 0.1f;

	// Token: 0x04003E38 RID: 15928
	public float currentDistance;

	// Token: 0x04003E39 RID: 15929
	private Vector3 startingLocalPosition;

	// Token: 0x04003E3A RID: 15930
	public float _multiplier = 1f;

	// Token: 0x04003E3B RID: 15931
	public bool clampMorphValue = true;

	// Token: 0x02000B36 RID: 2870
	public enum axis
	{
		// Token: 0x04003E3D RID: 15933
		X,
		// Token: 0x04003E3E RID: 15934
		Y,
		// Token: 0x04003E3F RID: 15935
		Z
	}
}
