using System;
using UnityEngine;

// Token: 0x020002F7 RID: 759
[Serializable]
public class LightMapDataContainerObject : ScriptableObject
{
	// Token: 0x060011E4 RID: 4580 RVA: 0x00062E6D File Offset: 0x0006126D
	public LightMapDataContainerObject()
	{
	}

	// Token: 0x04000F5F RID: 3935
	public int[] lightmapIndexes;

	// Token: 0x04000F60 RID: 3936
	public Vector4[] lightmapOffsetScales;
}
