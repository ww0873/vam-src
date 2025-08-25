using System;
using UnityEngine;

// Token: 0x02000BD2 RID: 3026
[ExecuteInEditMode]
public class DebugWorldPosition : MonoBehaviour
{
	// Token: 0x060055E6 RID: 21990 RVA: 0x001F69B2 File Offset: 0x001F4DB2
	public DebugWorldPosition()
	{
	}

	// Token: 0x060055E7 RID: 21991 RVA: 0x001F69BA File Offset: 0x001F4DBA
	private void Update()
	{
		this.worldPosition = base.transform.position;
	}

	// Token: 0x060055E8 RID: 21992 RVA: 0x001F69D0 File Offset: 0x001F4DD0
	private void Start()
	{
		Matrix4x4 localToWorldMatrix = base.transform.localToWorldMatrix;
		this.startingMatrixWorldPosition.x = localToWorldMatrix.m03;
		this.startingMatrixWorldPosition.y = localToWorldMatrix.m13;
		this.startingMatrixWorldPosition.z = localToWorldMatrix.m23;
	}

	// Token: 0x04004715 RID: 18197
	public Vector3 worldPosition;

	// Token: 0x04004716 RID: 18198
	public Vector3 startingMatrixWorldPosition;
}
