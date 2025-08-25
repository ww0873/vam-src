using System;
using UnityEngine;

// Token: 0x02000D63 RID: 3427
public class ForceProducerV2UIConnector : UIConnector
{
	// Token: 0x06006975 RID: 26997 RVA: 0x002769EB File Offset: 0x00274DEB
	public ForceProducerV2UIConnector()
	{
	}

	// Token: 0x06006976 RID: 26998 RVA: 0x002769F3 File Offset: 0x00274DF3
	public override void Connect()
	{
		Debug.LogError("ForceProducerV2UIonnector obsolete but still in use");
	}

	// Token: 0x04005A7C RID: 23164
	public ForceProducerV2 forceProducer;
}
