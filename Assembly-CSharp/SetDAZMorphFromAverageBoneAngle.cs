using System;
using UnityEngine;

// Token: 0x02000B2F RID: 2863
public class SetDAZMorphFromAverageBoneAngle : SetDAZMorph
{
	// Token: 0x06004E60 RID: 20064 RVA: 0x001B9874 File Offset: 0x001B7C74
	public SetDAZMorphFromAverageBoneAngle()
	{
	}

	// Token: 0x17000B29 RID: 2857
	// (get) Token: 0x06004E61 RID: 20065 RVA: 0x001B98A0 File Offset: 0x001B7CA0
	// (set) Token: 0x06004E62 RID: 20066 RVA: 0x001B98A8 File Offset: 0x001B7CA8
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

	// Token: 0x06004E63 RID: 20067 RVA: 0x001B98B4 File Offset: 0x001B7CB4
	public void DoUpdate()
	{
		if (this.isOn && this.dazBone1 != null && this.dazBone2 != null)
		{
			Vector3 anglesDegrees = this.dazBone1.GetAnglesDegrees();
			Vector3 anglesDegrees2 = this.dazBone2.GetAnglesDegrees();
			switch (this.angleAxis1)
			{
			case SetDAZMorphFromAverageBoneAngle.axis.X:
				this.currentAxisAngle1 = anglesDegrees.x;
				break;
			case SetDAZMorphFromAverageBoneAngle.axis.Y:
				this.currentAxisAngle1 = anglesDegrees.y;
				break;
			case SetDAZMorphFromAverageBoneAngle.axis.Z:
				this.currentAxisAngle1 = anglesDegrees.z;
				break;
			case SetDAZMorphFromAverageBoneAngle.axis.NegX:
				this.currentAxisAngle1 = -anglesDegrees.x;
				break;
			case SetDAZMorphFromAverageBoneAngle.axis.NegY:
				this.currentAxisAngle1 = -anglesDegrees.y;
				break;
			case SetDAZMorphFromAverageBoneAngle.axis.NegZ:
				this.currentAxisAngle1 = -anglesDegrees.z;
				break;
			}
			switch (this.angleAxis2)
			{
			case SetDAZMorphFromAverageBoneAngle.axis.X:
				this.currentAxisAngle2 = anglesDegrees2.x;
				break;
			case SetDAZMorphFromAverageBoneAngle.axis.Y:
				this.currentAxisAngle2 = anglesDegrees2.y;
				break;
			case SetDAZMorphFromAverageBoneAngle.axis.Z:
				this.currentAxisAngle2 = anglesDegrees2.z;
				break;
			case SetDAZMorphFromAverageBoneAngle.axis.NegX:
				this.currentAxisAngle2 = -anglesDegrees2.x;
				break;
			case SetDAZMorphFromAverageBoneAngle.axis.NegY:
				this.currentAxisAngle2 = -anglesDegrees2.y;
				break;
			case SetDAZMorphFromAverageBoneAngle.axis.NegZ:
				this.currentAxisAngle2 = -anglesDegrees2.z;
				break;
			}
			float num = ((this.currentAxisAngle1 + this.currentAxisAngle2) * 0.5f * this._multiplier - this.angleLow) / (this.angleHigh - this.angleLow);
			float num2 = num;
			if (this.clampMorphValue)
			{
				num2 = Mathf.Clamp(num, 0f, 1f);
			}
			if (float.IsNaN(num2))
			{
				Debug.LogError("Detected NaN in SetDAZMorphFromAverageBoneAngle " + base.name);
			}
			else if (this.morph1 != null)
			{
				this.currentMorph1Value = this.morph1Low + (this.morph1High - this.morph1Low) * num2;
				this.morph1.SetValueThreadSafe(this.currentMorph1Value);
			}
		}
	}

	// Token: 0x06004E64 RID: 20068 RVA: 0x001B9AEB File Offset: 0x001B7EEB
	protected void SyncMorphJSON()
	{
		if (this.morph1 != null)
		{
			this.morph1.SyncJSON();
		}
	}

	// Token: 0x06004E65 RID: 20069 RVA: 0x001B9B03 File Offset: 0x001B7F03
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

	// Token: 0x04003E0B RID: 15883
	public DAZBone dazBone1;

	// Token: 0x04003E0C RID: 15884
	public DAZBone dazBone2;

	// Token: 0x04003E0D RID: 15885
	public bool updateEnabled = true;

	// Token: 0x04003E0E RID: 15886
	public float angleLow;

	// Token: 0x04003E0F RID: 15887
	public float angleHigh = 20f;

	// Token: 0x04003E10 RID: 15888
	public SetDAZMorphFromAverageBoneAngle.axis angleAxis1;

	// Token: 0x04003E11 RID: 15889
	public SetDAZMorphFromAverageBoneAngle.axis angleAxis2;

	// Token: 0x04003E12 RID: 15890
	public float currentAxisAngle1;

	// Token: 0x04003E13 RID: 15891
	public float currentAxisAngle2;

	// Token: 0x04003E14 RID: 15892
	public float _multiplier = 1f;

	// Token: 0x04003E15 RID: 15893
	public bool clampMorphValue = true;

	// Token: 0x02000B30 RID: 2864
	public enum axis
	{
		// Token: 0x04003E17 RID: 15895
		X,
		// Token: 0x04003E18 RID: 15896
		Y,
		// Token: 0x04003E19 RID: 15897
		Z,
		// Token: 0x04003E1A RID: 15898
		NegX,
		// Token: 0x04003E1B RID: 15899
		NegY,
		// Token: 0x04003E1C RID: 15900
		NegZ
	}
}
