using System;
using UnityEngine;

// Token: 0x02000AE6 RID: 2790
public class DAZMeshData : ScriptableObject
{
	// Token: 0x06004A85 RID: 19077 RVA: 0x0019C617 File Offset: 0x0019AA17
	public DAZMeshData()
	{
	}

	// Token: 0x04003954 RID: 14676
	public Vector3[] baseVertices;

	// Token: 0x04003955 RID: 14677
	public MeshPoly[] basePolyList;

	// Token: 0x04003956 RID: 14678
	public DAZVertexMap[] baseVerticesToUVVertices;

	// Token: 0x04003957 RID: 14679
	public Vector3[] UVVertices;

	// Token: 0x04003958 RID: 14680
	public MeshPoly[] UVPolyList;

	// Token: 0x04003959 RID: 14681
	public Vector2[] OrigUV;
}
