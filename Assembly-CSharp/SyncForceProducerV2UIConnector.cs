using System;
using UnityEngine;

// Token: 0x02000D91 RID: 3473
public class SyncForceProducerV2UIConnector : ForceProducerV2UIConnector
{
	// Token: 0x06006B18 RID: 27416 RVA: 0x00284E2A File Offset: 0x0028322A
	public SyncForceProducerV2UIConnector()
	{
	}

	// Token: 0x06006B19 RID: 27417 RVA: 0x00284E32 File Offset: 0x00283232
	public override void Connect()
	{
		Debug.LogError("SyncForceProducerV2UIonnector obsolete but still in use");
	}
}
