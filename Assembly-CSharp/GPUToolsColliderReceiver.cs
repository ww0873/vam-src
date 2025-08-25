using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000D67 RID: 3431
public class GPUToolsColliderReceiver : MonoBehaviour
{
	// Token: 0x0600697C RID: 27004 RVA: 0x00277E3B File Offset: 0x0027623B
	public GPUToolsColliderReceiver()
	{
	}

	// Token: 0x17000F95 RID: 3989
	// (get) Token: 0x0600697D RID: 27005 RVA: 0x00277E43 File Offset: 0x00276243
	public List<GameObject> providerGameObjects
	{
		get
		{
			return this._providerGameObjects;
		}
	}

	// Token: 0x0600697E RID: 27006 RVA: 0x00277E4C File Offset: 0x0027624C
	public virtual void SyncProviders()
	{
		this._providerGameObjects = new List<GameObject>();
		foreach (GPUToolsColliderProvider gputoolsColliderProvider in this._providers)
		{
			this._providerGameObjects.Add(gputoolsColliderProvider.gameObject);
		}
	}

	// Token: 0x17000F96 RID: 3990
	// (get) Token: 0x0600697F RID: 27007 RVA: 0x00277EC0 File Offset: 0x002762C0
	// (set) Token: 0x06006980 RID: 27008 RVA: 0x00277EC8 File Offset: 0x002762C8
	public List<GPUToolsColliderProvider> providers
	{
		get
		{
			return this._providers;
		}
		set
		{
			this._providers = value;
			this.SyncProviders();
		}
	}

	// Token: 0x04005A7F RID: 23167
	protected List<GameObject> _providerGameObjects;

	// Token: 0x04005A80 RID: 23168
	protected List<GPUToolsColliderProvider> _providers;
}
