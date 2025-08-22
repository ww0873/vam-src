using System;
using UnityEngine;

// Token: 0x02000B42 RID: 2882
[Serializable]
public class DAZPhysicsMeshCapsuleCollider
{
	// Token: 0x06004FAD RID: 20397 RVA: 0x001C7B75 File Offset: 0x001C5F75
	public DAZPhysicsMeshCapsuleCollider()
	{
	}

	// Token: 0x04003F8E RID: 16270
	public int endVertex1;

	// Token: 0x04003F8F RID: 16271
	public int endVertex2;

	// Token: 0x04003F90 RID: 16272
	public int frontVertex;

	// Token: 0x04003F91 RID: 16273
	public float oversizeLength;

	// Token: 0x04003F92 RID: 16274
	public float oversizeRadius;

	// Token: 0x04003F93 RID: 16275
	public CapsuleCollider collider;
}
