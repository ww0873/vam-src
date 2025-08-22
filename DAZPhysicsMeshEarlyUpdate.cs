using System;
using UnityEngine;

// Token: 0x02000B44 RID: 2884
public class DAZPhysicsMeshEarlyUpdate : MonoBehaviour
{
	// Token: 0x06004FB5 RID: 20405 RVA: 0x001C7C08 File Offset: 0x001C6008
	public DAZPhysicsMeshEarlyUpdate()
	{
	}

	// Token: 0x06004FB6 RID: 20406 RVA: 0x001C7C10 File Offset: 0x001C6010
	private void Update()
	{
		if (this.dazPhysicsMesh != null)
		{
			this.dazPhysicsMesh.EarlyUpdate();
		}
	}

	// Token: 0x04003F99 RID: 16281
	public DAZPhysicsMesh dazPhysicsMesh;
}
