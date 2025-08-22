using System;
using UnityEngine;

// Token: 0x02000D8D RID: 3469
public class ScaleChangeReceiverMeshCollider : ScaleChangeReceiver
{
	// Token: 0x06006B00 RID: 27392 RVA: 0x00284790 File Offset: 0x00282B90
	public ScaleChangeReceiverMeshCollider()
	{
	}

	// Token: 0x06006B01 RID: 27393 RVA: 0x00284798 File Offset: 0x00282B98
	private void Start()
	{
		this._colliders = base.GetComponents<MeshCollider>();
	}

	// Token: 0x06006B02 RID: 27394 RVA: 0x002847A8 File Offset: 0x00282BA8
	public override void ScaleChanged(float scale)
	{
		base.ScaleChanged(scale);
		foreach (MeshCollider meshCollider in this._colliders)
		{
			meshCollider.enabled = false;
			meshCollider.enabled = true;
		}
	}

	// Token: 0x04005CE1 RID: 23777
	private MeshCollider[] _colliders;
}
