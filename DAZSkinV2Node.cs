using System;

// Token: 0x02000B08 RID: 2824
[Serializable]
public class DAZSkinV2Node
{
	// Token: 0x06004CB4 RID: 19636 RVA: 0x001AE607 File Offset: 0x001ACA07
	public DAZSkinV2Node()
	{
	}

	// Token: 0x04003BB4 RID: 15284
	public string url;

	// Token: 0x04003BB5 RID: 15285
	public string id;

	// Token: 0x04003BB6 RID: 15286
	public string name;

	// Token: 0x04003BB7 RID: 15287
	public Quaternion2Angles.RotationOrder rotationOrder;

	// Token: 0x04003BB8 RID: 15288
	public DAZSkinV2VertexWeights[] weights;

	// Token: 0x04003BB9 RID: 15289
	public DAZSkinV2GeneralVertexWeights[] generalWeights;

	// Token: 0x04003BBA RID: 15290
	public int[] fullyWeightedVertices;

	// Token: 0x04003BBB RID: 15291
	public DAZSkinV2BulgeFactors bulgeFactors;
}
