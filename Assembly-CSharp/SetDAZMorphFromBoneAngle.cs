using System;
using UnityEngine;

// Token: 0x02000B31 RID: 2865
public class SetDAZMorphFromBoneAngle : SetDAZMorph
{
	// Token: 0x06004E66 RID: 20070 RVA: 0x001B9B27 File Offset: 0x001B7F27
	public SetDAZMorphFromBoneAngle()
	{
	}

	// Token: 0x17000B2A RID: 2858
	// (get) Token: 0x06004E67 RID: 20071 RVA: 0x001B9B53 File Offset: 0x001B7F53
	// (set) Token: 0x06004E68 RID: 20072 RVA: 0x001B9B5B File Offset: 0x001B7F5B
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

	// Token: 0x06004E69 RID: 20073 RVA: 0x001B9B64 File Offset: 0x001B7F64
	public void DoUpdate()
	{
		if (this.isOn && this.dazBone != null)
		{
			Vector3 anglesDegrees = this.dazBone.GetAnglesDegrees();
			SetDAZMorphFromBoneAngle.axis axis = this.angleAxis;
			if (axis != SetDAZMorphFromBoneAngle.axis.X)
			{
				if (axis != SetDAZMorphFromBoneAngle.axis.Y)
				{
					if (axis == SetDAZMorphFromBoneAngle.axis.Z)
					{
						this.currentAxisAngle = anglesDegrees.z;
					}
				}
				else
				{
					this.currentAxisAngle = anglesDegrees.y;
				}
			}
			else
			{
				this.currentAxisAngle = anglesDegrees.x;
			}
			float num = (this.currentAxisAngle * this._multiplier - this.angleLow) / (this.angleHigh - this.angleLow);
			float num2 = num;
			if (this.clampMorphValue)
			{
				num2 = Mathf.Clamp(num, 0f, 1f);
			}
			if (float.IsNaN(num2))
			{
				Debug.LogError("Detected NaN in SetDAZMorphFromBoneAngle " + base.name);
			}
			else if (this.morph1 != null)
			{
				this.currentMorph1Value = this.morph1Low + (this.morph1High - this.morph1Low) * num2;
				this.morph1.SetValueThreadSafe(this.currentMorph1Value);
			}
		}
	}

	// Token: 0x06004E6A RID: 20074 RVA: 0x001B9C8E File Offset: 0x001B808E
	protected void SyncMorphJSON()
	{
		if (this.morph1 != null)
		{
			this.morph1.SyncJSON();
		}
	}

	// Token: 0x06004E6B RID: 20075 RVA: 0x001B9CA6 File Offset: 0x001B80A6
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

	// Token: 0x04003E1D RID: 15901
	public DAZBone dazBone;

	// Token: 0x04003E1E RID: 15902
	public bool updateEnabled = true;

	// Token: 0x04003E1F RID: 15903
	public float angleLow;

	// Token: 0x04003E20 RID: 15904
	public float angleHigh = 20f;

	// Token: 0x04003E21 RID: 15905
	public SetDAZMorphFromBoneAngle.axis angleAxis;

	// Token: 0x04003E22 RID: 15906
	public float currentAxisAngle;

	// Token: 0x04003E23 RID: 15907
	public float _multiplier = 1f;

	// Token: 0x04003E24 RID: 15908
	public bool clampMorphValue = true;

	// Token: 0x02000B32 RID: 2866
	public enum axis
	{
		// Token: 0x04003E26 RID: 15910
		X,
		// Token: 0x04003E27 RID: 15911
		Y,
		// Token: 0x04003E28 RID: 15912
		Z
	}
}
