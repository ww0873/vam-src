using System;

// Token: 0x02000AFF RID: 2815
[Serializable]
public class DAZNode
{
	// Token: 0x06004C7D RID: 19581 RVA: 0x001AE4A1 File Offset: 0x001AC8A1
	public DAZNode()
	{
	}

	// Token: 0x04003B40 RID: 15168
	public string name;

	// Token: 0x04003B41 RID: 15169
	public Quaternion2Angles.RotationOrder rotationOrder;

	// Token: 0x04003B42 RID: 15170
	public DAZMeshVertexWeights[] weights;

	// Token: 0x04003B43 RID: 15171
	public int[] fullyWeightedVertices;

	// Token: 0x04003B44 RID: 15172
	public DAZBulgeFactors bulgeFactors;
}
